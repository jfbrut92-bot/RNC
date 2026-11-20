using System;
using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class rReseau : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        private IList<pRapportException> _pRapportExceptions;

        partial void OnPRapportExceptionChanging();
        partial void OnPRapportExceptionChanged();

        public virtual IList<pRapportException> PRapportExceptions
        {
            get { return _pRapportExceptions; }
            set
            {
                OnPRapportExceptionChanging();
                _pRapportExceptions = value;
                OnPRapportExceptionChanged();
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

        /* EG le 12/04/2011 : prise en compte du controle technique cyclo CL */
        private bool _typeReseauCl = default(Boolean);

        partial void OnTypeReseauClChanging();
        partial void OnTypeReseauClChanged();

        /*PLE le 14/02/2024 CT CYCLO : ajout de la lettre réeau */
        private string _lettreReseau = string.Empty;
        public virtual string LettreReseau
        {
            get { return _lettreReseau; }
            set
            {
                OnLettreReseauChanging();
                _lettreReseau = value;
                OnLettreReseauChanged();
            }
        }
        partial void OnLettreReseauChanging();
        partial void OnLettreReseauChanged();


        public virtual bool TypeReseauCl
        {
            get { return _typeReseauCl; }
            set
            {
                OnTypeReseauClChanging();
                _typeReseauCl = value;
                OnTypeReseauClChanged();
            }
        }

        public virtual string ReseauAvecLettre
        {
            get
            {
                string reseauAvecLettre;

                if (this.Id != "-1")
                {
                    //PLE le 15/02/2024 Nouvelle colonne lettre réseau
                    //reseauAvecLettre = this.Nom + "(" + this.Id + ")";
                    reseauAvecLettre = this.Nom + "(" + this.LettreReseau + ")";
                }
                else
                {
                    reseauAvecLettre = this.Nom;
                }

                return reseauAvecLettre;

            }
        }
        //PLE le 16/02/2024 libellé réseau avec lettre + lib + catégorie
        public virtual string ReseauAvecLettreEtCategorie
        {
            get
            {
                string reseauAvecLettreEtCategorie = "";

                if (this.Id != "-1")
                {
                    //PLE le 15/02/2024 Nouvelle colonne lettre réseau
                    //reseauAvecLettre = this.Nom + "(" + this.Id + ")";
                    //reseauAvecLettre = "(" + this.LettreReseau + ")" + this.Nom;
                }
                else
                {
                    reseauAvecLettreEtCategorie = this.Nom;
                }

                if (this.TypeReseauVl)
                {
                    reseauAvecLettreEtCategorie = string.Format("({0}) {1} / {2}", this.LettreReseau, this.Nom, "VL");
                }
                if (this.TypeReseauPl)
                {
                    reseauAvecLettreEtCategorie = string.Format("({0}) {1} / {2}", this.LettreReseau, this.Nom, "PL");
                }
                if (this.TypeReseauCl)
                {
                    reseauAvecLettreEtCategorie = string.Format("({0}) {1} / {2}", this.LettreReseau, this.Nom, "CL");
                }

                return reseauAvecLettreEtCategorie;

            }
        }

        //PLE le 16/02/2024 libellé réseau avec lettre + catégorie
        public virtual string ReseauAvecLettreEtCategorieSeuls
        {
            get
            {
                string reseauAvecLettreEtCategorieSeuls = "";

                if (this.Id != "-1")
                {
                    //PLE le 15/02/2024 Nouvelle colonne lettre réseau
                    //reseauAvecLettre = this.Nom + "(" + this.Id + ")";
                    //reseauAvecLettre = "(" + this.LettreReseau + ")" + this.Nom;
                }
                else
                {
                    reseauAvecLettreEtCategorieSeuls = this.LettreReseau;
                }

                if (this.TypeReseauVl)
                {
                    reseauAvecLettreEtCategorieSeuls = string.Format("{0}({1})", this.LettreReseau, "VL");
                }
                if (this.TypeReseauPl)
                {
                    reseauAvecLettreEtCategorieSeuls = string.Format("{0}({1})", this.LettreReseau, "PL");
                }
                if (this.TypeReseauCl)
                {
                    reseauAvecLettreEtCategorieSeuls = string.Format("{0}({1})", this.LettreReseau, "CL");
                }

                return reseauAvecLettreEtCategorieSeuls;

            }
        }

        //YM le 10/11/2011 , Lettre des codes réseau
        public enum LettreCodeReseau
        {
            A, C, D, F, J, K, S, T, Z
        }
        //PLE le 14/02/2024, Id des codes réseaux
        public enum IdCodeReseau
        {
            A, C, D, F, J, K, S, T, Z, c, d, f, s, t
        }


    }
}
