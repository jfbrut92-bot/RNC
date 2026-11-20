using System.Collections.Generic;

namespace Utac.Rnc2.BusinessObjects.SharedModel
{
    public class AjouterUtilisateurModel
    {
        public IList<rProfil2> ListeSousProfils { get; set; }
        public IList<rPrefecture> ListePrefectures { get; set; }
        public IList<rRegion> ListeRegions { get; set; }
        public IList<rReseau> ListeReseaux { get; set; }

        public string TypeCentre { get; set; }
        public rUtilisateur Utilisateur { get; set; }
        //Reseaux d'appartenance géré en dehors de Utilisateur
        public string[] ReseauxAppartenance { get; set; }
    }
}
