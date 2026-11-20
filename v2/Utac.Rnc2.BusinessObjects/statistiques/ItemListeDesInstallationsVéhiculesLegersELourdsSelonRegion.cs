namespace Utac.Rnc2.BusinessObjects.statistiques
{
    public partial class ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion
    {
        public string Region { get; set; }
        public string RaisonSociale { get; set; }
        public string L1Adresse { get; set; }
        public string L2Adresse { get; set; }
        public string CPCentre { get; set; }
        public string Ville { get; set; }
        public string Agrement { get; set; }
        public string DateEffetAgrement { get; set; }
        public string DateEcheance { get; set; }
        public string CodeDepartement { get; set; }
        public string Reseau { get; set; }
        public string TypeCentre { get; set; }
        public string CategorieVehicule { get; set; }
        public string ControlGaz { get; set; }
        public string Decelerometre { get; set; }
        public string RouleauFou { get; set; }
        public string BancRotationInversee { get; set; }
        //PLE le 29/01/2024 Extension agrément CL
        public string ExtAgremClStatut { get; set; }
        public string ExtAgremClDateValid { get; set; }
        public string ExtAgremClCatVeh { get; set; }
        public pCentreEncour Centre { get; set; }

    }
}
