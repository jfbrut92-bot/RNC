using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using log4net;

namespace Utac.Rnc2.MVC.Extensions.mail
{
    public class Mailer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Mailer));

        public static bool Send(string objet, string contenu, string email)
        {
            try
            {
                string smtpHost = ConfigurationManager.AppSettings["smtp_host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtp_port"]);
                string authUser = ConfigurationManager.AppSettings["smtp_auth_user"];
                string authPassword = ConfigurationManager.AppSettings["smtp_auth_pwd"];
                string expediteur = ConfigurationManager.AppSettings["from"];
                string bcc = ConfigurationManager.AppSettings["bcc"];
                //EG le 18/01/2012 : pour gérer la possibilité d'obliger ou non la fourniture d'information de connexion
                bool mustAuth = Convert.ToBoolean(ConfigurationManager.AppSettings["smtp_must_auth"]);

                SmtpClient client = new SmtpClient(smtpHost, port);
                //EG le 18/01/2012 : pour gérer la possibilité d'obliger ou non la fourniture d'information de connexion
                if (mustAuth)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(authUser, authPassword);
                }

                MailAddress from = new MailAddress(expediteur);
                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // Specify the message content.
                MailMessage message = new MailMessage(from, to);
                message.Subject = objet;
                message.Body = contenu;

                //EG le 23/11/2011
                message.Bcc.Add(new MailAddress(bcc));

                //Envoi du message
                client.Send(message);
                return true;
            }
            catch (Exception e)
            {
                string stackTrace = e.StackTrace != null ? e.StackTrace.ToString() : "";
                log.Error(
                       string.Format("ERREUR dans Send(), MESSAGE = {0}\r\nINNER EXCEPTION = {1}\r\nSTACK = {2}", e.Message, e.InnerException, stackTrace)
                    );
                return false;
            }

        }

        public static bool SendHtml(string objet, string contenu, string email, string logoPath)
        {
            try
            {
                string smtpHost = ConfigurationManager.AppSettings["smtp_host"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtp_port"]);
                string authUser = ConfigurationManager.AppSettings["smtp_auth_user"];
                string authPassword = ConfigurationManager.AppSettings["smtp_auth_pwd"];
                string expediteur = ConfigurationManager.AppSettings["from"];
                string bcc = ConfigurationManager.AppSettings["bcc"];
                //EG le 18/01/2012 : pour gérer la possibilité d'obliger ou non la fourniture d'information de connexion
                bool mustAuth = Convert.ToBoolean(ConfigurationManager.AppSettings["smtp_must_auth"]);

                SmtpClient client = new SmtpClient(smtpHost, port);
                //EG le 18/01/2012 : pour gérer la possibilité d'obliger ou non la fourniture d'information de connexion
                if (mustAuth)
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(authUser, authPassword);
                }

                MailAddress from = new MailAddress(expediteur);
                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // Specify the message content.
                MailMessage message = new MailMessage(from, to);
                message.Subject = objet;
                LinkedResource logo = new LinkedResource(logoPath);
                logo.ContentId = "companylogo";
                // done HTML formatting in the next line to display my logo
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(contenu, null, MediaTypeNames.Text.Html);
                av1.LinkedResources.Add(logo);

                message.AlternateViews.Add(av1);
                //EG le 10/01/2012
                message.IsBodyHtml = true;
                //EG le 23/11/2011
                message.Bcc.Add(new MailAddress(bcc));

                //Envoi du message
                client.Send(message);
                return true;
            }
            catch (Exception e)
            {
                string stackTrace = e.StackTrace != null ? e.StackTrace.ToString() : "";
                log.Error(
                       string.Format("ERREUR dans SendHtml(), MESSAGE = {0}\r\nINNER EXCEPTION = {1}\r\nSTACK = {2}", e.Message, e.InnerException, stackTrace)
                    );
                return false;
            }

        }

    }
}
