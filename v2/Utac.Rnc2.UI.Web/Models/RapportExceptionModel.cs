using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class RapportExceptionModel
    {
        public pCentreEncour Centre { get; set; }

        public rCategorieVehicule.CategorieEnum CategVehicule { get; set; }

        public IList<pRapportException> ListeRapportPL { get; set; }

        public IList<pRapportExceptionVl> ListeRapportVL { get; set; }

        //PLE le 05/04/2024 Catégorie L
        public IList<pRapportExceptionCl> ListeRapportCL { get; set; }
    }
}
