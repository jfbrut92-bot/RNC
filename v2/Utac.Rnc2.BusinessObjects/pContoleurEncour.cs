using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pContoleurEncour : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        //EG pour avoir des libellés plutot que les codes
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
                switch (this.EtatAgrementPl)
                {
                    //25/03/2020 -PLE- modification libellé
                    //case "0": return this.CodAgremPl != null ? "annulé" : "sans agrément";
                    case "0":
                        if (this.CodAgremPl == null)
                        {
                            return "";
                        }
                        else if (this.MotifFinAgrementPl == Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementPl.RETRAIT_DEFINITIF_SUITE_SANCTION))
                        {
                            return "retiré";
                        }
                        else
                        {
                            return "annulé";
                        }
                    case "1":
                        //PLE le 22/01/2024 prise en compte catégorie L
                        String libExtAgrem = "";
                        if (((short?)1).Equals(this.ExtAgremClStatut) && ((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(this.ExtAgremClCatVeh))
                        { libExtAgrem = " avec extension agrément CL"; }
                        //PLE le 08/04/2025 prise en compte du SVA
                        String libSVA = "";
                        if (this.SvaPL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                        //EG le 05/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.PL))
                        {
                            return "suspendu" + libExtAgrem;
                        }
                        else
                        {
                            return "valide" + libExtAgrem + libSVA;
                        }
                    case "2": return "demande en cours";
                    //25/03/2020 -PLE- modification
                    //case "3": return "rejeté";
                    case "3": return "demande rejetée";
                    default: return "";
                }
            }
        }

        //25/03/2020 -PLE- affichage spécial pour la liste des contrôleurs de la page de recherche
        public virtual string EtatAgrementPlAsLibellePourListe
        {
            /*  ETAT agrement
            *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
               3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
           */
            get
            {
                //string libReturn = this.CodAgremPl;
                System.Text.StringBuilder libReturn = new System.Text.StringBuilder(this.CodAgremPl);

                switch (this.EtatAgrementPl)
                {
                    case "0":
                        if (this.CodAgremPl == null)
                        {
                            //libReturn.Append("");
                        }
                        else if (this.MotifFinAgrementPl == Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementPl.RETRAIT_DEFINITIF_SUITE_SANCTION))
                        {
                            libReturn.Append(" (<span class=\"annule\">retiré</span>)");
                        }
                        else
                        {
                            libReturn.Append(" (annulé)");
                        }
                        break;
                    case "1":
                        //EG le 05/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.PL))
                        {
                            libReturn.Append(" (<span class=\"annule\">suspendu</span>)");
                        }
                        else
                        {
                            libReturn.Append(" (<span class=\"valide\">valide</span>)");
                        }
                        break;
                    case "2": libReturn.Append("<span class=\"cours\">en cours</span>"); break;
                    case "3": libReturn.Append("(<span class=\"annule\">rejeté</span>)"); break;
                        //default: libReturn.Append(""); break;

                }
                return libReturn.ToString();
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
                switch (this.EtatAgrementVl)
                {
                    //25/03/2020 -PLE- modification libellé
                    //case "0": return this.CodAgremVl != null ? "annulé" : "sans agrément";
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
                    case "1":
                        //PLE le 22/01/2024 prise en compte catégorie L
                        String libExtAgrem = "";
                        if (((short?)1).Equals(this.ExtAgremClStatut) && ((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(this.ExtAgremClCatVeh))
                        { libExtAgrem = " avec extension agrément CL"; }
                        //PLE le 08/04/2025 prise en compte du SVA
                        String libSVA = "";
                        if (this.SvaVL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                        //EG le 05/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.VL))
                        {
                            return "suspendu" + libExtAgrem ;
                        }
                        else
                        {
                            return "valide" + libExtAgrem + libSVA;
                        }
                    case "2": return "demande en cours";
                    //25/03/2020 -PLE- modification libellé
                    //case "3": return "rejeté";
                    case "3": return "demande rejetée";
                    default: return "";

                }
            }
        }

        //27/11/2019 -PLE- affichage spécial pour la liste des contrôleurs de la page de recherche
        public virtual string EtatAgrementVlAsLibellePourListe
        {
            /*  ETAT agrement
            *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
               3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
           */
            get
            {
                //string libReturn = this.CodAgremVl;
                System.Text.StringBuilder libReturn = new System.Text.StringBuilder(this.CodAgremVl);

                switch (this.EtatAgrementVl)
                {
                    case "0":
                        if (this.CodAgremVl == null)
                        {
                            //libReturn += "";
                            //libReturn.Append("");
                        }
                        else if (this.MotifFinAgrementVl == Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementVl.RETRAIT_DEFINITIF_SUITE_SANCTION))
                        {
                            libReturn.Append(" (<span class=\"annule\">retiré</span>)");
                        }
                        else
                        {
                            libReturn.Append(" (annulé)");
                        }
                        break;
                    case "1":
                        //EG le 05/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.VL))
                        {
                            libReturn.Append(" (<span class=\"annule\">suspendu</span>)");
                        }
                        else
                        {
                            libReturn.Append(" (<span class=\"valide\">valide</span>)");
                        }
                        break;
                    case "2": libReturn.Append("<span class=\"cours\">en cours</span>"); break;
                    case "3": libReturn.Append("(<span class=\"annule\">rejeté</span>)"); break;
                        //default: libReturn.Append(""); break;

                }
                return libReturn.ToString();
            }
        }

        //01/03/2024 PLE Catégorie L
        public virtual string EtatAgrementClAsLibellePourListe
        {
            /*  ETAT agrement
            *  0 : AGREMENT_KO : le contrôleur n'a pas d'agrément valide. -->annulation de l'agrément précédent ou retrait définitif.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du contrôleur .
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
               3 : AGREMENT_REJETE : La demande d'agrément a été rejeté par la préfecture. 
           */
            get
            {
                //string libReturn = this.CodAgremVl;
                System.Text.StringBuilder libReturn = new System.Text.StringBuilder(this.CodAgremCl);

                switch (this.EtatAgrementCl)
                {
                    case "0":
                        if (this.CodAgremCl == null)
                        {
                            //libReturn += "";
                            //libReturn.Append("");
                        }
                        else if (this.MotifFinAgrementCl == Convert.ToString((int)pContoleurEncour.CodeMotifFinAgrementCl.RETRAIT_DEFINITIF_SUITE_SANCTION))
                        {
                            libReturn.Append(" (<span class=\"annule\">retiré</span>)");
                        }
                        else
                        {
                            libReturn.Append(" (annulé)");
                        }
                        break;
                    case "1":
                        //EG le 05/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.CL))
                        {
                            libReturn.Append(" (<span class=\"annule\">suspendu</span>)");
                        }
                        else
                        {
                            libReturn.Append(" (<span class=\"valide\">valide</span>)");
                        }
                        break;
                    case "2": libReturn.Append("<span class=\"cours\">en cours</span>"); break;
                    case "3": libReturn.Append("(<span class=\"annule\">rejeté</span>)"); break;
                        //default: libReturn.Append(""); break;

                }
                return libReturn.ToString();
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
                    case "1":
                        //PLE le 08/04/2025 prise en compte du SVA
                        String libSVA = "";
                        if (this.SvaCL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                        //EG le 05/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.CL))
                        {
                            return "suspension";
                        }
                        else
                        {
                            return "valide" + libSVA;
                        }
                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    default: return "";

                }
            }
        }

        //EG le 19/09/2011
        public virtual bool AunAgrementValide
        {
            get
            {
                return EstAgrementValideVL || EstAgrementValidePL || EstAgrementValideCL;
            }
        }

        //EG le 17/01/2012
        public virtual bool AunAgrementDemandeEnCours
        {
            get
            {
                return EstAgrementVLEnCours || EstAgrementPLEnCours || EstAgrementCLEnCours;
            }
        }

        //EG le 19/10/2011
        public virtual bool EstAgrementValideVL
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementVl) ? this.EtatAgrementVl : "-1") == (int)CodeEtatAgrementVl.AGREMENT_OK;

            }
        }

        public virtual bool EstAgrementValidePL
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementPl) ? this.EtatAgrementPl : "-1") == (int)CodeEtatAgrementPl.AGREMENT_OK;

            }
        }

        public virtual bool EstAgrementValideCL
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementCl) ? this.EtatAgrementCl : "-1") == (int)CodeEtatAgrementCl.AGREMENT_OK;

            }
        }

        //EG le 23/11/2011
        public virtual bool EstAgrementVLEnCours
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementVl) ? this.EtatAgrementVl : "-1") == (int)CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS;

            }
        }

        public virtual bool EstAgrementPLEnCours
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementPl) ? this.EtatAgrementPl : "-1") == (int)CodeEtatAgrementPl.AGREMENT_DEMANDE_EN_COURS;

            }
        }

        public virtual bool EstAgrementCLEnCours
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementCl) ? this.EtatAgrementCl : "-1") == (int)CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS;

            }
        }

        //EG le 24/11/2011 : agrement NonOK (Annulé + non initialisé)
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

        //EG le 24/11/2011 : agrement rejeté
        public virtual bool EstAgrementVLRejete
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementVl) ? this.EtatAgrementVl : "-1") == (int)CodeEtatAgrementVl.AGREMENT_REJETE;

            }
        }

        public virtual bool EstAgrementPLRejete
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementPl) ? this.EtatAgrementPl : "-1") == (int)CodeEtatAgrementPl.AGREMENT_REJETE;

            }
        }

        public virtual bool EstAgrementCLRejete
        {
            get
            {
                return Convert.ToInt16(!string.IsNullOrEmpty(this.EtatAgrementCl) ? this.EtatAgrementCl : "-1") == (int)CodeEtatAgrementCl.AGREMENT_REJETE;

            }
        }

        //EG le 17/01/2012
        public virtual bool EstAgrementAnnuleVL
        {

            get
            {
                if ("0".Equals(this.EtatAgrementVl) && this.CodAgremVl != null)
                    return true;
                else
                    return false;

            }
        }

        public virtual bool EstAgrementAnnulePL
        {

            get
            {
                if ("0".Equals(this.EtatAgrementPl) && this.CodAgremPl != null)
                    return true;
                else
                    return false;
            }

        }

        public virtual bool EstAgrementAnnuleCL
        {

            get
            {
                if ("0".Equals(this.EtatAgrementCl) && this.CodAgremCl != null)
                    return true;
                else
                    return false;

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

        //EG le 22/07/2011

        public enum CodeMotifFinAgrementVl
        {
            CESSATION_ACTIVITE = 1,
            RETRAIT_DEFINITIF_SUITE_SANCTION = 2

        }

        public enum CodeMotifFinAgrementPl
        {
            CESSATION_ACTIVITE = 1,
            RETRAIT_DEFINITIF_SUITE_SANCTION = 2

        }

        public enum CodeMotifFinAgrementCl
        {
            CESSATION_ACTIVITE = 1,
            RETRAIT_DEFINITIF_SUITE_SANCTION = 2

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

        private string _etatAgrementCl = String.Empty;
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

        //EG le 29/09/2011
        private IList<pAuditControleur> _pAuditControleurs;
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

        //EG le 03/10/2011
        private IList<pAuditControleurCL> _pAuditControleurCLs;
        public virtual IList<pAuditControleurCL> PAuditControleurCLs
        {
            get { return _pAuditControleurCLs; }
            set
            {
                OnPAuditControleurCLsChanging();
                _pAuditControleurCLs = value;
                OnPAuditControleurCLsChanged();
            }
        }
        partial void OnPAuditControleurCLsChanging();
        partial void OnPAuditControleurCLsChanged();

        private IList<pAuditControleurVL> _pAuditControleurVLs;
        public virtual IList<pAuditControleurVL> PAuditControleurVLs
        {
            get { return _pAuditControleurVLs; }
            set
            {
                OnPAuditControleurVLsChanging();
                _pAuditControleurVLs = value;
                OnPAuditControleurVLsChanged();
            }
        }
        partial void OnPAuditControleurVLsChanging();
        partial void OnPAuditControleurVLsChanged();

        //Formations controleur PL
        private IList<pFormationControleurPL> _pFormationControleurPLs;
        public virtual IList<pFormationControleurPL> PFormationControleurPLs
        {
            get { return _pFormationControleurPLs; }
            set
            {
                OnPFormationControleurPLsChanging();
                _pFormationControleurPLs = value;
                OnPFormationControleurPLsChanged();
            }
        }
        partial void OnPFormationControleurPLsChanging();
        partial void OnPFormationControleurPLsChanged();

        //Formations controleur VL
        private IList<pFormationControleurVL> _pFormationControleurVLs;
        public virtual IList<pFormationControleurVL> PFormationControleurVLs
        {
            get { return _pFormationControleurVLs; }
            set
            {
                OnPFormationControleurVLsChanging();
                _pFormationControleurVLs = value;
                OnPFormationControleurVLsChanged();
            }
        }
        partial void OnPFormationControleurVLsChanging();
        partial void OnPFormationControleurVLsChanged();

        //Formations controleur CL
        private IList<pFormationControleurCL> _pFormationControleurCLs;
        public virtual IList<pFormationControleurCL> PFormationControleurCLs
        {
            get { return _pFormationControleurCLs; }
            set
            {
                OnPFormationControleurCLsChanging();
                _pFormationControleurCLs = value;
                OnPFormationControleurCLsChanged();
            }
        }
        partial void OnPFormationControleurCLsChanging();
        partial void OnPFormationControleurCLsChanged();

        //EG le 05/10/2011 : prise en compte de l'historique des données du controleur technique
        private IList<pControleurHisto> _pControleurHistoriques;
        public virtual IList<pControleurHisto> PControleurHistoriques
        {
            get { return _pControleurHistoriques; }
            set
            {
                OnPControleurHistoriquesChanging();
                _pControleurHistoriques = value;
                OnPControleurHistoriquesChanged();
            }
        }
        partial void OnPControleurHistoriquesChanging();
        partial void OnPControleurHistoriquesChanged();

        //EG le 17/10/2011 : prise en compte des prestataires
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

        //Pour gérer les valeurs nulles
        public pContoleurEncour(pContoleurEncour p)
        {
            Dictionary<String, object> defaultValues = new Dictionary<string, object>();
            defaultValues.Add("System.Int16", 0);
            defaultValues.Add("String", "");

            Type t = this.GetType();
            System.Reflection.PropertyInfo[] props = t.GetProperties();
            foreach (System.Reflection.PropertyInfo pInfo in props)
            {
                var val = pInfo.GetValue(p, null);

                if (val == null)
                {
                    defaultValues.TryGetValue(pInfo.PropertyType.Name, out val);
                    if (val == null)
                    {
                        try
                        {
                            pInfo.SetValue(this, System.DateTime.MinValue, null);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                    else
                        pInfo.SetValue(this, val, null);
                }
                else
                    pInfo.SetValue(this, val, null);
            }

        }

        private T getDefaultValue<T>(Type t)
        {
            return default(T);
        }

        //EG le 14/11/2011
        public virtual string Titre
        {
            get
            {
                return this.EstPrestataire ? "prestataire" : "contrôleur";
            }
        }

        //EG le 19/12/2011
        private IList<pJournalActivite> _journaux = new List<pJournalActivite>();
        public virtual IList<pJournalActivite> Journaux
        {
            get { return _journaux; }
            set
            {
                OnJournauxChanging();
                _journaux = value;
                OnJournauxChanged();
            }
        }
        partial void OnJournauxChanging();
        partial void OnJournauxChanged();

        //EG le 05/01/2012 pour gérer l'affichage de la validité du centre sanctionné
        public virtual bool ExisteSanctionEncours(rCategorieVehicule.CategorieEnum categorie)
        {
            if (this.PSanctionControleurs == null) return false;
            if (this.PSanctionControleurs.Count == 0)
            {
                return false;
            }
            else
            {
                DateTime now = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);
                foreach (pSanctionControleur sanction in this.PSanctionControleurs)
                {
                    if (sanction.RTypeSanctionControleur.Id == (int)rTypeSanctionControleur.MotifSanctionEnum.SUSPENSION || sanction.RTypeSanctionControleur.Id == (int)rTypeSanctionControleur.MotifSanctionEnum.SUSP_CONSERVATOIRE)
                    {
                        if (sanction.DateDebut <= now)
                        {
                            if (sanction.DateFin == null || sanction.DateFin >= now)
                            {
                                switch (categorie)
                                {
                                    case rCategorieVehicule.CategorieEnum.VL:
                                        if (sanction.IsAgrementAffecteVL) return true;
                                        break;

                                    case rCategorieVehicule.CategorieEnum.PL:
                                        if (sanction.IsAgrementAffectePL) return true;
                                        break;

                                    case rCategorieVehicule.CategorieEnum.CL:
                                        if (sanction.IsAgrementAffecteCL) return true;
                                        break;

                                }
                            }
                        }
                    }
                }
                return false;
            }

        }

        //PLE le 13/03/2012
        public virtual string MotifFinAgrementVlAsLibelle
        {
            /*
            Motif de la fin d'agrement VL : 
            1 : Cessation d'activité (lors d'une annulation de l'agrément.)
            2 : Retrait définitif suite à sanction
            */
            get
            {
                switch (this.MotifFinAgrementVl)
                {
                    case "1": return "Cessation d'activité";
                    case "2": return "Retrait définitif suite à sanction";
                    default: return "";

                }
            }
        }
        //PLE le 13/03/2012
        public virtual string MotifFinAgrementPlAsLibelle
        {
            /*
            Motif de la fin d'agrement PL : 
            1 : Cessation d'activité (lors d'une annulation de l'agrément.)
            2 : Retrait définitif suite à sanction
            */
            get
            {
                switch (this.MotifFinAgrementPl)
                {
                    case "1": return "Cessation d'activité";
                    case "2": return "Retrait définitif suite à sanction";
                    default: return "";

                }
            }
        }
        //PLE le 13/03/2012
        public virtual string MotifFinAgrementClAsLibelle
        {
            /*
            Motif de la fin d'agrement CL : 
            1 : Cessation d'activité (lors d'une annulation de l'agrément.)
            2 : Retrait définitif suite à sanction
            */
            get
            {
                switch (this.MotifFinAgrementCl)
                {
                    case "1": return "Cessation d'activité";
                    case "2": return "Retrait définitif suite à sanction";
                    default: return "";

                }
            }
        }
        //PLE le 23/01/2024 Extension Catégorie L
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
