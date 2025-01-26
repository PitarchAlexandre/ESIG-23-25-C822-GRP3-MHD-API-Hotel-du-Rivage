using System;
using System.Collections.Generic;

namespace AP_Groupe3_Hotel.Models;

public partial class TbLocalite
{
    public int PkLoc { get; set; }

    public string NpaLoc { get; set; } = null!;

    public string NomLoc { get; set; } = null!;

    public virtual ICollection<TbClient> TbClients { get; set; } = new List<TbClient>();

    /// <summary>
    /// Créer par Alexandre et complètement copié sur Maëlle
    /// 
    /// Permet d'afficher le NPA et le nom de la localité
    /// </summary>
    public string NpaNomLoc
    {
        get { return $"{NpaLoc} {NomLoc}"; }
    }
}
