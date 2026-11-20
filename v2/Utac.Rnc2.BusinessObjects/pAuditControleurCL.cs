using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pAuditControleurCL : BusinessBase<int>
    {
        #region Declarations

        //private int _idControleurEncours = default(Int32);
        private System.DateTime _dateMiseaJour = new DateTime();
        private System.DateTime _dateAudit = new DateTime();
        //private string _codAgremCntr = String.Empty;
        //private string _immatVehicule = String.Empty;
        //private string _numPv = String.Empty;
        private bool _resultatAudit = default(Boolean);

        //private rCategorieVehicule _rCategorieVehicule = null;
        private rUtilisateur _rUtilisateur = null;
        private pContoleurEncour _controleur = null;

        #endregion

        #region Constructors

        public pAuditControleurCL() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            //sb.Append(_idControleurEncours);
            sb.Append(_dateMiseaJour);
            sb.Append(_dateAudit);
            //sb.Append(_codAgremCntr);
            //sb.Append(_immatVehicule);
            //sb.Append(_numPv);
            sb.Append(_resultatAudit);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

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

        public virtual System.DateTime DateAudit
        {
            get { return _dateAudit; }
            set
            {
                OnDateAuditChanging();
                _dateAudit = value;
                OnDateAuditChanged();
            }
        }
        partial void OnDateAuditChanging();
        partial void OnDateAuditChanged();
        /*
        public virtual string CodAgremCntr
        {
            get { return _codAgremCntr; }
            set
            {
                OnCodAgremCntrChanging();
                _codAgremCntr = value;
                OnCodAgremCntrChanged();
            }
        }
        partial void OnCodAgremCntrChanging();
        partial void OnCodAgremCntrChanged();
        
        public virtual string ImmatVehicule
        {
            get { return _immatVehicule; }
			set
			{
				OnImmatVehiculeChanging();
				_immatVehicule = value;
				OnImmatVehiculeChanged();
			}
        }
		partial void OnImmatVehiculeChanging();
		partial void OnImmatVehiculeChanged();
		
		public virtual string NumPv
        {
            get { return _numPv; }
			set
			{
				OnNumPvChanging();
				_numPv = value;
				OnNumPvChanged();
			}
        }
		partial void OnNumPvChanging();
		partial void OnNumPvChanged();
		 */

        public virtual bool ResultatAudit
        {
            get { return _resultatAudit; }
            set
            {
                OnResultatAuditChanging();
                _resultatAudit = value;
                OnResultatAuditChanged();
            }
        }
        partial void OnResultatAuditChanging();
        partial void OnResultatAuditChanged();

        /*
        public virtual rCategorieVehicule RCategorieVehicule
        {
            get { return _rCategorieVehicule; }
			set
			{
				OnRCategorieVehiculeChanging();
				_rCategorieVehicule = value;
				OnRCategorieVehiculeChanged();
			}
        }
		partial void OnRCategorieVehiculeChanging();
		partial void OnRCategorieVehiculeChanged();
		*/

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

        //EG le 30/09/2011
        public virtual pContoleurEncour Controleur
        {
            get { return _controleur; }
            set
            {
                OnControleurChanging();
                _controleur = value;
                OnControleurChanged();
            }
        }
        partial void OnControleurChanging();
        partial void OnControleurChanged();

        #endregion

        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }
    }
}
