using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class RechercheCentreModel
    {
        public string RaisonSociale { get; set; }
        public string NumSiren { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string NumAgrementVl { get; set; }
        public string NumAgrementPl { get; set; }
        public string NumAgrementCl { get; set; }
        //EG le 28/02/2012
        public string AncienNumAgrementVl { get; set; }
        public string AncienNumAgrementPl { get; set; }
        public string AncienNumAgrementCl { get; set; }

        /* VL */
        public bool IsAgrementVlValide { get; set; }
        public bool IsAgrementVlRejete { get; set; }
        public bool IsAgrementVlAnnule { get; set; }
        public bool IsAgrementVlDemandeEnCours { get; set; }

        /* PL */
        public bool IsAgrementPlValide { get; set; }
        public bool IsAgrementPlRejete { get; set; }
        public bool IsAgrementPlAnnule { get; set; }
        public bool IsAgrementPlDemandeEnCours { get; set; }

        /* CL */
        public bool IsAgrementClValide { get; set; }
        public bool IsAgrementClRejete { get; set; }
        public bool IsAgrementClAnnule { get; set; }
        public bool IsAgrementClDemandeEnCours { get; set; }

        //PLE le 23/01/2024 Extension catégorie L
        public bool AvecExtentionAgrement { get; set; }
        /*  */

        public IList<pCentreEncour> Resultat { get; set; }
    }
}
