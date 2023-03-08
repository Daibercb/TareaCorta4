using System;
using System.Collections.Generic;

namespace TareaCorta4API.DataAccess.Models;

public partial class Contacto
{
    public int Identificacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellidos { get; set; } = null!;

    public string? Telefonos { get; set; }

    public string? Correos { get; set; }

    public string? Facebook { get; set; }

    public string? Instagram { get; set; }

    public string? Twitter { get; set; }

    public virtual CorreoElectronico? CorreoElectronico { get; set; }

    public virtual Telefono? Telefono { get; set; }
}
