using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AjouterDerogationModel
    {
        public pCentreEncour Centre { get; set; }

        public pDerogationCentre Derogation { get; set; }

        public rCategorieVehicule.CategorieEnum CategorieVehicule { get; set; }

    }
}