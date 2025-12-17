using System;
using System.Collections.Generic;

namespace eMeterApi.Entities;

public partial class CatOficina
{
    public int Id { get; set; }

    public string? Oficina { get; set; }

    public bool? Inactivo { get; set; }

    public virtual ICollection<SysProyecto> SysProyectos { get; } = new List<SysProyecto>();
}
