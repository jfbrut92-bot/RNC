using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCentreHisto : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public virtual string AdresseComplete
        {
            get
            {
                string adresse = this.L1Adresse != null ? this.L1Adresse + "\n" : "";
                adresse += this.L2Adresse != null ? this.L2Adresse + "\n" : "";
                adresse += this.CpCentre != null ? this.CpCentre + " " : "";
                adresse += this.Ville != null ? this.Ville : "";
                return adresse;
            }
        }

        public virtual string EtatAgrementVlAsLibelle
        {
            /*  ETAT agrement
               0 : AGREMENT_KO : Le centre n'a pas d'agrément valide. soit par annulation de l'agrément précedent, soit par retrait définitif de l'agrément.
               1 : AGREMENT_OK : La préfecture a notifié l'agrément du centre.
               2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la préfecture (notification en cours).
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
                        //YM 22/03/2012 : mise à niveau VL par rapport au PL
                        if (this.CodAgremVl == null)
                            return "pas de demande initiale";
                        else
                            return "annulé";
                    case "1": return "valide" + libExtAgrem + libSVA;
                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    default: return "";

                }
            }
        }

        public virtual string EtatAgrementPlAsLibelle
        {
            /*  ETAT agrement
                0 : AGREMENT_KO : le centre n'a pas d'agrément valide. L'agrément précédent est annulé. La ppréfecture a effectué un retrait définitif de l'agrément.
                1 : AGREMENT_OK : La préfecture a notifié l'agrément du centre.
                2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
                3 : AGREMENT_REJETE : La demande d'agrément est rejeté par la préfecture.
                4 : AGREMENT_RENOUVELEMENT_EN COURS : L'agrément est en cours de renouvelement par la prefecture pour les installations auxiliaires.
                5 : AGREMENT_RENOUVELE_OK : La préfecture a notifié le renouvelement de l'agrément de l'installation auxiliaire.
                6 : AGREMENT_RENOUVELE_REJETE : La préfecture a rejeté le renouvelement de l'agrément de l'installation auxiliaire. 
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
                            return "pas de demande initiale";
                        else
                            return "annulé";
                    case "1": return "valide" + libExtAgrem + libSVA;
                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    case "4": return "renouvellement en cours";
                    case "5": return "renouvellement valide";
                    case "6": return "renouvellement rejeté";
                    default: return "";

                }
            }
        }

        public virtual string EtatAgrementClAsLibelle
        {
            /*  ETAT agrement
                0 : AGREMENT_KO : le centre n'a pas d'agrément valide. L'agrément précédent est annulé. La ppréfecture a effectué un retrait définitif de l'agrément.
                1 : AGREMENT_OK : La préfecture a notifié l'agrément du centre.
                2 : AGREMENT_DEMANDE_EN COURS : L'agrément est en cours de validation par la prefecture.
                3 : AGREMENT_REJETE : La demande d'agrément est rejeté par la préfecture.
                4 : AGREMENT_RENOUVELEMENT_EN COURS : L'agrément est en cours de renouvelement par la prefecture pour les installations auxiliaires.
                5 : AGREMENT_RENOUVELE_OK : La préfecture a notifié le renouvelement de l'agrément de l'installation auxiliaire.
                6 : AGREMENT_RENOUVELE_REJETE : La préfecture a rejeté le renouvelement de l'agrément de l'installation auxiliaire. 
           */
            get
            {
                //PLE le 08/04/2025 prise en compte du SVA
                String libSVA = "";
                if (this.SvaCL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                switch (this.EtatAgrementCl)
                {
                    case "0":
                        //PLE le 01/09/2025 Correction anomalie
                        //if (this.CodAgremPl == null)
                        if (this.CodAgremCl == null)
                            return "pas de demande initiale";
                        else
                            return "annulé";
                    case "1": return "valide"+ libSVA;
                    case "2": return "demande en cours";
                    case "3": return "rejeté";
                    default: return "";

                }
            }
        }

        public virtual string MotifFinAgrementVlAsLibelle
        {
            /*  Motif fin agrement
                1 : Fermeture définitive.
                2 : Chang. d'exploitant.
                3 : Chang. de localisation ou de local.
                4 : Chang. de réseau de rattachement.
                5 : Retrait suite à sanction.
                6 : Non renouvellement. 
           */
            get
            {
                switch (this.MotifFinAgrementVl)
                {
                    case "1": return "Fermeture définitive";
                    case "2": return "Chang. d'exploitant";
                    case "3": return "Chang. de localisation ou de local";
                    case "4": return "Chang. de réseau de rattachement";
                    case "5": return "Retrait suite à sanction";
                    case "6": return "Non renouvellement";
                    default: return "";

                }
            }
        }

        public virtual string MotifFinAgrementPlAsLibelle
        {
            /*  Motif fin agrement
                1 : Fermeture définitive.
                2 : Chang. d'exploitant.
                3 : Chang. de localisation ou de local.
                4 : Chang. de réseau de rattachement.
                5 : Retrait suite à sanction.
                6 : Non renouvellement. 
           */
            get
            {
                switch (this.MotifFinAgrementPl)
                {
                    case "1": return "Fermeture définitive";
                    case "2": return "Chang. d'exploitant";
                    case "3": return "Chang. de localisation ou de local";
                    case "4": return "Chang. de réseau de rattachement";
                    case "5": return "Retrait suite à sanction";
                    case "6": return "Non renouvellement";
                    default: return "";

                }
            }
        }

        public virtual string MotifFinAgrementClAsLibelle
        {
            /*  Motif fin agrement
                1 : Fermeture définitive.
                2 : Chang. d'exploitant.
                3 : Chang. de localisation ou de local.
                4 : Chang. de réseau de rattachement.
                5 : Retrait suite à sanction.
                6 : Non renouvellement. 
           */
            get
            {
                switch (this.MotifFinAgrementCl)
                {
                    case "1": return "Fermeture définitive";
                    case "2": return "Chang. d'exploitant";
                    case "3": return "Chang. de localisation ou de local";
                    case "4": return "Chang. de réseau de rattachement";
                    case "5": return "Retrait suite à sanction";
                    case "6": return "Non renouvellement";
                    default: return "";

                }
            }
        }

        //YM le 01/02/2012
        public virtual bool EstAgrementValideVL
        {

            get
            {
                if ("1".Equals(this.EtatAgrementVl))
                    return true;
                else
                    return false;

            }
        }

        public virtual bool EstAgrementValidePL
        {

            get
            {
                if ("1".Equals(this.EtatAgrementPl) || Convert.ToString((int)CodeEtatAgrementPl.AGREMENT_RENOUVELE_OK).Equals(this.EtatAgrementPl))
                    return true;
                else
                    return false;

            }
        }

        public virtual bool EstAgrementValideCL
        {

            get
            {
                if ("1".Equals(this.EtatAgrementCl))
                    return true;
                else
                    return false;

            }
        }

        //YM le 01/02/2012
        public enum CodeEtatAgrementPl
        {
            AGREMENT_KO = 0,
            AGREMENT_OK = 1,
            AGREMENT_DEMANDE_EN_COURS = 2,
            AGREMENT_REJETE = 3,
            AGREMENT_RENOUVELEMENT_EN_COURS = 4,
            AGREMENT_RENOUVELE_OK = 5,
            AGREMENT_RENOUVELE_REJETE = 6

        }

        public enum CodeEtatAgrementVl
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

        //EG le 06/05/2011 prise en compte du CL
        //num_ordre_cl
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

        //cod_agrem_cl
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

        private string _ancienNumeroAgrementCl;
        public virtual string AncienNumeroAgrementCl
        {
            get { return _ancienNumeroAgrementCl; }
            set
            {
                OnAncienNumeroAgrementClChanging();
                _ancienNumeroAgrementCl = value;
                OnAncienNumeroAgrementClChanged();
            }
        }
        partial void OnAncienNumeroAgrementClChanging();
        partial void OnAncienNumeroAgrementClChanged();

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

        //EG le 11/08/2011 prise en compte des réseaux de rattachement VL,PL et CL
        private rReseau _reseauRattachementVL = null;
        public virtual rReseau ReseauRattachementVL
        {

            get { return _reseauRattachementVL; }
            set
            {
                OnReseauRattachementVLChanging();
                _reseauRattachementVL = value;
                OnReseauRattachementVLChanged();
            }
        }
        partial void OnReseauRattachementVLChanging();
        partial void OnReseauRattachementVLChanged();

        private rReseau _reseauRattachementPL = null;
        public virtual rReseau ReseauRattachementPL
        {

            get { return _reseauRattachementPL; }
            set
            {
                OnReseauRattachementPLChanging();
                _reseauRattachementPL = value;
                OnReseauRattachementPLChanged();
            }
        }
        partial void OnReseauRattachementPLChanging();
        partial void OnReseauRattachementPLChanged();

        private rReseau _reseauRattachementCL = null;
        public virtual rReseau ReseauRattachementCL
        {

            get { return _reseauRattachementCL; }
            set
            {
                OnReseauRattachementCLChanging();
                _reseauRattachementCL = value;
                OnReseauRattachementCLChanged();
            }
        }
        partial void OnReseauRattachementCLChanging();
        partial void OnReseauRattachementCLChanged();

        private string _numAccuseReceptionVL = null;
        public virtual string NumAccuseReceptionVL
        {

            get { return _numAccuseReceptionVL; }
            set
            {
                OnNumAccuseReceptionVLChanging();
                _numAccuseReceptionVL = value;
                OnNumAccuseReceptionVLChanged();
            }
        }
        partial void OnNumAccuseReceptionVLChanging();
        partial void OnNumAccuseReceptionVLChanged();

        private string _numAccuseReceptionPL = null;
        public virtual string NumAccuseReceptionPL
        {

            get { return _numAccuseReceptionPL; }
            set
            {
                OnNumAccuseReceptionPLChanging();
                _numAccuseReceptionPL = value;
                OnNumAccuseReceptionPLChanged();
            }
        }
        partial void OnNumAccuseReceptionPLChanging();
        partial void OnNumAccuseReceptionPLChanged();

        private string _numAccuseReceptionCL = null;
        public virtual string NumAccuseReceptionCL
        {

            get { return _numAccuseReceptionCL; }
            set
            {
                OnNumAccuseReceptionCLChanging();
                _numAccuseReceptionCL = value;
                OnNumAccuseReceptionCLChanged();
            }
        }
        partial void OnNumAccuseReceptionCLChanging();
        partial void OnNumAccuseReceptionCLChanged();

        //EG le 06/12/2011
        private bool _accesPublicVl = default(bool);

        public virtual bool AccesPublicVl
        {
            get { return _accesPublicVl; }
            set
            {
                OnAccesPublicVlChanging();
                _accesPublicVl = value;
                OnAccesPublicVlChanged();
            }
        }

        partial void OnAccesPublicVlChanging();
        partial void OnAccesPublicVlChanged();

        private bool _accesPublicPl = default(bool);

        public virtual bool AccesPublicPl
        {
            get { return _accesPublicPl; }
            set
            {
                OnAccesPublicPlChanging();
                _accesPublicPl = value;
                OnAccesPublicPlChanged();
            }
        }

        partial void OnAccesPublicPlChanging();
        partial void OnAccesPublicPlChanged();

        private bool _accesPublicCl = default(bool);

        public virtual bool AccesPublicCl
        {
            get { return _accesPublicCl; }
            set
            {
                OnAccesPublicClChanging();
                _accesPublicCl = value;
                OnAccesPublicClChanged();
            }
        }

        partial void OnAccesPublicClChanging();
        partial void OnAccesPublicClChanged();

        //PLE le 19/01/2024 prise en compte de l'extension CL
        //ext_agrem_cl_statut
        private short? _extAgremClStatut;
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
        private System.DateTime? _extAgremClDateValid;
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
        private System.DateTime? _extAgremClDateDecla;
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
        private short? _extAgremClCatVeh;
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
