using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.Mvc;

using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Pv.UI.Web.Models;
using Utac.Rnc2.Pv.UI.Web.Utils;
using Utac.Rnc2.UI.Web.Controllers;
using Utac.Rnc2.UI.Web.Enums;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.ServiceRnc2;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.Pv.UI.Web.Controllers
{
    public class TraceRechPvVlController : RestrictedAcessController
    {
        private readonly IpTraceRechPvVlService _pTraceRechPvVlService;

        public TraceRechPvVlController(IpTraceRechPvVlService pTraceRechPvVlService)
        {
            this._pTraceRechPvVlService = pTraceRechPvVlService;
        }

        public TraceRechPvVlController()
            : this(
                   NhibernateServiceFactory.getInstance().getpTraceRechPvVlService(true)
               )
        {
        }

        // GET: /TraceRechPvVl/Ajouter
        //public ActionResult Ajouter()
        //{
        //    System.Diagnostics.Debug.WriteLine("TraceRechPvVlController.Ajouter()");
        //    return View();
        //}

        // POST: /TraceRechPvVl/Ajouter
        //[HttpPost]
        //public ActionResult Ajouter(pTraceRechPvVl t)
        //{
        //    //25/04/2019 : Suppression de l'URL sur l'ancien PV
        //    return View();
        //    try
        //    {
        //        validateAjouterTrace(t, PvCtWebUtil.EnumRequestTarget.VL);
        //        if (ModelState.IsValid)
        //        {
        //            //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
        //            //pTraceRechPvVl p = new pTraceRechPvVl();
        //            //p = this._pTraceRechPvVlService.nouvelleTrace(t, this.User.Identity.Name);

        //            //Téléchargement du PV
        //            try
        //            {
        //                MemoryStream mstr = null;
        //                PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;
        //                //pTraceRechPvVl.CodeTypeRechercheCT niveauDetail;
        //                PvCtWebUtil.EnumCodeRetourPVCT codRetour;
        //                PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));

        //                //niveauDetail = PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
        //                niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)t.TypeRecherche;

        //                codRetour = pvCtWebUtil.recupereCtvlPV(t.NumPvCtrl, t.CodAgremCentre, Convert.ToDateTime(t.DateCtrl), niveauDetail, ref mstr);

        //                if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_CL))
        //                {
        //                    //On enregistre la demande avec le code résultat
        //                    t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_CL;
        //                     this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

        //                    return File(mstr.GetBuffer(), "application/pdf", "otc_verification_pv_ctvl_" + t.CodAgremCentre + "_" + t.NumPvCtrl + ".pdf");
        //                }
        //                else
        //                {
        //                    if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT))
        //                    {
        //                        //On enregistre la demande avec le code résultat
        //                        t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT;
        //                        //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
        //                        //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
        //                        this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

        //                        ViewData["message"] = "Le contrôle technique n'a pas été trouvé dans la base de l'OTC. Veuillez vérifier la saisie de vos critères de recherche.";

        //                        return View("Ajouter");
        //                    }
        //                    else
        //                    {
        //                        if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE))
        //                        {
        //                            //On enregistre la demande avec le code résultat
        //                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE;
        //                            //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
        //                            //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
        //                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

        //                            ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";

        //                            return View("Ajouter");
        //                        }
        //                    }
        //                }

        //                throw new NotImplementedException();
        //            }
        //            catch (Exception e)
        //            {
        //                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //            }

        //            ////On affiche le formulaire vide pour saisir un nouveau contrôle
        //            //return this.RedirectToAction("Ajouter");
        //        }

        //        return View("Ajouter", t);
        //    }
        //    catch (Exception e)
        //    {
        //        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
        //    }
        //}

        // GET: /TraceRechPvVl/TelechargerPvVl
        public ActionResult TelechargerPvVl()
        {
            System.Diagnostics.Debug.WriteLine("TraceRechPvVlController.TelechargerPvVl()");
            return View();
        }

        // POST: /TraceRechPvVl/TelechargerPvVl
        [HttpPost]
        public ActionResult TelechargerPvVl(pTraceRechPvVl t)
        {
            try
            {
                validateAjouterTrace(t, PvCtWebUtil.EnumRequestTarget.VL);
                if (ModelState.IsValid)
                {
                    //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                    //pTraceRechPvVl p = new pTraceRechPvVl();
                    //p = this._pTraceRechPvVlService.nouvelleTrace(t, this.User.Identity.Name);

                    //Téléchargement du PV
                    try
                    {
                        MemoryStream mstr = null;
                        PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;
                        //pTraceRechPvVl.CodeTypeRechercheCT niveauDetail;
                        PvCtWebUtil.EnumCodeRetourPVCT codRetour;
                        PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));

                        //niveauDetail = PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
                        niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)t.TypeRecherche;

                        codRetour = pvCtWebUtil.NewRecupereCtvlPV(t.NumPvCtrl, t.CodAgremCentre, Convert.ToDateTime(t.DateCtrl), niveauDetail, ref mstr);

                        if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT))
                        {
                            //On enregistre la demande avec le code résultat
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT;
                            //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                            //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                            return File(mstr.GetBuffer(), "application/pdf", "otc_verification_pv_ctvl_" + t.CodAgremCentre + "_" + t.NumPvCtrl + ".pdf");
                        }
                        else
                        {
                            if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT))
                            {
                                //On enregistre la demande avec le code résultat
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT;
                                //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                                //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                                this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                                ViewData["message"] = "Le contrôle technique n'a pas été trouvé dans la base de l'OTC. Veuillez vérifier la saisie de vos critères de recherche.";

                                return View("TelechargerPvVl");
                            }
                            else
                            {
                                if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE))
                                {
                                    //On enregistre la demande avec le code résultat
                                    t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE;
                                    //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                                    //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                                    this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                                    ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";

                                    return View("TelechargerPvVl");
                                }
                            }
                        }

                        throw new NotImplementedException();
                    }
                    catch (Exception e)
                    {
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }

                    ////On affiche le formulaire vide pour saisir un nouveau contrôle
                    //return this.RedirectToAction("Ajouter");
                }

                return View("TelechargerPvVl", t);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /TraceRechPvVl/TelechargerPvCl
        public ActionResult TelechargerPvCl()
        {
            System.Diagnostics.Debug.WriteLine("TraceRechPvClController.TelechargerPvCl()");
            return View();
        }

        // POST: /TraceRechPvVl/TelechargerPvCl
        [HttpPost]
        public ActionResult TelechargerPvCl(pTraceRechPvVl t)
        {
            try
            {
                validateAjouterTrace(t, PvCtWebUtil.EnumRequestTarget.CL);
                if (ModelState.IsValid)
                {

                    //Téléchargement du PV
                    try
                    {
                        MemoryStream mstr = null;
                        PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;
                        //pTraceRechPvVl.CodeTypeRechercheCT niveauDetail;
                        PvCtWebUtil.EnumCodeRetourPVCT codRetour;
                        PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));

                        //niveauDetail = PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
                        niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)t.TypeRecherche;

                        codRetour = pvCtWebUtil.RecupereCtclPV(t.NumPvCtrl, t.CodAgremCentre, Convert.ToDateTime(t.DateCtrl), niveauDetail, ref mstr);

                        if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_CL))
                        {
                            //On enregistre la demande avec le code résultat
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_CL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                            return File(mstr.GetBuffer(), "application/pdf", "otc_verification_pv_ctcl_" + t.CodAgremCentre + "_" + t.NumPvCtrl + ".pdf");
                        }
                        else
                        {
                            if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_CL))
                            {
                                //On enregistre la demande avec le code résultat
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_CL;
                                //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                                //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                                this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                                ViewData["message"] = "Le contrôle technique n'a pas été trouvé dans la base de l'OTC. Veuillez vérifier la saisie de vos critères de recherche.";

                                return View("TelechargerPvCl");
                            }
                            else
                            {
                                if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL))
                                {
                                    //On enregistre la demande avec le code résultat
                                    t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_CL;
                                    //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                                    //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                                    this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                                    ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";

                                    return View("TelechargerPvCl");
                                }
                            }
                        }

                        throw new NotImplementedException();
                    }
                    catch (Exception e)
                    {
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }

                    ////On affiche le formulaire vide pour saisir un nouveau contrôle
                    //return this.RedirectToAction("Ajouter");
                }

                return View("TelechargerPvCl", t);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }


        // GET: /TraceRechPvVl/TelechargerPvPl
        public ActionResult TelechargerPvPl()
        {
            System.Diagnostics.Debug.WriteLine("TraceRechPvPlController.TelechargerPvVl()");
            return View();
        }

        // POST: /TraceRechPvVl/TelechargerPvPl
        [HttpPost]
        public ActionResult TelechargerPvPl(pTraceRechPvVl t)
        {
            try
            {
                validateAjouterTrace(t, PvCtWebUtil.EnumRequestTarget.PL);
                if (ModelState.IsValid)
                {
                    //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                    //pTraceRechPvVl p = new pTraceRechPvVl();
                    //p = this._pTraceRechPvVlService.nouvelleTrace(t, this.User.Identity.Name);

                    //Téléchargement du PV
                    try
                    {
                        MemoryStream mstr = null;
                        PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;
                        //pTraceRechPvVl.CodeTypeRechercheCT niveauDetail;
                        PvCtWebUtil.EnumCodeRetourPVCT codRetour;
                        PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));

                        //niveauDetail = PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
                        niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)t.TypeRecherche;

                        codRetour = pvCtWebUtil.RecupereCtplPV(t.NumPvCtrl, t.CodAgremCentre, Convert.ToDateTime(t.DateCtrl), niveauDetail, ref mstr);

                        if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL))
                        {
                            //On enregistre la demande avec le code résultat
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_PL;
                            //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                            //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                            return File(mstr.GetBuffer(), "application/pdf", "otc_verification_pv_ctpl_" + t.CodAgremCentre + "_" + t.NumPvCtrl + ".pdf");
                        }
                        else
                        {
                            if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL))
                            {
                                //On enregistre la demande avec le code résultat
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_PL;
                                //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                                //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                                this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                                ViewData["message"] = "Le contrôle technique n'a pas été trouvé dans la base de l'OTC. Veuillez vérifier la saisie de vos critères de recherche.";

                                return View("TelechargerPvPl");
                            }
                            else
                            {
                                if (codRetour.Equals(PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL))
                                {
                                    //On enregistre la demande avec le code résultat
                                    t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_PL;
                                    //PLE 15/04/2014 supression de cette demande car erreur de synchronisation
                                    //p = this._pTraceRechPvVlService.modifierTrace(p.Id, t, this.User.Identity.Name);
                                    this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);

                                    ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";

                                    return View("TelechargerPvPl");
                                }
                            }
                        }

                        throw new NotImplementedException();
                    }
                    catch (Exception e)
                    {
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }

                    ////On affiche le formulaire vide pour saisir un nouveau contrôle
                    //return this.RedirectToAction("Ajouter");
                }

                return View("TelechargerPvPl", t);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //private void ClearErrors()
        //{
        //    if (ModelState.ContainsKey("NumPvCtrl")) ModelState["NumPvCtrl"].Errors.Clear();
        //    if (ModelState.ContainsKey("CodAgremCentre")) ModelState["CodAgremCentre"].Errors.Clear();
        //    if (ModelState.ContainsKey("DateCtrl")) ModelState["DateCtrl"].Errors.Clear();
        //}

        private void validateAjouterTrace(pTraceRechPvVl t, PvCtWebUtil.EnumRequestTarget tg)
        {
            string pattern;

            if (t.NumPvCtrl == null)
            {
                ModelState.AddModelError("NumPvCtrl", "Le n° du procès verbal doit être renseigné.");
            }
            else
            {
                pattern = @"[0-9]{8}";
                if (!Regex.IsMatch(t.NumPvCtrl, pattern)) // retourne true ou false selon la vérification
                {
                    ModelState.AddModelError("NumPvCtrl", "Le format du n° de PV est incorrect (8 caractères numériques).");
                }
            }

            if (t.CodAgremCentre == null)
            {
                ModelState.AddModelError("CodAgremCentre", "Le n° d'agrément de l'installation de contrôle doit être renseigné.");
            }
            else
            {
                bool bSkip = false;
                //PLE 31/07/2014 : passage en majuscule du code agrment centre.
                t.CodAgremCentre = t.CodAgremCentre.ToUpper();
                switch (tg)
                {
                    case PvCtWebUtil.EnumRequestTarget.VL:
                        //pattern = @"(A|S)[A-B0-9]{3}(C|D|F|S|T|V|Z)[0-9]{3}";
                        pattern = StringValidator.CodeAgrementCentreVlPattern;
                        break;
                    //10/05/2024 PLE Catégorie L
                    case PvCtWebUtil.EnumRequestTarget.CL:
                        //pattern = @"(S|L)[A-B0-9]{3}(C|D|F|S|T|V|Z)[0-9]{3}";
                        pattern = StringValidator.CodeAgrementCentreClPattern;
                        break;
                    case PvCtWebUtil.EnumRequestTarget.PL:
                        //pattern = @"(A|S)[A-B0-9]{3}(J|K|Z)[0-9]{3}";
                        pattern = StringValidator.CodeAgrementCentrePlPattern;
                        break;
                    default:
                        pattern = @"";
                        ModelState.AddModelError("CodAgremCentre", "Le type de requête ne permet pas de valider le format du numéro d'agrément de l'installation.");
                        break;
                }
                if (!bSkip)
                {
                    if (!Regex.IsMatch(t.CodAgremCentre, pattern)) // retourne true ou false selon la vérification
                    {
                        ModelState.AddModelError("CodAgremCentre", "Le format du numéro d'agrément de l'installation " + tg.ToString() + " n'est pas valide.");
                    }
                }
            }

            if ((t.DateCtrl == null || Convert.ToDateTime(t.DateCtrl).Year < System.DateTime.Now.Year - 4 || Convert.ToDateTime(t.DateCtrl).Year > System.DateTime.Now.Year))
            {
                ModelState.AddModelError("DateCtrl", "La date du contrôle est en dehors de la période autorisée (année en cours et 4 années précédentes uniquement).");
            }

            if (t.TypeRecherche != (int)pTraceRechPvVl.CodeTypeRechercheCT.TOUT && t.TypeRecherche != (int)pTraceRechPvVl.CodeTypeRechercheCT.REDUIT)
            {
                ModelState.AddModelError("TypeRecherche", "Le type de recherche n'est pas renseigné.");
            }
        }

        // GET: /TraceRechPvVl/Ajouter
        public ActionResult SuiviTelechargementPV(string deb, string fin)
        {
            if (GestionDroitsExecution.isRightAfficheTelechergePV(this.User))
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

                IList<pTraceRechPvVl> liste = this._pTraceRechPvVlService.FindAllByDate(dateDebut, dateFin);
                return View(liste);
            }
            else
            {
                Exception e = new Exception("Vous n'êtes pas autorisé....");
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // GET: /TraceRechPvVl/RechercheImmatNumserie
        public ActionResult RechercheImmatNumserie()
        {
            System.Diagnostics.Debug.WriteLine("TraceRechPvPlController.RechercheParImmatSerie()");
            DateTime dateLimit = new DateTime(DateTime.Now.Year - 4, 1, 1);
            RechercheImmatSerieModel model = new RechercheImmatSerieModel(CategorieVehicule.VL, NiveauDetails.Summary, dateLimit);
            return View(model);
        }

        // POST: /TraceRechPvVl/RechercheImmatNumserie
        [HttpPost]
        public ActionResult RechercheImmatNumserie(RechercheImmatSerieModel form)
        {
            try
            {
                form.Categorie = CategorieVehicule.enumValueOf(form.CategorieValue);
                form.Details = NiveauDetails.enumValueOf(form.DetailValue);
                this.validateAjouterTrace(form);
                if (ModelState.IsValid)
                {
                    //Téléchargement du PV
                    try
                    {
                        WebServiceResult res;
                        PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;
                        PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));
                        niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)(form.Details == NiveauDetails.Full ? PvCtWebUtil.EnumNiveauDetailPVCT.TOUT : PvCtWebUtil.EnumNiveauDetailPVCT.REDUIT);
                        form.ResetLists();
                        pTraceRechPvVl t = new pTraceRechPvVl();
                        t.NumPvCtrl = form.NumberPlate;
                        t.CodAgremCentre = form.VinNumber;
                        switch (niveauDetail)
                        {
                            case PvCtWebUtil.EnumNiveauDetailPVCT.REDUIT:
                                t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.REDUIT;
                                break;
                            case PvCtWebUtil.EnumNiveauDetailPVCT.TOUT:
                                t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.TOUT;
                                break;
                            default:
                                t.TypeRecherche = (byte)niveauDetail;
                                break;
                        }
                        switch (form.Categorie.InnerEnumValue)
                        {
                            case CategorieVehicule.InnerEnum.VL:
                            case CategorieVehicule.InnerEnum.PL:
                            case CategorieVehicule.InnerEnum.CL:
                                res = pvCtWebUtil.Ws_FindByImmatOrNumSerie(form.NumberPlate, form.VinNumber, form.Categorie);
                                break;
                            default:
                                res = new WebServiceResult();
                                break;
                        }
                        switch (res.ResultCode)
                        {
                            case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT:  //Found VL filled list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT;
                                form.ControlesVL = (IList<VlPControle>)res.ResultValue;
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                ViewBag.ScrollTo = "#list";
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL:  //Found PL
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_PL;
                                form.ControlesPL = (IList<PlPControle>)res.ResultValue;
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                ViewBag.ScrollTo = "#list";
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_CL:  //Found PL
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_CL;
                                form.ControlesCL = (IList<ClPcontrole>)res.ResultValue;
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                ViewBag.ScrollTo = "#list";
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT:  //VL not found, Empty list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT;
                                ViewData["message"] = "Il n'y a pas de contrôle technique dans la base VL de l'OTC répondant aux critères de recherche. Veuillez vérifier la saisie de vos critères de recherche.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL:  //PL not found, Empty list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_PL;
                                ViewData["message"] = "Il n'y a pas de contrôle technique dans la base PL de l'OTC répondant aux critères de recherche. Veuillez vérifier la saisie de vos critères de recherche.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_CL:  //PL not found, Empty list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_CL;
                                ViewData["message"] = "Il n'y a pas de contrôle technique dans la base CL de l'OTC répondant aux critères de recherche. Veuillez vérifier la saisie de vos critères de recherche.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE:
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE;
                                ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL:
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_PL;
                                ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL:
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_CL;
                                ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            default:
                                break;
                        }
                        throw new NotImplementedException();
                    }
                    catch (Exception e)
                    {
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }
                return View(form);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        public ActionResult DownloadPV(string ep, string l)
        {
            try
            {
                NameValueCollection nc = RechercheImmatSerieModel.DecodeParams(ep);
                string cat = nc.Get("cat");
                long numVisit = long.Parse(nc.Get("num_visit"));
                int origine = int.Parse(nc.Get("origine"));
                PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)NiveauDetails.enumValueOf(l).CodeTypeRecherche;
                System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV({0}, {1}, {2}, {3})", cat, numVisit.ToString(), origine.ToString(), niveauDetail.ToString()));

                MemoryStream mstr = null;
                //pTraceRechPvVl.CodeTypeRechercheCT niveauDetail;
                PvCtWebUtil.EnumCodeRetourPVCT codRetour;
                PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));

                CategorieVehicule catVeh = CategorieVehicule.enumValueOf(cat);

                pTraceRechPvVl t = new pTraceRechPvVl();
                switch (niveauDetail)
                {
                    case PvCtWebUtil.EnumNiveauDetailPVCT.REDUIT:
                        t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.REDUIT;
                        break;
                    case PvCtWebUtil.EnumNiveauDetailPVCT.TOUT:
                        t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.TOUT;
                        break;
                    default:
                        t.TypeRecherche = (byte)niveauDetail;
                        break;
                }

                codRetour = PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE;//TO BE REMOVED

                String id1 = "";
                String id2 = "";
                DateTime dc = new DateTime();
                if (catVeh.InnerEnumValue == CategorieVehicule.VL.InnerEnumValue)
                {
                    codRetour = pvCtWebUtil.GetCtvlPV(numVisit, origine, niveauDetail, ref mstr, ref id1, ref id2, ref dc);
                    System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV, VL : code retour", codRetour.ToString()));
                    switch (codRetour)
                    {
                        case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT:
                            //On enregistre la demande avec le code résultat
                            t.NumPvCtrl = id1;
                            t.CodAgremCentre = id2;
                            t.DateCtrl = dc;
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return File(mstr.GetBuffer(), "application/pdf", String.Format("otc_verification_pv_ctvl_{0}_{1}.pdf", id1, id2));
                        case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT://Is it possible?
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Document VL non trouvé({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                        case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE:
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Erreur technique VL({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                    }
                }
                else if (catVeh.InnerEnumValue == CategorieVehicule.PL.InnerEnumValue)
                {
                    codRetour = pvCtWebUtil.GetCtplPV(numVisit, origine, niveauDetail, ref mstr, ref id1, ref id2, ref dc);
                    System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV, PL : code retour", codRetour.ToString()));
                    switch (codRetour)
                    {
                        case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL:
                            //On enregistre la demande avec le code résultat
                            t.NumPvCtrl = id1;
                            t.CodAgremCentre = id2;
                            t.DateCtrl = dc;
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_PL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return File(mstr.GetBuffer(), "application/pdf", String.Format("otc_verification_pv_ctpl_{0}_{1}.pdf", id1, id2));
                        case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL://Is it possible?
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_PL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Document PL non trouvé({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                        case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL:
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_PL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Erreur technique PL({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                    }
                }
                else if (catVeh.InnerEnumValue == CategorieVehicule.CL.InnerEnumValue)
                {
                    codRetour = pvCtWebUtil.GetCtclPV(numVisit, origine, niveauDetail, ref mstr, ref id1, ref id2, ref dc);
                    System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV, CL : code retour", codRetour.ToString()));
                    switch (codRetour)
                    {
                        case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_CL:
                            //On enregistre la demande avec le code résultat
                            t.NumPvCtrl = id1;
                            t.CodAgremCentre = id2;
                            t.DateCtrl = dc;
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_CL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return File(mstr.GetBuffer(), "application/pdf", String.Format("otc_verification_pv_ctcl_{0}_{1}.pdf", id1, id2));
                        case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_CL://Is it possible?
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_CL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Document CL non trouvé({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                        case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL:
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_CL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Erreur technique CL({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                    }
                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        public ActionResult DownloadSasPV(string ep, string l)
        {
            try
            {
                NameValueCollection nc = RechercheImmatSerieModel.DecodeSASVayaParams(ep);
                //02/07/2024 PLE
                //ep = "cat=CL&num_visit=1401&origine=1";
                //NameValueCollection nc = RechercheImmatSerieModel.NonDecodeSASVayaParams(ep);
                
                string cat = nc.Get("cat");
                long numVisit = long.Parse(nc.Get("num_visit"));
                int origine = int.Parse(nc.Get("origine"));
                PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)NiveauDetails.enumValueOf(l).CodeTypeRecherche;
                System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV({0}, {1}, {2}, {3})", cat, numVisit.ToString(), origine.ToString(), niveauDetail.ToString()));

                MemoryStream mstr = null;
                //pTraceRechPvVl.CodeTypeRechercheCT niveauDetail;
                PvCtWebUtil.EnumCodeRetourPVCT codRetour;
                PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));

                CategorieVehicule catVeh = CategorieVehicule.enumValueOf(cat);

                pTraceRechPvVl t = new pTraceRechPvVl();
                switch (niveauDetail)
                {
                    case PvCtWebUtil.EnumNiveauDetailPVCT.REDUIT:
                        t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.REDUIT;
                        break;
                    case PvCtWebUtil.EnumNiveauDetailPVCT.TOUT:
                        t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.TOUT;
                        break;
                    default:
                        t.TypeRecherche = (byte)niveauDetail;
                        break;
                }

                codRetour = PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE;//TO BE REMOVED

                String id1 = "";
                String id2 = "";
                DateTime dc = new DateTime();
                if (catVeh.InnerEnumValue == CategorieVehicule.VL.InnerEnumValue)
                {
                    codRetour = pvCtWebUtil.GetCtvlPV(numVisit, origine, niveauDetail, ref mstr, ref id1, ref id2, ref dc);
                    System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV, VL : code retour", codRetour.ToString()));
                    switch (codRetour)
                    {
                        case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT:
                            //On enregistre la demande avec le code résultat
                            t.NumPvCtrl = id1;
                            t.CodAgremCentre = id2;
                            t.DateCtrl = dc;
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return File(mstr.GetBuffer(), "application/pdf", String.Format("otc_verification_pv_ctvl_{0}_{1}.pdf", id1, id2));
                        case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT://Is it possible?
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Document VL non trouvé({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                        case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE:
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Erreur technique VL({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                    }
                }
                else if (catVeh.InnerEnumValue == CategorieVehicule.PL.InnerEnumValue)
                {
                    codRetour = pvCtWebUtil.GetCtplPV(numVisit, origine, niveauDetail, ref mstr, ref id1, ref id2, ref dc);
                    System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV, PL : code retour", codRetour.ToString()));
                    switch (codRetour)
                    {
                        case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL:
                            //On enregistre la demande avec le code résultat
                            t.NumPvCtrl = id1;
                            t.CodAgremCentre = id2;
                            t.DateCtrl = dc;
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_PL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return File(mstr.GetBuffer(), "application/pdf", String.Format("otc_verification_pv_ctpl_{0}_{1}.pdf", id1, id2));
                        case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL://Is it possible?
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_PL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Document PL non trouvé({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                        case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL:
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_PL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Erreur technique PL({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                    }
                }
                else if (catVeh.InnerEnumValue == CategorieVehicule.CL.InnerEnumValue)
                {
                    codRetour = pvCtWebUtil.GetCtclPV(numVisit, origine, niveauDetail, ref mstr, ref id1, ref id2, ref dc);
                    System.Diagnostics.Debug.WriteLine(String.Format("DownloadPV, CL : code retour", codRetour.ToString()));
                    switch (codRetour)
                    {
                        case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_CL:
                            //On enregistre la demande avec le code résultat
                            t.NumPvCtrl = id1;
                            t.CodAgremCentre = id2;
                            t.DateCtrl = dc;
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_CL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return File(mstr.GetBuffer(), "application/pdf", String.Format("otc_verification_pv_ctcl_{0}_{1}.pdf", id1, id2));
                        case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_CL://Is it possible?
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_CL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Document CL non trouvé({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                        case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL:
                            t.NumPvCtrl = nc.Get("num_visit");
                            t.CodAgremCentre = nc.Get("origine");
                            t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_CL;
                            this._pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                            return ErrorHelper.HandleError(null, this.ControllerContext, String.Format("Erreur technique CL({0}, {1})", t.NumPvCtrl, t.CodAgremCentre));
                    }
                }

                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        private void validateAjouterTrace(RechercheImmatSerieModel m)
        {

            if (String.IsNullOrEmpty(m.NumberPlate) && String.IsNullOrEmpty(m.VinNumber))
            {
                ModelState.AddModelError("Immatriculation", "L'immatriculation et/ou le numéro de série doivent être renseignés.");
                ModelState.AddModelError("NumeroSerie", "L'immatriculation et/ou le numéro de série doivent être renseignés.");
            }
            else
            {
                if (m.Categorie.InnerEnumValue.Equals(CategorieVehicule.InnerEnum.CL))
                {
                    if (!String.IsNullOrEmpty(m.NumberPlate) && !(StringValidator.IsNumberPlateMatch(m.NumberPlate) || StringValidator.IsFniCatgorieLNumberPlateMatch(m.NumberPlate)))
                    {
                        ModelState.AddModelError("Immatriculation", "Non respect du format CL : AA-111-AA ou 1111 AAA 11 ou A 99 A ou A 999 A ou AA 99 A ou AA 999 A");
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(m.NumberPlate) && !StringValidator.IsNumberPlateMatch(m.NumberPlate))
                    {
                        ModelState.AddModelError("Immatriculation", "Non respect du format AA-111-AA ou 1111 AAA 11");
                    }
                }

                if (!String.IsNullOrEmpty(m.VinNumber) && !StringValidator.IsVinNumberMatch(m.VinNumber))
                {
                    ModelState.AddModelError("NumeroSerie", "VIN incorrect");
                }
            }

        }

        // GET: /TraceRechPvVl/RechercheCodeCentreControleur
        public ActionResult RechercheCodeCentreControleur()
        {
            System.Diagnostics.Debug.WriteLine("TraceRechPvPlController.RechercheCodeCentreControleur()");
            RechercheCodeCentreControleurModel model = new RechercheCodeCentreControleurModel(CategorieVehicule.VL, NiveauDetails.Summary);
            return View(model);
        }

        // POST: /TraceRechPvVl/RechercheCodeCentreControleur
        [HttpPost]
        public ActionResult RechercheCodeCentreControleur(RechercheCodeCentreControleurModel form)
        {
            try
            {
                form.Categorie = CategorieVehicule.enumValueOf(form.CategorieValue);
                form.Details = NiveauDetails.enumValueOf(form.DetailValue);
                this.validateAjouterTrace(form);
                if (ModelState.IsValid)
                {
                    //Recherche liste PV
                    try
                    {
                        WebServiceResult res;
                        PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;
                        PvCtWebUtil pvCtWebUtil = new PvCtWebUtil(Server.MapPath("~/bin"));
                        niveauDetail = (PvCtWebUtil.EnumNiveauDetailPVCT)(form.Details == NiveauDetails.Full ? PvCtWebUtil.EnumNiveauDetailPVCT.TOUT : PvCtWebUtil.EnumNiveauDetailPVCT.REDUIT);
                        form.ResetLists();
                        pTraceRechPvVl t = new pTraceRechPvVl();
                        t.NumPvCtrl = form.ControllerApprovalCode;
                        t.CodAgremCentre = form.CenterApprovalCode;
                        t.DateCtrl = new DateTime(form.DateVisiteDebut.Year, form.DateVisiteDebut.Month, form.DateVisiteDebut.Day, 0, (form.DateVisiteFin - form.DateVisiteDebut).Days, 0);
                        switch (niveauDetail)
                        {
                            case PvCtWebUtil.EnumNiveauDetailPVCT.REDUIT:
                                t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.REDUIT;
                                break;
                            case PvCtWebUtil.EnumNiveauDetailPVCT.TOUT:
                                t.TypeRecherche = (byte)pTraceRechPvVl.CodeTypeRechercheCT.TOUT;
                                break;
                            default:
                                t.TypeRecherche = (byte)niveauDetail;
                                break;
                        }
                        switch (form.Categorie.InnerEnumValue)
                        {
                            case CategorieVehicule.InnerEnum.VL:
                            case CategorieVehicule.InnerEnum.PL:
                            case CategorieVehicule.InnerEnum.CL:
                                res = pvCtWebUtil.Ws_FindByCodAgrCentreControleur(form.CenterApprovalCode, form.ControllerApprovalCode, form.DateVisiteDebut, form.DateVisiteFin, form.Categorie);
                                break;
                            default:
                                res = new WebServiceResult();
                                break;
                        }
                        switch (res.ResultCode)
                        {
                            case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT:  //Found VL filled list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT;
                                form.ControlesVL = (IList<VlPControle>)res.ResultValue;
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                ViewBag.ScrollTo = "#list";
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL:  //Found PL
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_PL;
                                form.ControlesPL = (IList<PlPControle>)res.ResultValue;
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                ViewBag.ScrollTo = "#list";
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_CL:  //Found CL
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.TROUVE_CT_CL;
                                form.ControlesCL = (IList<ClPcontrole>)res.ResultValue;
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                ViewBag.ScrollTo = "#list";
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT:  //VL not found, Empty list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT;
                                ViewData["message"] = "Il n'y a pas de contrôle technique dans la base de l'OTC répondant aux critères de recherche. Veuillez vérifier la saisie de vos critères de recherche.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL:  //PL not found, Empty list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_PL;
                                ViewData["message"] = "Il n'y a pas de contrôle technique dans la base de l'OTC répondant aux critères de recherche. Veuillez vérifier la saisie de vos critères de recherche.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_CL:  //CL not found, Empty list
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.AUCUN_CT_CL;
                                ViewData["message"] = "Il n'y a pas de contrôle technique dans la base de l'OTC répondant aux critères de recherche. Veuillez vérifier la saisie de vos critères de recherche.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE:
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE;
                                ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL:
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_PL;
                                ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            case PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL:
                                t.CodResultat = (int)pTraceRechPvVl.CodeResultatRechercheCT.ERREUR_TECHNIQUE_CL;
                                ViewData["message"] = "Une erreur technique s'est produite durant la récupération des informations. Veuillez réessayer ultérieurement.";
                                _pTraceRechPvVlService.TracerAppel(t, this.User.Identity.Name);
                                return View(form);
                            default:
                                break;
                        }
                        throw new NotImplementedException();
                    }
                    catch (Exception e)
                    {
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }
                return View(form);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private int GetDateAgeInYears(DateTime d)
        {
            DateTime now = DateTime.Now;
            if (now > d)
            {
                if (now.Month < d.Month || (now.Month == d.Month && now.Day < d.Day))
                    return (now.Year - d.Year) - 1;
                else
                    return now.Year - d.Year;
            }
            else
                return -1;
        }

        private void validateAjouterTrace(RechercheCodeCentreControleurModel m)
        {

            if (String.IsNullOrEmpty(m.CenterApprovalCode) && String.IsNullOrEmpty(m.ControllerApprovalCode))
            {
                ModelState.AddModelError("CodeAgrementCentre", "Le numéro d'agrément du centre et/ou le numéro d'agrément du contrôleur doivent être renseignés.");
                ModelState.AddModelError("CodeAgrementControleur", "Le numéro d'agrément du centre et/ou le numéro d'agrément du contrôleur doivent être renseignés.");
            }
            else
            {
                if (!String.IsNullOrEmpty(m.CenterApprovalCode) && !StringValidator.IsCenterApprovalCodeMatch(m.CenterApprovalCode, m.CategorieValue))
                {
                    ModelState.AddModelError("CodeAgrementCentre", "Numéro d'agrément centre erronné");
                }
                if (!String.IsNullOrEmpty(m.ControllerApprovalCode) && !StringValidator.IsControllerApprovalCodeMatch(m.ControllerApprovalCode, m.CategorieValue))
                {
                    ModelState.AddModelError("CodeAgrementControleur", "Numéro d'agrément contrôleur erronné");
                }
            }
            if (m.DateVisiteDebut != null && m.DateVisiteFin != null)
            {
                int d = GetDateAgeInYears(m.DateVisiteDebut);
                if (d > 4)
                    ModelState.AddModelError("DateVisiteDebut", "La valeur est en dehors de la période autorisée (année en cours et 4 années précédentes uniquement).");
                else if (d < 0)
                    ModelState.AddModelError("DateVisiteDebut", "La valeur ne peut pas être postérieure à la date du jour.");
                d = GetDateAgeInYears(m.DateVisiteFin);
                if (d > 4)
                    ModelState.AddModelError("DateVisiteFin", "La valeur est en dehors de la période autorisée (année en cours et 4 années précédentes uniquement).");
                else if (d < 0)
                    ModelState.AddModelError("DateVisiteFin", "La valeur ne peut pas être postérieure à la date du jour.");
                if ((m.DateVisiteFin - m.DateVisiteDebut).Days > 2)
                {
                    ModelState.AddModelError("DateVisiteDebut", "La plage de recherche ne peut pas excéder 3 jours.");
                    ModelState.AddModelError("DateVisiteFin", "La plage de recherche ne peut pas excéder 3 jours.");
                }
                else if (m.DateVisiteFin < m.DateVisiteDebut)
                    ModelState.AddModelError("DateVisiteFin", "La valeur ne peut pas être antérieure à la date de début.");
            }
            else
            {
                if (m.DateVisiteDebut == null) ModelState.AddModelError("DateVisiteDebut", "Valeur obligatoire");
                if (m.DateVisiteFin == null) ModelState.AddModelError("DateVisiteFin", "Valeur obligatoire");
            }
        }

    }
}
