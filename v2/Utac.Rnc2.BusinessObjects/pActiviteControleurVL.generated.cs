namespace Utac.Rnc2.BusinessObjects
{
    public partial class pActiviteControleurVL : BusinessBase<int>
    {
        #region Declarations

        private string _codReseauRattachementVl = null;
        private string _nom = null;
        private string _prenom = null;
        private string _codAgremCntlr = null;
        private string _codAgremCentre = null;
        private string _codDepartement = null;
        private string _region = null;
        private System.DateTime? _dateEffetNotificationVl = null;
        private System.DateTime? _dateDebutRattachementVl = null;
        private System.DateTime? _dateFinAgrementVl = null;
        private System.DateTime? _dateDernierCtrl = null;
        private short? _activiteJanvierN1 = null;
        private short? _activiteFevrierN1 = null;
        private short? _activiteMarsN1 = null;
        private short? _activiteAvrilN1 = null;
        private short? _activiteMaiN1 = null;
        private short? _activiteJuinN1 = null;
        private short? _activiteJuilletN1 = null;
        private short? _activiteAoutN1 = null;
        private short? _activiteSeptembreN1 = null;
        private short? _activiteOctobreN1 = null;
        private short? _activiteNovembreN1 = null;
        private short? _activiteDecembreN1 = null;
        private short? _totalVtpN1 = null;
        private short? _seuilN1ARealiser = null;
        private string _seuilAtteintN1 = null;
        private short? _activiteJanvierN = null;
        private short? _activiteFevrierN = null;
        private short? _activiteMarsN = null;
        private short? _activiteAvrilN = null;
        private short? _activiteMaiN = null;
        private short? _activiteJuinN = null;
        private short? _activiteJuilletN = null;
        private short? _activiteAoutN = null;
        private short? _activiteSeptembreN = null;
        private short? _activiteOctobreN = null;
        private short? _activiteNovembreN = null;
        private short? _activiteDecembreN = null;
        private short? _totalVtpN = null;
        private short? _seuilNARealiser = null;
        private string _seuilAtteintN = null;
        private int _anneeMois;

        #endregion

        #region Constructors

        public pActiviteControleurVL() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_codReseauRattachementVl);
            sb.Append(_nom);
            sb.Append(_prenom);
            sb.Append(_codAgremCntlr);
            sb.Append(_codAgremCentre);
            sb.Append(_codDepartement);
            sb.Append(_region);
            sb.Append(_dateEffetNotificationVl);
            sb.Append(_dateDebutRattachementVl);
            sb.Append(_dateFinAgrementVl);
            sb.Append(_dateDernierCtrl);
            sb.Append(_activiteJanvierN1);
            sb.Append(_activiteFevrierN1);
            sb.Append(_activiteMarsN1);
            sb.Append(_activiteAvrilN1);
            sb.Append(_activiteMaiN1);
            sb.Append(_activiteJuinN1);
            sb.Append(_activiteJuilletN1);
            sb.Append(_activiteAoutN1);
            sb.Append(_activiteSeptembreN1);
            sb.Append(_activiteOctobreN1);
            sb.Append(_activiteNovembreN1);
            sb.Append(_activiteDecembreN1);
            sb.Append(_totalVtpN1);
            sb.Append(_seuilN1ARealiser);
            sb.Append(_seuilAtteintN1);
            sb.Append(_activiteJanvierN);
            sb.Append(_activiteFevrierN);
            sb.Append(_activiteMarsN);
            sb.Append(_activiteAvrilN);
            sb.Append(_activiteMaiN);
            sb.Append(_activiteJuinN);
            sb.Append(_activiteJuilletN);
            sb.Append(_activiteAoutN);
            sb.Append(_activiteSeptembreN);
            sb.Append(_activiteOctobreN);
            sb.Append(_activiteNovembreN);
            sb.Append(_activiteDecembreN);
            sb.Append(_totalVtpN);
            sb.Append(_seuilNARealiser);
            sb.Append(_seuilAtteintN);
            sb.Append(_anneeMois);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string CodReseauRattachementVl
        {
            get { return _codReseauRattachementVl; }
            set
            {
                OnCodReseauRattachementVlChanging();
                _codReseauRattachementVl = value;
                OnCodReseauRattachementVlChanged();
            }
        }
        partial void OnCodReseauRattachementVlChanging();
        partial void OnCodReseauRattachementVlChanged();

        public virtual string Nom
        {
            get { return _nom; }
            set
            {
                OnNomChanging();
                _nom = value;
                OnNomChanged();
            }
        }
        partial void OnNomChanging();
        partial void OnNomChanged();

        public virtual string Prenom
        {
            get { return _prenom; }
            set
            {
                OnPrenomChanging();
                _prenom = value;
                OnPrenomChanged();
            }
        }
        partial void OnPrenomChanging();
        partial void OnPrenomChanged();

        public virtual string CodAgremCntlr
        {
            get { return _codAgremCntlr; }
            set
            {
                OnCodAgremCntlrChanging();
                _codAgremCntlr = value;
                OnCodAgremCntlrChanged();
            }
        }
        partial void OnCodAgremCntlrChanging();
        partial void OnCodAgremCntlrChanged();

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

        public virtual string CodDepartement
        {
            get { return _codDepartement; }
            set
            {
                OnCodDepartementChanging();
                _codDepartement = value;
                OnCodDepartementChanged();
            }
        }
        partial void OnCodDepartementChanging();
        partial void OnCodDepartementChanged();

        public virtual string Region
        {
            get { return _region; }
            set
            {
                OnRegionChanging();
                _region = value;
                OnRegionChanged();
            }
        }
        partial void OnRegionChanging();
        partial void OnRegionChanged();

        public virtual System.DateTime? DateEffetNotificationVl
        {
            get { return _dateEffetNotificationVl; }
            set
            {
                OnDateEffetNotificationVlChanging();
                _dateEffetNotificationVl = value;
                OnDateEffetNotificationVlChanged();
            }
        }
        partial void OnDateEffetNotificationVlChanging();
        partial void OnDateEffetNotificationVlChanged();

        public virtual System.DateTime? DateDebutRattachementVl
        {
            get { return _dateDebutRattachementVl; }
            set
            {
                OnDateDebutRattachementVlChanging();
                _dateDebutRattachementVl = value;
                OnDateDebutRattachementVlChanged();
            }
        }
        partial void OnDateDebutRattachementVlChanging();
        partial void OnDateDebutRattachementVlChanged();

        public virtual System.DateTime? DateFinAgrementVl
        {
            get { return _dateFinAgrementVl; }
            set
            {
                OnDateFinAgrementVlChanging();
                _dateFinAgrementVl = value;
                OnDateFinAgrementVlChanged();
            }
        }
        partial void OnDateFinAgrementVlChanging();
        partial void OnDateFinAgrementVlChanged();

        public virtual System.DateTime? DateDernierCtrl
        {
            get { return _dateDernierCtrl; }
            set
            {
                OnDateDernierCtrlChanging();
                _dateDernierCtrl = value;
                OnDateDernierCtrlChanged();
            }
        }
        partial void OnDateDernierCtrlChanging();
        partial void OnDateDernierCtrlChanged();

        public virtual short? ActiviteJanvierN1
        {
            get { return _activiteJanvierN1; }
            set
            {
                OnActiviteJanvierN1Changing();
                _activiteJanvierN1 = value;
                OnActiviteJanvierN1Changed();
            }
        }
        partial void OnActiviteJanvierN1Changing();
        partial void OnActiviteJanvierN1Changed();

        public virtual short? ActiviteFevrierN1
        {
            get { return _activiteFevrierN1; }
            set
            {
                OnActiviteFevrierN1Changing();
                _activiteFevrierN1 = value;
                OnActiviteFevrierN1Changed();
            }
        }
        partial void OnActiviteFevrierN1Changing();
        partial void OnActiviteFevrierN1Changed();

        public virtual short? ActiviteMarsN1
        {
            get { return _activiteMarsN1; }
            set
            {
                OnActiviteMarsN1Changing();
                _activiteMarsN1 = value;
                OnActiviteMarsN1Changed();
            }
        }
        partial void OnActiviteMarsN1Changing();
        partial void OnActiviteMarsN1Changed();

        public virtual short? ActiviteAvrilN1
        {
            get { return _activiteAvrilN1; }
            set
            {
                OnActiviteAvrilN1Changing();
                _activiteAvrilN1 = value;
                OnActiviteAvrilN1Changed();
            }
        }
        partial void OnActiviteAvrilN1Changing();
        partial void OnActiviteAvrilN1Changed();

        public virtual short? ActiviteMaiN1
        {
            get { return _activiteMaiN1; }
            set
            {
                OnActiviteMaiN1Changing();
                _activiteMaiN1 = value;
                OnActiviteMaiN1Changed();
            }
        }
        partial void OnActiviteMaiN1Changing();
        partial void OnActiviteMaiN1Changed();

        public virtual short? ActiviteJuinN1
        {
            get { return _activiteJuinN1; }
            set
            {
                OnActiviteJuinN1Changing();
                _activiteJuinN1 = value;
                OnActiviteJuinN1Changed();
            }
        }
        partial void OnActiviteJuinN1Changing();
        partial void OnActiviteJuinN1Changed();

        public virtual short? ActiviteJuilletN1
        {
            get { return _activiteJuilletN1; }
            set
            {
                OnActiviteJuilletN1Changing();
                _activiteJuilletN1 = value;
                OnActiviteJuilletN1Changed();
            }
        }
        partial void OnActiviteJuilletN1Changing();
        partial void OnActiviteJuilletN1Changed();

        public virtual short? ActiviteAoutN1
        {
            get { return _activiteAoutN1; }
            set
            {
                OnActiviteAoutN1Changing();
                _activiteAoutN1 = value;
                OnActiviteAoutN1Changed();
            }
        }
        partial void OnActiviteAoutN1Changing();
        partial void OnActiviteAoutN1Changed();

        public virtual short? ActiviteSeptembreN1
        {
            get { return _activiteSeptembreN1; }
            set
            {
                OnActiviteSeptembreN1Changing();
                _activiteSeptembreN1 = value;
                OnActiviteSeptembreN1Changed();
            }
        }
        partial void OnActiviteSeptembreN1Changing();
        partial void OnActiviteSeptembreN1Changed();

        public virtual short? ActiviteOctobreN1
        {
            get { return _activiteOctobreN1; }
            set
            {
                OnActiviteOctobreN1Changing();
                _activiteOctobreN1 = value;
                OnActiviteOctobreN1Changed();
            }
        }
        partial void OnActiviteOctobreN1Changing();
        partial void OnActiviteOctobreN1Changed();

        public virtual short? ActiviteNovembreN1
        {
            get { return _activiteNovembreN1; }
            set
            {
                OnActiviteNovembreN1Changing();
                _activiteNovembreN1 = value;
                OnActiviteNovembreN1Changed();
            }
        }
        partial void OnActiviteNovembreN1Changing();
        partial void OnActiviteNovembreN1Changed();

        public virtual short? ActiviteDecembreN1
        {
            get { return _activiteDecembreN1; }
            set
            {
                OnActiviteDecembreN1Changing();
                _activiteDecembreN1 = value;
                OnActiviteDecembreN1Changed();
            }
        }
        partial void OnActiviteDecembreN1Changing();
        partial void OnActiviteDecembreN1Changed();

        public virtual short? TotalVtpN1
        {
            get { return _totalVtpN1; }
            set
            {
                OnTotalVtpN1Changing();
                _totalVtpN1 = value;
                OnTotalVtpN1Changed();
            }
        }
        partial void OnTotalVtpN1Changing();
        partial void OnTotalVtpN1Changed();

        public virtual short? SeuilN1ARealiser
        {
            get { return _seuilN1ARealiser; }
            set
            {
                OnSeuilN1ARealiserChanging();
                _seuilN1ARealiser = value;
                OnSeuilN1ARealiserChanged();
            }
        }
        partial void OnSeuilN1ARealiserChanging();
        partial void OnSeuilN1ARealiserChanged();

        public virtual string SeuilAtteintN1
        {
            get { return _seuilAtteintN1; }
            set
            {
                OnSeuilAtteintN1Changing();
                _seuilAtteintN1 = value;
                OnSeuilAtteintN1Changed();
            }
        }
        partial void OnSeuilAtteintN1Changing();
        partial void OnSeuilAtteintN1Changed();

        public virtual short? ActiviteJanvierN
        {
            get { return _activiteJanvierN; }
            set
            {
                OnActiviteJanvierNChanging();
                _activiteJanvierN = value;
                OnActiviteJanvierNChanged();
            }
        }
        partial void OnActiviteJanvierNChanging();
        partial void OnActiviteJanvierNChanged();

        public virtual short? ActiviteFevrierN
        {
            get { return _activiteFevrierN; }
            set
            {
                OnActiviteFevrierNChanging();
                _activiteFevrierN = value;
                OnActiviteFevrierNChanged();
            }
        }
        partial void OnActiviteFevrierNChanging();
        partial void OnActiviteFevrierNChanged();

        public virtual short? ActiviteMarsN
        {
            get { return _activiteMarsN; }
            set
            {
                OnActiviteMarsNChanging();
                _activiteMarsN = value;
                OnActiviteMarsNChanged();
            }
        }
        partial void OnActiviteMarsNChanging();
        partial void OnActiviteMarsNChanged();

        public virtual short? ActiviteAvrilN
        {
            get { return _activiteAvrilN; }
            set
            {
                OnActiviteAvrilNChanging();
                _activiteAvrilN = value;
                OnActiviteAvrilNChanged();
            }
        }
        partial void OnActiviteAvrilNChanging();
        partial void OnActiviteAvrilNChanged();

        public virtual short? ActiviteMaiN
        {
            get { return _activiteMaiN; }
            set
            {
                OnActiviteMaiNChanging();
                _activiteMaiN = value;
                OnActiviteMaiNChanged();
            }
        }
        partial void OnActiviteMaiNChanging();
        partial void OnActiviteMaiNChanged();

        public virtual short? ActiviteJuinN
        {
            get { return _activiteJuinN; }
            set
            {
                OnActiviteJuinNChanging();
                _activiteJuinN = value;
                OnActiviteJuinNChanged();
            }
        }
        partial void OnActiviteJuinNChanging();
        partial void OnActiviteJuinNChanged();

        public virtual short? ActiviteJuilletN
        {
            get { return _activiteJuilletN; }
            set
            {
                OnActiviteJuilletNChanging();
                _activiteJuilletN = value;
                OnActiviteJuilletNChanged();
            }
        }
        partial void OnActiviteJuilletNChanging();
        partial void OnActiviteJuilletNChanged();

        public virtual short? ActiviteAoutN
        {
            get { return _activiteAoutN; }
            set
            {
                OnActiviteAoutNChanging();
                _activiteAoutN = value;
                OnActiviteAoutNChanged();
            }
        }
        partial void OnActiviteAoutNChanging();
        partial void OnActiviteAoutNChanged();

        public virtual short? ActiviteSeptembreN
        {
            get { return _activiteSeptembreN; }
            set
            {
                OnActiviteSeptembreNChanging();
                _activiteSeptembreN = value;
                OnActiviteSeptembreNChanged();
            }
        }
        partial void OnActiviteSeptembreNChanging();
        partial void OnActiviteSeptembreNChanged();

        public virtual short? ActiviteOctobreN
        {
            get { return _activiteOctobreN; }
            set
            {
                OnActiviteOctobreNChanging();
                _activiteOctobreN = value;
                OnActiviteOctobreNChanged();
            }
        }
        partial void OnActiviteOctobreNChanging();
        partial void OnActiviteOctobreNChanged();

        public virtual short? ActiviteNovembreN
        {
            get { return _activiteNovembreN; }
            set
            {
                OnActiviteNovembreNChanging();
                _activiteNovembreN = value;
                OnActiviteNovembreNChanged();
            }
        }
        partial void OnActiviteNovembreNChanging();
        partial void OnActiviteNovembreNChanged();

        public virtual short? ActiviteDecembreN
        {
            get { return _activiteDecembreN; }
            set
            {
                OnActiviteDecembreNChanging();
                _activiteDecembreN = value;
                OnActiviteDecembreNChanged();
            }
        }
        partial void OnActiviteDecembreNChanging();
        partial void OnActiviteDecembreNChanged();

        public virtual short? TotalVtpN
        {
            get { return _totalVtpN; }
            set
            {
                OnTotalVtpNChanging();
                _totalVtpN = value;
                OnTotalVtpNChanged();
            }
        }
        partial void OnTotalVtpNChanging();
        partial void OnTotalVtpNChanged();

        public virtual short? SeuilNARealiser
        {
            get { return _seuilNARealiser; }
            set
            {
                OnSeuilNARealiserChanging();
                _seuilNARealiser = value;
                OnSeuilNARealiserChanged();
            }
        }
        partial void OnSeuilNARealiserChanging();
        partial void OnSeuilNARealiserChanged();

        public virtual string SeuilAtteintN
        {
            get { return _seuilAtteintN; }
            set
            {
                OnSeuilAtteintNChanging();
                _seuilAtteintN = value;
                OnSeuilAtteintNChanged();
            }
        }
        partial void OnSeuilAtteintNChanging();
        partial void OnSeuilAtteintNChanged();

        public virtual int AnneeMois
        {
            get { return _anneeMois; }
            set
            {
                OnAnneeMoisChanging();
                _anneeMois = value;
                OnAnneeMoisChanged();
            }
        }
        partial void OnAnneeMoisChanging();
        partial void OnAnneeMoisChanged();

        #endregion
    }
}
