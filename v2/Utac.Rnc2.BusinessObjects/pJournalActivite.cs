using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pJournalActivite : BusinessBase<int>
    {

        #region Declarations

        private rUtilisateur _utilisateurModification = null;
        private pCentreEncour _centreEncour = null;
        private pContoleurEncour _contoleurEncour = null;
        private rUtilisateur _utilisateurCreateur = null;
        private string _detail = String.Empty;
        private DateTime _dateCreation = new DateTime();
        private DateTime? _dateModification = null;

        #endregion

        #region Constructors

        public pJournalActivite() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dateCreation);
            sb.Append(_detail);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual rUtilisateur UtilisateurModification
        {
            get { return _utilisateurModification; }
            set
            {
                OnUtilisateurModificationChanging();
                _utilisateurModification = value;
                OnUtilisateurModificationChanged();
            }
        }
        partial void OnUtilisateurModificationChanging();
        partial void OnUtilisateurModificationChanged();

        public virtual pCentreEncour CentreEncour
        {

            get { return _centreEncour; }
            set
            {
                OnCentreEncourChanging();
                _centreEncour = value;
                OnCentreEncourChanged();
            }
        }
        partial void OnCentreEncourChanging();
        partial void OnCentreEncourChanged();

        public virtual pContoleurEncour ContoleurEncour
        {

            get { return _contoleurEncour; }
            set
            {
                OnContoleurEncourChanging();
                _contoleurEncour = value;
                OnContoleurEncourChanged();
            }
        }
        partial void OnContoleurEncourChanging();
        partial void OnContoleurEncourChanged();

        public virtual rUtilisateur UtilisateurCreateur
        {

            get { return _utilisateurCreateur; }
            set
            {
                OnUtilisateurCreateurChanging();
                _utilisateurCreateur = value;
                OnUtilisateurCreateurChanged();
            }
        }
        partial void OnUtilisateurCreateurChanging();
        partial void OnUtilisateurCreateurChanged();

        public virtual string Detail
        {

            get { return _detail; }
            set
            {
                OnDetailChanging();
                _detail = value;
                OnDetailChanged();
            }
        }
        partial void OnDetailChanging();
        partial void OnDetailChanged();

        public virtual DateTime DateCreation
        {

            get { return _dateCreation; }
            set
            {
                OnDateCreationChanging();
                _dateCreation = value;
                OnDateCreationChanged();
            }
        }
        partial void OnDateCreationChanging();
        partial void OnDateCreationChanged();

        public virtual DateTime? DateModification
        {

            get { return _dateModification; }
            set
            {
                OnDateModificationChanging();
                _dateModification = value;
                OnDateModificationChanged();
            }
        }
        partial void OnDateModificationChanging();
        partial void OnDateModificationChanged();

        #endregion

        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }
    }
}