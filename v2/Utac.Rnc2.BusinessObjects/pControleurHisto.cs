using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pControleurHisto : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        //YM 21/02/2012 pour avoir des libellés plutot que les codes
        public virtual string EtatAgrementPlAsLibelle
        {
            /*  ETAT agrement
            *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
               3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
           */
            get
            {
                //PLE le 22/01/2024 prise en compte catégorie L
                String libExtAgrem = "";
                if (((short?)1).Equals(this.ExtAgremClStatut) && ((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(this.ExtAgremClCatVeh))
                { libExtAgrem = " avec extension agrément CL"; }
                //PLE le 08/04/2025 prise en compte du SVA
                String libSVA = "";
                if (this.SvaPL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                switch (this.EtatAgrementPl)
                {
                    case "0":
                        if (this.CodAgremPl == null)
                        {
                            return "sans agrément";
                        }
                        else if (this.MotifFinAgrementPl == Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementPl.RETRAIT_DEFINITIF_SUITE_SANCTION))
                        {
                            return "retiré";
                        }
                        else
                        {
                            return "annulé";
                        }

                    case "1": return "valide" + libExtAgrem + libSVA;
                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    default: return "";

                }
            }
        }

        public virtual string EtatAgrementVlAsLibelle
        {
            /*  ETAT agrement
            *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
               3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
           */
            get
            {
                //PLE le 22/01/2024 prise en compte catégorie L
                String libExtAgrem = "";
                if (((short?)1).Equals(this.ExtAgremClStatut) && ((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(this.ExtAgremClCatVeh))
                { libExtAgrem = " avec extension agrément CL"; }
                //PLE le 08/04/2025 prise en compte du SVA
                String libSVA = "";
                if (this.SvaVL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                switch (this.EtatAgrementVl)
                {
                    case "0":
                        if (this.CodAgremVl == null)
                        {
                            return "sans agrément";
                        }
                        else if (this.MotifFinAgrementVl == Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementVl.RETRAIT_DEFINITIF_SUITE_SANCTION))
                        {
                            return "retiré";
                        }
                        else
                        {
                            return "annulé";
                        }

                    case "1": return "valide" + libExtAgrem + libSVA;
                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    default: return "";

                }
            }
        }

        public virtual string EtatAgrementClAsLibelle
        {
            /*  ETAT agrement
            *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
               3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
           */
            get
            {
                //PLE le 08/04/2025 prise en compte du SVA
                String libSVA = "";
                if (this.SvaCL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                switch (this.EtatAgrementCl)
                {
                    case "0":

                        if (this.CodAgremCl == null)
                        {
                            return "sans agrément";
                        }
                        else if (this.MotifFinAgrementCl == Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementCl.RETRAIT_DEFINITIF_SUITE_SANCTION))
                        {
                            return "retiré";
                        }
                        else
                        {
                            return "annulé";
                        }

                    case "1": return "valide" + libSVA;
                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    default: return "";

                }
            }
        }

        //YM le 21/02/2012 : agrement NonOK (Annulé + non initialisé)
        public virtual bool EstAgrementVLNonOK
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementVl) ? this.EtatAgrementVl : "-1") == (int)CodeEtatAgrementVl.AGREMENT_KO;

            }
        }

        public virtual bool EstAgrementPLNonOK
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementPl) ? this.EtatAgrementPl : "-1") == (int)CodeEtatAgrementPl.AGREMENT_KO;

            }
        }

        public virtual bool EstAgrementCLNonOK
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementCl) ? this.EtatAgrementCl : "-1") == (int)CodeEtatAgrementCl.AGREMENT_KO;

            }
        }

        public enum CodeEtatAgrementVl
        {
            AGREMENT_KO = 0,
            AGREMENT_OK = 1,
            AGREMENT_DEMANDE_EN_COURS = 2,
            AGREMENT_REJETE = 3,
        }

        public enum CodeEtatAgrementPl
        {
            AGREMENT_KO = 0,
            AGREMENT_OK = 1,
            AGREMENT_DEMANDE_EN_COURS = 2,
            AGREMENT_REJETE = 3,
        }

        public enum CodeEtatAgrementCl
        {
            AGREMENT_KO = 0,
            AGREMENT_OK = 1,
            AGREMENT_DEMANDE_EN_COURS = 2,
            AGREMENT_REJETE = 3,
        }

        //EG le 13/07/2011 : prise en compte du CL
        private pCentreEncour _pCentreEncourCL;

        public virtual pCentreEncour PCentreEncourCL
        {
            get { return _pCentreEncourCL; }
            set
            {
                OnPCentreEncourCLChanging();
                _pCentreEncourCL = value;
                OnPCentreEncourCLChanged();
            }
        }
        partial void OnPCentreEncourCLChanging();
        partial void OnPCentreEncourCLChanged();

        private string _codAgremRattachementCl;
        public virtual string CodAgremRattachementCl
        {
            get { return _codAgremRattachementCl; }
            set
            {
                OnCodAgremRattachementClChanging();
                _codAgremRattachementCl = value;
                OnCodAgremRattachementClChanged();
            }
        }
        partial void OnCodAgremRattachementClChanging();
        partial void OnCodAgremRattachementClChanged();

        private System.DateTime? _dateDebutRattachementCl;
        public virtual System.DateTime? DateDebutRattachementCl
        {
            get { return _dateDebutRattachementCl; }
            set
            {
                OnDateDebutRattachementClChanging();
                _dateDebutRattachementCl = value;
                OnDateDebutRattachementClChanged();
            }
        }
        partial void OnDateDebutRattachementClChanging();
        partial void OnDateDebutRattachementClChanged();

        private System.DateTime? _dateFinRattachementCl;
        public virtual System.DateTime? DateFinRattachementCl
        {
            get { return _dateFinRattachementCl; }
            set
            {
                OnDateFinRattachementClChanging();
                _dateFinRattachementCl = value;
                OnDateFinRattachementClChanged();
            }
        }
        partial void OnDateFinRattachementClChanging();
        partial void OnDateFinRattachementClChanged();

        private System.DateTime? _dateEffetNotificationCl;
        public virtual System.DateTime? DateEffetNotificationCl
        {
            get { return _dateEffetNotificationCl; }
            set
            {
                OnDateEffetNotificationClChanging();
                _dateEffetNotificationCl = value;
                OnDateEffetNotificationClChanged();
            }
        }
        partial void OnDateEffetNotificationClChanging();
        partial void OnDateEffetNotificationClChanged();

        private string _codAgremCl;
        public virtual string CodAgremCl
        {
            get { return _codAgremCl; }
            set
            {
                OnCodAgremClChanging();
                _codAgremCl = value;
                OnCodAgremClChanged();
            }
        }
        partial void OnCodAgremClChanging();
        partial void OnCodAgremClChanged();

        private System.DateTime? _dateFinAgrementCl;
        public virtual System.DateTime? DateFinAgrementCl
        {
            get { return _dateFinAgrementCl; }
            set
            {
                OnDateFinAgrementClChanging();
                _dateFinAgrementCl = value;
                OnDateFinAgrementClChanged();
            }
        }
        partial void OnDateFinAgrementClChanging();
        partial void OnDateFinAgrementClChanged();

        private string _motifFinAgrementCl;
        public virtual string MotifFinAgrementCl
        {
            get { return _motifFinAgrementCl; }
            set
            {
                OnMotifFinAgrementClChanging();
                _motifFinAgrementCl = value;
                OnMotifFinAgrementClChanged();
            }
        }
        partial void OnMotifFinAgrementClChanging();
        partial void OnMotifFinAgrementClChanged();

        private string _etatAgrementCl;
        public virtual string EtatAgrementCl
        {
            get { return _etatAgrementCl; }
            set
            {
                OnEtatAgrementClChanging();
                _etatAgrementCl = value;
                OnEtatAgrementClChanged();
            }
        }
        partial void OnEtatAgrementClChanging();
        partial void OnEtatAgrementClChanged();

        private string _commentaireAvisOtcCl;
        public virtual string CommentaireAvisOtcCl
        {
            get { return _commentaireAvisOtcCl; }
            set
            {
                OnCommentaireAvisOtcClChanging();
                _commentaireAvisOtcCl = value;
                OnCommentaireAvisOtcClChanged();
            }
        }
        partial void OnCommentaireAvisOtcClChanging();
        partial void OnCommentaireAvisOtcClChanged();

        private string _numOrdreCl;
        public virtual string NumOrdreCl
        {
            get { return _numOrdreCl; }
            set
            {
                OnNumOrdreClChanging();
                _numOrdreCl = value;
                OnNumOrdreClChanged();
            }
        }
        partial void OnNumOrdreClChanging();
        partial void OnNumOrdreClChanged();

        //EG le 16/09/2011
        private int? _habilitationGazVL = null;
        public virtual int? HabilitationGazVL
        {
            get { return _habilitationGazVL; }
            set
            {
                OnHabilitationGazVLChanging();
                _habilitationGazVL = value;
                OnHabilitationGazVLChanged();
            }
        }
        partial void OnHabilitationGazVLChanging();
        partial void OnHabilitationGazVLChanged();

        public virtual string HabilitationGazVLAsString
        {
            get
            {
                switch (this._habilitationGazVL)
                {
                    case 1: return "oui";
                    case 0: return "non";
                    default: return "non renseigné";
                }
            }
        }

        private DateTime? _dateDebutFinHabilitationGazVL = null;
        public virtual DateTime? DateDebutFinHabilitationGazVL
        {
            get { return _dateDebutFinHabilitationGazVL; }
            set
            {
                OnDateDebutFinHabilitationGazVLChanging();
                _dateDebutFinHabilitationGazVL = value;
                OnDateDebutFinHabilitationGazVLChanged();
            }
        }
        partial void OnDateDebutFinHabilitationGazVLChanging();
        partial void OnDateDebutFinHabilitationGazVLChanged();

        //YM le 21/02/2012 : prise en compte des prestataires
        private bool _estPrestataire = default(bool);
        public virtual bool EstPrestataire
        {
            get { return _estPrestataire; }
            set
            {
                OnEstPrestataireChanging();
                _estPrestataire = value;
                OnEstPrestataireChanged();
            }
        }
        partial void OnEstPrestataireChanging();
        partial void OnEstPrestataireChanged();

        public virtual string ExtAgremClStatutVLAsString
        {
            get
            {
                if (((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(this.ExtAgremClCatVeh))
                {
                    switch (this._extAgremClStatut)
                    {
                        case 1: return "Oui";
                        case 0: return "Non";
                        default: return "non renseigné";
                    }
                }
                else
                {
                    return "";
                }
            }
        }
        //PLE le 23/01/2024 Extension Catégorie L
        public virtual string ExtAgremClStatutPLAsString
        {
            get
            {
                if (((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(this.ExtAgremClCatVeh))
                {
                    switch (this._extAgremClStatut)
                    {
                        case 1: return "Oui";
                        case 0: return "Non";
                        default: return "non renseigné";
                    }
                }
                else
                {
                    return "";
                }
            }
        }

        public virtual DateTime? ExtAgremClDateDeclaVLAsDate
        {
            get
            {
                if (((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(this.ExtAgremClCatVeh))
                {
                    return this.ExtAgremClDateDecla;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual DateTime? ExtAgremClDateDeclaPLAsDate
        {
            get
            {
                if (((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(this.ExtAgremClCatVeh))
                {
                    return this.ExtAgremClDateDecla;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual DateTime? ExtAgremClDateValidVLAsDate
        {
            get
            {
                if (((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(this.ExtAgremClCatVeh))
                {
                    return this.ExtAgremClDateValid;
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual DateTime? ExtAgremClDateValidPLAsDate
        {
            get
            {
                if (((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(this.ExtAgremClCatVeh))
                {
                    return this.ExtAgremClDateValid;
                }
                else
                {
                    return null;
                }
            }
        }

    }
}
