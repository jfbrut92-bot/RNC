using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pTraceRechLieuVl : BusinessBase<int>
    {
        #region Declarations

        private System.DateTime _dateRecherche = new DateTime();
        private string _nom = String.Empty;
        private string _prenom = String.Empty;
        private string _immatVeh = String.Empty;
        private System.DateTime _dateCircu = new DateTime();
        private string _numSerie = String.Empty;
        private string _codAgremCentre = String.Empty;
        private byte _codResultat = new byte();

        #endregion

        #region Constructors

        public pTraceRechLieuVl() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_dateRecherche);
            sb.Append(_nom);
            sb.Append(_prenom);
            sb.Append(_immatVeh);
            sb.Append(_dateCircu);
            sb.Append(_numSerie);
            sb.Append(_codAgremCentre);
            sb.Append(_codResultat);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual System.DateTime DateRecherche
        {
            get { return _dateRecherche; }
            set
            {
                OnDateRechercheChanging();
                _dateRecherche = value;
                OnDateRechercheChanged();
            }
        }
        partial void OnDateRechercheChanging();
        partial void OnDateRechercheChanged();

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

        public virtual string ImmatVeh
        {
            get { return _immatVeh; }
            set
            {
                OnImmatVehChanging();
                _immatVeh = value;
                OnImmatVehChanged();
            }
        }
        partial void OnImmatVehChanging();
        partial void OnImmatVehChanged();

        public virtual System.DateTime DateCircu
        {
            get { return _dateCircu; }
            set
            {
                OnDateCircuChanging();
                _dateCircu = value;
                OnDateCircuChanged();
            }
        }
        partial void OnDateCircuChanging();
        partial void OnDateCircuChanged();

        public virtual string NumSerie
        {
            get { return _numSerie; }
            set
            {
                OnNumSerieChanging();
                _numSerie = value;
                OnNumSerieChanged();
            }
        }
        partial void OnNumSerieChanging();
        partial void OnNumSerieChanged();

        public virtual string CodAgremCentre
        {
            get { return _codAgremCentre; }
            set
            {
                OnCodAgremCentreChanging();
                _codAgremCentre = value;
                OnCodAgremCentreChanged();
            }
        }
        partial void OnCodAgremCentreChanging();
        partial void OnCodAgremCentreChanged();

        public virtual byte CodResultat
        {
            get { return _codResultat; }
            set
            {
                OnCodResultatChanging();
                _codResultat = value;
                OnCodResultatChanged();
            }
        }
        partial void OnCodResultatChanging();
        partial void OnCodResultatChanged();

        #endregion
    }
}
