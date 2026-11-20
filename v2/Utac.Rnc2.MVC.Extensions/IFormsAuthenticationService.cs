using System.Web;

namespace Utac.Rnc2.MVC.Extensions
{
    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        HttpCookie SignInEncrypt(string userName, bool createPersistentCookie);
        void SignOut();
    }

}
