using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class TelechargerNotificationAgrementModel
    {
        public pCentreEncour Centre { get; set; }

        public bool EtatVl { get; set; }

        public bool EtatPl { get; set; }

        public bool EtatCl { get; set; }

        public bool ModifNotif { get; set; }

    }
}

