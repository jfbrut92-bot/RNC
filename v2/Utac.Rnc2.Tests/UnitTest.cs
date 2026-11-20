using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Infrastructure.Utilitaires;
using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.UI.Web.Controllers;
using Utac.Rnc2.UI.Web.Models;
using Utac.Rnc2.UI.Web.Utils;

namespace Utac.Rnc2.Tests
{
    [TestClass]
    public class UnitTest
    {
        [TestInitialize]
        public void Before()
        {
            NHibernate.Context.CurrentSessionContext.Bind(SessionFactory.GetNewSession());
        }

        [TestCleanup]
        public void After()
        {
            //On ferme
            NHibernate.ISession session = default(NHibernate.ISession);
            session = NHibernate.Context.CurrentSessionContext.Unbind(SessionFactory.GetSessionFactory());
            session.Close();
            session.Dispose();
        }

        //[TestMethod]
        public void TestService()
        {
            try
            {
                IUnitOfWork transaction = new NhibernateUnitOfWork();
                IpControleurEnCourService service = NhibernateServiceFactory.getInstance().getpControleurEnCourService(false);

                pContoleurEncour controleurAModifie = service.FindBy(313);
                controleurAModifie.Nom = "MAZILLE-GERVINET";
                controleurAModifie.DateMiseaJour = System.DateTime.Now;
                service.Save(controleurAModifie);

                //existence d'une ligne historique

                //recherche par MAZILLE-GERVINET
                Query q = new Query();
                q.Criteria.Add(new Criteria("this.Nom", "MAZILLE-GERVINET", CriteriaOperator.EQUALS));
                pContoleurEncour controleurVerif = service.FindBy(q).ElementAt<pContoleurEncour>(0);

                Assert.AreNotEqual(controleurVerif, null);
                Assert.AreEqual<string>(controleurVerif.Nom, "MAZILLE-GERVINE");

                IEnumerable<pCentreEncour.CodeEtatAgrementCl> l = Enum.GetValues(typeof(pCentreEncour.CodeEtatAgrementCl)).Cast<pCentreEncour.CodeEtatAgrementCl>();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public void TestAncienCentre()
        {
            IUnitOfWork transaction = new NhibernateUnitOfWork();
            IpCentreEncourService service = NhibernateServiceFactory.getInstance().getpCentreEncourService(false, false);

            Assert.IsTrue(service.isAncienCentre(42));
            Assert.IsFalse(service.isAncienCentre(11492));

            Assert.IsTrue(service.isAncienCentre(service.FindBy(42)));
            Assert.IsFalse(service.isAncienCentre(service.FindBy(11492)));
        }

        [TestMethod]
        public void TestFindAllReseauPLAvecZ()
        {
            // Test
            IrReseauService service = NhibernateServiceFactory.getInstance().getrReseauService(false);

            List<rReseau> liste = service.FindAllReseauPLAvecZ();

            Assert.IsNotNull(liste);
            Assert.AreEqual(4, liste.Count);
            Assert.IsTrue(liste.Count == 4);
            Assert.IsTrue(liste.Exists(r => r.Id == "Z"));
            Assert.IsTrue(liste.Exists(r => r.Id == "G"));
        }

        [TestMethod]
        public void TestCreateCerttif()
        {

            IUnitOfWork transaction = new NhibernateUnitOfWork();
            IpCentreEncourService _pCentreEnCourservice = NhibernateServiceFactory.getInstance().getpCentreEncourService(false, false);
            IpCertificatService _pCertificatService = NhibernateServiceFactory.getInstance().getpCertificatService(true);

            // Récupération du centre
            int idCentre = 38072;
            string user = "s001z131";

            pCentreEncour centre = _pCentreEnCourservice.FindBy(idCentre);
            rCategorieVehicule.CategorieEnum categorieVehicule = rCategorieVehicule.CategorieEnum.VL;

            //R enseigner les infos du certificat
            InfoCertificat infoCert = new InfoCertificat();
            switch (categorieVehicule)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    infoCert.NomCert = centre.CodAgremVl;
                    break;
                case rCategorieVehicule.CategorieEnum.PL:
                    infoCert.NomCert = centre.CodAgremPl;
                    break;
                case rCategorieVehicule.CategorieEnum.CL:
                    infoCert.NomCert = centre.CodAgremCl;
                    break;
            }
            infoCert.NomCommunCert = centre.RaisonSociale;
            infoCert.CategorieVehicule = Convert.ToString(categorieVehicule);

            // Génération des mots de passe
            // Chaîne de caractères utilisée pour la génération des mdp (Web.config/rnc2.config)
            string chaine = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#=+.?";
            PasswordGenerator passwordGenerator = new PasswordGenerator(chaine);
            // Génération
            infoCert.MotDePassePfx = passwordGenerator.GenerateForDesizedSize();
            //infoCert.MotDePasseZip = passwordGenerator.Generate();

            // Génération du certificat
            GenerationCertificatResultat resultat = GenerateurCertificat.genere(infoCert);

            if (resultat.Resultat == true)
            {

                // On enregistre le certificat
                pCertificat certificat = new pCertificat();
                certificat.IdReq = resultat.NumeroCertificat;
                certificat.NumSerie = resultat.NumSerie;
                certificat.DateEffet = Convert.ToDateTime(resultat.DateDebutValidite);
                certificat.DateExpiration = Convert.ToDateTime(resultat.DateFinValidite);
                certificat.NomCert = resultat.NomCert;
                certificat.NomCommunCert = resultat.NomCommunCert;
                certificat.PasswordCert = resultat.PasswordCert;
                certificat.ImgFichier = resultat.Certificat;

                certificat.NomFichier = "CERTIFICAT_OTC_" + Convert.ToString(categorieVehicule) + "_" + string.Format("{0:yyyymmdd}", certificat.DateEffet) + "_" + string.Format("{0:yyyymmdd}", certificat.DateExpiration) + ".zip";
                certificat.CodCategorieVeh = Convert.ToString(categorieVehicule);
                bool isInitiale = true;
                _pCertificatService.nouveauCertificat(certificat, centre, categorieVehicule, user, isInitiale);
            }
        }

        [TestMethod]
        public void TestToString()
        {

            IUnitOfWork transaction = new NhibernateUnitOfWork();
            IpCentreEncourService _pCentreEnCourservice = NhibernateServiceFactory.getInstance().getpCentreEncourService(false, false);
            IpCertificatService _pCertificatService = NhibernateServiceFactory.getInstance().getpCertificatService(true);

            // Récupération du centre
            int idCentre = 54;

            pCentreEncour centre = _pCentreEnCourservice.FindBy(idCentre);
            //NHibernate.Impl.Printer p = SessionFactory.GetCurrentSession().;
            //System.Console.WriteLine(p.ToString(centre, EntityMode.Xml));
            XmlSerializer serializer = new XmlSerializer(typeof(pCentreEncour));
            TextWriter textWriter = new StreamWriter(@"D:\tmp\centre.xml");
            serializer.Serialize(textWriter, centre);
            textWriter.Close();

        }

        [TestMethod]
        public void test_d_edition_controleur_vl_cntr_dep()
        {
            IStatistiquesControleurService _controleurStatistiqueService = NhibernateServiceFactory.getInstance().getStatistiquesControleurService();

            rPrefecture prefecture = new rPrefecture() { Id = 95 };
            List<ItemListeDesControleursVLCentreRattachementSelonDepartement> liste = (List<ItemListeDesControleursVLCentreRattachementSelonDepartement>)_controleurStatistiqueService.listeDesControleursVLCentreRattachementSelonDepartement(prefecture);
            Assert.IsTrue(liste.Count == 160);
            Assert.IsTrue(liste.Exists(r => r.ControleurNom == "ALI BEN KARI SAID"));
        }

        [TestMethod]
        public void TestControleurStats()
        {
            StatistiquesController c = new StatistiquesController();

            StatistiquesModel model = new StatistiquesModel();
            model.PrefectureSelected = new rPrefecture();
            model.PrefectureSelected.Id = 95;
            model.FormatEdition = StatistiquesModel.FormatEditionEnum.XLS;
            System.Web.Mvc.ActionResult r = c.d_edition_controleur_vl_cntr_dep(model);
            Assert.IsNotNull(r);
        }

        [TestMethod]
        public void TestDroitAfficheDenominationSociale()
        {
            //Variables requises
            string[] roles = null;
            pCentreEncour centre = null;
            Utac.Rnc2.UI.Web.Helpers.GestionDroitsExecution.ActionsFicheCentre action;
            bool resultat;
            IPrincipal principal = null;

            // Changement de dénomination sociale
            // Profils : OTC_ADMIN , PREFECTURE 
            // Etat agrément centre : valide , encours
            action = UI.Web.Helpers.GestionDroitsExecution.ActionsFicheCentre.CHDECOM;

            // TEST 
            // Profil : OTC_Admin
            // Utilisateur : ple
            // Etat agrément VL = valide
            // Etat agrément PL = KO
            // Etat agrément CL = KO
            // Résultat attendu : True

            roles = new string[] { rUtilisateur.Profil.OTC_Administration.ToString() };
            centre = new pCentreEncour()
            {
                EtatAgrementVl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK).ToString(),
                EtatAgrementPl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO).ToString(),
                EtatAgrementCl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO).ToString()
            };
            principal = getPrincipal("ple", roles);
            resultat = Utac.Rnc2.UI.Web.Helpers.GestionDroitsExecution.isRightActionForCentre(principal, centre, action);
            Assert.IsTrue(resultat);

            // TEST 
            // Profil : OTC_Admin
            // Utilisateur : ple
            // Etat agrément VL = KO
            // Etat agrément PL = KO
            // Etat agrément CL = KO
            // Résultat attendu : False

            roles = new string[] { rUtilisateur.Profil.OTC_Administration.ToString() };
            centre = new pCentreEncour()
            {
                EtatAgrementVl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO).ToString(),
                EtatAgrementPl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO).ToString(),
                EtatAgrementCl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO).ToString()
            };
            principal = getPrincipal("ple", roles);
            resultat = Utac.Rnc2.UI.Web.Helpers.GestionDroitsExecution.isRightActionForCentre(principal, centre, action);
            Assert.IsFalse(resultat);

            // TEST 
            // Profil : OTC_Admin
            // Utilisateur : ple
            // Etat agrément VL = EN COURS
            // Etat agrément PL = KO
            // Etat agrément CL = KO
            // Résultat attendu : True

            roles = new string[] { rUtilisateur.Profil.OTC_Administration.ToString() };
            centre = new pCentreEncour()
            {
                EtatAgrementVl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS).ToString(),
                EtatAgrementPl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO).ToString(),
                EtatAgrementCl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementCl.AGREMENT_KO).ToString()
            };
            principal = getPrincipal("ple", roles);
            resultat = Utac.Rnc2.UI.Web.Helpers.GestionDroitsExecution.isRightActionForCentre(principal, centre, action);
            Assert.IsTrue(resultat);

            // TEST 
            // Profil : OTC_Admin
            // Utilisateur : ple
            // Etat agrément VL = KO
            // Etat agrément PL = KO
            // Etat agrément CL = EN COURS
            // Résultat attendu : True

            roles = new string[] { rUtilisateur.Profil.OTC_Administration.ToString() };
            centre = new pCentreEncour()
            {
                EtatAgrementVl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementVl.AGREMENT_KO).ToString(),
                EtatAgrementPl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementPl.AGREMENT_KO).ToString(),
                EtatAgrementCl = Convert.ToInt16(pCentreEncour.CodeEtatAgrementCl.AGREMENT_DEMANDE_EN_COURS).ToString()
            };
            principal = getPrincipal("ple", roles);
            resultat = Utac.Rnc2.UI.Web.Helpers.GestionDroitsExecution.isRightActionForCentre(principal, centre, action);
            Assert.IsTrue(resultat);
        }

        private IPrincipal getPrincipal(string name, string[] roles)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(3, name, DateTime.MinValue, DateTime.Now.AddSeconds(-1), false, "data", "path");
            FormsIdentity identity = new FormsIdentity(ticket);

            IPrincipal principal = new GenericPrincipal(identity, roles);
            return principal;
        }

        [TestMethod]
        public void TestFicheCentre()
        {
            var controller = new CentreController();
            System.Web.Routing.RouteData routeData = new System.Web.Routing.RouteData();
            routeData.Values.Add("controller", "Centre");
            routeData.Values.Add("action", "Fiche");
            controller.ControllerContext = new ControllerContext()
            {
                Controller = controller,
                RouteData = routeData,
                RequestContext = new System.Web.Routing.RequestContext()
                {
                    HttpContext = new MockHttpContext("ple", new string[] { rUtilisateur.Profil.OTC_Administration.ToString() }),
                    RouteData = routeData

                }
            };
            var result = controller.Fiche(38197) as ViewResult;
            Assert.AreEqual("Fiche", result.ViewName);

            //href="/Centre/ModificationDenominationSociale
        }

    }

    internal class MockHttpContext : HttpContextBase
    {
        private readonly IPrincipal _user = null;
        private readonly HttpSessionStateBase _session = null;

        public MockHttpContext(string username, string[] roles)
        {
            this._user = new GenericPrincipal(new GenericIdentity(username), roles);
            this._session = new MockHttpSessionState();
        }

        public override IPrincipal User
        {
            get
            {
                return _user;
            }
            set
            {
                base.User = value;
            }
        }

        public override HttpSessionStateBase Session
        {
            get
            {
                return this._session;
            }
        }
    }

    internal class MockHttpSessionState : HttpSessionStateBase
    {
        public override void Remove(string name)
        {

        }
    }
}
