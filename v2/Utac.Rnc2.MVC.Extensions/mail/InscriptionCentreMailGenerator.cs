using System;

namespace Utac.Rnc2.MVC.Extensions.mail
{
    public class InscriptionCentreMailGenerator : MailBodyGenarator
    {
        public override string generateSubject()
        {
            string sujet = this._data.TemplateMail.SujetTemplate;

            return sujet;
        }

        public override string generateBody()
        {
            string body = this._data.TemplateMail.BodyTemplate;
            body = body.Replace("%prenom%", _data.Destinataire.Prenom);
            body = body.Replace("%nom%", _data.Destinataire.Nom);
            body = body.Replace("%login%", _data.Destinataire.Id);
            body = body.Replace("%password%", _data.Destinataire.MotDePasse);
            body = body.Replace("%mail_dest%", _data.Destinataire.Email);
            body = body.Replace("%telephone%", _data.Destinataire.NumTel);
            body = body.Replace("%ref_courrier%", Convert.ToString(_data.Courrier.Id));
            return body;
        }
    }
}
