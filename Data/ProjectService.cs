using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Data.Exceptions;
using eMeterApi.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace eMeterApi.Data
{
    public class ProjectService : IProjectService
    {

        private readonly EMeterContext eMeterContext;
        private readonly ILogger<ProjectService> logger;

        public ProjectService( EMeterContext dbContext, ILogger<ProjectService> logger)
        {
            this.eMeterContext = dbContext;
            this.logger = logger;
        }

        public IEnumerable<SysProyecto> GetAll()
        {
            return this.eMeterContext.SysProyectos.Where(i => i.DeletedAt == null).OrderBy( m => m.Proyecto ).ToArray();
        }

        /// <summary>
        /// Store new project
        /// </summary>
        /// <param name="sysProyecto"></param>
        /// <returns>A entity stored with the id set it</returns>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException"> Error at save changes on the database </exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException"> The data in the database has been modified since it was loaded into memory. </exception>
        public SysProyecto Create(SysProyecto sysProyecto)
        {
            this.eMeterContext.SysProyectos.Add( sysProyecto );
            this.eMeterContext.SaveChanges();
            return sysProyecto;
        }

        /// <summary>
        /// Remove a project by the id 
        /// </summary>
        /// <param name="projectID"></param>
        /// <returns></returns>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException"> Error at save changes on the database </exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException"> The data in the database has been modified since it was loaded into memory. </exception>
        /// /// <exception cref="EntityNotFoundException"> Entity not found. </exception>
        public bool Remove(long projectID)
        {
            var proyecto = this.eMeterContext.SysProyectos.Where( e => e.DeletedAt == null && e.Id == projectID).FirstOrDefault();
            if( proyecto == null){
                throw new EntityNotFoundException( $" Proyect id {projectID} not found" );
            }
            proyecto.DeletedAt = DateTime.Now;
            this.eMeterContext.Update( proyecto );
            this.eMeterContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectID"></param>
        /// <param name="sysProyecto"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotFoundException"> Entity with the specific id not found </exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException"> Error at save changes on the database </exception>
        /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException"> The data in the database has been modified since it was loaded into memory. </exception>
        public bool Update(long projectID, SysProyecto sysProyecto)
        {
            var _project = this.eMeterContext.SysProyectos.Where( e => e.DeletedAt == null && e.Id == projectID).FirstOrDefault();
            if(_project == null){
                throw new EntityNotFoundException( $" Proyect id {projectID} not found" );
            }
            
            _project.Proyecto = sysProyecto.Proyecto;
            _project.Clave = sysProyecto.Clave;
            this.eMeterContext.SysProyectos.Update(_project);
            this.eMeterContext.SaveChanges();
            return true;
        }

    }
}