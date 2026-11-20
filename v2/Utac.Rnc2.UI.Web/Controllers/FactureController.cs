using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Controllers
{
    [Authorize()]
    public class FactureController : RestrictedAcessController
    {

        IrReseauService _reseauService;
        IpCentreEncourService _centreService;
        IpFactureService _factureService;
        IrUtilisateurService _utilisateurService;

        public FactureController(IrReseauService reseauService, IpCentreEncourService centreService, IpFactureService factureService, IrUtilisateurService utilisateurService)
        {
            this._reseauService = reseauService;
            this._centreService = centreService;
            this._factureService = factureService;
            this._utilisateurService = utilisateurService;
        }

        public FactureController()
            : this(NhibernateServiceFactory.getInstance().getrReseauService(false),
                    NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true),
                    NhibernateServiceFactory.getInstance().getpFactureService(true),
                    NhibernateServiceFactory.getInstance().getrUtilisateurService(true, true)
            )
        {

        }

        //
        // GET: /Facture/
        public ActionResult ListeFactureReseau(string id)
        {
            // récupération du réseau
            FactureModel model = new FactureModel();
            model.Reseau = this._reseauService.FindBy(id);

            if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, model.Reseau)) || this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()))
            {
                //model.Reseau.PFactures = this._factureService.FindAllByReseau(id);
                return View(model);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        //
        // GET: /Facture/
        public ActionResult ListeFactureCentre(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            // récupération du centre
            pCentreEncour centre = this._centreService.FindBy(id);

            if ((categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_VL))
            || (categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_PL))
            || (categorie.Equals(rCategorieVehicule.CategorieEnum.CL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_CL)))
            {
                //centre.PFactures = this._factureService.FindAllByCentre(id);
                FactureModel model = new FactureModel();
                model.MonCentre = centre;
                model.Categorie = categorie;
                //rechercher l'utilisateur lié au centre VL, PL ou CL
                IList<rUtilisateur> utilisateurs = this._utilisateurService.GetUserByCentre(centre, categorie);
                model.UtilisateurCentre = utilisateurs.ElementAt(0);
                return View(model);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        //
        // GET: /Facture/Telecharge

        public ActionResult Telecharge(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                byte[] b;
                string nomFichier;

                pFacture facture = this._factureService.FindBy(id);
                b = facture.ImgFichier;
                nomFichier = facture.NomFichier;
                if (b == null || nomFichier == null)
                {
                    Exception e = new Exception("Aucun fichier disponible en téléchargement pour cette facture....");
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }

                pCentreEncour c = facture.PCentreEncour;
                if (c != null)
                {
                    if ((categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_VL))
                    || (categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_PL))
                    || (categorie.Equals(rCategorieVehicule.CategorieEnum.CL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_CL)))
                    {
                        //si on est le proprio du centre alors on met la date de téléchargement
                        if (this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()))
                        {
                            if (facture.DateTelechargementFacture == null)
                            {
                                this._factureService.traceTelechargement(id, this.User.Identity.Name);
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
                    //Ajout du test si réseau (prise en compte du téléchargement pour réseau)
                    rReseau reseau = facture.RReseau;
                    if (reseau != null)
                    {
                        if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, reseau)) || this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()))
                        {
                            if (this.User.IsInRole(rUtilisateur.Profil.RESEAU.ToString()))
                            {
                                if (facture.DateTelechargementFacture == null)
                                {
                                    this._factureService.traceTelechargement(facture, this.User.Identity.Name);
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
                        Exception e = new Exception("Rapport sans centre ou réseau associé !!!....");
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }

                return File(b, "application/download", nomFichier);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }


        //
        // GET: /Facture/ModifierParametreReseau

        public ActionResult ModifierParametreReseau(string id)
        {
            rReseau reseau = this._reseauService.FindBy(id);
            if (reseau != null)
            {
                if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, reseau)) || this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                {
                    return View(reseau);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            else
            {
                Exception e = new Exception("Facture sans centre ou réseau associé !!!....");
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Facture/ModifierParametreReseau
        [HttpPost]
        public ActionResult ModifierParametreReseau(string id, rReseau form)
        {
            rReseau reseau = this._reseauService.FindBy(id);
            if (reseau != null)
            {
                if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, reseau)) || this.User.IsInRole(rUtilisateur.Profil.OTC.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DRIRE.ToString()) || this.User.IsInRole(rUtilisateur.Profil.DSCR.ToString()))
                {
                    validateModifierParametreReseau(form);
                    if (ModelState.IsValid)
                    {
                        reseau = this._reseauService.ModifierParametreFacturationReseau(reseau, form.FacturePapier, form.EmailFacturation);
                        FactureModel modele = new FactureModel();
                        modele.Reseau = reseau;
                        return View("ListeFactureReseau", modele);
                    }
                    return View("ModifierParametreReseau", form);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            else
            {
                Exception e = new Exception("Facture sans centre ou réseau associé !!!....");
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private void validateModifierParametreReseau(rReseau m)
        {
            if (m.FacturePapier == false && (m.EmailFacturation == null || m.EmailFacturation.Length < 5))
                ModelState.AddModelError("EmailFacturation", "L'adresse mail doit être renseignéé.");
            if (m.EmailFacturation != null)
                if (m.EmailFacturation.Length > 4 && ValidMail(m.EmailFacturation) == false)
                    ModelState.AddModelError("EmailFacturation", "L'adresse mail n'est pas valide.");
        }

        private bool ValidMail(string mail_address)
        {
            bool b = new bool();
            Regex myRegex = new Regex(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.IgnoreCase);
            b = myRegex.IsMatch(mail_address);
            return b;
        }


        //
        // GET: /Facture/ModifierParametreUtilisateur

        public ActionResult ModifierParametreUtilisateur(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            FactureModel modele = new FactureModel();
            //rechercher le centre
            modele.MonCentre = this._centreService.FindBy(id);
            modele.Categorie = categorie;
            //rechercher l'utilisateur lié au centre VL, PL ou CL
            IList<rUtilisateur> utilisateurs = this._utilisateurService.GetUserByCentre(modele.MonCentre, modele.Categorie);
            modele.UtilisateurCentre = utilisateurs.ElementAt(0);

            if (modele.UtilisateurCentre != null && utilisateurs.Count() == 1)
            {

                if ((categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, modele.MonCentre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_VL))
                || (categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, modele.MonCentre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_PL)))
                {
                    return View(modele);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            else
            {
                Exception e = new Exception("Facture sans centre ou réseau associé !!!....");
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        // POST: /Facture/ModifierParametreUtilisateur
        [HttpPost]
        public ActionResult ModifierParametreUtilisateur(int id, FactureModel model)
        {
            rUtilisateur utilisateur = this._utilisateurService.FindBy(model.UtilisateurCentre.Id);
            if (utilisateur != null)
            {
                pCentreEncour c = this._centreService.FindBy(model.MonCentre.Id);
                if ((model.Categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_VL))
                || (model.Categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_FACTURE_PL)))
                {
                    validateModifierParametreUtilisateur(model.UtilisateurCentre);
                    if (ModelState.IsValid)
                    {
                        utilisateur = this._utilisateurService.ModifierParametreFacturationUtilisateur(utilisateur, model.UtilisateurCentre.FacturePapier, model.UtilisateurCentre.Email, this.User.Identity.Name);
                        //rechercher le centre
                        //pCentreEncour centre = this._centreService.FindBy(model.MonCentre.Id);
                        //centre.PFactures = this._factureService.FindAllByCentre(id);
                        model.MonCentre = this._centreService.FindBy(model.MonCentre.Id);
                        model.UtilisateurCentre = utilisateur;
                        return View("ListeFactureCentre", model);
                    }
                    return View(model);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
            }
            else
            {
                Exception e = new Exception("Facture sans centre ou réseau associé !!!....");
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }


        private void validateModifierParametreUtilisateur(rUtilisateur m)
        {
            if (m.FacturePapier == false && (m.Email == null || m.Email.Length < 5))
                ModelState.AddModelError("UtilisateurCentre.Email", "L'adresse mail doit être renseignéé.");
            if (m.Email != null)
                if (m.Email.Length > 4 && ValidMail(m.Email) == false)
                    ModelState.AddModelError("UtilisateurCentre.Email", "L'adresse mail n'est pas valide.");
        }
    }
}
