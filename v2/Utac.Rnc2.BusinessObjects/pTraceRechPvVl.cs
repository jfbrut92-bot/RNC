namespace Utac.Rnc2.BusinessObjects
{
    public partial class pTraceRechPvVl : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum CodeTypeRechercheCT
        {
            TOUT = 1,
            REDUIT = 2
        }

        public enum CodeResultatRechercheCT
        {
            AUCUN_CT = 0,
            TROUVE_CT = 1,
            ERREUR_TECHNIQUE = 2,
            AUCUN_CT_PL = 3,
            TROUVE_CT_PL = 4,
            ERREUR_TECHNIQUE_PL = 5,
            AUCUN_CT_CL = 6,
            TROUVE_CT_CL = 7,
            ERREUR_TECHNIQUE_CL = 8
        }
    }
}
