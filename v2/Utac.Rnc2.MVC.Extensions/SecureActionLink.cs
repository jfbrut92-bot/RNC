using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Utac.Rnc2.MVC.Extensions
{
    public static class LinkExtensions
    {
        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public static MvcHtmlString SecuredActionLink(this HtmlHelper helper, string linkText, string actionName)
        {

            RouteData routeData = helper.ViewContext.RouteData;
            string controllerName = routeData.GetRequiredString("controller");

            return SecuredActionLink(helper, linkText, actionName, controllerName, null);
        }

        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static MvcHtmlString SecuredActionLink(this HtmlHelper helper, string linkText, string actionName, object routeValues)
        {

            RouteData routeData = helper.ViewContext.RouteData;
            string controllerName = routeData.GetRequiredString("controller");

            return SecuredActionLink(helper, linkText, actionName, controllerName, routeValues);
        }

        /// <summary>
        /// Renders the action link, provided the user has access to the action method being linked to
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static MvcHtmlString SecuredActionLink(this HtmlHelper helper, string linkText, string actionName, string controllerName, object routeValues)
        {
            MvcHtmlString html = MvcHtmlString.Empty;
            string[] roles = null;

            // Get any controller specific role mappings
            var controllerRoleMappings = AuthorizationConfiguration.Section.ControllerAuthorizationMappings.Where(e => e.Controller == controllerName).FirstOrDefault();
            ActionAuthorizationConfigurationElement actionRoleMappings = null;

            if (controllerRoleMappings != null)
            {
                actionRoleMappings = controllerRoleMappings.ActionAuthorizationMappings.Where(e => e.Action == actionName).FirstOrDefault();

                if (actionRoleMappings != null && !string.IsNullOrEmpty(actionRoleMappings.Roles))
                {
                    roles = actionRoleMappings.Roles.Split(',');
                }
            }

            if (roles != null)
            {
                foreach (string role in roles)
                {
                    //if (ApplicationUser.Current.Roles.Contains(role))
                    if (HttpContext.Current.User.IsInRole(role))
                    {
                        html = System.Web.Mvc.Html.LinkExtensions.ActionLink(helper, linkText, actionName, controllerName, routeValues, null);
                        break;
                    }
                }
            }
            else if (actionRoleMappings == null)
            {
                // If there's no roles associated to that action then render the link
                html = System.Web.Mvc.Html.LinkExtensions.ActionLink(helper, linkText, actionName, controllerName, routeValues, null);
            }

            return html;
        }

    }

}

