namespace Utac.Rnc2.BusinessObjects
{
    public partial class pConformiteLogiciel : BusinessBase<int>
    {
        #region Declarations

        private string _versionMajeureLogiciel = null;
        private string _nomRaisonSocialeDemandeur = null;
        private string _nomEtQualiteDuRepresentant = null;
        private string _adresse = null;
        private string _resultatConformite = null;
        private System.DateTime? _dateLeveeReserve = null;
        private string _commentaire = null;
        private string _refAttestationConformite = null;
        private System.DateTime? _dateAttestation = null;
        private System.DateTime? _dateFinReserve = null;

        private pCentreEncour _pCentreEncour = null;
        private rReseau _rReseau = null;
        private pLogiciel _pLogiciel = null;

        #endregion

        #region Constructors

        public pConformiteLogiciel() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_versionMajeureLogiciel);
            sb.Append(_nomRaisonSocialeDemandeur);
            sb.Append(_nomEtQualiteDuRepresentant);
            sb.Append(_adresse);
            sb.Append(_resultatConformite);
            sb.Append(_dateLeveeReserve);
            sb.Append(_commentaire);
            sb.Append(_refAttestationConformite);
            sb.Append(_dateAttestation);
            sb.Append(_dateFinReserve);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string VersionMajeureLogiciel
        {
            get { return _versionMajeureLogiciel; }
            set
            {
                OnVersionMajeureLogicielChanging();
                _versionMajeureLogiciel = value;
                OnVersionMajeureLogicielChanged();
            }
        }
        partial void OnVersionMajeureLogicielChanging();
        partial void OnVersionMajeureLogicielChanged();

        public virtual string NomRaisonSocialeDemandeur
        {
            get { return _nomRaisonSocialeDemandeur; }
            set
            {
                OnNomRaisonSocialeDemandeurChanging();
                _nomRaisonSocialeDemandeur = value;
                OnNomRaisonSocialeDemandeurChanged();
            }
        }
        partial void OnNomRaisonSocialeDemandeurChanging();
        partial void OnNomRaisonSocialeDemandeurChanged();

        public virtual string NomEtQualiteDuRepresentant
        {
            get { return _nomEtQualiteDuRepresentant; }
            set
            {
                OnNomEtQualiteDuRepresentantChanging();
                _nomEtQualiteDuRepresentant = value;
                OnNomEtQualiteDuRepresentantChanged();
            }
        }
        partial void OnNomEtQualiteDuRepresentantChanging();
        partial void OnNomEtQualiteDuRepresentantChanged();

        public virtual string Adresse
        {
            get { return _adresse; }
            set
            {
                OnAdresseChanging();
                _adresse = value;
                OnAdresseChanged();
            }
        }
        partial void OnAdresseChanging();
        partial void OnAdresseChanged();

        public virtual string ResultatConformite
        {
            get { return _resultatConformite; }
            set
            {
                OnResultatConformiteChanging();
                _resultatConformite = value;
                OnResultatConformiteChanged();
            }
        }
        partial void OnResultatConformiteChanging();
        partial void OnResultatConformiteChanged();

        public virtual System.DateTime? DateLeveeReserve
        {
            get { return _dateLeveeReserve; }
            set
            {
                OnDateLeveeReserveChanging();
                _dateLeveeReserve = value;
                OnDateLeveeReserveChanged();
            }
        }
        partial void OnDateLeveeReserveChanging();
        partial void OnDateLeveeReserveChanged();

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

        public virtual string RefAttestationConformite
        {
            get { return _refAttestationConformite; }
            set
            {
                OnRefAttestationConformiteChanging();
                _refAttestationConformite = value;
                OnRefAttestationConformiteChanged();
            }
        }
        partial void OnRefAttestationConformiteChanging();
        partial void OnRefAttestationConformiteChanged();

        public virtual System.DateTime? DateAttestation
        {
            get { return _dateAttestation; }
            set
            {
                OnDateAttestationChanging();
                _dateAttestation = value;
                OnDateAttestationChanged();
            }
        }
        partial void OnDateAttestationChanging();
        partial void OnDateAttestationChanged();

        public virtual System.DateTime? DateFinReserve
        {
            get { return _dateFinReserve; }
            set
            {
                OnDateFinReserveChanging();
                _dateFinReserve = value;
                OnDateFinReserveChanged();
            }
        }
        partial void OnDateFinReserveChanging();
        partial void OnDateFinReserveChanged();

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
        partial void OnRReseauChanging();
        partial void OnRReseauChanged();

        public virtual pLogiciel PLogiciel
        {
            get { return _pLogiciel; }
            set
            {
                OnPLogicielChanging();
                _pLogiciel = value;
                OnPLogicielChanged();
            }
        }
        partial void OnPLogicielChanging();
        partial void OnPLogicielChanged();

        #endregion
    }
}
