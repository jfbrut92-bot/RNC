using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class RechercheControleurModel
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        //PLE le 01/03/2024        
        public System.DateTime? DateNaissance { get; set; }
        //EG le 21/10/2011
        public string EstPrestataire { get; set; }
        /* VL */
        public string CodeAgrementVL { get; set; }
        public bool IsAgrementVLValide { get; set; }
        public bool IsAgrementVLAnnule { get; set; }
        public bool IsAgrementVLRejete { get; set; }
        public bool IsAgrementVLDemandeEnCours { get; set; }
        public string CodeAgrementCentreRattachementVL { get; set; }

        /* PL */
        public string CodeAgrementPL { get; set; }
        public bool IsAgrementPLValide { get; set; }
        public bool IsAgrementPLAnnule { get; set; }
        public bool IsAgrementPLRejete { get; set; }
        public bool IsAgrementPLDemandeEnCours { get; set; }
        public string CodeAgrementCentreRattachementPL { get; set; }

        /* CL */
        public string CodeAgrementCL { get; set; }
        public bool IsAgrementCLValide { get; set; }
        public bool IsAgrementCLAnnule { get; set; }
        public bool IsAgrementCLRejete { get; set; }
        public bool IsAgrementCLDemandeEnCours { get; set; }
        public string CodeAgrementCentreRattachementCL { get; set; }

        public IList<pContoleurEncour> Resultat { get; set; }

        //PLE le 23/01/2024 Extension catégorie L
        public bool AvecExtentionAgrement { get; set; }

    }
}
