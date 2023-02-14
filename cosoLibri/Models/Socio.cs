using System;
using System.Collections.Generic;

namespace cosoLibri.Models;

public partial class Socio
{
    public uint Codice { get; set; }

    public string Nome { get; set; } = null!;

    public string Cognome { get; set; } = null!;

    public string? Via { get; set; }

    public string? Civico { get; set; }

    public string? Città { get; set; }

    public string? Cap { get; set; }

    public string? Provincia { get; set; }

    public string? Telefono { get; set; }

    public string? EMail { get; set; }

    public virtual ICollection<Prestito> Prestitos { get; } = new List<Prestito>();
}
