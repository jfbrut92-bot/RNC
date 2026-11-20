namespace Utac.Rnc2.BusinessObjects
{
    public partial class rStatutJuridique : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum Statuts
        {
            ADMIN,
            AUTRE,
            EURL,
            NP,
            SA,
            SARL,
            SAS,
            EI,
            EIRL,
            SASU,
            SNC
        }

    }
}