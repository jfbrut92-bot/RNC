using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class ControleurHabilitationPLModel
    {
        public pContoleurEncour Controleur { get; set; }

        public IList<rHabilitation> ListeHabilitation { get; set; }

        public int NombreChargement { get; set; }

    }
}