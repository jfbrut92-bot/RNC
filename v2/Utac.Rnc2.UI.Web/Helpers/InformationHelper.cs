using System;
using System.Web;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.UI.Web.Models;

namespace Utac.Rnc2.UI.Web.Helpers
{
    public class InformationHelper
    {
        public static InformationPersonelle getInformationPersonnelle(bool force)
        {
            InformationPersonelle info = (InformationPersonelle)HttpContext.Current.Session["info_personnelle"];
            if (force || info == null)
            {
                //On récupère les informations de l'utilisateur
                IrUtilisateurService service = NhibernateServiceFactory.getInstance().getrUtilisateurService(false, true);
                object id = HttpContext.Current.User.Identity.Name;
                rUtilisateur u = service.FindBy(id);

                info = new InformationPersonelle();

                info.Id = id;
                info.DateDerniereConnexion = Convert.ToDateTime(u.DateDerniereConnection);
                info.Email = u.Email;
                info.Nom = u.Nom;
                info.Prenom = u.Prenom;
                info.Profil = u.RProfil2.LibelleProfil;
                info.Telephone = u.NumTel;

                //EG le 17/10/2011
                info.Utilisateur = u;

                HttpContext.Current.Session["info_personnelle"] = info;
            }

            return info;
        }

        public static void initInformationPersonnelle()
        {
            HttpContext.Current.Session["info_personnelle"] = null;
        }
    }
}