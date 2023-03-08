using System;
using System.Collections.Generic;

namespace TareaCorta4API.DataAccess.Models;

public partial class Usuario
{
    public int Identificacion { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int Estado { get; set; }
}
