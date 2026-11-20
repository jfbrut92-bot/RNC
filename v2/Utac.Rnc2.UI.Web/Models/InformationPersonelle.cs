using System;
using Utac.Rnc2.BusinessObjects;

namespace Utac.Rnc2.UI.Web.Models
{
    public class InformationPersonelle
    {
        private object id;

        public object Id
        {
            get { return id; }
            set { id = value; }
        }
        private string nom;
        private string telephone;

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
        private string profil;

        public string Profil
        {
            get { return profil; }
            set { profil = value; }
        }
        private string prenom;

        public string Prenom
        {
            get { return prenom; }
            set { prenom = value; }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private DateTime dateDerniereConnexion;

        public DateTime DateDerniereConnexion
        {
            get { return dateDerniereConnexion; }
            set { dateDerniereConnexion = value; }
        }

        public string Nom
        {
            get { return nom; }
            set { nom = value; }
        }

        public rUtilisateur Utilisateur { get; set; }

    }
}