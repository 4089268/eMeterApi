using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeter.Models;
using eMeterApi.Data.Contracts.Models;

namespace eMeterApi.Data.Contracts
{
    public interface IProjectService
    {
        public IEnumerable<Project>? GetProjects( long? userId, string? groupId );
        public long? CreateProject( Project project, out string? message );
        public bool UpdateProject( long projectId, Project project, out string? message );
        public bool DeletedProject( long projectId, out string? message);
        public bool UpdateProjectsUser( long userId, IEnumerable<long> projectsId, out string? message );
    }
}