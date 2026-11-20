using System;
using System.Diagnostics;

namespace RemoteCaInterface
{
    public class CRemoteCaInterface : MarshalByRefObject
    {
        private readonly EventLog EventLog1;

        public CRemoteCaInterface()
        {
            Console.WriteLine("CRemoteCaInterface activated");

            if (!System.Diagnostics.EventLog.SourceExists("RemoteCaInterface"))
                System.Diagnostics.EventLog.CreateEventSource("RemoteCaInterface", "Application");

            EventLog1 = new System.Diagnostics.EventLog();
            EventLog1.Source = "RemoteCaInterface";

        }

        public int RecupereNouveauCertificat(string sServeurPKI, string sNomCert, string sNomCommunCert, out byte[] byteArrayCert, string sPasswordCert, out int iReqid, string sCatVehicule, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iResult = 0;
            byteArrayCert = null;
            iReqid = 0;

            sErreurLibelle = "";
            sMessageException = "";
            iErreurCode = 0;

            Console.WriteLine("Début CRemoteCaInterface.RecupereNouveauCertificat : {0}", sNomCert);

            try
            {
                iResult = CaInterface.CaInterface.RecupereNouveauCertificat2(sServeurPKI, sNomCert, sNomCommunCert, out byteArrayCert, sPasswordCert, out iReqid, sCatVehicule);
                Console.WriteLine("Fin CRemoteCaInterface.RecupereNouveauCertificat : {0} , IDRequest: {1}", sNomCert, iReqid);
            }
            catch (Exception e)
            {
                string slibErreurGlobal = "";

                CaInterface.CaInterface.GestionExceptionGlobal(e, out iErreurCode, out sErreurLibelle, out sMessageException);

                Console.WriteLine("Exception appel CaInterface.RecupereNouveauCertificat :{0}", e);

                slibErreurGlobal = "ERREUR appel CaInterface.RecupereNouveauCertificat(" + sServeurPKI + "," + sNomCert + "," + sNomCommunCert + ")\n" + e.ToString();

                if (e.InnerException != null)
                {
                    slibErreurGlobal += "\n" + e.InnerException.ToString();
                }

                slibErreurGlobal += "\nERREURCODE=" + iErreurCode.ToString();
                slibErreurGlobal += "\nERREURLIB=" + sErreurLibelle;

                EventLog1.WriteEntry(slibErreurGlobal, EventLogEntryType.Error);

                //l'appel a échoué
                iResult = 1;
            }

            return iResult;
        }

        public int RecupereCACertificat(string sServeurPKI, out string sCertificat, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iResult = 0;

            sErreurLibelle = "";
            sMessageException = "";
            iErreurCode = 0;

            sCertificat = "";

            Console.WriteLine("Début CRemoteCaInterface.RecupereCACertificat : {0}", sServeurPKI);

            try
            {
                iResult = CaInterface.CaInterface.RecupereCACertificat(sServeurPKI, out sCertificat);
                Console.WriteLine("Fin CRemoteCaInterface.RecupereCACertificat : {0}", sServeurPKI);
            }
            catch (Exception e)
            {
                string slibErreurGlobal = "";

                CaInterface.CaInterface.GestionExceptionGlobal(e, out iErreurCode, out sErreurLibelle, out sMessageException);

                Console.WriteLine("Exception appel CaInterface.RecupereCACertificat :{0}", e);

                slibErreurGlobal = "ERREUR appel CaInterface.RecupereCACertificat(" + sServeurPKI + ")\n" + e.ToString();

                if (e.InnerException != null)
                {
                    slibErreurGlobal += "\n" + e.InnerException.ToString();
                }

                slibErreurGlobal += "\nERREURCODE=" + iErreurCode.ToString();
                slibErreurGlobal += "\nERREURLIB=" + sErreurLibelle;

                EventLog1.WriteEntry(slibErreurGlobal, EventLogEntryType.Error);

                //l'appel a échoué
                iResult = 1;
            }
            return iResult;
        }

        public int RevoqueCertificat(string sServeurPKI, string sNumSerieCert, int iRaisonRevoquation, DateTime dDateRevoquation, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iResult = 0;

            sErreurLibelle = "";
            sMessageException = "";
            iErreurCode = 0;

            Console.WriteLine("Début CRemoteCaInterface.RevoqueCertificat : {0}", sServeurPKI);

            try
            {
                iResult = CaInterface.CaInterface.RevoqueCertificat(sServeurPKI, sNumSerieCert, iRaisonRevoquation, dDateRevoquation);
                Console.WriteLine("Fin CRemoteCaInterface.RevoqueCertificat : {0} Num Série : {1}", sServeurPKI, sNumSerieCert);
            }
            catch (Exception e)
            {
                string slibErreurGlobal = "";

                CaInterface.CaInterface.GestionExceptionGlobal(e, out iErreurCode, out sErreurLibelle, out sMessageException);

                Console.WriteLine("Exception appel CaInterface.RevoqueCertificat :{0}", e);

                slibErreurGlobal = "ERREUR appel CaInterface.RevoqueCertificat(" + sServeurPKI + ")\n" + sNumSerieCert + "\n" + iRaisonRevoquation.ToString() + "\n" + dDateRevoquation + "\n" + e.ToString();

                if (e.InnerException != null)
                {
                    slibErreurGlobal += "\n" + e.InnerException.ToString();
                }

                slibErreurGlobal += "\nERREURCODE=" + iErreurCode.ToString();
                slibErreurGlobal += "\nERREURLIB=" + sErreurLibelle;

                EventLog1.WriteEntry(slibErreurGlobal, EventLogEntryType.Error);

                //l'appel a échoué
                iResult = 1;
            }

            return iResult;
        }

        public int TestAccesServeurCA(string sServeurPKI, out string sRolesServeurCA, out int iErreurCode, out string sErreurLibelle, out string sMessageException)
        {
            int iResult = 0;
            sRolesServeurCA = "";
            sErreurLibelle = "";
            sMessageException = "";
            iErreurCode = 0;

            Console.WriteLine("Début CRemoteCaInterface.TestAccesServeurCA : {0}", sServeurPKI);

            try
            {
                iResult = CaInterface.CaInterface.TestAccesServeurCA(sServeurPKI, out sRolesServeurCA);
                Console.WriteLine("Fin CRemoteCaInterface.TestAccesServeurCA : {0} , RolesServeurCA: {1}", sServeurPKI, sRolesServeurCA);
            }
            catch (Exception e)
            {
                string slibErreurGlobal = "";

                CaInterface.CaInterface.GestionExceptionGlobal(e, out iErreurCode, out sErreurLibelle, out sMessageException);

                Console.WriteLine("Exception appel CaInterface.TestAccesServeurCA :{0}", e);

                slibErreurGlobal = "ERREUR appel CaInterface.TestAccesServeurCA(" + sServeurPKI + ")\n" + e.ToString();

                if (e.InnerException != null)
                {
                    slibErreurGlobal += "\n" + e.InnerException.ToString();
                }

                slibErreurGlobal += "\nERREURCODE=" + iErreurCode.ToString();
                slibErreurGlobal += "\nERREURLIB=" + sErreurLibelle;

                EventLog1.WriteEntry(slibErreurGlobal, EventLogEntryType.Error);

                //l'appel a échoué
                iResult = 1;
            }

            return iResult;
        }
    }
}

