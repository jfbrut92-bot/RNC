namespace Utac.Rnc2.BusinessObjects
{
    public partial class pTraceRechLieuVl : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum CodeResultatRechercheCT
        {
            AUCUN_CT = 0,
            TROUVE_CT = 1,
            ERREUR_TECHNIQUE = 2
        }
    }
}