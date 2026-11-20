using System.Configuration;

namespace Utac.Rnc2.MVC.Extensions
{
    public class AuthorizationConfiguration : ConfigurationSection
    {
        private static readonly AuthorizationConfiguration _authorizationConfiguration = ConfigurationManager.GetSection("authorizationConfiguration") as AuthorizationConfiguration;

        public static AuthorizationConfiguration Section
        {
            get
            {
                return _authorizationConfiguration;
            }
        }

        [ConfigurationProperty("controllerAuthorizationMappings")]
        public ControllerAuthorizationConfigurationCollection ControllerAuthorizationMappings
        {
            get
            {
                return this["controllerAuthorizationMappings"] as ControllerAuthorizationConfigurationCollection;
            }
        }
    }

}
