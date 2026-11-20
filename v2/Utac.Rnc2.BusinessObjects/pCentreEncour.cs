using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCentreEncour : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }
        private IEnumerable<pRapportException> _pRapportExceptionPLs;

        partial void OnPRapportExceptionPlsChanging();
        partial void OnPRapportExceptionPlsChanged();

        public virtual IEnumerable<pRapportException> PRapportExceptionPLs
        {
            get { return _pRapportExceptionPLs; }
            set
            {
                OnPRapportExceptionPlsChanging();
                _pRapportExceptionPLs = value;
                OnPRapportExceptionPlsChanged();
            }
        }

        private IEnumerable<pRapportExceptionVl> _pRapportExceptionVls;

        partial void OnPRapportExceptionVLsChanging();
        partial void OnPRapportExceptionVLsChanged();

        public virtual IEnumerable<pRapportExceptionVl> PRapportExceptionVLs
        {
            get { return _pRapportExceptionVls; }
            set
            {
                OnPRapportExceptionVLsChanging();
                _pRapportExceptionVls = value;
                OnPRapportExceptionVLsChanged();
            }
        }

        private IEnumerable<pRapportExceptionCl> _pRapportExceptionCls;

        partial void OnPRapportExceptionCLsChanging();
        partial void OnPRapportExceptionCLsChanged();

        public virtual IEnumerable<pRapportExceptionCl> PRapportExceptionCLs
        {
            get { return _pRapportExceptionCls; }
            set
            {
                OnPRapportExceptionCLsChanging();
                _pRapportExceptionCls = value;
                OnPRapportExceptionCLsChanged();
            }
        }

        private IList<pFacture> _pFactures;

        partial void OnPFactureChanging();
        partial void OnPFactureChanged();

        public virtual IList<pFacture> PFactures
        {
            get { return _pFactures; }
            set
            {
                OnPFactureChanging();
                _pFactures = value;
                OnPFactureChanged();
            }
        }

        public virtual string AdresseComplete
        {
            get
            {
                System.Text.StringBuilder adresse = new System.Text.StringBuilder();
                //string adresse = this.L1Adresse != null ? this.L1Adresse + "\n" : "";
                //adresse += this.L2Adresse != null ? this.L2Adresse + "\n" : "";
                //adresse += this.CpCentre != null ? this.CpCentre + " " : "";
                //adresse += this.Ville != null ? this.Ville : "";
                adresse.Append(this.L1Adresse != null ? this.L1Adresse + "\n" : "");
                adresse.Append(this.L2Adresse != null ? this.L2Adresse + "\n" : "");
                adresse.Append(this.CpCentre != null ? this.CpCentre + " " : "");
                adresse.Append(this.Ville != null ? this.Ville : "");
                return adresse.ToString();
            }
        }

        /*EG le 18/03/2011
        private IEnumerable<pCertificat> _pCertificats;
        
        partial void OnPCertificatsChanging();
        partial void OnPCertificatsChanged();

        public virtual IEnumerable<pCertificat> PCertificats
        {
            get { return _pCertificats; }
            set
            {
                OnPCertificatsChanging();
                _pCertificats = value;
                OnPCertificatsChanged();
            }
        }
        */
        //13/10/2010 Collégial :
        private IList<pGestionCertificat> _pGestionCertificatVLs;

        partial void OnPGestionCertificatVLsChanging();
        partial void OnPGestionCertificatVLsChanged();

        public virtual IList<pGestionCertificat> PGestionCertificatVLs
        {
            get { return _pGestionCertificatVLs; }
            set
            {
                OnPGestionCertificatVLsChanging();
                _pGestionCertificatVLs = value;
                OnPGestionCertificatVLsChanged();
            }
        }

        private IList<pGestionCertificat> _pGestionCertificatPLs;

        partial void OnPGestionCertificatPLsChanging();
        partial void OnPGestionCertificatPLsChanged();

        public virtual IList<pGestionCertificat> PGestionCertificatPLs
        {
            get { return _pGestionCertificatPLs; }
            set
            {
                OnPGestionCertificatPLsChanging();
                _pGestionCertificatPLs = value;
                OnPGestionCertificatPLsChanged();
            }
        }

        private IList<pGestionCertificat> _pGestionCertificatCLs;

        partial void OnPGestionCertificatCLsChanging();
        partial void OnPGestionCertificatCLsChanged();

        public virtual IList<pGestionCertificat> PGestionCertificatCLs
        {
            get { return _pGestionCertificatCLs; }
            set
            {
                OnPGestionCertificatCLsChanging();
                _pGestionCertificatCLs = value;
                OnPGestionCertificatCLsChanged();
            }
        }

        //YM le 10/02/2012
        public virtual bool ExisteDerogationEncours(rCategorieVehicule.CategorieEnum categorie)
        {
            if (this.PDerogationCentres == null) return false;
            if (this.PDerogationCentres.Count == 0)
            {
                return false;
            }
            else
            {
                DateTime now = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);

                foreach (pDerogationCentre derogation in this.PDerogationCentres)
                {
                    if (derogation.DateDebut <= now)
                    {
                        if (derogation.DateFin == null || derogation.DateFin >= now)
                        {
                            /* EG le 28/02/2012
                            switch (categorie)
                            {
                                case rCategorieVehicule.CategorieEnum.VL:
                                    if (derogation.IsAgrementAffecteVL) return true;
                                    break;

                                case rCategorieVehicule.CategorieEnum.PL:
                                    if (derogation.IsAgrementAffectePL) return true;
                                    break;

                                case rCategorieVehicule.CategorieEnum.CL:
                                    if (derogation.IsAgrementAffecteCL) return true;
                                    break;

                            }
                             */
                            if (categorie.Equals(derogation.CategorieVehicule)) return true;
                        }
                    }
                }
                return false;
            }
        }

        //YM le 04/04/2011
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
                switch (this.EtatAgrementVl)
                {
                    case "0":
                        //PLE 13/03/2012 : mise à niveau VL par rapport au PL
                        if (this.CodAgremVl == null)
                            return "pas de demande initiale";
                        else if (this.MotifFinAgrementVl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementVL.RETRAIT_SANCTION))
                            return "retiré";
                        else
                            return "annulé";
                    case "1":
                        //PLE le 22/01/2024 prise en compte catégorie L
                        String libExtAgrem = "";
                        if (((short?)1).Equals(this.ExtAgremClStatut) && ((short?)rCategorieVehicule.ExtCatvehEnum.VL).Equals(this.ExtAgremClCatVeh))
                        { libExtAgrem = " avec extension agrément CL"; }
                        //PLE le 08/04/2025 prise en compte du SVA
                        String libSVA = "";
                        if (this.SvaVL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                        //EG le 02/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.VL))
                        {
                            return "suspendu" + libExtAgrem;
                        }
                        else
                        {
                            return "valide" + libExtAgrem + libSVA;
                        }

                    case "2": return "demande en cours";
                    case "3": return "demande rejetée";
                    default: return "";

                }
            }
        }

        //YM le 04/04/2011
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
                switch (this.EtatAgrementPl)
                {
                    case "0":
                        if (this.CodAgremPl == null)
                            return "pas de demande initiale";
                        else if (this.MotifFinAgrementPl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementPL.RETRAIT_SANCTION))
                            return "retiré";
                        else
                            return "annulé";
                    case "1":
                        //PLE le 22/01/2024 prise en compte catégorie L
                        String libExtAgrem = "";
                        if (((short?)1).Equals(this.ExtAgremClStatut) && ((short?)rCategorieVehicule.ExtCatvehEnum.PL).Equals(this.ExtAgremClCatVeh))
                        { libExtAgrem = " avec extension agrément CL"; }
                        //PLE le 08/04/2025 prise en compte du SVA
                        String libSVA = "";
                        if (this.SvaPL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                        //EG le 02/01/2012 prise en compte des sanctions
                        if (this.ExisteSanctionEncours(rCategorieVehicule.CategorieEnum.PL))
                        {
                            return "suspension" + libExtAgrem;
                        }
                        else
                        {
                            return "valide" + libExtAgrem + libSVA;
                        }
                    case "2": return "demande en cours";
                    case "3": return "demande rejeté";
                    case "4": return "renouvellement en cours";
                    case "5": return "renouvellement valide";
                    case "6": return "renouvellement rejeté";
                    default: return "";

                }
            }
        }
        //YM le 01/06/2011
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
                switch (this.EtatAgrementCl)
                {
                    case "0":
                        if (this.CodAgremCl == null)
                            return "pas de demande initiale";
                        else if (this.MotifFinAgrementCl == Convert.ToString((int)pCentreEncour.MotifAnnulerAgrementCL.RETRAIT_SANCTION))
                            return "retiré";
                        else
                            return "annulé";
                    case "1":
                        //PLE le 08/04/2025 prise en compte du SVA
                        String libSVA = "";
                        if (this.SvaCL) { libSVA = " (délivré par application de la règle Silence Vaut Acceptation)"; }
                        //EG le 02/01/2012 prise en compte des sanctions
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

        //YM le 10/08/2011
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
                    case "2": return "Chang. de titulaire de l'agrément";
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
                    case "2": return "Chang. de titulaire de l'agrément";
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
                    case "2": return "Chang. de titulaire de l'agrément";
                    case "3": return "Chang. de localisation ou de local";
                    case "4": return "Chang. de réseau de rattachement";
                    case "5": return "Retrait suite à sanction";
                    case "6": return "Non renouvellement";
                    default: return "";

                }
            }
        }

        //YM le 20/10/2011
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
                if (("0".Equals(this.EtatAgrementPl) && this.CodAgremPl != null) || (Convert.ToString((int)CodeEtatAgrementPl.AGREMENT_RENOUVELE_REJETE).Equals(this.EtatAgrementPl) && this.CodAgremPl != null))
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

        public virtual bool EstAgrementEnCoursVL
        {

            get
            {
                if ("2".Equals(this.EtatAgrementVl))
                    return true;
                else
                    return false;

            }
        }

        public virtual bool EstAgrementEnCoursPL
        {

            get
            {
                if ("2".Equals(this.EtatAgrementPl))
                    return true;
                else
                    return false;

            }
        }

        public virtual bool EstAgrementRejeteVL
        {
            get
            {
                return ("3".Equals(this.EtatAgrementVl));
            }
        }
        public virtual bool EstAgrementRejeteCL
        {
            get
            {
                return ("3".Equals(this.EtatAgrementCl));
            }
        }
        public virtual bool EstAgrementRejetePL
        {

            get
            {
                return ("3".Equals(this.EtatAgrementPl));

            }
        }

        public virtual bool EstAgrementRenouvellementRejetePL
        {

            get
            {
                if ("6".Equals(this.EtatAgrementPl))
                    return true;
                else
                    return false;

            }
        }

        public virtual bool EstAgrementEnCoursCL
        {

            get
            {
                if ("2".Equals(this.EtatAgrementCl))
                    return true;
                else
                    return false;

            }
        }

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
        /*
    YM 19/05/2011  : ajout des énumérations
        0 : AGREMENT_KO : le centre n'a pas d'agrément valide. L'agrément précédent est annulé.  La ppréfecture a effectué un retrait définitif de l'agrément.
        1 : AGREMENT_OK : La préfecture a notifié l'agrément du centre.
        2 : AGREMENT_DEMANDE_EN_COURS : L'agrément est en cours de validation par la prefecture.
        3 : AGREMENT_REJETE : La demande d'agrément est rejeté par la préfecture.
        4 : AGREMENT_RENOUVELEMENT_EN_COURS : L'agrément est en cours de renouvelement par la prefecture pour les installations auxiliaires.
        5 : AGREMENT_RENOUVELE_OK : La préfecture a notifié le renouvelement de l'agrément de l'installation auxiliaire.
        6 : AGREMENT_RENOUVELE_REJETE : La préfecture a rejeté le renouvelement de l'agrément de l'installation auxiliaire.
    */

        // TODO: créer 3 tables de réferences
        /*
         * YM 17/06/2011 ajout des énuméraions
            1:	Fermeture définitive	
            2:	Changement d’exploitant	
            3:	Changement de localisation ou de local	
            4:	Changement de réseau de rattachement	
            5:	Retrait suite à sanction

         */
        public enum MotifAnnulerAgrementVL
        {
            FERMETURE_DEFINITIVE = 1,
            CHANGEMENT_EXPLOITANT = 2,
            CHANGEMENT_LOCALISATION = 3,
            CHANGEMENT_RESEAU = 4,
            RETRAIT_SANCTION = 5

        }

        public enum MotifAnnulerAgrementPL
        {
            FERMETURE_DEFINITIVE = 1,
            CHANGEMENT_EXPLOITANT = 2,
            CHANGEMENT_LOCALISATION = 3,
            CHANGEMENT_RESEAU = 4,
            RETRAIT_SANCTION = 5,
            NON_RENOUVELLEMENT = 6

        }

        public enum MotifAnnulerAgrementCL
        {
            FERMETURE_DEFINITIVE = 1,
            CHANGEMENT_EXPLOITANT = 2,
            CHANGEMENT_LOCALISATION = 3,
            CHANGEMENT_RESEAU = 4,
            RETRAIT_SANCTION = 5

        }

        //YM le 10/02/2012
        private IList<pDerogationCentre> _pDerogationCentres;
        public virtual IList<pDerogationCentre> PDerogationCentres
        {
            get { return _pDerogationCentres; }
            set
            {
                OnPDerogationCentresChanging();
                _pDerogationCentres = value;
                OnPDerogationCentresChanged();
            }
        }
        partial void OnPDerogationCentresChanging();
        partial void OnPDerogationCentresChanged();

        //YM le 05/04/2011
        private IList<pSanctionCentre> _pSanctionCentres;
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

        //YM le 07/04/2011
        private IList<pCentreHisto> _pCentreHisto;
        public virtual IList<pCentreHisto> PCentreHisto
        {
            get { return _pCentreHisto; }
            set
            {
                OnPCentreHistoChanging();
                _pCentreHisto = value;
                OnPCentreHistoChanged();
            }
        }
        partial void OnPCentreHistoChanging();
        partial void OnPCentreHistoChanged();

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

        //EG le 19/05/2011 pour gérer la recherche d'un centre agrée valide
        public enum ResultatRechercheCentre
        {
            CENTRE_INEXISTANT, CENTRE_AGREE, CENTRE_NON_AGREE, UTILISATEUR_EXISTE_DEJA, CENTRE_EXISTANT_POUR_AUTRE_CATEGORIE
        }

        //EG le 10/08/2011 , type d'installation auxilaire
        public enum TypeInstallation
        {
            A, S
        }

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

        //EG le 29/11/2011
        private IList<pEquipementCentreVL> _equipementCentreVLs = new List<pEquipementCentreVL>();
        public virtual IList<pEquipementCentreVL> EquipementCentreVLs
        {

            get { return _equipementCentreVLs; }
            set
            {
                OnEquipementCentreVLsChanging();
                _equipementCentreVLs = value;
                OnEquipementCentreVLsChanged();
            }
        }
        partial void OnEquipementCentreVLsChanging();
        partial void OnEquipementCentreVLsChanged();

        //EG le 01/12/2011
        private IList<pEquipementCentrePL> _equipementCentrePLs = new List<pEquipementCentrePL>();
        public virtual IList<pEquipementCentrePL> EquipementCentrePLs
        {

            get { return _equipementCentrePLs; }
            set
            {
                OnEquipementCentrePLsChanging();
                _equipementCentrePLs = value;
                OnEquipementCentrePLsChanged();
            }
        }
        partial void OnEquipementCentrePLsChanging();
        partial void OnEquipementCentrePLsChanged();

        private IList<pEquipementCentreCL> _equipementCentreCLs = new List<pEquipementCentreCL>();
        public virtual IList<pEquipementCentreCL> EquipementCentreCLs
        {

            get { return _equipementCentreCLs; }
            set
            {
                OnEquipementCentreCLsChanging();
                _equipementCentreCLs = value;
                OnEquipementCentreCLsChanged();
            }
        }
        partial void OnEquipementCentreCLsChanging();
        partial void OnEquipementCentreCLsChanged();

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

        //EG le 06/12/2011
        public virtual bool AunAgrementValide
        {
            get
            {
                return EstAgrementValideVL || EstAgrementValidePL || EstAgrementValideCL;
            }
        }

        private IList<pContoleurEncour> _controleurVLs = new List<pContoleurEncour>();

        public virtual IList<pContoleurEncour> ControleurVLs
        {
            get { return _controleurVLs; }
            set
            {
                OnControleurVLsChanging();
                _controleurVLs = value;
                OnControleurVLsChanged();
            }
        }
        partial void OnControleurVLsChanging();
        partial void OnControleurVLsChanged();

        private IList<pContoleurEncour> _controleurPLs = new List<pContoleurEncour>();

        public virtual IList<pContoleurEncour> ControleurPLs
        {
            get { return _controleurPLs; }
            set
            {
                OnControleurPLsChanging();
                _controleurPLs = value;
                OnControleurPLsChanged();
            }
        }
        partial void OnControleurPLsChanging();
        partial void OnControleurPLsChanged();

        private IList<pContoleurEncour> _controleurCLs = new List<pContoleurEncour>();

        public virtual IList<pContoleurEncour> ControleurCLs
        {
            get { return _controleurCLs; }
            set
            {
                OnControleurCLsChanging();
                _controleurCLs = value;
                OnControleurCLsChanged();
            }
        }
        partial void OnControleurCLsChanging();
        partial void OnControleurCLsChanged();

        //EG le 13/12/2011 : prise de RDV
        private IList<pInfoRdv> _pInfoRdvs = new List<pInfoRdv>();

        public virtual IList<pInfoRdv> PInfoRdvs
        {
            get { return _pInfoRdvs; }
            set
            {
                OnPInfoRdvsChanging();
                _pInfoRdvs = value;
                OnPInfoRdvsChanged();
            }
        }
        partial void OnPInfoRdvsChanging();
        partial void OnPInfoRdvsChanged();

        //EG le 14/12/2011 : prise de RDV
        private IList<pHoraireOuverture> _pHoraireOuvertures = new List<pHoraireOuverture>();

        public virtual IList<pHoraireOuverture> PHoraireOuvertures
        {
            get { return _pHoraireOuvertures; }
            set
            {
                OnPHoraireOuverturesChanging();
                _pHoraireOuvertures = value;
                OnPHoraireOuverturesChanged();
            }
        }
        partial void OnPHoraireOuverturesChanging();
        partial void OnPHoraireOuverturesChanged();

        //EG le 15/12/2011 : pour le chainage dans les deux sens
        private pCentreEncour _pCentreEncourSuivant;
        public virtual pCentreEncour PCentreEncourSuivant
        {
            get { return _pCentreEncourSuivant; }
            set
            {
                OnPCentreEncourSuivantChanging();
                _pCentreEncourSuivant = value;
                OnPCentreEncourSuivantChanged();
            }
        }
        partial void OnPCentreEncourSuivantChanging();
        partial void OnPCentreEncourSuivantChanged();

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

        //EG le 02/01/2012 pour gérer l'affichage de la validité du centre sanctionné
        public virtual bool ExisteSanctionEncours(rCategorieVehicule.CategorieEnum categorie)
        {
            if (this.PSanctionCentres == null) return false;
            if (this.PSanctionCentres.Count == 0)
            {
                return false;
            }
            else
            {
                DateTime now = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day);

                foreach (pSanctionCentre sanction in this.PSanctionCentres)
                {
                    if (sanction.RTypeSanctionCentre.Id == (int)rTypeSanctionCentre.MotifSanctionEnum.SUSPENSION || sanction.RTypeSanctionCentre.Id == (int)rTypeSanctionCentre.MotifSanctionEnum.SUSP_CONSERVATOIRE)
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

        //PLE le 06/05/2015 Trouver l'existance au moins un contrôleur rattaché au centre et dont l'agrément est valide
        public virtual bool ExisteControleurValide(rCategorieVehicule.CategorieEnum categorie)
        {
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    if (this.ControleurVLs == null) return false;
                    if (this.ControleurVLs.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (pContoleurEncour c in this.ControleurVLs)
                        {
                            if (c.EstAgrementValideVL) return true;
                        }

                    };
                    break;

                case rCategorieVehicule.CategorieEnum.PL:
                    if (this.ControleurPLs == null) return false;
                    if (this.ControleurPLs.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (pContoleurEncour c in this.ControleurPLs)
                        {
                            if (c.EstAgrementValidePL) return true;
                        }

                    };
                    break;

                case rCategorieVehicule.CategorieEnum.CL:
                    if (this.ControleurCLs == null) return false;
                    if (this.ControleurCLs.Count == 0)
                    {
                        return false;
                    }
                    else
                    {
                        foreach (pContoleurEncour c in this.ControleurCLs)
                        {
                            if (c.EstAgrementValideCL) return true;
                        }

                    };
                    break;
            }
            return false;
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
