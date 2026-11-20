namespace Utac.Rnc2.BusinessObjects
{
    public partial class pRapportException : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        //Eddy ajout pour syncho avec ajout dans le fichier hbm
        private rReseau _rReseau;
        private pCentreEncour _pCentreEncour;

        partial void OnRReseauChanging();
        partial void OnRReseauChanged();

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

        partial void OnPCentreEncourChanging();
        partial void OnPCentreEncourChanged();

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

        //EG le 27/02/2012
        private System.DateTime? _dateTelechargementRapport = null;

        public virtual System.DateTime? DateTelechargementRapport
        {
            get { return _dateTelechargementRapport; }
            set
            {
                OnDateTelechargementRapportChanging();
                _dateTelechargementRapport = value;
                OnDateTelechargementRapportChanged();
            }
        }
        partial void OnDateTelechargementRapportChanging();
        partial void OnDateTelechargementRapportChanged();

        private string _codUtilisateurTelechargement = null;

        public virtual string CodUtilisateurTelechargement
        {
            get { return _codUtilisateurTelechargement; }
            set
            {
                OnCodUtilisateurTelechargementChanging();
                _codUtilisateurTelechargement = value;
                OnCodUtilisateurTelechargementChanged();
            }
        }
        partial void OnCodUtilisateurTelechargementChanging();
        partial void OnCodUtilisateurTelechargementChanged();

    }
}