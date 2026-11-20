using System.Collections.Generic;
using log4net;
using Utac.Rnc2.BusinessObjects;
using Utac.Rnc2.Contrats.Repositories;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.Repository
{
    public class pRapportExceptionCLRepository : BaseRepository<pRapportExceptionCl>, IpRapportExceptionCLRepository
    {

        private readonly ILog log = LogManager.GetLogger(typeof(pRapportExceptionRepository));

        //PLE 18/01/2012
        public IList<pRapportExceptionCl> findListRapportCL(string q)
        {

            string requeteSQL = "SELECT p_rapport_exception_cl.id_rapport_exception,p_rapport_exception_cl.cod_agrem_cntr,p_rapport_exception_cl.nom_rapport,p_rapport_exception_cl.date_debut_transmission_pv,p_rapport_exception_cl.date_fin_transmission_pv,p_rapport_exception_cl.date_generation_rapport,p_rapport_exception_cl.cod_utilisateur_telechargement,p_rapport_exception_cl.date_telechargement_rapport,p_rapport_exception_cl.id_centre_encours, null as img_fichier  " +
                                " FROM  p_rapport_exception_cl    " + q;

            NHibernate.ISQLQuery sqlQuery = SessionFactory.GetCurrentSession().CreateSQLQuery(requeteSQL);
            sqlQuery.AddEntity(typeof(pRapportExceptionCl));

            IList<pRapportExceptionCl> liste = sqlQuery.List<pRapportExceptionCl>();

            return liste;
        }
    }
}
