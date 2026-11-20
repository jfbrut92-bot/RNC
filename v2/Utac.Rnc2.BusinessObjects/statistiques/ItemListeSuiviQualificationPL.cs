using System;

namespace Utac.Rnc2.BusinessObjects.statistiques
{
    /// <summary>
    /// 27/08/2018 -PLE- Suivi qualification contrôleur PL
    /// </summary>
    public class ItemListeSuiviQualificationPL
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
        public long CumulActiviteQ1 { get; set; }
        public long CumulActiviteQ2 { get; set; }
        public long CumulActiviteQ3 { get; set; }
        public DateTime DateDernierAuditQ1 { get; set; }
        public DateTime DateDernierAuditQ2 { get; set; }
        public DateTime DateDernierAuditQ3 { get; set; }
        public DateTime DateFormationQ1 { get; set; }
        public DateTime DateFormationQ2 { get; set; }
        public DateTime DateFormationQ3 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }

        //27/08/2018 -PLE- Suivi qualification contrôleur PL  : Ajout résultats année n-1
        public long CumulActiviteQ1AnneeMoins1 { get; set; }
        public long CumulActiviteQ2AnneeMoins1 { get; set; }
        public long CumulActiviteQ3AnneeMoins1 { get; set; }
        public DateTime DateSuiviQualification { get; set; }
    }
}
