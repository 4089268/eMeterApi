using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eMeterApi.Models
{
    public class EnumerableResponse<T>
    {
        public IEnumerable<T> Data {get;set;} = null!;
        public int ChunkSize {get; set;}
        public int Page {get; set;}
        public int TotalItems {get; set;}
    }
}