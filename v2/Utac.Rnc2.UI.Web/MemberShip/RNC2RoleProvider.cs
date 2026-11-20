using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Fabriques;
using Utac.Rnc2.Infrastructure.Queries;

namespace Utac.Rnc2.UI.Web.MemberShip
{
    public class RNC2RoleProvider : RoleProvider
    {
        private string _applicationName;
        private readonly IrProfilService _profilService;
        private readonly IrUtilisateurService _utilisateurService;

        public RNC2RoleProvider()
        {
            this._profilService = NhibernateServiceFactory.getInstance().getrProfilService(false);
            this._utilisateurService = NhibernateServiceFactory.getInstance().getrUtilisateurService(false, true);
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
            set
            {
                this._applicationName = value;
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("Id", username, CriteriaOperator.EQUALS));

            rUtilisateur u = this._utilisateurService.FindBy(q).First<rUtilisateur>();

            List<string> roles = new List<string>();
            if (u != null)
            {
                if (u.RProfil2 != null)
                {
                    //profil détaillé
                    roles.Add(u.RProfil2.LibelleProfil);
                    //profil général
                    roles.Add(u.RProfil2.RProfil.LibelleProfil);
                }
                else
                {
                    if (u.RProfil != null)
                    {
                        //profil général
                        roles.Add(u.RProfil.LibelleProfil);
                    }
                }
            }

            return roles.ToArray();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}