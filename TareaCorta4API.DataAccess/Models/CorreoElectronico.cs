using System;
using System.Collections.Generic;

namespace TareaCorta4API.DataAccess.Models;

public partial class CorreoElectronico
{
    public int Idcontacto { get; set; }

    public string Correoelectronico1 { get; set; } = null!;

    public virtual Contacto IdcontactoNavigation { get; set; } = null!;
}
