using System;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class AnnulerAgrementModel
    {
        public rCategorieVehicule.CategorieEnum CategVehicule { get; set; }

        public pCentreEncour Centre { get; set; }

        public Boolean MsgControlMotif { get; set; }

        public Dictionary<int, string> ListeModification { get; set; }
    }
}