using System;
using System.Collections.Generic;
using eMeterApi.Data.Contracts.Models;

namespace eMeterApi.Entities;

public partial class SysProyecto : IProject
{
    public long Id { get; set; }

    public string? Proyecto { get; set; }

    public string? Clave { get; set; }

    public DateTime? DeletedAt { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public int OficinaId { get; set; }

    public virtual ICollection<SysProyectoUsuario> SysProyectoUsuarios { get; } = new List<SysProyectoUsuario>();
}
