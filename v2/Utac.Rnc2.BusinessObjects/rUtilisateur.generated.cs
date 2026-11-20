using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rUtilisateur : BusinessBase<string>
    {
        #region Declarations

        private string _motDePasse = String.Empty;
        private string _nom = null;
        private string _prenom = null;
        private bool _actif = default(Boolean);
        private bool _bloque = default(Boolean);
        private short? _numTentative = null;
        private System.DateTime? _dateDerniereConnection = null;
        private string _numTel = null;
        private string _email = null;
        private string _codUsrCrea = null;
        private string _codUsrMaj = null;
        private System.DateTime? _dateCrea = null;
        private System.DateTime? _dateMaj = null;
        private bool _facturePapier = default(Boolean);

        private rRegion _rRegion = null;
        private pCentreEncour _pCentreEncourPL = null;
        //EG le 12/04/2011 private rReseau _rReseau1 = null;
        private pCentreEncour _pCentreEncourVL = null;
        private rPrefecture _rPrefecture = null;
        //EG le 12/04/2011 private rReseau _rReseau2 = null;
        private rProfil2 _rProfil2 = null;
        private rProfil _rProfil = null;

        private IList<pAuditControleur> _pAuditControleurs = new List<pAuditControleur>();
        private IList<pCentreEncour> _pCentreEncours = new List<pCentreEncour>();
        private IList<pCentreHisto> _pCentreHistos = new List<pCentreHisto>();
        private IList<pContoleurEncour> _pContoleurEncours = new List<pContoleurEncour>();
        private IList<pControleurHisto> _pControleurHistos = new List<pControleurHisto>();
        private IList<pFormationControleurPL> _pFormationControleurPLs = new List<pFormationControleurPL>();
        private IList<pGestionCertificat> _pGestionCertificats = new List<pGestionCertificat>();
        private IList<pSanctionControleur> _pSanctionControleurs = new List<pSanctionControleur>();
        private IList<pSanctionCentre> _pSanctionCentres = new List<pSanctionCentre>();

        #endregion

        #region Constructors

        public rUtilisateur() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_motDePasse);
            sb.Append(_nom);
            sb.Append(_prenom);
            sb.Append(_actif);
            sb.Append(_bloque);
            sb.Append(_numTentative);
            sb.Append(_dateDerniereConnection);
            sb.Append(_numTel);
            sb.Append(_email);
            sb.Append(_codUsrCrea);
            sb.Append(_codUsrMaj);
            sb.Append(_dateCrea);
            sb.Append(_dateMaj);
            sb.Append(_facturePapier);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string MotDePasse
        {
            get { return _motDePasse; }
            set
            {
                OnMotDePasseChanging();
                _motDePasse = value;
                OnMotDePasseChanged();
            }
        }
        partial void OnMotDePasseChanging();
        partial void OnMotDePasseChanged();

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

        public virtual bool Actif
        {
            get { return _actif; }
            set
            {
                OnActifChanging();
                _actif = value;
                OnActifChanged();
            }
        }
        partial void OnActifChanging();
        partial void OnActifChanged();

        public virtual bool Bloque
        {
            get { return _bloque; }
            set
            {
                OnBloqueChanging();
                _bloque = value;
                OnBloqueChanged();
            }
        }
        partial void OnBloqueChanging();
        partial void OnBloqueChanged();

        public virtual short? NumTentative
        {
            get { return _numTentative; }
            set
            {
                OnNumTentativeChanging();
                _numTentative = value;
                OnNumTentativeChanged();
            }
        }
        partial void OnNumTentativeChanging();
        partial void OnNumTentativeChanged();

        public virtual System.DateTime? DateDerniereConnection
        {
            get { return _dateDerniereConnection; }
            set
            {
                OnDateDerniereConnectionChanging();
                _dateDerniereConnection = value;
                OnDateDerniereConnectionChanged();
            }
        }
        partial void OnDateDerniereConnectionChanging();
        partial void OnDateDerniereConnectionChanged();

        public virtual string NumTel
        {
            get { return _numTel; }
            set
            {
                OnNumTelChanging();
                _numTel = value;
                OnNumTelChanged();
            }
        }
        partial void OnNumTelChanging();
        partial void OnNumTelChanged();

        public virtual string Email
        {
            get { return _email; }
            set
            {
                OnEmailChanging();
                _email = value;
                OnEmailChanged();
            }
        }
        partial void OnEmailChanging();
        partial void OnEmailChanged();

        public virtual string CodUsrCrea
        {
            get { return _codUsrCrea; }
            set
            {
                OnCodUsrCreaChanging();
                _codUsrCrea = value;
                OnCodUsrCreaChanged();
            }
        }
        partial void OnCodUsrCreaChanging();
        partial void OnCodUsrCreaChanged();

        public virtual string CodUsrMaj
        {
            get { return _codUsrMaj; }
            set
            {
                OnCodUsrMajChanging();
                _codUsrMaj = value;
                OnCodUsrMajChanged();
            }
        }
        partial void OnCodUsrMajChanging();
        partial void OnCodUsrMajChanged();

        public virtual System.DateTime? DateCrea
        {
            get { return _dateCrea; }
            set
            {
                OnDateCreaChanging();
                _dateCrea = value;
                OnDateCreaChanged();
            }
        }
        partial void OnDateCreaChanging();
        partial void OnDateCreaChanged();

        public virtual System.DateTime? DateMaj
        {
            get { return _dateMaj; }
            set
            {
                OnDateMajChanging();
                _dateMaj = value;
                OnDateMajChanged();
            }
        }
        partial void OnDateMajChanging();
        partial void OnDateMajChanged();

        public virtual bool FacturePapier
        {
            get { return _facturePapier; }
            set
            {
                OnFacturePapierChanging();
                _facturePapier = value;
                OnFacturePapierChanged();
            }
        }
        partial void OnFacturePapierChanging();
        partial void OnFacturePapierChanged();

        public virtual rRegion RRegion
        {
            get { return _rRegion; }
            set
            {
                OnRRegionChanging();
                _rRegion = value;
                OnRRegionChanged();
            }
        }
        partial void OnRRegionChanging();
        partial void OnRRegionChanged();

        public virtual pCentreEncour PCentreEncourPL
        {
            get { return _pCentreEncourPL; }
            set
            {
                OnPCentreEncourPLChanging();
                _pCentreEncourPL = value;
                OnPCentreEncourPLChanged();
            }
        }
        partial void OnPCentreEncourPLChanging();
        partial void OnPCentreEncourPLChanged();

        /* EG le 12/04/2011
        public virtual rReseau RReseau1
        {
            get { return _rReseau1; }
			set
			{
				OnRReseau1Changing();
				_rReseau1 = value;
				OnRReseau1Changed();
			}
        }
		partial void OnRReseau1Changing();
		partial void OnRReseau1Changed();
		*/
        public virtual pCentreEncour PCentreEncourVL
        {
            get { return _pCentreEncourVL; }
            set
            {
                OnPCentreEncourVLChanging();
                _pCentreEncourVL = value;
                OnPCentreEncourVLChanged();
            }
        }
        partial void OnPCentreEncourVLChanging();
        partial void OnPCentreEncourVLChanged();

        public virtual rPrefecture RPrefecture
        {
            get { return _rPrefecture; }
            set
            {
                OnRPrefectureChanging();
                _rPrefecture = value;
                OnRPrefectureChanged();
            }
        }
        partial void OnRPrefectureChanging();
        partial void OnRPrefectureChanged();

        /* EG le 12/04/2011
		public virtual rReseau RReseau2
        {
            get { return _rReseau2; }
			set
			{
				OnRReseau2Changing();
				_rReseau2 = value;
				OnRReseau2Changed();
			}
        }
        */
        partial void OnRReseau2Changing();
        partial void OnRReseau2Changed();

        public virtual rProfil2 RProfil2
        {
            get { return _rProfil2; }
            set
            {
                OnRProfil2Changing();
                _rProfil2 = value;
                OnRProfil2Changed();
            }
        }
        partial void OnRProfil2Changing();
        partial void OnRProfil2Changed();

        public virtual rProfil RProfil
        {
            get { return _rProfil; }
            set
            {
                OnRProfilChanging();
                _rProfil = value;
                OnRProfilChanged();
            }
        }
        partial void OnRProfilChanging();
        partial void OnRProfilChanged();

        public virtual IList<pAuditControleur> PAuditControleurs
        {
            get { return _pAuditControleurs; }
            set
            {
                OnPAuditControleursChanging();
                _pAuditControleurs = value;
                OnPAuditControleursChanged();
            }
        }
        partial void OnPAuditControleursChanging();
        partial void OnPAuditControleursChanged();

        public virtual IList<pCentreEncour> PCentreEncours
        {
            get { return _pCentreEncours; }
            set
            {
                OnPCentreEncoursChanging();
                _pCentreEncours = value;
                OnPCentreEncoursChanged();
            }
        }
        partial void OnPCentreEncoursChanging();
        partial void OnPCentreEncoursChanged();

        public virtual IList<pCentreHisto> PCentreHistos
        {
            get { return _pCentreHistos; }
            set
            {
                OnPCentreHistosChanging();
                _pCentreHistos = value;
                OnPCentreHistosChanged();
            }
        }
        partial void OnPCentreHistosChanging();
        partial void OnPCentreHistosChanged();

        public virtual IList<pContoleurEncour> PContoleurEncours
        {
            get { return _pContoleurEncours; }
            set
            {
                OnPContoleurEncoursChanging();
                _pContoleurEncours = value;
                OnPContoleurEncoursChanged();
            }
        }
        partial void OnPContoleurEncoursChanging();
        partial void OnPContoleurEncoursChanged();

        public virtual IList<pControleurHisto> PControleurHistos
        {
            get { return _pControleurHistos; }
            set
            {
                OnPControleurHistosChanging();
                _pControleurHistos = value;
                OnPControleurHistosChanged();
            }
        }
        partial void OnPControleurHistosChanging();
        partial void OnPControleurHistosChanged();

        public virtual IList<pFormationControleurPL> PFormationControleurPLs
        {
            get { return _pFormationControleurPLs; }
            set
            {
                OnPFormationControleursChanging();
                _pFormationControleurPLs = value;
                OnPFormationControleursChanged();
            }
        }
        partial void OnPFormationControleursChanging();
        partial void OnPFormationControleursChanged();

        public virtual IList<pGestionCertificat> PGestionCertificats
        {
            get { return _pGestionCertificats; }
            set
            {
                OnPGestionCertificatsChanging();
                _pGestionCertificats = value;
                OnPGestionCertificatsChanged();
            }
        }
        partial void OnPGestionCertificatsChanging();
        partial void OnPGestionCertificatsChanged();

        public virtual IList<pSanctionControleur> PSanctionControleurs
        {
            get { return _pSanctionControleurs; }
            set
            {
                OnPSanctionControleursChanging();
                _pSanctionControleurs = value;
                OnPSanctionControleursChanged();
            }
        }
        partial void OnPSanctionControleursChanging();
        partial void OnPSanctionControleursChanged();

        public virtual IList<pSanctionCentre> PSanctionCentres
        {
            get { return _pSanctionCentres; }
            set
            {
                OnPSanctionCentresChanging();
                _pSanctionCentres = value;
                OnPSanctionCentresChanged();
            }
        }
        partial void OnPSanctionCentresChanging();
        partial void OnPSanctionCentresChanged();

        #endregion
    }
}
