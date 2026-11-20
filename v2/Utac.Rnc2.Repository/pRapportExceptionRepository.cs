using System.Collections.Generic;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class pRapportExceptionRepository : BaseRepository<pRapportException>, IpRapportExceptionRepository
    {
        private readonly ILog log = LogManager.GetLogger(typeof(pRapportExceptionRepository));

        public void traceTelechargement(pRapportException rapport, string codeUtilisateur)
        {
            log.Info(string.Format("On trace le téléchargement du rapport {0}", rapport.NomRapport));
            string dateTelechargement = string.Format("{0:yyyy-MM-dd HH:ss:mm}", System.DateTime.Now);
            string sqlUpdate = string.Format("UPDATE p_rapport_exception SET cod_utilisateur_telechargement = '{0}', date_telechargement_rapport = '{1}' WHERE id_rapport_exception = {2}",
                                              codeUtilisateur, dateTelechargement, rapport.Id);

            int ret = SessionFactory.GetCurrentSession().CreateSQLQuery(sqlUpdate).ExecuteUpdate();
            log.Info(string.Format("On trace le téléchargement du rapport {0} OK RET = {1}", rapport.NomRapport, ret));
        }

        //PLE 18/01/2012
        public IList<pRapportException> findListRapportPL(string q)
        {

            string requeteSQL = "SELECT p_rapport_exception.id_rapport_exception,p_rapport_exception.cod_agrem_ind,p_rapport_exception.cod_reseau,p_rapport_exception.nom_rapport,p_rapport_exception.date_debut_transmission_pv,p_rapport_exception.date_fin_transmission_pv,p_rapport_exception.date_generation_rapport,p_rapport_exception.cod_utilisateur_telechargement,p_rapport_exception.date_telechargement_rapport,p_rapport_exception.id_centre_encours, null as img_fichier  " +
                                " FROM  p_rapport_exception    " + q;

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pRapportException));

            IList<pRapportException> liste = sqlQuery.List<pRapportException>();

            return liste;
        }
    }
}
