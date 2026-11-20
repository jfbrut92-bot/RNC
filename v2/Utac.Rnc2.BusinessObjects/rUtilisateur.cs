using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rUtilisateur : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        //<!-- EG le 11/04/2011 pour gestion des logins temporaires -->

        private string _motDePasseTemporaire;
        public virtual string MotDePasseTemporaire
        {
            get
            {
                return this._motDePasseTemporaire;
            }

            set
            {
                OnMotDePasseChanging();
                this._motDePasseTemporaire = value;
                OnMotDePasseTemporaireChanged();
            }
        }

        partial void OnMotDePasseTemporaireChanging();
        partial void OnMotDePasseTemporaireChanged();

        //EG le 12/04/2011 pour prise en compte du CTCL
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

        //EG le 22/04/2011 pour prise en compte r_reseau_appartenance
        /* commenté le 01/06/2011 pour éviter collision avec RReseaux
        private IList<rReseauAppartenance> _rReseauAppartenances;

        public virtual IList<rReseauAppartenance> RReseauAppartenances
        {
            get { return _rReseauAppartenances; }
            set
            {
                OnRReseauAppartenancesChanging();
                _rReseauAppartenances = value;
                OnRReseauAppartenancesChanged();
            }
        }
        partial void OnRReseauAppartenancesChanging();
        partial void OnRReseauAppartenancesChanged();
        */

        //EG le 05/05/2011 pour "meilleur" prise en compte de r_reseau_appartenance
        //EG le 22/04/2011 pour prise en compte r_reseau_appartenance
        private IList<rReseau> _rReseaux;

        public virtual IList<rReseau> RReseaux
        {
            get { return _rReseaux; }
            set
            {
                OnRReseauxChanging();
                _rReseaux = value;
                OnRReseauxChanged();
            }
        }
        partial void OnRReseauxChanging();
        partial void OnRReseauxChanged();

        public virtual bool EstRattacheAUnCentreVL
        {
            get
            {
                bool estVl = false;
                if (this.RReseaux == null) return estVl;

                foreach (rReseau reseaux in this.RReseaux)
                {
                    if (reseaux.TypeReseauVl == true)
                    {
                        estVl = true;
                    }
                }
                return estVl;
            }
        }
        public virtual bool EstRattacheAUnCentrePL
        {
            get
            {
                bool estPl = false;
                if (this.RReseaux == null) return estPl;
                foreach (rReseau reseaux in this.RReseaux)
                {
                    if (reseaux.TypeReseauPl == true)
                    {
                        estPl = true;
                    }
                }
                return estPl;
            }
        }

        public virtual string BloqueAsString
        {
            get
            {
                return this.Bloque ? "oui" : "non";
            }
        }

        public virtual string ActifAsString
        {
            get
            {
                return this.Actif ? "oui" : "non";
            }
        }

        public virtual bool MonCentreEstVL
        {
            get
            {
                return this.PCentreEncourVL != null ? true : false;
            }
        }

        public virtual bool MonCentreEstPL
        {
            get
            {
                return this.PCentreEncourPL != null ? true : false;
            }
        }

        public virtual bool MonCentreEstCL
        {
            get
            {
                return this.PCentreEncourCL != null ? true : false;
            }
        }

        //Attention n'utiliser que pour les utilisateurs de profil CENTRE_INDEPENDANT
        public virtual pCentreEncour MonCentre
        {
            get
            {
                return this.PCentreEncourVL != null ? this.PCentreEncourVL : this.PCentreEncourPL != null ? this.PCentreEncourPL : this.PCentreEncourCL;
            }
        }

        public virtual string AgrementCentre
        {
            get
            {
                if (MonCentreEstVL)
                {
                    return this.MonCentre.CodAgremVl;
                }

                if (MonCentreEstPL)
                {
                    return this.MonCentre.CodAgremPl;
                }

                return this.MonCentre.CodAgremCl;

            }
        }

        public enum LogonType
        {
            TEMPORAIRE,
            PERMANENT
        }

        public enum Profil
        {
            OTC,
            OTC_Administration,
            OTC_Informatique,
            OTC_Consultation,
            PREFECTURE,
            DRIRE,
            CENTRE_INDEPENDANT,
            RESEAU,
            RESEAU_Gestionnaire,
            RESEAU_Consultation,
            RESEAU_Informatique,
            RESEAU_Administrateur,
            DSCR,
            PIV
        }

    }

}