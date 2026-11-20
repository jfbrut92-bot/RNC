using System.Web.Mvc;

namespace Utac.Rnc2.UI.Web.Models
{
    public class InfoCertificatModel : Controller
    {

        #region variable privée
        private string _nom;
        private string _prenom;
        private string _identifiantDb;
        private string _motDePasseDb;
        private string _entite;
        private string _motDePassePfx;
        private string _motDePasseZip;
        #endregion

        #region variable public
        public string Prenom
        {
            get { return _prenom; }
            set { _prenom = value; }
        }

        public string IdentifiantDb
        {
            get { return _identifiantDb; }
            set { _identifiantDb = value; }
        }

        public string MotDePasseDb
        {
            get { return _motDePasseDb; }
            set { _motDePasseDb = value; }
        }

        public string Entite
        {
            get { return _entite; }
            set { _entite = value; }
        }

        public string MotDePassePfx
        {
            get { return _motDePassePfx; }
            set { _motDePassePfx = value; }
        }

        public string MotDePasseZip
        {
            get { return _motDePasseZip; }
            set { _motDePasseZip = value; }
        }

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
        #endregion

    }
}
