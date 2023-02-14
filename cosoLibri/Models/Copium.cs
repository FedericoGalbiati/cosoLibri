using System;
using System.Collections.Generic;

namespace cosoLibri.Models;

public partial class Copium
{
    public uint NumeroInventario { get; set; }

    public string Collocazione { get; set; } = null!;

    public uint FkLibro { get; set; }

    public virtual Libro FkLibroNavigation { get; set; } = null!;

    public virtual ICollection<Prestito> Prestitos { get; } = new List<Prestito>();
}
