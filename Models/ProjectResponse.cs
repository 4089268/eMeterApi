using System;

namespace eMeterApi.Models;

public class ProjectResponse
{
    public long Id { get; set; }

    public string Proyecto { get; set; } = "";

    public string Clave { get; set; } = "";

    public int OficinaId { get; set; }

    public string OficinaDesc { get; set; } = string.Empty;

    public IEnumerable<Object> Devices {get; set;} = [];
    public int TotalDevices {get; set;}
}
