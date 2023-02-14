using System;
using System.Collections.Generic;

namespace cosoLibri.Models;

public partial class Prestito
{
    public uint FkSocio { get; set; }

    public uint FkCopia { get; set; }

    public DateTime DataPrestito { get; set; }

    public DateTime? DataRestituzione { get; set; }

    public virtual Copium FkCopiaNavigation { get; set; } = null!;

    public virtual Socio FkSocioNavigation { get; set; } = null!;
}
