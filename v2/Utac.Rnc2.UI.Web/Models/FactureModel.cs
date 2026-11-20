using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class FactureModel
    {
        public rUtilisateur UtilisateurCentre { get; set; }
        public pCentreEncour MonCentre { get; set; }
        public rReseau Reseau { get; set; }
        public rCategorieVehicule.CategorieEnum Categorie { get; set; }

    }
}