using System;
using System.Runtime.InteropServices;
using CERTCLILib;
using CERTENROLLLib;
//using CERTADMINLib;

namespace CaInterface
{
    public class CaInterface
    {
        private const int INDEX_DATE_REVOCATION = 13;
        private const int INDEX_DISP_DEMANDE = 9;
        private const int DISP_DEMANDE_OK = 20;
        private const int DISP_REVOQUE = 21;
        private const int DISP_REFUSE = 31;
        //private const int CR_DISP_ISSUED = 3;
        private const int AT_KEYEXCHANGE = 1;
        private const int CRYPT_EXPORTABLE = 1;
        private const int CERT_FRIENDLY_NAME_PROP_ID = 11;
        private const int XECP_STRING_PROPERTY = 1;
        private const int XECR_CMC = 3;
        private const int CR_IN_BASE64HEADER = 0;
        private const int CR_IN_FORMATANY = 0;
        private const int CR_IN_BASE64 = 0x1;
        private const int CR_OUT_BASE64 = 0x1;
        private const int CR_OUT_BASE64HEADER = 0;
        private static readonly int CR_OUT_CHAIN = HexUtils.HexToInt("H100");
        private static readonly int CR_OUT_BINARY = HexUtils.HexToInt("H2");
        private static readonly int PROPTYPE_STRING = HexUtils.HexToInt("H4");
        private static readonly int CV_OUT_BASE64 = HexUtils.HexToInt("H1");

        //certRequest.Submit return
        private const int CR_DISP_ERROR = 1;
        private const int CR_DISP_DENIED = 2;
        private const int CR_DISP_ISSUED = 3;
        private const int CR_DISP_ISSUED_OUT_OF_BAND = 4;
        private const int CR_DISP_UNDER_SUBMISSION = 5;

        private const int CR_CAPPROP_CANAME = 6;

        //Droit du user sur l'autorité de certification
        private const int CTE_MYROLE_OK = 770; //Lecture + emmettre et gérer les ceritificats + Demander des certificats

        private static readonly int COM_EXCEP_ERRORCODE_SERVEUR_CA_INACESSIBLE = -2147023174;

        /// <summary>
        /// Accès à l'objet CaInterfaceRemote pour la récupration d'un nouveau certificat Client
        /// </summary>
        /// <param name="sServeurPKI">nom serveur\nom de l'autorité de certification</param>
        /// <param name="sNomCert"> Nom court du certificat</param>
        /// <param name="sNomCommunCert">sNomCommunCert</param>
        /// <param name="byteArrayCert">fichier certificat au format binaire</param>
        /// <param name="sPasswordCert">mot de passe d'ouverture du certificat</param>
        /// <param name="iReqid">numéro de la requête de demande de certificat</param>
        /// <returns>0 si OK</returns>
        public static int RecupereNouveauCertificat2(string sServeurPKI, string sNomCert, string sNomCommunCert, out byte[] byteArrayCert, string sPasswordCert, out int iReqid, string sCatVehicule)
        {

            CX509CertificateRequestPkcs10 objPkcs10 = new CX509CertificateRequestPkcs10();
            CX509PrivateKey objPrivateKey = new CX509PrivateKey();
            CCspInformation objCSP = new CCspInformation();
            CCspInformations objCSPs = new CCspInformations();
            CX500DistinguishedName objDN = new CX500DistinguishedName();
            CX509Enrollment objEnroll = new CX509Enrollment();
            CObjectIds objObjectIds = new CObjectIds();
            CObjectId objObjectId = new CObjectId();
            CX509ExtensionKeyUsage objExtensionKeyUsage = new CX509ExtensionKeyUsage();
            CX509ExtensionEnhancedKeyUsage objX509ExtensionEnhancedKeyUsage = new CX509ExtensionEnhancedKeyUsage();

            objCSP.InitializeFromName("Microsoft Enhanced RSA and AES Cryptographic Provider");
            objCSPs.Add(objCSP);
            objPrivateKey.Length = 2048;

            //objPrivateKey.KeySpec = X509KeySpec.XCN_AT_SIGNATURE;
            objPrivateKey.KeySpec = X509KeySpec.XCN_AT_KEYEXCHANGE;
            objPrivateKey.KeyUsage = X509PrivateKeyUsageFlags.XCN_NCRYPT_ALLOW_ALL_USAGES;
            objPrivateKey.MachineContext = false;
            //  Provide the CSP collection object (in this case containing only 1 CSP object)
            //  to the private key object
            objPrivateKey.CspInformations = objCSPs;
            objPrivateKey.ExportPolicy = X509PrivateKeyExportFlags.XCN_NCRYPT_ALLOW_EXPORT_FLAG;

            objPrivateKey.Create();

            objPkcs10.InitializeFromPrivateKey(X509CertificateEnrollmentContext.ContextUser, objPrivateKey, "");
            objPkcs10.SuppressDefaults = true;
            // Key Usage Extension

            objExtensionKeyUsage.InitializeEncode(
                X509KeyUsageFlags.XCN_CERT_DIGITAL_SIGNATURE_KEY_USAGE |
                X509KeyUsageFlags.XCN_CERT_NON_REPUDIATION_KEY_USAGE |
                X509KeyUsageFlags.XCN_CERT_KEY_ENCIPHERMENT_KEY_USAGE |
                X509KeyUsageFlags.XCN_CERT_DATA_ENCIPHERMENT_KEY_USAGE
            );

            objPkcs10.X509Extensions.Add((CX509Extension)objExtensionKeyUsage);

            // Enhanced Key Usage Extension

            objObjectId.InitializeFromValue("1.3.6.1.5.5.7.3.2"); // OID for Client Authentication usage
            objObjectIds.Add(objObjectId);
            objX509ExtensionEnhancedKeyUsage.InitializeEncode(objObjectIds);
            objPkcs10.X509Extensions.Add((CX509Extension)objX509ExtensionEnhancedKeyUsage);

            string cn = "CN=" + sNomCert + ",OU=UTAC-OTC-" + sCatVehicule + ",O=UTAC,E=infos@utac.com,L=Monthlery,S=Ile-De-France,C=FR";
            objDN.Encode(cn, X500NameFlags.XCN_CERT_NAME_STR_NONE);

            //  Assing the subject name by using the Distinguished Name object initialized above
            objPkcs10.Subject = objDN;

            // Create enrollment request
            objEnroll.InitializeFromRequest(objPkcs10);
            objEnroll.CertificateFriendlyName = sNomCommunCert;

            String strRequest = objEnroll.CreateRequest(EncodingType.XCN_CRYPT_STRING_BASE64);

            //  Create all the objects that will be required
            CCertConfig objCertConfig = new CCertConfig();
            CCertRequest objCertRequest = new CCertRequest();

            int iDisposition;
            string strDisposition;
            string strCert;

            // Submit the request
            iDisposition = objCertRequest.Submit(
                  CR_IN_BASE64 | CR_IN_FORMATANY,
                  strRequest,
                  null,
                  sServeurPKI
             );
            // Check the submission status
            if (CR_DISP_ISSUED != iDisposition) // Not enrolled
            {
                strDisposition = objCertRequest.GetDispositionMessage();
                if (CR_DISP_UNDER_SUBMISSION == iDisposition) // Pending
                {
                    strDisposition = "The submission is pending: " + strDisposition;
                }
                else // Failed
                {
                    strDisposition = "The submission failed: " + strDisposition + "  Last status: " + objCertRequest.GetLastStatus().ToString();
                }

                throw new Exception("CaInterface.RecupereNouveauCertificat() -> Impossible de générer un certificat\n CA :" + sServeurPKI +
                   "\nNom : " + sNomCert +
                   "\nNom Commun : " + sNomCommunCert + "\n ERROR :" + strDisposition);
            }

            // Get the certificate
            strCert = objCertRequest.GetCertificate(
                CR_OUT_BASE64 | CR_OUT_CHAIN
            );

            iReqid = objCertRequest.GetRequestId();

            CX509Enrollment install = new CX509Enrollment();
            install.Initialize(X509CertificateEnrollmentContext.ContextUser);

            install.InstallResponse(InstallResponseRestrictionFlags.AllowUntrustedRoot, strCert, EncodingType.XCN_CRYPT_STRING_BASE64, sPasswordCert);
            string strout = install.CreatePFX(sPasswordCert, PFXExportOptions.PFXExportChainWithRoot);

            byteArrayCert = Convert.FromBase64String(strout);
            return 0;

        }
        /// <summary>
        /// Récuperation du certificat Racine
        /// </summary>
        /// <param name="sServeurPKI">nom serveur\nom de l'autorité de certification</param>
        /// <param name="sNomCert">Nom du certificat racine</param>
        /// <param name="byteArrayCert">fichier certificat au format binaire</param>
        /// <returns></returns>
        public static int RecupereCACertificat(string sServeurPKI, out string sCertificat)
        {

            CCertRequest objCertRequest = new CCertRequest();
            sCertificat = objCertRequest.GetCACertificate(0, sServeurPKI, CR_OUT_BINARY);

            return 0;
        }

        /*
         Renvoie les rôles de l'utilisateur courant sur le serveur de certificat
         Retour int : 0 --> Les rôles correspondent aux ceux attendus
                      1 --> Les rôles de correspondent pas à ceux attendus pour un fonctionnement correcte de l'application
         */
        public static int TestAccesServeurCA(string sServeurPKI, out string RolesServeurCA)
        {
            //28/10/2020 Le testeur d'accès n'est plus implémenté car par d'accès à CERTADM
            //CCertAdmin certAdmin = new CCertAdmin();
            int lResult = 0;
            RolesServeurCA = "";
            //lResult = certAdmin.GetMyRoles(sServeurPKI);
            //RolesServeurCA = lResult.ToString();
            //if (lResult == CTE_MYROLE_OK)
            // { 
            //    lResult = 0;
            // }
            //else
            // {
            //     lResult = 1;
            // };

            CCertRequest objCertRequest = new CCertRequest();
            String sCertificat;
            sCertificat = objCertRequest.GetCACertificate(0, sServeurPKI, CR_OUT_BINARY);

            if (sCertificat != null)
            {
                lResult = 0;
                RolesServeurCA = CTE_MYROLE_OK.ToString();
            }
            else
            {
                lResult = 1;
                RolesServeurCA = "";
            };

            return lResult;
        }

        public static int RevoqueCertificat(string sServeurPKI, string sNumSerieCert, int iRaisonRevoquation, DateTime dDateRevoquation)
        {
            //28/10/2020 la révocation d'un certificat n'est plus implémenté car par d'accès à CERTADM
            //Creation de l'objet qui delivre le certif.
            //CCertAdmin certAdmin = new CCertAdmin();
            //Révocation du certificat
            //certAdmin.RevokeCertificate(sServeurPKI, sNumSerieCert, iRaisonRevoquation, dDateRevoquation);
            return 0;
        }

        /*
         Gestion des exceptions d'appel au fonctions COM
         */
        public static void GestionExceptionGlobal(Exception e, out int iErrorCode, out string sErreurlibelle, out string sMessageException)
        {
            sMessageException = e.Message;
            GestionException(e, out iErrorCode, out sErreurlibelle);
        }

        private static void GestionException(COMException e, out int iErrorCode, out string sErreurlibelle)
        {
            iErrorCode = e.ErrorCode;
            if (iErrorCode == COM_EXCEP_ERRORCODE_SERVEUR_CA_INACESSIBLE)
                sErreurlibelle = "ERREUR serveur Remote: Le serveur de certificat est inaccessible par le composant COM.";
            else
                sErreurlibelle = "ERREUR Serveur Remote : Erreur Généré par le composant COM sur le serveur de certificat.";

        }

        private static void GestionException(ArgumentException e, out int iErrorCode, out string sErreurlibelle)
        {
            iErrorCode = 1;

            sErreurlibelle = "ERREUR Serveur Remote: Le nom du serveur serveur de certificat est incorrecte.";

        }

        private static void GestionException(UnauthorizedAccessException e, out int iErrorCode, out string sErreurlibelle)
        {
            iErrorCode = 1;

            sErreurlibelle = "Erreur Serveur Remote: L'utilisateur accédant au serveur de certificat n'a pas les droits d'accés suffisant.";

        }

        private static void GestionException(Exception e, out int iErrorCode, out string sErreurlibelle)
        {
            iErrorCode = 1;

            sErreurlibelle = "Erreur Serveur Remote : Autre Exception";

        }

    }
}
