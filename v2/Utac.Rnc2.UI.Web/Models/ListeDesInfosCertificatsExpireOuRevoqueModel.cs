using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.certificats;

namespace Utac.Rnc2.UI.Web.Models
{
    public class ListeDesInfosCertificatsExpireOuRevoqueModel
    {
        public IList<ItemListeDesInfosCertificatsExpireOuRevoque> Liste { get; set; }
        public rCategorieVehicule.CategorieEnum Categorie { get; set; }

    }
}