using System;
using System.IO;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace RemoteClientPKI
{

    public class CRemoteClientPKI
    {
        public CRemoteClientPKI() { }
        public int Add(int x, int y)
        {
            return (x + y);
        }

        /// <summary>
        /// Accès à l'objet CaInterfaceRemote pour la récupration d'un nouveau certificat Client
        /// </summary>
        /// <param name="sServeurPKI">sple\USERTEST : Serveur\Serveur de certificat</param>
        /// <param name="sNomCert">Nom du certificat : S0064Z023</param>
        /// <param name="sNomCommunCert">Nom commun du certificat : CTPL - LIMAY</param>
        /// <param name="sRepOut">Répertoire d'écriture du fichier certificat sur le client</param>
        /// <param name="sPasswordCert">password d'intallation du certificat</param>
        /// <param name="iReqid">Numéro de la demande de certificat</param>
        /// <param name="sDateDebutValidCert"></param>
        /// <param name="sDateFinValidCert"></param>
        /// <param name="sNumSerieCert"></param>
        /// <param name="iErreurCode">code d'erreur de retour 0 si OK</param>
        /// <param name="sErreurLibelle">Libellé de l'erreur</param>
        /// <param name="sMessageException">Message de l'exception levée</param>
        /// <returns>0 sur OK</returns>
        public int RecupereNouveauCertificat(string sServeurPKI, string sNomCert, string sNomCommunCert, string sRepOut, string sPasswordCert, out int iReqid, out string sDateDebutValidCert, out string sDateFinValidCert, out string sNumSerieCert, string sCatVehicule, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iresult = 0;
            iReqid = 0;
            sDateDebutValidCert = "";
            sDateFinValidCert = "";
            sNumSerieCert = "";

            byte[] byteArrayCert;

            try
            {
                iresult = RemoteInitClientPKI.GetRemoteObject().RecupereNouveauCertificat(sServeurPKI, sNomCert, sNomCommunCert, out byteArrayCert, sPasswordCert, out iReqid, sCatVehicule, out iErreurCode, out sErreurLibelle, out sMessageException);
                if (iresult == 0)
                {
                    File.WriteAllBytes(sRepOut + "\\" + sNomCert + ".pfx", byteArrayCert);

                    //Chargement du certificat

                    X509Certificate2 X509Cert = new X509Certificate2(byteArrayCert, sPasswordCert, X509KeyStorageFlags.MachineKeySet);
                    //X509Cert.Import(sRepOut + "\\" + sNomCert + ".pfx", sPasswordCert, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet);
                    //X509Cert.Import(sRepOut + "\\" + sNomCert + ".pfx", sPasswordCert, X509KeyStorageFlags.Exportable);
                    //X509Cert.Import(byteArrayCert, sPasswordCert, X509KeyStorageFlags.Exportable);

                    //Récupération des infos du certificat

                    sDateDebutValidCert = X509Cert.NotBefore.ToString("yyyy-MM-dd HH:mm:ss");
                    sDateFinValidCert = X509Cert.NotAfter.ToString("yyyy-MM-dd HH:mm:ss");
                    sNumSerieCert = X509Cert.SerialNumber;

                    X509Cert.Reset();
                    X509Cert = null;
                }

            }
            catch (IOException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur écriture fichier : " + sNomCert + ".pfx";
                sMessageException = e.Message;
            }
            catch (UnauthorizedAccessException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur droit écriture fichier : " + sNomCert + ".pfx";
                sMessageException = e.Message;
            }
            catch (RemotingException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur d'accès au serveur distant";
                sMessageException = e.Message;

            }
            catch (CryptographicException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : CryptographicException ->" + e.Message + " TargetSite :" + e.TargetSite.Name;
                sMessageException = "";
                if (e.InnerException != null)
                {
                    sMessageException = sMessageException + " InnerException : " + e.InnerException.ToString();
                }
                //sMessageException = sMessageException + " StackStrace :" + e.StackTrace;

            }

            catch (Exception e)
            {
                iresult = -1;
                if (e.Message == "-1")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Impossible d'instancier l'objet distant";
                    sMessageException = "";
                }
                else if (e.Message == "-2")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Lecture des paramètres de rnc2.ini";
                    sMessageException = "";
                }
                else
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : autre erreur ->" + e.GetType().ToString();
                    sMessageException = e.Message;
                }
            }

            return iresult;
        }

        /// <summary>
        /// Accès à l'objet CaInterfaceRemote pour la récupration d'un nouveau certificat Client
        /// </summary>
        /// <param name="sServeurPKI">sple\USERTEST : Serveur\Serveur de certificat</param>
        /// <param name="sNomCert">Nom du certificat : S0064Z023</param>
        /// <param name="sNomCommunCert">Nom commun du certificat : CTPL - LIMAY</param>
        /// <param name="sRepOut">Répertoire d'écriture du fichier certificat sur le client</param>
        /// <param name="sPasswordCert">password d'intallation du certificat</param>
        /// <param name="iReqid">Numéro de la demande de certificat</param>
        /// <param name="sDateDebutValidCert"></param>
        /// <param name="sDateFinValidCert"></param>
        /// <param name="sNumSerieCert"></param>
        /// <param name="iErreurCode">code d'erreur de retour 0 si OK</param>
        /// <param name="sErreurLibelle">Libellé de l'erreur</param>
        /// <param name="sMessageException">Message de l'exception levée</param>
        /// <returns>0 sur OK</returns>
        //public int RecupereNouveauCertificat(string sURL,bool sEnsureSecurity,string sUserName,string sPassword,string sDomain, string sServeurPKI, string sNomCert, string sNomCommunCert, string sRepOut, string sPasswordCert, out int iReqid, out string sDateDebutValidCert, out string sDateFinValidCert, out string sNumSerieCert, string sCatVehicule, out int iErreurCode, out string sErreurLibelle, out string sMessageException, out byte[] byteArrayCert)
        public int RecupereNouveauCertificat(string sURL, bool sEnsureSecurity, string sUserName, string sPassword, string sDomain, string sServeurPKI, string sNomCert, string sNomCommunCert, string sPasswordCert, out int iReqid, out string sDateDebutValidCert, out string sDateFinValidCert, out string sNumSerieCert, string sCatVehicule, out int iErreurCode, out string sErreurLibelle, out string sMessageException, out byte[] byteArrayCert)
        {
            int iresult = 0;
            iReqid = 0;
            sDateDebutValidCert = "";
            sDateFinValidCert = "";
            sNumSerieCert = "";

            //byte[] byteArrayCert;
            byteArrayCert = new byte[0];

            try
            {

                iresult = RemoteInitClientPKI.GetRemoteObject(sURL, sEnsureSecurity, sUserName, sPassword, sDomain).RecupereNouveauCertificat(sServeurPKI, sNomCert, sNomCommunCert, out byteArrayCert, sPasswordCert, out iReqid, sCatVehicule, out iErreurCode, out sErreurLibelle, out sMessageException);
                if (iresult == 0)
                {
                    //EG le 22/11/2011 
                    //File.WriteAllBytes(sRepOut + "\\" + sNomCert + ".pfx", byteArrayCert);

                    //Chargement du certificat
                    X509Certificate2 X509Cert = new X509Certificate2(byteArrayCert, sPasswordCert, X509KeyStorageFlags.MachineKeySet);

                    //Récupération des infos du certificat

                    sDateDebutValidCert = X509Cert.NotBefore.ToString("yyyy-MM-dd HH:mm:ss");
                    sDateFinValidCert = X509Cert.NotAfter.ToString("yyyy-MM-dd HH:mm:ss");
                    sNumSerieCert = X509Cert.SerialNumber;

                    X509Cert.Reset();
                    X509Cert = null;
                }

            }
            catch (IOException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur écriture fichier : " + sNomCert + ".pfx";
                sMessageException = e.Message;
            }
            catch (UnauthorizedAccessException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur droit écriture fichier : " + sNomCert + ".pfx";
                sMessageException = e.Message;
            }
            catch (RemotingException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur d'accès au serveur distant";
                sMessageException = e.Message;

            }
            catch (CryptographicException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : CryptographicException ->" + e.Message + " TargetSite :" + e.TargetSite.Name;
                sMessageException = "";
                if (e.InnerException != null)
                {
                    sMessageException = sMessageException + " InnerException : " + e.InnerException.ToString();
                }
                //sMessageException = sMessageException + " StackStrace :" + e.StackTrace;

            }

            catch (Exception e)
            {
                iresult = -1;
                if (e.Message == "-1")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Impossible d'instancier l'objet distant";
                    sMessageException = "";
                }
                else if (e.Message == "-2")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Lecture des paramètres de rnc2.ini";
                    sMessageException = "";
                }
                else
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : autre erreur ->" + e.GetType().ToString();
                    sMessageException = e.Message;
                }
            }

            return iresult;
        }

        public int RecupereCACertificat(string sServeurPKI, out string sNomCert, string sRepOut, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iresult = 0;
            int i;
            string sCertificat = "";
            sNomCert = "";

            try
            {
                iresult = RemoteInitClientPKI.GetRemoteObject().RecupereCACertificat(sServeurPKI, out sCertificat, out iErreurCode, out sErreurLibelle, out sMessageException);
                if (iresult == 0)
                {

                    //Transformation au format binaire
                    byte[] sCertBin = new byte[sCertificat.Length];
                    for (i = 0; i < sCertificat.Length; i++) sCertBin[i] = (Convert.ToByte(sCertificat[i]));

                    X509Certificate2 X509Cert = new X509Certificate2();
                    //Chargement du certificat
                    X509Cert.Import(sCertBin);

                    //Récupération du non du CA
                    string sIssuer = X509Cert.Issuer;
                    string[] sIssuerSplits = sIssuer.Split(new Char[] { ',' });
                    for (i = 0; i < sIssuerSplits.Length; i++)
                    {
                        if (sIssuerSplits[i].StartsWith("CN="))
                            sNomCert = sIssuerSplits[i].Substring(3);
                    }

                    //Export du certificat au format cer
                    byte[] certData = X509Cert.Export(X509ContentType.Cert);

                    //Ecriture du fichier
                    File.WriteAllBytes(sRepOut + "\\" + sNomCert + ".cer", certData);

                    X509Cert.Reset();
                    X509Cert = null;
                }

            }
            catch (IOException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur écriture fichier : " + sRepOut + "\\" + sNomCert + ".cer";
                sMessageException = e.Message;
            }
            catch (UnauthorizedAccessException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur droit écriture fichier : " + sRepOut + "\\" + sNomCert + ".cer";
                sMessageException = e.Message;
            }
            catch (RemotingException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur d'accès au serveur distant";
                sMessageException = e.Message;

            }
            catch (Exception e)
            {
                iresult = -1;
                if (e.Message == "-1")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Impossible d'instancier l'objet distant";
                    sMessageException = "";
                }
                else if (e.Message == "-2")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Lecture des paramètres de rnc2.ini";
                    sMessageException = "";
                }
                else
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : autre erreur ->" + e.GetType().ToString();
                    sMessageException = e.Message;
                }
            }

            return iresult;
        }

        /// <summary>
        /// Test d'accès au serveur de CA et récupération des rôles du USER courant sur l'autorité de certification.
        /// </summary>
        /// <param name="sServeurPKI"></param>
        /// <param name="sRolesServeurCA"></param>
        /// <param name="iErreurCode"></param>
        /// <param name="sErreurLibelle"></param>
        /// <param name="sMessageException"></param>
        /// <returns>0 si OK</returns>
        public int TestAccesServeurCA(string sServeurPKI, out string sRolesServeurCA, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iresult = 0;
            sRolesServeurCA = "";
            try
            {
                iresult = RemoteInitClientPKI.GetRemoteObject().TestAccesServeurCA(sServeurPKI, out sRolesServeurCA, out iErreurCode, out sErreurLibelle, out sMessageException);
            }
            catch (RemotingException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur d'accès au serveur distant";
                sMessageException = e.Message;

            }
            catch (Exception e)
            {
                iresult = -1;
                if (e.Message == "-1")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Impossible d'instancier l'objet distant";
                    sMessageException = "";
                }
                else if (e.Message == "-2")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Lecture des paramètres de rnc2.ini";
                    sMessageException = "";
                }
                else
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : autre erreur ->" + e.GetType().ToString();
                    sMessageException = e.Message;
                }
            }

            return iresult;
        }

        /// <summary>
        /// Test d'accès au serveur de CA et récupération des rôles du USER courant sur l'autorité de certification.
        /// </summary>
        /// <param name="sURL"></param>
        /// <param name="sEnsureSecurity"></param>
        /// <param name="sUserName"></param>
        /// <param name="sPassword"></param>
        /// <param name="sDomain"></param>
        /// <param name="sServeurPKI"></param>
        /// <param name="sRolesServeurCA"></param>
        /// <param name="iErreurCode"></param>
        /// <param name="sErreurLibelle"></param>
        /// <param name="sMessageException"></param>
        /// <returns>0 si OK</returns>
        public int TestAccesServeurCA(string sURL, bool sEnsureSecurity, string sUserName, string sPassword, string sDomain, string sServeurPKI, out string sRolesServeurCA, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iresult = 0;
            sRolesServeurCA = "";
            try
            {
                iresult = RemoteInitClientPKI.GetRemoteObject(sURL, sEnsureSecurity, sUserName, sPassword, sDomain).TestAccesServeurCA(sServeurPKI, out sRolesServeurCA, out iErreurCode, out sErreurLibelle, out sMessageException);
            }
            catch (RemotingException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur d'accès au serveur distant";
                sMessageException = e.Message;

            }
            catch (Exception e)
            {
                iresult = -1;
                if (e.Message == "-1")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Impossible d'instancier l'objet distant";
                    sMessageException = "";
                }
                else if (e.Message == "-2")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Lecture des paramètres de rnc2.ini";
                    sMessageException = "";
                }
                else
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : autre erreur ->" + e.GetType().ToString();
                    sMessageException = e.Message;
                }
            }

            return iresult;
        }

        public int RevoqueCertificat(string sServeurPKI, string sNumSerieCert, int iRaisonRevoquation, DateTime dDateRevoquation, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iresult = 0;
            try
            {
                iresult = RemoteInitClientPKI.GetRemoteObject().RevoqueCertificat(sServeurPKI, sNumSerieCert, iRaisonRevoquation, dDateRevoquation, out iErreurCode, out sErreurLibelle, out sMessageException);
            }
            catch (RemotingException e)
            {
                iresult = -1;
                iErreurCode = -1;
                sErreurLibelle = "ERREUR Client Remote : Erreur d'accès au serveur distant";
                sMessageException = e.Message;

            }
            catch (Exception e)
            {
                iresult = -1;
                if (e.Message == "-1")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Impossible d'instancier l'objet distant";
                    sMessageException = "";
                }
                else if (e.Message == "-2")
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : Lecture des paramètres de rnc2.ini";
                    sMessageException = "";
                }
                else
                {
                    iErreurCode = -1;
                    sErreurLibelle = "ERREUR Client Remote : autre erreur ->" + e.GetType().ToString();
                    sMessageException = e.Message;
                }
            }

            return iresult;
        }

    }

}
