using System;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCertificat : BusinessBase<string>
    {
        #region Declarations

        private int _idReq = default(Int32);
        private string _numSerie = String.Empty;
        private System.DateTime _dateEffet = new DateTime();
        private System.DateTime _dateExpiration = new DateTime();
        private string _nomCert = String.Empty;
        private string _nomCommunCert = String.Empty;
        private string _passwordCert = null;
        private string _bddTechLogin = String.Empty;
        private string _bddTechPassword = String.Empty;
        private short? _codRaisonRevocation = null;
        private string _libRaisonRevocation = null;
        private System.DateTime? _dateRevocation = null;
        private string _codCategorieVeh = String.Empty;
        private string _nomFichier = String.Empty;
        private byte[] _imgFichier = null;

        #endregion

        #region Constructors

        public pCertificat() { }

        #endregion

        #region Methods

        public override int GetHashCode()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.Append(this.GetType().FullName);
            sb.Append(_idReq);
            sb.Append(_numSerie);
            sb.Append(_dateEffet);
            sb.Append(_dateExpiration);
            sb.Append(_nomCert);
            sb.Append(_nomCommunCert);
            sb.Append(_passwordCert);
            sb.Append(_bddTechLogin);
            sb.Append(_bddTechPassword);
            sb.Append(_codRaisonRevocation);
            sb.Append(_libRaisonRevocation);
            sb.Append(_dateRevocation);
            sb.Append(_codCategorieVeh);
            sb.Append(_nomFichier);
            sb.Append(_imgFichier);

            return sb.ToString().GetHashCode();
        }

        #endregion

        #region Properties

        public override string Id
        {
            get
            {
                System.Text.StringBuilder uniqueId = new System.Text.StringBuilder();
                uniqueId.Append(_idReq.ToString());
                uniqueId.Append("^");
                uniqueId.Append(_numSerie.ToString());
                return uniqueId.ToString();
            }
        }

        public virtual int IdReq
        {
            get { return _idReq; }
            set
            {
                OnIdReqChanging();
                _idReq = value;
                OnIdReqChanged();
            }
        }
        partial void OnIdReqChanging();
        partial void OnIdReqChanged();

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

        public virtual System.DateTime DateEffet
        {
            get { return _dateEffet; }
            set
            {
                OnDateEffetChanging();
                _dateEffet = value;
                OnDateEffetChanged();
            }
        }
        partial void OnDateEffetChanging();
        partial void OnDateEffetChanged();

        public virtual System.DateTime DateExpiration
        {
            get { return _dateExpiration; }
            set
            {
                OnDateExpirationChanging();
                _dateExpiration = value;
                OnDateExpirationChanged();
            }
        }
        partial void OnDateExpirationChanging();
        partial void OnDateExpirationChanged();

        public virtual string NomCert
        {
            get { return _nomCert; }
            set
            {
                OnNomCertChanging();
                _nomCert = value;
                OnNomCertChanged();
            }
        }
        partial void OnNomCertChanging();
        partial void OnNomCertChanged();

        public virtual string NomCommunCert
        {
            get { return _nomCommunCert; }
            set
            {
                OnNomCommunCertChanging();
                _nomCommunCert = value;
                OnNomCommunCertChanged();
            }
        }
        partial void OnNomCommunCertChanging();
        partial void OnNomCommunCertChanged();

        public virtual string PasswordCert
        {
            get { return _passwordCert; }
            set
            {
                OnPasswordCertChanging();
                _passwordCert = value;
                OnPasswordCertChanged();
            }
        }
        partial void OnPasswordCertChanging();
        partial void OnPasswordCertChanged();

        public virtual string BddTechLogin
        {
            get { return _bddTechLogin; }
            set
            {
                OnBddTechLoginChanging();
                _bddTechLogin = value;
                OnBddTechLoginChanged();
            }
        }
        partial void OnBddTechLoginChanging();
        partial void OnBddTechLoginChanged();

        public virtual string BddTechPassword
        {
            get { return _bddTechPassword; }
            set
            {
                OnBddTechPasswordChanging();
                _bddTechPassword = value;
                OnBddTechPasswordChanged();
            }
        }
        partial void OnBddTechPasswordChanging();
        partial void OnBddTechPasswordChanged();

        public virtual short? CodRaisonRevocation
        {
            get { return _codRaisonRevocation; }
            set
            {
                OnCodRaisonRevocationChanging();
                _codRaisonRevocation = value;
                OnCodRaisonRevocationChanged();
            }
        }
        partial void OnCodRaisonRevocationChanging();
        partial void OnCodRaisonRevocationChanged();

        public virtual string LibRaisonRevocation
        {
            get { return _libRaisonRevocation; }
            set
            {
                OnLibRaisonRevocationChanging();
                _libRaisonRevocation = value;
                OnLibRaisonRevocationChanged();
            }
        }
        partial void OnLibRaisonRevocationChanging();
        partial void OnLibRaisonRevocationChanged();

        public virtual System.DateTime? DateRevocation
        {
            get { return _dateRevocation; }
            set
            {
                OnDateRevocationChanging();
                _dateRevocation = value;
                OnDateRevocationChanged();
            }
        }
        partial void OnDateRevocationChanging();
        partial void OnDateRevocationChanged();

        public virtual string CodCategorieVeh
        {
            get { return _codCategorieVeh; }
            set
            {
                OnCodCategorieVehChanging();
                _codCategorieVeh = value;
                OnCodCategorieVehChanged();
            }
        }

        partial void OnCodCategorieVehChanging();
        partial void OnCodCategorieVehChanged();

        public virtual string NomFichier
        {
            get { return _nomFichier; }
            set
            {
                OnNomFichierChanging();
                _nomFichier = value;
                OnNomFichierChanged();
            }
        }
        partial void OnNomFichierChanging();
        partial void OnNomFichierChanged();

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
