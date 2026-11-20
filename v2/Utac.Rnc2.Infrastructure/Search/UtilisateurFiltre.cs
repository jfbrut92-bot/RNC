using System.Collections.Generic;

namespace Utac.Rnc2.Infrastructure.Search
{
    public class UtilisateurFiltre
    {
        public string CodeDepartement { get; set; }
        public int? RegionId { get; set; }
        public string CodeReseau { get; set; }
        public int ProfilId { get; set; }

        //Pour les requetes faites par les utilisateurs de type Reseau-Administrateur
        public IList<string> CodeReseaux { get; set; }
        public string CodeReseauVL { get; set; }
        public string CodeReseauPL { get; set; }
        public string CodeReseauCL { get; set; }

        public string CategorieVehicule { get; set; } // : VL, PL ou CL
        public string Nom { get; set; }
        public bool LoginProvisoire { get; set; }
        //EG le 02/12/2011
        public string Identifiant { get; set; }

    }
}
