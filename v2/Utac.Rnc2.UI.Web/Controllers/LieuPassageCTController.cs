using System;
using System.Collections.Generic;
using System.Web.Mvc;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class LieuPassageCTController : Controller
    {
        private readonly ILog log = LogManager.GetLogger(typeof(LieuPassageCTController));

        private readonly IpTraceRechLieuVlService _pTraceRechLieuVlService;
        private readonly IpCentreEncourService _pCentreEnCourservice;
        private readonly IrPrefectureService _prefectureService;

        public LieuPassageCTController(IpCentreEncourService pCentreEnCourService, IpTraceRechLieuVlService pTraceRechLieuVlService, IrPrefectureService prefectureService)
        {
            this._pTraceRechLieuVlService = pTraceRechLieuVlService;
            this._pCentreEnCourservice = pCentreEnCourService;
            this._prefectureService = prefectureService;
        }

        public LieuPassageCTController()
            : this(
                   NhibernateServiceFactory.getInstance().getpCentreEncourService(true, true),
                   NhibernateServiceFactory.getInstance().getpTraceRechLieuVlService(true),
                   NhibernateServiceFactory.getInstance().getrPrefectureService(true)
               )
        {
        }

        // GET: /LieuPassageCT/Rechercher
        public ActionResult Rechercher()
        {
            LieuPassageCTModel m = new LieuPassageCTModel();
            //m.Nom = "";
            //m.Prenom = "";
            //m.NumImmat = "";
            m.DatePremImmat = null;
            //m.NumSerie = "";
            return View(m);
        }

        // POST: /LieuPassageCT/Rechercher
        [HttpPost]
        public ActionResult Rechercher(LieuPassageCTModel m)
        {
            pTraceRechLieuVl t = new pTraceRechLieuVl();

            try
            {
                ValidateRecherche(m);
                if (ModelState.IsValid)
                {
                    try
                    {
                        m.DateRecherche = DateTime.Now;

                        LieuVlWebUtil lieuVlWebUtil = new LieuVlWebUtil(Server.MapPath("~/bin"));
                        LieuVlWebUtil.EnumCodeRetourLieuVl codRetour;
                        string codeCentre = null, numImmat = null, numSerie = null;
                        numImmat = m.NumImmat.ToUpper();
                        numSerie = m.NumSerie.ToUpper().Replace("O", "0");
                        m.Nom = (m.Nom == null) ? "N/A" : m.Nom.ToUpper();
                        m.Prenom = (m.Prenom == null) ? "N/A" : m.Prenom.ToUpper();

                        codRetour = lieuVlWebUtil.recupereLieuVl(m.Nom + m.Prenom, ref numImmat, ref numSerie, m.DatePremImmat.Value, ref codeCentre);

                        t.Nom = m.Nom;
                        t.Prenom = m.Prenom;
                        t.ImmatVeh = numImmat;
                        t.DateCircu = m.DatePremImmat.Value;
                        t.NumSerie = numSerie;
                        t.DateRecherche = m.DateRecherche;
                        t.CodAgremCentre = codeCentre;

                        if (codRetour.Equals(LieuVlWebUtil.EnumCodeRetourLieuVl.TROUVE_CT))
                        {
                            m.CodeCentre = codeCentre;
                            //Recherche des coordonnées du centre VL
                            m.Centre = _pCentreEnCourservice.findByCodeAgrementVL(codeCentre);
                            if (m.Centre == null)
                            {
                                //Le centre n'a pas été trouvé dans RNC2
                                codRetour = LieuVlWebUtil.EnumCodeRetourLieuVl.AUCUN_CT;
                            }
                        }

                        if (codRetour.Equals(LieuVlWebUtil.EnumCodeRetourLieuVl.TROUVE_CT))
                        {
                            m.NumImmat = numImmat;
                            m.NumSerie = numSerie;
                            //m.CodeCentre = codeCentre;
                            //Recherche des coordonnées du centre VL
                            //m.Centre = _pCentreEnCourservice.findByCodeAgrementVL(codeCentre);
                            //On enregistre la demande avec le code résultat
                            t.CodResultat = (int)pTraceRechLieuVl.CodeResultatRechercheCT.TROUVE_CT;
                            this._pTraceRechLieuVlService.TracerAppel(t);
                            log.Debug("t.Id : " + t.Id);
                            int id = t.Id;
                            m.NumChrono = id;
                            log.Debug("m.NumChrono : " + m.NumChrono);

                            return View("ResultatLieuPassageCT", m);
                        }
                        else
                        {
                            if (codRetour.Equals(LieuVlWebUtil.EnumCodeRetourLieuVl.AUCUN_CT))
                            {
                                //On enregistre la demande avec le code résultat
                                t.CodResultat = (int)pTraceRechLieuVl.CodeResultatRechercheCT.AUCUN_CT;
                                this._pTraceRechLieuVlService.TracerAppel(t);

                                //ViewData["message"] = "Le contrôle technique n'a pas été trouvé dans la base de l'OTC. Veuillez vérifier la saisie de vos critères de recherche.";
                                ViewData["message"] = "Nous ne sommes pas en mesure d’indiquer les coordonnées d’un centre ayant effectué un contrôle datant de moins de 24 mois pour le véhicule ci-dessus.";
                                ViewData["message"] += "<br/>--> Veuillez vérifier la saisie de vos critères de recherche.";
                                //ViewData["message"] += "<br/>&nbsp;&nbsp;&nbsp;&nbsp;ou faire une demande écrite à l'Organisme Technique Central (<a title='Procédure à suivre.' href='/fr/ctvl/faq.asp#q8'>cliquez-ici</a>).";
                                return View("Rechercher", m);
                            }
                            else
                            {
                                if (codRetour.Equals(LieuVlWebUtil.EnumCodeRetourLieuVl.ERREUR_TECHNIQUE))
                                {
                                    //On enregistre la demande avec le code résultat
                                    t.CodResultat = (int)pTraceRechLieuVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE;
                                    this._pTraceRechLieuVlService.TracerAppel(t);

                                    ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";

                                    return View("Rechercher", m);
                                }
                            }
                        }

                        throw new NotImplementedException();
                    }
                    catch (Exception e)
                    {
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }
                return View("Rechercher", m);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public CaptchaImageResult ShowCaptchaImage()
        {
            return new CaptchaImageResult();
        }

        // GET: /TraceRechPvVl/Ajouter
        public ActionResult SuiviRechercheLieuPassageCT(string deb, string fin)
        {
            if (GestionDroitsExecution.isRightAfficheLieuPassageCT(this.User))
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

                IList<pTraceRechLieuVl> liste = this._pTraceRechLieuVlService.FindAllByDate(dateDebut, dateFin);
                ViewData["datedeb"] = String.Format("{0:d}", dateDebut);
                ViewData["datefin"] = String.Format("{0:d}", dateFin);
                return View(liste);
            }
            else
            {
                Exception e = new Exception("Vous n'êtes pas autorisé....");
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void ValidateRecherche(LieuPassageCTModel m)
        {
            switch (m.FormatImmat)
            {
                case "1":
                    if (m.NumImmatSIV == null)
                    {
                        ModelState.AddModelError("NumImmatSIV", "Obligatoire");
                    }
                    else
                    {
                        m.NumImmat = m.NumImmatSIV;
                    }
                    break;
                case "2":
                    if (m.NumImmatFNI == null)
                    {
                        ModelState.AddModelError("NumImmatFNI", "Obligatoire");
                    }
                    else
                    {
                        m.NumImmat = m.NumImmatFNI;
                    }
                    break;
                case "3":
                    if (m.NumImmatAUTRE == null)
                    {
                        ModelState.AddModelError("NumImmatAUTRE", "Obligatoire");
                    }
                    else
                    {
                        m.NumImmat = m.NumImmatAUTRE;
                    }
                    break;
            }

            if (HttpContext.Session["captchastring"] == null)
            {
                ModelState.AddModelError("Captcha", "Invalide !");
            }
            else
            {
                if (m.Captcha.ToUpper() != HttpContext.Session["captchastring"].ToString())
                    ModelState.AddModelError("Captcha", "Invalide !");
            }
        }

        // GET: /LieuPassageCT/ListCentreAgree
        public ActionResult ListCentreAgree()
        {
            SiteOtcListCentreModel m = new SiteOtcListCentreModel();

            //EG le 12/01/2012 : liste des préfectures pour liste des départements
            m.ListePrefecture = this._prefectureService.FindAll();
            m.ListePrefecture.Insert(0, null);

            return View(m);
        }

        // POST: /LieuPassageCT/ListCentreAgree
        [HttpPost]
        public ActionResult ListCentreAgree(SiteOtcListCentreModel m)
        {
            if (m.PrefectureSelected != null)
            {

                m.centresAgrees = this._pCentreEnCourservice.FindCentreCLPourSiteWeb(m.PrefectureSelected);
            }
            m.ListePrefecture = this._prefectureService.FindAll();
            m.ListePrefecture.Insert(0, null);
            return View(m);
        }

    }
}
