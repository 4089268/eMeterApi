using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeter.Models;
using eMeterApi.Data;
using eMeterApi.Data.Contracts;
using eMeterApi.Data.Contracts.Models;
using eMeterApi.Data.Exceptions;
using eMeterApi.Entities;
using eMeterApi.Helpers;
using eMeterApi.Models;
using Microsoft.Extensions.Options;
using eMeterApi.Models.ViewModels.Users;
using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Collections.ObjectModel;

namespace eMeterApi.Service
{
    public class UserService : IUserService
    {

        private readonly EMeterContext dbContext;
        private readonly ILogger<UserService> logger;
        private readonly JwtSettings jwtSettings;
        private readonly AppSettings appKeySettings;

        public UserService(EMeterContext eMeterContext, ILogger<UserService> logger, IOptions<JwtSettings> optionsJwt, IOptions<AppSettings> optionsAppKey )
        {
            this.dbContext = eMeterContext;
            this.logger = logger;
            this.jwtSettings = optionsJwt.Value;
            this.appKeySettings = optionsAppKey.Value;
        }

        public string? Authenticate(IUserCredentials userCredentials, out string? message)
        {
            message = null;
            
            // Validate user
            var hashedPassword = DataHasher.HashDataWithKey( userCredentials.Password, this.appKeySettings.AppKey );
            
            Entities.Usuario? user = null;
            try{
                user = dbContext.Usuarios
                    .Where( user =>
                        user.Usuario1 == userCredentials.Email &&
                        user.Password == hashedPassword && 
                        user.DeletedAt == null
                    )
                    .FirstOrDefault();
            }
            catch(Exception err){
                logger.LogError(err, "Error at validate user in the database");
                message = "Error at validate user in the database";
                return null;
            }

            if(user == null){
                message = "Usuario y/o contraseña incorrectos";
                return null;
            }

            // TODO: Get roles of the user ( admin, super user, user) for custom claims 
            
            // Generate token
            try{
                var tokenGenerator = new JwtGenerator( jwtSettings );
                return tokenGenerator.Generate( user.Id.ToString(), user.Usuario1, user.Operador!, new Dictionary<string,string>(){
                    {"company", user.Empresa??""}
                });
            }catch(Exception err){
                logger.LogError(err, "Error at generate token at UserService.Authenticate");
                message = "Error at generate user token";
                return null;
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="param"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="SimpleValidationException"></exception>
        public long? CreateUser(UserRequest user, IDictionary<string, object>? param, out string? message)
        {
            message = null;

            // Validate if user is already store
            var _user = dbContext.Usuarios.Where( usuario => usuario.Usuario1 == user.Email && usuario.DeletedAt == null ).FirstOrDefault();
            if( _user != null ){
                throw new SimpleValidationException("Validations fail", new Dictionary<string,string>(){
                    { "email", "El correo electrónico ya se encuentra registrado en la base de datos"}
                });
            }

            var hashedPassword = DataHasher.HashDataWithKey( user.Password!, appKeySettings.AppKey );

            // Create user entity
            var newUser = new Entities.Usuario(){
                Usuario1 = user.Email!,
                Operador = user.Name,
                Password =  hashedPassword,
                Company = user.Company
            };

            // Store the new entity
            long userId = 0;
            try{
                dbContext.Usuarios.Add( newUser );
                dbContext.SaveChanges();
                userId =  newUser.Id;

            }catch(Exception err){
                logger.LogError(err, "Error at stored new user at UserService.CreateUser");
                message = "Fail to create new user; " + err.Message;
                return null;
            }

            // Attach the projects ids
            try
            {
                var projects = dbContext.SysProyectos.Where( item => user.ProjectsId.Contains( item.Id ) ).ToImmutableArray();
                foreach( var project in projects ){
                    dbContext.SysProyectoUsuarios.Add( new SysProyectoUsuario {
                        IdUsuario = userId,
                        IdProyecto = project.Id
                    });
                }
                dbContext.SaveChanges();
            }
            catch (System.Exception ex)
            {
                this.logger.LogError("Can't attach the proyects to the user at UserService. CreateUser; {message}", ex.Message);
            }

            return  userId;

        }

        public bool DisableUser(long userId, out string? message)
        {
            message = "";

            // Find user
            var _user = dbContext.Usuarios
                .Where( usuario => usuario.Id == userId && usuario.DeletedAt == null )
                .FirstOrDefault();

            if(_user == null)
            {
                if( _user == null ){
                    throw new SimpleValidationException("Validations fail", new Dictionary<string,string>(){
                        { "userId", "The user is not found in the database "}
                    });
                }
            }
            
            // Update entity
            try{
                _user.DeletedAt =  DateTime.Now;
                dbContext.Usuarios.Update(_user);
                dbContext.SaveChanges();
                return true;
            }catch(Exception err){
                message = "Error at disabled user; " + err.Message;
                logger.LogError( err, "Error at disabled user");
                return false;
            }
        }

        public IEnumerable<User>? GetUsers()
        {
            try
            {
                var usersRaw = this.dbContext.Usuarios
                    .Where(u => u.DeletedAt == null)
                    .Include(u => u.SysProyectoUsuarios!)
                    .ThenInclude(pu => pu.IdProyectoNavigation)
                    .ToList();

                var users = new List<User>();
                foreach(var _user in usersRaw){
                    var user = new User{
                        Id  = _user.Id,
                        Email = _user.Usuario1,
                        Name = _user.Operador??"",
                        Company = _user.Empresa??"",
                    };

                    if( _user.SysProyectoUsuarios != null){
                        user.Projects = _user.SysProyectoUsuarios.Select( item => new Project {
                            Id = item.IdProyecto,
                            Proyecto = item.IdProyectoNavigation.Proyecto??""
                        });
                    }

                    users.Add( user);
                }
                return users;

            }
            catch(Exception err)
            {
                logger.LogError(err, "Error at get the users");
                return null;
            }
        }

        public bool UpdateUser(long userId, UserUpdateRequest userUpdateRequest, out string? message)
        {

            message = null;
            
            // Find user
            var _user = dbContext.Usuarios
                .Where(item => item.Id == userId && item.DeletedAt == null)
                .FirstOrDefault();

            if( _user == null)
            {
                throw new SimpleValidationException( "Validations fail", new Dictionary<string,string>(){
                    { "userId", "The user is not found in the database "}
                });
            }

            try
            {
                _user.Name = userUpdateRequest.Name != null ?userUpdateRequest.Name :_user.Name;
                _user.Email = userUpdateRequest.Email != null ? userUpdateRequest.Email :_user.Email;
                _user.Company = userUpdateRequest.Company != null ? userUpdateRequest.Company! : _user.Company;
                if( userUpdateRequest.Password != null){
                    var hashedPassword = DataHasher.HashDataWithKey(userUpdateRequest.Password, appKeySettings.AppKey);
                    _user.Password = hashedPassword;
                }

                // Update database
                dbContext.Update( _user );
                dbContext.SaveChanges();
                return true;

            }
            catch(Exception err)
            {
                message = "Error at upate user; " + err.Message;
                logger.LogError(err, "Error at update user" );
                return false;
            }
        }
    }
}