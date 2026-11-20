using DeviceDetectorNET;
using DeviceDetectorNET.Parser;

namespace Utac.Rnc2.UI.Web.Utils
{
    public class DeviceHelper
    {
        public static bool IsMobileDevice(string userAgent)
        {
            DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
            var dd = new DeviceDetector(userAgent);
            dd.SkipBotDetection();
            dd.Parse();
            return dd.IsMobile() && !dd.IsTablet();
        }
    }
}
