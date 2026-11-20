using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCentreHisto : BusinessBase<string>
    {
        #region Declarations

        private System.DateTime _dateHisto = new DateTime();
        private int _idCentreHisto = default(Int32);
        private string _raisonSociale = null;
        private string _numSiren = null;
        private string _enseigneCommerciale = null;
        private string _l1Adresse = null;
        private string _l2Adresse = null;
        private string _cpCentre = null;
        private string _ville = null;
        private string _numTel = null;
        private string _nomRepresentantLegal = null;
        private string _typeCentre = String.Empty;
        private string _codDepartement = String.Empty;
        private string _numOrdrePl = null;
        private string _numOrdreVl = null;
        private string _codAgremPl = null;
        private string _codAgremVl = null;
        private string _ancienNumeroAgrementVl = null;
        private string _ancienNumeroAgrementPl = null;
        private System.DateTime? _dateEffetNotificationPl = null;
        private System.DateTime? _dateEffetNotificationVl = null;
        private System.DateTime? _dateFinAgrementPl = null;
        private System.DateTime? _dateEcheanceAgrementPl = null;
        private System.DateTime? _dateFinAgrementVl = null;
        private string _motifFinAgrementPl = null;
        private string _motifFinAgrementVl = null;
        private short? _avisOtcVl = null;
        private string _referenceAvisOtcVl = null;
        private System.DateTime? _dateAvisOtcVl = null;
        private string _commentaireAvisOtcVl = null;
        private string _etatAgrementVl = String.Empty;
        private string _etatAgrementPl = String.Empty;
        private short? _avisOtcPl = null;
        private string _referenceAvisOtcPl = null;
        private System.DateTime? _dateAvisOtcPl = null;
        private string _commentaireAvisOtcPl = null;
        private System.DateTime? _dateEffetModification = null;
        private string _numFax = null;

        private rMotifModification _rMotifModification = null;
        private rPrefecture _rPrefecture = null;
        private rReseau _rReseau = null;
        private rStatutJuridique _rStatutJuridique = null;
        private rUtilisateur _rUtilisateur = null;
        private rRegroupementIndependant _rRegroupementIndependant = null;

        #endregion

        #region Constructors

        public pCentreHisto() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dateHisto);
            sb.Append(_idCentreHisto);
            sb.Append(_raisonSociale);
            sb.Append(_numSiren);
            sb.Append(_enseigneCommerciale);
            sb.Append(_l1Adresse);
            sb.Append(_l2Adresse);
            sb.Append(_cpCentre);
            sb.Append(_ville);
            sb.Append(_numTel);
            sb.Append(_nomRepresentantLegal);
            sb.Append(_typeCentre);
            sb.Append(_codDepartement);
            sb.Append(_numOrdrePl);
            sb.Append(_numOrdreVl);
            sb.Append(_codAgremPl);
            sb.Append(_codAgremVl);
            sb.Append(_ancienNumeroAgrementVl);
            sb.Append(_ancienNumeroAgrementPl);
            sb.Append(_dateEffetNotificationPl);
            sb.Append(_dateEffetNotificationVl);
            sb.Append(_dateFinAgrementPl);
            sb.Append(_dateEcheanceAgrementPl);
            sb.Append(_dateFinAgrementVl);
            sb.Append(_motifFinAgrementPl);
            sb.Append(_motifFinAgrementVl);
            sb.Append(_avisOtcVl);
            sb.Append(_referenceAvisOtcVl);
            sb.Append(_dateAvisOtcVl);
            sb.Append(_commentaireAvisOtcVl);
            sb.Append(_etatAgrementVl);
            sb.Append(_etatAgrementPl);
            sb.Append(_avisOtcPl);
            sb.Append(_referenceAvisOtcPl);
            sb.Append(_dateAvisOtcPl);
            sb.Append(_commentaireAvisOtcPl);
            sb.Append(_dateEffetModification);
            sb.Append(_numFax);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public override string Id
        {
            get
            {
                System.Text.StringBuilder uniqueId = new System.Text.StringBuilder();
                uniqueId.Append(_dateHisto.ToString());
                uniqueId.Append("^");
                uniqueId.Append(_idCentreHisto.ToString());
                return uniqueId.ToString();
            }
        }

        public virtual System.DateTime DateHisto
        {
            get { return _dateHisto; }
            set
            {
                OnDateHistoChanging();
                _dateHisto = value;
                OnDateHistoChanged();
            }
        }
        partial void OnDateHistoChanging();
        partial void OnDateHistoChanged();

        public virtual int IdCentreHisto
        {
            get { return _idCentreHisto; }
            set
            {
                OnIdCentreHistoChanging();
                _idCentreHisto = value;
                OnIdCentreHistoChanged();
            }
        }
        partial void OnIdCentreHistoChanging();
        partial void OnIdCentreHistoChanged();

        public virtual string RaisonSociale
        {
            get { return _raisonSociale; }
            set
            {
                OnRaisonSocialeChanging();
                _raisonSociale = value;
                OnRaisonSocialeChanged();
            }
        }
        partial void OnRaisonSocialeChanging();
        partial void OnRaisonSocialeChanged();

        public virtual string NumSiren
        {
            get { return _numSiren; }
            set
            {
                OnNumSirenChanging();
                _numSiren = value;
                OnNumSirenChanged();
            }
        }
        partial void OnNumSirenChanging();
        partial void OnNumSirenChanged();

        public virtual string EnseigneCommerciale
        {
            get { return _enseigneCommerciale; }
            set
            {
                OnEnseigneCommercialeChanging();
                _enseigneCommerciale = value;
                OnEnseigneCommercialeChanged();
            }
        }
        partial void OnEnseigneCommercialeChanging();
        partial void OnEnseigneCommercialeChanged();

        public virtual string L1Adresse
        {
            get { return _l1Adresse; }
            set
            {
                OnL1AdresseChanging();
                _l1Adresse = value;
                OnL1AdresseChanged();
            }
        }
        partial void OnL1AdresseChanging();
        partial void OnL1AdresseChanged();

        public virtual string L2Adresse
        {
            get { return _l2Adresse; }
            set
            {
                OnL2AdresseChanging();
                _l2Adresse = value;
                OnL2AdresseChanged();
            }
        }
        partial void OnL2AdresseChanging();
        partial void OnL2AdresseChanged();

        public virtual string CpCentre
        {
            get { return _cpCentre; }
            set
            {
                OnCpCentreChanging();
                _cpCentre = value;
                OnCpCentreChanged();
            }
        }
        partial void OnCpCentreChanging();
        partial void OnCpCentreChanged();

        public virtual string Ville
        {
            get { return _ville; }
            set
            {
                OnVilleChanging();
                _ville = value;
                OnVilleChanged();
            }
        }
        partial void OnVilleChanging();
        partial void OnVilleChanged();

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

        public virtual string NomRepresentantLegal
        {
            get { return _nomRepresentantLegal; }
            set
            {
                OnNomRepresentantLegalChanging();
                _nomRepresentantLegal = value;
                OnNomRepresentantLegalChanged();
            }
        }
        partial void OnNomRepresentantLegalChanging();
        partial void OnNomRepresentantLegalChanged();

        public virtual string TypeCentre
        {
            get { return _typeCentre; }
            set
            {
                OnTypeCentreChanging();
                _typeCentre = value;
                OnTypeCentreChanged();
            }
        }
        partial void OnTypeCentreChanging();
        partial void OnTypeCentreChanged();

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

        public virtual string NumOrdrePl
        {
            get { return _numOrdrePl; }
            set
            {
                OnNumOrdrePlChanging();
                _numOrdrePl = value;
                OnNumOrdrePlChanged();
            }
        }
        partial void OnNumOrdrePlChanging();
        partial void OnNumOrdrePlChanged();

        public virtual string NumOrdreVl
        {
            get { return _numOrdreVl; }
            set
            {
                OnNumOrdreVlChanging();
                _numOrdreVl = value;
                OnNumOrdreVlChanged();
            }
        }
        partial void OnNumOrdreVlChanging();
        partial void OnNumOrdreVlChanged();

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

        public virtual string AncienNumeroAgrementVl
        {
            get { return _ancienNumeroAgrementVl; }
            set
            {
                OnAncienNumeroAgrementVlChanging();
                _ancienNumeroAgrementVl = value;
                OnAncienNumeroAgrementVlChanged();
            }
        }
        partial void OnAncienNumeroAgrementVlChanging();
        partial void OnAncienNumeroAgrementVlChanged();

        public virtual string AncienNumeroAgrementPl
        {
            get { return _ancienNumeroAgrementPl; }
            set
            {
                OnAncienNumeroAgrementPlChanging();
                _ancienNumeroAgrementPl = value;
                OnAncienNumeroAgrementPlChanged();
            }
        }
        partial void OnAncienNumeroAgrementPlChanging();
        partial void OnAncienNumeroAgrementPlChanged();

        public virtual System.DateTime? DateEffetNotificationPl
        {
            get { return _dateEffetNotificationPl; }
            set
            {
                OnDateEffetNotificationPlChanging();
                _dateEffetNotificationPl = value;
                OnDateEffetNotificationPlChanged();
            }
        }
        partial void OnDateEffetNotificationPlChanging();
        partial void OnDateEffetNotificationPlChanged();

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

        public virtual System.DateTime? DateFinAgrementPl
        {
            get { return _dateFinAgrementPl; }
            set
            {
                OnDateFinAgrementPlChanging();
                _dateFinAgrementPl = value;
                OnDateFinAgrementPlChanged();
            }
        }
        partial void OnDateFinAgrementPlChanging();
        partial void OnDateFinAgrementPlChanged();

        public virtual System.DateTime? DateEcheanceAgrementPl
        {
            get { return _dateEcheanceAgrementPl; }
            set
            {
                OnDateEcheanceAgrementPlChanging();
                _dateEcheanceAgrementPl = value;
                OnDateEcheanceAgrementPlChanged();
            }
        }
        partial void OnDateEcheanceAgrementPlChanging();
        partial void OnDateEcheanceAgrementPlChanged();

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

        public virtual string MotifFinAgrementPl
        {
            get { return _motifFinAgrementPl; }
            set
            {
                OnMotifFinAgrementPlChanging();
                _motifFinAgrementPl = value;
                OnMotifFinAgrementPlChanged();
            }
        }
        partial void OnMotifFinAgrementPlChanging();
        partial void OnMotifFinAgrementPlChanged();

        public virtual string MotifFinAgrementVl
        {
            get { return _motifFinAgrementVl; }
            set
            {
                OnMotifFinAgrementVlChanging();
                _motifFinAgrementVl = value;
                OnMotifFinAgrementVlChanged();
            }
        }
        partial void OnMotifFinAgrementVlChanging();
        partial void OnMotifFinAgrementVlChanged();

        public virtual short? AvisOtcVl
        {
            get { return _avisOtcVl; }
            set
            {
                OnAvisOtcVlChanging();
                _avisOtcVl = value;
                OnAvisOtcVlChanged();
            }
        }
        partial void OnAvisOtcVlChanging();
        partial void OnAvisOtcVlChanged();

        public virtual string ReferenceAvisOtcVl
        {
            get { return _referenceAvisOtcVl; }
            set
            {
                OnReferenceAvisOtcVlChanging();
                _referenceAvisOtcVl = value;
                OnReferenceAvisOtcVlChanged();
            }
        }
        partial void OnReferenceAvisOtcVlChanging();
        partial void OnReferenceAvisOtcVlChanged();

        public virtual System.DateTime? DateAvisOtcVl
        {
            get { return _dateAvisOtcVl; }
            set
            {
                OnDateAvisOtcVlChanging();
                _dateAvisOtcVl = value;
                OnDateAvisOtcVlChanged();
            }
        }
        partial void OnDateAvisOtcVlChanging();
        partial void OnDateAvisOtcVlChanged();

        public virtual string CommentaireAvisOtcVl
        {
            get { return _commentaireAvisOtcVl; }
            set
            {
                OnCommentaireAvisOtcVlChanging();
                _commentaireAvisOtcVl = value;
                OnCommentaireAvisOtcVlChanged();
            }
        }
        partial void OnCommentaireAvisOtcVlChanging();
        partial void OnCommentaireAvisOtcVlChanged();

        public virtual string EtatAgrementVl
        {
            get { return _etatAgrementVl; }
            set
            {
                OnEtatAgrementVlChanging();
                _etatAgrementVl = value;
                OnEtatAgrementVlChanged();
            }
        }
        partial void OnEtatAgrementVlChanging();
        partial void OnEtatAgrementVlChanged();

        public virtual string EtatAgrementPl
        {
            get { return _etatAgrementPl; }
            set
            {
                OnEtatAgrementPlChanging();
                _etatAgrementPl = value;
                OnEtatAgrementPlChanged();
            }
        }
        partial void OnEtatAgrementPlChanging();
        partial void OnEtatAgrementPlChanged();

        public virtual short? AvisOtcPl
        {
            get { return _avisOtcPl; }
            set
            {
                OnAvisOtcPlChanging();
                _avisOtcPl = value;
                OnAvisOtcPlChanged();
            }
        }
        partial void OnAvisOtcPlChanging();
        partial void OnAvisOtcPlChanged();

        public virtual string ReferenceAvisOtcPl
        {
            get { return _referenceAvisOtcPl; }
            set
            {
                OnReferenceAvisOtcPlChanging();
                _referenceAvisOtcPl = value;
                OnReferenceAvisOtcPlChanged();
            }
        }
        partial void OnReferenceAvisOtcPlChanging();
        partial void OnReferenceAvisOtcPlChanged();

        public virtual System.DateTime? DateAvisOtcPl
        {
            get { return _dateAvisOtcPl; }
            set
            {
                OnDateAvisOtcPlChanging();
                _dateAvisOtcPl = value;
                OnDateAvisOtcPlChanged();
            }
        }
        partial void OnDateAvisOtcPlChanging();
        partial void OnDateAvisOtcPlChanged();

        public virtual string CommentaireAvisOtcPl
        {
            get { return _commentaireAvisOtcPl; }
            set
            {
                OnCommentaireAvisOtcPlChanging();
                _commentaireAvisOtcPl = value;
                OnCommentaireAvisOtcPlChanged();
            }
        }
        partial void OnCommentaireAvisOtcPlChanging();
        partial void OnCommentaireAvisOtcPlChanged();

        public virtual System.DateTime? DateEffetModification
        {
            get { return _dateEffetModification; }
            set
            {
                OnDateEffetModificationChanging();
                _dateEffetModification = value;
                OnDateEffetModificationChanged();
            }
        }
        partial void OnDateEffetModificationChanging();
        partial void OnDateEffetModificationChanged();

        public virtual string NumFax
        {
            get { return _numFax; }
            set
            {
                OnNumFaxChanging();
                _numFax = value;
                OnNumFaxChanged();
            }
        }
        partial void OnNumFaxChanging();
        partial void OnNumFaxChanged();

        public virtual rMotifModification RMotifModification
        {
            get { return _rMotifModification; }
            set
            {
                OnRMotifModificationChanging();
                _rMotifModification = value;
                OnRMotifModificationChanged();
            }
        }
        partial void OnRMotifModificationChanging();
        partial void OnRMotifModificationChanged();

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

        public virtual rStatutJuridique RStatutJuridique
        {
            get { return _rStatutJuridique; }
            set
            {
                OnRStatutJuridiqueChanging();
                _rStatutJuridique = value;
                OnRStatutJuridiqueChanged();
            }
        }
        partial void OnRStatutJuridiqueChanging();
        partial void OnRStatutJuridiqueChanged();

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

        public virtual rRegroupementIndependant RRegroupementIndependant
        {
            get { return _rRegroupementIndependant; }
            set
            {
                OnRRegroupementIndependantChanging();
                _rRegroupementIndependant = value;
                OnRRegroupementIndependantChanged();
            }
        }
        partial void OnRRegroupementIndependantChanging();
        partial void OnRRegroupementIndependantChanged();

        //08/04/2025 -PLE- Ajout SAV
        private bool _svaVL = default(bool);
        public virtual bool SvaVL
        {
            get { return _svaVL; }
            set
            {
                OnAccesSvaVLChanging();
                _svaVL = value;
                OnAccesSvaVLChanged();
            }
        }

        partial void OnAccesSvaVLChanging();
        partial void OnAccesSvaVLChanged();
        //08/04/2025 -PLE- Ajout SAV
        private bool _svaPL = default(bool);
        public virtual bool SvaPL
        {
            get { return _svaPL; }
            set
            {
                OnAccesSvaPLChanging();
                _svaPL = value;
                OnAccesSvaPLChanged();
            }
        }

        partial void OnAccesSvaPLChanging();
        partial void OnAccesSvaPLChanged();

        //08/04/2025 -PLE- Ajout SAV
        private bool _svaCL = default(bool);
        public virtual bool SvaCL
        {
            get { return _svaCL; }
            set
            {
                OnAccesSvaCLChanging();
                _svaCL = value;
                OnAccesSvaCLChanged();
            }
        }

        partial void OnAccesSvaCLChanging();
        partial void OnAccesSvaCLChanged();

        #endregion
    }
}
