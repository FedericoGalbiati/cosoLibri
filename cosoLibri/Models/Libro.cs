using System;
using System.Collections.Generic;

namespace cosoLibri.Models;

public partial class Libro
{
    public uint Codice { get; set; }

    public string? Isbn { get; set; }

    public string Titolo { get; set; } = null!;

    public DateTime AnnoPub { get; set; }

    public string Lingua { get; set; } = null!;

    public uint FkEditore { get; set; }

    public virtual ICollection<Copium> Copia { get; } = new List<Copium>();

    public virtual Editore FkEditoreNavigation { get; set; } = null!;

    public virtual ICollection<Autore> FkAutores { get; } = new List<Autore>();

    public virtual ICollection<Parolachiave> FkParolaChiaves { get; } = new List<Parolachiave>();
}
