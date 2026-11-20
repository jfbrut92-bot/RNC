using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pRapportExceptionCl : BusinessBase<int>
    {
        #region Declarations

        private string _codAgremCntr = String.Empty;
        private string _nomRapport = String.Empty;
        private System.DateTime _dateDebutTransmissionPv = new DateTime();
        private System.DateTime _dateFinTransmissionPv = new DateTime();
        private System.DateTime _dateGenerationRapport = new DateTime();
        private string _codUtilisateurTelechargement = null;
        private System.DateTime? _dateTelechargementRapport = null;
        private byte[] _imgFichier = null;

        #endregion

        #region Constructors

        public pRapportExceptionCl() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_codAgremCntr);
            sb.Append(_nomRapport);
            sb.Append(_dateDebutTransmissionPv);
            sb.Append(_dateFinTransmissionPv);
            sb.Append(_dateGenerationRapport);
            sb.Append(_codUtilisateurTelechargement);
            sb.Append(_dateTelechargementRapport);
            sb.Append(_imgFichier);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public virtual string CodAgremCntr
        {
            get { return _codAgremCntr; }
            set
            {
                OnCodAgremCntrChanging();
                _codAgremCntr = value;
                OnCodAgremCntrChanged();
            }
        }
        partial void OnCodAgremCntrChanging();
        partial void OnCodAgremCntrChanged();

        public virtual string NomRapport
        {
            get { return _nomRapport; }
            set
            {
                OnNomRapportChanging();
                _nomRapport = value;
                OnNomRapportChanged();
            }
        }
        partial void OnNomRapportChanging();
        partial void OnNomRapportChanged();

        public virtual System.DateTime DateDebutTransmissionPv
        {
            get { return _dateDebutTransmissionPv; }
            set
            {
                OnDateDebutTransmissionPvChanging();
                _dateDebutTransmissionPv = value;
                OnDateDebutTransmissionPvChanged();
            }
        }
        partial void OnDateDebutTransmissionPvChanging();
        partial void OnDateDebutTransmissionPvChanged();

        public virtual System.DateTime DateFinTransmissionPv
        {
            get { return _dateFinTransmissionPv; }
            set
            {
                OnDateFinTransmissionPvChanging();
                _dateFinTransmissionPv = value;
                OnDateFinTransmissionPvChanged();
            }
        }
        partial void OnDateFinTransmissionPvChanging();
        partial void OnDateFinTransmissionPvChanged();

        public virtual System.DateTime DateGenerationRapport
        {
            get { return _dateGenerationRapport; }
            set
            {
                OnDateGenerationRapportChanging();
                _dateGenerationRapport = value;
                OnDateGenerationRapportChanged();
            }
        }
        partial void OnDateGenerationRapportChanging();
        partial void OnDateGenerationRapportChanged();

        public virtual string CodUtilisateurTelechargement
        {
            get { return _codUtilisateurTelechargement; }
            set
            {
                OnCodUtilisateurTelechargementChanging();
                _codUtilisateurTelechargement = value;
                OnCodUtilisateurTelechargementChanged();
            }
        }
        partial void OnCodUtilisateurTelechargementChanging();
        partial void OnCodUtilisateurTelechargementChanged();

        public virtual System.DateTime? DateTelechargementRapport
        {
            get { return _dateTelechargementRapport; }
            set
            {
                OnDateTelechargementRapportChanging();
                _dateTelechargementRapport = value;
                OnDateTelechargementRapportChanged();
            }
        }
        partial void OnDateTelechargementRapportChanging();
        partial void OnDateTelechargementRapportChanged();

        public virtual byte[] ImgFichier
        {
            get { return _imgFichier; }
            set
            {
                OnImgFichierChanging();
                _imgFichier = value;
                OnImgFichierChanged();
            }
        }
        partial void OnImgFichierChanging();
        partial void OnImgFichierChanged();

        #endregion
    }
}
