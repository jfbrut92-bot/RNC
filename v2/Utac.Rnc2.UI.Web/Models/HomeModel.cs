using System.Collections.Generic;
using System.Linq;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class HomeModel
    {
        public rUtilisateur utilisateur { get; set; }
        public rMessageUtilisateur message { get; set; }
        public IList<pContoleurEncour> controleur { get; set; }

        //EG le 18/11/2011 pour recherche centre et contrôleur profil RESEAU*
        public RechercheProfilReseauModel RechercheProfilReseauModel { get; set; }
        //EG le 06/12/2011 pour la saisie des accuses de receptions des centres nouvellement notifiés
        private readonly Dictionary<rReseau, IList<pCentreEncour>> _listeCentreAccuseReception = new Dictionary<rReseau, IList<pCentreEncour>>();

        public Dictionary<rReseau, IList<pCentreEncour>> ListeCentreAccuseReception
        {
            get { return _listeCentreAccuseReception; }
            //set { _nombreAccuseReception = value; }
        }

        public bool existeAccuseReception(rReseau reseau)
        {
            IList<pCentreEncour> liste = this._listeCentreAccuseReception.ContainsKey(reseau) ? this._listeCentreAccuseReception[reseau] : null;
            return liste == null ? false : liste.Count() > 0 ? true : false;
        }

        //EG le 17/01/2012 pour les profils PREFECTURE
        public IList<rUtilisateur> ListeAutresUtilisateurs { get; set; }
        public IList<pContoleurEncour> ListeControleurAgreeCentreFermeVL { get; set; }
        public IList<pContoleurEncour> ListeControleurAgreeCentreFermePL { get; set; }
        public IList<pContoleurEncour> ListeControleurAgreeCentreFermeCL { get; set; }
        public IEnumerable<pContoleurEncour> ListeControleurAttenteAgrement { get; set; }
        public IList<pCentreEncour> ListeCentreAttenteAgrement { get; set; }
        public IList<pContoleurEncour> ListeControleurNouvellementAgree { get; set; }
        public IList<pCentreEncour> ListeCentreNouvellementAgree { get; set; }
        public IList<rUtilisateur> ListeUtilisateurDuReseau { get; set; }
        public IList<pCentreEncour> ListeCentreAgreeSansControleur { get; set; }
        public string datedeb { get; set; }
        //EG le 23/02/2012 liste des controleurs agréés sans centre de rattachement
        public IList<pContoleurEncour> ListeControleurAgreeSansCentreRattachementVL { get; set; }
        public IList<pContoleurEncour> ListeControleurAgreeSansCentreRattachementPL { get; set; }
        public IList<pContoleurEncour> ListeControleurAgreeSansCentreRattachementCL { get; set; }
        //YM le 23/10/2013 liste des préstataires Européens
        public IList<pContoleurEncour> ListePrestatairesEuropeensVL { get; set; }
        public IList<pContoleurEncour> ListePrestatairesEuropeensPL { get; set; }

    }
}
