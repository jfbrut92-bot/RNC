using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class ActionAgrementControleurModel
    {
        public pContoleurEncour controleur { get; set; }
        public rCategorieVehicule.CategorieEnum categorieVehicule { get; set; }
        public int NombreChargement { get; set; }
    }
}