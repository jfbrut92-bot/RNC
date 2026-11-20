using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class NouveauCentreModel
    {
        public bool EstCentreAdministration { get; set; }
        public string TypeCentre { get; set; }
        public rCategorieVehicule.CategorieEnum CategorieVehicule { get; set; }
        public pCentreEncour Centre { get; set; }
        public IList<rReseau> ListeReseau { get; set; }
        public IList<rStatutJuridique> ListeStatutJuridique { get; set; }
        public String Token { get; set; }
    }
}

