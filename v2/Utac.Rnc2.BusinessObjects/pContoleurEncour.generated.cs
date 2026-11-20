using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pContoleurEncour : BusinessBase<int>
    {
        #region Declarations

        private string _nom = null;
        private string _prenom = null;
        private System.DateTime? _dateNaissance = null;
        private string _codAgremRattachementVl = null;
        private string _codAgremRattachementPl = null;
        private System.DateTime? _dateDebutRattachementVl = null;
        private System.DateTime? _dateDebutRattachementPl = null;
        private System.DateTime? _dateFinRattachementVl = null;
        private System.DateTime? _dateFinRattachementPl = null;
        private System.DateTime? _dateEffetNotificationVl = null;
        private System.DateTime? _dateEffetNotificationPl = null;
        private string _codAgremVl = null;
        private string _codAgremPl = null;
        private System.DateTime? _dateFinAgrementVl = null;
        private System.DateTime? _dateFinAgrementPl = null;
        private string _motifFinAgrementVl = null;
        private string _motifFinAgrementPl = null;
        private string _etatAgrementVl = String.Empty;
        private string _etatAgrementPl = String.Empty;
        private short? _avisOtcVl = null;
        private short? _avisOtcPl = null;
        private string _referenceAvisOtcVl = null;
        private string _referenceAvisOtcPl = null;
        private System.DateTime? _dateAvisOtcVl = null;
        private System.DateTime? _dateAvisOtcPl = null;
        private string _commentaireAvisOtcVl = null;
        private string _commentaireAvisOtcPl = null;
        private System.DateTime? _dateDebutHabilitationPl = null;
        private string _codDepartement = null;
        private string _numOrdrePl = null;
        private string _numOrdreVl = null;
        private System.DateTime _dateMiseaJour = new DateTime();

        private rHabilitation _rHabilitation = null;
        private rPrefecture _rPrefecture = null;
        private rTypeDemandeControleur _rTypeDemandeControleur = null;
        private rUtilisateur _rUtilisateur = null;
        private pCentreEncour _pCentreEncourPL = null;
        private pCentreEncour _pCentreEncourVL = null;

        private IList<pAmendeControleur> _pAmendeControleurs = new List<pAmendeControleur>();
        private IList<pCumulActivitePlControleur> _pCumulActivitePlControleurs = new List<pCumulActivitePlControleur>();
        private IList<pSanctionControleur> _pSanctionControleurs = new List<pSanctionControleur>();

        #endregion

        #region Constructors

        public pContoleurEncour() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_nom);
            sb.Append(_prenom);
            sb.Append(_dateNaissance);
            sb.Append(_codAgremRattachementVl);
            sb.Append(_codAgremRattachementPl);
            sb.Append(_dateDebutRattachementVl);
            sb.Append(_dateDebutRattachementPl);
            sb.Append(_dateFinRattachementVl);
            sb.Append(_dateFinRattachementPl);
            sb.Append(_dateEffetNotificationVl);
            sb.Append(_dateEffetNotificationPl);
            sb.Append(_codAgremVl);
            sb.Append(_codAgremPl);
            sb.Append(_dateFinAgrementVl);
            sb.Append(_dateFinAgrementPl);
            sb.Append(_motifFinAgrementVl);
            sb.Append(_motifFinAgrementPl);
            sb.Append(_etatAgrementVl);
            sb.Append(_etatAgrementPl);
            sb.Append(_avisOtcVl);
            sb.Append(_avisOtcPl);
            sb.Append(_referenceAvisOtcVl);
            sb.Append(_referenceAvisOtcPl);
            sb.Append(_dateAvisOtcVl);
            sb.Append(_dateAvisOtcPl);
            sb.Append(_commentaireAvisOtcVl);
            sb.Append(_commentaireAvisOtcPl);
            sb.Append(_dateDebutHabilitationPl);
            sb.Append(_codDepartement);
            sb.Append(_numOrdrePl);
            sb.Append(_numOrdreVl);
            sb.Append(_dateMiseaJour);
            //EG le 16/09/2011
            sb.Append(_habilitationGazVL);
            sb.Append(_dateDebutFinHabilitationGazVL);
            sb.Append(_codAgremRattachementCl);
            sb.Append(_dateDebutRattachementCl);
            sb.Append(_dateFinRattachementCl);
            sb.Append(_dateEffetNotificationCl);
            sb.Append(_codAgremCl);
            sb.Append(_etatAgrementCl);
            sb.Append(_numOrdreCl);
            //EG le 17/10/2011
            sb.Append(_estPrestataire);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

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

        [Range(typeof(DateTime), "1900-01-01", "2024-01-01", ErrorMessage = "La date de naissance est invalide")]
        public virtual System.DateTime? DateNaissance
        {
            get { return _dateNaissance; }
            set
            {
                OnDateNaissanceChanging();
                _dateNaissance = value;
                OnDateNaissanceChanged();
            }
        }
        partial void OnDateNaissanceChanging();
        partial void OnDateNaissanceChanged();

        public virtual string CodAgremRattachementVl
        {
            get { return _codAgremRattachementVl; }
            set
            {
                OnCodAgremRattachementVlChanging();
                _codAgremRattachementVl = value;
                OnCodAgremRattachementVlChanged();
            }
        }
        partial void OnCodAgremRattachementVlChanging();
        partial void OnCodAgremRattachementVlChanged();

        public virtual string CodAgremRattachementPl
        {
            get { return _codAgremRattachementPl; }
            set
            {
                OnCodAgremRattachementPlChanging();
                _codAgremRattachementPl = value;
                OnCodAgremRattachementPlChanged();
            }
        }
        partial void OnCodAgremRattachementPlChanging();
        partial void OnCodAgremRattachementPlChanged();

        public virtual System.DateTime? DateDebutRattachementVl
        {
            get { return _dateDebutRattachementVl; }
            set
            {
                OnDateDebutRattachementVlChanging();
                _dateDebutRattachementVl = value;
                OnDateDebutRattachementVlChanged();
            }
        }
        partial void OnDateDebutRattachementVlChanging();
        partial void OnDateDebutRattachementVlChanged();

        public virtual System.DateTime? DateDebutRattachementPl
        {
            get { return _dateDebutRattachementPl; }
            set
            {
                OnDateDebutRattachementPlChanging();
                _dateDebutRattachementPl = value;
                OnDateDebutRattachementPlChanged();
            }
        }
        partial void OnDateDebutRattachementPlChanging();
        partial void OnDateDebutRattachementPlChanged();

        public virtual System.DateTime? DateFinRattachementVl
        {
            get { return _dateFinRattachementVl; }
            set
            {
                OnDateFinRattachementVlChanging();
                _dateFinRattachementVl = value;
                OnDateFinRattachementVlChanged();
            }
        }
        partial void OnDateFinRattachementVlChanging();
        partial void OnDateFinRattachementVlChanged();

        public virtual System.DateTime? DateFinRattachementPl
        {
            get { return _dateFinRattachementPl; }
            set
            {
                OnDateFinRattachementPlChanging();
                _dateFinRattachementPl = value;
                OnDateFinRattachementPlChanged();
            }
        }
        partial void OnDateFinRattachementPlChanging();
        partial void OnDateFinRattachementPlChanged();

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

        public virtual System.DateTime? DateDebutHabilitationPl
        {
            get { return _dateDebutHabilitationPl; }
            set
            {
                OnDateDebutHabilitationPlChanging();
                _dateDebutHabilitationPl = value;
                OnDateDebutHabilitationPlChanged();
            }
        }
        partial void OnDateDebutHabilitationPlChanging();
        partial void OnDateDebutHabilitationPlChanged();

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

        public virtual rHabilitation RHabilitation
        {
            get { return _rHabilitation; }
            set
            {
                OnRHabilitationChanging();
                _rHabilitation = value;
                OnRHabilitationChanged();
            }
        }
        partial void OnRHabilitationChanging();
        partial void OnRHabilitationChanged();

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

        public virtual rTypeDemandeControleur RTypeDemandeControleur
        {
            get { return _rTypeDemandeControleur; }
            set
            {
                OnRTypeDemandeControleurChanging();
                _rTypeDemandeControleur = value;
                OnRTypeDemandeControleurChanged();
            }
        }
        partial void OnRTypeDemandeControleurChanging();
        partial void OnRTypeDemandeControleurChanged();

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

        public virtual IList<pAmendeControleur> PAmendeControleurs
        {
            get { return _pAmendeControleurs; }
            set
            {
                OnPAmendeControleursChanging();
                _pAmendeControleurs = value;
                OnPAmendeControleursChanged();
            }
        }
        partial void OnPAmendeControleursChanging();
        partial void OnPAmendeControleursChanged();

        public virtual IList<pCumulActivitePlControleur> PCumulActivitePlControleurs
        {
            get { return _pCumulActivitePlControleurs; }
            set
            {
                OnPCumulActivitePlControleursChanging();
                _pCumulActivitePlControleurs = value;
                OnPCumulActivitePlControleursChanged();
            }
        }
        partial void OnPCumulActivitePlControleursChanging();
        partial void OnPCumulActivitePlControleursChanged();

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

        //PLE le 19/01/2024 prise en compte de l'extension CL
        //ext_agrem_cl_statut
        private short? _extAgremClStatut = null;
        public virtual short? ExtAgremClStatut
        {
            get { return _extAgremClStatut; }
            set
            {
                OnExtAgremClStatutChanging();
                _extAgremClStatut = value;
                OnExtAgremClStatutChanged();
            }
        }
        partial void OnExtAgremClStatutChanging();
        partial void OnExtAgremClStatutChanged();

        //PLE le 19/01/2024 prise en compte de l'extension CL
        //ext_agrem_cl_date_valid
        private System.DateTime? _extAgremClDateValid = null;
        public virtual System.DateTime? ExtAgremClDateValid
        {
            get { return _extAgremClDateValid; }
            set
            {
                OnExtAgremClDateValidChanging();
                _extAgremClDateValid = value;
                OnExtAgremClDateValidChanged();
            }
        }
        partial void OnExtAgremClDateValidChanging();
        partial void OnExtAgremClDateValidChanged();

        //PLE le 19/01/2024 prise en compte de l'extension CL
        //ext_agrem_cl_date_decla
        private System.DateTime? _extAgremClDateDecla = null;
        public virtual System.DateTime? ExtAgremClDateDecla
        {
            get { return _extAgremClDateDecla; }
            set
            {
                OnExtAgremClDateDeclaChanging();
                _extAgremClDateDecla = value;
                OnExtAgremClDateDeclaChanged();
            }
        }
        partial void OnExtAgremClDateDeclaChanging();
        partial void OnExtAgremClDateDeclaChanged();

        //PLE le 19/01/2024 prise en compte de l'extension CL
        //ext_agrem_cl_cat_veh
        private short? _extAgremClCatVeh = null;
        public virtual short? ExtAgremClCatVeh
        {
            get { return _extAgremClCatVeh; }
            set
            {
                OnExtAgremClCatVehChanging();
                _extAgremClCatVeh = value;
                OnExtAgremClCatVehChanged();
            }
        }
        partial void OnExtAgremClCatVehChanging();
        partial void OnExtAgremClCatVehChanged();
        #endregion

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

    }
}
