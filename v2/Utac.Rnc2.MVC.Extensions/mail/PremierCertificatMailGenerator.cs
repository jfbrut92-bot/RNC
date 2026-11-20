using System;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.MVC.Extensions.mail
{
    public class PremierCertificatMailGenerator : MailBodyGenarator
    {

        public override string generateBody()
        {
            string body = this._data.TemplateMail.BodyTemplate;
            body = body.Replace("%prenom%", _data.Destinataire.Prenom);
            body = body.Replace("%nom%", _data.Destinataire.Nom);
            body = body.Replace("%login%", _data.Destinataire.Id);
            string entite = string.Empty;
            pGestionCertificat gestionCertificat = _data.Certificat.PGestionCertificats[0];
            if (gestionCertificat.PCentreEncourVL != null)
            {
                entite = "Centre de Contrôle Technique Véhicules Légers non rattaché : " + gestionCertificat.PCentreEncourVL.CodAgremVl;
            }

            if (gestionCertificat.PCentreEncourPL != null)
            {
                entite = "Centre de Contrôle Technique Véhicules Lourds non rattaché : " + gestionCertificat.PCentreEncourPL.CodAgremPl;
            }

            if (gestionCertificat.PCentreEncourCL != null)
            {
                entite = "Centre de Contrôle Technique Véhicules de catégorie L non rattaché : " + gestionCertificat.PCentreEncourCL.CodAgremCl;
            }

            if (gestionCertificat.RReseau != null)
            {
                if (gestionCertificat.RReseau.TypeReseauVl)
                    entite = "Réseau de contrôle technique Véhicules Légers : " + gestionCertificat.RReseau.ReseauAvecLettreEtCategorie;
                else
                    if (gestionCertificat.RReseau.TypeReseauPl)
                    entite = "Réseau de contrôle technique Véhicules Lourds : " + gestionCertificat.RReseau.ReseauAvecLettreEtCategorie;
                else
                        if (gestionCertificat.RReseau.TypeReseauCl)
                    entite = "Réseau de contrôle technique Véhicules de catégorie L : " + gestionCertificat.RReseau.ReseauAvecLettreEtCategorie;
            }

            body = body.Replace("%as_entite%", entite);
            body = body.Replace("%as_nom_fichier_zip%", _data.Certificat.NomFichier);
            body = body.Replace("%as_password_certificat%", _data.Certificat.PasswordCert);
            body = body.Replace("%as_login_connect%", _data.Certificat.BddTechLogin);
            body = body.Replace("%as_password_connect%", _data.Certificat.BddTechPassword);

            body = body.Replace("%password%", _data.Destinataire.MotDePasse);
            body = body.Replace("%mail_dest%", _data.Destinataire.Email);
            body = body.Replace("%telephone%", _data.Destinataire.NumTel);
            body = body.Replace("%ref_courrier%", Convert.ToString(_data.Courrier.Id));
            return body;
        }
    }
}
