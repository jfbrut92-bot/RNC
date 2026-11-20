using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.SharedModel;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Contrats.Services;
using Utac.Rnc2.Infrastructure;
using Utac.Rnc2.Infrastructure.Queries;
using Utac.Rnc2.Infrastructure.Search;
using Utac.Rnc2.Infrastructure.Utilitaires;

namespace Utac.Rnc2.Services
{
    public class rUtilisateurService : BaseService<rUtilisateur>, IrUtilisateurService
    {
        private readonly ILog log = LogManager.GetLogger(typeof(rUtilisateurService));
        private readonly IrUtilisateurRepository _utilisateurRepository;
        private readonly IrPrefectureService _prefectureService;
        private readonly IpCentreEncourService _pCentreEncourService;
        private readonly IrReseauAppartenanceService _reseauAppartenanceService;

        public rUtilisateurService(IrUtilisateurRepository utilisateurRepository, IUnitOfWork UnitOfWork, IrPrefectureService prefService, IpCentreEncourService pCentreEncourService, IrReseauAppartenanceService reseauAppartenanceService)
            : base(utilisateurRepository, UnitOfWork)
        {
            this._utilisateurRepository = utilisateurRepository;
            this._prefectureService = prefService;
            this._pCentreEncourService = pCentreEncourService;
            this._reseauAppartenanceService = reseauAppartenanceService;
        }

        public List<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject search)
        {
            return (List<rUtilisateur>)_utilisateurRepository.LoadUtilisateurBy(search);
        }

        public List<rUtilisateur> LoadUtilisateurBy(rUtilisateurSearchObject search, int page, int pageSize, out long count)
        {
            return (List<rUtilisateur>)_utilisateurRepository.LoadUtilisateurBy(search, page, pageSize, out count);
        }

        private string BuildErrorListFrom(rUtilisateur utilisateur)
        {

            StringBuilder brokenRulesList = new StringBuilder();

            foreach (BrokenRule brokenRule in utilisateur.GetBrokenRules())
            {
                brokenRulesList.Append(brokenRule.Rule);
                brokenRulesList.Append(Environment.NewLine);
            }

            return brokenRulesList.ToString();
        }

        /* Pour la gestion des MemberShip Provider */
        public System.Web.Security.MembershipUser CreateUser(string username, string password, string email)
        {
            return this._utilisateurRepository.CreateUser(username, password, email);
        }

        //public rUtilisateur CreateUserTemporaire(string username, string password, rProfil profil, rProfil2 profil2, string codeUtilisateur)
        //{
        //    return this._utilisateurRepository.CreateUserTemporaire(username, password, profil, profil2, codeUtilisateur);
        //}

        public string GetUserNameByEmail(string email)
        {
            return this._utilisateurRepository.GetUserNameByEmail(email);
        }

        public System.Web.Security.MembershipUser GetUser(string username)
        {
            return this._utilisateurRepository.GetUser(username);
        }

        public bool ValidateUser(string username, string password, int maxInvalidPasswordAttempts)
        {
            return this._utilisateurRepository.ValidateUser(username, password, maxInvalidPasswordAttempts);
        }

        public bool ValidateTempUser(string username, string password, int maxInvalidPasswordAttempts)
        {
            return this._utilisateurRepository.ValidateTempUser(username, password, maxInvalidPasswordAttempts);
        }

        public void updateIdentite(object id, string nom, string prenom, string codUserMaj)
        {
            //1 on récupère le user
            rUtilisateur u = this.FindBy(id);

            //On met a jour
            u.Nom = nom;
            u.Prenom = prenom;
            u.DateMaj = System.DateTime.Now;
            u.CodUsrMaj = codUserMaj;

            //On sauvegarde
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();

        }

        public void updateEmail(object id, string email, string codUserMaj)
        {
            //1 on récupère le user
            rUtilisateur u = this.FindBy(id);

            //On met a jour
            u.Email = email;
            u.DateMaj = System.DateTime.Now;
            u.CodUsrMaj = codUserMaj;

            //On sauvegarde
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();
        }

        public void updateTelephone(object id, string telephone, string codUserMaj)
        {
            //1 on récupère le user
            rUtilisateur u = this.FindBy(id);

            //On met a jour
            u.NumTel = telephone;
            u.DateMaj = System.DateTime.Now;
            u.CodUsrMaj = codUserMaj;

            //On sauvegarde
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();
        }

        public void updateMotPasse(object id, string motPasse, string codUserMaj)
        {
            //1 on récupère le user
            rUtilisateur u = this.FindBy(id);

            //On met a jour
            u.MotDePasse = motPasse;
            u.DateMaj = System.DateTime.Now;
            u.CodUsrMaj = codUserMaj;

            //On sauvegarde
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();
        }

        public IList<rUtilisateur> findByFilter(UtilisateurFiltre filtre)
        {
            Query q = new Query();

            /*
            if (!string.IsNullOrEmpty(filtre.CodeReseauCL))
            {
                q.Criteria.Add(new Criteria("RReseauAppartenances.CodReseau", filtre.CodeReseauCL, CriteriaOperator.EQUALS));
            }
            */

            if (filtre.ProfilId != -1)
            {
                q.Criteria.Add(new Criteria("RProfil.Id", filtre.ProfilId, CriteriaOperator.EQUALS));
            }

            if (!string.IsNullOrEmpty(filtre.CodeDepartement))
            {
                if (filtre.ProfilId == 4)
                {
                    string codeDepartementS = filtre.CodeDepartement.ToLower();
                    int codeDepartementI;
                    string val = string.Format("{0:000}", codeDepartementS);
                    if (Int32.TryParse(codeDepartementS, out codeDepartementI))
                    {
                        //ex: 001
                        //PLE le 15/02/2024 Les logins commence par s ou L
                        //val = string.Format("s{0:000}", codeDepartementI);
                        val = string.Format("{0:000}", codeDepartementI);

                    }
                    else
                    {
                        //ex: 02A
                        while (codeDepartementS.Length < 3)
                        {
                            codeDepartementS = "0" + codeDepartementS;
                        }
                        //PLE le 15/02/2024 Les logins commence par s ou L
                        //val = string.Format("s{0}", codeDepartementS);
                    }

                    q.Criteria.Add(new Criteria("Id", "_" + val + "%", CriteriaOperator.LIKE));

                }
                /*03/11/2011 PLE le crtière code département ne s'applique que pour le profil PROFECTURE et CENTRE INDEPENDANT*/
                if (filtre.ProfilId == 2)
                {
                    JoinCriteria jc = new JoinCriteria("RPrefecture", "pref");
                    jc.Criterias.Add(new Criteria("CodDepartement", filtre.CodeDepartement, CriteriaOperator.EQUALS));
                    q.JoinCriteria.Add(jc);
                }
            }

            if (filtre.RegionId != -1)
            {
                /*03/11/2011 PLE le crtière région ne s'applique que pour le profil ORGANISME DE SURVEILLANCE (DRIRE ...)*/
                if (filtre.ProfilId == 3)
                {
                    q.Criteria.Add(new Criteria("RRegion.Id", filtre.RegionId, CriteriaOperator.EQUALS));
                }
            }

            //if (!string.IsNullOrEmpty(filtre.CodeReseau) && !(filtre.ProfilId == 5 && !string.IsNullOrEmpty(filtre.CategorieVehicule )))
            if (!string.IsNullOrEmpty(filtre.CodeReseau))
            {
                /*07/11/2011 PLE le crtière code département ne s'applique que pour le profil RESEAU */
                if (filtre.ProfilId == 5)
                {
                    JoinCriteria jc = new JoinCriteria("RReseaux", "appartenance");
                    jc.Criterias.Add(new Criteria("Id", filtre.CodeReseau, CriteriaOperator.EQUALS));
                    q.JoinCriteria.Add(jc);
                }
            }

            // Pour les recherches des utilisateurs de profil REseau-Administrateur
            if (string.IsNullOrEmpty(filtre.CodeReseau) && filtre.CodeReseaux != null)
            {
                JoinCriteria jc = new JoinCriteria("RReseaux", "appartenance");
                jc.Criterias.Add(new Criteria("Id", filtre.CodeReseaux.ToArray()));
                q.JoinCriteria.Add(jc);
            }

            if (!string.IsNullOrEmpty(filtre.CategorieVehicule))
            {
                if (filtre.ProfilId == 5)
                {
                    //Profil Réseau

                    JoinCriteria jc = q.findJoinCriteriaByAlias("appartenance");
                    if (jc == null)
                    {
                        jc = new JoinCriteria("RReseaux", "appartenance");
                        q.JoinCriteria.Add(jc);
                    }

                    switch (filtre.CategorieVehicule)
                    {
                        case "VL": jc.Criterias.Add(new Criteria("TypeReseauVl", true, CriteriaOperator.EQUALS)); break;
                        case "PL": jc.Criterias.Add(new Criteria("TypeReseauPl", true, CriteriaOperator.EQUALS)); break;
                        case "CL": jc.Criterias.Add(new Criteria("TypeReseauCl", true, CriteriaOperator.EQUALS)); break;
                    }

                }
                else if (filtre.ProfilId == 4)
                {
                    //Centre indépendant
                    switch (filtre.CategorieVehicule)
                    {
                        case "VL": q.Criteria.Add(new Criteria("PCentreEncourVL", null, CriteriaOperator.IS_NOT_NULL)); break;
                        case "PL": q.Criteria.Add(new Criteria("PCentreEncourPL", null, CriteriaOperator.IS_NOT_NULL)); break;
                        case "CL": q.Criteria.Add(new Criteria("PCentreEncourCL", null, CriteriaOperator.IS_NOT_NULL)); break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(filtre.Nom))
            {
                q.Criteria.Add(new Criteria("Nom", filtre.Nom + "%", CriteriaOperator.LIKE));
            }

            if (filtre.LoginProvisoire)
            {
                q.Criteria.Add(new Criteria("MotDePasseTemporaire", null, CriteriaOperator.IS_NOT_NULL));
            }
            //EG le 02/12/2011 pour faciliter les recherches

            q.OrderBy.Add(new OrderBy() { PropertyName = "Nom", Ascending = true });
            q.OrderBy.Add(new OrderBy() { PropertyName = "Prenom", Ascending = true });

            if (!string.IsNullOrWhiteSpace(filtre.Identifiant))
            {
                string filtreIdentifiant;
                if (filtre.Identifiant.StartsWith('l'.ToString()) || filtre.Identifiant.StartsWith('L'.ToString()))
                    filtreIdentifiant = filtre.Identifiant.ToUpper();
                else
                    filtreIdentifiant = filtre.Identifiant.ToLower();
                q.Criteria.Add(new Criteria("Id", filtreIdentifiant + "%", CriteriaOperator.LIKE));
            }

            return this.FindBy(q);
        }

        public void updateUser(object id, rUtilisateur userMaj, string codUserMaj)
        {
            //1 on récupère le user
            rUtilisateur u = this.FindBy(id);

            //On met a jour les informations communes à tous les profils
            u.Nom = userMaj.Nom.ToUpper().Trim(); //2011-11-04 PLE LE nom de l'utilisateur est sauvegardé en majuscule.                        
            u.Prenom = userMaj.Prenom.Trim();
            u.Email = userMaj.Email;
            u.NumTel = userMaj.NumTel;
            u.Actif = userMaj.Actif;
            u.Bloque = userMaj.Bloque;
            if (userMaj.RProfil2 != null)
                u.RProfil2 = userMaj.RProfil2;
            if (userMaj.RRegion != null)
                u.RRegion = userMaj.RRegion;

            //Les infos systèmes
            u.CodUsrMaj = codUserMaj;
            u.DateMaj = System.DateTime.Now;

            //On sauvegarde
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();
        }

        public rUtilisateur addUser(AjouterUtilisateurModel newUserModel, string codUserMaj)
        {
            //Si le mot de passe est vide, on le génère
            //@todo paramétrage des caractères autorisés
            rUtilisateur newUser = newUserModel.Utilisateur;

            if (string.IsNullOrWhiteSpace(newUser.MotDePasse))
            {
                PasswordGenerator pGenerateur = PasswordGenerator.getInstance();
                newUser.MotDePasse = pGenerateur.GenerateForDesizedSize();
            }

            //EG le 03/01/2012 : mot de passe en minuscule
            newUser.MotDePasse = newUser.MotDePasse.ToLower();

            //cas des préfectures
            if (newUser.RProfil.Id == 2) //2011-11-07 PLE Le code département est rensigné uniquement pour le profil PREFECTURE
            {
                if (newUser.RPrefecture != null)
                {
                    if (string.IsNullOrEmpty(newUser.RPrefecture.CodDepartement))
                    {
                        //le newUser vient d'un formulaire web par exemple donc ...
                        newUser.RPrefecture = this._prefectureService.FindBy(newUser.RPrefecture.Id);
                    }
                    newUser.Id = newUser.RPrefecture.CodDepartement + newUser.Id;
                }
            }

            //cas des centres indépendants
            if (newUser.PCentreEncourVL != null)
            {
                newUser.PCentreEncourVL = this._pCentreEncourService.findByCodeAgrementVL(newUser.PCentreEncourVL.CodAgremVl);
            }

            if (newUser.PCentreEncourPL != null)
            {
                newUser.PCentreEncourPL = this._pCentreEncourService.findByCodeAgrementPL(newUser.PCentreEncourPL.CodAgremPl);
            }

            if (newUser.PCentreEncourCL != null)
            {
                newUser.PCentreEncourCL = this._pCentreEncourService.findByCodeAgrementCL(newUser.PCentreEncourCL.CodAgremCl);
            }

            //Les infos système
            newUser.DateCrea = System.DateTime.Now;
            newUser.CodUsrCrea = codUserMaj;

            //2011-11-04 PLE LE nom de l'utilisateur est sauvegardé en majuscule et sans espace devant et derrière.
            newUser.Nom = newUser.Nom.ToUpper().Trim();
            //2011-11-07 PLE Le Prénom de l'utilisateur est sauvegardé  sans espace devant et derrière.
            newUser.Prenom = newUser.Prenom.Trim();

            //On sauvegarde
            this.Save(newUser);

            //Cas des RESEAUX
            if (newUser.RProfil.Id == 5)
            {
                //Sauvegarde des reseaux d'appartenance
                //Mettre dans le service ?
                if (newUserModel.ReseauxAppartenance != null)
                {
                    foreach (string codReseau in newUserModel.ReseauxAppartenance)
                    {
                        rReseauAppartenance r = new rReseauAppartenance() { CodReseau = codReseau, CodUtilisateur = newUser.Id };
                        this._reseauAppartenanceService.Save(r);
                    }
                }

            }

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();

            return newUser;
        }

        public void supprimeAppartenance(string codeUtilisateur, string codeReseau, string codUserMaj)
        {
            this._reseauAppartenanceService.supprimeAppartenance(codeUtilisateur, codeReseau);
            //1 on récupère le user
            rUtilisateur u = this.FindBy(codeUtilisateur);

            //Les infos systèmes
            u.CodUsrMaj = codUserMaj;
            u.DateMaj = System.DateTime.Now;
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();
        }

        public void ajouteAppartenance(string codeUtilisateur, string codeReseau, string codUserMaj)
        {
            this._reseauAppartenanceService.ajouteAppartenance(codeUtilisateur, codeReseau);
            //1 on récupère le user
            rUtilisateur u = this.FindBy(codeUtilisateur);

            //Les infos systèmes
            u.CodUsrMaj = codUserMaj;
            u.DateMaj = System.DateTime.Now;
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();
        }

        /*
        public override void Remove(rUtilisateur utilisateur)
        {
            //On delete d'abord les réseaux d'appartenance s'ils existent
            
            foreach (rReseauAppartenance r in utilisateur.RReseauAppartenances)
            {
                this._reseauAppartenanceService.Remove(r);

            }
            base.Remove(utilisateur);

            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.Commit();
            }

        } 
         */

        public bool estUtilisateurReseauRattachementCentre(rUtilisateur utilisateur, pCentreEncour centre)
        {
            // TODO: a modifier quand un centre aura plusieurs réseau d'appartenance centre.RReseaux
            if (centre.ReseauRattachementVL != null)
            {
                if (utilisateur.RReseaux.Contains(centre.ReseauRattachementVL))
                {
                    return true;
                }
            }

            if (centre.ReseauRattachementPL != null)
            {
                if (utilisateur.RReseaux.Contains(centre.ReseauRattachementPL))
                {
                    return true;
                }
            }

            if (centre.ReseauRattachementCL != null)
            {
                if (utilisateur.RReseaux.Contains(centre.ReseauRattachementCL))
                {
                    return true;
                }
            }

            return false;
        }

        public bool estUtilisateurReseauRattachementCentre(string codeUtilisateur, pCentreEncour centre)
        {
            rUtilisateur u = this.FindBy(codeUtilisateur);
            return this.estUtilisateurReseauRattachementCentre(u, centre);
        }

        public bool estUtilisateurReseauRattachementCentrePL(rUtilisateur utilisateur, pCentreEncour centre)
        {
            if (centre.CodAgremPl != null)
            {
                if (utilisateur.RReseaux.Contains(centre.ReseauRattachementPL))
                {
                    return true;
                }
            }

            return false;
        }

        public bool estUtilisateurReseauRattachementCentrePL(string codeUtilisateur, pCentreEncour centre)
        {
            rUtilisateur u = this.FindBy(codeUtilisateur);
            return this.estUtilisateurReseauRattachementCentrePL(u, centre);
        }

        public bool estUtilisateurRepresentantLegalCentrePL(string codeUtilisateur, pCentreEncour centre)
        {
            rUtilisateur u = this.FindBy(codeUtilisateur);
            return this.estUtilisateurRepresentantLegalCentrePL(u, centre);
        }

        public bool estUtilisateurRepresentantLegalCentrePL(rUtilisateur utilisateur, pCentreEncour centre)
        {
            /*
            r_utilisateur.id_profil=4  et 
            r_utilisateur.id_centre_pl = param_id_centre_encours et
            p_centre_en_cours.cod_reseau_pl = « code réseau PL indépendant  (Z) »
            */
            if (utilisateur.RProfil.Id == 4)
            {
                if (centre.CodAgremPl != null)
                {
                    if (utilisateur.PCentreEncourPL != null)
                    {
                        if (utilisateur.PCentreEncourPL.Equals(centre))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool estUtilisateurRepresentantLegalCentreVL(string codeUtilisateur, pCentreEncour centre)
        {
            rUtilisateur u = this.FindBy(codeUtilisateur);
            return this.estUtilisateurRepresentantLegalCentreVL(u, centre);
        }

        public bool estUtilisateurRepresentantLegalCentreVL(rUtilisateur utilisateur, pCentreEncour centre)
        {
            if (utilisateur.RProfil.Id == 4)
            {
                if (centre.CodAgremVl != null)
                {
                    if (utilisateur.PCentreEncourVL != null)
                    {
                        if (utilisateur.PCentreEncourVL.Equals(centre))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool estUtilisateurRepresentantLegalCentreCL(string codeUtilisateur, pCentreEncour centre)
        {
            rUtilisateur u = this.FindBy(codeUtilisateur);
            return this.estUtilisateurRepresentantLegalCentreCL(u, centre);
        }

        public bool estUtilisateurRepresentantLegalCentreCL(rUtilisateur utilisateur, pCentreEncour centre)
        {
            if (utilisateur.RProfil.Id == 4)
            {
                if (centre.CodAgremCl != null)
                {
                    if (utilisateur.PCentreEncourCL != null)
                    {
                        if (utilisateur.PCentreEncourCL.Equals(centre))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public void setActif(object id, string codUserMaj)
        {
            //1 on récupère le user
            rUtilisateur u = this.FindBy(id);

            //On met a jour
            u.Actif = true;
            u.DateMaj = System.DateTime.Now;
            u.CodUsrMaj = codUserMaj;

            //EG le 27/01/2012 : on rend son centre visible
            pCentreEncour centre = u.MonCentre;
            if (centre != null)
            {
                if (u.MonCentreEstVL) centre.AccesPublicVl = true;
                if (u.MonCentreEstPL) centre.AccesPublicPl = true;
                if (u.MonCentreEstCL) centre.AccesPublicCl = true;
            }

            //On sauvegarde
            this.Save(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
                transaction.Commit();
        }

        public void supprimerUtilisateur(string id)
        {
            Query q = new Query();
            IList<rUtilisateur> utils;

            q.Criteria.Add(new Criteria("Id", id, CriteriaOperator.EQUALS));
            utils = this.FindBy(q);
            if (utils.Count() == 1)
            {
                //EG le 17/11/2011 this.Remove(utils.ElementAt(0));
                this.supprimerUtilisateur(utils.ElementAt(0));

            }
        }

        public void supprimerUtilisateur(rUtilisateur u)
        {
            this.Remove(u);
            IUnitOfWork transaction = this.getUnitOfWork();
            if (transaction != null)
            {
                transaction.Commit();
            }
        }

        public rUtilisateur CreateUserTemporaire(string username, string passwordTmp, rProfil profil, rProfil2 profil2, string codeUtilisateur, rCategorieVehicule.CategorieEnum categVehicule, pCentreEncour centre)
        {
            //EG le 16/02/2012 : Ajout du log4net + vérification que l'Id du centre != 0
            log.Debug("CreateUserTemporaire()....debut");

            if (centre.Id != 0)
            {
                rUtilisateur user = new rUtilisateur();

                user.Id = username;
                user.MotDePasseTemporaire = passwordTmp;
                //user.PasswordSalt = CreateSalt(); //Pour sécuriser les mots de passe
                user.DateCrea = DateTime.Now;
                user.CodUsrCrea = codeUtilisateur;
                user.Actif = false;
                user.Bloque = false;
                /* Début id_profil est obligatoire */
                user.RProfil = profil;
                user.RProfil2 = profil2;
                /* Fin id_profil est obligatoire */

                switch (categVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        user.PCentreEncourVL = centre;
                        break;
                    case rCategorieVehicule.CategorieEnum.PL:
                        user.PCentreEncourPL = centre;
                        break;
                    case rCategorieVehicule.CategorieEnum.CL:
                        user.PCentreEncourCL = centre;
                        break;
                }

                this.Save(user);
                log.Debug("CreateUserTemporaire()....fin normale");
                return user;
            }
            else
            {
                switch (categVehicule)
                {
                    case rCategorieVehicule.CategorieEnum.VL:
                        log.Error(string.Format("Utilisateur non créé pour le centre agrem = {0} , Raison sociale = {1} , CP = {2} ", centre.CodAgremVl, centre.RaisonSociale, centre.CpCentre));
                        break;
                    case rCategorieVehicule.CategorieEnum.PL:
                        log.Error(string.Format("Utilisateur non créé pour le centre agrem = {0} , Raison sociale = {1} , CP = {2} ", centre.CodAgremPl, centre.RaisonSociale, centre.CpCentre));
                        break;
                    case rCategorieVehicule.CategorieEnum.CL:
                        log.Error(string.Format("Utilisateur non créé pour le centre agrem = {0} , Raison sociale = {1} , CP = {2} ", centre.CodAgremCl, centre.RaisonSociale, centre.CpCentre));
                        break;
                }
                log.Debug("CreateUserTemporaire()....fin anormale");
                return null;
            }

        }

        public IEnumerable<rUtilisateur> FindAllTemporaire()
        {
            IList<rUtilisateur> liste = new List<rUtilisateur>();

            //VL
            Query q = new Query();
            q.Criteria.Add(new Criteria("MotDePasseTemporaire", null, CriteriaOperator.IS_NOT_NULL));
            q.Criteria.Add(new Criteria("PCentreEncourVL", null, CriteriaOperator.IS_NOT_NULL));
            IList<rUtilisateur> listeVL = this.FindBy(q);

            //PL
            q = new Query();
            q.Criteria.Add(new Criteria("MotDePasseTemporaire", null, CriteriaOperator.IS_NOT_NULL));
            q.Criteria.Add(new Criteria("PCentreEncourPL", null, CriteriaOperator.IS_NOT_NULL));
            IList<rUtilisateur> listePL = this.FindBy(q);

            //CL
            q = new Query();
            q.Criteria.Add(new Criteria("MotDePasseTemporaire", null, CriteriaOperator.IS_NOT_NULL));
            q.Criteria.Add(new Criteria("PCentreEncourCL", null, CriteriaOperator.IS_NOT_NULL));
            IList<rUtilisateur> listeCL = this.FindBy(q);

            return liste.Union(listeVL).Union(listePL).Union(listeCL);

        }

        public IList<rUtilisateur> FindAllUser(rPrefecture rPrefecture)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("RPrefecture", rPrefecture, CriteriaOperator.EQUALS));
            //2022-05-16 PLE sélection des utilisateurs actifs uniquement
            q.Criteria.Add(new Criteria("Actif", true, CriteriaOperator.EQUALS));
            q.OrderBy.Add(new OrderBy() { PropertyName = "Nom", Ascending = true });
            IList<rUtilisateur> liste = this.FindBy(q);
            return liste;
        }

        //YM 25/01/2012
        public IList<rUtilisateur> FindAllUserRegionProfil(rRegion region, rProfil profil1, rProfil2 profil2)
        {
            Query q = new Query();
            q.Criteria.Add(new Criteria("RRegion", region, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("RProfil", profil1, CriteriaOperator.EQUALS));
            q.Criteria.Add(new Criteria("RProfil2", profil2, CriteriaOperator.EQUALS));
            //2022-05-16 PLE sélection des utilisateurs actifs uniquement
            q.Criteria.Add(new Criteria("Actif", true, CriteriaOperator.EQUALS));
            q.OrderBy.Add(new OrderBy() { PropertyName = "Nom", Ascending = true });
            IList<rUtilisateur> liste = this.FindBy(q);
            return liste;
        }

        public rUtilisateur ModifierParametreFacturationUtilisateur(rUtilisateur utilisateur, bool facturePapier, string adresseMail, string codUserMaj)
        {
            IUnitOfWork transaction = this.getUnitOfWork();

            utilisateur.FacturePapier = facturePapier;
            utilisateur.Email = adresseMail;

            //Les infos systèmes
            utilisateur.CodUsrMaj = codUserMaj;
            utilisateur.DateMaj = System.DateTime.Now;

            // On sauvegarde l'utilisateur
            this.Save(utilisateur);

            if (transaction != null)
                transaction.Commit();

            return utilisateur;
        }

        //YM 30/04/2014 Liste des enregistrements de rutilisateur pour un centre (un centre Vl, PL et CL à 3 enregistrements dans rutilisateur pour le même id_centre)
        public IList<rUtilisateur> GetUserByCentre(pCentreEncour centre, rCategorieVehicule.CategorieEnum categorie)
        {
            Query q;
            switch (categorie)
            {
                case rCategorieVehicule.CategorieEnum.VL:
                    {
                        //VL
                        q = new Query();
                        q.Criteria.Add(new Criteria("PCentreEncourVL", centre, CriteriaOperator.EQUALS));
                        IList<rUtilisateur> listeVL = this.FindBy(q);
                        return listeVL;
                    }
                case rCategorieVehicule.CategorieEnum.PL:
                    {
                        //PL
                        q = new Query();
                        q.Criteria.Add(new Criteria("PCentreEncourPL", centre, CriteriaOperator.EQUALS));
                        IList<rUtilisateur> listePL = this.FindBy(q);
                        return listePL;
                    }
                case rCategorieVehicule.CategorieEnum.CL:
                    {
                        //CL
                        q = new Query();
                        q.Criteria.Add(new Criteria("PCentreEncourCL", centre, CriteriaOperator.EQUALS));
                        IList<rUtilisateur> listeCL = this.FindBy(q);
                        return listeCL;
                    }
                default:
                    {
                        return null;
                    }
            }

        }

        public Boolean SynchroniseUserWithDatabaseRequisition(rUtilisateur utilisateur)
        {
            return _utilisateurRepository.SynchroniseUserWithDatabaseRequisition(utilisateur);
        }

        public Boolean SynchroniseAllUsersWithDatabaseRequisition(rUtilisateurSearchObject search)
        {
            return _utilisateurRepository.SynchroniseAllUsersWithDatabaseRequisition(search);
        }
    }
}
