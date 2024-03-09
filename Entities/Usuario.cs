using System;
using System.Collections.Generic;

namespace eMeterApi.Entities;

public partial class Usuario
{
    public long Id { get; set; }

    public string Usuario1 { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Operador { get; set; }

    public string? Empresa { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<SysProyectoUsuario> SysProyectoUsuarios { get; } = new List<SysProyectoUsuario>();
}
