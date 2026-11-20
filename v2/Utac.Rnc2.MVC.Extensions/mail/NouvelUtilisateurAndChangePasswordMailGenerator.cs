using System;

namespace Utac.Rnc2.MVC.Extensions.mail
{
    public class NouvelUtilisateurAndChangePasswordMailGenerator : MailBodyGenarator
    {

        public override string generateBody()
        {
            string body = this._data.TemplateMail.BodyTemplate;
            body = body.Replace("%prenom%", _data.Destinataire.Prenom);
            body = body.Replace("%nom%", _data.Destinataire.Nom);
            body = body.Replace("%login%", _data.Destinataire.Id);
            body = body.Replace("%password%", _data.Destinataire.MotDePasse);
            body = body.Replace("%ref_courrier%", Convert.ToString(_data.Courrier.Id));
            return body;
        }
    }
}
