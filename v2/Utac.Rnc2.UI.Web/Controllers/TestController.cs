using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Proxy;
//using Utac.Rnc2.Repository.Nhibernate;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.UI.Web.DataSets;

namespace Utac.Rnc2.UI.Web.Controllers
{
    public class TestController : RestrictedAcessController
    /* PLE : EN production le controleur doit être du type -->    RestrictedAcessController */
    {

        private readonly IpControleurEnCourService pControleurService;
        private readonly IpCentreEncourService pCentreService;
        private IStatistiquesControleurService _controleurStatistiqueService = null;

        public TestController(IpControleurEnCourService serviceControleur, IpCentreEncourService serviceCentre)
        {
            this.pControleurService = serviceControleur;
            this.pCentreService = serviceCentre;

        }

        public TestController()
            : this(NhibernateServiceFactory.getInstance().getpControleurEnCourService(false), NhibernateServiceFactory.getInstance().getpCentreEncourService(false, true))
        {
        }

        //
        // GET: /Test/

        public ActionResult Index()
        {
            IrUtilisateurService service = NhibernateServiceFactory.getInstance().getrUtilisateurService(false, true);
            //List<rUtilisateur> l = certificatService.FindAll();
            //Recherche multi critere
            Query q = new Query();
            Criteria critere1 = new Criteria("this.Nom", "GER%", CriteriaOperator.LIKE);
            q.Criteria.Add(critere1);

            List<rUtilisateur> l = service.FindBy(q);
            //ViewData.Model = l;
            return View(l);
        }

        //
        // GET: /Test/

        public ActionResult Edit(int id)
        {

            return View();
        }

        //
        // TestCrystal

        //public ActionResult TestCrystal()
        //{
        //    //Affichage de la liste des tables contenues dans le Report
        //    ReportDocument boReportDocument = new ReportDocument();
        //    boReportDocument.Load(Server.MapPath("~/reporting/test.rpt"));
        //    //PLE 06/06/2012 libération du rapport
        //    boReportDocument.Close();
        //    return View(boReportDocument.Database.Tables);

        //}

        //public ActionResult TestCrystalPdf()
        //{
        //    //Récupération des données
        //    //List<Utac.Rnc2.BusinessObjects.pContoleurEncour> liste = this.pControleurService.FindAll();
        //    BaseService<rRegion> service = new BaseService<rRegion>(new BaseRepository<rRegion>() , null);
        //    List<Utac.Rnc2.BusinessObjects.rRegion> liste = service.FindAll();

        //    //On charge le document Crystal
        //    ReportDocument boReportDocument = new ReportDocument();
        //    boReportDocument.Load(Server.MapPath("~/bin/reporting/regions.rpt"));

        //    //On cré un DataSet
        //    RCN2DataSet ds = new RCN2DataSet();
        //    DataTable t = ds.Tables.Add("Region");
        //    t.Columns.Add("Id", Type.GetType("System.Int32"));
        //    t.Columns.Add("Libelle", Type.GetType("System.String"));

        //    //On remplit le DataSet
        //    foreach (rRegion region in liste)
        //    {
        //        DataRow r = t.NewRow();
        //        r["Id"] = region.Id;
        //        r["Libelle"] = region.LibelleRegion;
        //        t.Rows.Add(r);
        //    }

        //    //On supprime les nulls
        //    //Converter<pContoleurEncour, pContoleurEncour> convert = (p) => new pContoleurEncour(p);

        //    //On définit la source de données du document
        //    //boReportDocument.Database.Tables[0].SetDataSource(liste.ConvertAll<pContoleurEncour>(convert));
        //    //boReportDocument.DataSourceConnections
        //    //boReportDocument.Database.Tables[0].SetDataSource(ds);
        //    boReportDocument.SetDataSource(ds.Tables[1]);

        //    //Définition du format d'export PDF
        //    ExportOptions exportOptions = new ExportOptions();
        //    //This sets which format we will export to.
        //    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //    //Create the FormatOptions class for PDF.
        //    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
        //    PDFexportOptions.FirstPageNumber = 1;
        //    PDFexportOptions.LastPageNumber = 2;
        //    PDFexportOptions.CreateBookmarksFromGroupTree = true;

        //    //Set the export format options with the page range we just set.
        //    exportOptions.FormatOptions = PDFexportOptions;

        //    //On exporte le document au format PDF
        //    //boReportDocument.Export(exportOptions);

        //    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

        //    MemoryStream memStream = new MemoryStream();

        //    byte[] respBuffer = new byte[1024];

        //    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
        //    while (bytesRead > 0)
        //    {
        //        memStream.Write(respBuffer, 0, bytesRead);
        //        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
        //    }

        //    //PLE 06/06/2012 libération du rapport
        //    boReportDocument.Close();
        //    return File(memStream.GetBuffer(), "application/pdf");
        //    /* Pour exporter vers le disque : OK ca marche
        //    ExportOptions CrExportOptions;
        //    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
        //    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
        //    CrDiskFileDestinationOptions.DiskFileName = Server.MapPath("~/tmp/test.pdf");
        //    CrExportOptions = boReportDocument.ExportOptions;
        //    {
        //        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
        //        CrExportOptions.FormatOptions = CrFormatTypeOptions;
        //    }
        //    boReportDocument.Export();
        //    */

        //}

        //GET
        public ActionResult Mail()
        {
            return View();
        }

        //POST
        [HttpPost]
        public ActionResult Mail(string email, string contenu)
        {
            string resultMsg = "mail envoyé!";
            try
            {
                //envoi du mail
                string smtpHost = ConfigurationManager.AppSettings["smtp_host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtp_port"]);
                string authUser = ConfigurationManager.AppSettings["smtp_auth_user"];
                string authPassword = ConfigurationManager.AppSettings["smtp_auth_pwd"];
                string expediteur = ConfigurationManager.AppSettings["from"];

                SmtpClient client = new SmtpClient(smtpHost, port);
                //Authentification: utilisateur: Email_Applis_DEV / mot de pase: appliutac
                //Adresse expéditeur autorisée: noreply@utac.com
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(authUser, authPassword);

                MailAddress from = new MailAddress(expediteur);
                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);
                // Specify the message content.
                MailMessage message = new MailMessage(from, to);
                message.Body = contenu;
                // Include some non-ASCII characters in body and subject.
                /*string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
                message.Body += Environment.NewLine + someArrows;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.Subject = "test message 1" + someArrows;
                message.SubjectEncoding = System.Text.Encoding.UTF8;
                */
                client.Send(message);

            }
            catch (Exception ex)
            {
                resultMsg = "Une erreur est survenue : " + ex.Message;
            }
            ViewData["message"] = resultMsg;
            return View();
        }

        //public ActionResult testImage()
        //{
        //    Sybase.Data.AseClient.AseConnection conn = new Sybase.Data.AseClient.AseConnection("Data Source=192.168.2.7;Port=4100;UID=appli_rnc2;PWD=appli_rnc2;Database=rnc2");
        //    conn.Open();
        //    string sql = "SELECT * from p_rapport_exception WHERE id_rapport_exception = 14674";
        //    Sybase.Data.AseClient.AseCommand cmdText = new Sybase.Data.AseClient.AseCommand("set textsize 3145728", conn);
        //    cmdText.ExecuteNonQuery();

        //    Sybase.Data.AseClient.AseCommand cmd= new Sybase.Data.AseClient.AseCommand(sql , conn);    

        //    cmd.CommandType = CommandType.Text;
        //    Sybase.Data.AseClient.AseDataReader rs = cmd.ExecuteReader();
        //    long bytesRead = 0;
        //    long dataIndex = 0;
        //    int length = 3145728;
        //    byte[] buf = new byte[length];
        //    MemoryStream ms = new MemoryStream();
        //    rs.Read();
        //    do
        //    {
        //        bytesRead = rs.GetBytes(rs.GetOrdinal("img_fichier"), dataIndex, buf, 0, length);
        //        dataIndex += length;
        //        ms.Write(buf, 0, (int)bytesRead);
        //        buf = new byte[length];
        //    }
        //    while (bytesRead == length);
        //    rs.Close();
        //    conn.Close();
        //    ms.Position = 0;
        //    return File(ms, "application/download" , "test.zip");
        //}

        public ActionResult TestNotifControleurPdf()
        {
            //Récupération des données

            //On charge le document Crystal
            ReportDocument boReportDocument = new ReportDocument();
            boReportDocument.Load(Server.MapPath("~/bin/reporting/Notification_decision_agrment_initial_controleur.rpt"));

            //On cré un DataSet
            RCN2DataSet ds = new RCN2DataSet();
            DataTable t = ds.Tables["notification_centre_vl"];
            /* SI dataset créé dynamic
             * t.Columns.Add("type_agrement_vl", Type.GetType("System.String"));
            t.Columns.Add("type_agrement_pl", Type.GetType("System.String"));
            t.Columns.Add("nom_prenom", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_jour_1", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_jour_2", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_mois_1", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_mois_2", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_annee_1", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_annee_2", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_annee_3", Type.GetType("System.String"));
            t.Columns.Add("date_naissance_annee_4", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_type_centre", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_code_dep_1", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_code_dep_2", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_code_dep_3", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_code_code_reseau", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_num_ordre_1", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_num_ordre_2", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_centr_num_ordre_3", Type.GetType("System.String"));
            t.Columns.Add("libelle_departement", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_code_dep_1", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_code_dep_2", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_code_dep_3", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_code_reseau", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_num_ordre_1", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_num_ordre_2", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_num_ordre_3", Type.GetType("System.String"));
            t.Columns.Add("n_agrement_cntlr_num_ordre_4", Type.GetType("System.String"));
            t.Columns.Add("date_notification_agrement", Type.GetType("System.String"));
            t.Columns.Add("titre_departement", Type.GetType("System.String"));
            */

            //On remplit le DataSet
            //foreach (rRegion region in liste)
            //{
            DataRow r = t.NewRow();
            r["type_agrement_vl"] = "1";
            r["type_agrement_pl"] = "0";
            r["nom_prenom"] = "MARTIN Renaud";
            r["date_naissance_jour_1"] = "2";
            r["date_naissance_jour_2"] = "0";
            r["date_naissance_mois_1"] = "1";
            r["date_naissance_mois_2"] = "2";
            r["date_naissance_annee_1"] = "1";
            r["date_naissance_annee_2"] = "9";
            r["date_naissance_annee_3"] = "7";
            r["date_naissance_annee_4"] = "6";
            r["n_agrement_centr_type_centre"] = "S";
            r["n_agrement_centr_code_dep_1"] = "9";
            r["n_agrement_centr_code_dep_2"] = "7";
            r["n_agrement_centr_code_dep_3"] = "2";
            r["n_agrement_centr_code_code_reseau"] = "D";
            r["n_agrement_centr_num_ordre_1"] = "0";
            r["n_agrement_centr_num_ordre_2"] = "0";
            r["n_agrement_centr_num_ordre_3"] = "1";
            r["libelle_departement"] = "ESSONNE";
            r["n_agrement_cntlr_code_dep_1"] = "0";
            r["n_agrement_cntlr_code_dep_2"] = "9";
            r["n_agrement_cntlr_code_dep_3"] = "1";
            r["n_agrement_cntlr_code_reseau"] = "D";
            r["n_agrement_cntlr_num_ordre_1"] = "0";
            r["n_agrement_cntlr_num_ordre_2"] = "0";
            r["n_agrement_cntlr_num_ordre_3"] = "0";
            r["n_agrement_cntlr_num_ordre_4"] = "1";
            r["date_notification_agrement"] = "12/07/2011";
            r["titre_departement"] = "VAL DZE MARNE";

            t.Rows.Add(r);
            //}

            boReportDocument.SetDataSource(ds.Tables["notification_centre_vl"]);

            //Définition du format d'export PDF
            ExportOptions exportOptions = new ExportOptions();
            //This sets which format we will export to.
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //Create the FormatOptions class for PDF.
            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
            PDFexportOptions.FirstPageNumber = 1;
            PDFexportOptions.LastPageNumber = 2;
            PDFexportOptions.CreateBookmarksFromGroupTree = true;

            //Set the export format options with the page range we just set.
            exportOptions.FormatOptions = PDFexportOptions;

            //On exporte le document au format PDF
            //boReportDocument.Export(exportOptions);

            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

            MemoryStream memStream = new MemoryStream();

            byte[] respBuffer = new byte[1024];

            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            while (bytesRead > 0)
            {
                memStream.Write(respBuffer, 0, bytesRead);
                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            }

            //PLE 06/06/2012 libération du rapport
            boReportDocument.Close();
            return File(memStream.GetBuffer(), "application/pdf");
            /* Pour exporter vers le disque : OK ca marche
            ExportOptions CrExportOptions;
            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            CrDiskFileDestinationOptions.DiskFileName = Server.MapPath("~/tmp/test.pdf");
            CrExportOptions = boReportDocument.ExportOptions;
            {
                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                CrExportOptions.FormatOptions = CrFormatTypeOptions;
            }
            boReportDocument.Export();
            */

        }

        //Test de l'état Notification d’agrément d’un centre spécialisé.
        //cntr_notification_decision_agrem_init_annexe2-A
        public ActionResult etat1(int? num_test)
        {
            //Récupération des données

            //On charge le document Crystal
            ReportDocument boReportDocument = new ReportDocument();
            boReportDocument.Load(Server.MapPath("~/bin/reporting/cntr_notification_decision_agrem_init_annexe2-A.rpt"));

            cntr_notification_decision_agrem_init_annexe2_A ds = new cntr_notification_decision_agrem_init_annexe2_A();
            cntr_notification_decision_agrem_init_annexe2_A.notification_centreDataTable dt = ds.notification_centre;
            cntr_notification_decision_agrem_init_annexe2_A.notification_centreRow dr = dt.Newnotification_centreRow();

            //PLE 06/03/2012
            // Ajout d'une ou plusieurs Dérogations
            cntr_notification_decision_agrem_init_annexe2_A.pDerogationCentreDataTable dtDerog = ds.pDerogationCentre;
            cntr_notification_decision_agrem_init_annexe2_A.pDerogationCentreRow drDerog1 = dtDerog.NewpDerogationCentreRow();
            drDerog1.dateDebut = Convert.ToDateTime("2012-12-12T00:00:00");
            drDerog1.dateFin = Convert.ToDateTime("2012-12-13T00:00:00");
            drDerog1.commentaire = "Commentaire N°1 VL /Commentaire N°1 VL /Commentaire N°1 /Commentaire N°1 /";
            drDerog1.categorieVehicule = Utac.Rnc2.BusinessObjects.rCategorieVehicule.CategorieEnum.VL.ToString();

            cntr_notification_decision_agrem_init_annexe2_A.pDerogationCentreRow drDerog2 = dtDerog.NewpDerogationCentreRow();
            drDerog2.dateDebut = Convert.ToDateTime("2012-12-12T00:00:00");
            drDerog2.dateFin = Convert.ToDateTime("2012-12-13T00:00:00");
            drDerog2.commentaire = "Commentaire N°1 PL /Commentaire N°1 PL /Commentaire N°1 /Commentaire N°1 /";
            drDerog2.categorieVehicule = Utac.Rnc2.BusinessObjects.rCategorieVehicule.CategorieEnum.VL.ToString();

            switch (num_test)
            {
                case 1:
                    //Centre indépendant VL avec annulation du numéro d'agréent precedant
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_centre_independant = true;
                    dr.ancien_numero_agrem = "S037D333";
                    dr.cod_agrem = "S037Z334";
                    dr.cod_reseau = "Z";
                    //dr.libelle_reseau="";
                    dr.titre_departement = "Préfet d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.nom_representant_legal = "LEBLANC Pierre";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.num_accuse_reception = "2343";
                    dr.identifiant_rnc2 = "S078Z345";
                    dr.pwd_rnc2 = "shrj9sqs";
                    dr.identifiant_otc = "cnr";
                    dr.pwd_otc = "xlx";
                    dr.est_soumis_derogation = true;
                    dr.url_site_rnc2 = "https://www.utac-otc.com/rnc2";
                    dr.url_site_otc = "http://www.utac.com";
                    dtDerog.Rows.Add(drDerog1);
                    dtDerog.Rows.Add(drDerog2);
                    break;
                case 2:
                    //Centre Réseau VL avec annulation du numéro d'agréent precedant + dérogation
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_centre_independant = false;
                    dr.ancien_numero_agrem = "S037Z333";
                    dr.cod_agrem = "S037D334";
                    dr.cod_reseau = "D";
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    dr.titre_departement = "Préfet d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE AVEC UN LIBELLE TRES LONG SUPER LONG SUPER TRES LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.nom_representant_legal = "LEBLANC Pierre LIBELLE LONG ONG LONG";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-13T00:00:00");
                    dr.num_accuse_reception = "2343";
                    //dr.identifiant_rnc2 = "S078Z345";
                    //dr.pwd_rnc2 = "shrj9sqs";
                    //dr.identifiant_otc = "cnr";
                    //dr.pwd_otc = "xlx";
                    //dr.est_soumis_a_derogation = true;
                    //dr.url_site_rnc2 = "https://www.utac-otc.com/rnc2";
                    //dr.url_site_otc = "http://www.utac.com";
                    dr.est_soumis_derogation = false;
                    break;
                case 3:
                    //Centre Réseau VL avec agrément initial + dérogation
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_centre_independant = false;
                    //dr.ancien_numero_agrem = "S037Z333"; <--- il s'agit d'un agrément initial : pas d'ancien numéro
                    dr.cod_agrem = "S037D334";
                    dr.cod_reseau = "D";
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    dr.titre_departement = "Préfet de Corse";
                    dr.libelle_prefet = "de Corse";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE AVEC UN LIBELLE TRES LONG SUPER LONG SUPER TRES LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.nom_representant_legal = "LEBLANC Pierre LIBELLE LONG ONG LONG";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-13T00:00:00");
                    dr.num_accuse_reception = "2343";
                    //dr.identifiant_rnc2 = "S078Z345";
                    //dr.pwd_rnc2 = "shrj9sqs";
                    //dr.identifiant_otc = "cnr";
                    //dr.pwd_otc = "xlx";
                    //dr.est_soumis_a_derogation = true;
                    //dr.url_site_rnc2 = "https://www.utac-otc.com/rnc2";
                    //dr.url_site_otc = "http://www.utac.com";
                    break;
                case 4:
                    //Centre indépendant PL avec annulation du numéro d'agréent precedant
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.est_centre_independant = true;
                    dr.ancien_numero_agrem = "S037K333";
                    dr.cod_agrem = "S037Z334";
                    dr.cod_reseau = "Z";
                    //dr.libelle_reseau="";
                    dr.titre_departement = "Préfet d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.nom_representant_legal = "LEBLANC Pierre";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.num_accuse_reception = "2343";
                    dr.identifiant_rnc2 = "S078Z345";
                    dr.pwd_rnc2 = "shrj9sqs";
                    dr.identifiant_otc = "cnr";
                    dr.pwd_otc = "xlx";
                    //dr.est_soumis_a_derogation = false;
                    dr.url_site_rnc2 = "https://www.utac-otc.com/rnc2";
                    dr.url_site_otc = "http://www.utac.com";
                    break;
                case 5:
                    //Centre indépendant CL sans annulation et avec modification de la dénomination sociale
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.est_centre_independant = true;
                    dr.ancien_numero_agrem = "S037W333";
                    dr.cod_agrem = "S037W334";
                    dr.cod_reseau = "Z";
                    //dr.libelle_reseau="";
                    dr.titre_departement = "Préfet d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.nom_representant_legal = "LEBLANC Pierre";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.num_accuse_reception = "2343";
                    dr.identifiant_rnc2 = "S078Z345";
                    dr.pwd_rnc2 = "shrj9sqs";
                    dr.identifiant_otc = "cnr";
                    dr.pwd_otc = "xlx";
                    //dr.est_soumis_a_derogation = false;
                    dr.url_site_rnc2 = "https://www.utac-otc.com/rnc2";
                    dr.url_site_otc = "http://www.utac.com";
                    break;
                default:

                    ViewData["message"] = "Erreur : Ajouter le parmètre ?num_test=1 ou 2 ou 3 ou 4 ou 5";
                    return View();
            }

            dt.Rows.Add(dr);
            //boReportDocument.SetDataSource((System.Data.DataTable)dt);
            boReportDocument.SetDataSource(ds);

            //Définition du format d'export PDF
            ExportOptions exportOptions = new ExportOptions();
            //This sets which format we will export to.
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //Create the FormatOptions class for PDF.
            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
            PDFexportOptions.FirstPageNumber = 1;
            PDFexportOptions.LastPageNumber = 2;
            PDFexportOptions.CreateBookmarksFromGroupTree = true;

            //Set the export format options with the page range we just set.
            exportOptions.FormatOptions = PDFexportOptions;

            //On exporte le document au format PDF
            //boReportDocument.Export(exportOptions);

            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

            MemoryStream memStream = new MemoryStream();

            byte[] respBuffer = new byte[1024];

            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            while (bytesRead > 0)
            {
                memStream.Write(respBuffer, 0, bytesRead);
                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            }

            //PLE 06/06/2012 libération du rapport
            boReportDocument.Close();
            return File(memStream.GetBuffer(), "application/pdf", "cntr_notification_decision_agrem_init_annexe2-A.pdf");

        }

        //Test de l'état Notification de modification d’agrément d’un centre spécialisé.
        //cntr_notification_decision_agrem_modif_annexe2-B
        public ActionResult etat2(int? num_test)
        {
            //Récupération des données

            //On charge le document Crystal
            ReportDocument boReportDocument = new ReportDocument();
            boReportDocument.Load(Server.MapPath("~/bin/reporting/cntr_notification_decision_agrem_modif_annexe2-B.rpt"));

            cntr_notification_decision_agrem_modif_annexe2_B ds = new cntr_notification_decision_agrem_modif_annexe2_B();
            cntr_notification_decision_agrem_modif_annexe2_B.notification_centreDataTable dt = ds.notification_centre;
            cntr_notification_decision_agrem_modif_annexe2_B.notification_centreRow dr = dt.Newnotification_centreRow();

            //PLE 06/03/2012
            // Ajout d'une ou plusieurs Dérogations
            cntr_notification_decision_agrem_modif_annexe2_B.pDerogationCentreDataTable dtDerog = ds.pDerogationCentre;
            cntr_notification_decision_agrem_modif_annexe2_B.pDerogationCentreRow drDerog1 = dtDerog.NewpDerogationCentreRow();
            drDerog1.dateDebut = Convert.ToDateTime("2012-12-12T00:00:00");
            drDerog1.dateFin = Convert.ToDateTime("2012-12-13T00:00:00");
            drDerog1.commentaire = "Commentaire N°1 VL /Commentaire N°1 VL /Commentaire N°1 /Commentaire N°1 /";
            drDerog1.categorieVehicule = Utac.Rnc2.BusinessObjects.rCategorieVehicule.CategorieEnum.VL.ToString();

            cntr_notification_decision_agrem_modif_annexe2_B.pDerogationCentreRow drDerog2 = dtDerog.NewpDerogationCentreRow();
            drDerog2.dateDebut = Convert.ToDateTime("2012-12-12T00:00:00");
            drDerog2.dateFin = Convert.ToDateTime("2012-12-13T00:00:00");
            drDerog2.commentaire = "Commentaire N°1 PL /Commentaire N°1 PL /Commentaire N°1 /Commentaire N°1 /";
            drDerog2.categorieVehicule = Utac.Rnc2.BusinessObjects.rCategorieVehicule.CategorieEnum.VL.ToString();

            switch (num_test)
            {
                case 1:
                    //Centre indépendant VL avec fermture définitive
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "S037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = true;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = false;
                    dtDerog.Rows.Add(drDerog1);
                    dtDerog.Rows.Add(drDerog2);
                    dr.est_soumis_derogation = true;
                    break;
                case 2:
                    //Centre indépendant PL avec chagement d'exploitant
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "S037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = true;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = false;
                    break;

                case 3:
                    //Centre indépendant CL avec chagement de localisation
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.cod_agrem = "S037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = true;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = false;
                    break;

                case 4:
                    //Centre indépendant CL avec chagement de rattachement
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.cod_agrem = "S037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = true;
                    dr.est_motif_chagement_denomination = false;
                    break;

                case 5:
                    //Centre indépendant CL avec chagement de dénomination commerciale
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.cod_agrem = "S037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.raison_sociale_nouveau = "NOUVELLE DENOMINATION CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = true;
                    dtDerog.Rows.Add(drDerog1);
                    dtDerog.Rows.Add(drDerog2);
                    dr.est_soumis_derogation = true;
                    break;

                default:

                    ViewData["message"] = "Erreur : Ajouter le parmètre ?num_test=1 ou 2 ou 3 ou 4 ou 5";
                    return View();

            }

            dt.Rows.Add(dr);
            boReportDocument.SetDataSource(ds);

            //Définition du format d'export PDF
            ExportOptions exportOptions = new ExportOptions();
            //This sets which format we will export to.
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //Create the FormatOptions class for PDF.
            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
            PDFexportOptions.FirstPageNumber = 1;
            PDFexportOptions.LastPageNumber = 2;
            PDFexportOptions.CreateBookmarksFromGroupTree = true;

            //Set the export format options with the page range we just set.
            exportOptions.FormatOptions = PDFexportOptions;

            //On exporte le document au format PDF
            //boReportDocument.Export(exportOptions);

            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

            MemoryStream memStream = new MemoryStream();

            byte[] respBuffer = new byte[1024];

            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            while (bytesRead > 0)
            {
                memStream.Write(respBuffer, 0, bytesRead);
                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            }

            //PLE 06/06/2012 libération du rapport
            boReportDocument.Close();
            return File(memStream.GetBuffer(), "application/pdf", "cntr_notification_decision_agrem_modif_annexe2-B.pdf");

        }

        //Test de l'état Notification  d’agrément d’une installation auxiliaire
        //auxi_notification_decision_agrem_init_annexe3-A
        public ActionResult etat3(int? num_test)
        {
            //Récupération des données

            //On charge le document Crystal
            ReportDocument boReportDocument = new ReportDocument();
            boReportDocument.Load(Server.MapPath("~/bin/reporting/auxi_notification_decision_agrem_init_annexe3-A.rpt"));

            auxi_notification_decision_agrem_init_annexe3_A ds = new auxi_notification_decision_agrem_init_annexe3_A();
            auxi_notification_decision_agrem_init_annexe3_A.notification_centreDataTable dt = ds.notification_centre;
            auxi_notification_decision_agrem_init_annexe3_A.notification_centreRow dr = dt.Newnotification_centreRow();

            //PLE 06/03/2012
            // Ajout d'une ou plusieurs Dérogations
            auxi_notification_decision_agrem_init_annexe3_A.pDerogationCentreDataTable dtDerog = ds.pDerogationCentre;
            auxi_notification_decision_agrem_init_annexe3_A.pDerogationCentreRow drDerog1 = dtDerog.NewpDerogationCentreRow();
            drDerog1.dateDebut = Convert.ToDateTime("2012-12-12T00:00:00");
            drDerog1.dateFin = Convert.ToDateTime("2012-12-13T00:00:00");
            drDerog1.commentaire = "Commentaire N°1 VL /Commentaire N°1 VL /Commentaire N°1 /Commentaire N°1 /";
            drDerog1.categorieVehicule = Utac.Rnc2.BusinessObjects.rCategorieVehicule.CategorieEnum.VL.ToString();

            auxi_notification_decision_agrem_init_annexe3_A.pDerogationCentreRow drDerog2 = dtDerog.NewpDerogationCentreRow();
            drDerog2.dateDebut = Convert.ToDateTime("2012-12-12T00:00:00");
            drDerog2.dateFin = Convert.ToDateTime("2012-12-13T00:00:00");
            drDerog2.commentaire = "Commentaire N°1 PL /Commentaire N°1 PL /Commentaire N°1 /Commentaire N°1 /";
            drDerog2.categorieVehicule = Utac.Rnc2.BusinessObjects.rCategorieVehicule.CategorieEnum.VL.ToString();

            switch (num_test)
            {
                case 1:
                    //Installation AUXI VL avec première demande d'agrément
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "A037D334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "D'INDRE ET LOIRE";
                    dr.raison_sociale = "GARAGE MARINI";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    //dr.date_echeance_notification = Convert.ToDateTime("2013-12-12T00:00:00");
                    dr.est_agrem_premiere_demande = true;
                    dr.est_agrem_renouvellement = false;
                    dr.est_agrem_modification = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    dr.num_accuse_reception = "1234";
                    dr.est_soumis_derogation = true;
                    dtDerog.Rows.Add(drDerog1);
                    dtDerog.Rows.Add(drDerog2);
                    break;
                case 2:
                    //Installation AUXI PL avec première demande de renouvellement
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "A037D334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "D'INDRE ET LOIRE";
                    dr.raison_sociale = "GARAGE MARINI LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.date_echeance_notification = Convert.ToDateTime("2013-12-12T00:00:00"); //obligatoire si PL
                    dr.est_agrem_premiere_demande = false;
                    dr.est_agrem_renouvellement = true;
                    dr.est_agrem_modification = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    //dr.num_accuse_reception = "1234"; //Lors d'un renouvellement l'installation conserve son numéro d'agrément
                    dr.est_soumis_derogation = false;
                    break;

                case 3:
                    //Installation AUXI VL avec modification d'agrément
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "A037D334";
                    dr.ancien_numero_agrem = "A037D333";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "D'INDRE ET LOIRE";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    //dr.date_echeance_notification = Convert.ToDateTime("2013-12-12T00:00:00");
                    dr.est_agrem_premiere_demande = false;
                    dr.est_agrem_renouvellement = false;
                    dr.est_agrem_modification = true;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    dr.num_accuse_reception = "1234";
                    dr.est_soumis_derogation = true;
                    break;

                case 4:
                    //Installation AUXI PL initiale
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "A037D334";
                    //dr.ancien_numero_agrem = "A037D333";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "D'INDRE ET LOIRE";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.date_echeance_notification = Convert.ToDateTime("2013-12-12T00:00:00");
                    dr.est_agrem_premiere_demande = true;
                    dr.est_agrem_renouvellement = false;
                    dr.est_agrem_modification = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    dr.num_accuse_reception = "1234";
                    dr.est_soumis_derogation = false;
                    break;

                case 5:
                    //Installation AUXI CL modification agrement
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.cod_agrem = "A037D334";
                    dr.ancien_numero_agrem = "A037D333";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "D'INDRE ET LOIRE";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    //dr.date_echeance_notification = Convert.ToDateTime("2013-12-12T00:00:00");
                    dr.est_agrem_premiere_demande = false;
                    dr.est_agrem_renouvellement = false;
                    dr.est_agrem_modification = true;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    dr.num_accuse_reception = "1234";
                    dr.est_soumis_derogation = false;
                    break;

                default:

                    ViewData["message"] = "Erreur : Ajouter le parmètre ?num_test=1 ou 2 ou 3 ou 4 ou 5";
                    return View();

            }

            dt.Rows.Add(dr);
            //boReportDocument.SetDataSource((System.Data.DataTable)dt);
            boReportDocument.SetDataSource(ds);

            //Définition du format d'export PDF
            ExportOptions exportOptions = new ExportOptions();
            //This sets which format we will export to.
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //Create the FormatOptions class for PDF.
            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
            PDFexportOptions.FirstPageNumber = 1;
            PDFexportOptions.LastPageNumber = 2;
            PDFexportOptions.CreateBookmarksFromGroupTree = true;

            //Set the export format options with the page range we just set.
            exportOptions.FormatOptions = PDFexportOptions;

            //On exporte le document au format PDF
            //boReportDocument.Export(exportOptions);

            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

            MemoryStream memStream = new MemoryStream();

            byte[] respBuffer = new byte[1024];

            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            while (bytesRead > 0)
            {
                memStream.Write(respBuffer, 0, bytesRead);
                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            }

            //PLE 06/06/2012 libération du rapport
            boReportDocument.Close();
            return File(memStream.GetBuffer(), "application/pdf", "auxi_notification_decision_agrem_init_annexe3-A.pdf");

        }

        //Test de l'état Notification de modification d’agrément d’une installation auxiliaire.
        //auxi_notification_decision_agrem_modif_annexe3-B
        public ActionResult etat4(int? num_test)
        {
            //Récupération des données

            //On charge le document Crystal
            ReportDocument boReportDocument = new ReportDocument();
            boReportDocument.Load(Server.MapPath("~/bin/reporting/auxi_notification_decision_agrem_modif_annexe3-B.rpt"));

            auxi_notification_decision_agrem_modif_annexe3_B ds = new auxi_notification_decision_agrem_modif_annexe3_B();
            auxi_notification_decision_agrem_modif_annexe3_B.notification_centreDataTable dt = ds.notification_centre;
            auxi_notification_decision_agrem_modif_annexe3_B.notification_centreRow dr = dt.Newnotification_centreRow();

            switch (num_test)
            {
                case 1:
                    //auxialiaire VL avec fermture définitive
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "A037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = true;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = false;
                    dr.est_motif_agrem_PL_non_renouvelle = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    break;
                case 2:
                    //Auxilaire PL avec chagement d'exploitant
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "A037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = true;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = false;
                    dr.est_motif_agrem_PL_non_renouvelle = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    break;

                case 3:
                    //Auxilaire CL avec chagement de localisation
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.cod_agrem = "A037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = true;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = false;
                    dr.est_motif_agrem_PL_non_renouvelle = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    break;

                case 4:
                    //Auxiliaire CL avec chagement de rattachement
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.cod_agrem = "A037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    //dr.raison_sociale_nouveau = "";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = true;
                    dr.est_motif_chagement_denomination = false;
                    dr.est_motif_agrem_PL_non_renouvelle = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    break;

                case 5:
                    //Auxalire CL avec chagement de dénomination commerciale
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.cod_agrem = "A037Z334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.raison_sociale_nouveau = "NOUVELLE DENOMINATION CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = true;
                    dr.est_motif_agrem_PL_non_renouvelle = false;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    break;

                case 6:
                    //Auxalire PL non renouvellement de l'agrément PL
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.cod_agrem = "A037K334";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.raison_sociale = "AUTO SUPER CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    //dr.raison_sociale_nouveau = "NOUVELLE DENOMINATION CENTER CONTROLE LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.l1_adresse = "Ligne adresse 1";
                    dr.l2_adresse = "Ligne adresse 2";
                    dr.cp_centre = "92260";
                    dr.ville = "FONTENAY AUX ROSES";
                    dr.num_tel = "0169801617";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_fermeture = false;
                    dr.est_motif_changement_exploitant = false;
                    dr.est_motif_changement_localisation = false;
                    dr.est_motif_changement_rattachement = false;
                    dr.est_motif_chagement_denomination = false;
                    dr.est_motif_agrem_PL_non_renouvelle = true;
                    dr.libelle_reseau = "DEKRA AUTOMOTIVE";
                    break;

                default:

                    ViewData["message"] = "Erreur : Ajouter le parmètre ?num_test=1 ou 2 ou 3 ou 4 ou 5 ou 6";
                    return View();

            }

            dt.Rows.Add(dr);
            boReportDocument.SetDataSource((System.Data.DataTable)dt);

            //Définition du format d'export PDF
            ExportOptions exportOptions = new ExportOptions();
            //This sets which format we will export to.
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //Create the FormatOptions class for PDF.
            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
            PDFexportOptions.FirstPageNumber = 1;
            PDFexportOptions.LastPageNumber = 2;
            PDFexportOptions.CreateBookmarksFromGroupTree = true;

            //Set the export format options with the page range we just set.
            exportOptions.FormatOptions = PDFexportOptions;

            //On exporte le document au format PDF
            //boReportDocument.Export(exportOptions);

            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

            MemoryStream memStream = new MemoryStream();

            byte[] respBuffer = new byte[1024];

            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            while (bytesRead > 0)
            {
                memStream.Write(respBuffer, 0, bytesRead);
                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            }

            //PLE 06/06/2012 libération du rapport
            boReportDocument.Close();
            return File(memStream.GetBuffer(), "application/pdf", "auxi_notification_decision_agrem_modif_annexe3-B.pdf");

        }

        //Test de l'état Notification initiale d’agrément d’un controleurForm/prestataire.
        //cntlr_notification_decision_agrem_init_annexe1-AB
        public ActionResult etat5(int? num_test)
        {
            //Récupération des données

            //On charge le document Crystal
            ReportDocument boReportDocument = new ReportDocument();
            boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_notification_decision_agrem_init_annexe1-AB.rpt"));

            cntlr_notification_decision_agrem_init_annexe1_AB ds = new cntlr_notification_decision_agrem_init_annexe1_AB();
            cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurDataTable dt = ds.notification_controleur;
            cntlr_notification_decision_agrem_init_annexe1_AB.notification_controleurRow dr = dt.Newnotification_controleurRow();

            switch (num_test)
            {
                case 1:
                    //controleurForm VL non prestataire
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_prestataire = false;
                    dr.cod_agrem_controleur = "037Z2222";
                    dr.nom = "LEBLANC";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach = "S037Z333";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    break;
                case 2:
                    //controleurForm VL prestataire
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_prestataire = true;
                    dr.cod_agrem_controleur = "037Z1234";
                    dr.nom = "LEBLANC LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach = "S037Z123";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    break;
                case 3:
                    //controleurForm PL prestataire
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.est_prestataire = true;
                    dr.cod_agrem_controleur = "037Z1234";
                    dr.nom = "LEBLANC LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach = "S037Z123";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    break;

                case 4:

                    //controleurForm CL non prestataire
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.est_prestataire = true;
                    dr.cod_agrem_controleur = "037Z1234";
                    dr.nom = "LEBLANC LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach = "S037Z123";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    break;

                default:

                    ViewData["message"] = "Erreur : Ajouter le paramètre ?num_test=1 ou 2 ou 3 ou 4";
                    return View();

            }

            dt.Rows.Add(dr);
            boReportDocument.SetDataSource((System.Data.DataTable)dt);

            //Définition du format d'export PDF
            ExportOptions exportOptions = new ExportOptions();
            //This sets which format we will export to.
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //Create the FormatOptions class for PDF.
            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
            PDFexportOptions.FirstPageNumber = 1;
            PDFexportOptions.LastPageNumber = 2;
            PDFexportOptions.CreateBookmarksFromGroupTree = true;

            //Set the export format options with the page range we just set.
            exportOptions.FormatOptions = PDFexportOptions;

            //On exporte le document au format PDF
            //boReportDocument.Export(exportOptions);

            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

            MemoryStream memStream = new MemoryStream();

            byte[] respBuffer = new byte[1024];

            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            while (bytesRead > 0)
            {
                memStream.Write(respBuffer, 0, bytesRead);
                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            }

            //PLE 06/06/2012 libération du rapport
            boReportDocument.Close();
            return File(memStream.GetBuffer(), "application/pdf", "cntlr_notification_decision_agrem_init_annexe1-AB.pdf");

        }

        //Test de l'état Notification de modification d’agrément d’un controleurForm/prestataire.
        //cntlr_notification_decision_agrem_modif_annexe1-CD
        public ActionResult etat6(int? num_test)
        {
            //Récupération des données

            //On charge le document Crystal
            ReportDocument boReportDocument = new ReportDocument();
            boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_notification_decision_agrem_modif_annexe1-CD.rpt"));

            cntlr_notification_decision_agrem_modif_annexe1_CD ds = new cntlr_notification_decision_agrem_modif_annexe1_CD();
            cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurDataTable dt = ds.notification_controleur;
            cntlr_notification_decision_agrem_modif_annexe1_CD.notification_controleurRow dr = dt.Newnotification_controleurRow();

            switch (num_test)
            {
                case 1:
                    //controleurForm VL non prestataire : annulation de l'agrément
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_prestataire = false;
                    dr.cod_agrem_controleur = "037Z2222";
                    dr.nom = "LEBLANC";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach_ancien = "S037Z333";
                    dr.cod_agrem_centre_rattach_nouveau = null;
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_cessation_activite = true;
                    dr.est_motif_chang_centre_rattach = false;
                    break;
                case 2:
                    //controleurForm VL prestataire : changement de centre de rattachement
                    dr.est_type_agrem_vl = true;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = false;
                    dr.est_prestataire = true;
                    dr.cod_agrem_controleur = "037Z1234";
                    dr.nom = "LEBLANC LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach_ancien = "S037Z333";
                    dr.cod_agrem_centre_rattach_nouveau = "S974D123";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_cessation_activite = false;
                    dr.est_motif_chang_centre_rattach = true;
                    break;
                case 3:
                    //controleurForm PL non prestataire : annulation de l'agrément
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = true;
                    dr.est_type_agrem_cl = false;
                    dr.est_prestataire = false;
                    dr.cod_agrem_controleur = "037Z2222";
                    dr.nom = "LEBLANC";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach_ancien = "S037Z333";
                    dr.cod_agrem_centre_rattach_nouveau = null;
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_cessation_activite = true;
                    dr.est_motif_chang_centre_rattach = false;
                    break;

                case 4:
                    //controleurForm CL prestataire : changement de centre de rattachement
                    dr.est_type_agrem_vl = false;
                    dr.est_type_agrem_pl = false;
                    dr.est_type_agrem_cl = true;
                    dr.est_prestataire = true;
                    dr.cod_agrem_controleur = "037Z1234";
                    dr.nom = "LEBLANC LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG LONG";
                    dr.prenom = "Pierre";
                    dr.date_naissance = Convert.ToDateTime("1968-04-20T00:00:00");
                    dr.cod_agrem_centre_rattach_ancien = "S037Z333";
                    dr.cod_agrem_centre_rattach_nouveau = "S974D123";
                    dr.titre_departement = "PREFET d'Indre et Loire";
                    dr.libelle_prefet = "d'Indre et Loire";
                    dr.date_effet_notification = Convert.ToDateTime("2011-12-12T00:00:00");
                    dr.est_motif_cessation_activite = false;
                    dr.est_motif_chang_centre_rattach = true;
                    break;

                default:

                    ViewData["message"] = "Erreur : Ajouter le paramètre ?num_test=1 ou 2 ou 3 ou 4";
                    return View();

            }

            dt.Rows.Add(dr);
            boReportDocument.SetDataSource((System.Data.DataTable)dt);

            //Définition du format d'export PDF
            ExportOptions exportOptions = new ExportOptions();
            //This sets which format we will export to.
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //Create the FormatOptions class for PDF.
            PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
            PDFexportOptions.FirstPageNumber = 1;
            PDFexportOptions.LastPageNumber = 2;
            PDFexportOptions.CreateBookmarksFromGroupTree = true;

            //Set the export format options with the page range we just set.
            exportOptions.FormatOptions = PDFexportOptions;

            //On exporte le document au format PDF
            //boReportDocument.Export(exportOptions);

            Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

            MemoryStream memStream = new MemoryStream();

            byte[] respBuffer = new byte[1024];

            int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            while (bytesRead > 0)
            {
                memStream.Write(respBuffer, 0, bytesRead);
                bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
            }

            //PLE 06/06/2012 libération du rapport
            boReportDocument.Close();
            return File(memStream.GetBuffer(), "application/pdf", "cntlr_notification_decision_agrem_modif_annexe1-CD.pdf");

        }

        //Test de l'état Notification de modification d’agrément d’un controleurForm/prestataire.
        //cntlr_notification_decision_agrem_modif_annexe1-CD
        public ActionResult SerializeControleur(int? id)
        {
            pContoleurEncour controleur;
            controleur = this.pControleurService.FindBy(id);
            controleur.RPrefecture.PCentreEncours.Clear();
            controleur.RPrefecture.PCentreHistos.Clear();
            controleur.RPrefecture.PContoleurEncours.Clear();
            controleur.RPrefecture.PControleurHistos.Clear();
            controleur.RPrefecture.PContoleurEncours.Clear();

            controleur.RTypeDemandeControleur.PContoleurEncours.Clear();
            controleur.RTypeDemandeControleur.PControleurHistos.Clear();
            controleur.RUtilisateur = null;
            controleur.RHabilitation = null;

            DSficheControleur ds = new DSficheControleur();
            ds.mapControleurDataset(controleur);

            if (controleur == null)
            {
                ViewData["message"] = "Erreur : ID controleur inconnu";
                return View();

            }
            else
            {
                //controleur.UnproxyObjectTree(SessionFactory.GetSessionFactory(), 1);

                //System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
                ////System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(controleur.GetType());
                //System.Runtime.Serialization.NetDataContractSerializer xmlSerializer = new System.Runtime.Serialization.NetDataContractSerializer();
                //using (MemoryStream xmlStream = new MemoryStream())
                //{
                //    xmlSerializer.Serialize(xmlStream, controleur);
                //    xmlStream.Position = 0;
                //    return File(xmlStream.GetBuffer(), "application/xml", "XML_ficheControleur_" + Convert.ToString(id) + ".xml");
                //}

                return View();

            }
        }
        public ActionResult FicheControleur(int? id)
        {
            {
                pContoleurEncour controleur;
                controleur = this.pControleurService.FindBy(id);

                if (controleur == null)
                {
                    ViewData["message"] = "Erreur : ID controleur inconnu";
                    return View();

                }
                else
                {
                    //On charge le document Crystal
                    ReportDocument boReportDocument = new ReportDocument();
                    boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_ficheControleur.rpt"));

                    // Chargement du DATASET de la fiche contrôleur dans l'état crystal
                    DSficheControleur ds = new DSficheControleur();
                    boReportDocument.SetDataSource(ds.mapControleurDataset(controleur));

                    //Définition du format d'export PDF
                    ExportOptions exportOptions = new ExportOptions();
                    //This sets which format we will export to.
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //Create the FormatOptions class for PDF.
                    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                    PDFexportOptions.FirstPageNumber = 1;
                    PDFexportOptions.LastPageNumber = 2;
                    PDFexportOptions.CreateBookmarksFromGroupTree = true;

                    //Set the export format options with the page range we just set.
                    exportOptions.FormatOptions = PDFexportOptions;

                    //On exporte le document au format PDF
                    //boReportDocument.Export(exportOptions);

                    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                    MemoryStream memStream = new MemoryStream();

                    byte[] respBuffer = new byte[1024];

                    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);
                        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    }

                    //PLE 06/06/2012 libération du rapport
                    boReportDocument.Close();
                    return File(memStream.GetBuffer(), "application/pdf", "ficheControleur_" + Convert.ToString(id) + ".pdf");

                    //return View();
                }
            }

        }

        public ActionResult FicheControleurSuiviQualificationPL(int? id)
        {

            pContoleurEncour controleur;
            controleur = this.pControleurService.FindBy(id);

            if (controleur == null)
            {
                ViewData["message"] = "Erreur : ID controleur inconnu";
                return View();

            }
            else
            {

                //Récupération des données
                this._controleurStatistiqueService = NhibernateServiceFactory.getInstance().getStatistiquesControleurService();
                DateTime? datedebut = null;
                DateTime? datefin = null;
                int anneeNMoins1 = 0;
                ItemListeSuiviQualificationPL item = this._controleurStatistiqueService.SuivisQualificationControleurs(controleur, ref datedebut, ref datefin, ref anneeNMoins1);

                //On charge le document Crystal
                ReportDocument boReportDocument = new ReportDocument();
                boReportDocument.Load(Server.MapPath("~/bin/reporting/cntlr_ficheControleurSuiviQualificationPL.rpt"));

                DSficheControleurSuiviQualificationPL ds = new DSficheControleurSuiviQualificationPL();
                DSficheControleurSuiviQualificationPL.dtItemListeSuiviQualificationPLDataTable dt = ds.dtItemListeSuiviQualificationPL;
                DSficheControleurSuiviQualificationPL.dtItemListeSuiviQualificationPLRow dr = dt.NewdtItemListeSuiviQualificationPLRow();
                if (item != null)
                {
                    dr.CodAgrement = item.CodAgrement;
                    dr.CodeAgrementCentre = item.CodeAgrementCentre;
                    dr.CodeDepartement = item.CodeDepartement;
                    dr.CodeHabilitation = item.CodeHabilitation;
                    dr.CodReseau = item.CodReseau;
                    dr.CumulActiviteQ1 = item.CumulActiviteQ1;
                    dr.CumulActiviteQ2 = item.CumulActiviteQ2;
                    dr.CumulActiviteQ3 = item.CumulActiviteQ3;
                    dr.DateAgrement = item.DateAgrement;
                    dr.DateDebutHabilitation = item.DateDebutHabilitation;
                    dr.DateDebutRattachementCentre = item.DateDebutRattachementCentre;
                    dr.DateDernierAuditQ1 = item.DateDernierAuditQ1;
                    dr.DateDernierAuditQ2 = item.DateDernierAuditQ2;
                    dr.DateDernierAuditQ3 = item.DateDernierAuditQ3;
                    dr.DateFormationQ1 = item.DateFormationQ1;
                    dr.DateFormationQ2 = item.DateFormationQ2;
                    dr.DateFormationQ3 = item.DateFormationQ3;
                    dr.DateNaissance = item.DateNaissance.Value;
                    dr.LibelleRegion = item.LibelleRegion;
                    dr.Nom = item.Nom;
                    dr.Prenom = item.Prenom;
                    dr.DateDebutPeriode = datedebut.Value;
                    dr.DateFinPeriode = datefin.Value;
                    //27/08/2018 -PLE- Suivi qualification contrôleur PL
                    dr.CumulActiviteQ1AnneeMoins1 = item.CumulActiviteQ1AnneeMoins1;
                    dr.CumulActiviteQ2AnneeMoins1 = item.CumulActiviteQ2AnneeMoins1;
                    dr.CumulActiviteQ3AnneeMoins1 = item.CumulActiviteQ3AnneeMoins1;
                    dr.anneeNMoins1 = anneeNMoins1;
                    dt.Rows.Add(dr);
                }
                // Chargement du DATASET de la fiche contrôleur dans l'état crystal

                boReportDocument.SetDataSource(ds);

                //Définition du format d'export PDF
                ExportOptions exportOptions = new ExportOptions();
                //This sets which format we will export to.
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                //Create the FormatOptions class for PDF.
                PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                PDFexportOptions.FirstPageNumber = 1;
                PDFexportOptions.LastPageNumber = 2;
                PDFexportOptions.CreateBookmarksFromGroupTree = true;

                //Set the export format options with the page range we just set.
                exportOptions.FormatOptions = PDFexportOptions;

                //On exporte le document au format PDF
                //boReportDocument.Export(exportOptions);

                Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                MemoryStream memStream = new MemoryStream();

                byte[] respBuffer = new byte[1024];

                int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                while (bytesRead > 0)
                {
                    memStream.Write(respBuffer, 0, bytesRead);
                    bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                }

                //PLE 06/06/2012 libération du rapport
                boReportDocument.Close();
                return File(memStream.GetBuffer(), "application/pdf", "ficheControleurSuiviQualificationPL_" + Convert.ToString(id) + ".pdf");
            }
        }

        public ActionResult FicheInstallation(int? id)
        {
            {
                pCentreEncour centre;
                centre = this.pCentreService.FindBy(id);

                if (centre == null)
                {
                    ViewData["message"] = "Erreur : ID centre inconnu";
                    return View();

                }
                else
                {

                    //On charge le document Crystal
                    ReportDocument boReportDocument = new ReportDocument();
                    boReportDocument.Load(Server.MapPath("~/bin/reporting/cntr_ficheInstallation.rpt"));

                    // Chargement du DATASET de la fiche contrôleur dans l'état crystal
                    DSficheInstallation ds = new DSficheInstallation();
                    boReportDocument.SetDataSource(ds.mapInstallationDataset(centre));

                    //Définition du format d'export PDF
                    ExportOptions exportOptions = new ExportOptions();
                    //This sets which format we will export to.
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    //Create the FormatOptions class for PDF.
                    PdfFormatOptions PDFexportOptions = new PdfFormatOptions();
                    PDFexportOptions.FirstPageNumber = 1;
                    PDFexportOptions.LastPageNumber = 2;
                    PDFexportOptions.CreateBookmarksFromGroupTree = true;

                    //Set the export format options with the page range we just set.
                    exportOptions.FormatOptions = PDFexportOptions;

                    //On exporte le document au format PDF
                    //boReportDocument.Export(exportOptions);

                    Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);

                    MemoryStream memStream = new MemoryStream();

                    byte[] respBuffer = new byte[1024];

                    int bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    while (bytesRead > 0)
                    {
                        memStream.Write(respBuffer, 0, bytesRead);
                        bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                    }

                    //PLE 06/06/2012 libération du rapport
                    boReportDocument.Close();
                    return File(memStream.GetBuffer(), "application/pdf", "ficheInstallation_" + Convert.ToString(id) + ".pdf");

                    //return View();
                }
            }

        }

        public ActionResult PvCtvlPdf(string p_numpv, string p_cod_agrem_centre, int? p_annee, int? p_mois, int? p_jour, short? p_niveaudetail)
        {
            /* format URL appel
             * http://localhost:8090/Test/PvCtvlPdf?p_numpv=1300035&p_cod_agrem_centre=S000D000&p_annee=2013&p_mois=10&p_jour=2&p_niveaudetail=2
             */

            ViewData["message"] = "pas d'information !";
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil pvCtWebUtil = new Utac.Rnc2.UI.Web.Utils.PvCtWebUtil(Server.MapPath("~/bin"));

            MemoryStream mstr = null;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT codRetour;

            string numpv, cod_agrem_centre;
            DateTime date_ctrl;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;

            if (p_numpv == null || p_numpv.Length == 0)
            {
                numpv = "13052997";
                cod_agrem_centre = "S057T065";
                date_ctrl = new DateTime(2013, 06, 1);
                niveauDetail = Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
            }
            else
            {
                try
                {
                    numpv = p_numpv;
                    cod_agrem_centre = p_cod_agrem_centre;
                    date_ctrl = new DateTime(p_annee.Value, p_mois.Value, p_jour.Value);
                    if (p_niveaudetail != null)
                    {
                        niveauDetail = (Utils.PvCtWebUtil.EnumNiveauDetailPVCT)p_niveaudetail;
                    }
                    else
                    {
                        niveauDetail = Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
                    }
                }
                catch (Exception ex)
                {
                    ViewData["message"] = "Erreur sur les paramètres d'entrée ! " + ex.Message;
                    return View();
                }
            }

            codRetour = pvCtWebUtil.recupereCtvlPV(numpv, cod_agrem_centre, date_ctrl, niveauDetail, ref mstr);

            if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT))
            {
                return File(mstr.GetBuffer(), "application/pdf", "otc_verification_pv_ctvl_" + cod_agrem_centre + "_" + numpv + ".pdf");
            }
            else
                if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT))
            {
                ViewData["message"] = "Affichage du PV CTVL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
                ViewData["message2"] = "CT non trouvé en base.";
            }
            else
                    if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE))
            {
                ViewData["message"] = "Affichage du PV CTVL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
                ViewData["message2"] = " Une erreur technique s'est produite !";
            }

            return View();
        }

        public ActionResult PvCtvlPdfInterface()
        {
            return View();
        }

        public ActionResult NewPvCtvlPdf(string p_numpv, string p_cod_agrem_centre, int? p_annee, int? p_mois, int? p_jour, short? p_niveaudetail)
        {
            /* format URL appel
             * http://localhost:8090/Test/NewPvCtvlPdf?p_numpv=18263963&p_cod_agrem_centre=S999F998&p_annee=2018&p_mois=05&p_jour=27&p_niveaudetail=2
             */

            ViewData["message"] = "pas d'information !";
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil pvCtWebUtil = new Utac.Rnc2.UI.Web.Utils.PvCtWebUtil(Server.MapPath("~/bin"));

            MemoryStream mstr = null;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT codRetour;

            string numpv, cod_agrem_centre;
            DateTime date_ctrl;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;

            if (p_numpv == null || p_numpv.Length == 0)
            {
                numpv = "18263963";
                cod_agrem_centre = "S999F998";
                date_ctrl = new DateTime(2018, 05, 27);
                niveauDetail = Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
            }
            else
            {
                try
                {
                    numpv = p_numpv;
                    cod_agrem_centre = p_cod_agrem_centre;
                    date_ctrl = new DateTime(p_annee.Value, p_mois.Value, p_jour.Value);
                    if (p_niveaudetail != null)
                    {
                        niveauDetail = (Utils.PvCtWebUtil.EnumNiveauDetailPVCT)p_niveaudetail;
                    }
                    else
                    {
                        niveauDetail = Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
                    }
                }
                catch (Exception ex)
                {
                    ViewData["message"] = "Erreur sur les paramètres d'entrée ! " + ex.Message;
                    return View();
                }
            }

            codRetour = pvCtWebUtil.NewRecupereCtvlPV(numpv, cod_agrem_centre, date_ctrl, niveauDetail, ref mstr);

            if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT))
            {
                return File(mstr.GetBuffer(), "application/pdf", "otc_verification_pv_ctvl_" + cod_agrem_centre + "_" + numpv + ".pdf");
            }
            else
                if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT))
            {
                ViewData["message"] = "Affichage du PV CTVL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
                ViewData["message2"] = "CT non trouvé en base.";
            }
            else
                    if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE))
            {
                ViewData["message"] = "Affichage du PV CTVL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
                ViewData["message2"] = " Une erreur technique s'est produite !";
            }

            return View();
        }

        public ActionResult PvCtplPdf(string p_numpv, string p_cod_agrem_centre, int? p_annee, int? p_mois, int? p_jour, short? p_niveaudetail)
        {
            /* format URL appel
             * http://localhost:8090/Test/PvCtplPdf?p_numpv=18263963&p_cod_agrem_centre=S999F998&p_annee=2018&p_mois=05&p_jour=27&p_niveaudetail=2
             */

            ViewData["message"] = "pas d'information !";
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil pvCtWebUtil = new Utac.Rnc2.UI.Web.Utils.PvCtWebUtil(Server.MapPath("~/bin"));

            MemoryStream mstr = null;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT codRetour;

            string numpv, cod_agrem_centre;
            DateTime date_ctrl;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;

            if (p_numpv == null || p_numpv.Length == 0)
            {
                numpv = "18263963";
                cod_agrem_centre = "S999F998";
                date_ctrl = new DateTime(2018, 05, 27);
                niveauDetail = Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
            }
            else
            {
                try
                {
                    numpv = p_numpv;
                    cod_agrem_centre = p_cod_agrem_centre;
                    date_ctrl = new DateTime(p_annee.Value, p_mois.Value, p_jour.Value);
                    if (p_niveaudetail != null)
                    {
                        niveauDetail = (Utils.PvCtWebUtil.EnumNiveauDetailPVCT)p_niveaudetail;
                    }
                    else
                    {
                        niveauDetail = Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
                    }
                }
                catch (Exception ex)
                {
                    ViewData["message"] = "Erreur sur les paramètres d'entrée ! " + ex.Message;
                    return View();
                }
            }

            codRetour = pvCtWebUtil.RecupereCtplPV(numpv, cod_agrem_centre, date_ctrl, niveauDetail, ref mstr);

            if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT_PL))
            {
                return File(mstr.GetBuffer(), "application/pdf", "otc_verification_pv_ctpl_" + cod_agrem_centre + "_" + numpv + ".pdf");
            }
            else
                if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL))
            {
                ViewData["message"] = "Affichage du PV CTPL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
                ViewData["message2"] = "CT non trouvé en base.";
            }
            else
                    if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL))
            {
                ViewData["message"] = "Affichage du PV CTPL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
                ViewData["message2"] = " Une erreur technique s'est produite !";
            }

            return View();
        }

        public ActionResult ListePvCtvl(string p_immat, string p_serial_no)
        {
            /* format URL appel
             * http://localhost:8090/Test/ListePvCtvl?p_immat=BC-658-EB&p_serial_no=VF37ANFZ230674442&p
             */

            ViewData["message"] = "pas d'information !";
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil pvCtWebUtil = new Utac.Rnc2.UI.Web.Utils.PvCtWebUtil(Server.MapPath("~/bin"));

            MemoryStream mstr = null;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT codRetour;

            string numpv, cod_agrem_centre;
            DateTime date_ctrl;
            Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT niveauDetail;

            //if (p_numpv == null || p_numpv.Length == 0)
            //{
            //    numpv = "18263963";
            //    cod_agrem_centre = "S999F998";
            //    date_ctrl = new DateTime(2018, 05, 27);
            //    niveauDetail = Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
            //}
            //else
            //{
            //    try
            //    {
            //        numpv = p_numpv;
            //        cod_agrem_centre = p_cod_agrem_centre;
            //        date_ctrl = new DateTime(p_annee.Value, p_mois.Value, p_jour.Value);
            //        if (p_niveaudetail != null)
            //        {
            //            niveauDetail = (Utils.PvCtWebUtil.EnumNiveauDetailPVCT)p_niveaudetail;
            //        }
            //        else
            //        {
            //            niveauDetail = Utils.PvCtWebUtil.EnumNiveauDetailPVCT.TOUT;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        ViewData["message"] = "Erreur sur les paramètres d'entrée ! " + ex.Message;
            //        return View();
            //    }
            //}

            Utils.WebServiceResult res = pvCtWebUtil.Ws_FindByImmatOrNumSerie(p_immat, p_serial_no, Utac.Rnc2.UI.Web.Enums.CategorieVehicule.VL);
            codRetour = res.ResultCode;

            if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.TROUVE_CT))
            {
                if (!String.IsNullOrEmpty(p_immat) && !String.IsNullOrEmpty(p_serial_no))
                    ViewData["message"] = "Résultat de recherche de contrôles VL pour l'immatriculation " + p_immat + " et/ou le n° de série " + p_serial_no;
                else if (!String.IsNullOrEmpty(p_immat))
                    ViewData["message"] = "Résultat de recherche de contrôles VL pour l'immatriculation " + p_immat;
                else if (!String.IsNullOrEmpty(p_serial_no))
                    ViewData["message"] = "Résultat de recherche de contrôles VL pour le n° de série " + p_serial_no;
                else
                    ViewData["message"] = "Paramètres incorrects (nuls)";
                Utac.Rnc2.UI.Web.ServiceRnc2.VlPControle[] cts = (Utac.Rnc2.UI.Web.ServiceRnc2.VlPControle[])res.ResultValue;
                if (cts != null)
                    ViewData["message2"] = cts.Length.ToString() + " contrôle(s) VL trouvé(s) en base.";
                else
                    ViewData["message2"] = "Contrôles VL non trouvés en base.";
            }
            //else
            //{
            //    if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.AUCUN_CT_PL))
            //    {
            //        ViewData["message"] = "Affichage du PV CTPL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
            //        ViewData["message2"] = "CT non trouvé en base.";
            //    }
            //    else
            //    {
            //        if (codRetour.Equals(Utac.Rnc2.UI.Web.Utils.PvCtWebUtil.EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL))
            //        {
            //            ViewData["message"] = "Affichage du PV CTPL : (numéro pv=" + numpv + " Installation=" + cod_agrem_centre + " Date ctlr=" + date_ctrl.ToShortDateString() + ")";
            //            ViewData["message2"] = " Une erreur technique s'est produite !";
            //        }
            //    }
            //}
            return View("message");
        }
    }

    public static class NHibernateProxyUtils
    {
        /// <summary>
        /// Force initialization of a proxy or persistent collection.
        /// </summary>
        /// <param name="persistentObject">a persistable object, proxy, persistent collection or null</param>
        /// <exception cref="HibernateException">if we can't initialize the proxy at this time, eg. the Session was closed</exception>
        public static T Unproxy<T>(this T persistentObject)
        {
            var proxy = persistentObject as INHibernateProxy;

            if (proxy != null)
                return (T)proxy.HibernateLazyInitializer.GetImplementation();

            return persistentObject;
        }

        /// <summary>
        /// Gets the underlying class type of a persistent object that may be proxied
        /// </summary>
        public static Type GetUnproxiedType<T>(this T persistentObject)
        {
            var proxy = persistentObject as INHibernateProxy;
            if (proxy != null)
                return proxy.HibernateLazyInitializer.PersistentClass;

            return persistentObject.GetType();
        }

        /// <summary>
        /// Force initialzation of a possibly proxied object tree up to the maxDepth.
        /// Once the maxDepth is reached, entity properties will be replaced with
        /// placeholder objects having only the identifier property populated.
        /// </summary>
        public static T UnproxyObjectTree<T>(this T persistentObject, ISessionFactory sessionFactory, int maxDepth)
        {
            // Determine persistent type of the object
            var persistentType = persistentObject.GetUnproxiedType();

            var classMetadata = sessionFactory.GetClassMetadata(persistentType);

            // If we've already reached the max depth, we will return a placeholder object
            if (maxDepth < 0)
                return CreatePlaceholder(persistentObject, persistentType, classMetadata);

            // Now lets go ahead and make sure everything is unproxied
            var unproxiedObject = persistentObject.Unproxy();

            // Iterate through each property and unproxy entity types
            for (int i = 0; i < classMetadata.PropertyTypes.Length; i++)
            {
                var nhType = classMetadata.PropertyTypes[i];
                var propertyName = classMetadata.PropertyNames[i];
                var propertyInfo = persistentType.GetProperty(propertyName);

                // Unproxy of collections is not currently supported.  We set the collection property to null.
                if (nhType.IsCollectionType)
                {
                    //propertyInfo.SetValue(unproxiedObject, null, null);
                    //continue;
                    // Unproxy of collections is supported. Iterate over the collection and unproxy in turn.

                    //var propertyValue = propertyInfo.GetValue(unproxiedObject, null);

                    //System.Collections.ICollection c = propertyValue as System.Collections.ICollection;
                    //foreach (var item in c)
                    //{
                    //    if (propertyValue == null)
                    //    continue;
                    //    item.UnproxyObjectTree(sessionFactory, maxDepth - 1);
                    //}
                    //continue;

                    var propertyValue = propertyInfo.GetValue(unproxiedObject, null);
                    System.Collections.ICollection c = propertyValue as System.Collections.ICollection;
                    Type[] typeArgs = c.GetType().GetGenericArguments();
                    Type algTypeGen = System.Reflection.Assembly.Load("mscorlib").GetType("System.Collections.Generic.List`1");
                    Type algType = algTypeGen.MakeGenericType(typeArgs);

                    var l = Activator.CreateInstance(algType);

                    IList lst = l as IList;
                    foreach (var item in c)
                    {
                        if (propertyValue == null)
                            continue;

                        //item.UnproxyObjectTree(sessionFactory,maxDepth-1);
                        lst.Add(item.UnproxyObjectTree(sessionFactory, maxDepth - 1));
                    }
                    propertyInfo.SetValue(unproxiedObject, lst, null);
                    continue;

                }

                if (nhType.IsEntityType)
                {
                    var propertyValue = propertyInfo.GetValue(unproxiedObject, null);

                    if (propertyValue == null)
                        continue;

                    propertyInfo.SetValue(
                        unproxiedObject,
                        propertyValue.UnproxyObjectTree(sessionFactory, maxDepth - 1),
                        null
                        );
                }
            }

            return unproxiedObject;
        }

        /// <summary>
        /// Return an empty placeholder object with the Identifier set.  We can safely access the identifier
        /// property without the object being initialized.
        /// </summary>
        private static T CreatePlaceholder<T>(T persistentObject, Type persistentType, IClassMetadata classMetadata)
        {
            var placeholderObject = (T)Activator.CreateInstance(persistentType);

            if (classMetadata.HasIdentifierProperty)
            {
                var identifier = classMetadata.GetIdentifier(persistentObject);
                classMetadata.SetIdentifier(placeholderObject, identifier);
            }

            return placeholderObject;
        }
    }

}
