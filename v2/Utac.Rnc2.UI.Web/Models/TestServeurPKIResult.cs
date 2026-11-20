namespace Utac.Rnc2.UI.Web.Models
{
    public class TestServeurPKIResult
    {
        private string _sURL;

        public string SURL
        {
            get { return _sURL; }
            set { _sURL = value; }
        }
        private bool _sEnsureSecurity;

        public bool SEnsureSecurity
        {
            get { return _sEnsureSecurity; }
            set { _sEnsureSecurity = value; }
        }
        private string _sUsername;

        public string SUsername
        {
            get { return _sUsername; }
            set { _sUsername = value; }
        }
        private string _sPassword;

        public string SPassword
        {
            get { return _sPassword; }
            set { _sPassword = value; }
        }
        private string _sDomain;

        public string SDomain
        {
            get { return _sDomain; }
            set { _sDomain = value; }
        }

        private int _resultCall;

        public int ResultCall
        {
            get { return _resultCall; }
            set { _resultCall = value; }
        }

        private string _sServeurPKI;

        public string SServeurPKI
        {
            get { return _sServeurPKI; }
            set { _sServeurPKI = value; }
        }

        private string _sRolesServeurCA;

        public string SRolesServeurCA
        {
            get { return _sRolesServeurCA; }
            set { _sRolesServeurCA = value; }
        }
        private int _iErreurCode;

        public int IErreurCode
        {
            get { return _iErreurCode; }
            set { _iErreurCode = value; }
        }
        private string _sErreurLibelle;

        public string SErreurLibelle
        {
            get { return _sErreurLibelle; }
            set { _sErreurLibelle = value; }
        }
        private string _sMessageException;

        public string SMessageException
        {
            get { return _sMessageException; }
            set { _sMessageException = value; }
        }

        private string _sFichierFichierCA;
        public string SFichierFichierCA
        {
            get { return _sFichierFichierCA; }
            set { _sFichierFichierCA = value; }
        }
    }
}