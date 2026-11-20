using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Criterion;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Infrastructure.Search;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class pContoleurEncourRepository : BaseRepository<pContoleurEncour>, IpControleurEnCoursRepository
    {
        private NHibernate.ICriteria createCriteria(ControleurTechniqueRecherche recherche)
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(pContoleurEncour));
            if (!string.IsNullOrEmpty(recherche.Nom))
                CriteriaQuery.Add(Expression.Like("this.Nom", recherche.Nom + "%"));

            if (!string.IsNullOrEmpty(recherche.Prenom))
                CriteriaQuery.Add(Expression.Like("this.Prenom", recherche.Prenom + "%"));

            if (!string.IsNullOrEmpty(recherche.NumeroAgrementControleurVL))
                CriteriaQuery.Add(Expression.Eq("this.CodAgremVl", recherche.NumeroAgrementControleurVL));

            if (!string.IsNullOrEmpty(recherche.NumeroAgrementControleurPL))
                CriteriaQuery.Add(Expression.Eq("this.CodAgremPl", recherche.NumeroAgrementControleurPL));

            if (!string.IsNullOrEmpty(recherche.NumeroAgrementCentreVL))
                CriteriaQuery.Add(Expression.Eq("this.CodAgremRattachementVl", recherche.NumeroAgrementCentreVL));

            if (!string.IsNullOrEmpty(recherche.NumeroAgrementCentrePL))
                CriteriaQuery.Add(Expression.Eq("this.CodAgremRattachementPl", recherche.NumeroAgrementCentrePL));

            //On trie par nom du contrôleur
            Order order = new Order("this.Nom", true);
            CriteriaQuery.AddOrder(order);

            return CriteriaQuery;
        }

        public IList<pContoleurEncour> LoadBy(ControleurTechniqueRecherche search)
        {
            NHibernate.ICriteria CriteriaQuery = this.createCriteria(search);
            return CriteriaQuery.List<pContoleurEncour>() as List<pContoleurEncour>;
        }

        public IList<pContoleurEncour> LoadBy(ControleurTechniqueRecherche search, int page, int pageSize, out long count)
        {
            throw new NotImplementedException();
        }

        public IList<pContoleurEncour> FindByFiltreProfilReseau(FiltreRechercheProfilReseau form, rReseau reseauFiltre, rPrefecture prefFiltre)
        {
            Dictionary<string, string[]> queryColumns = new Dictionary<string, string[]>();
            queryColumns.Add("VL", new string[] { "PCentreEncourVL", "ReseauRattachementVL", "EtatAgrementVl" });
            queryColumns.Add("PL", new string[] { "PCentreEncourPL", "ReseauRattachementPL", "EtatAgrementPl" });
            queryColumns.Add("CL", new string[] { "PCentreEncourCL", "ReseauRattachementCL", "EtatAgrementCl" });

            string propertyName = reseauFiltre.TypeReseauVl ? "VL" : reseauFiltre.TypeReseauPl ? "PL" : "CL";
            //reseauFiltre.PCentreEncours.
            ISession session = SessionFactory.GetCurrentSession();
            string hqlQuery = "select cntlr from pContoleurEncour cntlr where cntlr." + queryColumns[propertyName][2] + " = ? " +
                              " AND cntlr." + queryColumns[propertyName][0] + ".Id in (select centre.Id from pCentreEncour centre , rReseau r " + (prefFiltre != null ? ",rPrefecture pref " : "")  +
                              " where " + (prefFiltre != null ? "centre.RPrefecture = pref AND " : "") + " r = centre." + queryColumns[propertyName][1] + " AND r.Id = ? " +
                              (prefFiltre != null ? " AND pref.Id = ?" : "") + ") order by cntlr.Nom, cntlr.Prenom";

            IQuery q = session.CreateQuery(hqlQuery);
            q.SetString(0, "1");
            q.SetString(1, reseauFiltre.Id);
            if (prefFiltre != null)
            {
                q.SetInt32(2, prefFiltre.Id);
            }
            //q.SetString(1, reseauFiltre.Id);
            IList<pContoleurEncour> controleurs = q.List<pContoleurEncour>();

            return controleurs;
        }

        //EG le 16/01/2012
        public IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonRegion(rRegion region)
        {
            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_contoleur_encours ,p_centre_encours, r_prefecture " +
                                " WHERE ( p_contoleur_encours.etat_agrement_pl = '1' )    and  " +
                                " p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours and  " +
                                " p_centre_encours.id_prefecture = r_prefecture.id_prefecture " +
                                ((region != null) ? "and r_prefecture.id_region =" + region.Id : "") +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC ";

            //IList<pContoleurEncour> liste = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL, "controleur", typeof(pContoleurEncour)).List<pContoleurEncour>();
            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;

        }

        //YM 24/05/2013
        public IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursVLSelonRegion(rRegion region)
        {
            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_contoleur_encours ,p_centre_encours, r_prefecture " +
                                " WHERE ( p_contoleur_encours.etat_agrement_vl = '1' )    and  " +
                                " p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours and  " +
                                " p_centre_encours.id_prefecture = r_prefecture.id_prefecture " +
                                ((region != null) ? "and r_prefecture.id_region =" + region.Id : "") +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC ";

            //IList<pContoleurEncour> liste = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL, "controleur", typeof(pContoleurEncour)).List<pContoleurEncour>();
            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;

        }

        public IList<pContoleurEncour> FindAllForListeDesSuivisQualificationControleursPLSelonReseaux(IList<rReseau> reseaux)
        {
            string listeIdReseaux = string.Empty;
            var i = 0;
            foreach (var r in reseaux)
            {
                if (i > 0) listeIdReseaux += ",";
                listeIdReseaux += string.Format("'{0}'", r.Id);
                i++;
            }

            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_centre_encours,    " +
                                " p_contoleur_encours   " +
                                " WHERE ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and   " +
                                " ( ( p_centre_encours.cod_reseau_pl IN(" + listeIdReseaux + ") ) AND   " +
                                " ( p_contoleur_encours.etat_agrement_pl = '1' )    " +
                                " )    " +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;
        }

        public IList<pContoleurEncour> FindAllAgreesCentreFerme(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "id_centre_vl", "etat_agrement_vl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "id_centre_pl", "etat_agrement_pl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "id_centre_cl", "etat_agrement_cl" });

            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_centre_encours,    " +
                                " p_contoleur_encours " +
                                " WHERE ( p_contoleur_encours." + queryColumns[categorieVehicule][0] + " = p_centre_encours.id_centre_encours ) and   " +
                                " ( p_contoleur_encours." + queryColumns[categorieVehicule][1] + " = '1' )  AND   " +
                                " ( p_centre_encours." + queryColumns[categorieVehicule][1] + " = '0' )   AND " +
                                " p_centre_encours.id_prefecture = " + prefecture.Id +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;
        }

        public IList<pContoleurEncour> FindAllAgreesCentreFermeReseau(rCategorieVehicule.CategorieEnum categorieVehicule, IList<rReseau> reseaux)
        {

            string listeIdReseaux = string.Empty;
            var i = 0;
            foreach (var r in reseaux)
            {
                if (i > 0) listeIdReseaux += ",";
                listeIdReseaux += string.Format("'{0}'", r.Id);
                i++;
            }

            Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "id_centre_vl", "etat_agrement_vl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "id_centre_pl", "etat_agrement_pl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "id_centre_cl", "etat_agrement_cl" });

            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_centre_encours,    " +
                                " p_contoleur_encours " +
                                " WHERE ( p_contoleur_encours." + queryColumns[categorieVehicule][0] + " = p_centre_encours.id_centre_encours ) and   " +
                                " ( p_contoleur_encours." + queryColumns[categorieVehicule][1] + " = '1' )  AND   " +
                                " ( p_centre_encours." + queryColumns[categorieVehicule][1] + " = '0' )   AND " +

                                ((categorieVehicule == rCategorieVehicule.CategorieEnum.VL) ? "( p_centre_encours.cod_reseau_vl IN(" + listeIdReseaux + ") )  " : "") +
                                ((categorieVehicule == rCategorieVehicule.CategorieEnum.PL) ? "( p_centre_encours.cod_reseau_pl IN(" + listeIdReseaux + ") )  " : "") +
                                ((categorieVehicule == rCategorieVehicule.CategorieEnum.CL) ? "( p_centre_encours.cod_reseau_cl IN(" + listeIdReseaux + ") )  " : "") +

                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;
        }

        public IList<pContoleurEncour> FindAllAgreesCentreFermeRegion(rCategorieVehicule.CategorieEnum categorieVehicule, rRegion region)
        {

            Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "id_centre_vl", "etat_agrement_vl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "id_centre_pl", "etat_agrement_pl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "id_centre_cl", "etat_agrement_cl" });

            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_centre_encours,    " +
                                " p_contoleur_encours, " +
                                " r_prefecture " +
                                " WHERE ( p_contoleur_encours." + queryColumns[categorieVehicule][0] + " = p_centre_encours.id_centre_encours ) and   " +
                                " ( p_contoleur_encours." + queryColumns[categorieVehicule][1] + " = '1' )  AND   " +
                                " ( p_centre_encours." + queryColumns[categorieVehicule][1] + " = '0' )   AND " +
                                " ( p_centre_encours.id_prefecture = r_prefecture.id_prefecture ) AND " +
                                " (r_prefecture.id_region = " + string.Format("{0}", region.Id) + " ) AND " +

                                ((categorieVehicule == rCategorieVehicule.CategorieEnum.VL) ? "( p_centre_encours.cod_reseau_vl  is not null )  " : "") +
                                ((categorieVehicule == rCategorieVehicule.CategorieEnum.PL) ? "( p_centre_encours.cod_reseau_pl  is not null )  " : "") +
                                ((categorieVehicule == rCategorieVehicule.CategorieEnum.CL) ? "( p_centre_encours.cod_reseau_cl  is not null )  " : "") +

                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;
        }

        //EG le 26/01/2012
        public IList<pContoleurEncour> FindAllDemandeEnCours(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "id_centre_vl", "etat_agrement_vl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "id_centre_pl", "etat_agrement_pl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "id_centre_cl", "etat_agrement_cl" });

            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_centre_encours,    " +
                                " p_contoleur_encours " +
                                " WHERE ( p_contoleur_encours." + queryColumns[categorieVehicule][0] + " = p_centre_encours.id_centre_encours ) and   " +
                                " ( p_contoleur_encours." + queryColumns[categorieVehicule][1] + " = '" + Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_DEMANDE_EN_COURS) + "' )  AND   " +
                                //" ( p_centre_encours." + queryColumns[categorieVehicule][1] + " = '1' )   AND " +
                                " p_centre_encours.id_prefecture = " + prefecture.Id +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;
        }

        public IList<pContoleurEncour> FindAllNouvellementAgree(rPrefecture prefecture, rCategorieVehicule.CategorieEnum categorie, DateTime debut, DateTime fin)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "id_centre_vl", "etat_agrement_vl", "date_effet_notification_vl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "id_centre_pl", "etat_agrement_pl", "date_effet_notification_pl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "id_centre_cl", "etat_agrement_cl", "date_effet_notification_cl" });

            string dateDebut = string.Format("{0:yyyy-MM-dd}", debut);
            string dateFin = string.Format("{0:yyyy-MM-dd}", fin);

            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_centre_encours,    " +
                                " p_contoleur_encours " +
                                " WHERE ( p_contoleur_encours." + queryColumns[categorie][0] + " = p_centre_encours.id_centre_encours ) and   " +
                                " ( p_contoleur_encours." + queryColumns[categorie][1] + " = '" + Convert.ToString((int)pContoleurEncour.CodeEtatAgrementVl.AGREMENT_OK) + "' )  AND   " +
                                //" ( p_centre_encours." + queryColumns[categorieVehicule][1] + " = '1' )   AND " +
                                " p_contoleur_encours." + queryColumns[categorie][2] + " between '" + dateDebut + "' AND '" + dateFin + "' AND" +
                                " p_centre_encours.id_prefecture = " + prefecture.Id +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;
        }

        public IList<pContoleurEncour> FindAllAgreesSansCentreRattachement(rCategorieVehicule.CategorieEnum categorieVehicule, rPrefecture prefecture)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "id_centre_vl", "etat_agrement_vl", "cod_agrem_vl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "id_centre_pl", "etat_agrement_pl", "cod_agrem_pl" });
            queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "id_centre_cl", "etat_agrement_cl", "cod_agrem_cl" });

            string departement = string.Empty;
            if (prefecture != null)
            {
                departement = prefecture.CodDepartement;
                if (departement.Length == 1) departement = "00" + departement;
                if (departement.Length == 2) departement = "0" + departement;
            }

            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_contoleur_encours " +
                                " WHERE ( p_contoleur_encours." + queryColumns[categorieVehicule][0] + " IS NULL ) AND   " +
                                " ( p_contoleur_encours." + queryColumns[categorieVehicule][1] + " = '1' ) " +
                                (prefecture != null ? " AND substring(" + queryColumns[categorieVehicule][2] + ",1,3) = '" + departement + "'" : "") +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;
        }

        //YM le 23/10/2013
        public IList<pContoleurEncour> FindAllPrestatairesEuropeensVL(rPrefecture prefecture)
        {
            string departement = string.Empty;
            if (prefecture != null)
            {
                departement = prefecture.CodDepartement;
                if (departement.Length == 1) departement = "00" + departement;
                if (departement.Length == 2) departement = "0" + departement;
            }
            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_contoleur_encours  " +
                                " WHERE  est_prestataire = 1 " +
                                (prefecture != null ? " AND substring(cod_agrem_rattachement_vl,2,3) = '" + departement + "'" : "") +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;

        }

        public IList<pContoleurEncour> FindAllPrestatairesEuropeensPL(rPrefecture prefecture)
        {
            string departement = string.Empty;
            if (prefecture != null)
            {
                departement = prefecture.CodDepartement;
                if (departement.Length == 1) departement = "00" + departement;
                if (departement.Length == 2) departement = "0" + departement;
            }
            string requeteSQL = "SELECT p_contoleur_encours.*  " +
                                " FROM p_contoleur_encours  " +
                                " WHERE  est_prestataire = 1 " +
                                (prefecture != null ? " AND substring(cod_agrem_rattachement_pl,2,3) = '" + departement + "'" : "") +
                                " ORDER BY p_contoleur_encours.nom ASC,    " +
                                " p_contoleur_encours.prenom ASC ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pContoleurEncour));

            IList<pContoleurEncour> liste = sqlQuery.List<pContoleurEncour>();

            return liste;

        }

    }
}
