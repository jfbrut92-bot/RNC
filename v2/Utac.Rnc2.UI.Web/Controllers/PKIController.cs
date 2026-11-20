// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Infrastructure.Utilitaires;
using Utac.Rnc2.MVC.Extensions.mail;
using Utac.Rnc2.UI.Web.Helpers;
using Utac.Rnc2.UI.Web.Models;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class PKIController : RestrictedAcessController
    {

        private readonly IpCertificatService _certificatService;
        private readonly IpCentreEncourService _centreEnCoursService;
        private readonly IrReseauService _reseauService;
        private readonly IpGestionCertificatService _gestionCertificatService;
        private readonly IUtilisateurWebServiceVLService _utilisateurWebServiceVL;
        private readonly IUtilisateurWebServicePLService _utilisateurWebServicePL;
        private readonly IUtilisateurWebServiceCLService _utilisateurWebServiceCL;

        public PKIController(
                                IpCertificatService certificatService,
                                IpCentreEncourService centreService,
                                IrReseauService reseauService,
                                IpGestionCertificatService gestionCertificatService,
                                IUtilisateurWebServiceVLService utilisateurWebServiceVL,
                                IUtilisateurWebServicePLService utilisateurWebServicePL,
                                IUtilisateurWebServiceCLService utilisateurWebServiceCL
            )
        {
            this._certificatService = certificatService;
            this._centreEnCoursService = centreService;
            this._reseauService = reseauService;
            this._gestionCertificatService = gestionCertificatService;
            this._utilisateurWebServiceVL = utilisateurWebServiceVL;
            this._utilisateurWebServicePL = utilisateurWebServicePL;
            this._utilisateurWebServiceCL = utilisateurWebServiceCL;
        }

        public PKIController()
            : this(
                NhibernateServiceFactory.getInstance().getpCertificatService(false),
                NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true),
                NhibernateServiceFactory.getInstance().getrReseauService(false),
                NhibernateServiceFactory.getInstance().getpGestionCertificatService(false),
                NhibernateServiceFactory.getInstance().getUtilisateurWebServiceVLService(true),
                NhibernateServiceFactory.getInstance().getUtilisateurWebServicePLService(true),
                NhibernateServiceFactory.getInstance().getUtilisateurWebServiceCLService(true)
            )
        {
        }

        //
        // GET: /PKI/

        public ActionResult Index()
        {
            var sURL = "tcp://sintradev:8085/RemoteServeurPKI";
            var sEnsureSecurity = true;
            var sUsername = "rnc2remote";
            var sPassword = "rnc2remote";
            var sDomain = "sintradev";
            var sServeurPKI = @"sintradev\UTACTEST";
            var sNomCert = "SXXXZXXX2";
            var sNomCommunCert = "Contrôle poid lourd pierre LEBLANC2";
            //string sRepout="C:\\WINDOWS\\Temp";
            var sPasswordCert = "coucou";
            int iReqid;
            string sDateDebutValidCert;
            string sDateFinValidCert;
            string sNumSerieCert;
            int iErreurCode;
            string sErreurLibelle;
            string sMessageException;
            byte[] byteArrayCert;

            if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
            {
                var clientPKI = new RemoteClientPKI.CRemoteClientPKI();

                var lresult = clientPKI.RecupereNouveauCertificat(sURL, sEnsureSecurity, sUsername, sPassword, sDomain, sServeurPKI, sNomCert, sNomCommunCert, sPasswordCert, out iReqid, out sDateDebutValidCert, out sDateFinValidCert, out sNumSerieCert, "VL", out iErreurCode, out sErreurLibelle, out sMessageException, out byteArrayCert);
                if (lresult == 0)
                {
                    //On enregistre le certificat
                    var certificat = new pCertificat();
                    certificat.IdReq = iReqid;
                    certificat.ImgFichier = byteArrayCert;
                    certificat.NomCert = sNomCert;
                    certificat.NomCommunCert = sNomCommunCert;
                    certificat.NumSerie = sNumSerieCert;
                    certificat.PasswordCert = sPasswordCert;
                    certificat.DateEffet = Convert.ToDateTime(sDateDebutValidCert);
                    certificat.DateExpiration = Convert.ToDateTime(sDateFinValidCert);
                    //certificat.CodCategorieVeh = "VL";
                    this._certificatService.Save(certificat);
                    ViewData["message"] = sDateDebutValidCert + ":" + sDateFinValidCert + ":" + sMessageException;

                }
                else
                {
                    //C pas bon
                    ViewData["message"] = iErreurCode + ":" + sErreurLibelle + ":" + sMessageException;

                }

                return View();
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }

        }

        //GET
        public ActionResult Telecharge(int reqId, string numeroSerie)
        {
            var q = new Query();
            q.Criteria.Add(new Criteria("this.IdReq", reqId, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("this.NumSerie", numeroSerie, CriteriaOperator.EQUALS));

            var certificat = this._certificatService.FindBy(q).ElementAt<pCertificat>(0);
            var b = certificat.ImgFichier;

            return File(b, "application/download");
        }

        //GET
        public ActionResult List()
        {
            if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            {
                var certificats = this._certificatService.FindAll();

                return View(certificats);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        // GET: /PKI/TestServeurPKI

        public ActionResult TestServeurPKI()
        {
            var sURL = ConfigurationManager.AppSettings["remoting_url"];
            var sEnsureSecurity = Convert.ToBoolean(ConfigurationManager.AppSettings["remoting_security"]);
            var sUsername = ConfigurationManager.AppSettings["remoting_auth_user"];
            var sPassword = ConfigurationManager.AppSettings["remoting_auth_pwd"];
            var sDomain = ConfigurationManager.AppSettings["remoting_auth_domain"];
            var sServeurPKI = ConfigurationManager.AppSettings["remoting_pki_server"];
            string sRolesServeurCA;
            int iErreurCode;
            string sErreurLibelle;
            string sMessageException;

            if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            {
                var clientPKI = new RemoteClientPKI.CRemoteClientPKI();
                var res = clientPKI.TestAccesServeurCA(sURL, sEnsureSecurity, sUsername, sPassword, sDomain, sServeurPKI, out sRolesServeurCA, out iErreurCode, out sErreurLibelle, out sMessageException);
                var result = new TestServeurPKIResult();
                result.SURL = sURL;
                result.SEnsureSecurity = sEnsureSecurity;
                result.SUsername = sUsername;
                result.SPassword = sPassword;
                result.SDomain = sDomain;
                result.ResultCall = res;
                result.SServeurPKI = sServeurPKI;
                result.SRolesServeurCA = sRolesServeurCA;
                result.IErreurCode = iErreurCode;
                result.SErreurLibelle = sErreurLibelle;
                result.SMessageException = sMessageException;

                //PLE le 04/06/2012 Ajout du test de la présence du fichier UTAC-OTC.cer
                //definir le chemin du fichier racine de confiance

                if (result.ResultCall == 0)
                {
                    var fichierCA = ConfigurationManager.AppSettings["fichier_certificat_racine_confiance"];
                    fichierCA = Server.MapPath("~/Ressources/" + fichierCA);
                    if (System.IO.File.Exists(fichierCA))
                    {
                        result.SFichierFichierCA = fichierCA + " (OK)";
                    }
                    else
                    {
                        result.SFichierFichierCA = fichierCA + " (KO)";
                        result.ResultCall = 1;
                    }
                }
                else
                {
                    result.SFichierFichierCA = "Non testé !";
                }

                return View(result);
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }

        }

        // GET: /PKI/GenCertificatCentre

        public ActionResult GenCertificatCentre()
        {

            var liste = this._centreEnCoursService.findAllVLAgrees();
            var model = new GenCertificatModel() { Centres = liste };
            return View(model);

        }

        // GET: /PKI/GenCertificatCentre

        public ActionResult GenCertificatCentre(int IdCentre, FormCollection form, rCategorieVehicule.CategorieEnum categorieVehicule)
        {
            var centre = this._centreEnCoursService.FindBy(IdCentre);
            var sURL = "tcp://sintradev:8085/RemoteServeurPKI";
            var sEnsureSecurity = true;
            var sUsername = "rnc2remote";
            var sPassword = "rnc2remote";
            var sDomain = "sintradev";
            var sServeurPKI = @"sintradev\UTACTEST";
            var sNomCert = centre.CodAgremVl;
            var sNomCommunCert = centre.RaisonSociale;
            //string sRepout = "C:\\WINDOWS\\Temp";
            var sPasswordCert = "coucou";
            int iReqid;
            string sDateDebutValidCert;
            string sDateFinValidCert;
            string sNumSerieCert;
            int iErreurCode;
            string sErreurLibelle;
            string sMessageException;
            byte[] byteArrayCert;

            var clientPKI = new RemoteClientPKI.CRemoteClientPKI();

            //PLE le 25/05/2012 Correction anomalie : la catégorie du véhicule est en dur : VL
            //int lresult = clientPKI.RecupereNouveauCertificat(sURL, sEnsureSecurity, sUsername, sPassword, sDomain, sServeurPKI, sNomCert, sNomCommunCert, sPasswordCert, out iReqid, out sDateDebutValidCert, out sDateFinValidCert, out sNumSerieCert, "VL", out iErreurCode, out sErreurLibelle, out sMessageException, out byteArrayCert);
            var lresult = clientPKI.RecupereNouveauCertificat(sURL, sEnsureSecurity, sUsername, sPassword, sDomain, sServeurPKI, sNomCert, sNomCommunCert, sPasswordCert, out iReqid, out sDateDebutValidCert, out sDateFinValidCert, out sNumSerieCert, Convert.ToString(categorieVehicule), out iErreurCode, out sErreurLibelle, out sMessageException, out byteArrayCert);
            if (lresult == 0)
            {
                //On enregistre le certificat
                var certificat = new pCertificat();
                certificat.IdReq = iReqid;
                certificat.ImgFichier = byteArrayCert;
                certificat.NomCert = sNomCert;
                certificat.NomCommunCert = sNomCommunCert;
                certificat.NumSerie = sNumSerieCert;
                certificat.PasswordCert = sPasswordCert;
                certificat.DateEffet = Convert.ToDateTime(sDateDebutValidCert);
                certificat.DateExpiration = Convert.ToDateTime(sDateFinValidCert);
                //certificat.PCentreEncour.Id = centre.Id;
                certificat.CodCategorieVeh = Convert.ToString(categorieVehicule);

                this._certificatService.Save(certificat);
                ViewData["message"] = sDateDebutValidCert + ":" + sDateFinValidCert + ":" + sMessageException;

            }
            else
            {
                //C pas bon
                ViewData["message"] = iErreurCode + ":" + sErreurLibelle + ":" + sMessageException;

            }

            return RedirectToAction("GenCertificatCentre");
        }

        //
        // GET: /PKI/ListeOperationReseau
        // PLE 29/05/2012 Ajout du contrôle de l'appartenance au réseau de l'utilisateur.

        public ActionResult ListeOperationReseau(string id)
        {
            var r = this._reseauService.FindBy(id);

            if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, r) && (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) || this.User.IsInRole(rUtilisateur.Profil.RESEAU_Informatique.ToString())))
                || this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            {
                try
                {

                    var m = new CertificatModel();
                    //EG le 01/06/2012
                    m.MustRenew = false;
                    m.Reseau = r;
                    m.AUnCertificatValide = this._reseauService.aUnCertificatValide(r);

                    m.DernierCertificat = this._gestionCertificatService.findLastValideOuNonValide(r);
                    // Even when deactivating the Lazy Loading, The pGestionCertificat still lazy loads
                    // because what is being queried are the pGestionCertificats and then the parent is returned
                    // This will force loading of navigation property
                    IList<pGestionCertificat> gestion = null;
                    if (m.DernierCertificat != null)
                    {
                        gestion = m.DernierCertificat.PGestionCertificats.ToList();
                    }
                    m.DerniereTraceGestion = null;
                    if (m.DernierCertificat != null)
                    {
                        m.DerniereTraceGestion = m.DernierCertificat.PGestionCertificats.OrderByDescending(g => g.DateMaj).ElementAt(0);
                        //EG le 01/06/2012 pour gérer le cas du renouvellement proche de la date
                        var nbJourDelaiRenouvellementCertifSsl = Convert.ToInt32(ConfigurationManager.AppSettings["nb_jour_delai_renouv_certif_ssl"]);
                        var now = System.DateTime.Now;
                        if (now.AddDays(nbJourDelaiRenouvellementCertifSsl).CompareTo(m.DernierCertificat.DateExpiration) > 0)
                        {
                            m.MustRenew = true;
                        }
                    }

                    return View(m);
                }
                catch (Exception e)
                {
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }

        }

        //EG le 01/12/2011
        //
        // GET: /PKI/ListeOperationReseau

        public ActionResult ListeOperationCentreNonRattache(int id, rCategorieVehicule.CategorieEnum categorie)
        {

            var c = this._centreEnCoursService.FindBy(id);
            //06/06/2013 PLE Interdiction de télécharger un certificat pour une centre non valide
            //if ((GestionDroitsExecution.isUtilisateurProprietaireCentre(this.User, c, categorie) && this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && ((c.EstAgrementValideVL && categorie.Equals(rCategorieVehicule.CategorieEnum.VL)) || (c.EstAgrementValidePL && categorie.Equals(rCategorieVehicule.CategorieEnum.PL))))
            //     || this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            if ((categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_VL)) ||
                (categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_PL)) ||
                (categorie.Equals(rCategorieVehicule.CategorieEnum.CL) && GestionDroitsExecution.isRightActionForCentre(this.User, c, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_CL)))
            {

                try
                {
                    var actions = new Dictionary<rCategorieVehicule.CategorieEnum, GestionDroitsExecution.ActionsFicheCentre>();
                    actions.Add(rCategorieVehicule.CategorieEnum.VL, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_VL);
                    actions.Add(rCategorieVehicule.CategorieEnum.PL, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_PL);
                    actions.Add(rCategorieVehicule.CategorieEnum.CL, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_CL);

                    var m = new CertificatModel();
                    //EG le 01/06/2012
                    m.MustRenew = false;
                    m.Centre = c;
                    m.CategeorieVehicule = categorie;
                    try
                    {
                        m.AUnCertificatValide = this._centreEnCoursService.aUnCertificatValide(c, categorie);
                    }
                    catch (NHibernate.ObjectNotFoundException ex) //pour gérer le cas ou le certificat n'existe plus dans la base !
                    {
                        m.AUnCertificatValide = false;
                    }

                    try
                    {
                        m.DernierCertificat = this._gestionCertificatService.findLastValideOuNonValide(c, categorie);
                        if (m.DernierCertificat != null)
                        {
                            if (m.DernierCertificat.DateExpiration != null) { } //juste pour lever une exception
                            //EG le 01/06/2012 pour gérer le cas du renouvellement proche de la date
                            var nbJourDelaiRenouvellementCertifSsl = Convert.ToInt32(ConfigurationManager.AppSettings["nb_jour_delai_renouv_certif_ssl"]);
                            var now = System.DateTime.Now;
                            if (now.AddDays(nbJourDelaiRenouvellementCertifSsl).CompareTo(m.DernierCertificat.DateExpiration) > 0)
                            {
                                m.MustRenew = true;
                            }
                        }
                    }
                    catch (NHibernate.ObjectNotFoundException ex) //pour gérer le cas ou le certificat n'existe plus dans la base !
                    {
                        m.DernierCertificat = null;
                    }

                    if (m.DernierCertificat != null)
                    {
                        // Even when deactivating the Lazy Loading, The pGestionCertificat still lazy loads
                        // because what is being queried are the pGestionCertificats and then the parent is returned
                        // This will force loading of navigation property
                        var gestion = m.DernierCertificat.PGestionCertificats.ToList();
                        m.DerniereTraceGestion = m.DernierCertificat.PGestionCertificats.OrderByDescending(g => g.DateMaj).ElementAt(0);
                    }

                    return View(m);
                }
                catch (Exception e)
                {
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }

        }

        //
        // GET : /PKI/InformationCertificat
        public ActionResult InformationCertificat(int idReq, string NumSerie)
        {
            try
            {
                var c = this._certificatService.findByIdAndNumeroSerie(idReq, NumSerie);

                var m = new CertificatModel();

                m.DernierCertificat = c;

                var gestionCertificat = c.PGestionCertificats[0];
                m.Reseau = gestionCertificat.RReseau;

                if (gestionCertificat.PCentreEncourVL != null)
                {
                    m.Centre = gestionCertificat.PCentreEncourVL;
                }

                if (gestionCertificat.PCentreEncourPL != null)
                {
                    m.Centre = gestionCertificat.PCentreEncourPL;
                }

                if (gestionCertificat.PCentreEncourCL != null)
                {
                    m.Centre = gestionCertificat.PCentreEncourCL;
                }

                m.DerniereTraceGestion = m.DernierCertificat.PGestionCertificats.OrderByDescending(g => g.DateMaj).ElementAt(0);

                return View(m);
            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }
        }

        private ActionResult TelechargerCertificatReseau(string id, bool internalCall, bool renew, bool first)
        {
            // récupération du réseau
            var reseau = this._reseauService.FindBy(id);

            if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, reseau) && (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) || this.User.IsInRole(rUtilisateur.Profil.RESEAU_Informatique.ToString())))
                || this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            {

                try
                {
                    // récupération du dernier certificat valide
                    var certificat = this._gestionCertificatService.findLastValide(reseau);
                    // Even when deactivating the Lazy Loading, The pGestionCertificat still lazy loads
                    // because what is being queried are the pGestionCertificats and then the parent is returned
                    // This will force loading of navigation property
                    var gestion = certificat.PGestionCertificats.ToList();

                    if (certificat != null)
                    {
                        //contenu du ficher zip
                        var zipParameter = new ZipParameter();

                        //Nom du fichier zip
                        zipParameter.NomFichierZip = Server.MapPath("~/Ressources/") + certificat.NomFichier;

                        //definir le nom et le contenu du fichier pfx
                        zipParameter.CertificatPfx = certificat.ImgFichier;
                        zipParameter.NomFichierPfx = string.Format("RNC2_{0}.pfx", certificat.CodCategorieVeh);

                        //definir le chemin du fichier racine de confiance
                        var fichierCA = ConfigurationManager.AppSettings["fichier_certificat_racine_confiance"];
                        zipParameter.CheminFichierCertificatRacine = Server.MapPath("~/Ressources/" + fichierCA);

                        //6) génération du zip
                        var zip = Zip.Generate(zipParameter);

                        //7) Envoi de mail avec
                        /*string objet = "RNC2: les informations conernant votre certificat SSL de communication avec l'OTC";
                        string contenu = string.Format("Vous avez téléchargé un certificat SSL sur RNC2. Veuillez trouver ci-dessous les informations suivantes :\n" +
                                                       "Utilisateur Web services:{0}\n" +
                                                       "Mot de passe Web services:{1}\n" +
                                                       "Mot de passe d'installation du certificat SSL:{2}", certificat.BddTechLogin, certificat.BddTechPassword, certificat.PasswordCert);

                        InformationPersonelle info =  InformationHelper.getInformationPersonnelle();
                        string email = info.Email;
                        Mailer.Send(objet, contenu, email);
                        */

                        var declencheur = InformationHelper.getInformationPersonnelle(false).Utilisateur;
                        var logoPath = Server.MapPath("~/Ressources/logoOTC.gif");

                        var templateEnum = rTemplateMail.TemplateEnum.COPIE_CERTIFICAT;
                        if (first)
                        {
                            templateEnum = rTemplateMail.TemplateEnum.PREMIER_CERTIFICAT;
                        }

                        if (renew)
                        {
                            templateEnum = rTemplateMail.TemplateEnum.RENOUVELLEMENT_CERTIFICAT;
                        }

                        var mailArgs = new MailTemplateSender.MailTemplateSenderArguments()
                        {
                            Declencheur = declencheur,
                            Destinataire = declencheur,
                            LogoPath = logoPath,
                            Certificat = certificat,
                            TemplateMailEnum = templateEnum
                        };
                        var sender = new MailTemplateSender(mailArgs);
                        TempData["Message"] = "Certificat récupéré...";
                        TempData["MessageType"] = "Success";

                        if (sender.Send())
                        {
                            TempData["Message"] += "email envoyé";
                        }
                        else
                        {
                            TempData["Message"] += "MAIS ECHEC ENVOI MAIL !";
                        }

                        var nomFichierZip = certificat.NomFichier;

                        //8) est-ce un appel interne ou externe
                        if (!internalCall)
                        {
                            //on trace dans p_gestionCertificat
                            this._certificatService.traceTelechargementCertificat(certificat, reseau, this.User.Identity.Name);
                        }

                        return File(zip, "application/download", nomFichierZip);
                    }
                    else
                    {
                        var e = new Exception("Il n'y a pas de certificat valide !");
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }
                catch (Exception e)
                {
                    if (internalCall) throw e;
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }

        }

        private ActionResult TelechargerCertificatCentreSecurise(int id, rCategorieVehicule.CategorieEnum categorie, bool internalCall, bool renew, bool first)
        {

            var centre = this._centreEnCoursService.FindBy(id);

            //06/06/2013 PLE Interdiction de télécharger un certificat pour une centre non valide
            //if ((GestionDroitsExecution.isUtilisateurProprietaireCentre(this.User, centre, categorie) && this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && ((centre.EstAgrementValideVL && categorie.Equals(rCategorieVehicule.CategorieEnum.VL)) || (centre.EstAgrementValidePL && categorie.Equals(rCategorieVehicule.CategorieEnum.PL))))
            //     || this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))

            if ((categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_VL)) ||
            (categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_PL)) ||
            (categorie.Equals(rCategorieVehicule.CategorieEnum.CL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_CL)))
            {
                //05/11/2026 PLE interdiction du téléchargement d'une copie du certificat
                //06/11/2026 PLE sauf pour la profil admin
                if (!internalCall)
                {
                    if (!this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                    {
                        {
                            return ErrorHelper.InvaliAccessError(this.ControllerContext);
                        }
                    }
                }
                try
                {
                    // Récupération du dernier certificat valide
                    var certificat = this._gestionCertificatService.findLastValide(centre, categorie);
                    // Even when deactivating the Lazy Loading, The pGestionCertificat still lazy loads
                    // because what is being queried are the pGestionCertificats and then the parent is returned
                    // This will force loading of navigation property
                    var gestion = certificat.PGestionCertificats.ToList();

                    if (certificat != null)
                    {
                        //contenu du ficher zip
                        var zipParameter = new ZipParameter();

                        //Nom du fichier zip
                        zipParameter.NomFichierZip = Server.MapPath("~/Ressources/") + certificat.NomFichier;

                        //definir le nom et le contenu du fichier pfx
                        zipParameter.CertificatPfx = certificat.ImgFichier;
                        zipParameter.NomFichierPfx = $"RNC2_{certificat.CodCategorieVeh}.pfx";

                        //definir le chemin du fichier racine de confiance
                        var fichierCA = ConfigurationManager.AppSettings["fichier_certificat_racine_confiance"];
                        zipParameter.CheminFichierCertificatRacine = Server.MapPath($"~/Ressources/{fichierCA}");

                        //6) génération du zip
                        var zip = Zip.Generate(zipParameter);

                        //7) Envoi de mail avec
                        /*string objet = "RNC2: les informations conernant votre certificat SSL de communication avec l'OTC";
                        string contenu = string.Format("Vous avez téléchargé un certificat SSL sur RNC2. Veuillez trouver ci-dessous les informations suivantes :\n" +
                                                       "Utilisateur Web services:{0}\n" +
                                                       "Mot de passe Web services:{1}\n" +
                                                       "Mot de passe d'installation du certificat SSL:{2}", certificat.BddTechLogin, certificat.BddTechPassword, certificat.PasswordCert);

                        InformationPersonelle info = InformationHelper.getInformationPersonnelle();
                        string email = info.Email;
                        Mailer.Send(objet, contenu, email);
                         */
                        var declencheur = InformationHelper.getInformationPersonnelle(false).Utilisateur;
                        var logoPath = Server.MapPath("~/Ressources/logoOTC.gif");
                        var templateEnum = rTemplateMail.TemplateEnum.COPIE_CERTIFICAT;
                        if (first)
                        {
                            templateEnum = rTemplateMail.TemplateEnum.PREMIER_CERTIFICAT;
                        }

                        if (renew)
                        {
                            templateEnum = rTemplateMail.TemplateEnum.RENOUVELLEMENT_CERTIFICAT;
                        }
                        /*
                        if (!renew && !first)
                        {
                            templateEnum = rTemplateMail.TemplateEnum.COPIE_CERTIFICAT;
                        }
                        */

                        var mailArgs = new MailTemplateSender.MailTemplateSenderArguments()
                        {
                            Declencheur = declencheur,
                            Destinataire = declencheur,
                            LogoPath = logoPath,
                            Certificat = certificat,
                            TemplateMailEnum = templateEnum
                        };
                        var sender = new MailTemplateSender(mailArgs);
                        TempData["Message"] = "Certificat récupéré...";
                        TempData["MessageType"] = "Success";

                        if (sender.Send())
                        {
                            TempData["Message"] += "email envoyé";
                        }
                        else
                        {
                            TempData["Message"] += "MAIS ECHEC ENVOI MAIL !";
                        }

                        var nomFichierZip = certificat.NomFichier;

                        //8) est-ce un appel interne ou externe
                        if (!internalCall)
                        {
                            //on trace dans p_gestionCertificat
                            this._certificatService.traceTelechargementCertificat(certificat, centre, categorie, this.User.Identity.Name);
                        }

                        return File(zip, "application/download", nomFichierZip);
                    }
                    else
                    {
                        var e = new Exception("Il n'y a pas de certificat valide !");
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }
                catch (Exception e)
                {
                    if (internalCall) throw e;
                    return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                }
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }

        }

        public ActionResult TelechargerCertificatCentre(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            return TelechargerCertificatCentreSecurise(id, categorie, false, false, false);
        }

        public ActionResult TelechargerCertificatReseau(string id)
        {
            return TelechargerCertificatReseau(id, false, false, false);
        }

        public ActionResult CreerCertificatCentre(int id, rCategorieVehicule.CategorieEnum categorie, bool renew, bool first)
        {

            var centre = this._centreEnCoursService.FindBy(id);

            //06/06/2013 PLE Interdiction de télécharger un certificat pour une centre non valide
            //if ((GestionDroitsExecution.isUtilisateurProprietaireCentre(this.User, centre, categorie) && this.User.IsInRole(rUtilisateur.Profil.CENTRE_INDEPENDANT.ToString()) && ((centre.EstAgrementValideVL && categorie.Equals(rCategorieVehicule.CategorieEnum.VL)) || (centre.EstAgrementValidePL && categorie.Equals(rCategorieVehicule.CategorieEnum.PL))))
            //     || this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            if ((categorie.Equals(rCategorieVehicule.CategorieEnum.VL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_VL)) ||
                (categorie.Equals(rCategorieVehicule.CategorieEnum.PL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_PL)) ||
                (categorie.Equals(rCategorieVehicule.CategorieEnum.CL) && GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.TELECHARGE_CERTIF_SSL_CL)))
            {

                //05/11/2026 TEST de la validité de la demande
                // test validité "first" et "renew"
                pCertificat dernierCertificat = this._gestionCertificatService.findLastValideOuNonValide(centre, categorie);
                if (first && (dernierCertificat != null))
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }
                if (renew && (dernierCertificat == null))
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

                if (renew)
                {
                    var nbJourDelaiRenouvellementCertifSsl = Convert.ToInt32(ConfigurationManager.AppSettings["nb_jour_delai_renouv_certif_ssl"]);
                    var now = System.DateTime.Now;
                    if (!(now.AddDays(nbJourDelaiRenouvellementCertifSsl).CompareTo(dernierCertificat.DateExpiration) > 0))
                    {
                        return ErrorHelper.InvaliAccessError(this.ControllerContext);
                    }
                }

                try
                {

                    //renseigner les infos du certificat
                    var infoCert = new InfoCertificat();
                    infoCert.NomCert = categorie.Equals(rCategorieVehicule.CategorieEnum.VL) ? centre.CodAgremVl : categorie.Equals(rCategorieVehicule.CategorieEnum.PL) ? centre.CodAgremPl : centre.CodAgremCl;
                    infoCert.CategorieVehicule = categorie.ToString();

                    if (renew)
                    {
                        var gest = categorie.Equals(rCategorieVehicule.CategorieEnum.VL) ? centre.PGestionCertificatVLs[0] : categorie.Equals(rCategorieVehicule.CategorieEnum.PL) ? centre.PGestionCertificatPLs[0] : centre.PGestionCertificatCLs[0];
                        infoCert.NomCommunCert = gest.PCertificat.BddTechLogin;
                    }
                    else
                    {
                        //TODO Appeler proc stockée qui va chercher dans r_utilisateur
                        infoCert.NomCommunCert = categorie.Equals(rCategorieVehicule.CategorieEnum.VL) ? centre.CodAgremVl : categorie.Equals(rCategorieVehicule.CategorieEnum.PL) ? centre.CodAgremPl : centre.CodAgremCl;
                    }

                    var passwordGenerator = PasswordGenerator.getInstance();
                    //Génération
                    infoCert.MotDePassePfx = passwordGenerator.GenerateForDesizedSize();

                    //EG le 03/01/2012 : mot de passe en minuscule
                    infoCert.MotDePassePfx = infoCert.MotDePassePfx.ToLower();

                    //Génération du certificat
                    var resultat = GenerateurCertificat.genere(infoCert);

                    if (resultat.Resultat == true)
                    {

                        //On enregistre le certificat
                        var certificat = new pCertificat();
                        certificat.IdReq = resultat.NumeroCertificat;
                        certificat.NumSerie = resultat.NumSerie;
                        certificat.DateEffet = Convert.ToDateTime(resultat.DateDebutValidite);
                        certificat.DateExpiration = Convert.ToDateTime(resultat.DateFinValidite);
                        certificat.NomCert = resultat.NomCert;
                        certificat.NomCommunCert = resultat.NomCommunCert;
                        certificat.PasswordCert = resultat.PasswordCert;
                        certificat.ImgFichier = resultat.Certificat;
                        //TODO certificat.BddTechLogin;
                        //TODO certificat.BddTechPassword;
                        /*
                        if (renew)
                        {
                            pGestionCertificat gest = categorie.Equals(rCategorieVehicule.CategorieEnum.VL) ? centre.PGestionCertificatVLs[0] : categorie.Equals(rCategorieVehicule.CategorieEnum.PL) ? centre.PGestionCertificatPLs[0] : centre.PGestionCertificatCLs[0];
                            certificat.BddTechLogin = gest.PCertificat.BddTechLogin;
                            certificat.BddTechPassword = gest.PCertificat.BddTechPassword;
                        }
                        else
                        {
                            //TODO Appeler proc stockée qui va chercher dans r_utilisateur
                            certificat.BddTechLogin = "TODO";
                            certificat.BddTechPassword = "TODO";
                        }
                       */
                        //EG le 18/01/2012 : récupération des informations de la base du CT sur l'utilisateur web services
                        UtilisateurWebService utilisateurWebService = null;
                        if (rCategorieVehicule.CategorieEnum.VL.Equals(categorie))
                        {
                            utilisateurWebService = this._utilisateurWebServiceVL.FindByLogin(centre.CodAgremVl);
                            if (utilisateurWebService == null)
                            {
                                var newUser = new UtilisateurWebServiceVL();
                                newUser.Login = centre.CodAgremVl;
                                newUser.Password = PasswordGenerator.getInstance().GenerateForDesizedSize().ToLower();
                                newUser.CodReseau = rReseau.IdCodeReseau.Z.ToString();
                                this._utilisateurWebServiceVL.ajouterUtilisateur(newUser);
                                utilisateurWebService = newUser;
                            }
                        }

                        if (rCategorieVehicule.CategorieEnum.PL.Equals(categorie))
                        {
                            utilisateurWebService = this._utilisateurWebServicePL.FindByLogin(centre.CodAgremPl);
                            if (utilisateurWebService == null)
                            {
                                var newUser = new UtilisateurWebServicePL();
                                newUser.Login = centre.CodAgremPl;
                                newUser.Password = PasswordGenerator.getInstance().GenerateForDesizedSize().ToLower();
                                newUser.CodReseau = rReseau.IdCodeReseau.Z.ToString();
                                this._utilisateurWebServicePL.ajouterUtilisateur(newUser);
                                utilisateurWebService = newUser;
                            }

                        }
                        //PLE le 20/02/2024
                        if (rCategorieVehicule.CategorieEnum.CL.Equals(categorie))
                        {
                            utilisateurWebService = this._utilisateurWebServiceCL.FindByLogin(centre.CodAgremCl);
                            if (utilisateurWebService == null)
                            {
                                var newUser = new UtilisateurWebServiceCL();
                                newUser.Login = centre.CodAgremCl;
                                newUser.Password = PasswordGenerator.getInstance().GenerateForDesizedSize().ToLower();
                                newUser.CodReseau = rReseau.IdCodeReseau.Z.ToString();
                                this._utilisateurWebServiceCL.ajouterUtilisateur(newUser);
                                utilisateurWebService = newUser;
                            }

                        }

                        if (utilisateurWebService != null)
                        {
                            certificat.BddTechLogin = utilisateurWebService.Login;
                            certificat.BddTechPassword = utilisateurWebService.Password;
                            //utilisateurWebService.DateMaj = System.DateTime.Now;
                        }
                        else
                        {
                            //pas normal mais ...
                            certificat.BddTechLogin = "Contacter OTC";
                            certificat.BddTechPassword = "Contacter OTC";
                        }

                        certificat.NomFichier = "CERTIFICAT_OTC_CT" + infoCert.CategorieVehicule + "_" + infoCert.NomCert + "_" + string.Format("{0:yyyyMMdd}", certificat.DateEffet) + "_" + string.Format("{0:yyyyMMdd}", certificat.DateExpiration) + ".zip";
                        certificat.CodCategorieVeh = infoCert.CategorieVehicule;

                        this._certificatService.nouveauCertificat(certificat, centre, categorie, this.User.Identity.Name, renew == false);

                        return TelechargerCertificatCentreSecurise(id, categorie, true, renew, first);
                    }
                    else
                    {
                        var e = new Exception("Erreur lors de la création du certificat !\r\n" + resultat.ErreurLibelle + "\r\n" + resultat.MessageException);
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleError(ex, this.ControllerContext, "Error");
                }
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        public ActionResult CreerCertificatReseau(string id, bool renew, bool first)
        {

            // récupération du réseau
            var reseau = this._reseauService.FindBy(id);

            if ((GestionDroitsExecution.isUtilisateurAppartientReseau(this.User, reseau) && (this.User.IsInRole(rUtilisateur.Profil.RESEAU_Administrateur.ToString()) || this.User.IsInRole(rUtilisateur.Profil.RESEAU_Informatique.ToString())))
                || this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()) || this.User.IsInRole(rUtilisateur.Profil.OTC_Informatique.ToString()))
            {

                try
                {

                    //renseigner les infos du certificat
                    var infoCert = new InfoCertificat();
                    infoCert.NomCert = reseau.LettreReseau + "_" + reseau.Nom;
                    infoCert.CategorieVehicule = reseau.TypeReseauVl ? "VL" : reseau.TypeReseauPl ? "PL" : "CL";
                    /*
                    if (renew)
                    {
                        pGestionCertificat gest = reseau.PGestionCertificats[0];
                        infoCert.NomCommunCert = gest.PCertificat.BddTechLogin;
                    }
                    else
                    {
                        infoCert.NomCommunCert = "Controle Technique " + infoCert.CategorieVehicule + " RESEAU :" + reseau.Id;
                    }
                    */

                    infoCert.NomCommunCert = "Controle Technique " + infoCert.CategorieVehicule + " RESEAU : " + reseau.LettreReseau;

                    var passwordGenerator = PasswordGenerator.getInstance();
                    //Génération
                    infoCert.MotDePassePfx = passwordGenerator.GenerateForDesizedSize();

                    //EG le 03/01/2012 : mot de passe en minuscule
                    infoCert.MotDePassePfx = infoCert.MotDePassePfx.ToLower();

                    //Génération du certificat
                    var resultat = GenerateurCertificat.genere(infoCert);

                    if (resultat.Resultat == true)
                    {

                        //On enregistre le certificat
                        var certificat = new pCertificat();
                        certificat.IdReq = resultat.NumeroCertificat;
                        certificat.NumSerie = resultat.NumSerie;
                        certificat.DateEffet = Convert.ToDateTime(resultat.DateDebutValidite);
                        certificat.DateExpiration = Convert.ToDateTime(resultat.DateFinValidite);
                        certificat.NomCert = resultat.NomCert;
                        certificat.NomCommunCert = resultat.NomCommunCert;
                        certificat.PasswordCert = resultat.PasswordCert;
                        certificat.ImgFichier = resultat.Certificat;
                        /* EG commenté le 18/01/2012 : dans tous les cas on va chercher les infos dans la base du CT
                        if (renew)
                        {
                            pGestionCertificat gest = reseau.PGestionCertificats[0];
                            certificat.BddTechLogin = gest.PCertificat.BddTechLogin;
                            certificat.BddTechPassword = gest.PCertificat.BddTechPassword;
                        }
                        else
                        {
                            //TODO Appeler proc stockée qui va chercher dans r_utilisateur
                            certificat.BddTechLogin = "TODO";
                            certificat.BddTechPassword = "TODO";
                        }
                        */
                        //EG le 18/01/2012 : récupération des informations de la base du CT sur l'utilisateur web services
                        UtilisateurWebService utilisateurWebService = null;
                        if (reseau.TypeReseauVl)
                        {
                            utilisateurWebService = this._utilisateurWebServiceVL.FindByCodeReseau(reseau.Id);
                            if (utilisateurWebService == null)
                            {
                                //TODO ??? pour les réseaux
                            }
                        }

                        if (reseau.TypeReseauPl)
                        {
                            utilisateurWebService = this._utilisateurWebServicePL.FindByCodeReseau(reseau.Id);

                        }
                        //PLE le 20/02/2024 requete sur la lettre réseau en MAJUSCULE.
                        if (reseau.TypeReseauCl)
                        {
                            utilisateurWebService = this._utilisateurWebServiceCL.FindByCodeReseau(reseau.LettreReseau);

                        }

                        if (utilisateurWebService != null)
                        {
                            certificat.BddTechLogin = utilisateurWebService.Login;
                            certificat.BddTechPassword = utilisateurWebService.Password;
                            //utilisateurWebService.DateMaj = System.DateTime.Now;
                        }
                        else
                        {
                            //pas normal mais ...
                            certificat.BddTechLogin = "Contacter OTC";
                            certificat.BddTechPassword = "Contacter OTC";
                        }

                        /* CL
                         * 
                         *  if (reseau.TypeReseauCl)
                            {
                                utilisateurWebService = this._utilisateurWebServiceCL.FindByCodeReseau(reseau.Id);
                            }
                         */

                        certificat.NomFichier = "CERTIFICAT_OTC_CT" + infoCert.CategorieVehicule + "_" + reseau.LettreReseau + "_" + string.Format("{0:yyyyMMdd}", certificat.DateEffet) + "_" + string.Format("{0:yyyyMMdd}", certificat.DateExpiration) + ".zip";
                        certificat.CodCategorieVeh = infoCert.CategorieVehicule;

                        this._certificatService.nouveauCertificat(certificat, reseau, this.User.Identity.Name, renew == false);

                        return TelechargerCertificatReseau(id, true, renew, first);
                    }
                    else
                    {
                        var e = new Exception("Erreur lors de la création du certificat !\r\n" + resultat.ErreurLibelle + "\r\n" + resultat.MessageException);
                        return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
                    }
                }
                catch (Exception ex)
                {
                    return ErrorHelper.HandleError(ex, this.ControllerContext, "Error");
                }
            }
            else
            {
                return ErrorHelper.InvaliAccessError(this.ControllerContext);
            }
        }

        public ActionResult RevoquerCertificatCentre(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                var centre = this._centreEnCoursService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.REVOCATION_CERTIF))
                {
                    var model = new CertificatModel();
                    model.Centre = centre;
                    model.CategeorieVehicule = categorie;

                    return View(model);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        [HttpPost]
        public ActionResult RevoquerCertificatCentre(int id, CertificatModel model)
        {
            try
            {
                var centre = this._centreEnCoursService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.REVOCATION_CERTIF))
                {
                    var categorie = model.CategeorieVehicule;
                    var gests = categorie.Equals(rCategorieVehicule.CategorieEnum.VL) ? centre.PGestionCertificatVLs : categorie.Equals(rCategorieVehicule.CategorieEnum.PL) ? centre.PGestionCertificatPLs : centre.PGestionCertificatCLs;
                    var gest = gests.OrderByDescending(g => g.DateMaj).ElementAt(0);
                    var autorisationRenew = model.AutorisationRenew;
                    this._certificatService.revocationCertificat(gest.PCertificat, centre, categorie, autorisationRenew, this.User.Identity.Name);
                    return RedirectToAction("ListeOperationCentreNonRattache", new { id = id, categorie = categorie });
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        public ActionResult AutoriserRenouvellementCertificatCentre(int id, rCategorieVehicule.CategorieEnum categorie)
        {
            try
            {
                var centre = this._centreEnCoursService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.REVOCATION_CERTIF))
                {
                    var model = new CertificatModel();
                    model.Centre = centre;
                    model.CategeorieVehicule = categorie;

                    return View(model);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        [HttpPost]
        public ActionResult AutoriserRenouvellementCertificatCentre(int id, CertificatModel model)
        {
            try
            {
                var centre = this._centreEnCoursService.FindBy(id);
                if (GestionDroitsExecution.isRightActionForCentre(this.User, centre, GestionDroitsExecution.ActionsFicheCentre.REVOCATION_CERTIF))
                {
                    var categorie = model.CategeorieVehicule;
                    var gest = categorie.Equals(rCategorieVehicule.CategorieEnum.VL) ? centre.PGestionCertificatVLs[0] : categorie.Equals(rCategorieVehicule.CategorieEnum.PL) ? centre.PGestionCertificatPLs[0] : centre.PGestionCertificatCLs[0];
                    this._certificatService.autorisationCertificat(gest.PCertificat, centre, categorie, this.User.Identity.Name);
                    return RedirectToAction("ListeOperationCentreNonRattache", new { id = id, categorie = categorie });
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        //Révocation Réseau
        public ActionResult RevoquerCertificatReseau(string id)
        {
            try
            {
                var reseau = this._reseauService.FindBy(id);
                if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    var model = new CertificatModel();
                    model.Reseau = reseau;

                    return View(model);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        [HttpPost]
        public ActionResult RevoquerCertificatReseau(string id, CertificatModel model)
        {
            try
            {
                var reseau = this._reseauService.FindBy(id);
                if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    var categorie = model.CategeorieVehicule;
                    var gest = reseau.PGestionCertificats.OrderByDescending(g => g.DateMaj).ElementAt(0);
                    var autorisationRenew = model.AutorisationRenew;
                    this._certificatService.revocationCertificat(gest.PCertificat, reseau, autorisationRenew, this.User.Identity.Name);
                    return RedirectToAction("ListeOperationReseau", new { id = id });
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        public ActionResult AutoriserRenouvellementCertificatReseau(string id)
        {
            try
            {
                var reseau = this._reseauService.FindBy(id);
                if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    var model = new CertificatModel();
                    model.Reseau = reseau;

                    return View(model);
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

        [HttpPost]
        public ActionResult AutoriserRenouvellementCertificatReseau(string id, CertificatModel model)
        {
            try
            {
                var reseau = this._reseauService.FindBy(id);
                if (this.User.IsInRole(rUtilisateur.Profil.OTC_Administration.ToString()))
                {
                    var categorie = model.CategeorieVehicule;
                    var gest = reseau.PGestionCertificats[0];
                    this._certificatService.autorisationCertificat(gest.PCertificat, reseau, this.User.Identity.Name);
                    return RedirectToAction("ListeOperationReseau", new { id = id });
                }
                else
                {
                    return ErrorHelper.InvaliAccessError(this.ControllerContext);
                }

            }
            catch (Exception e)
            {
                return ErrorHelper.HandleError(e, this.ControllerContext, "Error");
            }

        }

    }
}
