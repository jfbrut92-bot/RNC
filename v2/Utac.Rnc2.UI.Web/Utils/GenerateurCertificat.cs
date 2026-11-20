using System;
using System.Configuration;
using log4net;
using RemoteClientPKI;

namespace Utac.Rnc2.UI.Web.Utils
{
    public class GenerateurCertificat
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GenerateurCertificat));
        /// <summary>
        /// Genere un certificat SSL
        /// </summary>
        /// <param name="info">variable contenant les caractéristiques du certificat</param>
        /// <returns>une variable de type GenerationCertificatResultat</returns>
        public static GenerationCertificatResultat genere(InfoCertificat infoCert)
        {
            //Lecture du fichier web.config cf <appSettings>
            string sURL = ConfigurationManager.AppSettings["remoting_url"];
            bool sEnsureSecurity = Convert.ToBoolean(ConfigurationManager.AppSettings["remoting_security"]);
            string sUsername = ConfigurationManager.AppSettings["remoting_auth_user"];
            string sPassword = ConfigurationManager.AppSettings["remoting_auth_pwd"];
            string sDomain = ConfigurationManager.AppSettings["remoting_auth_domain"];
            string sServeurPKI = ConfigurationManager.AppSettings["remoting_pki_server"];
            // EG le 22/11/2011  : on n'écrit plus sur le disque
            // string sRepout = ConfigurationManager.AppSettings["temporary_folder"];
            string sNomCert = infoCert.NomCert;
            string sNomCommunCert = infoCert.NomCommunCert;
            string sPasswordCert = infoCert.MotDePassePfx;
            string sCategorie = infoCert.CategorieVehicule;

            int iReqid;
            string sDateDebutValidCert;
            string sDateFinValidCert;
            string sNumSerieCert;
            int iErreurCode;
            string sErreurLibelle;
            string sMessageException;
            byte[] byteArrayCert;

            log.Info("Genere debut : " + sNomCert + " " + sNomCommunCert + " " + sCategorie);

            CRemoteClientPKI clientPKI = new CRemoteClientPKI();

            int lresult = clientPKI.RecupereNouveauCertificat(sURL, sEnsureSecurity, sUsername, sPassword, sDomain, sServeurPKI, sNomCert, sNomCommunCert, sPasswordCert, out iReqid, out sDateDebutValidCert, out sDateFinValidCert, out sNumSerieCert, sCategorie, out iErreurCode, out sErreurLibelle, out sMessageException, out byteArrayCert);
            GenerationCertificatResultat resultat = new GenerationCertificatResultat();

            if (lresult == 0)
            {
                resultat.Resultat = true;
                resultat.NumeroCertificat = iReqid;
                resultat.NumSerie = sNumSerieCert;
                resultat.DateDebutValidite = Convert.ToDateTime(sDateDebutValidCert);
                resultat.DateFinValidite = Convert.ToDateTime(sDateFinValidCert);
                resultat.NomCert = sNomCert;
                resultat.NomCommunCert = sNomCommunCert;
                resultat.PasswordCert = sPasswordCert;
                resultat.Certificat = byteArrayCert;
            }
            else
            {
                resultat.Resultat = false;
                resultat.ErreurLibelle = sErreurLibelle;
                resultat.MessageException = sMessageException;
            }

            log.Info("Genere Fin" + sNomCert + " " + sNomCommunCert + " " + sCategorie);
            return resultat;
        }

    }
}
