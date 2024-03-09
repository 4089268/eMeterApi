using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eMeterApi.Entities;

namespace eMeterApi.Data
{
    public interface IProjectService
    {
        public IEnumerable<SysProyecto>? GetAll();

        public SysProyecto Create(SysProyecto sysProyecto);

        public bool Remove(long projectID);

        public bool Update(long projectID, SysProyecto sysProyecto);
        
    }
}