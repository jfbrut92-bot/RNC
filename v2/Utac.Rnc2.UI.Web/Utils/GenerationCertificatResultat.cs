using System;

namespace Utac.Rnc2.UI.Web.Utils
{
    public class GenerationCertificatResultat
    {
        private bool _resultat;

        public bool Resultat
        {
            get { return _resultat; }
            set { _resultat = value; }
        }
        private byte[] _certificat;

        public byte[] Certificat
        {
            get { return _certificat; }
            set { _certificat = value; }
        }
        private int _numeroCertificat;

        public int NumeroCertificat
        {
            get { return _numeroCertificat; }
            set { _numeroCertificat = value; }
        }
        private DateTime _dateDebutValidite;

        public DateTime DateDebutValidite
        {
            get { return _dateDebutValidite; }
            set { _dateDebutValidite = value; }
        }
        private DateTime _dateFinValidite;

        public DateTime DateFinValidite
        {
            get { return _dateFinValidite; }
            set { _dateFinValidite = value; }
        }
        private string _numSerie;

        public string NumSerie
        {
            get { return _numSerie; }
            set { _numSerie = value; }
        }

        private string _erreurLibelle;

        public string ErreurLibelle
        {
            get { return _erreurLibelle; }
            set { _erreurLibelle = value; }
        }

        private string _messageException;

        public string MessageException
        {
            get { return _messageException; }
            set { _messageException = value; }
        }
        private string _nomCert;

        public string NomCert
        {
            get { return _nomCert; }
            set { _nomCert = value; }
        }
        private string _nomCommunCert;

        public string NomCommunCert
        {
            get { return _nomCommunCert; }
            set { _nomCommunCert = value; }
        }
        private string _passwordCert;

        public string PasswordCert
        {
            get { return _passwordCert; }
            set { _passwordCert = value; }
        }

    }
}