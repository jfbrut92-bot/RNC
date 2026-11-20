using System.Collections.Generic;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class pFactureRepository : BaseRepository<pFacture>, IpFactureRepository
    {
        private readonly ILog log = LogManager.GetLogger(typeof(pRapportExceptionRepository));

        public void traceTelechargement(pFacture facture, string codeUtilisateur)
        {
            log.Info(string.Format("On trace le téléchargement des factures {0}", facture.NomFichier));
            string dateTelechargement = string.Format("{0:yyyy-MM-dd HH:ss:mm}", System.DateTime.Now);
            string sqlUpdate = string.Format("UPDATE p_facture SET cod_utilisateur_telechargement = '{0}', date_telechargement_facture = '{1}' WHERE id_facture = {2}",
                                              codeUtilisateur, dateTelechargement, facture.Id);

            int ret = SessionFactory.GetCurrentSession().CreateSQLQuery(sqlUpdate).ExecuteUpdate();
            log.Info(string.Format("On trace le téléchargement des factures {0} OK RET = {1}", facture.NomFichier, ret));
        }

        public IList<pFacture> FindListFacture(string q)
        {
            string requeteSQL = "SELECT p_facture.id_facture, p_facture.cod_agrem_ind, p_facture.cod_categorie_veh, p_facture.cod_reseau, p_facture.nom_fichier, p_facture.num_facture, p_facture.date_facture, p_facture.date_generation_facture, p_facture.id_centre_encours, p_facture.date_telechargement_facture, p_facture.cod_utilisateur_telechargement, null as img_fichier " +
                                " FROM  p_facture    " + q;

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pFacture));

            IList<pFacture> liste = sqlQuery.List<pFacture>();

            return liste;
        }

    }
}
