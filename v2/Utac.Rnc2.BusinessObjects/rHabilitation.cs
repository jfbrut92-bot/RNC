namespace Utac.Rnc2.BusinessObjects
{
    public partial class rHabilitation : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public virtual string LibelleAvecCode
        {
            get
            {
                return this.Id + " - " + this.LibelleHabilitation;
            }
        }

        public enum HabilitationEnum
        {
            Q1,
            Q2,
            Q3,
            Q4
        }
    }
}