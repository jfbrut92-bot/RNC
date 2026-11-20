using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class RapportCEController : RestrictedAcessController
    {
        readonly IrReseauService _reseauService;
        readonly IpRapportExceptionService _rapportExceptionService;
        readonly IpRapportExceptionVLService _rapportExceptionVLService;
        readonly IpCentreEncourService _centreService;
        readonly IpRapportExceptionCLService _rapportExceptionCLService;

        public RapportCEController(IrReseauService reseauService, IpRapportExceptionService rapportExceptionService, IpRapportExceptionVLService rapportExceptionVLService, IpCentreEncourService centreService, IpRapportExceptionCLService rapportExceptionCLService)
        {
            this._reseauService = reseauService;
            this._rapportExceptionService = rapportExceptionService;
            this._rapportExceptionVLService = rapportExceptionVLService;
            this._rapportExceptionCLService = rapportExceptionCLService;
            this._centreService = centreService;
        }

        public RapportCEController()
            : this(
                    NhibernateServiceFactory.getInstance().getrReseauService(false),
                    NhibernateServiceFactory.getInstance().getpRapportExceptionService(true),
                    NhibernateServiceFactory.getInstance().getpRapportExceptionVLService(true),
                    NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true),
                    NhibernateServiceFactory.getInstance().getpRapportExceptionCLService(true)
              )
        {
        }

        //
        // GET: /RapportCE/ListeAllDispoReseau

        public ActionResult ListeDispoAllReseau()
        {
            if (this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()))
            {
                List<rReseau> listeReseaux = this._reseauService.FindAllReseauPLSansZ();
                if (listeReseaux[0].Id.Equals("A")) listeReseaux.RemoveAt(0);
                return View(listeReseaux);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        public ActionResult ListeDispoAllCNRPL()
        {
            if (this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()))
            {
                IList<pRapportException> listeRapport = this._rapportExceptionService.FindAllForCNR();
                return View(listeRapport);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        //
        // GET: /RapportCE/ListeDispoReseau

        public ActionResult ListeDispoReseau(string id)
        {

            // récupération du réseau
            rReseau reseau = this._reseauService.FindBy(id);

            if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, reseau)) || this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
            {
                reseau.PRapportExceptions = this._rapportExceptionService.FindAllByReseau(id);
                return View(reseau);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        //
        // GET: /RapportCE/ListeDispoCNR

        public ActionResult ListeDispoCNR(int id)
        {

            pCentreEncour c = this._centreService.FindBy(id);
            if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_PL))
            {

                return View(c);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        //
        // GET: /RapportCE/Telecharge

        public ActionResult Telecharge(int id, rCategorieVehicule.CategorieEnum CategVehicule)
        {
            try
            {
                byte[] b = null;
                string nomRapport = "";

                switch (CategVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        {
                            pRapportExceptionVl rapport = this._rapportExceptionVLService.FindBy(id);
                            b = rapport.ImgFichier;
                            nomRapport = rapport.NomRapport;
                            pCentreEncour c = rapport.PCentreEncour;
                            if (c != null)
                            {
                                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_VL))
                                {
                                    //EG le 27/02/2012 si on est le proprio du centre alors on met la date de téléchargement
                                    if (this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                                    {
                                        if (rapport.DateTelechargementRapport == null)
                                        {
                                            this._rapportExceptionVLService.traceTelechargement(id, this.User.Identity.Name);
                                        }
                                    }
                                }
                                else
                                {
                                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                                }
                            }
                            else
                            {
                                Exception e = new Exception("rapport sans centre associé !!!....");
                                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                            }

                        }
                        break;

                    case rCategorieVehicule.CategorieEnum.PL:
                        {
                            pRapportException rapport = this._rapportExceptionService.FindBy(id);
                            b = rapport.ImgFichier;
                            nomRapport = rapport.NomRapport;
                            pCentreEncour c = rapport.PCentreEncour;
                            if (c != null)
                            {
                                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_PL))
                                {
                                    //EG le 27/02/2012 si on est le proprio du centre alors on met la date de téléchargement
                                    if (this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                                    {
                                        if (rapport.DateTelechargementRapport == null)
                                        {
                                            this._rapportExceptionService.traceTelechargement(rapport, this.User.Identity.Name);
                                        }
                                    }
                                }
                                else
                                {
                                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                                }
                            }
                            else
                            {
                                //EG le 01/06/2012 : Ajout du test si réseau (prise en compte du téléchargement CE pour réseau)
                                rReseau reseau = rapport.RReseau;
                                if (reseau != null)
                                {
                                    if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, reseau)) || this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                                    {
                                        if (this.User.IsInRole(rUtilisateur.Profil.RESEAU.ToString()))
                                        {
                                            if (rapport.DateTelechargementRapport == null)
                                            {
                                                this._rapportExceptionService.traceTelechargement(rapport, this.User.Identity.Name);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Exception e = new Exception("Vous n'êtes pas autorisé....");
                                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                                    }
                                }
                                else
                                {

                                    Exception e = new Exception("rapport sans centre ou réseau associé !!!....");
                                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                                }
                            }
                        }
                        break;

                    case rCategorieVehicule.CategorieEnum.CL:
                        {
                            pRapportExceptionCl rapport = this._rapportExceptionCLService.FindBy(id);
                            b = rapport.ImgFichier;
                            nomRapport = rapport.NomRapport;
                            pCentreEncour c = rapport.PCentreEncour;
                            if (c != null)
                            {
                                if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_CL))
                                {
                                    //EG le 27/02/2012 si on est le proprio du centre alors on met la date de téléchargement
                                    if (this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                                    {
                                        if (rapport.DateTelechargementRapport == null)
                                        {
                                            this._rapportExceptionCLService.traceTelechargement(id, this.User.Identity.Name);
                                        }
                                    }
                                }
                                else
                                {
                                    Exception e = new Exception("Vous n'êtes pas autorisé....");
                                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                                }
                            }
                            else
                            {
                                Exception e = new Exception("rapport sans centre associé !!!....");
                                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                            }

                        }
                        break;
                }
                return File(b, "application/download", nomRapport);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        //EG le 05/12/2011
        //
        // GET: /RapportCE/ListeDispoCTVL

        public ActionResult ListeDispoCTVL(int id)
        {

            pCentreEncour c = this._centreService.FindBy(id);
            if (GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CE_VL))
            {

                return View(c);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

    }
}
