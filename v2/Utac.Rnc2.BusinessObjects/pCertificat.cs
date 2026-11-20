using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects
{
    public partial class pCertificat : BusinessBase<string>
    {
        protected override void validate()
        {
            /* Implementer ICI les r?gles de validation */
        }

        // EG le 18/03/2011 : ajout de la colonne id_centre_encours
        private pCentreEncour _pCentreEncour;
        partial void OnPCentreEncourChanging();
        partial void OnPCentreEncourChanged();

        public virtual pCentreEncour PCentreEncour
        {
            get { return _pCentreEncour; }
            set
            {
                OnPCentreEncourChanging();
                _pCentreEncour = value;
                OnPCentreEncourChanged();
            }
        }

        private IList<pGestionCertificat> _pGestionCertificats = new List<pGestionCertificat>();

        partial void OnPGestionCertificatsChanging();
        partial void OnPGestionCertificatsChanged();

        public virtual IList<pGestionCertificat> PGestionCertificats
        {
            get { return _pGestionCertificats; }
            set
            {
                OnPGestionCertificatsChanging();
                _pGestionCertificats = value;
                OnPGestionCertificatsChanged();
            }
        }

    }
}
