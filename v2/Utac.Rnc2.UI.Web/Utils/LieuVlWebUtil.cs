using System;
using System.IdentityModel.Selectors;
//using System.Web.Services.Protocols;

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

using log4net;
using Utac.Rnc2.UI.Web.ctvlpv;
//using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Utils
{
    /// <summary>
    /// Gestion de l'édition du PDF "LIEU DE PASSAGE DU DERNIER CONTROLE D'UN VEHICULE LEGER"
    /// </summary>
    class LieuVlWebUtil
    {
        private readonly ILog log = LogManager.GetLogger(typeof(LieuVlWebUtil));

        //Fichier XSLT de transformation
        private readonly string ctsFileXSLWS_ctvlpv = "\\reporting\\XSLWS_ctvlpv.xslt";
        //Fichier PVCTVL crystal Report
        private readonly string ctsFileRptPvTransfertCtvl = "\\reporting\\PvTransfertCTVL.rpt";

        public LieuVlWebUtil()
        {
            throw new Exception("Constructeur interdit");
        }

        /// <summary>
        /// Constructeur à utiliser obligatoirement
        /// </summary>
        /// <param name="repertoireRessources"> Server.MapPath("~/bin")</param>
        public LieuVlWebUtil(string repertoireRessources)
        {
            //initialisation des chemins réels aux ressources
            ctsFileXSLWS_ctvlpv = repertoireRessources + ctsFileXSLWS_ctvlpv;
            ctsFileRptPvTransfertCtvl = repertoireRessources + ctsFileRptPvTransfertCtvl;
        }

        /// <summary>
        /// Méthode public de récupération du contenu du fichier PDF à imprimer
        /// </summary>
        /// <param name="nomPrenom">nom et prénom du demandeur</param>
        /// <param name="immatVeh">immatriculation du véhicule</param>
        /// <param name="numSerie">n° de série</param>
        /// <param name="dateCircu">date de première immatriculation</param>
        /// <returns></returns>
        public EnumCodeRetourLieuVl recupereLieuVl(string nomPrenom, ref string immatVeh, ref string numSerie, DateTime dateCircu, ref string codeCentre)
        {

            log.Info("Recupération d'un lieu VL (méthode recupereLieuVl), nomPrenom=" + nomPrenom + " immatVeh=" + immatVeh + " DateCircu=" + dateCircu.ToShortDateString() + "numSerie=" + numSerie + " Début ...");

            EnumCodeRetourLieuVl codeRetour = EnumCodeRetourLieuVl.ERREUR_TECHNIQUE;

            //BeanLieuOut beanLieuOut = null;
            BeanLieuIn input = new BeanLieuIn();
            input.dateAppelWS = DateTime.Now;
            input.dateCircu = new DateTime?(dateCircu);
            input.immatVeh = immatVeh;
            input.nomPrenom = nomPrenom;
            input.numSerie = numSerie;

            //Appel du web service
            BeanLieuOut output = this.appel_WS_lieuvl(input);

            if (output != null)
            {
                codeRetour = (EnumCodeRetourLieuVl)output.resultatRecherche.Value;
            }

            if (codeRetour.Equals(EnumCodeRetourLieuVl.TROUVE_CT))
            {
                codeCentre = output.codAgremCentre;
                immatVeh = output.immatVeh;
                numSerie = output.numSerie;
            }
            log.Info("Recupération d'un lieu VL (méthode recupereLieuVl), Résultat=" + codeRetour);
            log.Info("Recupération d'un lieu VL (méthode recupereLieuVl), nomPrenom=" + nomPrenom + " immatVeh=" + immatVeh + " DateCircu=" + dateCircu.ToShortDateString() + "numSerie=" + numSerie + " Début ...");
            return codeRetour;
        }
        /// <summary>
        /// Appel de la méthode RecherchePV du service web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private BeanLieuOut appel_WS_lieuvl(BeanLieuIn input)
        {
            log.Debug("Appel web service Début...");
            BeanLieuOut beanLieuOut = null;
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = CheckServeurCertificat;
                //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                //ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                //{
                //    if ((certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST")))
                //        return true;
                //    else
                //    {
                //        log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                //        return false;
                //    }

                //};

                Utac.Rnc2.UI.Web.ctvlpv.WS_ctvlpvPortTypeClient clientWeb = new Utac.Rnc2.UI.Web.ctvlpv.WS_ctvlpvPortTypeClient();

                try
                {
                    beanLieuOut = clientWeb.rechercheLieu(input);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (clientWeb != null) { clientWeb.Close(); }
                }

            }
            catch (TimeoutException ex)
            {
                //Web service en time out
                log.Error("L'appel du web service est en time out : " + ex.Message);

            }
            catch (System.ServiceModel.FaultException<Exception> ex)
            {
                log.Error("L'appel du web service est en erreur : " + ex.Detail.Message);
            }
            catch (System.ServiceModel.FaultException ex)
            {
                //Exception inconnu
                log.Error("L'appel du web service est en erreur (Exception inconnu): " + ex.Message);

            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                string strMessageInnerEx = "Pas de innerException !";
                if (ex.InnerException != null)
                {
                    strMessageInnerEx = ex.InnerException.Message;
                }
                log.Error("L'appel du web service est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
            }
            catch (ArgumentException ex)
            {
                log.Error("Erreur de paramètrage dans le web.config: " + ex.Message);
            }

            log.Debug("Appel web service Fin...");
            return beanLieuOut;
        }

        public enum EnumNiveauDetailLieuVl
        {
            TOUT = 1,
            REDUIT = 2
        }

        /// <summary>
        /// Code retour du service web de récupération des infos contrôle technique
        /// </summary>
        public enum EnumCodeRetourLieuVl
        {
            AUCUN_CT = 0,
            TROUVE_CT = 1,
            ERREUR_TECHNIQUE = 2
        }



    }
    public class MyX509CertificateValidator : X509CertificateValidator
    {

        public override void Validate(X509Certificate2 certificate)
        {
            {
                if (certificate == null)
                {
                    throw new ArgumentNullException("certificate");
                }
                if (!(certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST")))
                {
                    throw new ArgumentNullException("certificate");
                }

            };
        }
    }
}
