using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AP_Groupe3_Hotel.Models;

public partial class TbClient
{
    public int PkCli { get; set; }

    public string NomCli { get; set; } = null!;

    public string PreCli { get; set; } = null!;

    public string RueCli { get; set; } = null!;

    public string TelCli { get; set; } = null!;

    public string MailCli { get; set; } = null!;

    public DateOnly? DatNaisCli { get; set; }

    public int FkCliLoc { get; set; }

    public virtual TbLocalite FkCliLocNavigation { get; set; } = null!;

    public virtual ICollection<TbReservation> TbReservations { get; set; } = new List<TbReservation>();


    /// <summary>
    ///   Créer par Maelle
    /// </summary>
    /*Le code sélectionné est une propriété calculée dans la classe TbClient du fichier TbClient.cs.
      L'attribut [NotMapped] indique que cette propriété ne doit pas être mappée à une colonne dans la base de données. Cela signifie la propriété NomPrenom ne sera pas prise en compte.
      La propriété NomPrenom retourne une chaîne de caractères représentant le nom et le prénom du client.*/

    [NotMapped]
    public string NomPrenom
    {
        get { return $"{PreCli} {NomCli} "; }
    }
}