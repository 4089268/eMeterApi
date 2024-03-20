using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using eMeterApi.Data;
using eMeterApi.Data.Contracts;
using eMeterApi.Data.Exceptions;
using eMeterApi.Entities;
using eMeterAPi.Data.Contracts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace eMeterAPi.Service
{
    public class ProjectsService : IProjectsService
    {

        private readonly EMeterContext dbContext;
        private readonly ILogger<ProjectsService> logger;

        public ProjectsService( EMeterContext dbContext, ILogger<ProjectsService> logger )
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }


        public IEnumerable<IProject>? GetProjects(long? userId, string? groupId)
        {
            var query = dbContext.SysProyectos.Where( p => p.DeletedAt ==null).AsQueryable();

            if( groupId != null){
                query = query.Where( p => p.Clave == groupId );
            }

            if( userId != null){
                query = query.Where( x => x.SysProyectoUsuarios.Any( u => u.IdUsuario == userId!));
            }

            return query.ToList();
        }


        public long? CreateProject(IProject project, out string? message)
        {
            message = null;

            // Validate clave is not duplicated
            var alreadyStore = dbContext.SysProyectos.Where( p => p.Clave == project.Clave).Count() > 0;
            if( alreadyStore ){
                var errorsMessages = new Dictionary<string,string>{
                    { "clave", "The value is already stored in the database"}
                };
                throw new SimpleValidationException("Validations fail", errorsMessages);
            }

            // Create new entity
            var newProject = new SysProyecto(){
                Proyecto = project.Proyecto,
                Clave = project.Clave
            };

            // Save changes in db
            try{
                dbContext.SysProyectos.Add( newProject );
                dbContext.SaveChanges();
                return newProject.Id;
            }catch( Exception err){
                logger.LogError( err, "Cant store the new project");
                message = err.Message;
                return null;
            }
        }

        public bool DeletedProject(long projectId, out string? message)
        {
            message = null;

            // Validate 
            var storedProject = dbContext.SysProyectos.Where( p => p.Id == projectId && p.DeletedAt == null).FirstOrDefault();
            if( storedProject == null){
                var errorsMessages = new Dictionary<string,string>{{"projectId", "The project was not found int the database"}};
                throw new SimpleValidationException("The project is not found", errorsMessages );
            }

            storedProject.DeletedAt = DateTime.Now;

            try{
                dbContext.SysProyectos.Update( storedProject );
                dbContext.SaveChanges();
                return true;
            }catch(Exception err){
                logger.LogError( err, "Cant delete the project");
                message = err.Message;
                return false;
            }
        }

        
        public bool UpdateProject(long projectId, IProject project, out string? message)
        {
            message = null;

            var errorsMessages = new Dictionary<string,string>();

            // Validate project id exist
            var storedProject = dbContext.SysProyectos.Where( p => p.Id == projectId && p.DeletedAt == null).FirstOrDefault();
            if( storedProject == null){
                errorsMessages.Add("projectId", "The project was not found int the database");
            }

            // Validate clave is not already stored
            var claveStored = dbContext.SysProyectos.Where( p => p.DeletedAt == null && p.Id != projectId && p.Clave == project.Clave).Count() > 0;
            if( claveStored){
                errorsMessages.Add("clave", "The value is already stored in the database");
            }

            if( !errorsMessages.IsNullOrEmpty()){
                throw new SimpleValidationException("The validations fail", errorsMessages );
            }
            
            // Set new values
            storedProject!.Proyecto = project.Proyecto;
            storedProject.Clave = project.Clave;

            // Save changes
            try{
                dbContext.SysProyectos.Update( storedProject );
                dbContext.SaveChanges();
                return true;
            }catch(Exception err){
                logger.LogError( err, "Cant update the project");
                message = err.Message;
                return false;
            }
        }

        public bool UpdateProjectsUser(long userId, IEnumerable<long> projectsId, out string? message)
        {
            message = null;

            // Validate user
            var user = dbContext.Usuarios.Where( u => u.DeletedAt == null && u.Id == userId).Include( u=> u.SysProyectoUsuarios).FirstOrDefault();
            if( user == null){
                throw new SimpleValidationException( "The validations fail", new Dictionary<string,string>{{"userId","The user was not found in the database"}});
            }

            // Remove and update the ralations user-project
            user.SysProyectoUsuarios.Clear();
            foreach( var projectId in projectsId){
                var tmpProject = dbContext.SysProyectos.Find( projectId);
                if( tmpProject != null){
                    user.SysProyectoUsuarios.Add( new SysProyectoUsuario{
                        IdUsuario = user.Id,
                        IdProyecto = tmpProject.Id
                    });
                }
            }

            // Save changes
            try{
                dbContext.Usuarios.Update(user);
                dbContext.SaveChanges();
                return true;
            }catch(Exception err){
                message = err.Message;
                logger.LogError(err, "Cant update relations user-proyects");
                return false;
            }
        }   
    }
}       