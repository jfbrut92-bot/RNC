using System;
using System.Web;
using log4net;
using Utac.Rnc2.Repository.Nhibernate;

namespace Utac.Rnc2.UI.Web.Modules
{
    public class NHibernateHttpModule : IHttpModule
    {

        private readonly ILog log = LogManager.GetLogger(typeof(NHibernateHttpModule));

        public void Dispose()
        {
        }

        public void Init(System.Web.HttpApplication context)
        {
            context.EndRequest += Application_EndRequest;
            context.BeginRequest += Application_BeginRequest;
            //todo Gestion des erreurs context.Error += 
            //http://blogs.microsoft.co.il/blogs/oshvartz/archive/2008/05/17/asp-net-error-handling-using-httpmodule-full-and-partial-post-back-ajax-updatepanel.aspx

        }

        public void Application_BeginRequest(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.CurrentExecutionFilePathExtension))
            {
                if (!HttpContext.Current.Request.CurrentExecutionFilePathExtension.Contains("aspx"))
                {
                    return;
                }
            }
            string rawUrl = HttpContext.Current.Request.RawUrl;
            string ip = HttpContext.Current.Request.UserHostAddress;

            log.Info(string.Format("Application_BeginRequest URL:{0} FROM IP:{1} ...debut", rawUrl, ip));
            var factory = SessionFactory.GetSessionFactory(); // GetFactory returns an ISessionFactory in my helper class
            NHibernate.ISession session;
            if (NHibernate.Context.CurrentSessionContext.HasBind(factory))
            {
                session = factory.GetCurrentSession();
                log.Info(string.Format("Application_BeginRequest URL:{0}...session  récupérée : {1} , Entity COUNT = {2}", rawUrl, session, session.Statistics.EntityCount));
            }
            else
            {
                session = factory.OpenSession();
                NHibernate.Context.CurrentSessionContext.Bind(session);
                log.Info(string.Format("Application_BeginRequest URL = {0}... NEW session : {1} , Entity COUNT = {2}", rawUrl, session, session.Statistics.EntityCount));
                //EG pour le téléchargement des champs image > 32K (rapport CE par exemple)

                System.Data.IDbConnection conn = session.Connection;
                System.Data.IDbCommand cmd = conn.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "set textsize 10485760";
                cmd.ExecuteNonQuery();

                cmd.Dispose();
                cmd = null;
            }

            /* Pour gérer les transactions ici
              if (session != null) session.BeginTransaction();
             */

        }

        public void Application_EndRequest(object sender, EventArgs e)
        {
            string rawUrl = HttpContext.Current.Request.RawUrl;
            NHibernate.ISession session = default(NHibernate.ISession);

            session = NHibernate.Context.CurrentSessionContext.Unbind(SessionFactory.GetSessionFactory());

            if (session != null)
            {
                //EG le 17/02/2012
                //CurrentSession.Clear();
                log.Info(string.Format("Application_EndRequest URL = {0}...session  fermée : {1} , Entity COUNT = {2}", rawUrl, session, session.Statistics.EntityCount));

                /* Pour gérer les transactions ici
                if (session.Transaction != null && session.Transaction.IsActive)
                {
                    session.Transaction.Commit();
                }
                 */

                session.Close();

                session.Dispose();
            }
            else
            {
                if (HttpContext.Current.Request.CurrentExecutionFilePathExtension.Contains("aspx"))
                    log.Warn(string.Format("Application_EndRequest URL = {0}... Impossible de fermer la session", rawUrl));
            }

        }

    }

}
