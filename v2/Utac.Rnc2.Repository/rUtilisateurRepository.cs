using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web.Security;
using log4net;
using NHibernate.Criterion;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Infrastructure.Search;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{

    public class rUtilisateurRepository : BaseRepository<rUtilisateur>, IrUtilisateurRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(rUtilisateurRepository));

        private NHibernate.ICriteria createCriteria(rUtilisateurSearchObject search)
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(rUtilisateur));
            if (search.IdRegion != null)
                CriteriaQuery.Add(Expression.Eq("RRegion.Id", search.IdRegion));

            if (search.IdProfil != null)
                CriteriaQuery.Add(Expression.Eq("RProfil.Id", search.IdProfil));

            if (search.Nom != null)
                CriteriaQuery.Add(Expression.Like("this.Nom", search.Nom + "%"));

            if (search.IdProfil2 != null)
                CriteriaQuery.Add(Expression.Eq("RProfil2.Id", search.IdProfil2));

            //On trie par nom de l'utilisateur
            Order order = new Order("this.Nom", true);
            CriteriaQuery.AddOrder(order);

            return CriteriaQuery;
        }

        public IList<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject search)
        {
            NHibernate.ICriteria CriteriaQuery = this.createCriteria(search);
            return CriteriaQuery.List<rUtilisateur>() as List<rUtilisateur>;
        }

        public IList<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject search, int page, int pageSize, out long count)
        {
            try
            {

                NHibernate.ICriteria CriteriaQuery = this.createCriteria(search);
                IList<rUtilisateur> utilisateurs = CriteriaQuery.SetFirstResult((page - 1) * pageSize).SetMaxResults(pageSize).List<rUtilisateur>();

                //Deuxieme requete
                CriteriaQuery = this.createCriteria(search);
                CriteriaQuery.ClearOrders();
                IList resultCount = CriteriaQuery.SetProjection(Projections.RowCountInt64()).List();

                count = (long)resultCount[0];

                return utilisateurs;

            }
            catch (Exception ex) { throw new Exception("GetPagedData Customer da hata", ex); }
        }

        public MembershipUser CreateUser(string username, string password, string email)
        {
            rUtilisateur user = new rUtilisateur();

            user.Id = username;
            user.Email = email;
            user.MotDePasse = password;
            //user.PasswordSalt = CreateSalt(); //Pour sécuriser les mots de passe
            user.CodUsrCrea = Convert.ToString(DateTime.Now);
            user.Actif = false;
            user.Bloque = false;
            //user.LastLockedOutDate = DateTime.Now;
            user.DateDerniereConnection = DateTime.Now;
            /* Début id_profil est obligatoire */
            user.RProfil = new rProfil();
            user.RProfil.Id = 1;
            /* Fin id_profil est obligatoire */
            this.Save(user);

            return this.GetUser(username);

        }

        #region A utiliser pour renforcer sécurité des mots de passe

        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }

        private static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                    saltAndPwd, System.Web.Configuration.FormsAuthPasswordFormat.SHA1.ToString());
            return hashedPwd;
        }

        #endregion A utiliser pour renforcer sécurité des mots de passe

        public string GetUserNameByEmail(string email)
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(rUtilisateur));
            CriteriaQuery.Add(Expression.Eq("this.Email", email));
            rUtilisateur u = CriteriaQuery.UniqueResult<rUtilisateur>();
            if (u != null)
                return u.Id;
            else
                return ""; //C'est le résultat attendu par le memberShipProvider, surtout pas null !
        }

        public System.Web.Security.MembershipUser GetUser(string username)
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(rUtilisateur));
            CriteriaQuery.Add(Expression.Eq("this.Id", username));
            rUtilisateur u = CriteriaQuery.UniqueResult<rUtilisateur>();
            if (u != null)
            {
                string _username = u.Id;
                string _providerUserKey = u.Id;
                string _email = u.Email;
                string _passwordQuestion = "";
                string _comment = null;
                bool _isApproved = u.Actif;
                bool _isLockedOut = u.Bloque;
                DateTime _creationDate = DateTime.MinValue;
                try
                {
                    _creationDate = Convert.ToDateTime(u.DateCrea);
                }
                catch (Exception e)
                { }

                DateTime _lastLoginDate = Convert.ToDateTime(u.DateDerniereConnection);
                DateTime _lastActivityDate = DateTime.Now;
                DateTime _lastPasswordChangedDate = DateTime.Now;
                DateTime _lastLockedOutDate = default(DateTime);

                MembershipUser user = new MembershipUser("RNC2MembershipProvider",
                                                          _username,
                                                          _providerUserKey,
                                                          _email,
                                                          _passwordQuestion,
                                                          _comment,
                                                          _isApproved,
                                                          _isLockedOut,
                                                          _creationDate,
                                                          _lastLoginDate,
                                                          _lastActivityDate,
                                                          _lastPasswordChangedDate,
                                                          _lastLockedOutDate);

                return user;
            }
            else
            {
                return null;
            }
        }

        public bool ValidateUser(string username, string password, int maxInvalidPasswordAttempts)
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(rUtilisateur));
            CriteriaQuery.Add(Expression.Eq("this.Id", username));
            //On ne veut que les utilisateurs actifs
            CriteriaQuery.Add(Expression.Eq("this.Actif", true));
            rUtilisateur u = CriteriaQuery.UniqueResult<rUtilisateur>();
            bool valide = true;
            if (u != null)
            {
                if (password.Equals(u.MotDePasse))
                {
                    //on met à jour la date de connexion
                    u.DateDerniereConnection = System.DateTime.Now;
                    u.NumTentative = 0;

                    //EG le 12/04/2011 pour gestion des logins temporaires des CNR
                    u.MotDePasseTemporaire = null;
                    /* si l'on veut déplacer l'activation  du centre ici plutot que lors de l'inscription
                    if (u.MotDePasseTemporaire != null)
                    {
                        //EG le 12/04/2011 pour gestion des logins temporaires des CNR
                        u.MotDePasseTemporaire = null;
                        pCentreEncour centre = u.MonCentre;
                        if (centre != null)
                        {
                            if (u.MonCentreEstVL)
                            {
                                centre.AccesPublicVl = true;
                            }
                            else
                            {
                                if (u.MonCentreEstPL)
                                {
                                    centre.AccesPublicPl = true;
                                }
                                else
                                {
                                    centre.AccesPublicCl = true;
                                }
                            }

                        }
                    }*/

                    valide = true;
                }
                else
                {
                    //On incrémente le nombre de tentative
                    u.NumTentative++;
                    if (u.NumTentative > maxInvalidPasswordAttempts)
                    {
                        u.Bloque = true;
                    }

                    valide = false;
                }
                //On sauvegarde l'utilisateur
                this.Save(u);
            }
            else
            {
                valide = false;
            }

            return valide;
        }

        public bool ValidateTempUser(string username, string password, int maxInvalidPasswordAttempts)
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(rUtilisateur));
            CriteriaQuery.Add(Expression.Eq("this.Id", username));
            //On ne veut que les utilisateurs actifs -> EG le 09/11/2011 NON pas pour les logins temporaires
            //CriteriaQuery.Add(Expression.Eq("this.Actif", true));
            rUtilisateur u = CriteriaQuery.UniqueResult<rUtilisateur>();
            bool valide = true;
            if (u != null)
            {
                if (password.Equals(u.MotDePasseTemporaire))
                {
                    //on met à jour la date de connexion
                    u.DateDerniereConnection = System.DateTime.Now;
                    u.NumTentative = 0;

                    valide = true;
                }
                else
                {
                    //On incrémente le nombre de tentative
                    u.NumTentative++;
                    if (u.NumTentative > maxInvalidPasswordAttempts)
                    {
                        u.Bloque = true;
                    }

                    valide = false;
                }
                //On sauvegarde l'utilisateur
                this.Save(u);
            }
            else
            {
                valide = false;
            }

            return valide;
        }

        /// <summary>
        /// Synchronise le user dans la base réquisition
        /// 24/10/2017 synchronisation des agents du PIV dans l'application réquisition
        /// </summary>
        /// <param name="username"></param>
        /// <returns>true si OK</returns>
        public Boolean SynchroniseUserWithDatabaseRequisition(rUtilisateur u)
        {
            /*base requis_prod*/
            /*Synchronisation des utilisateurs depuis la base RNC2_NET*/
            /*Version 1.0 -PLE- du 23/10/2017*/

            log.Info("Synchronisation du user " + u.Id + " dans réquisition on-line ... Début");
            object result;
            NHibernate.ISQLQuery procedureQuery = SessionFactory.GetCurrentSession().CreateSQLQuery("exec requis_prod..usp_synchro_utilisateur_rnc2 :cod_utilisateur,:id_profil,:id_profil2,:mot_de_passe,:nom,:prenom,:actif,:bloque,:num_tentative,:date_derniere_connection,:num_tel,:email,:cod_usr_crea,:cod_usr_maj,:date_crea,:date_maj");
            procedureQuery.SetParameter("cod_utilisateur", u.Id);
            procedureQuery.SetParameter("id_profil", u.RProfil.Id);
            procedureQuery.SetParameter("id_profil2", u.RProfil2.Id);
            procedureQuery.SetParameter("mot_de_passe", u.MotDePasse);
            procedureQuery.SetParameter("nom", u.Nom);
            procedureQuery.SetParameter("prenom", u.Prenom);
            procedureQuery.SetParameter("actif", u.Actif);
            procedureQuery.SetParameter("bloque", u.Bloque);
            procedureQuery.SetParameter("num_tentative", u.NumTentative);
            procedureQuery.SetParameter("date_derniere_connection", u.DateDerniereConnection);
            procedureQuery.SetParameter("num_tel", u.NumTel);
            procedureQuery.SetParameter("email", u.Email);
            procedureQuery.SetParameter("cod_usr_crea", u.CodUsrCrea);
            procedureQuery.SetParameter("cod_usr_maj", u.CodUsrMaj);
            procedureQuery.SetParameter("date_crea", u.DateCrea);
            procedureQuery.SetParameter("date_maj", u.DateMaj);

            try
            {
                result = procedureQuery.UniqueResult();
                // retour de la procédure stockée--> 1:erreur, 2:mise à jour, 3:création
                if (result.ToString().Equals("1"))
                {
                    log.Error("Erreur retrounée par le procédure stokée usp_synchro_utilisateur_rnc2");
                }
                else
                {
                    log.Info("Mise à jour du user " + u.Id + " : Résultat = " + result.ToString());
                }
            }
            catch (NHibernate.ADOException e)
            {
                log.Error("Erreur lors de l'exécution de la procédure stockée usp_synchro_utilisateur_rnc2", e);
                return false;
            }
            log.Info("Synchronisation du user " + u.Id + " dans réquisition on-line ... Fin");
            return (!result.ToString().Equals("1"));
        }

        /// <summary>
        /// Synchronise de tous les users d'un profil dans la base réquisition
        /// 24/10/2017 synchronisation des agents du PIV dans l'application réquisition
        /// </summary>
        /// <param name="username"></param>
        /// <returns>true si OK</returns>
        public Boolean SynchroniseAllUsersWithDatabaseRequisition(rUtilisateurSearchObject search)
        {
            Boolean result = true;
            log.Info("Synchronisation d'une liste de profil dans réquisition on-line ... Début");
            if (search != null && search.IdProfil != null)
            {
                log.Info("Filtre profil_id= " + search.IdProfil.Value);
            }
            else
            {
                log.Error("Filtre profil_id= vide");
            }

            IList<rUtilisateur> usersList = this.LoadUtilisateurBy(search);

            foreach (rUtilisateur user in usersList)
            {
                result = result && this.SynchroniseUserWithDatabaseRequisition(user);
            }
            return result;

        }
    }
}
