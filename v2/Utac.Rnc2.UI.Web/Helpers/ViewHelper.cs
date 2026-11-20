using System.Web.Mvc;

namespace Utac.Rnc2.UI.Web.Helpers
{
    public class ViewHelper
    {
        static public string getFormValue(FormCollection form, string name)
        {
            if (form == null) return "";
            if (form[name] == null)
                return "";
            else
                return form[name];
        }

        static public string getBoolValue(bool bValue, string txtTrue, string txtFalse, string tag)
        {
            if (bValue)
            {
                if (!string.IsNullOrEmpty(tag))
                    return string.Format("<{0}>{1}</{0}>", tag, txtTrue);
                else
                    return txtTrue;
            }
            else
                return txtFalse;
        }
    }
}