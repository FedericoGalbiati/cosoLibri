using System;
using System.Collections.Generic;

namespace cosoLibri.Models;

public partial class Parolachiave
{
    public uint IdParola { get; set; }

    public string? ParolaChiave1 { get; set; }

    public virtual ICollection<Libro> Fklibros { get; } = new List<Libro>();
}
