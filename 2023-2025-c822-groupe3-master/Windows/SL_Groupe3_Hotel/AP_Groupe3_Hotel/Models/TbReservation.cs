using System;
using System.Collections.Generic;

namespace AP_Groupe3_Hotel.Models;

public partial class TbReservation
{
    public int PkRes { get; set; }

    public string NbrPerRes { get; set; } = null!;

    public sbyte DejRes { get; set; }

    public DateTime DatJouRes { get; set; }

    // le "?" autorise le null, ce qui permet de ne pas avoir la date du 01.01.0001 par défaut
    public DateOnly? DatArrRes { get; set; }

    public DateOnly? DatDepRes { get; set; }

    public int FkResCli { get; set; }

    public int FkResCha { get; set; }

    public int FkResChaEta { get; set; }

    public virtual TbClient FkResCliNavigation { get; set; } = null!;

    public virtual TbChambre TbChambre { get; set; } = null!;
}
