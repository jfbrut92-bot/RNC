namespace Utac.Rnc2.BusinessObjects
{
    public partial class rProfil : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum CodeProfil
        {
            OTC = 1,
            PREFECTURE = 2,
            DRIRE = 3,
            CENTRE_INDEPENDANT = 4,
            RESEAU = 5,
            DSCR = 6,
            PIV = 7

        }
    }
}