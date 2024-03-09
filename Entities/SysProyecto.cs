using System;
using System.Collections.Generic;

namespace eMeterApi.Entities;

public partial class SysProyecto
{
    public long Id { get; set; }

    public string? Proyecto { get; set; }

    public string? Clave { get; set; }

    public virtual ICollection<SysProyectoUsuario> SysProyectoUsuarios { get; } = new List<SysProyectoUsuario>();
}
