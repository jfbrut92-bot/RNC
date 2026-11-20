namespace Utac.Rnc2.BusinessObjects
{
    public partial class pRapportExceptionCl : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */

        }

        private pCentreEncour _pCentreEncour;

        partial void OnPCentreEncourChanging();
        partial void OnPCentreEncourChanged();

        public virtual pCentreEncour PCentreEncour
        {
            get { return _pCentreEncour; }
            set
            {
                OnPCentreEncourChanging();
                _pCentreEncour = value;
                OnPCentreEncourChanged();
            }
        }
    }
}
