namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeSanctionControleur : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum MotifSanctionEnum
        {
            RETRAIT_DEFINITIF = 1,
            SUSPENSION = 2,
            SUSP_CONSERVATOIRE = 3,
            AVERTISSEMENT = 4,
            AUTRE_SANCTION = 5
        }
    }
}