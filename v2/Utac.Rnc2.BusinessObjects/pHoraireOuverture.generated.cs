namespace Utac.Rnc2.BusinessObjects
{
    public partial class pHoraireOuverture : BusinessBase<int>
    {
        #region Declarations

        private System.DateTime? _lundiMatin = null;
        private System.DateTime? _lundiSoir = null;
        private System.DateTime? _mardiMatin = null;
        private System.DateTime? _mardiSoir = null;
        private System.DateTime? _mercrediMatin = null;
        private System.DateTime? _mercrediSoir = null;
        private System.DateTime? _jeudiMatin = null;
        private System.DateTime? _jeudiSoir = null;
        private System.DateTime? _vendrediMatin = null;
        private System.DateTime? _vendrediSoir = null;
        private System.DateTime? _samediMatin = null;
        private System.DateTime? _samediSoir = null;

        #endregion

        #region Constructors

        public pHoraireOuverture() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_lundiMatin);
            sb.Append(_lundiSoir);
            sb.Append(_mardiMatin);
            sb.Append(_mardiSoir);
            sb.Append(_mercrediMatin);
            sb.Append(_mercrediSoir);
            sb.Append(_jeudiMatin);
            sb.Append(_jeudiSoir);
            sb.Append(_vendrediMatin);
            sb.Append(_vendrediSoir);
            sb.Append(_samediMatin);
            sb.Append(_samediSoir);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual System.DateTime? LundiMatin
        {
            get { return _lundiMatin; }
            set
            {
                OnLundiMatinChanging();
                _lundiMatin = value;
                OnLundiMatinChanged();
            }
        }
        partial void OnLundiMatinChanging();
        partial void OnLundiMatinChanged();

        public virtual System.DateTime? LundiSoir
        {
            get { return _lundiSoir; }
            set
            {
                OnLundiSoirChanging();
                _lundiSoir = value;
                OnLundiSoirChanged();
            }
        }
        partial void OnLundiSoirChanging();
        partial void OnLundiSoirChanged();

        public virtual System.DateTime? MardiMatin
        {
            get { return _mardiMatin; }
            set
            {
                OnMardiMatinChanging();
                _mardiMatin = value;
                OnMardiMatinChanged();
            }
        }
        partial void OnMardiMatinChanging();
        partial void OnMardiMatinChanged();

        public virtual System.DateTime? MardiSoir
        {
            get { return _mardiSoir; }
            set
            {
                OnMardiSoirChanging();
                _mardiSoir = value;
                OnMardiSoirChanged();
            }
        }
        partial void OnMardiSoirChanging();
        partial void OnMardiSoirChanged();

        public virtual System.DateTime? MercrediMatin
        {
            get { return _mercrediMatin; }
            set
            {
                OnMercrediMatinChanging();
                _mercrediMatin = value;
                OnMercrediMatinChanged();
            }
        }
        partial void OnMercrediMatinChanging();
        partial void OnMercrediMatinChanged();

        public virtual System.DateTime? MercrediSoir
        {
            get { return _mercrediSoir; }
            set
            {
                OnMercrediSoirChanging();
                _mercrediSoir = value;
                OnMercrediSoirChanged();
            }
        }
        partial void OnMercrediSoirChanging();
        partial void OnMercrediSoirChanged();

        public virtual System.DateTime? JeudiMatin
        {
            get { return _jeudiMatin; }
            set
            {
                OnJeudiMatinChanging();
                _jeudiMatin = value;
                OnJeudiMatinChanged();
            }
        }
        partial void OnJeudiMatinChanging();
        partial void OnJeudiMatinChanged();

        public virtual System.DateTime? JeudiSoir
        {
            get { return _jeudiSoir; }
            set
            {
                OnJeudiSoirChanging();
                _jeudiSoir = value;
                OnJeudiSoirChanged();
            }
        }
        partial void OnJeudiSoirChanging();
        partial void OnJeudiSoirChanged();

        public virtual System.DateTime? VendrediMatin
        {
            get { return _vendrediMatin; }
            set
            {
                OnVendrediMatinChanging();
                _vendrediMatin = value;
                OnVendrediMatinChanged();
            }
        }
        partial void OnVendrediMatinChanging();
        partial void OnVendrediMatinChanged();

        public virtual System.DateTime? VendrediSoir
        {
            get { return _vendrediSoir; }
            set
            {
                OnVendrediSoirChanging();
                _vendrediSoir = value;
                OnVendrediSoirChanged();
            }
        }
        partial void OnVendrediSoirChanging();
        partial void OnVendrediSoirChanged();

        public virtual System.DateTime? SamediMatin
        {
            get { return _samediMatin; }
            set
            {
                OnSamediMatinChanging();
                _samediMatin = value;
                OnSamediMatinChanged();
            }
        }
        partial void OnSamediMatinChanging();
        partial void OnSamediMatinChanged();

        public virtual System.DateTime? SamediSoir
        {
            get { return _samediSoir; }
            set
            {
                OnSamediSoirChanging();
                _samediSoir = value;
                OnSamediSoirChanged();
            }
        }
        partial void OnSamediSoirChanging();
        partial void OnSamediSoirChanged();

        #endregion
    }
}
