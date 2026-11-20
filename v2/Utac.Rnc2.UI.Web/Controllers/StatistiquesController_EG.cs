using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public partial class StatistiquesController : RendererController
    {
        private readonly IStatistiquesControleurService _controleurStatistiqueService = null;
        private readonly IrRegionService _regionService = null;
        private readonly IrEditionService _editionService = null;
        private readonly IrPrefectureService _prefectureService = null;
        private readonly IrUtilisateurService _utilisateurService = null;
        private readonly IStatistiquesCentreService _centreStatistiqueService;

        public StatistiquesController() :
            this(
                    NhibernateServiceFactory.getInstance().getStatistiquesControleurService(),
                    NhibernateServiceFactory.getInstance().getStatistiquesCentreService(),
                    NhibernateServiceFactory.getInstance().getrRegionService(false),
                    NhibernateServiceFactory.getInstance().getrEditionService(false),
                    NhibernateServiceFactory.getInstance().getrPrefectureService(false),
                    NhibernateServiceFactory.getInstance().getrUtilisateurService(false, false)
            )
        {

        }

        public StatistiquesController(IStatistiquesControleurService controleurStatistiqueService, IStatistiquesCentreService centreStatistiqueService, IrRegionService regionService, IrEditionService editionService, IrPrefectureService prefectureService, IrUtilisateurService utilisateurService)
        {
            this._controleurStatistiqueService = controleurStatistiqueService;
            this._centreStatistiqueService = centreStatistiqueService;
            this._regionService = regionService;
            this._editionService = editionService;
            this._prefectureService = prefectureService;
            this._utilisateurService = utilisateurService;
        }

        //
        // GET: /Statistiques/

        public ActionResult Index()
        {
            StatistiquesModel model = new StatistiquesModel();
            //PLE le 30/01/2024 Filtrage des éditions sur Actif/non actifs
            //model.ListeEdition = this._editionService.FindAll();
            model.ListeEdition = this._editionService.FindAllActif();
            model.ListeRegion = this._regionService.FindAll();
            //EG le 12/01/2012 : ajout d'une région null pour imposer le choix
            model.ListeRegion.Insert(0, null);

            //EG le 12/01/2012 : liste des préfectures pour liste des départements
            model.ListePrefecture = this._prefectureService.FindAll();
            model.ListePrefecture.Insert(0, null);

            model.FormatEdition = StatistiquesModel.FormatEditionEnum.XLS;
            model.Utilisateur = this._utilisateurService.FindBy(this.User.Identity.Name);

            return View(model);
        }

        public ActionResult d_edition_excel_centre_vlpl(StatistiquesModel model)
        {
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            rUtilisateur utilisateur = this._utilisateurService.FindBy(this.User.Identity.Name);
            IList<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> liste = this._centreStatistiqueService.listeDesInstallationsVéhiculesLegersELourdsSelonRegion(region, utilisateur);
            if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
            {
                return View(liste);
            }
            else
            {
                if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                {
                    string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                    System.Text.Encoding encoding = System.Text.Encoding.Default;
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeInstallation.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        public ActionResult d_edition_installation_vl_dep(StatistiquesModel model)
        {
            //EG le 13/01/2012 rPrefecture prefecture = this._prefectureService.findByCodeDepartement(model.CodeDepartement);
            if (model.PrefectureSelected.Id == 0)
            {
                TempData["Message"] = "Veuillez choisir un département ";
                TempData["MessageType"] = "Error";
                return RedirectToAction("Index");
            }

            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);

            IList<ItemListeDesInstallations> liste = this._centreStatistiqueService.ListeDesInstallationsVehiculesLegersSelonDepartement(prefecture);
            if (liste == null || liste.Count == 0)
            {
                TempData["Message"] = "Aucune installation pour le départment demandé : " + prefecture.CodDepartement;
                TempData["MessageType"] = "Error";
                return RedirectToAction("Index");
            }

            if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
            {
                return View(liste);
            }
            else
            {
                if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                {
                    string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                    System.Text.Encoding encoding = System.Text.Encoding.Default;
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesInstallationsVehiculesLegersSelonDepartement.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        public ActionResult d_edition_installation_pl_dep(StatistiquesModel model)
        {
            try
            {
                if (model.PrefectureSelected.Id == 0)
                {
                    TempData["Message"] = "Veuillez choisir un département ";
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);

                IList<ItemListeDesInstallations> liste = this._centreStatistiqueService.ListeDesInstallationsVehiculesLourdsSelonDepartement(prefecture);
                if (liste == null || liste.Count == 0)
                {
                    TempData["Message"] = "Aucune installation pour le départment demandé : " + prefecture.CodDepartement;
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
                {
                    return View(liste);
                }
                else
                {
                    if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                    {
                        //string xlsOut = this.RenderViewToString("d_edition_installation_vl_dep", liste); //C'est la même vue que pour le VL
                        string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                        //System.Text.Encoding encoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                        System.Text.Encoding encoding = System.Text.Encoding.Default;
                        return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesInstallationsVehiculesLourdsSelonDepartement.csv");
                    }
                    else
                    {
                        return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                    }

                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult d_edition_installation_pl_region(StatistiquesModel model)
        {
            try
            {

                if (model.RegionSelected.Id == 0)
                {
                    TempData["Message"] = "Veuillez choisir une région ";
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (model.PrefectureSelected.Id == 0)
                {
                    TempData["Message"] = "Veuillez choisir un département ";
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
                rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);

                IList<ItemListeDesInstallations> liste = this._centreStatistiqueService.ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(region, prefecture);
                if (liste == null || liste.Count == 0)
                {
                    TempData["Message"] = "Aucune installation pour la région : " + region.LibelleRegion + " et le départment demandé : " + prefecture.CodDepartement;
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
                {
                    return View(liste);
                }
                else
                {
                    if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                    {
                        //string xlsOut = this.RenderViewToString("d_edition_installation_vl_dep", liste); //C'est la même vue que pour le VL
                        string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString("d_edition_installation_pl_dep", liste));
                        System.Text.Encoding encoding = System.Text.Encoding.Default;
                        return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesInstallationsVehiculesLourdsSelonRegionDepartement.csv");
                    }
                    else
                    {
                        return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult d_edition_excel_suivi_pl(StatistiquesModel model)
        {
            try
            {
                rRegion region = this._regionService.FindBy(model.RegionSelected.Id);

                IList<ItemListeSuiviQualificationPL> liste = this._controleurStatistiqueService.ListeDesSuivisQualificationControleursPLSelonRegion(region);
                if (liste == null || liste.Count == 0)
                {
                    TempData["Message"] = "Aucun suivi pour la région demandée : " + (region != null ? region.LibelleRegion : "");
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
                {
                    return View(liste);
                }
                else
                {
                    if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                    {
                        string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                        System.Text.Encoding encoding = System.Text.Encoding.Default;
                        return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesSuivisQualificationControleursPLSelonRegion.csv");
                    }
                    else
                    {
                        return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //YM 24/05/2013
        public ActionResult d_edition_excel_suivi_vl(StatistiquesModel model)
        {
            try
            {
                rRegion region = this._regionService.FindBy(model.RegionSelected.Id);

                IList<ItemListeSuiviQualificationVL> liste = this._controleurStatistiqueService.ListeDesSuivisQualificationControleursVLSelonRegion(region);
                if (liste == null || liste.Count == 0)
                {
                    TempData["Message"] = "Aucun suivi pour la région demandée : " + (region != null ? region.LibelleRegion : "");
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
                {
                    return View(liste);
                }
                else
                {
                    if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                    {
                        string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                        System.Text.Encoding encoding = System.Text.Encoding.Default;
                        return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesSuivisQualificationControleursVLSelonRegion.csv");
                    }
                    else
                    {
                        return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //YM 15/10/2013
        public ActionResult d_edition_excel_activite_vl(StatistiquesModel model)
        {
            try
            {
                rRegion region = this._regionService.FindBy(model.RegionSelected.Id);

                IList<ItemListeSuiviActiviteVL> liste = this._controleurStatistiqueService.ListeDesSuivisActiviteControleursVLSelonRegion(region);
                if (liste == null || liste.Count == 0)
                {
                    TempData["Message"] = "Aucun suivi pour la région demandée : " + (region != null ? region.LibelleRegion : "");
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
                {
                    return View(liste);
                }
                else
                {
                    if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                    {
                        string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                        System.Text.Encoding encoding = System.Text.Encoding.Default;
                        return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesSuivisActiviteControleursVLSelonRegion.csv");
                    }
                    else
                    {
                        return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //YM 16/10/2013
        public ActionResult d_edition_excel_activite_vl_nat_reseau(StatistiquesModel model)
        {
            rUtilisateur utilisateur = this._utilisateurService.FindBy(this.User.Identity.Name);

            IList<ItemListeSuiviActiviteVL> liste = this._controleurStatistiqueService.ListeDesSuivisActiviteControleursVLRattachesAuxCentresDuReseau(utilisateur);
            if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
            {
                return View(liste);
            }
            else
            {
                if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                {
                    string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                    System.Text.Encoding encoding = System.Text.Encoding.Default;
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_excel_activite_cntlr_vl_nat_reseau.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/ItemListeDesInstallationsVehiculesLegersSelonRegion

        public ActionResult d_cntlr_edition_suivi_liste(StatistiquesModel model)
        {
            try
            {
                if (model.RegionSelected.Id == 0)
                {
                    TempData["Message"] = "Saisir une région !";
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                rRegion region = this._regionService.FindBy(model.RegionSelected.Id);

                IList<ItemListeSuiviQualificationPL> liste = this._controleurStatistiqueService.ListeDesSuivisQualificationControleursPLSelonRegion(region);
                if (liste == null || liste.Count == 0)
                {
                    TempData["Message"] = "Aucun suivi pour la région demandée : " + (region != null ? region.LibelleRegion : "");
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
                {
                    return View(liste);
                }
                else
                {
                    if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                    {
                        string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString("d_edition_excel_suivi_pl", liste)); //On utilise la vue d_edition_excel_suivi_pl
                        System.Text.Encoding encoding = System.Text.Encoding.Default;
                        return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesSuivisQualificationControleursPLSelonRegion.csv");
                    }
                    else
                    {
                        return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult d_edition_excel_suivi_pl_reseau(StatistiquesModel model)
        {
            try
            {
                rUtilisateur u = InformationHelper.getInformationPersonnelle(false).Utilisateur;
                IList<ItemListeSuiviQualificationPL> liste = this._controleurStatistiqueService.ListeDesSuivisQualificationControleursPLSelonReseaux(u.RReseaux);

                if (liste == null || liste.Count == 0)
                {
                    TempData["Message"] = "Aucun suivi pour votre profil utilisateur ";
                    TempData["MessageType"] = "Error";
                    return RedirectToAction("Index");
                }

                if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
                {
                    return View(liste);
                }
                else
                {
                    if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                    {
                        string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString("d_edition_excel_suivi_pl", liste)); //On utilise la vue d_edition_excel_suivi_pl
                        System.Text.Encoding encoding = System.Text.Encoding.Default;
                        return File(encoding.GetBytes(xlsOut), "application/octet-stream", "ListeDesSuivisQualificationControleursPLReseau.csv");
                    }
                    else
                    {
                        return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                    }
                }
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

    }
}
