namespace Utac.Rnc2.BusinessObjects.statistiques
{
    public partial class ItemListeDesControleursRattachesAuxCentresDuReseau
    {
        public pContoleurEncour Controleur { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string DateNaissance { get; set; }
        public string AgrementControleur { get; set; }
        public string QualificationPL { get; set; }
        public string DateQualificationPl { get; set; }
        public string HabilitationGazVl { get; set; }
        public string DateDeclarationHabilGazVl { get; set; }
        public string DateEffetNotification { get; set; }
        public string AgrementCentreRattachement { get; set; }
        public string CodeReseau { get; set; }
        public string CodeDepartement { get; set; }
        public string CategorieVehicule { get; set; }
        //PLE le 29/01/2024 Extension agrément CL
        public string ExtAgremClStatut { get; set; }
        public string ExtAgremClDateValid { get; set; }
        public string ExtAgremClCatVeh { get; set; }
    }
}
