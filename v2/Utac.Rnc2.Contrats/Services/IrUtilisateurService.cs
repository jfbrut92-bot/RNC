using System;
using System.Collections.Generic;
using System.Web.Security;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.SharedModel;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Search;

namespace Utac.Rnc2.Contrats.Services
{
    public interface IrUtilisateurService : IService<rUtilisateur>
    {
        List<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject search);
        List<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject search, int page, int pageSize, out long count);

        /* Pour la gestion d'un MemberShipProvider */
        MembershipUser CreateUser(string username, string password, string email);
        string GetUserNameByEmail(string email);
        MembershipUser GetUser(string username);
        bool ValidateUser(string username, string password, int maxInvalidPasswordAttempts);
        bool ValidateTempUser(string username, string password, int maxInvalidPasswordAttempts);

        /* pour la gestion des inscriptions des centres indépendants */
        void updateIdentite(object id, string nom, string prenom, string codUserMaj);
        void updateEmail(object id, string email, string codUserMaj);
        void updateTelephone(object id, string telephone, string codUserMaj);
        void updateMotPasse(object id, string motPasse, string codUserMaj);
        void setActif(object id, string codUserMaj);

        /* gestion du filtre de recherche */
        IList<rUtilisateur> findByFilter(UtilisateurFiltre filtre);

        /* Mise a jour des informations de l'Utilisateur */
        void updateUser(object id, rUtilisateur userMaj, string codUserMaj);

        rUtilisateur addUser(AjouterUtilisateurModel newUserModel, string codUserMaj);

        void supprimeAppartenance(string codeUtilisateur, string codeReseau, string codUserMaj);
        void ajouteAppartenance(string codeUtilisateur, string codeReseau, string codUserMaj);

        //EG le 10/08/2011
        bool estUtilisateurReseauRattachementCentre(rUtilisateur utilisateur, pCentreEncour centre);
        bool estUtilisateurReseauRattachementCentre(string codeUtilisateur, pCentreEncour centre);
        bool estUtilisateurReseauRattachementCentrePL(string codeUtilisateur, pCentreEncour centre);
        bool estUtilisateurReseauRattachementCentrePL(rUtilisateur utilisateur, pCentreEncour centre);

        bool estUtilisateurRepresentantLegalCentrePL(string codeUtilisateur, pCentreEncour centre);
        bool estUtilisateurRepresentantLegalCentrePL(rUtilisateur utilisateur, pCentreEncour centre);

        bool estUtilisateurRepresentantLegalCentreVL(string codeUtilisateur, pCentreEncour centre);
        bool estUtilisateurRepresentantLegalCentreVL(rUtilisateur utilisateur, pCentreEncour centre);

        bool estUtilisateurRepresentantLegalCentreCL(string codeUtilisateur, pCentreEncour centre);
        bool estUtilisateurRepresentantLegalCentreCL(rUtilisateur utilisateur, pCentreEncour centre);

        rUtilisateur CreateUserTemporaire(string username, string password, rProfil profil, rProfil2 profil2, string codeUtilisateur, rCategorieVehicule.CategorieEnum categVehicule, pCentreEncour centre);

        void supprimerUtilisateur(string id);
        void supprimerUtilisateur(rUtilisateur u);

        IEnumerable<rUtilisateur> FindAllTemporaire();

        IList<rUtilisateur> FindAllUser(rPrefecture rPrefecture);

        IList<rUtilisateur> FindAllUserRegionProfil(rRegion region, rProfil profil1, rProfil2 profil2);

        rUtilisateur ModifierParametreFacturationUtilisateur(rUtilisateur utilisateur, bool facturePapier, string adresseMail, string codeUtilisateur);
        IList<rUtilisateur> GetUserByCentre(pCentreEncour Centre, rCategorieVehicule.CategorieEnum categVehicule);

        /// <summary>
        /// Synchronise le user dans la base réquisition
        /// 24/10/2017 synchronisation des agents du PIV dans l'application réquisition
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Boolean SynchroniseUserWithDatabaseRequisition(rUtilisateur utilisateur);

        /// <summary>
        /// Synchronise une liste de users dans la base réquisition selon un profil
        /// 24/10/2017 synchronisation des agents du PIV dans l'application réquisition
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Boolean SynchroniseAllUsersWithDatabaseRequisition(rUtilisateurSearchObject search);
    }
}
