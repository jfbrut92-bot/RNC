using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.MVC.Extensions.mail
{
    public class MailData
    {
        private rUtilisateur _declencheur;

        public rUtilisateur Declencheur
        {
            get { return _declencheur; }
            set { _declencheur = value; }
        }
        private rUtilisateur _destinataire;

        public rUtilisateur Destinataire
        {
            get { return _destinataire; }
            set { _destinataire = value; }
        }
        private pCourrierEnvoye _courrier;

        public pCourrierEnvoye Courrier
        {
            get { return _courrier; }
            set { _courrier = value; }
        }

        private string _pathLogo;

        public string PathLogo
        {
            get { return _pathLogo; }
            set { _pathLogo = value; }
        }

        private rTemplateMail _templateMail;

        public rTemplateMail TemplateMail
        {
            get { return _templateMail; }
            set { _templateMail = value; }
        }

        /* Pour les certificats */

        public pCertificat Certificat { get; set; }
    }
}
