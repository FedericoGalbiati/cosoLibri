using System;
using System.Collections.Generic;

namespace cosoLibri.Models;

public partial class Editore
{
    public uint CodiceEditore { get; set; }

    public string? Nome { get; set; }

    public string? Via { get; set; }

    public string? Civico { get; set; }

    public string? Città { get; set; }

    public string? Cap { get; set; }

    public string? Provincia { get; set; }

    public string? Agente { get; set; }

    public string? Telefono { get; set; }

    public string? SitoWeb { get; set; }

    public virtual ICollection<Libro> Libros { get; } = new List<Libro>();
}
