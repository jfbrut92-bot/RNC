using System;

namespace RemoteCaInterface
{
    public interface ICRemoteCaInterface
    {
        int RecupereNouveauCertificat(string sServeurPKI, string sNomCert, string sNomCommunCert, out byte[] byteArrayCert, string sPasswordCert, out int iReqid, string sCatVehicule, out int iErreurCode, out string sErreurLibelle, out string sMessageException);
        int RecupereCACertificat(string sServeurPKI, out string sCertificat, out int iErreurCode, out string sErreurLibelle, out string sMessageException);
        int RevoqueCertificat(string sServeurPKI, string sNumSerieCert, int iRaisonRevoquation, DateTime dDateRevoquation, out int iErreurCode, out string sErreurLibelle, out string sMessageException);
        int TestAccesServeurCA(string sServeurPKI, out string sRolesServeurCA, out int iErreurCode, out string sErreurLibelle, out string sMessageException);
    }
}

