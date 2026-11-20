using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class StatistiquesModel
    {
        public IList<rEdition> ListeEdition { get; set; }
        public IList<rRegion> ListeRegion { get; set; }
        //public string CodeDepartement { get; set; }
        public rRegion RegionSelected { get; set; }
        public FormatEditionEnum FormatEdition { get; set; }
        public rUtilisateur Utilisateur { get; set; }

        //EG le 12/01/2012
        public IList<rPrefecture> ListePrefecture { get; set; }
        public rPrefecture PrefectureSelected { get; set; }

        public enum FormatEditionEnum
        {
            XLS,
            PDF,
            HTML
        }

    }

}