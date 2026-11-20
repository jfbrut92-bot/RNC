using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.MVC.Extensions;
using Utac.Rnc2.UI.Web.MemberShip;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(new RNC2RoleProvider()); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        public ActionResult Contact()
        {

            return View();
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************
        public ActionResult LogOn()
        {

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.IsExistUser(model.UserName))
                {
                    if (!MembershipService.IsBlockedUser(model.UserName))
                    {

                        if (MembershipService.ValidateUser(model.UserName, model.Password))
                        {
                            //FormsService.SignIn(model.UserName, model.RememberMe);
                            HttpCookie cookie = FormsService.SignInEncrypt(model.UserName, model.RememberMe);
                            this.Response.Cookies.Add(cookie);
                            //EG le 09/11/2011 pour gestion des accès en mode login temporaire
                            HttpCookie cookieLogOn = new HttpCookie("logOnType", rUtilisateur.LogonType.PERMANENT.ToString());
                            this.Response.Cookies.Add(cookieLogOn);
                            if (!String.IsNullOrEmpty(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Le nom d'utilisateur ou mot de passe fourni est incorrect.");
                        }
                    }
                    else
                    {
                        //Le user est bloqué!
                        ModelState.AddModelError("", "Votre accès a été bloqué !");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Le nom d'utilisateur ou mot de passe fourni est incorrect.");
                }
            }

            // Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            //On supprime toutes les variables session
            this.Session.Clear();

            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/TempLogOn
        // **************************************
        //Permet de gérer les logins pour les inscriptions des centres indépendant

        public ActionResult TempLogOn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult TempLogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!MembershipService.IsBlockedUser(model.UserName))
                {
                    if (MembershipService.ValidateTempUser(model.UserName, model.Password))
                    {
                        //FormsService.SignIn(model.UserName, model.RememberMe);
                        HttpCookie cookie = FormsService.SignInEncrypt(model.UserName, false);
                        this.Response.Cookies.Add(cookie);
                        //EG le 09/11/2011 pour gestion des accès en mode login temporaire
                        HttpCookie cookieLogOn = new HttpCookie("logOnType", rUtilisateur.LogonType.TEMPORAIRE.ToString());
                        this.Response.Cookies.Add(cookieLogOn);
                        return RedirectToAction("Index", "Inscription");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Le nom d'utilisateur ou mot de passe fourni est incorrect.");
                    }
                }
                else
                {
                    //Le user est bloqué!
                    ModelState.AddModelError("", "Votre accès a été bloqué !");
                }
            }


            // Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
            return View(model);
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        //public ActionResult Register()
        //{
        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Tentative d'inscription de l'utilisateur
        //        MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
        //            return RedirectToAction("Index", "Home");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
        //        }
        //    }

        //    // Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
        //    return View(model);
        //}

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        //[Authorize]
        //public ActionResult ChangePassword()
        //{
            
        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
        //    return View();
        //}

        //[Authorize]
        //[HttpPost]
        //public ActionResult ChangePassword(ChangePasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
        //        {
        //            return RedirectToAction("ChangePasswordSuccess");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Le mot de passe actuel est incorrect ou le nouveau mot de passe n'est pas valide.");
        //        }
        //    }

        //    // Si nous sommes arrivés là, quelque chose a échoué, réafficher le formulaire
        //    ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
        //    return View(model);
        //}

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        //public ActionResult ChangePasswordSuccess()
        //{
        //    return View();
        //}

    }
}
