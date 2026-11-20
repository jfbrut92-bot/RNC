namespace Utac.Rnc2.BusinessObjects
{
    public partial class rProfil2 : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum CodeProfil2
        {
            OTC_ADMINISTRATION = 11,
            OTC_INFORMATIQUE = 12,
            OTC_CONSULTATION = 13,
            PREFECTURE = 20,
            DRIRE = 30,
            CENTRE_INDEPENDANT = 40,
            RESEAU_GESTIONNAIRE = 51,
            RESEAU_CONSULTATION = 52,
            RESEAU_INFORMATIQUE = 53,
            RESEAU_ADMINISTRATEUR = 54,
            DSCR = 60,
            PIV = 70
        }
    }
}