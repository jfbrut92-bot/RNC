using System.Web.Mvc;
using Utac.Rnc2.MVC.Extensions;

namespace Utac.Rnc2.UI.Web.Controllers
{
    [ControllerAuthorize()]
    [ControllerFilterSessionExpired()]
    [LoggerFilter()]
    public class RestrictedAcessController : Controller
    {

    }
}
