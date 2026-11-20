namespace Utac.Rnc2.BusinessObjects
{
    public partial class pGestionCertificat : BusinessBase<int>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        public enum CodeGestionCertificat
        {
            TELECHARGER_CERTI_INITIAL = 1,
            TELECHARGER_COIPIE_CERTI = 2,
            TELECHARGER_CERTI_RENOUVELE = 3,
            REVOCATION_CERTI = 4,
            REVOCATION_CERTI_SANS_RENOUVELLEMENT = 5,
            AUTORISATION_RENOUVELLEMENT = 6

        }

        private pCertificat _pCertificat = null;
        public virtual pCertificat PCertificat
        {
            get { return _pCertificat; }
            set
            {
                OnPCertificatChanging();
                _pCertificat = value;
                OnPCertificatChanged();
            }
        }
        partial void OnPCertificatChanging();
        partial void OnPCertificatChanged();

        public virtual bool CertificatRevoqueAvecRenouvellement
        {
            get
            {
                if (this.RActionCertificat.Id.Equals((short)pGestionCertificat.CodeGestionCertificat.REVOCATION_CERTI))
                {
                    return true;
                }
                return false;
            }
        }

        public virtual bool CertificatRevoqueSansRenouvellement
        {
            get
            {
                if (this.RActionCertificat.Id.Equals((short)pGestionCertificat.CodeGestionCertificat.REVOCATION_CERTI_SANS_RENOUVELLEMENT))
                {
                    return true;
                }
                return false;
            }
        }

        public virtual bool CertificatRevoque
        {
            get
            {
                return this.CertificatRevoqueAvecRenouvellement || this.CertificatRevoqueSansRenouvellement;
            }
        }

        public virtual bool CertificatAutorisationRenouvellement
        {
            get
            {
                if (this.RActionCertificat.Id.Equals((short)pGestionCertificat.CodeGestionCertificat.AUTORISATION_RENOUVELLEMENT))
                {
                    return true;
                }
                return false;
            }
        }

    }
}
