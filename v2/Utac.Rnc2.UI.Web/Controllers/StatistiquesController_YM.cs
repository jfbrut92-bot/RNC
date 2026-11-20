using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public partial class StatistiquesController : RendererController
    {
        //-------------------------------------------------------------------------------
        //    Attention les services sont à créer dans StatistiquesController_EG
        //-------------------------------------------------------------------------------

        //
        // GET: /Statistiques/NBControleurParRegionEtDepartement

        public ActionResult d_edition_excel_cntlr_vlpl(StatistiquesModel model)
        {
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            IList<ItemListeDesControleursVéhiculesLegersEtLourds> liste = this._controleurStatistiqueService.listeDesControleursVehiculesLegersEtLourdsSelonRegion(region);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_excel_cntlr_vlpl.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/ListeDesControleursVehiculesLourdsSelonRegion

        public ActionResult d_edition_controleur_pl_region(StatistiquesModel model)
        {
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            IList<ItemListeDesControleurs> liste = this._controleurStatistiqueService.listeDesControleursVehiculesLourdsSelonRegion(region);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_controleur_pl.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/ListeDesControleursVehiculesLegersSelonRegion

        public ActionResult d_edition_controleur_vl_region(StatistiquesModel model)
        {
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            IList<ItemListeDesControleurs> liste = this._controleurStatistiqueService.listeDesControleursVehiculesLegersSelonRegion(region);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_controleur_vl.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/NombreDeControleursVehiculesLegersEtLourdsSelonRegion

        public ActionResult d_edition_nb_controleur(StatistiquesModel model)
        {
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);
            IList<ItemNombreDeControleurs> liste = this._controleurStatistiqueService.NombreDeControleursVehiculesLegersEtLourdsSelonRegion(region, prefecture);
            if (StatistiquesModel.FormatEditionEnum.HTML.Equals(model.FormatEdition))
            {
                return View(liste);
            }
            else
            {
                if (StatistiquesModel.FormatEditionEnum.XLS.Equals(model.FormatEdition))
                {
                    string xlsOut = System.Net.WebUtility.HtmlDecode(this.RenderViewToString(liste));
                    //System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding(true);
                    System.Text.Encoding encoding = System.Text.Encoding.Default;

                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Nombre_controleur_vlpl.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/NombreInstallationVehiculesLegersSelonRegion

        public ActionResult d_edition_nb_installation_vl(StatistiquesModel model)
        {
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);
            IList<ItemNombreInstallations> liste = this._centreStatistiqueService.NombreInstallationsVehiculesLegersSelonRegion(region, prefecture);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Nombre_installation_vl.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/NombreInstallationVehiculesLourdsSelonRegion

        public ActionResult d_edition_nb_installation_pl(StatistiquesModel model)
        {
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);
            IList<ItemNombreInstallations> liste = this._centreStatistiqueService.NombreInstallationsVehiculesLourdsSelonRegion(region, prefecture);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Nombre_installation_pl.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/ListeDesControleursRattachesAuxCentresDuReseau

        public ActionResult d_edition_excel_cntlr_vlpl_nat_reseau(StatistiquesModel model)
        {
            rUtilisateur utilisateur = this._utilisateurService.FindBy(this.User.Identity.Name);

            IList<ItemListeDesControleursRattachesAuxCentresDuReseau> liste = this._controleurStatistiqueService.listeDesControleursRattachesAuxCentresDuReseau(utilisateur);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_excel_cntlr_vlpl_nat_reseau.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/ItemListeDesInstallationsVehiculesLegersSelonRegion

        public ActionResult d_edition_installation_vl_region(StatistiquesModel model)
        {
            //Todo : selon département aussi !!!
            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);
            IList<ItemListeDesInstallationsVehiculesLegersSelonRegion> liste = this._centreStatistiqueService.listeDesInstallationsVehiculesLegersSelonRegion(region, prefecture);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_installation_vl_region.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/listeDesControleursVLCentreRattachementSelonDepartement

        public ActionResult d_edition_controleur_vl_cntr_dep(StatistiquesModel model)
        {

            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);

            IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> liste = this._controleurStatistiqueService.listeDesControleursVLCentreRattachementSelonDepartement(prefecture);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_controleur_vl_cntr_dep.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/listeDesControleursVLCentreRattachementSelonRegion

        public ActionResult d_edition_controleur_vl_cntr_region(StatistiquesModel model)
        {

            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);

            IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> liste = this._controleurStatistiqueService.listeDesControleursVLCentreRattachementSelonRegion(region, prefecture);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_controleur_vl_cntr_Reg.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/listeDesControleursPLCentreRattachementSelonDepartement

        public ActionResult d_edition_controleur_pl_cntr_dep(StatistiquesModel model)
        {

            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);

            IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> liste = this._controleurStatistiqueService.listeDesControleursPLCentreRattachementSelonDepartement(prefecture);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_controleur_pl_cntr_dep.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

        //
        // GET: /Statistiques/listeDesControleursPLCentreRattachementSelonRegionDepartement

        public ActionResult d_edition_controleur_pl_cntr_region(StatistiquesModel model)
        {

            rRegion region = this._regionService.FindBy(model.RegionSelected.Id);
            rPrefecture prefecture = this._prefectureService.FindBy(model.PrefectureSelected.Id);

            IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> liste = this._controleurStatistiqueService.listeDesControleursPLCentreRattachementSelonRegionDepartement(region, prefecture);
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
                    return File(encoding.GetBytes(xlsOut), "application/octet-stream", "Edition_controleur_pl_cntr_reg.csv");
                }
                else
                {
                    return ErrorHelper.HandleError(new Exception("Invalid format"), this.ControllerContext, "Error");
                }
            }
        }

    }
}
