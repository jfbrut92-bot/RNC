using System.Collections.Generic;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class pRapportExceptionVLRepository : BaseRepository<pRapportExceptionVl>, IpRapportExceptionVLRepository
    {

        private readonly ILog log = LogManager.GetLogger(typeof(pRapportExceptionRepository));

        //PLE 18/01/2012
        public IList<pRapportExceptionVl> findListRapportVL(string q)
        {

            string requeteSQL = "SELECT p_rapport_exception_vl.id_rapport_exception,p_rapport_exception_vl.cod_agrem_cntr,p_rapport_exception_vl.nom_rapport,p_rapport_exception_vl.date_debut_transmission_pv,p_rapport_exception_vl.date_fin_transmission_pv,p_rapport_exception_vl.date_generation_rapport,p_rapport_exception_vl.cod_utilisateur_telechargement,p_rapport_exception_vl.date_telechargement_rapport,p_rapport_exception_vl.id_centre_encours, null as img_fichier  " +
                                " FROM  p_rapport_exception_vl    " + q;

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pRapportExceptionVl));

            IList<pRapportExceptionVl> liste = sqlQuery.List<pRapportExceptionVl>();

            return liste;
        }
    }
}
