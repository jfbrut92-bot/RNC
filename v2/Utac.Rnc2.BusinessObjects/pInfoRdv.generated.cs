namespace Utac.Rnc2.BusinessObjects
{
    public partial class pInfoRdv : BusinessBase<int>
    {
        #region Declarations

        private string _numTelCentrale = null;
        private string _numTelTarif = null;
        private string _serveurMinitel = null;
        private string _siteWeb = null;

        private pCentreEncour _pCentreEncour = null;
        private rReseau _rReseau = null;

        #endregion

        #region Constructors

        public pInfoRdv() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_numTelCentrale);
            sb.Append(_numTelTarif);
            sb.Append(_serveurMinitel);
            sb.Append(_siteWeb);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string NumTelCentrale
        {
            get { return _numTelCentrale; }
            set
            {
                OnNumTelCentraleChanging();
                _numTelCentrale = value;
                OnNumTelCentraleChanged();
            }
        }
        partial void OnNumTelCentraleChanging();
        partial void OnNumTelCentraleChanged();

        public virtual string NumTelTarif
        {
            get { return _numTelTarif; }
            set
            {
                OnNumTelTarifChanging();
                _numTelTarif = value;
                OnNumTelTarifChanged();
            }
        }
        partial void OnNumTelTarifChanging();
        partial void OnNumTelTarifChanged();

        public virtual string ServeurMinitel
        {
            get { return _serveurMinitel; }
            set
            {
                OnServeurMinitelChanging();
                _serveurMinitel = value;
                OnServeurMinitelChanged();
            }
        }
        partial void OnServeurMinitelChanging();
        partial void OnServeurMinitelChanged();

        public virtual string SiteWeb
        {
            get { return _siteWeb; }
            set
            {
                OnSiteWebChanging();
                _siteWeb = value;
                OnSiteWebChanged();
            }
        }
        partial void OnSiteWebChanging();
        partial void OnSiteWebChanged();

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

        public virtual rReseau RReseau
        {
            get { return _rReseau; }
            set
            {
                OnRReseauChanging();
                _rReseau = value;
                OnRReseauChanged();
            }
        }
        partial void OnRReseauChanging();
        partial void OnRReseauChanged();

        #endregion
    }
}
