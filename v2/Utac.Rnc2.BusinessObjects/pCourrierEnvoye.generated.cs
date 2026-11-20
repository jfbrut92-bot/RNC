using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCourrierEnvoye : BusinessBase<int>
    {
        #region Declarations

        private System.DateTime _date = new DateTime();
        private string _emailDest = String.Empty;
        private string _codUtilisateurDest = String.Empty;
        private string _nomDest = String.Empty;
        private string _prenomDest = String.Empty;
        private string _codUtilisateur = String.Empty;
        private string _nom = String.Empty;
        private string _prenom = String.Empty;
        private int _idProfil2 = default(Int32);
        private string _resultat = null;

        #endregion

        #region Constructors

        public pCourrierEnvoye() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_date);
            sb.Append(_emailDest);
            sb.Append(_codUtilisateurDest);
            sb.Append(_nomDest);
            sb.Append(_prenomDest);
            sb.Append(_codUtilisateur);
            sb.Append(_nom);
            sb.Append(_prenom);
            sb.Append(_idProfil2);
            sb.Append(_resultat);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual System.DateTime Date
        {
            get { return _date; }
            set
            {
                OnDateChanging();
                _date = value;
                OnDateChanged();
            }
        }
        partial void OnDateChanging();
        partial void OnDateChanged();

        public virtual string EmailDest
        {
            get { return _emailDest; }
            set
            {
                OnEmailDestChanging();
                _emailDest = value;
                OnEmailDestChanged();
            }
        }
        partial void OnEmailDestChanging();
        partial void OnEmailDestChanged();

        public virtual string CodUtilisateurDest
        {
            get { return _codUtilisateurDest; }
            set
            {
                OnCodUtilisateurDestChanging();
                _codUtilisateurDest = value;
                OnCodUtilisateurDestChanged();
            }
        }
        partial void OnCodUtilisateurDestChanging();
        partial void OnCodUtilisateurDestChanged();

        public virtual string NomDest
        {
            get { return _nomDest; }
            set
            {
                OnNomDestChanging();
                _nomDest = value;
                OnNomDestChanged();
            }
        }
        partial void OnNomDestChanging();
        partial void OnNomDestChanged();

        public virtual string PrenomDest
        {
            get { return _prenomDest; }
            set
            {
                OnPrenomDestChanging();
                _prenomDest = value;
                OnPrenomDestChanged();
            }
        }
        partial void OnPrenomDestChanging();
        partial void OnPrenomDestChanged();

        public virtual string CodUtilisateur
        {
            get { return _codUtilisateur; }
            set
            {
                OnCodUtilisateurChanging();
                _codUtilisateur = value;
                OnCodUtilisateurChanged();
            }
        }
        partial void OnCodUtilisateurChanging();
        partial void OnCodUtilisateurChanged();

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

        public virtual int IdProfil2
        {
            get { return _idProfil2; }
            set
            {
                OnIdProfil2Changing();
                _idProfil2 = value;
                OnIdProfil2Changed();
            }
        }
        partial void OnIdProfil2Changing();
        partial void OnIdProfil2Changed();

        public virtual string Resultat
        {
            get { return _resultat; }
            set
            {
                OnResultatChanging();
                _resultat = value;
                OnResultatChanged();
            }
        }
        partial void OnResultatChanging();
        partial void OnResultatChanged();

        #endregion
    }
}
