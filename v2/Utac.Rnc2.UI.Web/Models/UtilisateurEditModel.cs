using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class UtilisateurEditModel
    {
        public rUtilisateur Utilisateur { get; set; }

        public IList<rProfil2> ListeSousProfil { get; set; }

        public IList<rRegion> ListeRegion { get; set; }

        public IList<rReseau> ListeReseau { get; set; }

        public string Message { get; set; }
        public string MessageTechnique { get; set; }

    }
}