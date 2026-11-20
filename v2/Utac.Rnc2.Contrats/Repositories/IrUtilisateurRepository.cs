using System;
using System.Collections.Generic;
using System.Web.Security;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.Contrats.Repositories
{

    public interface IrUtilisateurRepository : IRepository<rUtilisateur>
    {
        IList<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject o);
        IList<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject search, int page, int pageSize, out long count);

        /* Pour la gestion d'un MemberShipProvider */
        MembershipUser CreateUser(string username, string password, string email);
        string GetUserNameByEmail(string email);
        MembershipUser GetUser(string username);
        bool ValidateUser(string username, string password, int maxInvalidPasswordAttempts);
        bool ValidateTempUser(string username, string password, int maxInvalidPasswordAttempts);

        /// <summary>
        /// Synchronise le user dans la base réquisition
        /// 24/10/2017 synchronisation des agents du PIV dans l'application réquisition
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool SynchroniseUserWithDatabaseRequisition(rUtilisateur utilisateur);
        /// <summary>
        /// Synchronise une liste de user dans la base réquisition
        /// 24/10/2017 synchronisation des agents du PIV dans l'application réquisition
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Boolean SynchroniseAllUsersWithDatabaseRequisition(rUtilisateurSearchObject search);
    }
}
