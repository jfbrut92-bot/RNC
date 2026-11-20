namespace Utac.Rnc2.BusinessObjects
{
    public partial class rReseauAppartenance : BusinessBase<string>
    {
        #region Declarations

        private string _codUtilisateur = null;
        private string _codReseau = null;

        //private rUtilisateur _rUtilisateur = null;
        //private rReseau _rReseau = null;

        #endregion

        #region Constructors

        public rReseauAppartenance() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_codUtilisateur);
            sb.Append(_codReseau);

            return sb.ToString().GetHashCode();
        }

        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        #endregion

        #region Properties

        public override string Id
        {
            get
            {
                System.Text.StringBuilder uniqueId = new System.Text.StringBuilder();
                uniqueId.Append(_codUtilisateur.ToString());
                uniqueId.Append("^");
                uniqueId.Append(_codReseau.ToString());
                return uniqueId.ToString();
            }
        }

        public virtual string CodUtilisateur
        {
            get { return _codUtilisateur; }
            set
            {
                OnCodUtilisateurChanging();
                _codUtilisateur = value;
                OnCodUtilisateurChanged();
            }
        }

        partial void OnCodUtilisateurChanging();
        partial void OnCodUtilisateurChanged();

        public virtual string CodReseau
        {
            get { return _codReseau; }
            set
            {
                OnIdControleurHistoChanging();
                _codReseau = value;
                OnIdControleurHistoChanged();
            }
        }
        partial void OnIdControleurHistoChanging();
        partial void OnIdControleurHistoChanged();

        /*public virtual rUtilisateur RRUtilisateur
        {
            get { return _rUtilisateur; }
            set
            {
                OnRRUtilisateurChanging();
                _rUtilisateur = value;
                _codUtilisateur = _rUtilisateur.Id;
                OnRRUtilisateurChanged();
            }
        }*/
        partial void OnRRUtilisateurChanging();
        partial void OnRRUtilisateurChanged();

        /* public virtual rReseau RReseau
         {
             get { return _rReseau; }
             set
             {
                 OnRReseauChanging();
                 _rReseau = value;
                 _codReseau = _rReseau.Id;
                 OnRReseauChanged();
             }
         }*/
        partial void OnRReseauChanging();
        partial void OnRReseauChanged();

        #endregion
    }
}
