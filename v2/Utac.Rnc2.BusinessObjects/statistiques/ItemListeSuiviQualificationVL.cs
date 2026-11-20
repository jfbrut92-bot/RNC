using System;

namespace Utac.Rnc2.BusinessObjects.statistiques
{
    public class ItemListeSuiviQualificationVL
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string CodAgrement { get; set; }
        public string CodeAgrementCentre { get; set; }
        public string CodeDepartement { get; set; }
        public string LibelleRegion { get; set; }
        public DateTime DateDebutRattachementCentre { get; set; }
        public string CodeHabilitation { get; set; }
        public DateTime DateDebutHabilitation { get; set; }
        public string CodReseau { get; set; }
        public DateTime DateAgrement { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }

    }
}
