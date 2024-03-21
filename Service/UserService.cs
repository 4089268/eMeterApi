using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Data;
using eMeterApi.Data.Contracts;
using eMeterApi.Data.Contracts.Models;
using eMeterApi.Data.Exceptions;
using eMeterApi.Entities;
using eMeterApi.Helpers;
using eMeterApi.Models;
using Microsoft.Extensions.Options;

namespace eMeterApi.Service
{
    public class UserService : IUserService
    {

        private readonly EMeterContext dbContext;
        private readonly ILogger<UserService> logger;
        private readonly JwtSettings jwtSettings;

        public UserService(EMeterContext eMeterContext, ILogger<UserService> logger, IOptions<JwtSettings> optionsJwt )
        {
            this.dbContext = eMeterContext;
            this.logger = logger;
            this.jwtSettings = optionsJwt.Value;
        }

        public string? Authenticate(IUserCredentials userCredentials, out string? message)
        {
            message = null;
            
            // Validate user
            Entities.Usuario? user = null;
            try{
                user = dbContext.Usuarios
                    .Where( user =>
                        user.Usuario1 == userCredentials.Email &&
                        user.Password == userCredentials.Password && 
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
        public long? CreateUser(IUser user, IDictionary<string, object>? param, out string? message)
        {
            message = null;

            // Validate if user is already store
            var _user = dbContext.Usuarios.Where( usuario => usuario.Usuario1 == user.Email && usuario.DeletedAt == null ).FirstOrDefault();
            if( _user != null ){
                throw new SimpleValidationException("Validations fail", new Dictionary<string,string>(){
                    { "email", "The email is already stored in the database "}
                });
            }

            // Create user entity
            var newUser = new Entities.Usuario(){
                Usuario1 = user.Email,
                Operador = user.Name,
                Password = user.Password, //TODO: Hash password
                Company = user.Company
            };

            // Store the new entity
            try{
                dbContext.Usuarios.Add( newUser );
                dbContext.SaveChanges();
                return newUser.Id;

            }catch(Exception err){
                logger.LogError(err, "Error at stored new user at UserService.CreateUser");
                message = "Fail to create new user; " + err.Message;
                return null;
            }
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

        public IEnumerable<IUser>? GetUsers()
        {
            try{
                return dbContext.Usuarios.ToList<IUser>();
            }catch(Exception err){
                logger.LogError( err, "Error at get the users");
                return null;
            }
        }

        public bool UpdateUser(long userId, UserUpdateRequest userUpdateRequest, out string? message)
        {
            message = null;
            
            // Find user
            var _user = dbContext.Usuarios.Where( item => item.Id == userId && item.DeletedAt == null).FirstOrDefault();
            if( _user == null){
                throw new SimpleValidationException( "Validations fail", new Dictionary<string,string>(){
                    { "userId", "The user is not found in the database "}
                });
            }

            try{
                _user.Name = userUpdateRequest.Name != null ?userUpdateRequest.Name :_user.Name;
                _user.Email = userUpdateRequest.Email != null ? userUpdateRequest.Email :_user.Email;
                _user.Company = userUpdateRequest.Company != null ? userUpdateRequest.Company! : _user.Company;
                if( userUpdateRequest.Password != null){
                    var hashedPassword = userUpdateRequest.Password; // TODO: Hash password
                    _user.Password = hashedPassword;
                }

                // Update database
                dbContext.Update( _user );
                dbContext.SaveChanges();
                return true;

            }catch(Exception err){
                message = "Error at upate user; " + err.Message;
                logger.LogError(err, "Error at update user" );
                return false;
            }
        }
    }
}