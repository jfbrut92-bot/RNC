using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.UI.Web.Models
{
    public class UtilisateurListeModel
    {
        public UtilisateurFiltre filtre { get; set; }
        public IList<rUtilisateur> liste { get; set; }

        public rProfil profilFiltre { get; set; }

        //les valeurs de référence
        public IList<rRegion> listeRegion { get; set; }
        public IList<string> listeCategorieVehicule
        {
            get
            {
                List<string> liste = new List<string>();
                liste.Add("");
                liste.Add("VL");
                liste.Add("PL");
                liste.Add("CL");
                return liste;
            }
        }

        public IList<rReseau> listeReseau { get; set; }
        public IList<rProfil> listeProfil { get; set; }

    }
}