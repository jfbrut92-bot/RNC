using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rLoginTempCnr : BusinessBase<string>
    {
        #region Declarations

        private string _motDePasse = String.Empty;
        private string _codCategorieVeh = String.Empty;

        private pCentreEncour _pCentreEncour = null;

        #endregion

        #region Constructors

        public rLoginTempCnr() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_motDePasse);
            sb.Append(_codCategorieVeh);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string MotDePasse
        {
            get { return _motDePasse; }
            set
            {
                OnMotDePasseChanging();
                _motDePasse = value;
                OnMotDePasseChanged();
            }
        }
        partial void OnMotDePasseChanging();
        partial void OnMotDePasseChanged();

        public virtual string CodCategorieVeh
        {
            get { return _codCategorieVeh; }
            set
            {
                OnCodCategorieVehChanging();
                _codCategorieVeh = value;
                OnCodCategorieVehChanged();
            }
        }
        partial void OnCodCategorieVehChanging();
        partial void OnCodCategorieVehChanged();

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
        partial void OnPCentreEncourChanging();
        partial void OnPCentreEncourChanged();

        #endregion
    }
}
