namespace Utac.Rnc2.BusinessObjects
{
    public partial class pSanctionCentre : BusinessBase<int>
    {
        #region Declarations

        private System.DateTime? _dateEffet = null;
        private string _typeSanctionAutre = null;
        private string _motifSanction = null;
        private System.DateTime? _dateEvenementOrigine = null;
        private System.DateTime? _dateDebut = null;
        private System.DateTime? _dateFin = null;
        //EG le 14/09/2011 suppression private int? _typeAgrementAffecte = null;
        private string _codAgremVl = null;
        private string _codAgremPl = null;

        private rUtilisateur _rUtilisateur = null;
        private pCentreEncour _pCentreEncour = null;
        private rTypeOrganismeDemandeur _rTypeOrganismeDemandeur = null;
        private rTypeSanctionCentre _rTypeSanctionCentre = null;

        #endregion

        #region Constructors

        public pSanctionCentre() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dateEffet);
            sb.Append(_typeSanctionAutre);
            sb.Append(_motifSanction);
            sb.Append(_dateEvenementOrigine);
            sb.Append(_dateDebut);
            sb.Append(_dateFin);
            //EG le 14/09/2011 suppression sb.Append(_typeAgrementAffecte);
            sb.Append(_codAgremVl);
            sb.Append(_codAgremPl);
            sb.Append(_codAgremCl);
            sb.Append(_isAgrementAffecteVL);
            sb.Append(_isAgrementAffectePL);
            sb.Append(_isAgrementAffecteCL);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual System.DateTime? DateEffet
        {
            get { return _dateEffet; }
            set
            {
                OnDateEffetChanging();
                _dateEffet = value;
                OnDateEffetChanged();
            }
        }
        partial void OnDateEffetChanging();
        partial void OnDateEffetChanged();

        public virtual string TypeSanctionAutre
        {
            get { return _typeSanctionAutre; }
            set
            {
                OnTypeSanctionAutreChanging();
                _typeSanctionAutre = value;
                OnTypeSanctionAutreChanged();
            }
        }
        partial void OnTypeSanctionAutreChanging();
        partial void OnTypeSanctionAutreChanged();

        public virtual string MotifSanction
        {
            get { return _motifSanction; }
            set
            {
                OnMotifSanctionChanging();
                _motifSanction = value;
                OnMotifSanctionChanged();
            }
        }
        partial void OnMotifSanctionChanging();
        partial void OnMotifSanctionChanged();

        public virtual System.DateTime? DateEvenementOrigine
        {
            get { return _dateEvenementOrigine; }
            set
            {
                OnDateEvenementOrigineChanging();
                _dateEvenementOrigine = value;
                OnDateEvenementOrigineChanged();
            }
        }
        partial void OnDateEvenementOrigineChanging();
        partial void OnDateEvenementOrigineChanged();

        public virtual System.DateTime? DateDebut
        {
            get { return _dateDebut; }
            set
            {
                OnDateDebutChanging();
                _dateDebut = value;
                OnDateDebutChanged();
            }
        }
        partial void OnDateDebutChanging();
        partial void OnDateDebutChanged();

        public virtual System.DateTime? DateFin
        {
            get { return _dateFin; }
            set
            {
                OnDateFinChanging();
                _dateFin = value;
                OnDateFinChanged();
            }
        }
        partial void OnDateFinChanging();
        partial void OnDateFinChanged();

        /* EG le 14/09/2011 suppression
		public virtual int? TypeAgrementAffecte
        {
            get { return _typeAgrementAffecte; }
			set
			{
				OnTypeAgrementAffecteChanging();
				_typeAgrementAffecte = value;
				OnTypeAgrementAffecteChanged();
			}
        }
		partial void OnTypeAgrementAffecteChanging();
		partial void OnTypeAgrementAffecteChanged();
		*/

        public virtual string CodAgremVl
        {
            get { return _codAgremVl; }
            set
            {
                OnCodAgremVlChanging();
                _codAgremVl = value;
                OnCodAgremVlChanged();
            }
        }
        partial void OnCodAgremVlChanging();
        partial void OnCodAgremVlChanged();

        public virtual string CodAgremPl
        {
            get { return _codAgremPl; }
            set
            {
                OnCodAgremPlChanging();
                _codAgremPl = value;
                OnCodAgremPlChanged();
            }
        }
        partial void OnCodAgremPlChanging();
        partial void OnCodAgremPlChanged();

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

        public virtual rTypeOrganismeDemandeur RTypeOrganismeDemandeur
        {
            get { return _rTypeOrganismeDemandeur; }
            set
            {
                OnRTypeOrganismeDemandeurChanging();
                _rTypeOrganismeDemandeur = value;
                OnRTypeOrganismeDemandeurChanged();
            }
        }
        partial void OnRTypeOrganismeDemandeurChanging();
        partial void OnRTypeOrganismeDemandeurChanged();

        public virtual rTypeSanctionCentre RTypeSanctionCentre
        {
            get { return _rTypeSanctionCentre; }
            set
            {
                OnRTypeSanctionCentreChanging();
                _rTypeSanctionCentre = value;
                OnRTypeSanctionCentreChanged();
            }
        }
        partial void OnRTypeSanctionCentreChanging();
        partial void OnRTypeSanctionCentreChanged();

        #endregion
    }
}
