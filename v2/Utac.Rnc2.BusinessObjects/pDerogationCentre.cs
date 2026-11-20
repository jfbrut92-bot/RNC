namespace Utac.Rnc2.BusinessObjects
{
    public partial class pDerogationCentre : BusinessBase<int>
    {

        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        #region Declarations

        private System.DateTime? _dateDebut = null;
        private System.DateTime? _dateFin = null;
        private string _commentaire = string.Empty;
        /* EG le 28/02/2012 supprimé
        private bool _isAgrementAffecteVL = default(bool);
        private bool _isAgrementAffectePL = default(bool);
        private bool _isAgrementAffecteCL = default(bool);
        */
        //EG le 28/02/2012 : ajout
        private rCategorieVehicule.CategorieEnum _categorieVehicule = default(rCategorieVehicule.CategorieEnum);
        private rUtilisateur _rUtilisateur = null;
        private pCentreEncour _pCentreEncour = null;

        #endregion

        #region Constructors

        public pDerogationCentre() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dateDebut);
            sb.Append(_dateFin);
            /*sb.Append(_isAgrementAffecteVL);
            sb.Append(_isAgrementAffectePL);
            sb.Append(_isAgrementAffecteCL);*/
            sb.Append(_categorieVehicule);
            sb.Append(_commentaire);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

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

        public virtual string Commentaire
        {
            get { return _commentaire; }
            set
            {
                OnCommentaireChanging();
                _commentaire = value;
                OnCommentaireChanged();
            }
        }
        partial void OnCommentaireChanging();
        partial void OnCommentaireChanged();

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

        /* EG le 28/02/2012 : ajout */
        public virtual rCategorieVehicule.CategorieEnum CategorieVehicule
        {
            get { return _categorieVehicule; }
            set
            {
                OnCategorieVehiculeChanging();
                _categorieVehicule = value;
                OnCategorieVehiculeChanged();
            }
        }
        partial void OnCategorieVehiculeChanging();
        partial void OnCategorieVehiculeChanged();

        /* EG le 28/02/2012 : supprimé
        public virtual bool IsAgrementAffecteVL
        {
            get
            {
                return this._isAgrementAffecteVL;
            }

            set
            {
                OnIsAgrementAffecteVLChanging();
                _isAgrementAffecteVL = value;
                OnIsAgrementAffecteVLChanged();
            }
        }
        partial void OnIsAgrementAffecteVLChanging();
        partial void OnIsAgrementAffecteVLChanged();

        public virtual bool IsAgrementAffectePL
        {
            get
            {
                return this._isAgrementAffectePL;
            }

            set
            {
                OnIsAgrementAffectePLChanging();
                _isAgrementAffectePL = value;
                OnIsAgrementAffectePLChanged();
            }
        }
        partial void OnIsAgrementAffectePLChanging();
        partial void OnIsAgrementAffectePLChanged();

        public virtual bool IsAgrementAffecteCL
        {
            get
            {
                return this._isAgrementAffecteCL;
            }

            set
            {
                OnIsAgrementAffecteCLChanging();
                _isAgrementAffecteCL = value;
                OnIsAgrementAffecteCLChanged();
            }
        }
        partial void OnIsAgrementAffecteCLChanging();
        partial void OnIsAgrementAffecteCLChanged();
		*/

        #endregion

    }
}
