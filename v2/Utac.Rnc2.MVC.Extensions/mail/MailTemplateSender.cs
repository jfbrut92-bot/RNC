using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;

namespace Utac.Rnc2.MVC.Extensions.mail
{
    public class MailTemplateSender
    {

        private readonly MailData _data = null;
        private readonly rTemplateMail.TemplateEnum _templateEnum;
        private readonly IrTemplateMailService _rTemplateMailService;
        private readonly IpCourrierEnvoyeService _courrierService;

        private readonly Dictionary<rTemplateMail.TemplateEnum, MailBodyGenarator> _mailGeneratorDictionnary = null;

        private MailTemplateSender()
        {
            this._rTemplateMailService = NhibernateServiceFactory.getInstance().getrTemplateMailService(false);
            this._courrierService = NhibernateServiceFactory.getInstance().getpCourrierEnvoyeService(false);
            _mailGeneratorDictionnary = new Dictionary<rTemplateMail.TemplateEnum, MailBodyGenarator>();
            NouvelUtilisateurAndChangePasswordMailGenerator g1 = new NouvelUtilisateurAndChangePasswordMailGenerator();
            _mailGeneratorDictionnary.Add(rTemplateMail.TemplateEnum.NOUVEL_UTILISATEUR, g1);
            _mailGeneratorDictionnary.Add(rTemplateMail.TemplateEnum.NOUVEAU_PASSWORD, g1);
            _mailGeneratorDictionnary.Add(rTemplateMail.TemplateEnum.NOUVEAU_EMAIL, g1);
            _mailGeneratorDictionnary.Add(rTemplateMail.TemplateEnum.INSCRIPTION_CENTRE_INDEPENDANT, new InscriptionCentreMailGenerator());
            _mailGeneratorDictionnary.Add(rTemplateMail.TemplateEnum.PREMIER_CERTIFICAT, new PremierCertificatMailGenerator());
            _mailGeneratorDictionnary.Add(rTemplateMail.TemplateEnum.COPIE_CERTIFICAT, new PremierCertificatMailGenerator());
            _mailGeneratorDictionnary.Add(rTemplateMail.TemplateEnum.RENOUVELLEMENT_CERTIFICAT, new PremierCertificatMailGenerator());

        }

        public MailTemplateSender(rUtilisateur declencheur, rUtilisateur destinataire, rTemplateMail.TemplateEnum templateMailEnum, string logoPath) : this()
        {
            _data = new MailData();
            _data.Declencheur = declencheur;
            _data.Destinataire = destinataire;
            _data.TemplateMail = this._rTemplateMailService.FindByName(templateMailEnum.ToString());
            _data.PathLogo = logoPath;
            _templateEnum = templateMailEnum;
        }

        public MailTemplateSender(MailTemplateSenderArguments mailArgs)
            : this()
        {
            _data = new MailData();
            _data.Declencheur = mailArgs.Declencheur;
            _data.Destinataire = mailArgs.Destinataire;
            _data.TemplateMail = this._rTemplateMailService.FindByName(mailArgs.TemplateMailEnum.ToString());
            _data.PathLogo = mailArgs.LogoPath;
            _data.Certificat = mailArgs.Certificat;
            _templateEnum = mailArgs.TemplateMailEnum;
        }

        public bool Send()
        {
            _data.Courrier = this._courrierService.nouveau(_data.Declencheur, _data.Destinataire);
            MailBodyGenarator mailGenerator = this._mailGeneratorDictionnary[this._templateEnum];
            mailGenerator.setData(_data);
            string contenu = mailGenerator.generateBody();
            string objet = mailGenerator.generateSubject();
            bool etatEnvoi = Mailer.SendHtml(objet, contenu, _data.Destinataire.Email, _data.PathLogo);
            _data.Courrier.Resultat = etatEnvoi ? "OK" : "KO";
            this._courrierService.modifier(_data.Courrier);

            return etatEnvoi;
        }

        public struct MailTemplateSenderArguments
        {
            public rUtilisateur Declencheur;
            public rUtilisateur Destinataire;
            public rTemplateMail.TemplateEnum TemplateMailEnum;
            public string LogoPath;
            public pCertificat Certificat;

        }

    }
}
