using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NHibernate.Criterion;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.certificats;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;
namespace Utac.Rnc2.Repository
{
    public class pCentreEncourRepository : BaseRepository<pCentreEncour>, IpCentreEncourRepository
    {

        public IList<pCentreEncour> findAllPL()
        {
            NHibernate.ICriteria criteria = SessionFactory.GetCurrentSession().CreateCriteria<pCentreEncour>();
            criteria.Add(Expression.IsNotNull("CodAgremPl"));
            return criteria.List<pCentreEncour>();
        }

        public IList<pCentreEncour> findAllVL()
        {
            NHibernate.ICriteria criteria = SessionFactory.GetCurrentSession().CreateCriteria<pCentreEncour>();
            criteria.Add(Expression.IsNotNull("CodAgremVl"));
            return criteria.List<pCentreEncour>();
        }

        private struct ListeProprietesQuery
        {
            public string DateEffetNotificationProp;
            public string CodeAgremProp;
            public string EtatAgremProp;
            public string CodeReseauProp;
            public string IdCentreProp;
        }

        public IList<ItemListeDesInfosCertificatsExpireOuRevoque> findAllAgreeAvecCertificatExpireOuRevoque(rCategorieVehicule.CategorieEnum categorie)
        {
            Dictionary<rCategorieVehicule.CategorieEnum, ListeProprietesQuery> ListePropriete = new Dictionary<rCategorieVehicule.CategorieEnum, ListeProprietesQuery>();
            ListePropriete.Add(rCategorieVehicule.CategorieEnum.VL, new ListeProprietesQuery() { IdCentreProp = "id_centre_encours_vl", DateEffetNotificationProp = "date_effet_notification_vl", CodeAgremProp = "cod_agrem_vl", EtatAgremProp = "etat_agrement_vl", CodeReseauProp = "cod_reseau_vl" });
            ListePropriete.Add(rCategorieVehicule.CategorieEnum.PL, new ListeProprietesQuery() { IdCentreProp = "id_centre_encours_pl", DateEffetNotificationProp = "date_effet_notification_pl", CodeAgremProp = "cod_agrem_pl", EtatAgremProp = "etat_agrement_pl", CodeReseauProp = "cod_reseau_pl" });
            ListePropriete.Add(rCategorieVehicule.CategorieEnum.CL, new ListeProprietesQuery() { IdCentreProp = "id_centre_encours_cl", DateEffetNotificationProp = "date_effet_notification_cl", CodeAgremProp = "cod_agrem_cl", EtatAgremProp = "etat_agrement_cl", CodeReseauProp = "cod_reseau_cl" });

            string requete = "SELECT p_centre_encours." + ListePropriete[categorie].CodeAgremProp + ",   " +
                             "p_centre_encours." + ListePropriete[categorie].EtatAgremProp + ",   " +
                             "p_centre_encours.id_centre_encours,  " +
                             "p_centre_encours." + ListePropriete[categorie].CodeReseauProp + ",    " +
                             "p_centre_encours.raison_sociale,   " +
                             "p_centre_encours.cp_centre,    " +
                             "p_centre_encours.ville,    " +
                             "p_centre_encours.nom_representant_legal,    " +
                             "p_centre_encours." + ListePropriete[categorie].DateEffetNotificationProp + ", " +
                             "p_certificat.nom_fichier, " +
                             "p_certificat.id_req, " +
                             "p_certificat.num_serie " +
                             "FROM p_centre_encours,    " +
                             "p_certificat,    " +
                             "p_gestion_certificat gc1   " +
                             " WHERE ( gc1.id_req = p_certificat.id_req ) and   " +
                             "( gc1.num_serie = p_certificat.num_serie ) and   " +
                             "( p_centre_encours.id_centre_encours = gc1." + ListePropriete[categorie].IdCentreProp + ") and   " +
                             "( ( p_centre_encours." + ListePropriete[categorie].CodeReseauProp + " = 'Z' ) AND   " +
                             "( p_centre_encours." + ListePropriete[categorie].EtatAgremProp + " = '1' ) )    " +
                             "                                      and gc1.id_gestion_certificat = (select max(gc2.id_gestion_certificat) from p_gestion_certificat gc2 where gc2." + ListePropriete[categorie].IdCentreProp + " = p_centre_encours.id_centre_encours) " +
                             "                                      AND ((p_certificat.date_expiration <=getdate()) OR (p_certificat.date_revocation <= getdate())) " +
                             " ORDER BY p_centre_encours." + ListePropriete[categorie].CodeAgremProp + " ASC   ";

            List<ItemListeDesInfosCertificatsExpireOuRevoque> infos = new List<ItemListeDesInfosCertificatsExpireOuRevoque>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requete).List();
            foreach (object[] row in results)
            {
                ItemListeDesInfosCertificatsExpireOuRevoque item = new ItemListeDesInfosCertificatsExpireOuRevoque();
                item.CodAgrement = (string)row[0];
                item.EtatAgrement = (string)row[1];
                item.IdCentreEncours = Convert.ToInt32(row[2]);
                item.CodReseau = (string)(row[3]);
                item.RaisonSociale = (string)(row[4]);
                item.CodePostale = (string)(row[5]);
                item.Ville = (string)(row[6]);
                item.NomRepresentantLegal = (string)(row[7]);
                item.DateEffetNotification = string.Format("{0:d}", row[8]);
                item.NomFichier = (string)(row[9]);
                item.IdReq = Convert.ToString((row[10]));
                item.NumSerie = (string)(row[11]);

                infos.Add(item);
            }
            return infos;

        }

        //YM 26/01/2012
        public IList<pCentreEncour> FindAllCentreNouvellementAgreeRegion(rRegion region, string datedeb)
        {
            //DateTime datedeb = System.DateTime.Today;
            //int daydeb = datedeb.Day;
            //datedeb = System.DateTime.Today.AddDays(-daydeb);
            //datedeb = datedeb.AddMonths(-1);
            //string datedebmois = string.Format("{0000}", datedeb.Year) + '/' + string.Format("{00}", datedeb.Month) + '/' + string.Format("{00}", datedeb.Day);

            //Dictionary<rCategorieVehicule.CategorieEnum, string[]> queryColumns = new Dictionary<rCategorieVehicule.CategorieEnum, string[]>();
            //queryColumns.Add(rCategorieVehicule.CategorieEnum.VL, new string[] { "id_centre_vl", "etat_agrement_vl" });
            //queryColumns.Add(rCategorieVehicule.CategorieEnum.PL, new string[] { "id_centre_pl", "etat_agrement_pl" });
            //queryColumns.Add(rCategorieVehicule.CategorieEnum.CL, new string[] { "id_centre_cl", "etat_agrement_cl" });

            string requeteSQL = "SELECT p_centre_encours.*  " +
                                " FROM  p_centre_encours,    " +
                                "       r_prefecture " +
                                " WHERE ( p_centre_encours.id_prefecture = r_prefecture.id_prefecture ) AND" +
                                "       (r_prefecture.id_region = " + string.Format("{0}", region.Id) + " ) AND " +
                                "       ((p_centre_encours.etat_agrement_vl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK) + "' AND p_centre_encours.date_effet_notification_vl > '" + datedeb + "')  OR " +
                                "        (p_centre_encours.etat_agrement_pl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK) + "' AND p_centre_encours.date_effet_notification_pl > '" + datedeb + "')  OR " +
                                "        (p_centre_encours.etat_agrement_cl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK) + "' AND p_centre_encours.date_effet_notification_cl > '" + datedeb + "'))    " +
                                " ORDER BY p_centre_encours.raison_sociale ASC  ";

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pCentreEncour));

            IList<pCentreEncour> liste = sqlQuery.List<pCentreEncour>();

            return liste;
        }

        //YM 27/01/2012
        public IList<pCentreEncour> FindAllCentreAgreeSansControleurRegion(rRegion region)
        {

            //string requeteSQL = "SELECT p_centre_encours.*  " +
            //                    " FROM  p_centre_encours,    " +
            //                    "       r_prefecture " +
            //                    " WHERE ( p_centre_encours.id_prefecture = r_prefecture.id_prefecture ) AND" +
            //                    "       (r_prefecture.id_region = " + string.Format("{0}", region.Id) + " ) AND " +
            //                    "       ((p_centre_encours.etat_agrement_vl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK) + "' )  OR " +
            //                    "        (p_centre_encours.etat_agrement_pl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK) + "' )  OR " +
            //                    "        (p_centre_encours.etat_agrement_cl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK) + "' )) AND   " +
            //                    "       NOT EXISTS (SELECT * FROM p_contoleur_encours WHERE p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours)  AND " +
            //                    "       NOT EXISTS (SELECT * FROM p_contoleur_encours WHERE p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours)  AND " +
            //                    "       NOT EXISTS (SELECT * FROM p_contoleur_encours WHERE p_contoleur_encours.id_centre_cl = p_centre_encours.id_centre_encours)      " +
            //                    " ORDER BY p_centre_encours.raison_sociale ASC  ";

            StringBuilder requeteSQL = new StringBuilder();
            requeteSQL.Append("SELECT p_centre_encours.* FROM p_centre_encours, r_prefecture  ");
            requeteSQL.Append(" WHERE ( p_centre_encours.id_prefecture = r_prefecture.id_prefecture ) AND ");
            requeteSQL.Append("       (r_prefecture.id_region = " + string.Format("{0}", region.Id) + " ) AND ");
            requeteSQL.Append("       (p_centre_encours.etat_agrement_vl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementVl.AGREMENT_OK) + "' )  AND");
            requeteSQL.Append("       NOT EXISTS (SELECT 1 FROM p_contoleur_encours WHERE p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours) ");
            requeteSQL.Append(" UNION ALL ");
            requeteSQL.Append("SELECT p_centre_encours.* FROM p_centre_encours, r_prefecture  ");
            requeteSQL.Append(" WHERE ( p_centre_encours.id_prefecture = r_prefecture.id_prefecture ) AND");
            requeteSQL.Append("       (r_prefecture.id_region = " + string.Format("{0}", region.Id) + " ) AND ");
            requeteSQL.Append("       (p_centre_encours.etat_agrement_pl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementPl.AGREMENT_OK) + "' )  AND ");
            requeteSQL.Append("       NOT EXISTS (SELECT 1 FROM p_contoleur_encours WHERE p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours) ");
            requeteSQL.Append(" UNION ALL ");
            requeteSQL.Append("SELECT p_centre_encours.* FROM p_centre_encours, r_prefecture  ");
            requeteSQL.Append(" WHERE ( p_centre_encours.id_prefecture = r_prefecture.id_prefecture ) AND");
            requeteSQL.Append("       (r_prefecture.id_region = " + string.Format("{0}", region.Id) + " ) AND ");
            requeteSQL.Append("       (p_centre_encours.etat_agrement_cl = '" + Convert.ToString((int)pCentreEncour.CodeEtatAgrementCl.AGREMENT_OK) + "' )  AND ");
            requeteSQL.Append("       NOT EXISTS (SELECT 1 FROM p_contoleur_encours WHERE p_contoleur_encours.id_centre_cl = p_centre_encours.id_centre_encours) ");
            requeteSQL.Append(" ORDER BY p_centre_encours.raison_sociale ASC  ");



            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL.ToString());
            sqlQuery.AddEntity(typeof(pCentreEncour));

            IList<pCentreEncour> liste = sqlQuery.List<pCentreEncour>();

            return liste;
        }
    }
}
