using System;
using System.Collections;
using System.Collections.Generic;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.BusinessObjects.statistiques;
using Utac.Rnc2.Contrats.Repositories.statistiques;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class StatistiquesControleurRepository : BaseRepository<pContoleurEncour>, IStatistiquesControleurRepository
    {

        public IList<ItemNombreDeControleurs> nombreDeControleursVehiculesLegersEtLourdsSelonRegion(rRegion region, rPrefecture prefecture)
        {
            string requeteSQL =
                    "select  nb_cntlr_vl_ratt, nb_cntlr_vl_non_ratt, nb_cntlr_pl_ratt, nb_cntlr_pl_non_ratt, nb_cntlr_vlpl_ratt, nb_cntlr_vlpl_non_ratt, r_prefecture.id_prefecture, r_prefecture.libelle_prefecture, r_prefecture.cod_departement, r_prefecture.id_region, r_region.libelle_region " +
                    "from " +
                    "    (SELECT cntr2.id_prefecture as id_2 ,   count(cntlr2.id_controleur_encours) as nb_cntlr_vl_ratt" +
                    "        FROM p_centre_encours as cntr2,   " +
                    "             p_contoleur_encours  as cntlr2 " +
                    "       WHERE " +
                    "             ( cntlr2.id_centre_vl = cntr2.id_centre_encours ) and  " +
                    "             ( cntlr2.etat_agrement_vl = '1' ) and (cntr2.cod_reseau_vl <> 'Z') " +
                    "     GROUP BY cntr2.id_prefecture) as count_2,  " +
                    "    (SELECT cntr1.id_prefecture id_1,   count(cntlr1.id_controleur_encours) as nb_cntlr_vl_non_ratt " +
                    "        FROM p_centre_encours  as cntr1, " +
                    "             p_contoleur_encours  as cntlr1 " +
                    "       WHERE  " +
                    "             ( cntlr1.id_centre_vl = cntr1.id_centre_encours ) and   " +
                    "             ( cntlr1.etat_agrement_vl = '1' ) and (cntr1.cod_reseau_vl = 'Z') " +
                    "     GROUP BY cntr1.id_prefecture) as count_1, " +
                    "    (SELECT cntr3.id_prefecture as id_3 ,   count(cntlr3.id_controleur_encours) as nb_cntlr_pl_ratt " +
                    "        FROM p_centre_encours as cntr3,    " +
                    "             p_contoleur_encours  as cntlr3 " +
                    "       WHERE  " +
                    "             ( cntlr3.id_centre_pl = cntr3.id_centre_encours ) and   " +
                    "             ( cntlr3.etat_agrement_pl = '1' ) and (cntr3.cod_reseau_pl <> 'Z') " +
                    "     GROUP BY cntr3.id_prefecture) as count_3,  " +
                    "    (SELECT cntr4.id_prefecture id_4,   count(cntlr4.id_controleur_encours) as nb_cntlr_pl_non_ratt " +
                    "        FROM p_centre_encours  as cntr4, " +
                    "             p_contoleur_encours  as cntlr4 " +
                    "       WHERE  " +
                    "             ( cntlr4.id_centre_pl = cntr4.id_centre_encours ) and   " +
                    "             ( cntlr4.etat_agrement_pl = '1' ) and (cntr4.cod_reseau_pl = 'Z') " +
                    "     GROUP BY cntr4.id_prefecture) as count_4, " +
                    "    (SELECT cntr5.id_prefecture as id_5 ,   count(cntlr5.id_controleur_encours) as nb_cntlr_vlpl_ratt " +
                    "        FROM p_centre_encours as cntr5,    " +
                    "             p_contoleur_encours  as cntlr5 " +
                    "       WHERE  " +
                    "             ( cntlr5.id_centre_pl = cntr5.id_centre_encours ) and   " +
                    "             ( cntlr5.etat_agrement_pl = '1' and cntr5.cod_reseau_pl <> 'Z') and ( cntlr5.etat_agrement_vl = '1' and cntr5.cod_reseau_vl <> 'Z')  " +
                    "     GROUP BY cntr5.id_prefecture) as count_5,  " +
                    "    (SELECT cntr6.id_prefecture as id_6 ,   count(cntlr6.id_controleur_encours) as nb_cntlr_vlpl_non_ratt " +
                    "        FROM p_centre_encours as cntr6,    " +
                    "             p_contoleur_encours  as cntlr6 " +
                    "       WHERE  " +
                    "             ( cntlr6.id_centre_pl = cntr6.id_centre_encours ) and   " +
                    "             ( cntlr6.etat_agrement_pl = '1' and cntr6.cod_reseau_pl = 'Z') and ( cntlr6.etat_agrement_vl = '1' and cntr6.cod_reseau_vl = 'Z')  " +
                    "     GROUP BY cntr6.id_prefecture) as count_6, " +
                    "    r_prefecture, r_region " +
                    "WHERE r_prefecture.id_prefecture *= count_1.id_1 and r_prefecture.id_prefecture *= count_2.id_2 and  " +
                    "      r_prefecture.id_prefecture *= count_3.id_3 and r_prefecture.id_prefecture *= count_4.id_4 and " +
                    "      r_prefecture.id_prefecture *= count_5.id_5 and r_prefecture.id_prefecture *= count_6.id_6 and " +
                    "      r_prefecture.id_region = r_region.id_region " +

                    ((prefecture != null) ? " AND ( r_prefecture.id_prefecture = " + prefecture.Id + ")" : "") +
                    ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "");

            List<ItemNombreDeControleurs> stat = new List<ItemNombreDeControleurs>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemNombreDeControleurs item = new ItemNombreDeControleurs();
                item.RattacheVl = Convert.ToString(row[0]);
                item.NonRattacheVl = Convert.ToString(row[1]);
                item.RattachePl = Convert.ToString(row[2]);
                item.NonRattachePl = Convert.ToString(row[3]);
                item.RattacheVlPl = Convert.ToString(row[4]);
                item.NonRattacheVlPl = Convert.ToString(row[5]);
                item.Prefecture = (string)(row[7]);
                item.CodeDepartement = (string)(row[8]);
                item.Region = (string)(row[10]);
                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesControleursVéhiculesLegersEtLourds> listeDesControleursVehiculesLegersEtLourdsSelonRegion(rRegion region)
        {

            string requeteSQL =
                    "SELECT p_contoleur_encours.nom, " +
                    "       p_contoleur_encours.prenom, " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,    " +
                    "       p_contoleur_encours.cod_agrem_pl as agrement_controleur,    " +
                    "       p_contoleur_encours.cod_habilitation as qualification_PL, " +
                    "       null as habilitation_gaz_vl, " +
                    "       null as date_declaration_habil_gaz_vl,     " +
                    "       convert(char(12),p_contoleur_encours.date_effet_notification_pl,103) as date_effet_notification,    " +
                    "       p_contoleur_encours.cod_agrem_rattachement_pl as agrement_centre_rattachement,    " +
                    "       p_centre_encours.cod_reseau_pl as code_reseau,    " +
                    "       r_prefecture.cod_departement as code_departement,    " +
                    "       'PL' as categorie_vehicule,   " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    "       p_centre_encours.raison_sociale,    " +
                    "       p_centre_encours.l1_adresse,    " +
                    "       p_centre_encours.l2_adresse,    " +
                    "       p_centre_encours.cp_centre,    " +
                    "       p_centre_encours.ville,   " +
                    "       convert(char(12),p_contoleur_encours.date_debut_habilitation_pl,103) as date_debut_habilitation_pl,   " +
                    "       r_region.libelle_region as libelle_region    " +
                    "FROM p_centre_encours,    " +
                    "     r_prefecture,    " +
                    "     p_contoleur_encours,    " +
                    "     r_region   " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
                    "      ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and   " +
                    "      ( r_region.id_region = r_prefecture.id_region ) and   " +
                    "      ( p_contoleur_encours.etat_agrement_pl = '1' )     " +
                            ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +

                    "UNION ALL    " +

                    "SELECT p_contoleur_encours.nom,       " +
                    "       p_contoleur_encours.prenom,       " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,       " +
                    "       p_contoleur_encours.cod_agrem_vl as agrement_controleur,       " +
                    "       '' as qualification_PL,    " +
                    "       convert(char(1),p_contoleur_encours.habilitation_gaz_vl) as habilitation_gaz_vl,   " +
                    //"       CASE     " +
                    //"           WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then 'Oui'    " +
                    //"           WHEN p_contoleur_encours.habilitation_gaz_vl = 0 then 'Non'     " +
                    //"           ELSE null    " +
                    //"       END as habilitation_gaz_vl,    " +
                    "       convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103) as date_declaration_habil_gaz_vl,         " +
                    "       convert(char(12),p_contoleur_encours.date_effet_notification_vl,103)as date_effet_notification,       " +
                    "       p_contoleur_encours.cod_agrem_rattachement_vl as agrement_centre_rattachement,       " +
                    "       p_centre_encours.cod_reseau_vl as code_reseau,       " +
                    "       r_prefecture.cod_departement as code_departement,       " +
                    "       'VL' as categorie_vehicule,      " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    "       p_centre_encours.raison_sociale,    " +
                    "       p_centre_encours.l1_adresse,    " +
                    "       p_centre_encours.l2_adresse,    " +
                    "       p_centre_encours.cp_centre,    " +
                    "       p_centre_encours.ville,   " +
                    "       null as date_debut_habilitation_pl,   " +
                    "       r_region.libelle_region as libelle_region    " +
                    "FROM p_centre_encours,       " +
                    "     r_prefecture,       " +
                    "     p_contoleur_encours,       " +
                    "     r_region      " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and      " +
                    "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and      " +
                    "      ( r_region.id_region = r_prefecture.id_region ) and      " +
                    "      ( p_contoleur_encours.etat_agrement_vl = '1' )     " +
                            ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +

                    "UNION ALL    " +

                    "SELECT p_contoleur_encours.nom,       " +
                    "       p_contoleur_encours.prenom,       " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,       " +
                    "       p_contoleur_encours.cod_agrem_cl as agrement_controleur,       " +
                    "       '' as qualification_PL,    " +
                    "       null as habilitation_gaz_vl, " +
                    "       null as date_declaration_habil_gaz_vl,     " +
                    "       convert(char(12),p_contoleur_encours.date_effet_notification_vl,103)as date_effet_notification,       " +
                    "       p_contoleur_encours.cod_agrem_rattachement_vl as agrement_centre_rattachement,       " +
                    "       UPPER(p_centre_encours.cod_reseau_cl) as code_reseau,       " +
                    "       r_prefecture.cod_departement as code_departement,       " +
                    "       'CL' as categorie_vehicule,      " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    "       p_centre_encours.raison_sociale,    " +
                    "       p_centre_encours.l1_adresse,    " +
                    "       p_centre_encours.l2_adresse,    " +
                    "       p_centre_encours.cp_centre,    " +
                    "       p_centre_encours.ville,   " +
                    "       null as date_debut_habilitation_pl,   " +
                    "       r_region.libelle_region as libelle_region    " +
                    "FROM p_centre_encours,       " +
                    "     r_prefecture,       " +
                    "     p_contoleur_encours,       " +
                    "     r_region      " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and      " +
                    "      ( p_contoleur_encours.id_centre_cl = p_centre_encours.id_centre_encours ) and      " +
                    "      ( r_region.id_region = r_prefecture.id_region ) and      " +
                    "      ( p_contoleur_encours.etat_agrement_cl = '1' )     " +
                            ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +

                    "order by p_contoleur_encours.nom,p_contoleur_encours.prenom    ";

            List<ItemListeDesControleursVéhiculesLegersEtLourds> stat = new List<ItemListeDesControleursVéhiculesLegersEtLourds>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleursVéhiculesLegersEtLourds item = new ItemListeDesControleursVéhiculesLegersEtLourds();
                item.Nom = (string)row[0];
                item.Prenom = (string)row[1];
                item.DateNaissance = (string)(row[2]);
                item.AgrementControleur = (string)(row[3]);
                item.QualificationPL = (string)(row[4]);
                item.HabilitationGazVl = (string)(row[5]);
                item.DateDeclarationHabilGazVl = (string)(row[6]);
                item.DateEffetNotification = (string)(row[7]);
                item.AgrementCentreRattachement = (string)(row[8]);
                item.CodeReseau = (string)(row[9]);
                item.CodeDepartement = (string)(row[10]);
                item.CategorieVehicule = (string)(row[11]);
                //PLE 03/08/2012 Amélioration des performances d'execution
                //item.Controleur = this.FindBy(Convert.ToInt32(row[12]));
                item.Controleur = new pContoleurEncour();
                item.Controleur.HabilitationGazVL = (row[5] == null ? null : new int?(Convert.ToInt32((string)(row[5]))));
                //PLE FIN 03/08/2012
                item.RaisonSociale = (string)(row[13]);
                item.Adresse1 = (string)(row[14]);
                item.Adresse2 = (string)(row[15]);
                item.CodePostal = (string)(row[16]);
                item.Ville = (string)(row[17]);
                item.DateQualificationPl = (string)(row[18]);
                item.Region = (string)(row[19]);
                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesControleurs> listeDesControleursVehiculesLourdsSelonRegion(rRegion region)
        {
            string requeteSQL =
                    "SELECT " +
                    "       r_region.libelle_region ,  " +
                    "       r_prefecture.cod_departement , " +
                    "       r_prefecture.libelle_prefecture ,  " +
                    "       p_contoleur_encours.nom ,  " +
                    "       p_contoleur_encours.prenom ,  " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance ,  " +
                    "       p_contoleur_encours.cod_agrem_pl ,  " +
                    "       convert(char(12),p_contoleur_encours.date_effet_notification_pl,103) as date_effet_notification_pl ,  " +
                    "       p_contoleur_encours.cod_agrem_rattachement_pl ,  " +
                    "       p_contoleur_encours.cod_habilitation as qualification_PL, " +
                    "       convert(char(12),p_contoleur_encours.date_debut_habilitation_pl,103) as date_debut_habilitation_pl ,  " +
                    "       p_centre_encours.raison_sociale,    " +
                    "       p_centre_encours.l1_adresse,    " +
                    "       p_centre_encours.l2_adresse,    " +
                    "       p_centre_encours.cp_centre,    " +
                    "       p_centre_encours.ville,   " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    //PLE 06/08/2012 Ajout du calcul de l'état agrément
                    "       CASE     " +
                    "           WHEN exists(select 1  from p_sanction_controleur  where ( p_sanction_controleur.id_controleur_encours = p_contoleur_encours.id_controleur_encours ) and  ( p_sanction_controleur.date_debut <= getdate() ) and  ( p_sanction_controleur.date_fin >= getdate() ) and  ( p_sanction_controleur.cod_type_sanction_cntlr in (2,3)) and( p_sanction_controleur.agrement_affecte_pl = 1)) " +
                    "           THEN 'Suspension'    " +
                    "           ELSE 'Valide'    " +
                    "       END as etat_agrement_cntlr_pl    " +
                    "FROM  " +
                    "       p_centre_encours ,  " +
                    "       r_prefecture ,  " +
                    "       r_region ,  " +
                    "       p_contoleur_encours  " +
                    "WHERE  " +
                    "       ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
                    "       ( r_region.id_region = r_prefecture.id_region ) and " +
                    "       ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and  " +
                    "       ( p_contoleur_encours.etat_agrement_pl = '1' )  " +
                            ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +
                    "ORDER BY p_contoleur_encours.nom , p_contoleur_encours.prenom  ";

            List<ItemListeDesControleurs> stat = new List<ItemListeDesControleurs>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleurs item = new ItemListeDesControleurs();
                item.Region = (string)(row[0]);
                item.CodeDepartement = (string)(row[1]);
                item.Prefecture = (string)(row[2]);
                item.Nom = (string)row[3];
                item.Prenom = (string)row[4];
                item.DateNaissance = (string)(row[5]);
                item.AgrementControleur = (string)(row[6]);
                item.DateEffetNotification = (string)(row[7]);
                item.AgrementCentreRattachement = (string)(row[8]);
                item.QualificationPL = (string)(row[9]);
                item.DateQualificationPl = (string)(row[10]);
                item.RaisonSociale = (string)(row[11]);
                item.Adresse1 = (string)(row[12]);
                item.Adresse2 = (string)(row[13]);
                item.CodePostal = (string)(row[14]);
                item.Ville = (string)(row[15]);
                //PLE 06/08/2012 Amélioration des performances d'execution : Appel inutil
                //item.Controleur = this.FindBy(Convert.ToInt32(row[16]));
                //PLE FIN 06/08/2012

                //PLE 06/08/2012 Ajout du calcul de l'état agrément.
                item.EtatAgrementCntlr = (string)(row[17]);
                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesControleurs> listeDesControleursVehiculesLegersSelonRegion(rRegion region)
        {

            string requeteSQL =
                    "SELECT " +
                    "       r_region.libelle_region ,  " +
                    "       r_prefecture.cod_departement , " +
                    "       r_prefecture.libelle_prefecture ,  " +
                    "       p_contoleur_encours.nom ,  " +
                    "       p_contoleur_encours.prenom ,  " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance ,  " +
                    "       p_contoleur_encours.cod_agrem_vl ,  " +
                    "       convert(char(12),p_contoleur_encours.date_effet_notification_vl,103) as date_effet_notification_vl ,  " +
                    "       p_contoleur_encours.cod_agrem_rattachement_vl ,  " +
                    "       convert(char(1),p_contoleur_encours.habilitation_gaz_vl) ,  " +
                    "       convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103) as date_debut_habilitation_gaz_vl ,  " +
                    "       p_centre_encours.raison_sociale,    " +
                    "       p_centre_encours.l1_adresse,    " +
                    "       p_centre_encours.l2_adresse,    " +
                    "       p_centre_encours.cp_centre,    " +
                    "       p_centre_encours.ville,   " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    //PLE 06/08/2012 Ajout du calcul de l'état agrément
                    "       CASE     " +
                    "           WHEN exists(select 1  from p_sanction_controleur  where ( p_sanction_controleur.id_controleur_encours = p_contoleur_encours.id_controleur_encours ) and  ( p_sanction_controleur.date_debut <= getdate() ) and  ( p_sanction_controleur.date_fin >= getdate() ) and  ( p_sanction_controleur.cod_type_sanction_cntlr in (2,3)) and( p_sanction_controleur.agrement_affecte_vl = 1)) " +
                    "           THEN 'Suspension'    " +
                    "           ELSE 'Valide'    " +
                    "       END as etat_agrement_cntlr_vl    " +
                    "FROM  " +
                    "       p_centre_encours ,  " +
                    "       r_prefecture ,  " +
                    "       r_region ,  " +
                    "       p_contoleur_encours  " +
                    "WHERE  " +
                    "       ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
                    "       ( r_region.id_region = r_prefecture.id_region ) and " +
                    "       ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and  " +
                    "       ( p_contoleur_encours.etat_agrement_vl = '1' )  " +
                            ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +
                    "ORDER BY p_contoleur_encours.nom , p_contoleur_encours.prenom  ";

            List<ItemListeDesControleurs> stat = new List<ItemListeDesControleurs>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleurs item = new ItemListeDesControleurs();
                item.Region = (string)(row[0]);
                item.CodeDepartement = (string)(row[1]);
                item.Prefecture = (string)(row[2]);
                item.Nom = (string)row[3];
                item.Prenom = (string)row[4];
                item.DateNaissance = (string)(row[5]);
                item.AgrementControleur = (string)(row[6]);
                item.DateEffetNotification = (string)(row[7]);
                item.AgrementCentreRattachement = (string)(row[8]);
                item.HabilitationGazVl = (string)(row[9]);
                item.DateDeclarationHabilGazVl = (string)(row[10]);
                item.RaisonSociale = (string)(row[11]);
                item.Adresse1 = (string)(row[12]);
                item.Adresse2 = (string)(row[13]);
                item.CodePostal = (string)(row[14]);
                item.Ville = (string)(row[15]);
                //PLE 03/08/2012 Amélioration des performances d'execution
                //item.Controleur = this.FindBy(Convert.ToInt32(row[16]));
                item.Controleur = new pContoleurEncour();
                item.Controleur.HabilitationGazVL = (row[9] == null ? null : new int?(Convert.ToInt32((string)(row[9]))));
                //PLE FIN 03/08/2012
                //PLE 06/08/2012 Ajout du calcul de l'état agrément.
                item.EtatAgrementCntlr = (string)(row[17]);

                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesControleursRattachesAuxCentresDuReseau> listeDesControleursRattachesAuxCentresDuReseau(rUtilisateur utilisateur)
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
                    "SELECT p_contoleur_encours.nom,  " +
                    "       p_contoleur_encours.prenom,    " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
                    "       p_contoleur_encours.cod_agrem_pl as agrement_controleur,   " +
                    "       p_contoleur_encours.cod_habilitation as qualification_PL," +
                    "       convert(char(12),p_contoleur_encours.date_debut_habilitation_pl,103)as date_debut_habilitation_pl," +
                    "       null as habilitation_gaz_vl," +
                    "       null as date_declaration_habil_gaz_vl, " +
                    "       convert(char(12),p_contoleur_encours.date_effet_notification_pl,103)as date_effet_notification,   " +
                    "       p_contoleur_encours.cod_agrem_rattachement_pl as agrement_centre_rattachement,   " +
                    "       UPPER(p_centre_encours.cod_reseau_pl) as code_reseau,   " +
                    "       r_prefecture.cod_departement as code_departement,   " +
                    "       'PL' as categorie_vehicule,  " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    "       p_contoleur_encours.ext_agrem_cl_statut as  ext_agrem_cl_statut," +
                    "       p_contoleur_encours.ext_agrem_cl_date_valid as ext_agrem_cl_date_valid," +
                    "       p_contoleur_encours.ext_agrem_cl_cat_veh as ext_agrem_cl_cat_veh " +
                    "FROM p_centre_encours,   " +
                    "     r_prefecture,   " +
                    "     p_contoleur_encours " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
                    "      ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and" +
                    "      ( p_contoleur_encours.etat_agrement_pl = '1' ) and " +
                    "      ( p_centre_encours.acces_public_pl = 1 ) and " +
                    "      ( p_centre_encours.cod_reseau_pl IN (" + sqlin + ")) " +

                    "UNION ALL " +

                    "SELECT p_contoleur_encours.nom,   " +
                    "       p_contoleur_encours.prenom,   " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
                    "       p_contoleur_encours.cod_agrem_vl as agrement_controleur,   " +
                    "       null as qualification_PL," +
                    "       null as date_debut_habilitation_pl," +
                    "       convert(char(1),p_contoleur_encours.habilitation_gaz_vl) as habilitation_gaz_vl,   " +
                    //"       CASE " +
                    //"          WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then 'Oui' " +
                    //"          WHEN p_contoleur_encours.habilitation_gaz_vl = 0 then 'Non' " +
                    //"          ELSE null" +
                    //"       END as habilitation_gaz_vl," +
                    "       convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103) as date_declaration_habil_gaz_vl,  " +
                    "       convert(char(12),p_contoleur_encours.date_effet_notification_vl,103)as date_effet_notification,   " +
                    "       p_contoleur_encours.cod_agrem_rattachement_vl as agrement_centre_rattachement,   " +
                    "       UPPER(p_centre_encours.cod_reseau_vl) as code_reseau,   " +
                    "       r_prefecture.cod_departement as code_departement,   " +
                    "       'VL' as categorie_vehicule,  " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    "       p_contoleur_encours.ext_agrem_cl_statut as  ext_agrem_cl_statut," +
                    "       p_contoleur_encours.ext_agrem_cl_date_valid as ext_agrem_cl_date_valid," +
                    "       p_contoleur_encours.ext_agrem_cl_cat_veh as ext_agrem_cl_cat_veh " +
                    "FROM p_centre_encours,   " +
                    "     r_prefecture,   " +
                    "     p_contoleur_encours     " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
                    "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and  " +
                    "      ( p_contoleur_encours.etat_agrement_vl = '1' ) and " +
                    "      ( p_centre_encours.acces_public_vl = 1 ) and " +
                    "      ( p_centre_encours.cod_reseau_vl IN (" + sqlin + ")) " +

                    "UNION ALL " +

                    "SELECT p_contoleur_encours.nom,   " +
                    "       p_contoleur_encours.prenom,   " +
                    "       convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
                    "       p_contoleur_encours.cod_agrem_cl as agrement_controleur,   " +
                    "       null as qualification_PL," +
                    "       null as date_debut_habilitation_pl," +
                    "       null as habilitation_gaz_vl," +
                    "       null as date_declaration_habil_gaz_vl,  " +
                    "       null as date_effet_notification,   " +
                    "       p_contoleur_encours.cod_agrem_rattachement_cl as agrement_centre_rattachement,   " +
                    "       UPPER(p_centre_encours.cod_reseau_cl) as code_reseau,   " +
                    "       r_prefecture.cod_departement as code_departement,   " +
                    "       'CL' as categorie_vehicule,  " +
                    "       p_contoleur_encours.id_controleur_encours, " +
                    "       p_contoleur_encours.ext_agrem_cl_statut as  ext_agrem_cl_statut," +
                    "       p_contoleur_encours.ext_agrem_cl_date_valid as ext_agrem_cl_date_valid," +
                    "       p_contoleur_encours.ext_agrem_cl_cat_veh as ext_agrem_cl_cat_veh " +
                    "FROM p_centre_encours,   " +
                    "     r_prefecture,   " +
                    "     p_contoleur_encours     " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
                    "      ( p_contoleur_encours.id_centre_cl = p_centre_encours.id_centre_encours ) and  " +
                    "      ( p_contoleur_encours.etat_agrement_cl = '1' ) and " +
                    "      ( p_centre_encours.acces_public_cl = 1 ) and " +
                    "      ( p_centre_encours.cod_reseau_cl IN (" + sqlin + ")) " +
                    "order by code_reseau, p_contoleur_encours.nom,p_contoleur_encours.prenom";

            List<ItemListeDesControleursRattachesAuxCentresDuReseau> stat = new List<ItemListeDesControleursRattachesAuxCentresDuReseau>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleursRattachesAuxCentresDuReseau item = new ItemListeDesControleursRattachesAuxCentresDuReseau();
                item.Nom = (string)row[0];
                item.Prenom = (string)row[1];
                item.DateNaissance = (string)(row[2]);
                item.AgrementControleur = (string)(row[3]);
                item.QualificationPL = (string)(row[4]);
                item.DateQualificationPl = (string)(row[5]);
                item.HabilitationGazVl = (string)(row[6]);
                item.DateDeclarationHabilGazVl = (string)(row[7]);
                item.DateEffetNotification = (string)(row[8]);
                item.AgrementCentreRattachement = (string)(row[9]);
                item.CodeReseau = (string)(row[10]);
                item.CodeDepartement = (string)(row[11]);
                item.CategorieVehicule = (string)(row[12]);
                //PLE 03/08/2012 Amélioration des performances d'execution
                //item.Controleur = this.FindBy(Convert.ToInt32(row[13]));
                item.Controleur = new pContoleurEncour();
                item.Controleur.HabilitationGazVL = (row[6] == null ? null : new int?(Convert.ToInt32((string)(row[6]))));
                //PLE 29/01/2024 Extension Agrement Catgorie L
                item.Controleur.ExtAgremClStatut = (short?)(row[14]);
                item.Controleur.ExtAgremClDateValid = (DateTime?)(row[15]);
                item.Controleur.ExtAgremClCatVeh = (short?)(row[16]);
                if (rCategorieVehicule.CategorieEnum.VL.ToString().Equals(item.CategorieVehicule))
                {
                    item.ExtAgremClStatut = item.Controleur.ExtAgremClStatutVLAsString;
                    item.ExtAgremClDateValid = String.Format("{0:d}", item.Controleur.ExtAgremClDateValidVLAsDate);
                }
                if (rCategorieVehicule.CategorieEnum.PL.ToString().Equals(item.CategorieVehicule))
                {
                    item.ExtAgremClStatut = item.Controleur.ExtAgremClStatutPLAsString;
                    item.ExtAgremClDateValid = String.Format("{0:d}", item.Controleur.ExtAgremClDateValidPLAsDate);
                }
                //PLE FIN 03/08/2012
                stat.Add(item);
            }
            return stat;
        }

        //public IList<ItemListeDesInstallationsVehiculesLegersSelonRegion> listeDesInstallationsVehiculesLegersSelonRegion(rRegion region)
        //{

        //    string requeteSQL =
        //            "SELECT convert(char(2),r_region.id_region) as id_region,   " +
        //            "r_region.libelle_region,   " +
        //            "convert(char(3),r_prefecture.id_prefecture) as id_prefecture,    " +
        //            "r_prefecture.cod_departement,   " +
        //            "r_prefecture.libelle_prefecture,   " +
        //            "p_centre_encours.type_centre,   " +
        //            "p_centre_encours.raison_sociale,   " +
        //            "p_centre_encours.cod_reseau_vl,   " +
        //            "convert(char(12),p_centre_encours.date_effet_notification_vl) as date_effet_notification_vl,   " +
        //            "p_centre_encours.cod_agrem_vl,   " +
        //            "p_centre_encours.cp_centre,   " +
        //            "p_centre_encours.ville   " +
        //            "FROM p_centre_encours,   " +
        //            "    r_prefecture,   " +
        //            "    r_region   " +
        //            "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
        //            "      ( r_region.id_region = r_prefecture.id_region ) and     " +
        //            "      ( p_centre_encours.etat_agrement_vl = '1' ) " +
        //                   ((region != null) ? " AND ( r_region.id_region = " + region.Id + ")" : "") +
        //            "ORDER BY  p_centre_encours.raison_sociale     ";

        //    List<ItemListeDesInstallationsVehiculesLegersSelonRegion> stat = new List<ItemListeDesInstallationsVehiculesLegersSelonRegion>();

        //     var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesInstallationsVehiculesLegersSelonRegion item = new ItemListeDesInstallationsVehiculesLegersSelonRegion();
        //        item.IdRegion = (string)row[0];
        //        item.LibelleRegion = (string)row[1];
        //        item.IdPrefecture = (string)(row[2]);
        //        item.CodDepartement = (string)(row[3]);
        //        item.LibellePrefecture = (string)(row[4]);
        //        item.TypeCentre = (string)(row[5]);
        //        item.RaisonSociale = (string)(row[6]);
        //        item.CodeReseau = (string)(row[7]);
        //        item.DateEffetNotification = (string)(row[8]);
        //        item.CodAgrement = (string)(row[9]);
        //        item.CpCentre = (string)(row[10]);
        //        item.Ville = (string)(row[11]);
        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        public IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonDepartement(rPrefecture prefecture)
        {
            string requeteSQL =
                    "SELECT  convert(varchar(4),r_prefecture.id_prefecture) as id_prefecture , " +
                    "r_prefecture.cod_departement , " +
                    "r_prefecture.libelle_prefecture , " +
                    "p_centre_encours.type_centre , " +
                    "p_centre_encours.raison_sociale , " +
                    "p_centre_encours.cod_reseau_vl , " +
                    "convert(char(12),p_centre_encours.date_effet_notification_vl,103) as centre_date_notif_vl,   " +
                    "p_centre_encours.cod_agrem_vl , " +
                    "p_centre_encours.cp_centre , " +
                    "p_centre_encours.ville , " +

                    "p_contoleur_encours.nom+' '+p_contoleur_encours.prenom , " +
                    "p_contoleur_encours.cod_agrem_vl as cod_agrem_controleur, " +
                    "convert(char(12),p_contoleur_encours.date_effet_notification_vl,103) as controleur_date_notif_vl,   " +
                    "convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
                    "p_contoleur_encours.etat_agrement_vl,  " +
                    "convert(char(1),p_contoleur_encours.habilitation_gaz_vl) as habilitation_gaz_vl,  " +

                    //"CASE     " +
                    //"    WHEN p_contoleur_encours.etat_agrement_vl = '0' and p_contoleur_encours.cod_agrem_vl = null then 'sans agrément'    " +
                    //"    WHEN p_contoleur_encours.etat_agrement_vl = '0' and p_contoleur_encours.cod_agrem_vl <> null then 'annulé'    " +
                    //"    WHEN p_contoleur_encours.etat_agrement_vl = '1' and (SELECT COUNT(*) FROM p_sanction_controleur WHERE p_sanction_controleur.id_controleur_encours = p_contoleur_encours.id_controleur_encours AND p_sanction_controleur.cod_type_sanction_cntlr in (2,3) AND getdate() BETWEEN p_sanction_controleur.date_debut AND p_sanction_controleur.date_fin ) = 0 then 'valide'     " +
                    //"    WHEN p_contoleur_encours.etat_agrement_vl = '1' and (SELECT COUNT(*) FROM p_sanction_controleur WHERE p_sanction_controleur.id_controleur_encours = p_contoleur_encours.id_controleur_encours AND p_sanction_controleur.cod_type_sanction_cntlr in (2,3) AND getdate() BETWEEN p_sanction_controleur.date_debut AND p_sanction_controleur.date_fin ) > 0 then 'suspendu'     " +
                    //"    WHEN p_contoleur_encours.etat_agrement_vl = '2' then 'demande en cours'     " +
                    //"    WHEN p_contoleur_encours.etat_agrement_vl = '3' then 'rejeté'     " +
                    //"    ELSE null    " +
                    //"END as etat_agrement_vl,    " +

                    //"CASE  " +
                    //"    WHEN p_contoleur_encours.habilitation_gaz_vl = 0 then 'Non'  " +
                    //"    WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then 'Oui'  " +
                    //"    ELSE 'Non'    " +
                    //"END as habilitation_gaz_vl,    " +

                    "CASE  " +
                    "    WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103)  " +
                    "    ELSE null    " +
                    "END as date_habilitation ,   " +
                    "p_contoleur_encours.id_controleur_encours " +

                    "FROM p_centre_encours , " +
                    "    r_prefecture , " +
                    "    p_contoleur_encours " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
                    "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and " +
                    "      ( p_contoleur_encours.etat_agrement_vl = '1' ) and  " +
                    "      ( p_centre_encours.type_centre = 'S' ) " +
                           ((prefecture != null) ? " and ( p_centre_encours.id_prefecture = " + prefecture.Id + ") " : "") +
                    "ORDER BY p_contoleur_encours.nom          ASC  ";

            List<ItemListeDesControleursVLCentreRattachementSelonDepartement> stat = new List<ItemListeDesControleursVLCentreRattachementSelonDepartement>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleursVLCentreRattachementSelonDepartement item = new ItemListeDesControleursVLCentreRattachementSelonDepartement();
                item.IdPrefecture = (string)row[0];
                item.CodDepartement = (string)row[1];
                item.LibellePrefecture = (string)(row[2]);
                item.TypeCentre = (string)(row[3]);
                item.RaisonSociale = (string)(row[4]);
                item.CodeReseau = (string)(row[5]);
                item.DateEffetNotificationVl = (string)(row[6]);
                item.CodAgrementVl = (string)(row[7]);
                item.CpCentre = (string)(row[8]);
                item.Ville = (string)(row[9]);
                item.ControleurNom = (string)(row[10]);
                item.ControleurCodeAgrementVl = (string)(row[11]);
                item.ControleurDateEffetNotificationVl = (string)(row[12]);
                item.ControleurDateNaissance = (string)(row[13]);
                item.ControleurEtatAgrementVl = (string)(row[14]);
                item.ControleurHabilitationGaz = (string)(row[15]);
                item.ControleurDateHabilitationGaz = (string)(row[16]);
                item.Controleur = this.FindBy(Convert.ToInt32(row[17]));
                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesControleursVLCentreRattachementSelonDepartement> listeDesControleursVLCentreRattachementSelonRegion(rRegion region, rPrefecture prefecture)
        {
            string requeteSQL =
                    "SELECT  convert(varchar(4),r_prefecture.id_prefecture) as id_prefecture , " +
                    "r_prefecture.cod_departement , " +
                    "r_prefecture.libelle_prefecture , " +
                    "p_centre_encours.type_centre , " +
                    "p_centre_encours.raison_sociale , " +
                    "p_centre_encours.cod_reseau_vl , " +
                    "convert(char(12),p_centre_encours.date_effet_notification_vl,103) as centre_date_notif_vl,   " +
                    "p_centre_encours.cod_agrem_vl as cod_agrem_vl_centre , " +
                    "p_centre_encours.cp_centre , " +
                    "p_centre_encours.ville , " +
                    "p_contoleur_encours.nom+' '+p_contoleur_encours.prenom , " +
                    "p_contoleur_encours.cod_agrem_vl as cod_agrem_vl_cntrl , " +
                    "convert(char(12),p_contoleur_encours.date_effet_notification_vl,103) as controleur_date_notif_vl,   " +
                    "convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
                    "p_centre_encours.etat_agrement_vl, " +
                    "r_region.libelle_region, " +
                    "convert(char(1),p_contoleur_encours.habilitation_gaz_vl) as habilitation_gaz_vl,   " +
                    "convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103) as date_debut_habilitation_gaz_vl,   " +
                    //"CASE  " +
                    //"    WHEN p_contoleur_encours.habilitation_gaz_vl = 1 then convert(char(12),p_contoleur_encours.date_debut_habilitation_gaz_vl,103)  " +
                    //"    ELSE null    " +
                    //"END as date_habilitation ,   " +
                    "p_contoleur_encours.id_controleur_encours, " +
                    "p_centre_encours.l1_adresse , " +
                    "p_centre_encours.l2_adresse  " +
                    "FROM p_centre_encours , " +
                    "    r_prefecture , " +
                    "    p_contoleur_encours , " +
                    "    r_region   " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
                    "      ( r_region.id_region = r_prefecture.id_region ) and     " +
                    "      ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and " +
                    "      ( p_contoleur_encours.etat_agrement_vl = '1' ) and  " +
                    "      ( p_centre_encours.type_centre = 'S' ) " +
                           ((region != null) ? " AND ( r_region.id_region = " + region.Id + ") " : "") +
                           ((prefecture != null) ? " and ( p_centre_encours.id_prefecture = " + prefecture.Id + ") " : "") +
                    "ORDER BY p_contoleur_encours.nom          ASC  ";

            List<ItemListeDesControleursVLCentreRattachementSelonDepartement> stat = new List<ItemListeDesControleursVLCentreRattachementSelonDepartement>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleursVLCentreRattachementSelonDepartement item = new ItemListeDesControleursVLCentreRattachementSelonDepartement();
                item.IdPrefecture = (string)row[0];
                item.CodDepartement = (string)row[1];
                item.LibellePrefecture = (string)(row[2]);
                item.TypeCentre = (string)(row[3]);
                item.RaisonSociale = (string)(row[4]);
                item.CodeReseau = (string)(row[5]);
                item.DateEffetNotificationVl = (string)(row[6]);
                item.CodAgrementVl = (string)(row[7]);
                item.CpCentre = (string)(row[8]);
                item.Ville = (string)(row[9]);
                item.ControleurNom = (string)(row[10]);
                item.ControleurCodeAgrementVl = (string)(row[11]);
                item.ControleurDateEffetNotificationVl = (string)(row[12]);
                item.ControleurDateNaissance = (string)(row[13]);
                item.ControleurEtatAgrementVl = (string)(row[14]);
                item.LibelleRegion = (string)row[15];
                item.ControleurHabilitationGaz = (string)(row[16]);
                item.ControleurDateHabilitationGaz = (string)(row[17]);
                item.Controleur = this.FindBy(Convert.ToInt32(row[18]));
                item.L1Adresse = (string)(row[19]);
                item.L2Adresse = (string)(row[20]);
                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonDepartement(rPrefecture prefecture)
        {
            string requeteSQL =
                    "SELECT  convert(varchar(4),r_prefecture.id_prefecture) as id_prefecture , " +
                    "r_prefecture.cod_departement , " +
                    "r_prefecture.libelle_prefecture , " +
                    "p_centre_encours.type_centre , " +
                    "p_centre_encours.raison_sociale , " +
                    "p_centre_encours.cod_reseau_pl , " +
                    "p_centre_encours.cod_agrem_pl as centrecodeagrempl, " +
                    "convert(char(12),p_centre_encours.date_effet_notification_pl,103) as centre_date_notif_pl,   " +
                    "p_centre_encours.etat_agrement_pl , " +
                    "p_centre_encours.cp_centre , " +
                    "p_centre_encours.ville , " +
                    "p_contoleur_encours.nom+' '+p_contoleur_encours.prenom , " +
                    "p_contoleur_encours.cod_agrem_pl as controleurcodeagrem, " +
                    "convert(char(12),p_contoleur_encours.date_effet_notification_pl,103) as controleur_date_notif_pl,   " +
                    "convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
                    "p_contoleur_encours.cod_habilitation, " +
                    "convert(char(12),p_contoleur_encours.date_debut_habilitation_pl,103) as date_debut_habilitation_pl   " +
                    "FROM p_centre_encours , " +
                    "    r_prefecture , " +
                    "    p_contoleur_encours " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
                    "      ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and " +
                    "      ( p_contoleur_encours.etat_agrement_pl = '1' ) and  " +
                    "      ( p_centre_encours.type_centre = 'S' ) " +
                           ((prefecture != null) ? " and ( p_centre_encours.id_prefecture = " + prefecture.Id + ") " : "") +
                    "ORDER BY p_contoleur_encours.nom          ASC  ";

            List<ItemListeDesControleursPLCentreRattachementSelonDepartement> stat = new List<ItemListeDesControleursPLCentreRattachementSelonDepartement>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleursPLCentreRattachementSelonDepartement item = new ItemListeDesControleursPLCentreRattachementSelonDepartement();
                item.IdPrefecture = (string)row[0];
                item.CodDepartement = (string)row[1];
                item.LibellePrefecture = (string)(row[2]);
                item.TypeCentre = (string)(row[3]);
                item.RaisonSociale = (string)(row[4]);
                item.CodeReseau = (string)(row[5]);
                item.CodAgrementPl = (string)(row[6]);
                item.DateEffetNotificationPl = (string)(row[7]);
                item.EtatAgrementPl = (string)(row[8]);
                item.CpCentre = (string)(row[9]);
                item.Ville = (string)(row[10]);
                item.ControleurNom = (string)(row[11]);
                item.ControleurCodeAgrementPl = (string)(row[12]);
                item.ControleurDateEffetNotificationPl = (string)(row[13]);
                item.ControleurDateNaissance = (string)(row[14]);
                item.ControleurCodeHabilitation = (string)(row[15]);
                item.ControleurDateHabilitation = (string)(row[16]);
                stat.Add(item);
            }
            return stat;
        }

        public IList<ItemListeDesControleursPLCentreRattachementSelonDepartement> listeDesControleursPLCentreRattachementSelonRegionDepartement(rRegion region, rPrefecture prefecture)
        {
            string requeteSQL =
                    "SELECT  convert(varchar(4),r_prefecture.id_prefecture) as id_prefecture , " +
                    "r_prefecture.cod_departement , " +
                    "r_prefecture.libelle_prefecture , " +
                    "p_centre_encours.type_centre , " +
                    "p_centre_encours.raison_sociale , " +
                    "p_centre_encours.cod_reseau_pl , " +
                    "p_centre_encours.cod_agrem_pl as centrecodeagrempl, " +
                    "convert(char(12),p_centre_encours.date_effet_notification_pl,103) as centre_date_notif_pl,   " +
                    "p_centre_encours.etat_agrement_pl , " +
                    "p_centre_encours.cp_centre , " +
                    "p_centre_encours.ville , " +
                    "p_contoleur_encours.nom+' '+p_contoleur_encours.prenom , " +
                    "p_contoleur_encours.cod_agrem_pl as controleurcodeagrem, " +
                    "convert(char(12),p_contoleur_encours.date_effet_notification_pl,103) as controleur_date_notif_pl,   " +
                    "convert(char(12),p_contoleur_encours.date_naissance,103) as date_naissance,   " +
                    "p_contoleur_encours.cod_habilitation, " +
                    "convert(char(12),p_contoleur_encours.date_debut_habilitation_pl,103) as date_debut_habilitation_pl,   " +
                    "r_region.libelle_region " +
                    "FROM p_centre_encours , " +
                    "    r_prefecture , " +
                    "    p_contoleur_encours, " +
                    "    r_region " +
                    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
                    "      ( p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours ) and " +
                    "      ( p_contoleur_encours.etat_agrement_pl = '1' ) and  " +
                    "      ( p_centre_encours.type_centre = 'S' )  " +
                           ((region != null) ? " and ( r_region.id_region = " + region.Id + ")  " : "") +
                           ((prefecture != null) ? " and ( p_centre_encours.id_prefecture = " + prefecture.Id + ") " : "") +
                    "ORDER BY p_contoleur_encours.nom          ASC  ";

            List<ItemListeDesControleursPLCentreRattachementSelonDepartement> stat = new List<ItemListeDesControleursPLCentreRattachementSelonDepartement>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeDesControleursPLCentreRattachementSelonDepartement item = new ItemListeDesControleursPLCentreRattachementSelonDepartement();
                item.IdPrefecture = (string)row[0];
                item.CodDepartement = (string)row[1];
                item.LibellePrefecture = (string)(row[2]);
                item.TypeCentre = (string)(row[3]);
                item.RaisonSociale = (string)(row[4]);
                item.CodeReseau = (string)(row[5]);
                item.CodAgrementPl = (string)(row[6]);
                item.DateEffetNotificationPl = (string)(row[7]);
                item.EtatAgrementPl = (string)(row[8]);
                item.CpCentre = (string)(row[9]);
                item.Ville = (string)(row[10]);
                item.ControleurNom = (string)(row[11]);
                item.ControleurCodeAgrementPl = (string)(row[12]);
                item.ControleurDateEffetNotificationPl = (string)(row[13]);
                item.ControleurDateNaissance = (string)(row[14]);
                item.ControleurCodeHabilitation = (string)(row[15]);
                item.ControleurDateHabilitation = (string)(row[16]);
                item.LibelleRegion = (string)row[17];
                stat.Add(item);
            }
            return stat;
        }

        public List<NombreControleursParRegionEtDepartement> getNombreControleurParRegionEtDepartement(rRegion region)
        {
            string requete = "SELECT " +
                    "r.libelle_region , p.cod_departement , " +
                    "SUM(CASE WHEN cod_agrem_rattachement_pl IS NOT NULL THEN 1 ELSE 0 END) as nb_cntlr_pl  , " +
                    "SUM(CASE WHEN cod_agrem_rattachement_vl IS NOT NULL THEN 1 ELSE 0 END) as nb_cntlr_vl, " +
                    "SUM(CASE WHEN cod_agrem_rattachement_vl IS NOT NULL AND cod_agrem_rattachement_pl IS NOT NULL  THEN 1 ELSE 0 END) as nb_cntlr_pl_et_vl " +
                    "FROM r_prefecture p , r_region r , p_contoleur_encours c  " +
                    "WHERE p.id_region = r.id_region and c.cod_departement = p.cod_departement  " +
                    ((region != null) ? " AND r.id_region = " + region.Id : "") +
                    " GROUP BY r.libelle_region , p.cod_departement " +
                    " order by r.libelle_region ";

            List<NombreControleursParRegionEtDepartement> stat = new List<NombreControleursParRegionEtDepartement>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requete).List();
            foreach (object[] row in results)
            {
                NombreControleursParRegionEtDepartement item = new NombreControleursParRegionEtDepartement();
                item.LibelleRegion = (string)row[0];
                item.CodeDepartement = (string)row[1];
                item.NombreControleurPL = Convert.ToInt32(row[2]);
                item.NombreControleurVL = Convert.ToInt32(row[3]);
                item.NombreControleurPlEtVl = Convert.ToInt32(row[4]);
                stat.Add(item);
            }
            return stat;
        }

        ////EG le 20/12/2012
        //public IList<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> listeDesInstallationsVéhiculesLegersELourdsSelonRegion(rRegion region)
        //{

        //    string requeteSQL = "SELECT p_centre_encours.raison_sociale as raison_sociale,   " +
        //             "p_centre_encours.l1_adresse as adresse_1,   " +
        //             "p_centre_encours.l2_adresse as adresse_2,   " +
        //             "p_centre_encours.cp_centre as code_postal,   " +
        //             "p_centre_encours.ville as ville,   " +
        //             "p_centre_encours.cod_agrem_pl as agrement,   " +
        //             "convert(char(12),p_centre_encours.date_effet_notification_pl,103) as date_effet_agrement,   " +
        //             "convert(char(12),p_centre_encours.date_echeance_agrement_pl,103) as date_echeance,   " +
        //             "p_centre_encours.cod_departement as code_departement,   " +
        //             "p_centre_encours.cod_reseau_pl as reseau,   " +
        //             "p_centre_encours.type_centre as type_inst_special_auxiliaire, " +
        //             " 'PL' as categorie_vehicule" +
        //             " FROM p_centre_encours,   " +
        //             "        r_prefecture,   " +
        //             "        r_region  " +
        //             "  WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
        //             "        ( r_region.id_region = r_prefecture.id_region ) and  " +
        //             "        ( ( p_centre_encours.etat_agrement_pl in ('1','4','5') ) )  " +
        //             ((region != null) ? " AND r_region.id_region = " + region.Id : "") +
        //             " UNION ALL " +
        //             " SELECT p_centre_encours.raison_sociale as raison_sociale,   " +
        //             "        p_centre_encours.l1_adresse as adresse_1,   " +
        //             "        p_centre_encours.l2_adresse as adresse_2,   " +
        //             "        p_centre_encours.cp_centre as code_postal,   " +
        //             "        p_centre_encours.ville as ville,   " +
        //             "        p_centre_encours.cod_agrem_vl as agrement,   " +
        //             "        convert(char(12),p_centre_encours.date_effet_notification_vl,103) as date_effet_agrement,   " +
        //             "        null as date_echeance,   " +
        //             "        p_centre_encours.cod_departement as code_departement,   " +
        //             "        p_centre_encours.cod_reseau_vl as reseau,   " +
        //             "        p_centre_encours.type_centre as type_inst_special_auxiliaire, " +
        //             "        'VL' as categorie_vehicule" +
        //             "    FROM p_centre_encours,   " +
        //             "        r_prefecture,   " +
        //             "        r_region  " +
        //             "  WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
        //             "        ( r_region.id_region = r_prefecture.id_region ) and  " +
        //             "        ( ( p_centre_encours.etat_agrement_vl = '1' ) ) " +
        //             ((region != null) ? " AND r_region.id_region = " + region.Id : "") +
        //             " UNION ALL " +
        //             " SELECT p_centre_encours.raison_sociale as raison_sociale,   " +
        //             "        p_centre_encours.l1_adresse as adresse_1,   " +
        //             "        p_centre_encours.l2_adresse as adresse_2,   " +
        //             "        p_centre_encours.cp_centre as code_postal,   " +
        //             "        p_centre_encours.ville as ville,   " +
        //             "        p_centre_encours.cod_agrem_cl as agrement,   " +
        //             "        convert(char(12),p_centre_encours.date_effet_notification_cl,103) as date_effet_agrement,   " +
        //             "        null as date_echeance,   " +
        //             "        p_centre_encours.cod_departement as code_departement,   " +
        //             "        p_centre_encours.cod_reseau_cl as reseau,   " +
        //             "        p_centre_encours.type_centre as type_inst_special_auxiliaire, " +
        //             "        'CL' as categorie_vehicule" +
        //             "    FROM p_centre_encours,   " +
        //             "        r_prefecture,   " +
        //             "        r_region  " +
        //             "  WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and  " +
        //             "        ( r_region.id_region = r_prefecture.id_region ) and  " +
        //             "        ( ( p_centre_encours.etat_agrement_cl = '1' ) ) " +
        //             ((region != null) ? " AND r_region.id_region = " + region.Id : "") +
        //            " ORDER BY p_centre_encours.raison_sociale ASC, p_centre_encours.cp_centre ASC    ";

        //    List<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion> stat = new List<ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion>();

        //     var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion item = new ItemListeDesInstallationsVéhiculesLegersELourdsSelonRegion();
        //        item.RaisonSociale = (string)row[0];
        //        item.L1Adresse = (string)row[1];
        //        item.L2Adresse = (string)(row[2]);
        //        item.CPCentre = (string)(row[3]);
        //        item.Ville = (string)(row[4]);
        //        item.Agrement = (string)(row[5]);
        //        item.DateEffetAgrement = (string)(row[6]);
        //        item.DateEcheance = (string)(row[7]);
        //        item.CodeDepartement = (string)(row[8]);
        //        item.Reseau = (string)(row[9]);
        //        item.TypeCentre = (string)(row[10]);
        //        item.CategorieVehicule = (string)(row[11]);
        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        //public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonDepartement(rPrefecture prefecture)
        //{
        //    string requeteSQL = "SELECT   " +
        //     "r_prefecture.cod_departement,   " +
        //     "r_prefecture.libelle_prefecture, " +
        //     "p_centre_encours.type_centre,    " +
        //     "p_centre_encours.raison_sociale,    " +
        //     "p_centre_encours.cod_reseau_pl,    " +
        //     "p_centre_encours.date_effet_notification_pl,    " +
        //     "p_centre_encours.cod_agrem_pl,    " +
        //     "p_centre_encours.cp_centre,    " +
        //     "p_centre_encours.ville,   " +
        //     "CASE  " +
        //     "    WHEN p_equipement_centre_pl.decelerometre = 1 then 'oui'  " +
        //     "    ELSE 'non'    " +
        //     "END as decelerometre    " +
        //     "FROM p_centre_encours,    " +
        //     "p_equipement_centre_pl,   " +
        //     "r_prefecture    " +
        //     "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
        //     " ( p_centre_encours.etat_agrement_pl in ('1','4','5') ) and " +
        //     " ( r_prefecture.id_prefecture = " + prefecture.Id + " ) and " +
        //     " ( p_centre_encours.id_centre_encours *= p_equipement_centre_pl.id_centre_encours) " +
        //     "ORDER BY  p_centre_encours.raison_sociale   ";

        //    List<ItemListeDesInstallations> stat = new List<ItemListeDesInstallations>();

        //     var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesInstallations item = new ItemListeDesInstallations();
        //        item.CodeDepartement = (string)row[0];
        //        item.LibellePrefecture = (string)row[1];
        //        item.TypeCentre = (string)row[2];
        //        item.RaisonSociale = (string)row[3];
        //        item.Reseau = (string)row[4];
        //        item.DateAgrement = Convert.ToDateTime(row[5]);
        //        item.Agrement = (string)row[6];
        //        item.CodePostale = (string)row[7];
        //        item.Ville = (string)row[8];
        //        item.Decelerometre = (string)row[9];

        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        //public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLegersSelonDepartement(rPrefecture prefecture)
        //{
        //    string requeteSQL = "SELECT   " +
        //    "r_prefecture.cod_departement,   " +
        //    "r_prefecture.libelle_prefecture, " +
        //    "p_centre_encours.type_centre,    " +
        //    "p_centre_encours.raison_sociale,    " +
        //    "p_centre_encours.cod_reseau_vl,    " +
        //    "p_centre_encours.date_effet_notification_vl,    " +
        //    "p_centre_encours.cod_agrem_vl,    " +
        //    "p_centre_encours.cp_centre,    " +
        //    "p_centre_encours.ville,   " +
        //    "CASE     " +
        //    "    WHEN p_equipement_centre_vl.cntlr_veh_gaz = 1 then 'Oui'    " +
        //    "    WHEN p_equipement_centre_vl.cntlr_veh_gaz = 0 then 'Non'     " +
        //    "    ELSE null    " +
        //    "END as cntlr_veh_gaz,    " +
        //    "CASE     " +
        //    "    WHEN p_equipement_centre_vl.decelerometre = 1 then 'Oui'    " +
        //    "    WHEN p_equipement_centre_vl.decelerometre = 0 then 'Non'     " +
        //    "    ELSE null    " +
        //    "END as decelerometre,    " +
        //    "CASE     " +
        //    "    WHEN p_equipement_centre_vl.rouleau_fou = 1 then 'Oui'    " +
        //    "    WHEN p_equipement_centre_vl.rouleau_fou = 0 then 'Non'     " +
        //    "    ELSE null    " +
        //    "END as rouleau_fou,    " +
        //    "CASE     " +
        //    "    WHEN p_equipement_centre_vl.banc_rotation_inv = 1 then 'Oui'    " +
        //    "    WHEN p_equipement_centre_vl.banc_rotation_inv = 0 then 'Non'     " +
        //    "    ELSE null    " +
        //    "END as banc_rotation_inv    " +
        //    "FROM p_centre_encours,    " +
        //    "r_prefecture,    " +
        //    "p_equipement_centre_vl " +
        //    "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
        //    " ( p_centre_encours.etat_agrement_vl in ('1','4','5') ) and " +
        //    " ( r_prefecture.id_prefecture = " + prefecture.Id + " ) and " +
        //    " ( p_centre_encours.id_centre_encours *= p_equipement_centre_vl.id_centre_encours ) " +
        //    "ORDER BY  p_centre_encours.raison_sociale   ";

        //    List<ItemListeDesInstallations> stat = new List<ItemListeDesInstallations>();

        //     var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesInstallations item = new ItemListeDesInstallations();
        //        item.CodeDepartement = (string)row[0];
        //        item.LibellePrefecture = (string)row[1];
        //        item.TypeCentre = (string)row[2];
        //        item.RaisonSociale = (string)row[3];
        //        item.Reseau = (string)row[4];
        //        item.DateAgrement = Convert.ToDateTime(row[5]);
        //        item.Agrement = (string)row[6];
        //        item.CodePostale = (string)row[7];
        //        item.Ville = (string)row[8];
        //        item.ControlGaz = (string)row[9];
        //        item.Decelerometre = (string)row[10];
        //        item.RouleauFou = (string)row[11];
        //        item.BancRotationInversee = (string)row[12];

        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        public ItemListeSuiviQualificationPL FindItemListeSuiviQualificationPL(ItemListeSuiviQualificationPLQuery query)
        {
            string requete = "select * from " +
                    " (select p_centre_encours.cp_centre as codepostal, p_centre_encours.ville as ville,p_contoleur_encours.nom as nom, p_contoleur_encours.prenom as prenom, p_contoleur_encours.date_naissance as date_naissance," +
                    " p_contoleur_encours.cod_agrem_pl as code_agrement_controleur, p_contoleur_encours.cod_agrem_rattachement_pl as code_agrement_centre, r_prefecture.cod_departement as departement, r_region.libelle_region as region," +
                    " p_contoleur_encours.date_debut_rattachement_pl date_rattachement_centre, p_contoleur_encours.cod_habilitation as niveau_habilitation ,p_contoleur_encours.date_debut_habilitation_pl as date_habilitation," +
                    " p_centre_encours.cod_reseau_pl as code_reseau" +
                    " from p_contoleur_encours, p_centre_encours, r_prefecture, r_region" +
                    " where p_contoleur_encours.id_controleur_encours = " + query.Controleur.Id + " and " +
                    " p_contoleur_encours.id_centre_pl = p_centre_encours.id_centre_encours and " +
                    " p_centre_encours.id_prefecture = r_prefecture.id_prefecture and r_prefecture.id_region = r_region.id_region ) " +
                    " tbl1," +
                    " (select max(date_agrement) as date_agrement_controleur" +
                    " from" +
                    " (select p_contoleur_encours.date_effet_notification_pl as date_agrement" +
                    " from p_contoleur_encours" +
                    " where p_contoleur_encours.cod_type_demande_cntlr = 'NOTPL'  " +
                    " and p_contoleur_encours.id_controleur_encours = " + query.Controleur.Id +
                    " union all " +
                    " select p_controleur_histo.date_effet_notification_pl as date_agrement " +
                    " from p_controleur_histo " +
                    " where p_controleur_histo.cod_type_demande_cntlr = 'NOTPL' " +
                    " and p_controleur_histo.id_controleur_histo = " + query.Controleur.Id + ") tbl22) " +
                    " tbl2, " +
                    " (select sum(cumul_activite_q1) as nb_ct_MA_q1, sum(cumul_activite_q2) as nb_ct_TCP_q2, sum(cumul_activite_q3) as nb_ct_TMD_q3 " +
                    " from p_cumul_activite_pl_controleur " +
                    " where p_cumul_activite_pl_controleur.mois_activite between " + query.MoisDebut + " and " + query.MoisFin +
                    " and p_cumul_activite_pl_controleur.id_controleur_encours = " + query.Controleur.Id + ") " +
                    " tbl3, " +
                    " (select max(p_audit_controleur.date_audit) as date_dernier_audit_q1 " +
                    " from p_audit_controleur, r_categorie_vehicule " +
                    " where p_audit_controleur.id_controleur_encours = " + query.Controleur.Id + " and " +
                    " p_audit_controleur.id_categorie = r_categorie_vehicule.id_categorie and " +
                    " r_categorie_vehicule.cod_habilitation = 'Q1' and p_audit_controleur.resultat_audit = 1) " +
                    " tbl4, " +
                    " (select max(p_audit_controleur.date_audit) as date_dernier_audit_q2 " +
                    " from p_audit_controleur, r_categorie_vehicule " +
                    " where p_audit_controleur.id_controleur_encours = " + query.Controleur.Id + " and  " +
                    " p_audit_controleur.id_categorie = r_categorie_vehicule.id_categorie and " +
                    " r_categorie_vehicule.cod_habilitation = 'Q2' and p_audit_controleur.resultat_audit = 1)   " +
                    " tbl5, " +
                    " (select max(p_audit_controleur.date_audit) as date_dernier_audit_q3 " +
                    " from p_audit_controleur, r_categorie_vehicule " +
                    " where p_audit_controleur.id_controleur_encours = " + query.Controleur.Id + " and  " +
                    " p_audit_controleur.id_categorie = r_categorie_vehicule.id_categorie and " +
                    " r_categorie_vehicule.cod_habilitation = 'Q3' and p_audit_controleur.resultat_audit = 1)  " +
                    " tbl6, " +
                    " (select max(p_formation_controleur_pl.date_debut) as date_formation_q1 " +
                    " from p_formation_controleur_pl, r_type_formation " +
                    " where p_formation_controleur_pl.id_controleur_encours = " + query.Controleur.Id + " and " +
                    " p_formation_controleur_pl.id_type_formation = r_type_formation.id_type_formation and " +
                    " r_type_formation.cod_habilitation = 'Q1') " +
                    " tbl7, " +
                    " (select max(p_formation_controleur_pl.date_debut) as date_formation_q2 " +
                    " from p_formation_controleur_pl, r_type_formation " +
                    " where p_formation_controleur_pl.id_controleur_encours = " + query.Controleur.Id + " and " +
                    " p_formation_controleur_pl.id_type_formation = r_type_formation.id_type_formation and " +
                    " r_type_formation.cod_habilitation = 'Q2') " +
                    " tbl8, " +
                    " (select max(p_formation_controleur_pl.date_debut) as date_formation_q3 " +
                    " from p_formation_controleur_pl, r_type_formation " +
                    " where p_formation_controleur_pl.id_controleur_encours = " + query.Controleur.Id + " and " +
                    " p_formation_controleur_pl.id_type_formation = r_type_formation.id_type_formation and " +
                    " r_type_formation.cod_habilitation = 'Q3' ) " +
                    " tbl9, " +
                    //27/08/2018 -PLE- Suivi qualification contrôleur PL
                    " (select sum(cumul_activite_q1) as nb_ct_MA_q1_annee, sum(cumul_activite_q2) as nb_ct_TCP_q2_annee, sum(cumul_activite_q3) as nb_ct_TMD_q3_annee " +
                    " from p_cumul_activite_pl_controleur " +
                    " where p_cumul_activite_pl_controleur.mois_activite between " + query.annee_n_moins1Debut + " and " + query.annee_n_moins1Fin +
                    " and p_cumul_activite_pl_controleur.id_controleur_encours = " + query.Controleur.Id + ") " +
                    " tbl20 ";

            var result = SessionFactory.GetCurrentSession().CreateSQLQuery(requete).List();
            if (result != null && result.Count > 0)
            {
                object[] row = (object[])result[0];
                if (row != null)
                {
                    ItemListeSuiviQualificationPL item = new ItemListeSuiviQualificationPL();
                    item.CodePostal = (string)row[0];
                    item.Ville = (string)row[1];
                    item.Nom = (string)row[2];
                    item.Prenom = (string)row[3];
                    item.DateNaissance = Convert.ToDateTime(row[4]);
                    item.CodAgrement = Convert.ToString(row[5]);
                    item.CodeAgrementCentre = Convert.ToString(row[6]);
                    item.CodeDepartement = Convert.ToString(row[7]);
                    item.LibelleRegion = Convert.ToString(row[8]);
                    item.DateDebutRattachementCentre = Convert.ToDateTime(row[9]);
                    item.CodeHabilitation = Convert.ToString(row[10]);
                    item.DateDebutHabilitation = Convert.ToDateTime(row[11]);
                    item.CodReseau = Convert.ToString(row[12]);
                    item.DateAgrement = Convert.ToDateTime(row[13]);
                    item.CumulActiviteQ1 = Convert.ToInt32(row[14]);
                    item.CumulActiviteQ2 = Convert.ToInt32(row[15]);
                    item.CumulActiviteQ3 = Convert.ToInt32(row[16]);
                    item.DateDernierAuditQ1 = Convert.ToDateTime(row[17]);
                    item.DateDernierAuditQ2 = Convert.ToDateTime(row[18]);
                    item.DateDernierAuditQ3 = Convert.ToDateTime(row[19]);
                    item.DateFormationQ1 = Convert.ToDateTime(row[20]);
                    item.DateFormationQ2 = Convert.ToDateTime(row[21]);
                    item.DateFormationQ3 = Convert.ToDateTime(row[22]);
                    item.CumulActiviteQ1AnneeMoins1 = Convert.ToInt32(row[23]);
                    item.CumulActiviteQ2AnneeMoins1 = Convert.ToInt32(row[24]);
                    item.CumulActiviteQ3AnneeMoins1 = Convert.ToInt32(row[25]);
                    item.DateSuiviQualification = Convert.ToDateTime(query.DateMoisFin);
                    return item;
                }
            }

            return null;

        }

        //YM 24/05/2013
        public ItemListeSuiviQualificationVL FindItemListeSuiviQualificationVL(ItemListeSuiviQualificationVLQuery query)
        {
            string requete = "select * from " +
                    " (select p_centre_encours.cp_centre as codepostal, p_centre_encours.ville as ville,p_contoleur_encours.nom as nom, p_contoleur_encours.prenom as prenom, p_contoleur_encours.date_naissance as date_naissance," +
                    " p_contoleur_encours.cod_agrem_vl as code_agrement_controleur, p_contoleur_encours.cod_agrem_rattachement_vl as code_agrement_centre, r_prefecture.cod_departement as departement, r_region.libelle_region as region," +
                    " p_contoleur_encours.date_debut_rattachement_vl date_rattachement_centre, p_contoleur_encours.habilitation_gaz_vl as habilitation_gaz ,p_contoleur_encours.date_debut_habilitation_gaz_vl as date_habilitation_gaz," +
                    " p_centre_encours.cod_reseau_vl as code_reseau" +
                    " from p_contoleur_encours, p_centre_encours, r_prefecture, r_region" +
                    " where p_contoleur_encours.id_controleur_encours = " + query.Controleur.Id + " and " +
                    " p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours and " +
                    " p_centre_encours.id_prefecture = r_prefecture.id_prefecture and r_prefecture.id_region = r_region.id_region ) " +
                    " tbl1," +
                    " (select max(date_agrement) as date_agrement_controleur" +
                    " from" +
                    " (select p_contoleur_encours.date_effet_notification_vl as date_agrement" +
                    " from p_contoleur_encours" +
                    " where p_contoleur_encours.cod_type_demande_cntlr = 'NOTVL'  " +
                    " and p_contoleur_encours.id_controleur_encours = " + query.Controleur.Id +
                    " union all " +
                    " select p_controleur_histo.date_effet_notification_vl as date_agrement " +
                    " from p_controleur_histo " +
                    " where p_controleur_histo.cod_type_demande_cntlr = 'NOTVL' " +
                    " and p_controleur_histo.id_controleur_histo = " + query.Controleur.Id + ") tbl22) " +
                    " tbl2 ";

            var result = SessionFactory.GetCurrentSession().CreateSQLQuery(requete).List();
            if (result != null && result.Count > 0)
            {
                object[] row = (object[])result[0];
                if (row != null)
                {
                    ItemListeSuiviQualificationVL item = new ItemListeSuiviQualificationVL();
                    item.CodePostal = (string)row[0];
                    item.Ville = (string)row[1];
                    item.Nom = (string)row[2];
                    item.Prenom = (string)row[3];
                    item.DateNaissance = Convert.ToDateTime(row[4]);
                    item.CodAgrement = Convert.ToString(row[5]);
                    item.CodeAgrementCentre = Convert.ToString(row[6]);
                    item.CodeDepartement = Convert.ToString(row[7]);
                    item.LibelleRegion = Convert.ToString(row[8]);
                    item.DateDebutRattachementCentre = Convert.ToDateTime(row[9]);
                    item.CodeHabilitation = Convert.ToString(row[10]);
                    item.DateDebutHabilitation = Convert.ToDateTime(row[11]);
                    item.CodReseau = Convert.ToString(row[12]);
                    item.DateAgrement = Convert.ToDateTime(row[13]);

                    return item;
                }
            }

            return null;

        }

        ////EG le 16/01/2012
        //public IList<ItemListeDesInstallations> ListeDesInstallationsVehiculesLourdsSelonRegionDepartement(rRegion region, rPrefecture prefecture)
        //{
        //    string requeteSQL = "SELECT   " +
        //     "r_prefecture.cod_departement,   " +
        //     "r_prefecture.libelle_prefecture, " +
        //     "p_centre_encours.type_centre,    " +
        //     "p_centre_encours.raison_sociale,    " +
        //     "p_centre_encours.cod_reseau_pl,    " +
        //     "p_centre_encours.date_effet_notification_pl,    " +
        //     "p_centre_encours.cod_agrem_pl,    " +
        //     "p_centre_encours.cp_centre,    " +
        //     "p_centre_encours.ville   " +
        //     "FROM p_centre_encours,    " +
        //     "r_prefecture,    " +
        //     "r_region " +
        //     "WHERE ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and   " +
        //     "( ( p_centre_encours.etat_agrement_pl in ('1','4','5')) ) and " +
        //     "( r_region.id_region = r_prefecture.id_region ) and " +
        //     "( r_region.id_region = " + region.Id +  ") and " +
        //     " ( (r_prefecture.id_prefecture = " + prefecture.Id + ") )                                           " +
        //     "ORDER BY  p_centre_encours.raison_sociale   ";

        //    List<ItemListeDesInstallations> stat = new List<ItemListeDesInstallations>();

        //     var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
        //    foreach (object[] row in results)
        //    {
        //        ItemListeDesInstallations item = new ItemListeDesInstallations();
        //        item.CodeDepartement = (string)row[0];
        //        item.LibellePrefecture = (string)row[1];
        //        item.TypeCentre = (string)row[2];
        //        item.RaisonSociale = (string)row[3];
        //        item.Reseau = (string)row[4];
        //        item.DateAgrement = Convert.ToDateTime(row[5]);
        //        item.Agrement = (string)row[6];
        //        item.CodePostale = (string)row[7];
        //        item.Ville = (string)row[8];

        //        stat.Add(item);
        //    }
        //    return stat;
        //}

        //YMA 15/10/2013
        public IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLSelonRegion(rRegion region)
        {

            string requeteSQL =
                    "SELECT " +
                    "       p_activite_controleur_vl.cod_reseau_rattachement_vl , " +
                    "       p_activite_controleur_vl.nom , " +
                    "       p_activite_controleur_vl.prenom , " +
                    "       p_activite_controleur_vl.cod_agrem_cntlr , " +
                    "       p_activite_controleur_vl.cod_agrem_centre , " +
                    "       p_activite_controleur_vl.cod_departement , " +
                    "       p_activite_controleur_vl.region , " +
                    "       convert(char(12),p_activite_controleur_vl.date_effet_notification_vl,103) as date_effet_notification_vl ,  " +
                    "       convert(char(12),p_activite_controleur_vl.date_fin_agrement_vl,103) as date_fin_agrement_vl ,  " +
                    "       convert(char(12),p_activite_controleur_vl.date_debut_rattachement_vl,103) as date_debut_rattachement_vl ,  " +
                    "       convert(char(12),p_activite_controleur_vl.date_dernier_ctrl,103) as date_dernier_ctrl ,  " +
                    "       p_activite_controleur_vl.activite_janvier_n1 , " +
                    "       p_activite_controleur_vl.activite_fevrier_n1 , " +
                    "       p_activite_controleur_vl.activite_mars_n1 , " +
                    "       p_activite_controleur_vl.activite_avril_n1 , " +
                    "       p_activite_controleur_vl.activite_mai_n1 , " +
                    "       p_activite_controleur_vl.activite_juin_n1 , " +
                    "       p_activite_controleur_vl.activite_juillet_n1 , " +
                    "       p_activite_controleur_vl.activite_aout_n1 , " +
                    "       p_activite_controleur_vl.activite_septembre_n1 , " +
                    "       p_activite_controleur_vl.activite_octobre_n1 , " +
                    "       p_activite_controleur_vl.activite_novembre_n1 , " +
                    "       p_activite_controleur_vl.activite_decembre_n1 , " +
                    "       p_activite_controleur_vl.total_vtp_n1 , " +
                    "       p_activite_controleur_vl.seuil_n1_a_realiser , " +
                    "       p_activite_controleur_vl.seuil_atteint_n1 , " +
                    "       p_activite_controleur_vl.activite_janvier_n , " +
                    "       p_activite_controleur_vl.activite_fevrier_n , " +
                    "       p_activite_controleur_vl.activite_mars_n , " +
                    "       p_activite_controleur_vl.activite_avril_n , " +
                    "       p_activite_controleur_vl.activite_mai_n , " +
                    "       p_activite_controleur_vl.activite_juin_n , " +
                    "       p_activite_controleur_vl.activite_juillet_n , " +
                    "       p_activite_controleur_vl.activite_aout_n , " +
                    "       p_activite_controleur_vl.activite_septembre_n , " +
                    "       p_activite_controleur_vl.activite_octobre_n , " +
                    "       p_activite_controleur_vl.activite_novembre_n , " +
                    "       p_activite_controleur_vl.activite_decembre_n , " +
                    "       p_activite_controleur_vl.total_vtp_n , " +
                    "       p_activite_controleur_vl.seuil_n_a_realiser , " +
                    "       p_activite_controleur_vl.seuil_atteint_n  " +
                    "FROM  " +
                    "       p_activite_controleur_vl,   " +
                    "       p_centre_encours,    " +
                    "       r_prefecture,    " +
                    "       p_contoleur_encours,    " +
                    "       r_region   " +
                    "WHERE  " +
                    "       ( p_activite_controleur_vl.id_controleur_encours = p_contoleur_encours.id_controleur_encours ) and  " +
                    "       ( p_contoleur_encours.id_centre_vl = p_centre_encours.id_centre_encours ) and  " +
                    "       ( r_prefecture.id_prefecture = p_centre_encours.id_prefecture ) and " +
                    "       ( r_region.id_region = r_prefecture.id_region )  " +
                            ((region != null) ? " AND ( r_prefecture.id_region = " + region.Id + ")" : "") +
                    "ORDER BY p_contoleur_encours.nom , p_contoleur_encours.prenom  ";

            List<ItemListeSuiviActiviteVL> stat = new List<ItemListeSuiviActiviteVL>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeSuiviActiviteVL item = new ItemListeSuiviActiviteVL();
                item.CodeReseau = Convert.ToString(row[0]);
                item.Nom = Convert.ToString(row[1]);
                item.Prenom = Convert.ToString(row[2]);
                item.CodeAgrement = Convert.ToString(row[3]);
                item.CodeAgrementCentre = Convert.ToString(row[4]);
                item.CodeDepartement = Convert.ToString(row[5]);
                item.Region = Convert.ToString(row[6]);
                item.DateAgrement = Convert.ToDateTime(row[7]);
                item.DateFinAgrement = Convert.ToDateTime(row[8]);
                item.DateDebutRattachementCentre = Convert.ToDateTime(row[9]);
                item.DateDernierCtrl = Convert.ToDateTime(row[10]);

                item.ActiviteJanvierN1 = Convert.ToInt32(row[11]);
                item.ActiviteFevrierN1 = Convert.ToInt32(row[12]);
                item.ActiviteMarsN1 = Convert.ToInt32(row[13]);
                item.ActiviteAvrilN1 = Convert.ToInt32(row[14]);
                item.ActiviteMaiN1 = Convert.ToInt32(row[15]);
                item.ActiviteJuinN1 = Convert.ToInt32(row[16]);
                item.ActiviteJuilletN1 = Convert.ToInt32(row[17]);
                item.ActiviteAoutN1 = Convert.ToInt32(row[18]);
                item.ActiviteSeptembreN1 = Convert.ToInt32(row[19]);
                item.ActiviteOctobreN1 = Convert.ToInt32(row[20]);
                item.ActiviteNovembreN1 = Convert.ToInt32(row[21]);
                item.ActiviteDecembreN1 = Convert.ToInt32(row[22]);
                item.TotalVtpN1 = Convert.ToInt32(row[23]);
                item.SeuilN1ARealiser = Convert.ToInt32(row[24]);
                item.SeuilAtteintN1 = Convert.ToString(row[25]);

                item.ActiviteJanvierN = Convert.ToInt32(row[26]);
                item.ActiviteFevrierN = Convert.ToInt32(row[27]);
                item.ActiviteMarsN = Convert.ToInt32(row[28]);
                item.ActiviteAvrilN = Convert.ToInt32(row[29]);
                item.ActiviteMaiN = Convert.ToInt32(row[30]);
                item.ActiviteJuinN = Convert.ToInt32(row[31]);
                item.ActiviteJuilletN = Convert.ToInt32(row[32]);
                item.ActiviteAoutN = Convert.ToInt32(row[33]);
                item.ActiviteSeptembreN = Convert.ToInt32(row[34]);
                item.ActiviteOctobreN = Convert.ToInt32(row[35]);
                item.ActiviteNovembreN = Convert.ToInt32(row[36]);
                item.ActiviteDecembreN = Convert.ToInt32(row[37]);
                item.TotalVtpN = Convert.ToInt32(row[38]);
                item.SeuilNARealiser = Convert.ToInt32(row[39]);
                item.SeuilAtteintN = Convert.ToString(row[40]);

                stat.Add(item);
            }
            return stat;
        }

        //YMA 16/10/2013
        public IList<ItemListeSuiviActiviteVL> ListeDesSuivisActiviteControleursVLRattachesAuxCentresDuReseau(rUtilisateur utilisateur)
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
                    "SELECT " +
                    "       p_activite_controleur_vl.cod_reseau_rattachement_vl , " +
                    "       p_activite_controleur_vl.nom , " +
                    "       p_activite_controleur_vl.prenom , " +
                    "       p_activite_controleur_vl.cod_agrem_cntlr , " +
                    "       p_activite_controleur_vl.cod_agrem_centre , " +
                    "       p_activite_controleur_vl.cod_departement , " +
                    "       p_activite_controleur_vl.region , " +
                    "       convert(char(12),p_activite_controleur_vl.date_effet_notification_vl,103) as date_effet_notification_vl ,  " +
                    "       convert(char(12),p_activite_controleur_vl.date_fin_agrement_vl,103) as date_fin_agrement_vl ,  " +
                    "       convert(char(12),p_activite_controleur_vl.date_debut_rattachement_vl,103) as date_debut_rattachement_vl ,  " +
                    "       convert(char(12),p_activite_controleur_vl.date_dernier_ctrl,103) as date_dernier_ctrl ,  " +
                    "       p_activite_controleur_vl.activite_janvier_n1 , " +
                    "       p_activite_controleur_vl.activite_fevrier_n1 , " +
                    "       p_activite_controleur_vl.activite_mars_n1 , " +
                    "       p_activite_controleur_vl.activite_avril_n1 , " +
                    "       p_activite_controleur_vl.activite_mai_n1 , " +
                    "       p_activite_controleur_vl.activite_juin_n1 , " +
                    "       p_activite_controleur_vl.activite_juillet_n1 , " +
                    "       p_activite_controleur_vl.activite_aout_n1 , " +
                    "       p_activite_controleur_vl.activite_septembre_n1 , " +
                    "       p_activite_controleur_vl.activite_octobre_n1 , " +
                    "       p_activite_controleur_vl.activite_novembre_n1 , " +
                    "       p_activite_controleur_vl.activite_decembre_n1 , " +
                    "       p_activite_controleur_vl.total_vtp_n1 , " +
                    "       p_activite_controleur_vl.seuil_n1_a_realiser , " +
                    "       p_activite_controleur_vl.seuil_atteint_n1 , " +
                    "       p_activite_controleur_vl.activite_janvier_n , " +
                    "       p_activite_controleur_vl.activite_fevrier_n , " +
                    "       p_activite_controleur_vl.activite_mars_n , " +
                    "       p_activite_controleur_vl.activite_avril_n , " +
                    "       p_activite_controleur_vl.activite_mai_n , " +
                    "       p_activite_controleur_vl.activite_juin_n , " +
                    "       p_activite_controleur_vl.activite_juillet_n , " +
                    "       p_activite_controleur_vl.activite_aout_n , " +
                    "       p_activite_controleur_vl.activite_septembre_n , " +
                    "       p_activite_controleur_vl.activite_octobre_n , " +
                    "       p_activite_controleur_vl.activite_novembre_n , " +
                    "       p_activite_controleur_vl.activite_decembre_n , " +
                    "       p_activite_controleur_vl.total_vtp_n , " +
                    "       p_activite_controleur_vl.seuil_n_a_realiser , " +
                    "       p_activite_controleur_vl.seuil_atteint_n  " +
                    "FROM  " +
                    "       p_activite_controleur_vl   " +
                    "WHERE  " +
                    "       ( p_activite_controleur_vl.cod_reseau_rattachement_vl IN (" + sqlin + ")) " +
                    "ORDER BY p_activite_controleur_vl.cod_reseau_rattachement_vl, p_activite_controleur_vl.nom , p_activite_controleur_vl.prenom  ";

            List<ItemListeSuiviActiviteVL> stat = new List<ItemListeSuiviActiviteVL>();

            var results = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL).List();
            foreach (object[] row in results)
            {
                ItemListeSuiviActiviteVL item = new ItemListeSuiviActiviteVL();
                item.CodeReseau = Convert.ToString(row[0]);
                item.Nom = Convert.ToString(row[1]);
                item.Prenom = Convert.ToString(row[2]);
                item.CodeAgrement = Convert.ToString(row[3]);
                item.CodeAgrementCentre = Convert.ToString(row[4]);
                item.CodeDepartement = Convert.ToString(row[5]);
                item.Region = Convert.ToString(row[6]);
                item.DateAgrement = Convert.ToDateTime(row[7]);
                item.DateFinAgrement = Convert.ToDateTime(row[8]);
                item.DateDebutRattachementCentre = Convert.ToDateTime(row[9]);
                item.DateDernierCtrl = Convert.ToDateTime(row[10]);

                item.ActiviteJanvierN1 = Convert.ToInt32(row[11]);
                item.ActiviteFevrierN1 = Convert.ToInt32(row[12]);
                item.ActiviteMarsN1 = Convert.ToInt32(row[13]);
                item.ActiviteAvrilN1 = Convert.ToInt32(row[14]);
                item.ActiviteMaiN1 = Convert.ToInt32(row[15]);
                item.ActiviteJuinN1 = Convert.ToInt32(row[16]);
                item.ActiviteJuilletN1 = Convert.ToInt32(row[17]);
                item.ActiviteAoutN1 = Convert.ToInt32(row[18]);
                item.ActiviteSeptembreN1 = Convert.ToInt32(row[19]);
                item.ActiviteOctobreN1 = Convert.ToInt32(row[20]);
                item.ActiviteNovembreN1 = Convert.ToInt32(row[21]);
                item.ActiviteDecembreN1 = Convert.ToInt32(row[22]);
                item.TotalVtpN1 = Convert.ToInt32(row[23]);
                item.SeuilN1ARealiser = Convert.ToInt32(row[24]);
                item.SeuilAtteintN1 = Convert.ToString(row[25]);

                item.ActiviteJanvierN = Convert.ToInt32(row[26]);
                item.ActiviteFevrierN = Convert.ToInt32(row[27]);
                item.ActiviteMarsN = Convert.ToInt32(row[28]);
                item.ActiviteAvrilN = Convert.ToInt32(row[29]);
                item.ActiviteMaiN = Convert.ToInt32(row[30]);
                item.ActiviteJuinN = Convert.ToInt32(row[31]);
                item.ActiviteJuilletN = Convert.ToInt32(row[32]);
                item.ActiviteAoutN = Convert.ToInt32(row[33]);
                item.ActiviteSeptembreN = Convert.ToInt32(row[34]);
                item.ActiviteOctobreN = Convert.ToInt32(row[35]);
                item.ActiviteNovembreN = Convert.ToInt32(row[36]);
                item.ActiviteDecembreN = Convert.ToInt32(row[37]);
                item.TotalVtpN = Convert.ToInt32(row[38]);
                item.SeuilNARealiser = Convert.ToInt32(row[39]);
                item.SeuilAtteintN = Convert.ToString(row[40]);

                stat.Add(item);
            }
            return stat;
        }

    }
}
