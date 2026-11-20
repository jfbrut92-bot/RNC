namespace Utac.Rnc2.BusinessObjects
{
    public partial class rTypeFormation : BusinessBase<short>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum TypeFormationEnum : short
        {
            FORMATION_INIT_CL_VL = 100,
            FORMATION_INIT_CL_PL = 200
        };
    }
}
