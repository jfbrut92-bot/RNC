namespace Utac.Rnc2.BusinessObjects
{
    public partial class pTraceRechPvVl : BusinessBase<int>
    {
        #region Declarations

        private System.DateTime? _dateRecherche = null;
        private string _numPvCtrl = null;
        private string _codAgremCentre = null;
        private System.DateTime? _dateCtrl = null;
        private byte? _typeRecherche = null;
        private byte? _codResultat = null;

        private rUtilisateur _rUtilisateur = null;

        #endregion

        #region Constructors

        public pTraceRechPvVl() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dateRecherche);
            sb.Append(_numPvCtrl);
            sb.Append(_codAgremCentre);
            sb.Append(_dateCtrl);
            sb.Append(_typeRecherche);
            sb.Append(_codResultat);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual System.DateTime? DateRecherche
        {
            get { return _dateRecherche; }
            set
            {
                OnDateRechercheChanging();
                _dateRecherche = value;
                OnDateRechercheChanged();
            }
        }
        partial void OnDateRechercheChanging();
        partial void OnDateRechercheChanged();

        public virtual string NumPvCtrl
        {
            get { return _numPvCtrl; }
            set
            {
                OnNumPvCtrlChanging();
                _numPvCtrl = value;
                OnNumPvCtrlChanged();
            }
        }
        partial void OnNumPvCtrlChanging();
        partial void OnNumPvCtrlChanged();

        public virtual string CodAgremCentre
        {
            get { return _codAgremCentre; }
            set
            {
                OnCodAgremCentreChanging();
                _codAgremCentre = value;
                OnCodAgremCentreChanged();
            }
        }
        partial void OnCodAgremCentreChanging();
        partial void OnCodAgremCentreChanged();

        public virtual System.DateTime? DateCtrl
        {
            get { return _dateCtrl; }
            set
            {
                OnDateCtrlChanging();
                _dateCtrl = value;
                OnDateCtrlChanged();
            }
        }
        partial void OnDateCtrlChanging();
        partial void OnDateCtrlChanged();

        public virtual byte? TypeRecherche
        {
            get { return _typeRecherche; }
            set
            {
                OnTypeRechercheChanging();
                _typeRecherche = value;
                OnTypeRechercheChanged();
            }
        }
        partial void OnTypeRechercheChanging();
        partial void OnTypeRechercheChanged();

        public virtual byte? CodResultat
        {
            get { return _codResultat; }
            set
            {
                OnCodResultatChanging();
                _codResultat = value;
                OnCodResultatChanged();
            }
        }
        partial void OnCodResultatChanging();
        partial void OnCodResultatChanged();

        public virtual rUtilisateur RUtilisateur
        {
            get { return _rUtilisateur; }
            set
            {
                OnRUtilisateurChanging();
                _rUtilisateur = value;
                OnRUtilisateurChanged();
            }
        }
        partial void OnRUtilisateurChanging();
        partial void OnRUtilisateurChanged();

        #endregion
    }
}
