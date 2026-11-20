using System;
using System.Collections;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Repositories.statistiques;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class StatistiquesCentreRepository : BaseRepository<pCentreEncour>, IStatistiquesCentreRepository
    {

        public IList<ItemNombreInstallations> NombreInstallationsVehiculesLegersSelonRegion(rRegion region, rPrefecture prefecture)
        {
            string requeteSQL =
                    "select   nb_cntr_s_non_ratt, nb_cntr_s_ratt, nb_cntr_a_non_ratt, nb_cntr_a_ratt, " +
                    "         r_prefecture.libelle_prefecture, r_prefecture.cod_departement, r_region.libelle_region, r_prefecture.id_region  " +
                    "from " +
                    "     (SELECT id_prefecture as id_1,   count(id_centre_encours) as nb_cntr_s_non_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_vl = '1'  and cod_reseau_vl = 'Z' and type_centre = 'S' " +
                    "      GROUP BY id_prefecture) as count_1, " +
                    "     (SELECT id_prefecture id_2,   count(id_centre_encours) as nb_cntr_s_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_vl = '1'  and cod_reseau_vl <> 'Z' and type_centre = 'S' " +
                    "      GROUP BY id_prefecture ) as count_2, " +
                    "     (SELECT id_prefecture as id_3,   count(id_centre_encours) as nb_cntr_a_non_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_vl = '1'  and cod_reseau_vl = 'Z'  and type_centre = 'A' " +
                    "      GROUP BY id_prefecture UNION ALL SELECT 0 as id_3, 0 as  nb_cntr_a_non_ratt) as count_3, " +
                    "     (SELECT id_prefecture id_4,  count(id_centre_encours) as nb_cntr_a_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_vl = '1'  and cod_reseau_vl <> 'Z'  and type_centre = 'A' " +
                    "      GROUP BY id_prefecture UNION ALL SELECT 0 as id_4, 0 as  nb_cntr_a_ratt) as count_4, " +
                    "     r_prefecture, r_region " +
                    "WHERE r_prefecture.id_prefecture *= count_1.id_1 and r_prefecture.id_prefecture *= count_2.id_2 " +
                    "      and r_prefecture.id_prefecture *= count_3.id_3 and r_prefecture.id_prefecture *= count_4.id_4 " +
                    "      and r_prefecture.id_region = r_region.id_region " +

                    ((prefecture != null) ? " AND ( r_prefecture.id_prefecture = " + prefecture.Id + ")" : "") +
                    ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "");

            List<ItemNombreInstallations> stat = new List<ItemNombreInstallations>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemNombreInstallations item = new ItemNombreInstallations();
                item.SpecialiseNonRattache = Convert.ToString(row[0]);
                item.SpecialiseRattache = Convert.ToString(row[1]);
                item.AuxiliaireNonRattache = Convert.ToString(row[2]);
                item.AuxiliaireRattache = Convert.ToString(row[3]);
                item.Prefecture = (string)(row[4]);
                item.CodeDepartement = (string)(row[5]);
                item.Region = (string)(row[6]);
                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemNombreInstallations> NombreInstallationsVehiculesLourdsSelonRegion(rRegion region, rPrefecture prefecture)
        {
            string requeteSQL =
                    "select   nb_cntr_s_non_ratt, nb_cntr_s_ratt, nb_cntr_a_non_ratt, nb_cntr_a_ratt, " +
                    "         r_prefecture.libelle_prefecture, r_prefecture.cod_departement, r_region.libelle_region, r_prefecture.id_region  " +
                    "from " +
                    "     (SELECT id_prefecture as id_1,   count(id_centre_encours) as nb_cntr_s_non_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_pl = '1'  and cod_reseau_pl = 'Z' and type_centre = 'S' " +
                    "      GROUP BY id_prefecture) as count_1, " +
                    "     (SELECT id_prefecture id_2,   count(id_centre_encours) as nb_cntr_s_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_pl = '1'  and cod_reseau_pl <> 'Z' and type_centre = 'S' " +
                    "      GROUP BY id_prefecture ) as count_2, " +
                    "     (SELECT id_prefecture as id_3,   count(id_centre_encours) as nb_cntr_a_non_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_pl in ('1','4','5')  and cod_reseau_pl = 'Z'  and type_centre = 'A' " +
                    "      GROUP BY id_prefecture ) as count_3, " +
                    "     (SELECT id_prefecture id_4,  count(id_centre_encours) as nb_cntr_a_ratt " +
                    "      FROM p_centre_encours " +
                    "      WHERE  etat_agrement_pl in ('1','4','5')  and cod_reseau_pl <> 'Z'  and type_centre = 'A' " +
                    "      GROUP BY id_prefecture ) as count_4, " +
                    "      r_prefecture, r_region " +
                    "WHERE r_prefecture.id_prefecture *= count_1.id_1 and r_prefecture.id_prefecture *= count_2.id_2 " +
                    "      and r_prefecture.id_prefecture *= count_3.id_3 and r_prefecture.id_prefecture *= count_4.id_4 " +
                    "      and r_prefecture.id_region = r_region.id_region " +

                    ((prefecture != null) ? " AND ( r_prefecture.id_prefecture = " + prefecture.Id + ")" : "") +
                    ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "");

            List<ItemNombreInstallations> stat = new List<ItemNombreInstallations>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemNombreInstallations item = new ItemNombreInstallations();
                item.SpecialiseNonRattache = Convert.ToString(row[0]);
                item.SpecialiseRattache = Convert.ToString(row[1]);
                item.AuxiliaireNonRattache = Convert.ToString(row[2]);
                item.AuxiliaireRattache = Convert.ToString(row[3]);
                item.Prefecture = (string)(row[4]);
                item.CodeDepartement = (string)(row[5]);
                item.Region = (string)(row[6]);
                stat.Add(item);
            }
            return stat;
        }

        //public IList<ItemListeDesControleursVéhiculesLegersEtLourdsSelonRegion> listeDesControleursVehiculesLegersEtLourdsSelonRegion(rRegion region)
        //{

        //    string requeteSQL = 
        //            "SELECT p_contoleur_encours.nom, "   +
        //            "       p_contoleur_encours.prenom, "  +
        //            "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,    "   +
        //            "       p_contoleur_encours.cod_agrem_pl as agrement_controleur,    "   +
        //            "       p_contoleur_encours.cod_habilitation as qualification_PL, "   +
        //            "       null as habilitation_gaz_vl, "   +
        //            "       null as date_declaration_habil_gaz_vl,     "   +
        //            "       convert(char(12),p_contoleur_encours.date_effet_notification_pl,103)as date_effet_notification,    "   +
        //            "       p_contoleur_encours.cod_agrem_rattachement_pl as agrement_centre_rattachement,    "   +
        //            "       p_centre_encours.cod_reseau_pl as code_reseau,    "   +
        //            "       r_prefecture.cod_departement as code_departement,    "   +
        //            "       'PL' as categorie_vehicule   "   +
        //            "FROM p_centre_encours,    "   +
        //            "     r_prefecture,    "   +
        //            "     p_contoleur_encours,    "   +
        //            "     r_region   "   +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   "   +
        //            "      ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and   "   +
        //            "      ( r_region.id_region = r_prefecture.id_region ) and   "   +
        //            "      ( p_contoleur_encours.etat_agrement_pl = '1' )     "   +
        //                    ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +

        //            "UNION ALL    " +

        //            "SELECT p_contoleur_encours.nom,       " +
        //            "       p_contoleur_encours.prenom,       " +
        //            "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,       " +
        //            "       p_contoleur_encours.cod_agrem_vl as agrement_controleur,       " +
        //            "       '' as qualification_PL,    " +
        //            "       CASE     " +
        //            "           WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then 'Oui'    " +
        //            "           WHEN p_contoleur_encours.habilitation_gaz_vl = 0 then 'Non'     " +
        //            "           ELSE null    " +
        //            "       END as habilitation_gaz_vl,    " +
        //            "       convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103) as date_declaration_habil_gaz_vl,         " +
        //            "       convert(char(12),p_contoleur_encours.date_effet_notification_vl,103)as date_effet_notification,       " +
        //            "       p_contoleur_encours.cod_agrem_rattachement_vl as agrement_centre_rattachement,       " +
        //            "       p_centre_encours.cod_reseau_vl as code_reseau,       " +
        //            "       r_prefecture.cod_departement as code_departement,       " +
        //            "       'VL' as categorie_vehicule      " +
        //            "FROM p_centre_encours,       " +
        //            "     r_prefecture,       " +
        //            "     p_contoleur_encours,       " +
        //            "     r_region      " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and      " +
        //            "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and      " +
        //            "      ( r_region.id_region = r_prefecture.id_region ) and      " +
        //            "      ( p_contoleur_encours.etat_agrement_vl = '1' )     " +
        //                    ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +

        //            "UNION ALL    " +

        //            "SELECT p_contoleur_encours.nom,       " +
        //            "       p_contoleur_encours.prenom,       " +
        //            "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,       " +
        //            "       p_contoleur_encours.cod_agrem_cl as agrement_controleur,       " +
        //            "       '' as qualification_PL,    " +
        //            "       null as habilitation_gaz_vl, " +
        //            "       null as date_declaration_habil_gaz_vl,     " +
        //            "       convert(char(12),p_contoleur_encours.date_effet_notification_vl,103)as date_effet_notification,       " +
        //            "       p_contoleur_encours.cod_agrem_rattachement_vl as agrement_centre_rattachement,       " +
        //            "       p_centre_encours.cod_reseau_cl as code_reseau,       " +
        //            "       r_prefecture.cod_departement as code_departement,       " +
        //            "       'CL' as categorie_vehicule      " +
        //            "FROM p_centre_encours,       " +
        //            "     r_prefecture,       " +
        //            "     p_contoleur_encours,       " +
        //            "     r_region      " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and      " +
        //            "      ( p_contoleur_encours.id_centre_cl = p_centre_encours.id_centre_encours ) and      " +
        //            "      ( r_region.id_region = r_prefecture.id_region ) and      " +
        //            "      ( p_contoleur_encours.etat_agrement_cl = '1' )     " +
        //                    ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +

        //            "order by p_contoleur_encours.nom,p_contoleur_encours.prenom    ";

        //    List<ItemListeDesControleursVéhiculesLegersEtLourdsSelonRegion> stat = new List<ItemListeDesControleursVéhiculesLegersEtLourdsSelonRegion>();

        //    var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesControleursVéhiculesLegersEtLourdsSelonRegion item = new ItemListeDesControleursVéhiculesLegersEtLourdsSelonRegion();
        //        item.Nom = (string)row[0];
        //        item.Prenom = (string)row[1];
        //        item.DateNaissance = (string)(row[2]);
        //        item.AgrementControleur = (string)(row[3]);
        //        item.QualificationPL = (string)(row[4]);
        //        item.HabilitationGazVl = (string)(row[5]);
        //        item.DateDeclarationHabilGazVl = (string)(row[6]);
        //        item.DateEffetNotification = (string)(row[7]);
        //        item.AgrementCentreRattachement = (string)(row[8]);
        //        item.CodeReseau = (string)(row[9]);
        //        item.CodeDepartement = (string)(row[10]);
        //        item.CategorieVehicule = (string)(row[11]);
        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        //public IList<ItemListeDesControleursRattachesAuxCentresDuReseau> listeDesControleursRattachesAuxCentresDuReseau(rUtilisateur utilisateur)
        //{
        //    string sqlin = null;
        //    foreach (rReseau r in utilisateur.RReseaux)
        //    {
        //        if (sqlin != null)
        //        {
        //            sqlin += ",";
        //        }
        //        sqlin += "'"+r.Id+"'";
        //    }

        //    string requeteSQL =
        //            "SELECT p_contoleur_encours.nom,  " +
        //            "       p_contoleur_encours.prenom,    " +
        //            "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
        //            "       p_contoleur_encours.cod_agrem_pl as agrement_controleur,   " +
        //            "       p_contoleur_encours.cod_habilitation as qualification_PL," +
        //            "       null as habilitation_gaz_vl," +
        //            "       null as date_declaration_habil_gaz_vl, " +
        //            "       convert(char(12),p_contoleur_encours.date_effet_notification_pl,103)as date_effet_notification,   " +
        //            "       p_contoleur_encours.cod_agrem_rattachement_pl as agrement_centre_rattachement,   " +
        //            "       p_centre_encours.cod_reseau_pl as code_reseau,   " +
        //            "       r_prefecture.cod_departement as code_departement,   " +
        //            "       'PL' as categorie_vehicule  " +
        //            "FROM p_centre_encours,   " +
        //            "     r_prefecture,   " +
        //            "     p_contoleur_encours " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
        //            "      ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and" +
        //            "      ( p_contoleur_encours.etat_agrement_pl = '1' ) and " +
        //            "      ( p_centre_encours.cod_reseau_pl IN ("+sqlin+")) " +

        //            "UNION ALL " +

        //            "SELECT p_contoleur_encours.nom,   " +
        //            "       p_contoleur_encours.prenom,   " +
        //            "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
        //            "       p_contoleur_encours.cod_agrem_vl as agrement_controleur,   " +
        //            "       null as qualification_PL," +
        //            "       CASE " +
        //            "          WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then 'Oui' " +
        //            "          WHEN p_contoleur_encours.habilitation_gaz_vl = 0 then 'Non' " +
        //            "          ELSE null" +
        //            "       END as habilitation_gaz_vl," +
        //            "       convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103) as date_declaration_habil_gaz_vl,  " +
        //            "       convert(char(12),p_contoleur_encours.date_effet_notification_vl,103)as date_effet_notification,   " +
        //            "       p_contoleur_encours.cod_agrem_rattachement_vl as agrement_centre_rattachement,   " +
        //            "       p_centre_encours.cod_reseau_vl as code_reseau,   " +
        //            "       r_prefecture.cod_departement as code_departement,   " +
        //            "       'VL' as categorie_vehicule  " +
        //            "FROM p_centre_encours,   " +
        //            "     r_prefecture,   " +
        //            "     p_contoleur_encours     " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
        //            "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and  " +
        //            "      ( p_contoleur_encours.etat_agrement_vl = '1' ) and " +
        //            "      ( p_centre_encours.cod_reseau_vl IN (" + sqlin + ")) " +

        //            "UNION ALL " +

        //            "SELECT p_contoleur_encours.nom,   " +
        //            "       p_contoleur_encours.prenom,   " +
        //            "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
        //            "       p_contoleur_encours.cod_agrem_vl as agrement_controleur,   " +
        //            "       null as qualification_PL," +
        //            "       null as habilitation_gaz_vl," +
        //            "       null as date_declaration_habil_gaz_vl,  " +
        //            "       null as date_effet_notification,   " +
        //            "       p_contoleur_encours.cod_agrem_rattachement_cl as agrement_centre_rattachement,   " +
        //            "       p_centre_encours.cod_reseau_cl as code_reseau,   " +
        //            "       r_prefecture.cod_departement as code_departement,   " +
        //            "       'CL' as categorie_vehicule  " +
        //            "FROM p_centre_encours,   " +
        //            "     r_prefecture,   " +
        //            "     p_contoleur_encours     " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
        //            "      ( p_contoleur_encours.id_centre_cl = p_centre_encours.id_centre_encours ) and  " +
        //            "      ( p_contoleur_encours.etat_agrement_cl = '1' ) and " +
        //            "      ( p_centre_encours.cod_reseau_cl IN (" + sqlin + ")) " +
        //            "order by p_contoleur_encours.nom,p_contoleur_encours.prenom";

        //    List<ItemListeDesControleursRattachesAuxCentresDuReseau> stat = new List<ItemListeDesControleursRattachesAuxCentresDuReseau>();

        //    var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesControleursRattachesAuxCentresDuReseau item = new ItemListeDesControleursRattachesAuxCentresDuReseau();
        //        item.Nom = (string)row[0];
        //        item.Prenom = (string)row[1];
        //        item.DateNaissance = (string)(row[2]);
        //        item.AgrementControleur = (string)(row[3]);
        //        item.QualificationPL = (string)(row[4]);
        //        item.HabilitationGazVl = (string)(row[5]);
        //        item.DateDeclarationHabilGazVl = (string)(row[6]);
        //        item.DateEffetNotification = (string)(row[7]);
        //        item.AgrementCentreRattachement = (string)(row[8]);
        //        item.CodeReseau = (string)(row[9]);
        //        item.CodeDepartement = (string)(row[10]);
        //        item.CategorieVehicule = (string)(row[11]);
        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        public IList<ItemListeDesInstallationsVehiculesLegersSelonRegion> listeDesInstallationsVehiculesLegersSelonRegion(rRegion region, rPrefecture prefecture)
        {

            string requeteSQL =
                    "SELECT convert(char(2),r_region.id_region) as id_region,   " +
                    "r_region.libelle_region,   " +
                    "convert(char(3),r_prefecture.id_prefecture) as id_prefecture,    " +
                    "r_prefecture.cod_departement,   " +
                    "r_prefecture.libelle_prefecture,   " +
                    "p_centre_encours.type_centre,   " +
                    "p_centre_encours.raison_sociale,   " +
                    "p_centre_encours.cod_reseau_vl,   " +
                    "convert(char(12),p_centre_encours.date_effet_notification_vl) as date_effet_notification_vl,   " +
                    "p_centre_encours.cod_agrem_vl,   " +
                    "p_centre_encours.cp_centre,   " +
                    "p_centre_encours.ville,   " +
                    "p_centre_encours.num_tel " +
                    "FROM p_centre_encours,   " +
                    "    r_prefecture,   " +
                    "    r_region   " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
                    "      ( r_region.id_region = r_prefecture.id_region ) and     " +
                    "      ( p_centre_encours.etat_agrement_vl = '1' ) " +
                           ((region != null) ? " AND ( r_region.id_region = " + region.Id + ")" : "") +
                           ((prefecture != null) ? " AND ( r_prefecture.id_prefecture = " + prefecture.Id + ")" : "") +
                    "ORDER BY  p_centre_encours.raison_sociale     ";

            List<ItemListeDesInstallationsVehiculesLegersSelonRegion> stat = new List<ItemListeDesInstallationsVehiculesLegersSelonRegion>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesInstallationsVehiculesLegersSelonRegion item = new ItemListeDesInstallationsVehiculesLegersSelonRegion();
                item.IdRegion = (string)row[0];
                item.LibelleRegion = (string)row[1];
                item.IdPrefecture = (string)(row[2]);
                item.CodDepartement = (string)(row[3]);
                item.LibellePrefecture = (string)(row[4]);
                item.TypeCentre = (string)(row[5]);
                item.RaisonSociale = (string)(row[6]);
                item.CodeReseau = (string)(row[7]);
                item.DateEffetNotification = (string)(row[8]);
                item.CodAgrement = (string)(row[9]);
                item.CpCentre = (string)(row[10]);
                item.Ville = (string)(row[11]);
                item.numtel = (string)(row[12]);
                stat.Add(item);
            }
            return stat;
        }

        //public IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonDepartement(rPrefecture prefecture)
        //{
        //    string requeteSQL =
        //            "SELECT  convert(varchar(4),r_prefecture.id_prefecture) as id_prefecture , " +
        //            "r_prefecture.cod_departement , " +
        //            "r_prefecture.libelle_prefecture , " +
        //            "p_centre_encours.type_centre , " +
        //            "p_centre_encours.raison_sociale , " +
        //            "p_centre_encours.cod_reseau_vl , " +
        //            "convert(char(12),p_centre_encours.date_effet_notification_vl,103) as centre_date_notif_vl,   " +
        //            "p_centre_encours.cod_agrem_vl , " +
        //            "p_centre_encours.cp_centre , " +
        //            "p_centre_encours.ville , " +

        //            "p_contoleur_encours.nom+' '+p_contoleur_encours.prenom , " +
        //            "p_contoleur_encours.cod_agrem_vl as cod_agrem_controleur, " +
        //            "convert(char(12),p_contoleur_encours.date_effet_notification_vl,103) as controleur_date_notif_vl,   " +
        //            "convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
        //            "p_contoleur_encours.etat_agrement_vl,  " +
        //            "convert(char(1),p_contoleur_encours.habilitation_gaz_vl) as habilitation_gaz_vl,  " +

        //            //"CASE     " +
        //            //"    WHEN p_contoleur_encours.etat_agrement_vl = '0' and p_contoleur_encours.cod_agrem_vl = null then 'sans agrément'    " +
        //            //"    WHEN p_contoleur_encours.etat_agrement_vl = '0' and p_contoleur_encours.cod_agrem_vl <> null then 'annulé'    " +
        //            //"    WHEN p_contoleur_encours.etat_agrement_vl = '1' and (SELECT COUNT(*) FROM p_sanction_controleur WHERE p_sanction_controleur.id_controleur_encours = p_contoleur_encours.id_controleur_encours AND p_sanction_controleur.cod_type_sanction_cntlr in (2,3) AND getdate() BETWEEN p_sanction_controleur.date_debut AND p_sanction_controleur.date_fin ) = 0 then 'valide'     " +
        //            //"    WHEN p_contoleur_encours.etat_agrement_vl = '1' and (SELECT COUNT(*) FROM p_sanction_controleur WHERE p_sanction_controleur.id_controleur_encours = p_contoleur_encours.id_controleur_encours AND p_sanction_controleur.cod_type_sanction_cntlr in (2,3) AND getdate() BETWEEN p_sanction_controleur.date_debut AND p_sanction_controleur.date_fin ) > 0 then 'suspendu'     " +
        //            //"    WHEN p_contoleur_encours.etat_agrement_vl = '2' then 'demande en cours'     " +
        //            //"    WHEN p_contoleur_encours.etat_agrement_vl = '3' then 'rejeté'     " +
        //            //"    ELSE null    " +
        //            //"END as etat_agrement_vl,    " +

        //            //"CASE  " +
        //            //"    WHEN p_contoleur_encours.habilitation_gaz_vl = 0 then 'Non'  " +
        //            //"    WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then 'Oui'  " +
        //            //"    ELSE 'Non'    " +
        //            //"END as habilitation_gaz_vl,    " +

        //            "CASE  " +
        //            "    WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103)  " +
        //            "    ELSE null    " +
        //            "END as date_habilitation ,   " +
        //            "p_contoleur_encours.id_controleur_encours " +

        //            "FROM p_centre_encours , " +
        //            "    r_prefecture , " +
        //            "    p_contoleur_encours " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
        //            "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and " +
        //            "      ( p_contoleur_encours.etat_agrement_vl = '1' ) and  " +
        //            "      ( p_centre_encours.type_centre = 'S' ) " +
        //                   ((prefecture != null) ? " and ( p_centre_encours.id_prefecture = " + prefecture.Id + ") " : "") +
        //            "ORDER BY p_contoleur_encours.nom          ASC  " ;

        //    List<ItemListeDesControleursVLCentreRattachementSelonDepartement> stat = new List<ItemListeDesControleursVLCentreRattachementSelonDepartement>();

        //    var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesControleursVLCentreRattachementSelonDepartement item = new ItemListeDesControleursVLCentreRattachementSelonDepartement();
        //        item.IdPrefecture = (string)row[0];
        //        item.CodDepartement = (string)row[1];
        //        item.LibellePrefecture = (string)(row[2]);
        //        item.TypeCentre = (string)(row[3]);
        //        item.RaisonSociale = (string)(row[4]);
        //        item.CodeReseau = (string)(row[5]);
        //        item.DateEffetNotificationVl = (string)(row[6]);
        //        item.CodAgrementVl = (string)(row[7]);
        //        item.CpCentre = (string)(row[8]);
        //        item.Ville = (string)(row[9]);
        //        item.ControleurNom = (string)(row[10]);
        //        item.ControleurCodeAgrementVl = (string)(row[11]);
        //        item.ControleurDateEffetNotificationVl = (string)(row[12]);
        //        item.ControleurDateNaissance = (string)(row[13]);
        //        item.ControleurEtatAgrementVl = (string)(row[14]);
        //        item.ControleurHabilitationGaz = (string)(row[15]);
        //        item.ControleurDateHabilitationGaz = (string)(row[16]);
        //        item.Controleur = this.FindBy(Convert.ToInt32(row[17]));
        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        //public IList<ItemListeDesControleursVLCentreRattachementSelonRegion> listeDesControleursVLCentreRattachementSelonRegion(rRegion region, rPrefecture prefecture)
        //{
        //    string requeteSQL =
        //            "SELECT  convert(char(2),r_prefecture.id_prefecture) as id_prefecture , " +
        //            "r_prefecture.cod_departement , " +
        //            "r_prefecture.libelle_prefecture , " +
        //            "p_centre_encours.type_centre , " +
        //            "p_centre_encours.raison_sociale , " +
        //            "p_centre_encours.cod_reseau_vl , " +
        //            "convert(char(12),p_centre_encours.date_effet_notification_vl) as centre_date_notif_vl,   " +
        //            "p_centre_encours.cod_agrem_vl , " +
        //            "p_centre_encours.cp_centre , " +
        //            "p_centre_encours.ville , " +
        //            "p_contoleur_encours.nom+' '+p_contoleur_encours.prenom , " +
        //            "p_contoleur_encours.cod_agrem_vl , " +
        //            "convert(char(12),p_contoleur_encours.date_effet_notification_vl) as controleur_date_notif_vl,   " +
        //            "convert(char(12),p_contoleur_encours.date_naissance) as date_naissance,   " +
        //            "p_centre_encours.etat_agrement_vl " +
        //            "FROM p_centre_encours , " +
        //            "    r_prefecture , " +
        //            "    p_contoleur_encours , " +
        //            "    r_region   " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
        //            "      ( r_region.id_region = r_prefecture.id_region ) and     " +
        //            "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and " +
        //            "      ( p_contoleur_encours.etat_agrement_vl = '1' ) and  " +
        //            "      ( p_centre_encours.type_centre = 'S' ) " +
        //                   ((region != null) ? " AND ( r_region.id_region = " + region.Id + ") " : "") +
        //                   ((prefecture != null) ? " and ( p_centre_encours.id_prefecture = " + prefecture.Id + ") " : "") +
        //            "ORDER BY p_contoleur_encours.nom          ASC  ";

        //    List<ItemListeDesControleursVLCentreRattachementSelonRegion> stat = new List<ItemListeDesControleursVLCentreRattachementSelonRegion>();

        //    var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesControleursVLCentreRattachementSelonRegion item = new ItemListeDesControleursVLCentreRattachementSelonRegion();
        //        item.IdPrefecture = (string)row[0];
        //        item.CodDepartement = (string)row[1];
        //        item.LibellePrefecture = (string)(row[2]);
        //        item.TypeCentre = (string)(row[3]);
        //        item.RaisonSociale = (string)(row[4]);
        //        item.CodeReseau = (string)(row[5]);
        //        item.DateEffetNotificationVl = (string)(row[6]);
        //        item.CodAgrementVl = (string)(row[7]);
        //        item.CpCentre = (string)(row[8]);
        //        item.Ville = (string)(row[9]);
        //        item.ControleurNom = (string)(row[10]);
        //        item.ControleurCodeAgrementVl = (string)(row[11]);
        //        item.ControleurDateEffetNotificationVl = (string)(row[12]);
        //        item.ControleurDateNaissance = (string)(row[13]);
        //        item.ControleurEtatAgrementVl = (string)(row[14]);
        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        //public IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonDepartement(rPrefecture prefecture)
        //{
        //    string requeteSQL =
        //            "SELECT  convert(varchar(4),r_prefecture.id_prefecture) as id_prefecture , " +
        //            "r_prefecture.cod_departement , " +
        //            "r_prefecture.libelle_prefecture , " +
        //            "p_centre_encours.type_centre , " +
        //            "p_centre_encours.raison_sociale , " +
        //            "p_centre_encours.cod_reseau_pl , " +
        //            "p_centre_encours.cod_agrem_pl as centrecodeagrempl, " +
        //            "convert(char(12),p_centre_encours.date_effet_notification_pl,103) as centre_date_notif_pl,   " +
        //            "p_centre_encours.etat_agrement_pl , " +
        //            "p_centre_encours.cp_centre , " +
        //            "p_centre_encours.ville , " +
        //            "p_contoleur_encours.nom+' '+p_contoleur_encours.prenom , " +
        //            "p_contoleur_encours.cod_agrem_pl as controleurcodeagrem, " +
        //            "convert(char(12),p_contoleur_encours.date_effet_notification_pl,103) as controleur_date_notif_pl,   " +
        //            "convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
        //            "p_contoleur_encours.cod_habilitation " +
        //            "FROM p_centre_encours , " +
        //            "    r_prefecture , " +
        //            "    p_contoleur_encours " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
        //            "      ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and " +
        //            "      ( p_contoleur_encours.etat_agrement_pl = '1' ) and  " +
        //            "      ( p_centre_encours.type_centre = 'S' ) " +
        //                   ((prefecture != null) ? " and ( p_centre_encours.id_prefecture = " + prefecture.Id + ") " : "") +
        //            "ORDER BY p_contoleur_encours.nom          ASC  ";

        //    List<ItemListeDesControleursPLCentreRattachementSelonDepartement> stat = new List<ItemListeDesControleursPLCentreRattachementSelonDepartement>();

        //    var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesControleursPLCentreRattachementSelonDepartement item = new ItemListeDesControleursPLCentreRattachementSelonDepartement();
        //        item.IdPrefecture = (string)row[0];
        //        item.CodDepartement = (string)row[1];
        //        item.LibellePrefecture = (string)(row[2]);
        //        item.TypeCentre = (string)(row[3]);
        //        item.RaisonSociale = (string)(row[4]);
        //        item.CodeReseau = (string)(row[5]);
        //        item.CodAgrementPl = (string)(row[6]);
        //        item.DateEffetNotificationPl = (string)(row[7]);
        //        item.EtatAgrementPl = (string) (row[8]);
        //        item.CpCentre = (string)(row[9]);
        //        item.Ville = (string)(row[10]);
        //        item.ControleurNom = (string)(row[11]);
        //        item.ControleurCodeAgrementPl = (string)(row[12]);
        //        item.ControleurDateEffetNotificationPl = (string)(row[13]);
        //        item.ControleurDateNaissance = (string)(row[14]);
        //        item.ControleurCodeHabilitation = (string)(row[15]);
        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        //public List<NombreControleursParRegionEtDepartement> getNombreControleurParRegionEtDepartement(rRegion region)
        //    {
        //        string requete = "SELECT " +
        //                "r.libelle_region , p.cod_departement , " +
        //                "SUM(CASE WHEN cod_agrem_rattachement_pl IS NOT NULL THEN 1 ELSE 0 END) as nb_cntlr_pl  , " +
        //                "SUM(CASE WHEN cod_agrem_rattachement_vl IS NOT NULL THEN 1 ELSE 0 END) as nb_cntlr_vl, " +
        //                "SUM(CASE WHEN cod_agrem_rattachement_vl IS NOT NULL AND cod_agrem_rattachement_pl IS NOT NULL  THEN 1 ELSE 0 END) as nb_cntlr_pl_et_vl " +
        //                "FROM r_prefecture p , r_region r , p_contoleur_encours c  " +
        //                "WHERE p.id_region = r.id_region and c.cod_departement = p.cod_departement  " +
        //                ((region != null)?" AND r.id_region = " + region.Id : "") +
        //                " GROUP BY r.libelle_region , p.cod_departement " +
        //                " order by r.libelle_region ";

        //        List<NombreControleursParRegionEtDepartement> stat = new List<NombreControleursParRegionEtDepartement>();

        //        var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requete).List();
        //        foreach (object[] row in results)
        //        {
        //           NombreControleursParRegionEtDepartement item = new NombreControleursParRegionEtDepartement();
        //           item.LibelleRegion = (string)row[0];
        //           item.CodeDepartement = (string)row[1];
        //           item.NombreControleurPL = Convert.ToInt32(row[2]);
        //           item.NombreControleurVL = Convert.ToInt32(row[3]);
        //           item.NombreControleurPlEtVl  = Convert.ToInt32(row[4]);
        //           stat.Add(item);
        //        }
        //        return stat;
        //    }

        //EG le 20/12/2012
        public IList<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> listeDesInstallationsVéhiculesLegersELourdsSelonRegion(rRegion region, rUtilisateur utilisateur)
        {

            string sqlin = null;
            foreach (rReseau r in utilisateur.RReseaux)
            {
                if (sqlin != null)
                {
                    sqlin += ",";
                }
                sqlin += "'" + r.Id + "'";
            }

            string requeteSQL =
                     " SELECT p_centre_encours.raison_sociale as raison_sociale,   " +
                     "        p_centre_encours.l1_adresse as adresse_1,   " +
                     "        p_centre_encours.l2_adresse as adresse_2,   " +
                     "        p_centre_encours.cp_centre as code_postal,   " +
                     "        p_centre_encours.ville as ville,   " +
                     "        p_centre_encours.cod_agrem_pl as agrement,   " +
                     "        convert(char(12),p_centre_encours.date_effet_notification_pl,103) as date_effet_agrement,   " +
                     "        convert(char(12),p_centre_encours.date_echeance_agrement_pl,103) as date_echeance,   " +
                     "        p_centre_encours.cod_departement as code_departement,   " +
                     "        UPPER(p_centre_encours.cod_reseau_pl) as reseau,   " +
                     "        p_centre_encours.type_centre as type_inst_special_auxiliaire, " +
                     "       'PL' as categorie_vehicule, " +
                     "       null as cntlr_veh_gaz,    " +
                     "       CASE     " +
                     "          WHEN p_equipement_centre_pl.decelerometre = 1 then 'Oui'    " +
                     "          WHEN p_equipement_centre_pl.decelerometre = 0 then 'Non'     " +
                     "          ELSE null    " +
                     "       END as decelerometre,    " +
                     "       null as rouleau_fou,    " +
                     "       null as banc_rotation_inv,    " +
                    "        r_region.libelle_region as libelle_region,    " +
                    "       p_centre_encours.ext_agrem_cl_statut as  ext_agrem_cl_statut," +
                    "       p_centre_encours.ext_agrem_cl_date_valid as ext_agrem_cl_date_valid," +
                    "       p_centre_encours.ext_agrem_cl_cat_veh as ext_agrem_cl_cat_veh " +
                     "  FROM p_centre_encours,   " +
                     "       r_prefecture,   " +
                     "       r_region,  " +
                     "       p_equipement_centre_pl " +
                     " WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
                     "       ( r_region.id_region = r_prefecture.id_region ) and  " +
                     "       ( p_centre_encours.etat_agrement_pl in ('1','4','5') ) and " +
                     //"       ( p_centre_encours.cod_reseau_pl IN (" + sqlin + ")) and " +
                     "       ( p_centre_encours.id_centre_encours *= p_equipement_centre_pl.id_centre_encours ) " +
                             ((sqlin != null) ? " AND p_centre_encours.cod_reseau_pl IN (" + sqlin + ") and  p_centre_encours.acces_public_pl = 1 " : "") +
                             ((region != null) ? " AND r_region.id_region = " + region.Id : "") +

                     " UNION ALL " +

                     " SELECT p_centre_encours.raison_sociale as raison_sociale,   " +
                     "        p_centre_encours.l1_adresse as adresse_1,   " +
                     "        p_centre_encours.l2_adresse as adresse_2,   " +
                     "        p_centre_encours.cp_centre as code_postal,   " +
                     "        p_centre_encours.ville as ville,   " +
                     "        p_centre_encours.cod_agrem_vl as agrement,   " +
                     "        convert(char(12),p_centre_encours.date_effet_notification_vl,103) as date_effet_agrement,   " +
                     "        null as date_echeance,   " +
                     "        p_centre_encours.cod_departement as code_departement,   " +
                     "        UPPER(p_centre_encours.cod_reseau_vl) as reseau,   " +
                     "        p_centre_encours.type_centre as type_inst_special_auxiliaire, " +
                     "        'VL' as categorie_vehicule, " +
                     "        CASE     " +
                     "            WHEN p_equipement_centre_vl.cntlr_veh_gaz = 1 then 'Oui'    " +
                     "            WHEN p_equipement_centre_vl.cntlr_veh_gaz = 0 then 'Non'     " +
                     "            ELSE null    " +
                     "        END as cntlr_veh_gaz,    " +
                     "        CASE     " +
                     "            WHEN p_equipement_centre_vl.decelerometre = 1 then 'Oui'    " +
                     "            WHEN p_equipement_centre_vl.decelerometre = 0 then 'Non'     " +
                     "            ELSE null    " +
                     "        END as decelerometre,    " +
                     "        CASE     " +
                     "            WHEN p_equipement_centre_vl.rouleau_fou = 1 then 'Oui'    " +
                     "            WHEN p_equipement_centre_vl.rouleau_fou = 0 then 'Non'     " +
                     "            ELSE null    " +
                     "        END as rouleau_fou,    " +
                     "        CASE     " +
                     "            WHEN p_equipement_centre_vl.banc_rotation_inv = 1 then 'Oui'    " +
                     "            WHEN p_equipement_centre_vl.banc_rotation_inv = 0 then 'Non'     " +
                     "            ELSE null    " +
                     "        END as banc_rotation_inv,    " +
                    "         r_region.libelle_region as libelle_region,    " +
                     "       p_centre_encours.ext_agrem_cl_statut as  ext_agrem_cl_statut," +
                    "       p_centre_encours.ext_agrem_cl_date_valid as ext_agrem_cl_date_valid," +
                    "       p_centre_encours.ext_agrem_cl_cat_veh as ext_agrem_cl_cat_veh " +
                     "   FROM p_centre_encours,   " +
                     "        r_prefecture,   " +
                     "        r_region,  " +
                     "        p_equipement_centre_vl " +
                     "  WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
                     "        ( r_region.id_region = r_prefecture.id_region ) and  " +
                     "        ( p_centre_encours.etat_agrement_vl = '1' )     and  " +
                     //"        ( p_centre_encours.cod_reseau_vl IN (" + sqlin + ")) and " +
                     "        ( p_centre_encours.id_centre_encours *= p_equipement_centre_vl.id_centre_encours ) " +
                              ((sqlin != null) ? " AND p_centre_encours.cod_reseau_vl IN (" + sqlin + ") and  p_centre_encours.acces_public_vl = 1 " : "") +
                              ((region != null) ? " AND r_region.id_region = " + region.Id : "") +

                     " UNION ALL " +

                     " SELECT p_centre_encours.raison_sociale as raison_sociale,   " +
                     "        p_centre_encours.l1_adresse as adresse_1,   " +
                     "        p_centre_encours.l2_adresse as adresse_2,   " +
                     "        p_centre_encours.cp_centre as code_postal,   " +
                     "        p_centre_encours.ville as ville,   " +
                     "        p_centre_encours.cod_agrem_cl as agrement,   " +
                     "        convert(char(12),p_centre_encours.date_effet_notification_cl,103) as date_effet_agrement,   " +
                     "        null as date_echeance,   " +
                     "        p_centre_encours.cod_departement as code_departement,   " +
                     "        UPPER(p_centre_encours.cod_reseau_cl as reseau),   " +
                     "        p_centre_encours.type_centre as type_inst_special_auxiliaire, " +
                     "        'CL' as categorie_vehicule, " +
                     "        null as cntlr_veh_gaz,    " +
                     "        null as decelerometre,    " +
                     "        null as rouleau_fou,    " +
                     "        null as banc_rotation_inv,    " +
                    "         r_region.libelle_region as libelle_region,    " +
                      "       p_centre_encours.ext_agrem_cl_statut as  ext_agrem_cl_statut," +
                    "       p_centre_encours.ext_agrem_cl_date_valid as ext_agrem_cl_date_valid," +
                    "       p_centre_encours.ext_agrem_cl_cat_veh as ext_agrem_cl_cat_veh " +
                     "    FROM p_centre_encours,   " +
                     "        r_prefecture,   " +
                     "        r_region,  " +
                     "        p_equipement_centre_cl " +
                     "  WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
                     "        ( r_region.id_region = r_prefecture.id_region ) and  " +
                     "        ( p_centre_encours.etat_agrement_cl = '1'  )    and  " +
                     "        ( p_centre_encours.id_centre_encours *= p_equipement_centre_cl.id_centre_encours ) " +
                              ((sqlin != null) ? " AND p_centre_encours.cod_reseau_cl IN (" + sqlin + ") and  p_centre_encours.acces_public_cl = 1 " : "") +
                              ((region != null) ? " AND r_region.id_region = " + region.Id : "") +
                    " ORDER BY p_centre_encours.raison_sociale ASC, p_centre_encours.cp_centre ASC    ";

            List<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> stat = new List<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion item = new ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion();
                item.RaisonSociale = (string)row[0];
                item.L1Adresse = (string)row[1];
                item.L2Adresse = (string)(row[2]);
                item.CPCentre = (string)(row[3]);
                item.Ville = (string)(row[4]);
                item.Agrement = (string)(row[5]);
                item.DateEffetAgrement = (string)(row[6]);
                item.DateEcheance = (string)(row[7]);
                item.CodeDepartement = (string)(row[8]);
                item.Reseau = (string)(row[9]);
                item.TypeCentre = (string)(row[10]);
                item.CategorieVehicule = (string)(row[11]);
                item.ControlGaz = (string)(row[12]);
                item.Decelerometre = (string)(row[13]);
                item.RouleauFou = (string)(row[14]);
                item.BancRotationInversee = (string)(row[15]);
                item.Region = (string)(row[16]);

                //PLE 29/01/2024 Extension Agrement Catgorie L
                item.Centre = new pCentreEncour();
                item.Centre.ExtAgremClStatut = (short?)(row[17]);
                item.Centre.ExtAgremClDateValid = (DateTime?)(row[18]);
                item.Centre.ExtAgremClCatVeh = (short?)(row[19]);
                if (rCategorieVehicule.CategorieEnum.VL.ToString().Equals(item.CategorieVehicule))
                {
                    item.ExtAgremClStatut = item.Centre.ExtAgremClStatutVLAsString;
                    item.ExtAgremClDateValid = String.Format("{0:d}", item.Centre.ExtAgremClDateValidVLAsDate);
                }
                if (rCategorieVehicule.CategorieEnum.PL.ToString().Equals(item.CategorieVehicule))
                {
                    item.ExtAgremClStatut = item.Centre.ExtAgremClStatutPLAsString;
                    item.ExtAgremClDateValid = String.Format("{0:d}", item.Centre.ExtAgremClDateValidPLAsDate);
                }

                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonDepartement(rPrefecture prefecture)
        {
            string requeteSQL = "SELECT   " +
             "r_prefecture.cod_departement,   " +
             "r_prefecture.libelle_prefecture, " +
             "p_centre_encours.type_centre,    " +
             "p_centre_encours.raison_sociale,    " +
             "p_centre_encours.cod_reseau_pl,    " +
             "p_centre_encours.date_effet_notification_pl,    " +
             "p_centre_encours.date_echeance_agrement_pl,   " +
             "p_centre_encours.cod_agrem_pl,    " +
             "p_centre_encours.cp_centre,    " +
             "p_centre_encours.ville,   " +
             "CASE  " +
             "    WHEN p_equipement_centre_pl.decelerometre = 1 then 'oui'  " +
             "    ELSE 'non'    " +
             "END as decelerometre    " +
             "FROM p_centre_encours,    " +
             "p_equipement_centre_pl,   " +
             "r_prefecture    " +
             "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
             " ( p_centre_encours.etat_agrement_pl in ('1','4','5') ) and " +
             " ( r_prefecture.id_prefecture = " + prefecture.Id + " ) and " +
             " ( p_centre_encours.id_centre_encours *= p_equipement_centre_pl.id_centre_encours) " +
             "ORDER BY  p_centre_encours.raison_sociale   ";

            List<ItemListeDesInstallations> stat = new List<ItemListeDesInstallations>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesInstallations item = new ItemListeDesInstallations();
                item.CodeDepartement = (string)row[0];
                item.LibellePrefecture = (string)row[1];
                item.TypeCentre = (string)row[2];
                item.RaisonSociale = (string)row[3];
                item.Reseau = (string)row[4];
                item.DateAgrement = Convert.ToDateTime(row[5]);
                item.DateEcheance = Convert.ToDateTime(row[6]);
                item.Agrement = (string)row[7];
                item.CodePostale = (string)row[8];
                item.Ville = (string)row[9];
                item.Decelerometre = (string)row[10];

                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLegersSelonDepartement(rPrefecture prefecture)
        {
            string requeteSQL = "SELECT   " +
            "r_prefecture.cod_departement,   " +
            "r_prefecture.libelle_prefecture, " +
            "p_centre_encours.type_centre,    " +
            "p_centre_encours.raison_sociale,    " +
            "p_centre_encours.cod_reseau_vl,    " +
            "p_centre_encours.date_effet_notification_vl,    " +
            "p_centre_encours.cod_agrem_vl,    " +
            "p_centre_encours.cp_centre,    " +
            "p_centre_encours.ville,   " +
            "CASE     " +
            "    WHEN p_equipement_centre_vl.cntlr_veh_gaz = 1 then 'Oui'    " +
            "    WHEN p_equipement_centre_vl.cntlr_veh_gaz = 0 then 'Non'     " +
            "    ELSE null    " +
            "END as cntlr_veh_gaz,    " +
            "CASE     " +
            "    WHEN p_equipement_centre_vl.decelerometre = 1 then 'Oui'    " +
            "    WHEN p_equipement_centre_vl.decelerometre = 0 then 'Non'     " +
            "    ELSE null    " +
            "END as decelerometre,    " +
            "CASE     " +
            "    WHEN p_equipement_centre_vl.rouleau_fou = 1 then 'Oui'    " +
            "    WHEN p_equipement_centre_vl.rouleau_fou = 0 then 'Non'     " +
            "    ELSE null    " +
            "END as rouleau_fou,    " +
            "CASE     " +
            "    WHEN p_equipement_centre_vl.banc_rotation_inv = 1 then 'Oui'    " +
            "    WHEN p_equipement_centre_vl.banc_rotation_inv = 0 then 'Non'     " +
            "    ELSE null    " +
            "END as banc_rotation_inv    " +
            "FROM p_centre_encours,    " +
            "r_prefecture,    " +
            "p_equipement_centre_vl " +
            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
            " ( p_centre_encours.etat_agrement_vl in ('1','4','5') ) and " +
            " ( r_prefecture.id_prefecture = " + prefecture.Id + " ) and " +
            " ( p_centre_encours.id_centre_encours *= p_equipement_centre_vl.id_centre_encours ) " +
            "ORDER BY  p_centre_encours.raison_sociale   ";

            List<ItemListeDesInstallations> stat = new List<ItemListeDesInstallations>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesInstallations item = new ItemListeDesInstallations();
                item.CodeDepartement = (string)row[0];
                item.LibellePrefecture = (string)row[1];
                item.TypeCentre = (string)row[2];
                item.RaisonSociale = (string)row[3];
                item.Reseau = (string)row[4];
                //item.DateAgrement = Convert.ToDateTime(row[5]);
                item.DateAgrement = Convert.ToDateTime(row[5]);
                item.Agrement = (string)row[6];
                item.CodePostale = (string)row[7];
                item.Ville = (string)row[8];
                item.ControlGaz = (string)row[9];
                item.Decelerometre = (string)row[10];
                item.RouleauFou = (string)row[11];
                item.BancRotationInversee = (string)row[12];

                stat.Add(item);
            }
            return stat;
        }

        //public ItemListeSuiviQualificationPL FindItemListeSuiviQualificationPL(ItemListeSuiviQualificationPLQuery query)
        //{
        //    string requete = "select * from " +
        //            " (select p_contoleur_encours.nom as nom, p_contoleur_encours.prenom as prenom, p_contoleur_encours.date_naissance as date_naissance," +
        //            " p_contoleur_encours.cod_agrem_pl as code_agrement_controleur, p_contoleur_encours.cod_agrem_rattachement_pl as code_agrement_centre, r_prefecture.cod_departement as departement, r_region.libelle_region as region," +
        //            " p_contoleur_encours.date_debut_rattachement_pl date_rattachement_centre, p_contoleur_encours.cod_habilitation as niveau_habilitation ,p_contoleur_encours.date_debut_habilitation_pl as date_habilitation," +
        //            " p_centre_encours.cod_reseau_pl as code_reseau" +
        //            " from p_contoleur_encours, p_centre_encours, r_prefecture, r_region" +
        //            " where p_contoleur_encours.id_controleur_encours = " + query.Controleur.Id + " and " +
        //            " p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours and " +
        //            " p_centre_encours.id_prefecture = r_prefecture.id_prefecture and r_prefecture.id_region = r_region.id_region ) " +
        //            " tbl1," +
        //            " (select max(date_agrement) as date_agrement_controleur" +
        //            " from" +
        //            " (select p_contoleur_encours.date_effet_notification_pl as date_agrement" +
        //            " from p_contoleur_encours" +
        //            " where p_contoleur_encours.cod_type_demande_cntlr = 'NOTPL'  "+ 
        //            " and p_contoleur_encours.id_controleur_encours = " + query.Controleur.Id +
        //            " union all " +
        //            " select p_controleur_histo.date_effet_notification_pl as date_agrement " +   
        //            " from p_controleur_histo " +
        //            " where p_controleur_histo.cod_type_demande_cntlr = 'NOTPL' " +
        //            " and p_controleur_histo.id_controleur_histo = "+ query.Controleur.Id + ") tbl22) " +
        //            " tbl2, " +
        //            " (select sum(cumul_activite_q1) as nb_ct_MA_q1, sum(cumul_activite_q2) as nb_ct_TCP_q2, sum(cumul_activite_q3) as nb_ct_TMD_q3 " +
        //            " from p_cumul_activite_pl_controleur " +
        //            " where p_cumul_activite_pl_controleur.mois_activite between " + query.MoisDebut + " and " + query.MoisFin +
        //            " and p_cumul_activite_pl_controleur.id_controleur_encours = " + query.Controleur.Id + ") " + 
        //            " tbl3, " +
        //            " (select max(p_audit_controleur.date_audit) as date_dernier_audit_q1 " +
        //            " from p_audit_controleur, r_categorie_vehicule " +
        //            " where p_audit_controleur.id_controleur_encours = " + query.Controleur.Id + " and " +
        //            " p_audit_controleur.id_categorie = r_categorie_vehicule.id_categorie and " +
        //            " r_categorie_vehicule.cod_habilitation = 'Q1' and p_audit_controleur.resultat_audit = 1) " +
        //            " tbl4, " +
        //            " (select max(p_audit_controleur.date_audit) as date_dernier_audit_q2 " +
        //            " from p_audit_controleur, r_categorie_vehicule " +
        //            " where p_audit_controleur.id_controleur_encours = " + query.Controleur.Id + " and  " +
        //            " p_audit_controleur.id_categorie = r_categorie_vehicule.id_categorie and " +
        //            " r_categorie_vehicule.cod_habilitation = 'Q2' and p_audit_controleur.resultat_audit = 1)   " +
        //            " tbl5, " +
        //            " (select max(p_audit_controleur.date_audit) as date_dernier_audit_q3 " +
        //            " from p_audit_controleur, r_categorie_vehicule " +
        //            " where p_audit_controleur.id_controleur_encours = " + query.Controleur.Id + " and  " +
        //            " p_audit_controleur.id_categorie = r_categorie_vehicule.id_categorie and " +
        //            " r_categorie_vehicule.cod_habilitation = 'Q3' and p_audit_controleur.resultat_audit = 1)  " +
        //            " tbl6, "+
        //            " (select max(p_formation_controleur_pl.date_debut) as date_formation_q1 "+
        //            " from p_formation_controleur_pl, r_type_formation "+
        //            " where p_formation_controleur_pl.id_controleur_encours = " + query.Controleur.Id + " and " +
        //            " p_formation_controleur_pl.id_type_formation = r_type_formation.id_type_formation and " +
        //            " r_type_formation.cod_habilitation = 'Q1') " +
        //            " tbl7, " +
        //            " (select max(p_formation_controleur_pl.date_debut) as date_formation_q2 " +
        //            " from p_formation_controleur_pl, r_type_formation " +
        //            " where p_formation_controleur_pl.id_controleur_encours = " + query.Controleur.Id + " and " +
        //            " p_formation_controleur_pl.id_type_formation = r_type_formation.id_type_formation and " +
        //            " r_type_formation.cod_habilitation = 'Q2') " +
        //            " tbl8, " +
        //            " (select max(p_formation_controleur_pl.date_debut) as date_formation_q3 " +
        //            " from p_formation_controleur_pl, r_type_formation " +
        //            " where p_formation_controleur_pl.id_controleur_encours = " + query.Controleur.Id + " and " +
        //            " p_formation_controleur_pl.id_type_formation = r_type_formation.id_type_formation and " +
        //            " r_type_formation.cod_habilitation = 'Q3' ) " +
        //            " tbl9" ;

        //    var result = SessionFactory.GetCurrentSession().CreateSQLQuery(requete).List();
        //    if (result != null && result.Count > 0)
        //    {
        //        object[] row = (object[])result[0];
        //        if (row != null)
        //        {
        //            ItemListeSuiviQualificationPL item = new ItemListeSuiviQualificationPL();
        //            item.Nom = (string)row[0];
        //            item.Prenom = (string)row[1];
        //            item.DateNaissance = Convert.ToDateTime(row[2]);
        //            item.CodAgrement = Convert.ToString(row[3]);
        //            item.CodeAgrementCentre = Convert.ToString(row[4]);
        //            item.CodeDepartement = Convert.ToString(row[5]);
        //            item.LibelleRegion = Convert.ToString(row[6]);
        //            item.DateDebutRattachementCentre = Convert.ToDateTime(row[7]);
        //            item.CodeHabilitation = Convert.ToString(row[8]);
        //            item.DateDebutHabilitation = Convert.ToDateTime(row[9]);
        //            item.CodReseau = Convert.ToString(row[10]);
        //            item.DateAgrement = Convert.ToDateTime(row[11]);
        //            item.CumulActiviteQ1 = Convert.ToInt32(row[12]);
        //            item.CumulActiviteQ2 = Convert.ToInt32(row[13]);
        //            item.CumulActiviteQ3 = Convert.ToInt32(row[14]);
        //            item.DateDernierAuditQ1 = Convert.ToDateTime(row[15]);
        //            item.DateDernierAuditQ2 = Convert.ToDateTime(row[16]);
        //            item.DateDernierAuditQ3 = Convert.ToDateTime(row[17]);
        //            item.DateFormationQ1 = Convert.ToDateTime(row[18]);
        //            item.DateFormationQ2 = Convert.ToDateTime(row[19]);
        //            item.DateFormationQ3 = Convert.ToDateTime(row[20]);

        //            return item;
        //        }
        //    }

        //    return null;
        //}

        //EG le 16/01/2012
        public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(rRegion region, rPrefecture prefecture)
        {
            string requeteSQL = "SELECT   " +
             "r_prefecture.cod_departement,   " +
             "r_prefecture.libelle_prefecture, " +
             "p_centre_encours.type_centre,    " +
             "p_centre_encours.raison_sociale,    " +
             "p_centre_encours.cod_reseau_pl,    " +
             "p_centre_encours.date_effet_notification_pl,    " +
             "p_centre_encours.date_echeance_agrement_pl,   " +
             "p_centre_encours.cod_agrem_pl,    " +
             "p_centre_encours.cp_centre,    " +
             "p_centre_encours.ville   " +
             "FROM p_centre_encours,    " +
             "r_prefecture,    " +
             "r_region " +
             "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
             "( ( p_centre_encours.etat_agrement_pl in ('1','4','5')) ) and " +
             "( r_region.id_region = r_prefecture.id_region ) and " +
             "( r_region.id_region = " + region.Id + ") and " +
             " ( (r_prefecture.id_prefecture = " + prefecture.Id + ") )                                           " +
             "ORDER BY  p_centre_encours.raison_sociale   ";

            List<ItemListeDesInstallations> stat = new List<ItemListeDesInstallations>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesInstallations item = new ItemListeDesInstallations();
                item.CodeDepartement = (string)row[0];
                item.LibellePrefecture = (string)row[1];
                item.TypeCentre = (string)row[2];
                item.RaisonSociale = (string)row[3];
                item.Reseau = (string)row[4];
                item.DateAgrement = Convert.ToDateTime(row[5]);
                item.DateEcheance = Convert.ToDateTime(row[6]);
                item.Agrement = (string)row[7];
                item.CodePostale = (string)row[8];
                item.Ville = (string)row[9];

                stat.Add(item);
            }
            return stat;
        }

    }
}
