using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.UI.Web.Models
{
    public class RechercheProfilReseauModel
    {
        public FiltreRechercheProfilReseau Filtre { get; set; }
        public List<pCentreEncour> ListeCentres { get; set; }
        public IList<pContoleurEncour> ListeControleurs { get; set; }
        public rReseau ReseauCourant { get; set; }
    }
}