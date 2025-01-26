using System;
using System.Collections.Generic;

namespace AP_Groupe3_Hotel.Models;

public partial class TbChambre
{
    public int PkCha { get; set; }

    public int CodeCha { get; set; }

    public string CapCha { get; set; } = null!;

    public decimal PrixCha { get; set; }

    public int PfkChaEta { get; set; }

    public virtual TbEtage PfkChaEtaNavigation { get; set; } = null!;

    public virtual ICollection<TbReservation> TbReservations { get; set; } = new List<TbReservation>();

    /// <summary>
    /// Retourne le code de la chambre et le code de l'étage
    /// Qui forme le numéro de la chambre
    /// </summary>
    public string ChaEta
    {
        get { return $"{PfkChaEtaNavigation.CodeEta} - {CodeCha}"; }
    }

}