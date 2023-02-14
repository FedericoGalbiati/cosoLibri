using System;
using System.Collections.Generic;

namespace cosoLibri.Models;

public partial class Autore
{
    public uint CodiceAutore { get; set; }

    public string Nome { get; set; } = null!;

    /// <summary>
    /// Il cognome potrebbe essere nullo per quegli autori che si firmano con uno pseudonimo
    /// </summary>
    public string? Cognome { get; set; }

    public string Nazione { get; set; } = null!;

    public virtual ICollection<Libro> FkLibros { get; } = new List<Libro>();
}
