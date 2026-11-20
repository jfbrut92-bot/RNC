using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pFormationControleurVL : BusinessBase<int>
    {
        #region Declarations

        //private int _idControleurEncours = default(Int32);
        private pContoleurEncour _controleur = null;
        private System.DateTime _dateMiseaJour = new DateTime();
        private System.DateTime? _dateDebut = null;
        private System.DateTime? _dateFin = null;
        private bool _resultatFormation = default(Boolean);

        //private rTypeFormation _rTypeFormation = null;
        private rUtilisateur _rUtilisateur = null;
        //PLE le 26/01/2024 Extension catégorie L
        private rTypeFormation _rTypeFormation = null;

        #endregion

        #region Constructors

        public pFormationControleurVL() { }

        #endregion

        #region Methods

        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            //sb.Append(_idControleurEncours);
            sb.Append(_dateMiseaJour);
            sb.Append(_dateDebut);
            sb.Append(_dateFin);
            sb.Append(_resultatFormation);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual pContoleurEncour Controleur
        {
            get { return _controleur; }
            set
            {
                OnPControleurEncoursChanging();
                _controleur = value;
                OnPControleurEncoursChanged();
            }
        }
        partial void OnPControleurEncoursChanging();
        partial void OnPControleurEncoursChanged();

        public virtual System.DateTime DateMiseaJour
        {
            get { return _dateMiseaJour; }
            set
            {
                OnDateMiseaJourChanging();
                _dateMiseaJour = value;
                OnDateMiseaJourChanged();
            }
        }
        partial void OnDateMiseaJourChanging();
        partial void OnDateMiseaJourChanged();

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

        public virtual bool ResultatFormation
        {
            get { return _resultatFormation; }
            set
            {
                OnResultatFormationChanging();
                _resultatFormation = value;
                OnResultatFormationChanged();
            }
        }
        partial void OnResultatFormationChanging();
        partial void OnResultatFormationChanged();

        public virtual rTypeFormation RTypeFormation
        {
            get { return _rTypeFormation; }
            set
            {
                OnRTypeFormationChanging();
                _rTypeFormation = value;
                OnRTypeFormationChanged();
            }
        }
        partial void OnRTypeFormationChanging();
        partial void OnRTypeFormationChanged();

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
