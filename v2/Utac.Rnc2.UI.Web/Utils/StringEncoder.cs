using System.Text;

namespace Utac.Rnc2.UI.Web.Utils
{
    public class StringEncoder
    {
        /* http://stackoverflow.com/questions/1922199/c-sharp-convert-string-from-utf-8-to-iso-8859-1-latin1-h */

        public static string Utf8ToIso(string str)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;

            byte[] utfBytes = utf8.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            string convStr = iso.GetString(isoBytes);

            return convStr;
        }
    }
}