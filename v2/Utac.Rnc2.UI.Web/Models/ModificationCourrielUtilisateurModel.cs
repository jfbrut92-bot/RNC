using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class ModificationCourrielUtilisateurModel
    {
        public rUtilisateur Utilisateur { get; set; }

        public string NouveauCourriel { get; set; }

        public string ConfirmationCourriel { get; set; }

        public int IdCentre { get; set; }

    }
}
