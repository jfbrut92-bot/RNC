using System;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class pControleurHistoRepository : BaseRepository<pControleurHisto>, IpControleurHistoRepository
    {
        private readonly ILog log = LogManager.GetLogger(typeof(pControleurHistoRepository));

        //EG le 21/02/2012 pour pouvoir supprimer l'historique d'un centre : 1) d'1 coup 2) en évitant le bug du Datetime sans les millisecondes
        public void removeAll(pContoleurEncour controleur)
        {
            if (controleur != null && controleur.Id != 0)
            {
                log.Info(string.Format("removeAll() : ID CONTROLEUR TECH = {0} , NOM = {1} PRENOM = {2}  ", controleur.Id, controleur.Nom, controleur.Prenom));
                string sqlDelete = string.Format("DELETE FROM p_controleur_histo WHERE id_controleur_histo = {0}", controleur.Id);
                int ret = SessionFactory.GetCurrentSession().CreateSQLQuery(sqlDelete).ExecuteUpdate();
                log.Info(string.Format("removeAll() : ID CONTROLEUR TECH = {0} , NOM = {1} PRENOM = {2} , RET = {3}... OK  ", controleur.Id, controleur.Nom, controleur.Prenom, ret));
            }
        }

        //EG le 22/02/2012 : on masque le Remove() de BaseRepository
        public new void Remove(pControleurHisto histo)
        {
            if (histo != null && histo.IdControleurHisto != 0)
            {
                DateTime to = histo.DateHisto.AddSeconds(1);
                string deb = string.Format("{0:yyyy-MM-dd HH:mm:ss}", histo.DateHisto);
                string fin = string.Format("{0:yyyy-MM-dd HH:mm:ss}", to);

                log.Info(string.Format("Remove() : ID CONTROLEUR HISTO = {0} , DATE HISTO = {1} , NOM = {2} PRENOM = {3} ", histo.IdControleurHisto, histo.DateHisto, histo.Nom, histo.Prenom));
                string sqlDelete = string.Format("DELETE FROM p_controleur_histo WHERE id_controleur_histo = {0} AND date_histo >= '{1}' AND date_histo < '{2}'", histo.IdControleurHisto, deb, fin);
                int ret = SessionFactory.GetCurrentSession().CreateSQLQuery(sqlDelete).ExecuteUpdate();
                log.Info(string.Format("Remove() : ID CONTROLEUR HISTO = {0} , DATE HISTO = {1} , NOM = {2} PRENOM = {3} ...RET= {4} OK ", histo.IdControleurHisto, histo.DateHisto, histo.Nom, histo.Prenom, ret));
            }
        }

    }
}
