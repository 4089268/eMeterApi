﻿using System;
using System.Collections.Generic;
using eMeterApi.Data.Contracts.Models;

namespace eMeterApi.Entities;

public partial class Usuario : IUser
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


    public string Email { get => this.Usuario1??""; set => this.Usuario1 = value; }
    public string Name { get => this.Operador??""; set => this.Operador = value; }
    public string? Company { get => this.Empresa; set =>  this.Empresa = value ; }
}
