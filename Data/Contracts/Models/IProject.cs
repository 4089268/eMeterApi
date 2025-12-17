using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eMeterApi.Data.Contracts.Models
{
    public interface IProject
    {
        public long Id { get; set; }
        public string Proyecto { get; set; }
        public string Clave { get; set; }
        public int OficinaId { get; set; }
    }
}