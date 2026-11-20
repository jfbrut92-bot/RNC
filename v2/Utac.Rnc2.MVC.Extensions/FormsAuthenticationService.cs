using System;
using System.Web;
using System.Web.Security;

namespace Utac.Rnc2.MVC.Extensions
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private readonly RoleProvider _roleProvider;

        public FormsAuthenticationService(RoleProvider provider)
        {
            this._roleProvider = provider;
        }

        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("La valeur ne peut pas être null ou vide.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public HttpCookie SignInEncrypt(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("La valeur ne peut pas être null ou vide.", "userName");

            string[] roles = this._roleProvider.GetRolesForUser(userName);
            string role = string.Empty;
            if (roles != null)
            {
                foreach (string r in roles)
                {
                    if (string.IsNullOrEmpty(role))
                        role = r;
                    else
                        role += ";" + r;
                }
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    1,                              	// version
                    userName,       	  			  	// user name
                    DateTime.Now,                   	// issue time
                    DateTime.Now.AddMinutes(30),       	// expires soon
                    createPersistentCookie, 			// persist cookie
                    role                    			// roles
                );

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            return cookie;

        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
