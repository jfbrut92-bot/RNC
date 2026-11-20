using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class SiteOtcListCentreModel
    {

        public IList<pCentreEncour> centresAgrees{ get; set; }

        public IList<rPrefecture> ListePrefecture { get; set; }
        [Required(ErrorMessage = "Obligatoire")]
        public rPrefecture PrefectureSelected { get; set; }

    }

}
