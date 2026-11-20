using System;
using System.Collections.Generic;
using System.Web.Mvc;

using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class SuiviTelechargementPvController : RestrictedAcessController
    {
        private readonly IpTraceRechPvVlService _pTraceRechPvVlService;

        public SuiviTelechargementPvController(IpTraceRechPvVlService pTraceRechPvVlService)
        {
            _pTraceRechPvVlService = pTraceRechPvVlService;
        }

        public SuiviTelechargementPvController()
            : this(
                   NhibernateServiceFactory.getInstance().getpTraceRechPvVlService(true)
               )
        {
        }
        public ActionResult SuiviTelechargementPV(string deb, string fin)
        {
            if (GestionDroitsExecution.isRightAfficheTelechergePV(User))
            {
                DateTime dateDebut = default(DateTime);
                DateTime dateFin = default(DateTime);

                if (string.IsNullOrWhiteSpace(deb))
                {
                    dateFin = Convert.ToDateTime(string.Format("{0:d}", System.DateTime.Now));
                    dateDebut = Convert.ToDateTime(dateFin).AddDays(-1);
                }
                else
                {
                    dateDebut = Convert.ToDateTime(deb);
                    if (string.IsNullOrWhiteSpace(fin))
                    {
                        dateFin = Convert.ToDateTime(string.Format("{0:d}", System.DateTime.Now));
                    }
                    else
                    {
                        dateFin = Convert.ToDateTime(fin);
                    }

                }

                //Dans tous les cas
                dateFin = Convert.ToDateTime(dateFin).AddDays(1);

                IList<pTraceRechPvVl> liste = _pTraceRechPvVlService.FindAllByDate(dateDebut, dateFin);
                return View("~/Views/SuiviTelechargementPv/Index.cshtml", liste);
            }
            else
            {
                Exception e = new Exception("Vous n'êtes pas autorisé....");
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }
    }
}
