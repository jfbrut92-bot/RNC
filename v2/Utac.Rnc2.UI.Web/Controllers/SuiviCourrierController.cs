using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class SuiviCourrierController : RestrictedAcessController
    {

        private readonly IpCourrierEnvoyeService _courrierEnvoyeService;

        public SuiviCourrierController(IpCourrierEnvoyeService service)
        {
            this._courrierEnvoyeService = service;
        }

        public SuiviCourrierController()
            : this(NhibernateServiceFactory.getInstance().getpCourrierEnvoyeService(false))
        {
        }

        //
        // GET: /SuiviCourrier/

        public ActionResult Index(string deb, string fin)
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
                dateFin = Convert.ToDateTime(fin);
                dateDebut = Convert.ToDateTime(deb);
            }

            //Dans tous les cas
            dateFin = Convert.ToDateTime(dateFin).AddDays(1);

            IList<pCourrierEnvoye> liste = this._courrierEnvoyeService.FindAllByDate(dateDebut, dateFin);
            return View(liste);
        }

    }
}
