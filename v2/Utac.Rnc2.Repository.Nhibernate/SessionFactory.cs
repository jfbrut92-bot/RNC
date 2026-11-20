using System;

namespace Utac.Rnc2.Repository.Nhibernate
{
    public class SessionFactory
    {

        private static NHibernate.ISessionFactory _SessionFactory;

        private static void Init()
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();

                NHibernate.Cfg.Configuration config;

                config = new NHibernate.Cfg.Configuration();
                config.AddAssembly(typeof(SessionFactory).Assembly);

                // As long as we have an empty database with the same name as the one in our 
                // connection string Nhibernate will generate the schema for us, we only want 
                // to run this once. 
                //Dim se As New SchemaExport(config) 
                //se.Execute(False, True, False, False) 

                config.Configure();

                _SessionFactory = config.BuildSessionFactory();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static NHibernate.ISessionFactory GetSessionFactory()
        {
            try
            {
                if (_SessionFactory == null)
                {
                    Init();
                }

                return _SessionFactory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static NHibernate.ISession GetNewSession()
        {

            return GetSessionFactory().OpenSession();
        }

        public static NHibernate.ISession GetCurrentSession()
        {

            return GetSessionFactory().GetCurrentSession();
        }

    }

}
