using System;
using System.Collections.Generic;

namespace TareaCorta4API.DataAccess.Models;

public partial class Telefono
{
    public int Idcontacto { get; set; }

    public string Telefonos { get; set; } = null!;

    public virtual Contacto IdcontactoNavigation { get; set; } = null!;
}
