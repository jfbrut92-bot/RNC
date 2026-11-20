using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class CertificatModel
    {
        public pCentreEncour Centre { get; set; }

        public rCategorieVehicule.CategorieEnum CategeorieVehicule { get; set; }

        public bool AUnCertificatVlValide { get; set; }

        public bool AUnCertificatPlValide { get; set; }

        public bool AUnCertificatClValide { get; set; }

        public pCertificat DernierCertificat { get; set; }

        //EG le 22/11/2011 pour factorisation avec le Réseau
        public bool AUnCertificatValide { get; set; }
        public rReseau Reseau { get; set; }
        public bool AutorisationRenew { get; set; }

        public pGestionCertificat DerniereTraceGestion { get; set; }

        //EG le 01/06/2012 pour prendre en compte le cas du renouvellement avant la date d'expiration
        public bool MustRenew { get; set; }
    }
}