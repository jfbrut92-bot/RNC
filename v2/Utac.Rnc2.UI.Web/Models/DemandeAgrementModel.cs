using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class DemandeAgrementModel
    {
        public rCategorieVehicule.CategorieEnum CategorieVehicule { get; set; }

        public pCentreEncour Centre { get; set; }

        public IList<rStatutJuridique> ListeStatut { get; set; }

        public IList<rReseau> ListeReseau { get; set; }

        //EG le 12/12/2011 pour gérer l'action "Nouvel agrément"
        public bool DateValide { get; set; }

        //EG le 04/01/2012 pour gérer le cas d’agrément de centres mixtes avec un nom de réseau différents
        public bool ExisteUnAgrementValideReseau { get; set; }
    }
}