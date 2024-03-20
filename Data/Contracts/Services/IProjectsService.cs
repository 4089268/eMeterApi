using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using eMeterApi.Data.Contracts.Models;
using eMeterAPi.Data.Contracts.Models;

namespace eMeterApi.Data.Contracts
{
    public interface IProjectsService
    {
        public IEnumerable<IProject>? GetProjects( long? userId, string? groupId );
        public long? CreateProject( IProject project, out string? message );
        public bool UpdateProject( long projectId, IProject project, out string? message );
        public bool DeletedProject( long projectId, out string? message);
        public bool UpdateProjectsUser( long userId, IEnumerable<long> projectsId, out string? message );
    }
}