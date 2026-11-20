namespace Utac.Rnc2.UI.Web.Utils
{
    public partial class InfoCertificat
    {
        #region variable privée
        private string _nomCert;
        private string _nomCommunCert;
        private string _motDePassePfx;
        private string _categorieVehicule;
        #endregion

        #region variable public
        public string NomCert
        {
            get { return _nomCert; }
            set { _nomCert = value; }
        }

        public string NomCommunCert
        {
            get { return _nomCommunCert; }
            set { _nomCommunCert = value; }
        }
        public string MotDePassePfx
        {
            get { return _motDePassePfx; }
            set { _motDePassePfx = value; }
        }

        public string CategorieVehicule
        {
            get { return _categorieVehicule; }
            set { _categorieVehicule = value; }
        }

        #endregion
    }

}