using System;
using System.Linq;
using log4net;
using NHibernate.Criterion;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class pCentreHistoRepository : BaseRepository<pCentreHisto>, IpCentreHistoRepository
    {
        private readonly ILog log = LogManager.GetLogger(typeof(pCentreEncourRepository));

        public pCentreHisto findLastBy(object id)
        {
            NHibernate.ICriteria CriteriaQuery = SessionFactory.GetCurrentSession().CreateCriteria(typeof(pCentreHisto));
            //filtrer sur l'id
            CriteriaQuery.Add(Expression.Eq("this.IdCentreHisto", id));
            //trier par DateHisto
            CriteriaQuery.AddOrder(new NHibernate.Criterion.Order("DateHisto", false));

            if (CriteriaQuery.List<pCentreHisto>().Count() > 0)
            {
                return CriteriaQuery.List<pCentreHisto>().ElementAt(0);
            }
            return null;
        }

        //EG le 21/02/2012 pour pouvoir supprimer l'historique d'un centre : 1) d'1 coup 2) en évitant le bug du Datetime sans les millisecondes
        public void removeAll(pCentreEncour centre)
        {
            if (centre != null && centre.Id != 0)
            {
                log.Info(string.Format("removeAll() : ID CENTRE = {0} , RAISON SOCIALE = {1} ", centre.Id, centre.RaisonSociale));
                string sqlDelete = string.Format("DELETE FROM p_centre_histo WHERE id_centre_histo = {0}", centre.Id);
                int ret = SessionFactory.GetCurrentSession().CreateSQLQuery(sqlDelete).ExecuteUpdate();
                log.Info(string.Format("removeAll() : ID CENTRE = {0} , RAISON SOCIALE = {1} , RET = {2}... OK", centre.Id, centre.RaisonSociale, ret));
            }
        }

        //EG le 05/06/2012 : on masque le Remove() de BaseRepository

        public new void Remove(pCentreHisto histo)
        {
            if (histo != null && histo.IdCentreHisto != 0)
            {
                string deb = string.Format("{0:yyyy-MM-dd HH:mm:ss}", histo.DateHisto);
                DateTime to = histo.DateHisto.AddSeconds(1);
                string fin = string.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

                log.Info(string.Format("Remove() : ID CENTRE HISTO = {0} , DATE HISTO = {1} , RAISON SOCIALE = {2} VILLE = {3} ", histo.IdCentreHisto, histo.DateHisto, histo.RaisonSociale, histo.Ville));
                string sqlDelete = string.Format("DELETE FROM p_centre_histo WHERE id_centre_histo = {0} AND date_histo >= '{1}' AND date_histo < '{2}'", histo.IdCentreHisto, deb, fin);
                int ret = SessionFactory.GetCurrentSession().CreateSQLQuery(sqlDelete).ExecuteUpdate();
                log.Info(string.Format("Remove() : ID CENTRE HISTO = {0} , DATE HISTO = {1} , RAISON SOCIALE = {2} VILLE = {3} ...RET= {4} OK ", histo.IdCentreHisto, histo.DateHisto, histo.RaisonSociale, histo.Ville, ret));
            }
        }

    }
}
