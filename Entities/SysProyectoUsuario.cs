using System;
using System.Collections.Generic;

namespace eMeterApi.Entities;

public partial class SysProyectoUsuario
{
    public long Id { get; set; }

    public long IdUsuario { get; set; }

    public long IdProyecto { get; set; }

    public virtual SysProyecto IdProyectoNavigation { get; set; } = null!;

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
