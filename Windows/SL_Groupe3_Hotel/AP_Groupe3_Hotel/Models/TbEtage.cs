using System;
using System.Collections.Generic;

namespace AP_Groupe3_Hotel.Models;

public partial class TbEtage
{
    public int PkEta { get; set; }

    public int CodeEta { get; set; }

    public virtual ICollection<TbChambre> TbChambres { get; set; } = new List<TbChambre>();
}
