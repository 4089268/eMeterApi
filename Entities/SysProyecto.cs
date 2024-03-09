using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace eMeterApi.Entities;

public partial class SysProyecto
{
    [JsonIgnore]
    public long Id { get; set; }

    [Required]
    public string? Proyecto { get; set; }

    [Required]
    public string? Clave { get; set; }

    [JsonIgnore]
    public DateTime? DeletedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<SysProyectoUsuario> SysProyectoUsuarios { get; } = new List<SysProyectoUsuario>();
}
