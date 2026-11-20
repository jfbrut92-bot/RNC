using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace Utac.Rnc2.MVC.Extensions
{
    /// <summary>     
    /// Authorization attribute for an MVC controller     
    /// </summary>     

    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] _rolesArray;
        private readonly string _authorizationGroupName;

        /// <summary>         
        ///         
        /// </summary>         
        public string AccessDeniedController { get; set; }

        /// <summary>         
        ///         
        /// </summary>         
        public string AccessDeniedAction { get; set; }

        /// <summary>         
        ///         
        /// </summary>         

        public ControllerAuthorizeAttribute()
        {
            AccessDeniedController = ConfigurationManager.AppSettings["DefaultAccessDeniedController"] ?? "AccessDenied";
            AccessDeniedAction = ConfigurationManager.AppSettings["DefaultAccessDeniedAction"] ?? "Index";
            _authorizationGroupName = string.Empty;
        }

        /// <summary>         
        ///         
        /// </summary>         
        public string[] RolesArray
        {
            get
            {
                return _rolesArray;
            }
            set
            {
                _rolesArray = value;
                this.Roles = string.Join(",", _rolesArray);
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;               // Get any controller specific role mappings
            var controllerRoleMappings = AuthorizationConfiguration.Section.ControllerAuthorizationMappings.Where(e => e.Controller == controllerName).FirstOrDefault();
            //EG ajout pour éviter de déclarer des ActionAuthorize() au niveau de chaque action
            string actionName = filterContext.ActionDescriptor.ActionName;
            ActionAuthorizationConfigurationElement actionRoleMappings = null;
            if (controllerRoleMappings != null)
                actionRoleMappings = controllerRoleMappings.ActionAuthorizationMappings.Where(e => e.Action.ToLower().Equals(actionName.ToLower())).FirstOrDefault();
            //Priorité aux actions
            if (actionRoleMappings != null && !string.IsNullOrEmpty(actionRoleMappings.Roles))
            {
                this.RolesArray = actionRoleMappings.Roles.Split(',');
            }
            else
            {
                if (controllerRoleMappings != null && !string.IsNullOrEmpty(controllerRoleMappings.Roles))
                {
                    this.RolesArray = controllerRoleMappings.Roles.Split(',');
                }
            }
            //             
            base.OnAuthorization(filterContext);

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //27/11/2019 PLE Ajout d'une redirection vers la page de login si l'utilisateur n'est pas authentifié
            if (filterContext.HttpContext.Request.IsAuthenticated == false)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary
                        {
                                { "langCode", filterContext.RouteData.Values[ "langCode" ] },
                                { "controller", "Account" },
                                { "action", "LogOn" },
                                { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
                        });
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = AccessDeniedController, Action = AccessDeniedAction, Roles = Roles }));
            }
        }
    }

}
