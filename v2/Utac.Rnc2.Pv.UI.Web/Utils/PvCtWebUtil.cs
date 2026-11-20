using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using CrystalDecisions.Shared;
using log4net;

using Utac.Rnc2.Pv.UI.Web.ctvlpv;
using Utac.Rnc2.UI.Web.DataSets;
using Utac.Rnc2.UI.Web.Enums;
using Utac.Rnc2.UI.Web.ServiceRnc2;

namespace Utac.Rnc2.Pv.UI.Web.Utils
{
    /// <summary>
    /// Gestion de l'édition du PDF "VERIFICATION d'UN CONTROLE TECHNIQUE DE VEHICULE LEGER"
    /// </summary>
    class PvCtWebUtil
    {
        private readonly ILog log = LogManager.GetLogger(typeof(PvCtWebUtil));

        //Fichier XSLT de transformation
        //private readonly string ctsFileXSLWS_ctvlpv = "\\reporting\\XSLWS_ctvlpv.xslt";
        //Fichier PVCTVL crystal Report
        //private readonly string ctsFileRptPvTransfertCtvl = "\\reporting\\PvTransfertCTVL.rpt";

        /// <summary>
        /// Constructeur à utiliser obligatoirement
        /// </summary>
        /// <param name="repertoireRessources"> Server.MapPath("~/bin")</param>
        public PvCtWebUtil(string repertoireRessources)
        {
            //initialisation des chemins réels aux ressources
            //ctsFileXSLWS_ctvlpv = repertoireRessources + ctsFileXSLWS_ctvlpv;
            //ctsFileRptPvTransfertCtvl = repertoireRessources + ctsFileRptPvTransfertCtvl;
        }

        /// <summary>
        /// Méthode public de récupération du contenu du fichier PDF à imprimer
        /// </summary>
        /// <param name="numPvCtrl">numéro du PV de contrôle</param>
        /// <param name="codAgremCentre">code agrément du centre de contrôle</param>
        /// <param name="dateCtrl">date du contrôle</param>
        /// <param name="niveauDetail">niveaud de détail d'impression</param>
        /// <param name="pvPDF">flux binaire contenant le PDF à renvoyer au navigateur</param>
        /// <returns></returns>
        //public EnumCodeRetourPVCT recupereCtvlPV(string numPvCtrl, string codAgremCentre, DateTime dateCtrl, EnumNiveauDetailPVCT niveauDetail, ref MemoryStream pvPDF)
        //{

        //    log.Info("Recupération d'un PV VL (méthode recupereCtvlPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Début ...");

        //    pvPDF = null;
        //    var codeRetour = EnumCodeRetourPVCT.ERREUR_TECHNIQUE;

        //    var input = new BeanPvIn();
        //    input.numPvCtrl = numPvCtrl;
        //    input.codAgremCentre = codAgremCentre;
        //    input.dateCtrl = new DateTime?(dateCtrl);
        //    var output = this.appel_WS_ctvlpv(input);

        //    if (output != null)
        //    {
        //        codeRetour = (EnumCodeRetourPVCT)output.resultatRecherche.Value;
        //    }

        //    if (codeRetour.Equals(EnumCodeRetourPVCT.TROUVE_CT))
        //    {
        //        string xmlStr;
        //        xmlStr = SerialiseUTF8(output);

        //        if (xmlStr != null)
        //        {
        //            xmlStr = TransformFLuxPV(xmlStr);

        //            if (xmlStr != null)
        //            {
        //                var ds = genereDataSetCtvlPv(xmlStr);

        //                if (ds != null)
        //                {
        //                    pvPDF = genereCtvlPvPdf(ds, niveauDetail);
        //                }
        //            }
        //        }

        //        if (pvPDF == null)
        //        {
        //            codeRetour = EnumCodeRetourPVCT.ERREUR_TECHNIQUE;
        //        }
        //    }

        //    log.Info("Recupération d'un PV VL (méthode recupereCtvlPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Fin ...");
        //    return codeRetour;
        //}

        /// <summary>
        /// Appel de la méthode RecherchePV du service web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private BeanPvOut appel_WS_ctvlpv(BeanPvIn input)
        {
            log.Debug("Appel web service Début...");
            BeanPvOut beanPvOut = null;
            try
            {
                //ServicePointManager.ServerCertificateValidationCallback = CheckServeurCertificat;
                //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                {
                    if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                        return true;

                    log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                    return false;
                };

                var clientWeb = new WS_ctvlpvPortTypeClient();

                var ws = new Rnc2Client();

                try
                {
                    beanPvOut = clientWeb.recherchePv(input);

                    if (ws != null && input.dateCtrl.HasValue)
                    {
                        var ctrls = ws.findVlByNumPvCodAgrDateCtrl(input.numPvCtrl, input.codAgremCentre, input.dateCtrl.Value);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    clientWeb.Close();
                    ws.Close();
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
                var strMessageInnerEx = "Pas de innerException !";
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
            return beanPvOut;
        }

        /// <summary>
        /// Méthode public de récupération du contenu du fichier PDF à imprimer
        /// </summary>
        /// <param name="numPvCtrl">numéro du PV de contrôle</param>
        /// <param name="codAgremCentre">code agrément du centre de contrôle</param>
        /// <param name="dateCtrl">date du contrôle</param>
        /// <param name="niveauDetail">niveaud de détail d'impression</param>
        /// <param name="pvPDF">flux binaire contenant le PDF à renvoyer au navigateur</param>
        /// <returns></returns>
        public EnumCodeRetourPVCT NewRecupereCtvlPV(string numPvCtrl, string codAgremCentre, DateTime dateCtrl, EnumNiveauDetailPVCT niveauDetail, ref MemoryStream pvPDF)
        {

            log.Info("Recupération d'un PV VL (méthode NewRecupereCtvlPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Début ...");

            pvPDF = null;

            var output = this.WsCtvlPv(numPvCtrl, codAgremCentre, dateCtrl);

            if (output.ResultCode.Equals(EnumCodeRetourPVCT.TROUVE_CT))
            {
                pvPDF = NewGenereCtvlPvPdf(output.ResultValue, niveauDetail);

                if (pvPDF == null)
                {
                    output.ResultCode = EnumCodeRetourPVCT.ERREUR_TECHNIQUE;
                }
            }

            log.Info("Recupération d'un PV VL (méthode NewRecupereCtvlPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Fin ...");
            return output.ResultCode;
        }

        /// <summary>
        /// Appel de la méthode RecherchePV du service web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private WebServiceResult WsCtvlPv(String numPvCtrl, String codAgremCentre, DateTime dateCtrl)
        {
            log.Debug("Appel web service transverse Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE);
            if (!String.IsNullOrWhiteSpace(numPvCtrl) && !String.IsNullOrWhiteSpace(codAgremCentre) && dateCtrl != null)
            {
                log.Debug(String.Format("Paramètres numPvCtrl={0}; codAgremCentre={1}; dateCtrl={2}", numPvCtrl, codAgremCentre, dateCtrl.ToString()));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;
                        else
                        {
                            log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                            return false;
                        }
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        //Utac.Rnc2.UI.Web.ServiceRnc2.pControle[] ctrls = ws.findVlByNumPvCodAgrDateCtrl(numPvCtrl, codAgremCentre, dateCtrl.Value);
                        result.ResultValue = ws.findVlByNumPvCodAgrDateCtrl(numPvCtrl, codAgremCentre, dateCtrl);
                        result.ResultCode = result.ResultValue != null ? EnumCodeRetourPVCT.TROUVE_CT : EnumCodeRetourPVCT.AUCUN_CT;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("L'appel du web service transverse est en time out : " + ex.Message);

                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);

                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("Paramètres transmis au webservice incorrects numPvCtrl={0}; codAgremCentre={1}; dateCtrl={2}",
                    numPvCtrl ?? "null",
                    codAgremCentre ?? "null",
                    dateCtrl.ToString(CultureInfo.InvariantCulture)));
            }

            log.Debug(String.Format("Appel web service transverse Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }

        private WebServiceResult WsCtvlPv(long numVisit, int origine)
        {
            log.Debug("Appel web service transverse Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE);
            if (numVisit > 0 && origine > 0 && origine < 3)
            {
                log.Debug(String.Format("Paramètres numVisit={0:d}; origine={1:d}", numVisit, origine));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;

                        log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                        return false;
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        var ctrl = ws.findVlByNumVisitOrigine(numVisit, origine);
                        if (ctrl != null)
                        {
                            var ctrls = new VlPControle[1];
                            ctrls[0] = ctrl;
                            result.ResultValue = ctrls;
                            result.ResultCode = EnumCodeRetourPVCT.TROUVE_CT;
                        }
                        else
                        {
                            result.ResultValue = null;
                            result.ResultCode = EnumCodeRetourPVCT.AUCUN_CT;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("L'appel du web service transverse est en time out : " + ex.Message);

                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);

                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("Paramètres transmis au webservice incorrects numVisit={0:d}; origine={1:d}", numVisit, origine));
            }
            log.Debug(String.Format("Appel web service transverse Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }

        /// <summary>
        /// Méthode publique de récupération du contenu du fichier PDF à imprimer (PL)
        /// </summary>
        /// <param name="numPvCtrl">numéro du PV de contrôle</param>
        /// <param name="codAgremCentre">code agrément du centre de contrôle</param>
        /// <param name="dateCtrl">date du contrôle</param>
        /// <param name="niveauDetail">niveaud de détail d'impression</param>
        /// <param name="pvPDF">flux binaire contenant le PDF à renvoyer au navigateur</param>
        /// <returns></returns>
        public EnumCodeRetourPVCT RecupereCtplPV(string numPvCtrl, string codAgremCentre, DateTime dateCtrl, EnumNiveauDetailPVCT niveauDetail, ref MemoryStream pvPDF)
        {

            log.Info("Recupération d'un PV PL (méthode RecupereCtplPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Début ...");

            pvPDF = null;

            var output = this.WsCtplPv(numPvCtrl, codAgremCentre, dateCtrl);

            if (output.ResultCode.Equals(EnumCodeRetourPVCT.TROUVE_CT_PL))
            {
                pvPDF = GenereCtplPvPdf(output.ResultValue, niveauDetail);

                if (pvPDF == null)
                {
                    output.ResultCode = EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL;
                }
            }

            log.Info("Recupération d'un PV PL (méthode RecupereCtplPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Fin ...");
            return output.ResultCode;
        }

        /// <summary>
        /// Appel de la méthode RecherchePV PL du service web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private WebServiceResult WsCtplPv(String numPvCtrl, String codAgremCentre, DateTime dateCtrl)
        {
            log.Debug("PL Appel web service transverse Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL);
            if (!String.IsNullOrWhiteSpace(numPvCtrl) && !String.IsNullOrWhiteSpace(codAgremCentre) && dateCtrl != null)
            {
                log.Debug(String.Format("PL Paramètres numPvCtrl={0}; codAgremCentre={1}; dateCtrl={2}", numPvCtrl, codAgremCentre, dateCtrl.ToString()));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;
                        else
                        {
                            log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                            return false;
                        }
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        if (ws != null)
                        {
                            result.ResultValue = ws.findPlByNumPvCodAgrDateCtrl(numPvCtrl, codAgremCentre, dateCtrl);
                            result.ResultCode = result.ResultValue != null ? EnumCodeRetourPVCT.TROUVE_CT_PL : EnumCodeRetourPVCT.AUCUN_CT_PL;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("PL L'appel du web service transverse est en time out : " + ex.Message);
                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("PL L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("PL L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);
                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("PL L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("PL Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("PL Paramètres transmis au webservice incorrects numPvCtrl={0}; codAgremCentre={1}; dateCtrl={2}",
                    numPvCtrl ?? "null",
                    codAgremCentre ?? "null",
                    dateCtrl.ToString(CultureInfo.InvariantCulture)));
            }
            log.Debug(String.Format("PL Appel web service transverse Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }

        private WebServiceResult WsCtplPv(long numVisit, int origine)
        {
            log.Debug("PL Appel web service transverse Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL);
            if (numVisit > 0 && origine > 0 && origine < 3)
            {
                log.Debug(String.Format("PL Paramètres numVisit={0:d}; origine={1:d}", numVisit, origine));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;
                        else
                        {
                            log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                            return false;
                        }
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        if (ws != null)
                        {
                            var ctrl = ws.findPlByNumVisitOrigine(numVisit, origine);
                            if (ctrl != null)
                            {
                                var ctrls = new PlPControle[1];
                                ctrls[0] = ctrl;
                                result.ResultValue = ctrls;
                                result.ResultCode = EnumCodeRetourPVCT.TROUVE_CT_PL;
                            }
                            else
                            {
                                result.ResultValue = null;
                                result.ResultCode = EnumCodeRetourPVCT.AUCUN_CT_PL;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("PL L'appel du web service transverse est en time out : " + ex.Message);
                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("PL L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("PL L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);

                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("PL L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("PL Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("PL Paramètres transmis au webservice incorrects numVisit={0:d}; origine={1:d}", numVisit, origine));
            }
            log.Debug(String.Format("PL Appel web service transverse Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }

        private WebServiceResult WsCtclPv(long numVisit, int origine)
        {
            log.Debug("CL Appel web service transverse Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL);
            if (numVisit > 0 && origine > 0 && origine < 3)
            {
                log.Debug(String.Format("CL Paramètres numVisit={0:d}; origine={1:d}", numVisit, origine));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;
                        else
                        {
                            log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                            return false;
                        }
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        if (ws != null)
                        {
                            var ctrl = ws.findClByNumVisitOrigine(numVisit, origine);
                            if (ctrl != null)
                            {
                                var ctrls = new ClPcontrole[1];
                                ctrls[0] = ctrl;
                                result.ResultValue = ctrls;
                                result.ResultCode = EnumCodeRetourPVCT.TROUVE_CT_CL;
                            }
                            else
                            {
                                result.ResultValue = null;
                                result.ResultCode = EnumCodeRetourPVCT.AUCUN_CT_CL;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("CL L'appel du web service transverse est en time out : " + ex.Message);
                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("CL L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("CL L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);

                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("CL L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("CL Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("CL Paramètres transmis au webservice incorrects numVisit={0:d}; origine={1:d}", numVisit, origine));
            }
            log.Debug(String.Format("CL Appel web service transverse Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        public EnumCodeRetourPVCT GetCtvlPV(long numVisit, int origine, EnumNiveauDetailPVCT niveauDetail, ref MemoryStream pvPDF, ref String id1, ref String id2, ref DateTime dc)
        {
            log.Info("Recupération d'un PV VL (méthode GetCtvlPV), numVisit=" + numVisit.ToString() + " origine=" + origine.ToString() + " Début ...");

            pvPDF = null;

            var output = this.WsCtvlPv(numVisit, origine);

            if (output.ResultCode.Equals(EnumCodeRetourPVCT.TROUVE_CT))
            {
                var e = output.ResultValue.GetEnumerator();
                if (e != null && e.MoveNext())
                {
                    var ctrl = (VlPControle)e.Current;
                    id1 = ctrl.numPvCtrl;
                    id2 = ctrl.codAgremCentre;
                    dc = ctrl.dateCtrl;
                    pvPDF = NewGenereCtvlPvPdf(output.ResultValue, niveauDetail);
                }
                if (pvPDF == null)
                {
                    output.ResultCode = EnumCodeRetourPVCT.ERREUR_TECHNIQUE;
                }
            }

            log.Info("Recupération d'un PV VL (méthode GetCtvlPV), numVisit=" + numVisit.ToString() + " origine=" + origine.ToString() + " Fin ...");
            return output.ResultCode;
        }

        public EnumCodeRetourPVCT GetCtplPV(long numVisit, int origine, EnumNiveauDetailPVCT niveauDetail, ref MemoryStream pvPDF, ref String id1, ref String id2, ref DateTime dc)
        {

            log.Info("Recupération d'un PV PL (méthode GetCtplPV), numVisit=" + numVisit.ToString() + " origine=" + origine.ToString() + " Début ...");

            pvPDF = null;

            var output = this.WsCtplPv(numVisit, origine);

            if (output.ResultCode.Equals(EnumCodeRetourPVCT.TROUVE_CT_PL))
            {
                var e = output.ResultValue.GetEnumerator();
                if (e != null && e.MoveNext())
                {
                    var ctrl = (PlPControle)e.Current;
                    id1 = ctrl.numPvCtrl;
                    id2 = ctrl.codAgremCentre;
                    dc = ctrl.dateCtrl;
                    pvPDF = GenereCtplPvPdf(output.ResultValue, niveauDetail);
                }
                if (pvPDF == null)
                {
                    output.ResultCode = EnumCodeRetourPVCT.ERREUR_TECHNIQUE_PL;
                }
            }

            log.Info("Recupération d'un PV PL (méthode GetCtplPV), numVisit=" + numVisit.ToString() + " origine=" + origine.ToString() + " Fin ...");
            return output.ResultCode;
        }

        public EnumCodeRetourPVCT GetCtclPV(long numVisit, int origine, EnumNiveauDetailPVCT niveauDetail, ref MemoryStream pvPDF, ref String id1, ref String id2, ref DateTime dc)
        {

            log.Info("Recupération d'un PV CL (méthode GetCtclPV), numVisit=" + numVisit.ToString() + " origine=" + origine.ToString() + " Début ...");

            pvPDF = null;

            var output = this.WsCtclPv(numVisit, origine);

            if (output.ResultCode.Equals(EnumCodeRetourPVCT.TROUVE_CT_CL))
            {
                var e = output.ResultValue.GetEnumerator();
                if (e != null && e.MoveNext())
                {
                    var ctrl = (ClPcontrole)e.Current;
                    id1 = ctrl.numPvCtrl;
                    id2 = ctrl.codAgremCentre;
                    dc = ctrl.dateCtrl;
                    pvPDF = GenereCtclPvPdf(output.ResultValue, niveauDetail);
                }
                if (pvPDF == null)
                {
                    output.ResultCode = EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL;
                }
            }

            log.Info("Recupération d'un PV CL (méthode GetCtclPV), numVisit=" + numVisit.ToString() + " origine=" + origine.ToString() + " Fin ...");
            return output.ResultCode;
        }

        /// <summary>
        /// Sériablisation de l'objet BeanPvOut
        /// </summary>
        /// <param name="instance">intance de l'objet à sérialiser</param>
        /// <returns>Chaine de caractère sérialisé en XML</returns>
        private String SerialiseUTF8(BeanPvOut instance)
        {
            log.Debug("SerialisationUTF8 Début...");
            var xmls = new System.Xml.Serialization.XmlSerializer(typeof(BeanPvOut));

            string xmlStr = null;
            try
            {
                using (var ms = new MemoryStream())
                {
                    var settings = new System.Xml.XmlWriterSettings();
                    settings.Encoding = Encoding.UTF8;
                    settings.Indent = true;
                    settings.IndentChars = "\t";
                    settings.NewLineChars = Environment.NewLine;
                    settings.ConformanceLevel = System.Xml.ConformanceLevel.Document;
                    using (var writer = System.Xml.XmlWriter.Create(ms, settings))
                    {
                        xmls.Serialize(writer, instance);
                    }
                    var msArray = ms.ToArray();
                    xmlStr = Encoding.UTF8.GetString(msArray, 3, msArray.Length - 3);
                }
            }
            catch (InvalidOperationException ex)
            {
                log.Error("Erreur SerializeUTF8 InvalidOperationException: " + ex.Message);
            }
            catch (Exception ex)
            {
                log.Error("Erreur SerializeUTF8 Inconnue: " + ex.Message);
            }
            log.Debug("SerialisationUTF8 Fin...");
            return xmlStr;
        }

        /// <summary>
        /// Transforme le XML selon le ficheir de transformation XSLWS_ctvlpv.xslt
        /// </summary>
        /// <param name="BeanPVOutStr"></param>
        /// <returns></returns>
        //private String TransformFLuxPV(String BeanPVOutStr)
        //{
        //    log.Debug("TransformFluxPV Début...");
        //    string stout = null;

        //    var settings = new System.Xml.Xsl.XsltSettings(true, false);

        //    // Create the writer.
        //    var wsettings = new System.Xml.XmlWriterSettings();
        //    wsettings.Encoding = Encoding.UTF8;
        //    wsettings.Indent = true;
        //    wsettings.IndentChars = "\t";

        //    try
        //    {
        //        // Load the style sheet.
        //        var xslt = new System.Xml.Xsl.XslCompiledTransform();
        //        xslt.Load(ctsFileXSLWS_ctvlpv, settings, new System.Xml.XmlUrlResolver());

        //        using (var ms = new MemoryStream())
        //        {
        //            var writer = System.Xml.XmlWriter.Create(ms, wsettings);
        //            var reader = System.Xml.XmlReader.Create(new StringReader(BeanPVOutStr));

        //            try
        //            {
        //                reader.MoveToContent();

        //                // Execute the transformation.
        //                xslt.Transform(reader, writer);

        //                var msArray = ms.ToArray();
        //                stout = Encoding.UTF8.GetString(msArray, 3, msArray.Length - 3);

        //            }
        //            catch (ArgumentNullException ex)
        //            {
        //                log.Error("Erreur de transformation XSLT : ArgumentnullException " + ex.Message);
        //            }
        //            catch (System.Xml.Xsl.XsltException ex)
        //            {
        //                log.Error("Erreur de transformation XSLT : XsltException " + "ligne=" + ex.LineNumber + " position=" + ex.LinePosition + " " + ex.Message);
        //            }
        //            finally
        //            {
        //                writer.Close();
        //                reader.Close();
        //            }
        //        }
        //    }
        //    catch (FileNotFoundException ex)
        //    {
        //        log.Error("Erreur de transformation XSLT : FileNotFoundException " + ex.FileName + " " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error("Erreur de transformation XSLT : Inconnu " + ex.Message);
        //    }

        //    log.Debug("TransformFluxPV Fin...");
        //    return stout;
        //}

        private DSlistPcontrole genereDataSetCtvlPv(string xmlString)
        {
            log.Debug("genereDataSetCtvlPv Début...");
            var ds = new DSlistPcontrole();

            try
            {
                var strR = new StringReader(xmlString);
                ds.ReadXml(strR);
            }
            catch (Exception ex)
            {
                log.Error("Erreur lors de la génération du dataset : Inconnu " + ex.Message);
                ds = null;
            }
            log.Debug("genereDataSetCtvlPv Fin...");
            return ds;
        }

        private MemoryStream genereCtvlPvPdf(DataSet ds, EnumNiveauDetailPVCT niveauDetailParam)
        {
            log.Debug("genereCtvlPvPdf Début...");
            //On charge le document Crystal
            //ReportDocument boReportDocument = new ReportDocument();

            var pvTransfertCTVL = new Rnc2.Web.Pv.Reporting.PvTransfertCTVL();
            var memStream = new MemoryStream();

            try
            {
                log.Debug("genereCtvlPvPdf.Document.load Début...");
                //boReportDocument.Load(ctsFileRptPvTransfertCtvl);
                log.Debug("genereCtvlPvPdf.Document.load Fin...");

                //boReportDocument.SetDataSource(ds);
                pvTransfertCTVL.SetDataSource(ds);
                //boReportDocument.SetParameterValue("niveauDetailParam", niveauDetailParam.GetHashCode());
                pvTransfertCTVL.SetParameterValue("niveauDetailParam", niveauDetailParam.GetHashCode());

                //Stream pdfStream = boReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                var pdfStream = pvTransfertCTVL.ExportToStream(ExportFormatType.PortableDocFormat);

                var respBuffer = new byte[1024];

                var bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                while (bytesRead > 0)
                {
                    memStream.Write(respBuffer, 0, bytesRead);
                    bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                }

            }
            catch (CrystalReportsException ex)
            {
                memStream.Close();
                memStream = null;

                var strMessageInnerEx = "Pas de innerException !";
                if (ex.InnerException != null)
                {
                    strMessageInnerEx = ex.InnerException.Message;
                }

                log.Error("Erreur lors de la génération de l'édition crystal (CrystalReportsException) " + ex.Message + " " + strMessageInnerEx);
            }
            catch (Exception ex)
            {
                memStream.Close();
                memStream = null;

                log.Error("Erreur lors de la génération de l'édition crystal (Exception) " + ex.Message);
            }
            finally
            {
                pvTransfertCTVL.Close();
                pvTransfertCTVL.Dispose();
            }
            log.Debug("genereCtvlPvPdf Fin...");
            return memStream;
        }

        private MemoryStream NewGenereCtvlPvPdf(System.Collections.IEnumerable col, EnumNiveauDetailPVCT niveauDetailParam)
        {
            log.Debug("NewGenereCtvlPvPdf Début...");
            //On charge le document Crystal
            //ReportDocument boReportDocument = new ReportDocument();

            //26/08/2024 PLE Ajout Historique km + compteurs d'exception
            //var pvTransfertCTVL = new Rnc2.Web.Pv.Reporting.NewPvTransfertCTVL();
            var pvTransfertCTVL = new Rnc2.Web.Pv.Reporting.PvTransfertCTVL();

            var memStream = new MemoryStream();

            try
            {
                log.Debug("NewGenereCtvlPvPdf.Document.load Début...");
                //boReportDocument.Load(ctsFileRptPvTransfertCtvl);
                log.Debug("NewGenereCtvlPvPdf.Document.load Fin...");
                #region R&D code générique (JMA)
                //boReportDocument.SetDataSource(ds);
                //pvTransfertCTVL.SetDataSource(col);

                //foreach (Table t in pvTransfertCTVL.Database.Tables)
                //{
                //    System.Diagnostics.Debug.WriteLine(t.Location);
                //}

                //foreach (pControle p in ((pControle[])col))
                //{
                //    PropertyInfo[] props = p.GetType().GetProperties();
                //    foreach (PropertyInfo pi in props)
                //    {
                //        System.Diagnostics.Debug.WriteLine("Nom : {0}; Type : {1}", pi.Name, pi.PropertyType.ToString());
                //        if (pi.PropertyType.IsArray)
                //        {
                //            System.Diagnostics.Debug.WriteLine("Enumerable");
                //            IEnumerable<object> a = (IEnumerable<object>)pi.GetValue(p, null);
                //            if (a != null)
                //            {
                //                // at least foreach now is available
                //                foreach (object o in a)
                //                {
                //                    // get the value
                //                    System.Diagnostics.Debug.WriteLine(o.ToString());
                //                }
                //            }
                //        }
                //    }
                //    break;
                //}
                #endregion
                //Create dictionaries for entities storage (dictionaries usage avoid duplicate entries when key is relevant, ie primary key, for current entity)
                var rDocs = new Dictionary<string, VlRDoc>();
                var pMesuress = new Dictionary<string, VlPMesures>();
                var pControles = new Dictionary<string, VlPControle>();
                var pTransferts = new Dictionary<string, VlPtransfert>();
                var rResultCtrls = new Dictionary<string, VlRResultCtrl>();
                var pDocConstates = new Dictionary<string, VlPDocConstate>();
                var rLocalisations = new Dictionary<string, VlRLocalisation>();
                var rMaterielStatuts = new Dictionary<string, VlRMaterielStatut>();
                var rNatureControles = new Dictionary<string, VlRNatureControle>();
                var rNatureCtrlSuivants = new Dictionary<string, VlRNatureControle>();
                var pLocalisationDocs = new Dictionary<string, VlPLocalisationDoc>();
                var pVehiculePresentes = new Dictionary<string, VlPVehiculePresente>();
                var rDocumentIdentifications = new Dictionary<string, VlRDocumentIdentification>();
                //26/08/2024 PLE Ajout Historique km + compteurs d'exception
                var pHistoCtrl = new Dictionary<string, pHistoCtrl>();
                var pAno = new Dictionary<string, pAno>();
                var rAno = new Dictionary<string, rAno>();
                //Fill dictionaries
                foreach (var p in (VlPControle[])col)
                {
                    if (p != null)
                    {
                        pControles[String.Format("{0:d}-{1:d}", p.numVisit, p.origine)] = p;
                        System.Diagnostics.Debug.WriteLine(String.Format("p_controle : {0:d}-{1:d}", p.numVisit, p.origine));
                        if (p.RNatureControle != null)
                        {
                            rNatureControles[String.Format("{0}", p.RNatureControle.codNatureCtrl)] = p.RNatureControle;
                            rNatureCtrlSuivants[String.Format("{0}", p.RNatureControle.codNatureCtrl)] = p.RNatureControle;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_nature_controle (actuel) : {0}", p.RNatureControle.codNatureCtrl));
                        }
                        if (p.RNatureCtrlSuivant != null)
                        {
                            rNatureControles[String.Format("{0}", p.RNatureCtrlSuivant.codNatureCtrl)] = p.RNatureCtrlSuivant;
                            rNatureCtrlSuivants[String.Format("{0}", p.RNatureCtrlSuivant.codNatureCtrl)] = p.RNatureCtrlSuivant;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_nature_controle (suivant) : {0}", p.RNatureCtrlSuivant.codNatureCtrl));
                        }
                        if (p.RResultCtrl != null)
                        {
                            rResultCtrls[String.Format("{0}", p.RResultCtrl.codResultCtrl)] = p.RResultCtrl;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_result_ctrl : {0}", p.RResultCtrl.codResultCtrl));
                        }
                        if (p.PVehiculePresente != null)
                        {
                            pVehiculePresentes[String.Format("{0:d}-{1:d}", p.PVehiculePresente.numVisit, p.PVehiculePresente.origine)] = p.PVehiculePresente;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_vehicule_presente : {0:d}-{1:d}", p.PVehiculePresente.numVisit, p.PVehiculePresente.origine));
                        }
                        if (p.RDocumentIdentification != null)
                        {
                            rDocumentIdentifications[String.Format("{0}", p.RDocumentIdentification.codDocument)] = p.RDocumentIdentification;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_document_identification : {0}", p.RDocumentIdentification.codDocument));
                        }
                        if (p.PMesures != null)
                        {
                            pMesuress[String.Format("{0:d}-{1:d}", p.PMesures.numVisit, p.PMesures.origine)] = p.PMesures;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_mesures : {0:d}-{1:d}", p.PMesures.numVisit, p.PMesures.origine));
                        }
                        if (p.RMaterielStatutFrl != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutFrl.codStatutMateriel)] = p.RMaterielStatutFrl;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Frl) : {0:d}", p.RMaterielStatutFrl.codStatutMateriel));
                        }
                        if (p.RMaterielStatutOpa != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutOpa.codStatutMateriel)] = p.RMaterielStatutOpa;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Opa) : {0:d}", p.RMaterielStatutOpa.codStatutMateriel));
                        }
                        if (p.RMaterielStatutAdg != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutAdg.codStatutMateriel)] = p.RMaterielStatutAdg;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Adg) : {0:d}", p.RMaterielStatutAdg.codStatutMateriel));
                        }
                        if (p.RMaterielStatutObd != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutObd.codStatutMateriel)] = p.RMaterielStatutObd;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Obd) : {0:d}", p.RMaterielStatutObd.codStatutMateriel));
                        }
                        if (p.RMaterielStatutRip != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutRip.codStatutMateriel)] = p.RMaterielStatutRip;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Rip) : {0:d}", p.RMaterielStatutRip.codStatutMateriel));
                        }
                        if (p.RMaterielStatutReg != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutReg.codStatutMateriel)] = p.RMaterielStatutReg;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Reg) : {0:d}", p.RMaterielStatutReg.codStatutMateriel));
                        }
                        if (p.RMaterielStatutDec != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutDec.codStatutMateriel)] = p.RMaterielStatutDec;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Dec) : {0:d}", p.RMaterielStatutDec.codStatutMateriel));
                        }
                        if (p.RMaterielStatutBsu != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutBsu.codStatutMateriel)] = p.RMaterielStatutBsu;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Bsu) : {0:d}", p.RMaterielStatutBsu.codStatutMateriel));
                        }
                        if (p.PTransfert != null)
                        {
                            pTransferts[String.Format("{0:d}-{1:d}", p.PTransfert.numTransfert, p.PTransfert.origine)] = p.PTransfert;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_transfert : {0:d}-{1:d}", p.PTransfert.numTransfert, p.PTransfert.origine));
                        }
                        if (p.PDocConstates != null)
                        {
                            foreach (var pdc in p.PDocConstates)
                            {
                                if (pdc != null)
                                {
                                    pDocConstates[String.Format("{0:d}-{1:d}-{2}-{3:d}", pdc.numVisit, pdc.origine, pdc.codDoc, pdc.numeroDoc)] = pdc;
                                    System.Diagnostics.Debug.WriteLine(String.Format("p_doc_constate : {0:d}-{1:d}-{2}-{3:d}", pdc.numVisit, pdc.origine, pdc.codDoc, pdc.numeroDoc));
                                    if (pdc.PLocalisationDocs != null)
                                    {
                                        foreach (var ld in pdc.PLocalisationDocs)
                                        {
                                            if (ld != null)
                                            {
                                                pLocalisationDocs[String.Format("{0:d}-{1:d}-{2}-{3:d}-{4}", ld.numVisit, ld.origine, ld.codDoc, ld.numeroDoc, ld.codLoc)] = ld;
                                                System.Diagnostics.Debug.WriteLine(String.Format("p_localisation_doc : {0:d}-{1:d}-{2}-{3:d}-{4}", ld.numVisit, ld.origine, ld.codDoc, ld.numeroDoc, ld.codLoc));
                                                if (ld.RLocalisation != null)
                                                {
                                                    rLocalisations[ld.RLocalisation.codLoc] = ld.RLocalisation;
                                                    System.Diagnostics.Debug.WriteLine(String.Format("r_localisation : {0}", ld.RLocalisation.codLoc));
                                                }
                                            }
                                        }
                                    }
                                    if (pdc.RDoc != null)
                                    {
                                        rDocs[String.Format("{0}-{1}", pdc.RDoc.codDoc, pdc.RDoc.dateDebutEffet.ToString())] = pdc.RDoc;
                                        System.Diagnostics.Debug.WriteLine(String.Format("r_doc : {0}-{1}", pdc.RDoc.codDoc, pdc.RDoc.dateDebutEffet.ToString()));
                                    }
                                }
                            }
                        }
                        //26/08/2024 PLE Ajout Historique km + compteurs d'exception
                        if (p.PHistoCtrls != null)
                        {
                            foreach (var phc in p.PHistoCtrls)
                            {
                                if (phc != null)
                                {
                                    pHistoCtrl[String.Format("{0:d}-{1:d}-{2:d}", phc.numVisit, phc.origine, phc.numChrono)] = phc;
                                    System.Diagnostics.Debug.WriteLine(String.Format("p_histo_ctrl : {0:d}-{1:d}-{2:d}", phc.numVisit, phc.origine, phc.numChrono));
                                }
                            }
                        }
                        if (p.PAnoes != null)
                        {
                            foreach (var pan in p.PAnoes)
                            {
                                if (pan != null)
                                {
                                    pAno[String.Format("{0:d}-{1:d}-{2}", pan.numVisit, pan.origine, pan.comptExcep)] = pan;
                                    System.Diagnostics.Debug.WriteLine(String.Format("p_ano : {0:d}-{1:d}-{2}", pan.numVisit, pan.origine, pan.comptExcep));
                                }
                                if (pan.RAno != null)
                                {
                                    rAno[String.Format("{0}-{1}", pan.RAno.comptExcep, pan.RAno.dateDebutEffet)] = pan.RAno;
                                    System.Diagnostics.Debug.WriteLine(String.Format("r_ano : {0}-{1}", pan.RAno.comptExcep, pan.RAno.dateDebutEffet.ToString()));
                                }
                            }
                        }
                    }
                }
                //Avoid some bugs due to empty collections
                if (rDocs.Values.Count == 0) rDocs.Add("fake_item", new VlRDoc());
                if (pMesuress.Values.Count == 0) pMesuress.Add("fake_item", new VlPMesures());
                //26/08/2024 PLE Ajout Historique km + compteurs d'exception
                if (pHistoCtrl.Values.Count == 0) pHistoCtrl.Add("fake_item", new pHistoCtrl());
                if (pAno.Values.Count == 0) pAno.Add("fake_item", new pAno());
                if (rAno.Values.Count == 0) rAno.Add("fake_item", new rAno());

                //if (pDocConstates.Values.Count == 0) pDocConstates.Add("fake_item", new VlPDocConstate());
                //if (rLocalisations.Values.Count == 0) rLocalisations.Add("fake_item", new VlRLocalisation());
                //if (rMaterielStatuts.Values.Count == 0) rMaterielStatuts.Add("fake_item", new VlRMaterielStatut());
                //if (pLocalisationDocs.Values.Count == 0) pLocalisationDocs.Add("fake_item", new VlPLocalisationDoc());
                //Assign collections to report
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRDoc"].SetDataSource(rDocs.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlPMesures"].SetDataSource(pMesuress.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlPControle"].SetDataSource(pControles.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlPTransfert"].SetDataSource(pTransferts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRResultCtrl"].SetDataSource(rResultCtrls.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlPDocConstate"].SetDataSource(pDocConstates.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRLocalisation"].SetDataSource(rLocalisations.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRNatureControle"].SetDataSource(rNatureControles.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlPLocalisationDoc"].SetDataSource(pLocalisationDocs.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutFrl"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutOpa"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutAdg"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutObd"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutRip"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutReg"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutDec"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRMaterielStatutBsu"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlPVehiculePresente"].SetDataSource(pVehiculePresentes.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRNatureCtrlSuivant"].SetDataSource(rNatureCtrlSuivants.Values);
                pvTransfertCTVL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_VlRDocumentIdentification"].SetDataSource(rDocumentIdentifications.Values);

                pvTransfertCTVL.Subreports["ctvlListHistoCtrl"].Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_pHistoCtrl"].SetDataSource(pHistoCtrl.Values);
                pvTransfertCTVL.Subreports["ctvlListAno"].Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_pAno"].SetDataSource(pAno.Values);
                pvTransfertCTVL.Subreports["ctvlListAno"].Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_rAno"].SetDataSource(rAno.Values);

                pvTransfertCTVL.SetParameterValue("niveauDetailParam", niveauDetailParam.GetHashCode());

                var pdfStream = pvTransfertCTVL.ExportToStream(ExportFormatType.PortableDocFormat);

                var respBuffer = new byte[1024];

                var bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                while (bytesRead > 0)
                {
                    memStream.Write(respBuffer, 0, bytesRead);
                    bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                }

            }
            catch (CrystalReportsException ex)
            {
                memStream.Close();
                memStream = null;

                var strMessageInnerEx = "Pas de innerException !";
                if (ex.InnerException != null)
                {
                    strMessageInnerEx = ex.InnerException.Message;
                }

                log.Error("Erreur lors de la génération de l'édition crystal (CrystalReportsException) " + ex.Message + " " + strMessageInnerEx);
            }
            catch (Exception ex)
            {
                memStream.Close();
                memStream = null;

                log.Error("Erreur lors de la génération de l'édition crystal (Exception) " + ex.Message);
            }
            finally
            {
                pvTransfertCTVL?.Close();
                pvTransfertCTVL?.Dispose();
            }
            log.Debug("NewGenereCtvlPvPdf Fin...");
            return memStream;
        }

        private MemoryStream GenereCtplPvPdf(System.Collections.IEnumerable col, EnumNiveauDetailPVCT niveauDetailParam)
        {
            log.Debug("GenereCtplPvPdf Début...");
            //On charge le document Crystal

            var pvTransfertCTPL = new Rnc2.Web.Pv.Reporting.PvTransfertCTPL();
            var memStream = new MemoryStream();

            try
            {
                log.Debug("GenereCtplPvPdf.Document.load Début...");
                log.Debug("GenereCtplPvPdf.Document.load Fin...");
                //Create dictionaries for entities storage (dictionaries usage avoid duplicate entries when key is relevant, ie primary key, for current entity)
                var rDacs = new Dictionary<string, PlRDac>();
                var pMesuress = new Dictionary<string, PlPMesures>();
                var rCategVehs = new Dictionary<string, PlRCategVeh>();
                var pControles = new Dictionary<string, PlPControle>();
                var pTransferts = new Dictionary<string, PlPTransfert>();
                var rResultCtrls = new Dictionary<string, PlRResultCtrl>();
                var rEtatCharges = new Dictionary<string, PlREtatCharge>();
                var rDerogCharges = new Dictionary<string, PlRDerogCharge>();
                var pDacConstates = new Dictionary<string, PlPDacConstate>();
                var rLocalisations = new Dictionary<string, PlRLocalisation>();
                var rMaterielStatuts = new Dictionary<string, PlRMaterielStatut>();
                var rNatureControles = new Dictionary<string, PlRNatureControle>();
                var pLocalisationDacs = new Dictionary<string, PlPLocalisationDac>();
                var rMotifPonderations = new Dictionary<string, PlRMotifPonderation>();
                var pVehiculePresentes = new Dictionary<string, PlPVehiculePresente>();
                var embeddableMeasurements = new Dictionary<string, PlEmbeddableMeasurements>();
                var rDocumentIdentifications = new Dictionary<string, PlRDocumentIdentification>();
                //Fill dictionaries
                foreach (var p in (PlPControle[])col)
                {
                    if (p != null)
                    {
                        pControles[String.Format("{0:d}-{1:d}", p.numVisit, p.origine)] = p;
                        System.Diagnostics.Debug.WriteLine(String.Format("p_controle : {0:d}-{1:d}", p.numVisit, p.origine));
                        if (p.RNatureControle != null)
                        {
                            rNatureControles[String.Format("{0}", p.RNatureControle.codNatureCtrl)] = p.RNatureControle;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_nature_controle (actuel) : {0}", p.RNatureControle.codNatureCtrl));
                        }
                        if (p.RNatureProchainCtrl != null)
                        {
                            rNatureControles[String.Format("{0}", p.RNatureProchainCtrl.codNatureCtrl)] = p.RNatureProchainCtrl;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_nature_controle (prochain) : {0}", p.RNatureProchainCtrl.codNatureCtrl));
                        }
                        if (p.RResultCtrl != null)
                        {
                            rResultCtrls[String.Format("{0}", p.RResultCtrl.codResultCtrl)] = p.RResultCtrl;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_result_ctrl : {0}", p.RResultCtrl.codResultCtrl));
                        }
                        if (p.PVehiculePresente != null)
                        {
                            pVehiculePresentes[String.Format("{0:d}-{1:d}", p.PVehiculePresente.numVisit, p.PVehiculePresente.origine)] = p.PVehiculePresente;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_vehicule_presente : {0:d}-{1:d}", p.PVehiculePresente.numVisit, p.PVehiculePresente.origine));
                            if (p.PVehiculePresente.RCategVeh != null)
                            {
                                rCategVehs[String.Format("{0}", p.PVehiculePresente.RCategVeh.codCategVeh)] = p.PVehiculePresente.RCategVeh;
                                System.Diagnostics.Debug.WriteLine(String.Format("r_categ_veh : {0}", p.PVehiculePresente.RCategVeh.codCategVeh));
                            }
                            if (p.PVehiculePresente.RDerogCharge != null)
                            {
                                rDerogCharges[String.Format("{0:d}", p.PVehiculePresente.RDerogCharge.codDerog)] = p.PVehiculePresente.RDerogCharge;
                                System.Diagnostics.Debug.WriteLine(String.Format("r_derog_charge : {0:d}", p.PVehiculePresente.RDerogCharge.codDerog));
                            }
                        }
                        if (p.RDocumentIdentification != null)
                        {
                            rDocumentIdentifications[String.Format("{0}", p.RDocumentIdentification.codDocument)] = p.RDocumentIdentification;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_document_identification : {0}", p.RDocumentIdentification.codDocument));
                        }
                        if (p.REtatCharge != null)
                        {
                            rEtatCharges[String.Format("{0}", p.REtatCharge.codEtatCharg)] = p.REtatCharge;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_etat_charge : {0}", p.REtatCharge.codEtatCharg));
                        }
                        if (p.embeddableMeasurements != null)
                        {
                            embeddableMeasurements[String.Format("{0:d}-{1:d}", p.numVisit, p.origine)] = p.embeddableMeasurements;
                            System.Diagnostics.Debug.WriteLine(String.Format("EmbeddableMeasurements : {0:d}-{1:d}", p.numVisit, p.origine));
                        }
                        else
                        {
                            var em = new PlEmbeddableMeasurements();
                            em.numVisit = p.numVisit;
                            em.origine = p.origine;
                            embeddableMeasurements[String.Format("{0:d}-{1:d}", p.numVisit, p.origine)] = em;
                            System.Diagnostics.Debug.WriteLine(String.Format("Fake EmbeddableMeasurements : {0:d}-{1:d}", p.numVisit, p.origine));
                        }
                        if (p.RMotifPonderation != null)
                        {
                            rMotifPonderations[String.Format("{0}", p.RMotifPonderation.codMotifPond)] = p.RMotifPonderation;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_motif_ponderation : {0}", p.RMotifPonderation.codMotifPond));
                        }
                        if (p.PMesures != null)
                        {
                            pMesuress[String.Format("{0:d}-{1:d}", p.PMesures.numVisit, p.PMesures.origine)] = p.PMesures;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_mesures : {0:d}-{1:d}", p.PMesures.numVisit, p.PMesures.origine));
                        }
                        if (p.RMaterielStatutFrp != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutFrp.codStatutMateriel)] = p.RMaterielStatutFrp;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Frp) : {0:d}", p.RMaterielStatutFrp.codStatutMateriel));
                        }
                        if (p.RMaterielStatutOpa != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutOpa.codStatutMateriel)] = p.RMaterielStatutOpa;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Opa) : {0:d}", p.RMaterielStatutOpa.codStatutMateriel));
                        }
                        if (p.RMaterielStatutObd != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutObd.codStatutMateriel)] = p.RMaterielStatutObd;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Obd) : {0:d}", p.RMaterielStatutObd.codStatutMateriel));
                        }
                        if (p.RMaterielStatutReg != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutReg.codStatutMateriel)] = p.RMaterielStatutReg;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Reg) : {0:d}", p.RMaterielStatutReg.codStatutMateriel));
                        }
                        if (p.RMaterielStatutDec != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutDec.codStatutMateriel)] = p.RMaterielStatutDec;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Dec) : {0:d}", p.RMaterielStatutDec.codStatutMateriel));
                        }
                        if (p.PTransfert != null)
                        {
                            pTransferts[String.Format("{0:d}-{1:d}", p.PTransfert.numTransfert, p.PTransfert.origine)] = p.PTransfert;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_transfert : {0:d}-{1:d}", p.PTransfert.numTransfert, p.PTransfert.origine));
                        }
                        if (p.PDacConstates != null)
                        {
                            foreach (var pdc in p.PDacConstates)
                            {
                                if (pdc != null)
                                {
                                    pDacConstates[String.Format("{0:d}-{1:d}-{2}-{3:d}", pdc.numVisit, pdc.origine, pdc.codDac, pdc.numeroDac)] = pdc;
                                    System.Diagnostics.Debug.WriteLine(String.Format("p_dac_constate : {0:d}-{1:d}-{2}-{3:d}", pdc.numVisit, pdc.origine, pdc.codDac, pdc.numeroDac));
                                    if (pdc.PLocalisationDacs != null)
                                    {
                                        foreach (var ld in pdc.PLocalisationDacs)
                                        {
                                            if (ld != null)
                                            {
                                                pLocalisationDacs[String.Format("{0:d}-{1:d}-{2}-{3:d}-{4}", ld.numVisit, ld.origine, ld.codDac, ld.numeroDac, ld.codLoc)] = ld;
                                                System.Diagnostics.Debug.WriteLine(String.Format("p_localisation_dac : {0:d}-{1:d}-{2}-{3:d}-{4}", ld.numVisit, ld.origine, ld.codDac, ld.numeroDac, ld.codLoc));
                                                if (ld.RLocalisation != null)
                                                {
                                                    rLocalisations[ld.RLocalisation.codLoc] = ld.RLocalisation;
                                                    System.Diagnostics.Debug.WriteLine(String.Format("r_localisation : {0}", ld.RLocalisation.codLoc));
                                                }
                                            }
                                        }
                                    }
                                    if (pdc.RDac != null)
                                    {
                                        rDacs[String.Format("{0}-{1}", pdc.RDac.codDac, pdc.RDac.dateDebut.ToString())] = pdc.RDac;
                                        System.Diagnostics.Debug.WriteLine(String.Format("r_dac : {0}-{1}", pdc.RDac.codDac, pdc.RDac.dateDebut.ToString()));
                                    }
                                }
                            }
                        }
                    }
                }
                //Avoid some bugs due to empty collections
                if (rDacs.Values.Count == 0) rDacs.Add("fake_item", new PlRDac());
                if (pMesuress.Values.Count == 0) pMesuress.Add("fake_item", new PlPMesures());
                //Assign collections to report
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRDac"].SetDataSource(rDacs.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlPMesures"].SetDataSource(pMesuress.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlPControle"].SetDataSource(pControles.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRCategVeh"].SetDataSource(rCategVehs.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlPTransfert"].SetDataSource(pTransferts.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlREtatCharge"].SetDataSource(rEtatCharges.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRResultCtrl"].SetDataSource(rResultCtrls.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRDerogCharge"].SetDataSource(rDerogCharges.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlPDacConstate"].SetDataSource(pDacConstates.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRLocalisation"].SetDataSource(rLocalisations.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRNatureControle"].SetDataSource(rNatureControles.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRNatureProchainCtrl"].SetDataSource(rNatureControles.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlPLocalisationDac"].SetDataSource(pLocalisationDacs.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRMaterielStatutFrp"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRMaterielStatutOpa"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRMaterielStatutObd"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRMaterielStatutReg"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRMaterielStatutDec"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRMotifPonderation"].SetDataSource(rMotifPonderations.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlPVehiculePresente"].SetDataSource(pVehiculePresentes.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlEmbeddableMeasurements"].SetDataSource(embeddableMeasurements.Values);
                pvTransfertCTPL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_PlRDocumentIdentification"].SetDataSource(rDocumentIdentifications.Values);

                pvTransfertCTPL.SetParameterValue("niveauDetailParam", niveauDetailParam.GetHashCode());

                var pdfStream = pvTransfertCTPL.ExportToStream(ExportFormatType.PortableDocFormat);

                var respBuffer = new byte[1024];

                var bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                while (bytesRead > 0)
                {
                    memStream.Write(respBuffer, 0, bytesRead);
                    bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                }

            }
            catch (CrystalReportsException ex)
            {
                memStream.Close();
                memStream = null;

                var strMessageInnerEx = "Pas de innerException !";
                if (ex.InnerException != null)
                {
                    strMessageInnerEx = ex.InnerException.Message;
                }

                log.Error("Erreur lors de la génération de l'édition crystal (CrystalReportsException) " + ex.Message + " " + strMessageInnerEx);
            }
            catch (Exception ex)
            {
                memStream.Close();
                memStream = null;

                log.Error("Erreur lors de la génération de l'édition crystal (Exception) " + ex.Message);
            }
            finally
            {
                pvTransfertCTPL?.Close();
                pvTransfertCTPL?.Dispose();
            }
            log.Debug("GenereCtplPvPdf Fin...");
            return memStream;
        }

        /// <summary>
        /// Niveau de détail du PV devant être généré
        /// </summary>
        public enum EnumNiveauDetailPVCT
        {
            TOUT = 1,
            REDUIT = 2
        }

        /// <summary>
        /// Code retour du service web de récupération des infos contrôle technique
        /// </summary>
        public enum EnumCodeRetourPVCT
        {
            AUCUN_CT = 0,
            TROUVE_CT = 1,
            ERREUR_TECHNIQUE = 2,
            AUCUN_CT_PL = 3,
            TROUVE_CT_PL = 4,
            ERREUR_TECHNIQUE_PL = 5,
            AUCUN_CT_CL = 6,
            TROUVE_CT_CL = 7,
            ERREUR_TECHNIQUE_CL = 8
        }

        /// <summary>
        /// Cible de la requête
        /// </summary>
        public enum EnumRequestTarget
        {
            VL = 1,
            PL = 2,
            CL = 3
        }

        #region Recherche liste de PV par immatriculation et / ou n° de série
        /// <summary>
        /// Appel de la méthode find[Vl|Pl]ByImmatVehOrNumSerie du service web
        /// </summary>
        public WebServiceResult Ws_FindByImmatOrNumSerie(String immat, String vin, CategorieVehicule cat)
        {
            log.Debug("Appel web service, méthode find[Vl|Pl|Cl]ByImmatVehOrNumSerie Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE);
            if (!String.IsNullOrWhiteSpace(immat) || !String.IsNullOrWhiteSpace(vin))
            {
                log.Debug(String.Format("Paramètres immat={0}; vin={1}", immat, vin));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;
                        else
                        {
                            log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                            return false;
                        }
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        if (ws != null)
                        {
                            switch (cat.InnerEnumValue)
                            {
                                case CategorieVehicule.InnerEnum.VL:
                                    result = new WebServiceResult(ws.findVlByImmatVehOrNumSerie(immat, vin), cat);
                                    break;
                                case CategorieVehicule.InnerEnum.PL:
                                    result = new WebServiceResult(ws.findPlByImmatVehOrNumSerie(immat, vin), cat);
                                    break;
                                case CategorieVehicule.InnerEnum.CL:
                                    result = new WebServiceResult(ws.findClByImmatVehOrNumSerie(immat, vin), cat);
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("L'appel du web service transverse est en time out : " + ex.Message);

                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);

                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("Paramètres transmis pour appel au webservice incorrects immat={0}; vin={1}",
                    immat ?? "null",
                    vin ?? "null"));
            }
            log.Debug(String.Format("Appel web service, méthode findVlByImmatVehOrNumSerie Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }
        #endregion

        #region Recherche liste de PV par code agrément centre et / ou code agrément contrôleur
        /// <summary>
        /// Appel de la méthode find[Vl|Pl]ByCodAgrCentreControleur du service web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public WebServiceResult Ws_FindByCodAgrCentreControleur(String codeAgrementCentre, String codeAgrementControleur, DateTime dateVisiteDebut, DateTime dateVisiteFin, CategorieVehicule cat)
        {
            log.Debug("Appel web service, méthode find[Vl|Pl|CL]ByCodAgrCentreControleur Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE);
            if (!String.IsNullOrWhiteSpace(codeAgrementCentre) || !String.IsNullOrWhiteSpace(codeAgrementControleur) && dateVisiteDebut != null && dateVisiteFin != null)
            {
                log.Debug(String.Format("Paramètres : code agrément centre={0}; code agrément contrôleur={1}; date visite début={2}; date visite fin={3}",
                    codeAgrementCentre, codeAgrementControleur, dateVisiteDebut.ToShortDateString(), dateVisiteFin.ToShortDateString()));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;

                        log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                        return false;
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        switch (cat.InnerEnumValue)
                        {
                            case CategorieVehicule.InnerEnum.VL:
                                result = new WebServiceResult(ws.findVlByCodAgrCentreControleur(codeAgrementCentre, codeAgrementControleur, dateVisiteDebut, dateVisiteFin), cat);
                                break;
                            case CategorieVehicule.InnerEnum.PL:
                                result = new WebServiceResult(ws.findPlByCodAgrCentreControleur(codeAgrementCentre, codeAgrementControleur, dateVisiteDebut, dateVisiteFin), cat);
                                break;
                            case CategorieVehicule.InnerEnum.CL:
                                result = new WebServiceResult(ws.findClByCodAgrCentreControleur(codeAgrementCentre, codeAgrementControleur, dateVisiteDebut, dateVisiteFin), cat);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("L'appel du web service transverse est en time out : " + ex.Message);

                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);
                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("Paramètres transmis pour appel au webservice incorrects code agément centre={0}; code agrément contrôleur={1}; date visite début={2}; date visite fin={3}",
                    codeAgrementCentre ?? "null",
                    codeAgrementControleur ?? "null",
                    dateVisiteDebut.ToShortDateString(),
                    dateVisiteFin.ToShortDateString()));
            }
            log.Debug(String.Format("Appel web service, find[Vl|Pl|CL]ByCodAgrCentreControleur Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }
        #endregion

        /// <summary>
        /// Méthode public de récupération du contenu CTCL du fichier PDF à imprimer
        /// 10/05/2024 PLE
        /// </summary>
        /// <param name="numPvCtrl">numéro du PV de contrôle</param>
        /// <param name="codAgremCentre">code agrément du centre de contrôle</param>
        /// <param name="dateCtrl">date du contrôle</param>
        /// <param name="niveauDetail">niveaud de détail d'impression</param>
        /// <param name="pvPDF">flux binaire contenant le PDF à renvoyer au navigateur</param>
        /// <returns></returns>
        public EnumCodeRetourPVCT RecupereCtclPV(string numPvCtrl, string codAgremCentre, DateTime dateCtrl, EnumNiveauDetailPVCT niveauDetail, ref MemoryStream pvPDF)
        {

            log.Info("Recupération d'un PV CL (méthode RecupereCtclPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Début ...");

            pvPDF = null;

            var output = this.WsCtclPv(numPvCtrl, codAgremCentre, dateCtrl);

            if (output.ResultCode.Equals(EnumCodeRetourPVCT.TROUVE_CT_CL))
            {
                pvPDF = GenereCtclPvPdf(output.ResultValue, niveauDetail);

                if (pvPDF == null)
                {
                    output.ResultCode = EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL;
                }
            }

            log.Info("Recupération d'un PV CL (méthode RecupereCtclPV), numPvCtrl=" + numPvCtrl + " codAgremCentre=" + codAgremCentre + " Fin ...");
            return output.ResultCode;
        }

        /// <summary>
        /// Appel de la méthode RecherchePV du service web
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private WebServiceResult WsCtclPv(String numPvCtrl, String codAgremCentre, DateTime dateCtrl)
        {
            log.Debug("Appel web WsCtclPv service transverse Début...");
            var result = new WebServiceResult(EnumCodeRetourPVCT.ERREUR_TECHNIQUE_CL);
            if (!String.IsNullOrWhiteSpace(numPvCtrl) && !String.IsNullOrWhiteSpace(codAgremCentre) && dateCtrl != null)
            {
                log.Debug(String.Format("Paramètres numPvCtrl={0}; codAgremCentre={1}; dateCtrl={2}", numPvCtrl, codAgremCentre, dateCtrl.ToString()));
                try
                {
                    //Certificats de confiances acceptés UTACTEST ou UTAC-OTC
                    ServicePointManager.ServerCertificateValidationCallback = delegate (Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
                    {
                        if (certificate.Issuer.Contains("UTAC-OTC") || certificate.Issuer.Contains("UTACTEST"))
                            return true;
                        else
                        {
                            log.Error("ServicePointManager.ServerCertificateValidationCallback : Certificat serveur refusé !");
                            return false;
                        }
                    };

                    var ws = new Rnc2Client();

                    try
                    {
                        result.ResultValue = ws.findClByNumPvCodAgrDateCtrl(numPvCtrl, codAgremCentre, dateCtrl);
                        result.ResultCode = result.ResultValue != null ? EnumCodeRetourPVCT.TROUVE_CT_CL : EnumCodeRetourPVCT.AUCUN_CT_CL;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        ws.Close();
                    }
                }
                #region Exceptions
                catch (TimeoutException ex)
                {
                    //Web service en time out
                    log.Error("L'appel du web service transverse est en time out : " + ex.Message);

                }
                catch (System.ServiceModel.FaultException<Exception> ex)
                {
                    log.Error("L'appel du web service transverse est en erreur : " + ex.Detail.Message);
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    //Exception inconnu
                    log.Error("L'appel du web service transverse est en erreur (Exception inconnu): " + ex.Message);

                }
                catch (System.ServiceModel.CommunicationException ex)
                {
                    var strMessageInnerEx = "Pas de innerException !";
                    if (ex.InnerException != null)
                    {
                        strMessageInnerEx = ex.InnerException.Message;
                    }
                    log.Error("L'appel du web service transverse est en erreur (Problème de communication): " + ex.Message + " " + strMessageInnerEx);
                }
                catch (ArgumentException ex)
                {
                    log.Error("Erreur de paramètrage dans le web.config: " + ex.Message);
                }
                #endregion
            }
            else
            {
                log.Error(String.Format("Paramètres transmis au webservice incorrects numPvCtrl={0}; codAgremCentre={1}; dateCtrl={2}",
                    numPvCtrl ?? "null",
                    codAgremCentre ?? "null",
                    dateCtrl.ToString(CultureInfo.InvariantCulture)));
            }

            log.Debug(String.Format("Appel web service WsCtclPv transverse Fin, code retour={0}", result.ResultCode.ToString("g")));
            return result;
        }

        private MemoryStream GenereCtclPvPdf(System.Collections.IEnumerable col, EnumNiveauDetailPVCT niveauDetailParam)
        {
            log.Debug("GenereCtclPvPdf Début...");
            //On charge le document Crystal
            //ReportDocument boReportDocument = new ReportDocument();

            var pvTransfertCTCL = new Rnc2.Web.Pv.Reporting.PvTransfertCTCL();
            //Rnc2.Web.Pv.Reporting.CrystalReport1 pvTransfertCTVL = new Rnc2.Web.Pv.Reporting.CrystalReport1();
            var memStream = new MemoryStream();

            try
            {
                log.Debug("GenereCtclPvPdf.Document.load Début...");
                //boReportDocument.Load(ctsFileRptPvTransfertCtvl);
                log.Debug("GenereCtclPvPdf.Document.load Fin...");
                #region R&D code générique (JMA)
                //boReportDocument.SetDataSource(ds);
                //pvTransfertCTVL.SetDataSource(col);

                //foreach (Table t in pvTransfertCTVL.Database.Tables)
                //{
                //    System.Diagnostics.Debug.WriteLine(t.Location);
                //}

                //foreach (pControle p in ((pControle[])col))
                //{
                //    PropertyInfo[] props = p.GetType().GetProperties();
                //    foreach (PropertyInfo pi in props)
                //    {
                //        System.Diagnostics.Debug.WriteLine("Nom : {0}; Type : {1}", pi.Name, pi.PropertyType.ToString());
                //        if (pi.PropertyType.IsArray)
                //        {
                //            System.Diagnostics.Debug.WriteLine("Enumerable");
                //            IEnumerable<object> a = (IEnumerable<object>)pi.GetValue(p, null);
                //            if (a != null)
                //            {
                //                // at least foreach now is available
                //                foreach (object o in a)
                //                {
                //                    // get the value
                //                    System.Diagnostics.Debug.WriteLine(o.ToString());
                //                }
                //            }
                //        }
                //    }
                //    break;
                //}
                #endregion
                //Create dictionaries for entities storage (dictionaries usage avoid duplicate entries when key is relevant, ie primary key, for current entity)
                var rDocs = new Dictionary<string, ClRDoc>();
                var pMesuress = new Dictionary<string, ClPmesures>();
                var pControles = new Dictionary<string, ClPcontrole>();
                var pTransferts = new Dictionary<string, ClPtransfert>();
                var rResultCtrls = new Dictionary<string, ClRResultCtrl>();
                var pDocConstates = new Dictionary<string, ClPDocConstate>();
                var rLocalisations = new Dictionary<string, ClRlocalisation>();
                var rMaterielStatuts = new Dictionary<string, ClRMaterielStatut>();
                var rNatureControles = new Dictionary<string, ClRNatureControle>();
                var rNatureCtrlSuivants = new Dictionary<string, ClRNatureControle>();
                var pLocalisationDocs = new Dictionary<string, ClPLocalisationDoc>();
                var pVehiculePresentes = new Dictionary<string, ClPVehiculePresente>();
                var rDocumentIdentifications = new Dictionary<string, ClRDocumentIdentification>();
                var pHistoCtrl = new Dictionary<string, ClPHistoCtrl>();
                var pAno = new Dictionary<string, ClPano>();
                var rAno = new Dictionary<string, ClRano>();

                //Fill dictionaries
                foreach (var p in (ClPcontrole[])col)
                {
                    if (p != null)
                    {
                        pControles[String.Format("{0:d}-{1:d}", p.numVisit, p.origine)] = p;
                        System.Diagnostics.Debug.WriteLine(String.Format("p_controle : {0:d}-{1:d}", p.numVisit, p.origine));
                        if (p.RNatureControle != null)
                        {
                            rNatureControles[String.Format("{0}", p.RNatureControle.codNatureCtrl)] = p.RNatureControle;
                            rNatureCtrlSuivants[String.Format("{0}", p.RNatureControle.codNatureCtrl)] = p.RNatureControle;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_nature_controle (actuel) : {0}", p.RNatureControle.codNatureCtrl));
                        }
                        if (p.RNatureCtrlSuivant != null)
                        {
                            rNatureControles[String.Format("{0}", p.RNatureCtrlSuivant.codNatureCtrl)] = p.RNatureCtrlSuivant;
                            rNatureCtrlSuivants[String.Format("{0}", p.RNatureCtrlSuivant.codNatureCtrl)] = p.RNatureCtrlSuivant;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_nature_controle (suivant) : {0}", p.RNatureCtrlSuivant.codNatureCtrl));
                        }
                        if (p.RResultCtrl != null)
                        {
                            rResultCtrls[String.Format("{0}", p.RResultCtrl.codResultCtrl)] = p.RResultCtrl;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_result_ctrl : {0}", p.RResultCtrl.codResultCtrl));
                        }
                        if (p.PVehiculePresente != null)
                        {
                            pVehiculePresentes[String.Format("{0:d}-{1:d}", p.PVehiculePresente.numVisit, p.PVehiculePresente.origine)] = p.PVehiculePresente;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_vehicule_presente : {0:d}-{1:d}", p.PVehiculePresente.numVisit, p.PVehiculePresente.origine));
                        }
                        if (p.RDocumentIdentification != null)
                        {
                            rDocumentIdentifications[String.Format("{0}", p.RDocumentIdentification.codDocument)] = p.RDocumentIdentification;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_document_identification : {0}", p.RDocumentIdentification.codDocument));
                        }
                        if (p.PMesures != null)
                        {
                            pMesuress[String.Format("{0:d}-{1:d}", p.PMesures.numVisit, p.PMesures.origine)] = p.PMesures;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_mesures : {0:d}-{1:d}", p.PMesures.numVisit, p.PMesures.origine));
                        }
                        //if (p.RMaterielStatutFrl != null)
                        //{
                        //    rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutFrl.codStatutMateriel)] = p.RMaterielStatutFrl;
                        //    System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Frl) : {0:d}", p.RMaterielStatutFrl.codStatutMateriel));
                        //}
                        if (p.RMaterielStatutOpa != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutOpa.codStatutMateriel)] = p.RMaterielStatutOpa;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Opa) : {0:d}", p.RMaterielStatutOpa.codStatutMateriel));
                        }
                        if (p.RMaterielStatutAdg != null)
                        {
                            rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutAdg.codStatutMateriel)] = p.RMaterielStatutAdg;
                            System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Adg) : {0:d}", p.RMaterielStatutAdg.codStatutMateriel));
                        }
                        //if (p.RMaterielStatutObd != null)
                        //{
                        //    rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutObd.codStatutMateriel)] = p.RMaterielStatutObd;
                        //    System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Obd) : {0:d}", p.RMaterielStatutObd.codStatutMateriel));
                        //}
                        //if (p.RMaterielStatutRip != null)
                        //{
                        //    rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutRip.codStatutMateriel)] = p.RMaterielStatutRip;
                        //    System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Rip) : {0:d}", p.RMaterielStatutRip.codStatutMateriel));
                        //}
                        //if (p.RMaterielStatutReg != null)
                        //{
                        //    rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutReg.codStatutMateriel)] = p.RMaterielStatutReg;
                        //    System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Reg) : {0:d}", p.RMaterielStatutReg.codStatutMateriel));
                        //}
                        //if (p.RMaterielStatutDec != null)
                        //{
                        //    rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutDec.codStatutMateriel)] = p.RMaterielStatutDec;
                        //    System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Dec) : {0:d}", p.RMaterielStatutDec.codStatutMateriel));
                        //}
                        //if (p.RMaterielStatutBsu != null)
                        //{
                        //    rMaterielStatuts[String.Format("{0:d}", p.RMaterielStatutBsu.codStatutMateriel)] = p.RMaterielStatutBsu;
                        //    System.Diagnostics.Debug.WriteLine(String.Format("r_materiel_statut (Bsu) : {0:d}", p.RMaterielStatutBsu.codStatutMateriel));
                        //}
                        if (p.PTransfert != null)
                        {
                            pTransferts[String.Format("{0:d}-{1:d}", p.PTransfert.numTransfert, p.PTransfert.origine)] = p.PTransfert;
                            System.Diagnostics.Debug.WriteLine(String.Format("p_transfert : {0:d}-{1:d}", p.PTransfert.numTransfert, p.PTransfert.origine));
                        }
                        if (p.pDocConstates != null)
                        {
                            foreach (var pdc in p.pDocConstates)
                            {
                                if (pdc != null)
                                {
                                    pDocConstates[String.Format("{0:d}-{1:d}-{2}-{3:d}", pdc.numVisit, pdc.origine, pdc.codDoc, pdc.numeroDoc)] = pdc;
                                    System.Diagnostics.Debug.WriteLine(String.Format("p_doc_constate : {0:d}-{1:d}-{2}-{3:d}", pdc.numVisit, pdc.origine, pdc.codDoc, pdc.numeroDoc));
                                    if (pdc.PLocalisationDocs != null)
                                    {
                                        foreach (var ld in pdc.PLocalisationDocs)
                                        {
                                            if (ld != null)
                                            {
                                                pLocalisationDocs[String.Format("{0:d}-{1:d}-{2}-{3:d}-{4}", ld.numVisit, ld.origine, ld.codDoc, ld.numeroDoc, ld.codLoc)] = ld;
                                                System.Diagnostics.Debug.WriteLine(String.Format("p_localisation_doc : {0:d}-{1:d}-{2}-{3:d}-{4}", ld.numVisit, ld.origine, ld.codDoc, ld.numeroDoc, ld.codLoc));
                                                if (ld.RLocalisation != null)
                                                {
                                                    rLocalisations[ld.RLocalisation.codLoc] = ld.RLocalisation;
                                                    System.Diagnostics.Debug.WriteLine(String.Format("r_localisation : {0}", ld.RLocalisation.codLoc));
                                                }
                                            }
                                        }
                                    }
                                    if (pdc.RDoc != null)
                                    {
                                        rDocs[String.Format("{0}-{1}", pdc.RDoc.codDoc, pdc.RDoc.dateDebutEffet.ToString())] = pdc.RDoc;
                                        System.Diagnostics.Debug.WriteLine(String.Format("r_doc : {0}-{1}", pdc.RDoc.codDoc, pdc.RDoc.dateDebutEffet.ToString()));
                                    }
                                }
                            }
                        }

                        if (p.pHistoCtrls != null)
                        {
                            foreach (var phc in p.pHistoCtrls)
                            {
                                if (phc != null)
                                {
                                    pHistoCtrl[String.Format("{0:d}-{1:d}-{2:d}", phc.numVisit, phc.origine, phc.numChrono)] = phc;
                                    System.Diagnostics.Debug.WriteLine(String.Format("p_histo_ctrl : {0:d}-{1:d}-{2:d}", phc.numVisit, phc.origine, phc.numChrono));
                                }
                            }
                        }
                        if (p.pAnoes != null)
                        {
                            foreach (var pan in p.pAnoes)
                            {
                                if (pan != null)
                                {
                                    pAno[String.Format("{0:d}-{1:d}-{2}", pan.numVisit, pan.origine, pan.comptExcep)] = pan;
                                    System.Diagnostics.Debug.WriteLine(String.Format("p_ano : {0:d}-{1:d}-{2}", pan.numVisit, pan.origine, pan.comptExcep));
                                }
                                if (pan.RAno != null)
                                {
                                    rAno[String.Format("{0}-{1}", pan.RAno.comptExcep, pan.RAno.dateDebutEffet)] = pan.RAno;
                                    System.Diagnostics.Debug.WriteLine(String.Format("r_ano : {0}-{1}", pan.RAno.comptExcep, pan.RAno.dateDebutEffet.ToString()));
                                }
                            }
                        }
                    }
                }
                //Avoid some bugs due to empty collections
                if (rDocs.Values.Count == 0) rDocs.Add("fake_item", new ClRDoc());
                if (pMesuress.Values.Count == 0) pMesuress.Add("fake_item", new ClPmesures());
                if (pHistoCtrl.Values.Count == 0) pHistoCtrl.Add("fake_item", new ClPHistoCtrl());
                if (pAno.Values.Count == 0) pAno.Add("fake_item", new ClPano());
                if (rAno.Values.Count == 0) rAno.Add("fake_item", new ClRano());

                //if (pDocConstates.Values.Count == 0) pDocConstates.Add("fake_item", new VlPDocConstate());
                //if (rLocalisations.Values.Count == 0) rLocalisations.Add("fake_item", new VlRLocalisation());
                //if (rMaterielStatuts.Values.Count == 0) rMaterielStatuts.Add("fake_item", new VlRMaterielStatut());
                //if (pLocalisationDocs.Values.Count == 0) pLocalisationDocs.Add("fake_item", new VlPLocalisationDoc());
                //Assign collections to report
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRDoc"].SetDataSource(rDocs.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPMesures"].SetDataSource(pMesuress.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPControle"].SetDataSource(pControles.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPTransfert"].SetDataSource(pTransferts.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRResultCtrl"].SetDataSource(rResultCtrls.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPDocConstate"].SetDataSource(pDocConstates.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRLocalisation"].SetDataSource(rLocalisations.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRNatureControle"].SetDataSource(rNatureControles.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPLocalisationDoc"].SetDataSource(pLocalisationDocs.Values);

                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRMaterielStatutOpa"].SetDataSource(rMaterielStatuts.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRMaterielStatutAdg"].SetDataSource(rMaterielStatuts.Values);

                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPVehiculePresente"].SetDataSource(pVehiculePresentes.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRNatureCtrlSuivant"].SetDataSource(rNatureCtrlSuivants.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRDocumentIdentification"].SetDataSource(rDocumentIdentifications.Values);
                pvTransfertCTCL.Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPHistoCtrl"].SetDataSource(pHistoCtrl.Values);
                pvTransfertCTCL.Subreports["ctclListHistoCtrl"].Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPHistoCtrl"].SetDataSource(pHistoCtrl.Values);
                pvTransfertCTCL.Subreports["ctclListAno"].Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClPAno"].SetDataSource(pAno.Values);
                pvTransfertCTCL.Subreports["ctclListAno"].Database.Tables["Utac_Rnc2_UI_Web_ServiceRnc2_ClRAno"].SetDataSource(rAno.Values);

                pvTransfertCTCL.SetParameterValue("niveauDetailParam", niveauDetailParam.GetHashCode());

                var pdfStream = pvTransfertCTCL.ExportToStream(ExportFormatType.PortableDocFormat);

                var respBuffer = new byte[1024];

                var bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                while (bytesRead > 0)
                {
                    memStream.Write(respBuffer, 0, bytesRead);
                    bytesRead = pdfStream.Read(respBuffer, 0, respBuffer.Length);
                }

            }
            catch (CrystalReportsException ex)
            {
                memStream.Close();
                memStream = null;

                var strMessageInnerEx = "Pas de innerException !";
                if (ex.InnerException != null)
                {
                    strMessageInnerEx = ex.InnerException.Message;
                }

                log.Error("Erreur lors de la génération de l'édition crystal (CrystalReportsException) " + ex.Message + " " + strMessageInnerEx);
            }
            catch (Exception ex)
            {
                memStream.Close();
                memStream = null;

                log.Error("Erreur lors de la génération de l'édition crystal (Exception) " + ex.Message);
            }
            finally
            {
                pvTransfertCTCL?.Close();
                pvTransfertCTCL?.Dispose();
            }
            log.Debug("GenereCtclPvPdf Fin...");
            return memStream;
        }

    }


}
