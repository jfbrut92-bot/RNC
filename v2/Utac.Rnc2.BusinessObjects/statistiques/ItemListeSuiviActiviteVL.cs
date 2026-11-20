using System;

namespace Utac.Rnc2.BusinessObjects.statistiques
{
    public partial class ItemListeSuiviActiviteVL
    {
        public string CodeReseau { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string CodeAgrement { get; set; }
        public string CodeAgrementCentre { get; set; }
        public string CodeDepartement { get; set; }
        public string Region { get; set; }
        public DateTime DateAgrement { get; set; }
        public DateTime DateFinAgrement { get; set; }
        public DateTime DateDebutRattachementCentre { get; set; }
        public DateTime DateDernierCtrl { get; set; }

        public long ActiviteJanvierN1 { get; set; }
        public long ActiviteFevrierN1 { get; set; }
        public long ActiviteMarsN1 { get; set; }
        public long ActiviteAvrilN1 { get; set; }
        public long ActiviteMaiN1 { get; set; }
        public long ActiviteJuinN1 { get; set; }
        public long ActiviteJuilletN1 { get; set; }
        public long ActiviteAoutN1 { get; set; }
        public long ActiviteSeptembreN1 { get; set; }
        public long ActiviteOctobreN1 { get; set; }
        public long ActiviteNovembreN1 { get; set; }
        public long ActiviteDecembreN1 { get; set; }
        public long TotalVtpN1 { get; set; }
        public long SeuilN1ARealiser { get; set; }
        public string SeuilAtteintN1 { get; set; }

        public long ActiviteJanvierN { get; set; }
        public long ActiviteFevrierN { get; set; }
        public long ActiviteMarsN { get; set; }
        public long ActiviteAvrilN { get; set; }
        public long ActiviteMaiN { get; set; }
        public long ActiviteJuinN { get; set; }
        public long ActiviteJuilletN { get; set; }
        public long ActiviteAoutN { get; set; }
        public long ActiviteSeptembreN { get; set; }
        public long ActiviteOctobreN { get; set; }
        public long ActiviteNovembreN { get; set; }
        public long ActiviteDecembreN { get; set; }
        public long TotalVtpN { get; set; }
        public long SeuilNARealiser { get; set; }
        public string SeuilAtteintN { get; set; }

    }
}
