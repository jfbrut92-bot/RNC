using System.Web.Optimization;

namespace Utac.Rnc2.Pv.UI.Web
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                        "~/Scripts/jquery-3.5.1.js"));

            bundles.Add(new ScriptBundle("~/scripts/date-picker").Include(
                    "~/Scripts/jquery-ui.js",
                    "~/Scripts/jquery-ui-i18n.js",
                    "~/Scripts/Rnc2/date-picker.js"));

            bundles.Add(new ScriptBundle("~/scripts/bootsrap").Include(
                       "~/lib/bootstrap/dist/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/scripts/dropdown-menu").Include(
                        "~/Scripts/Rnc2/dropdown-menu.js"));

            bundles.Add(new ScriptBundle("~/scripts/saisie-numeric").Include(
                        "~/Scripts/Rnc2/saisie-numeric.js"));

            bundles.Add(new ScriptBundle("~/scripts/set-focus").Include(
                        "~/Scripts/Rnc2/set-focus.js"));

            bundles.Add(new ScriptBundle("~/scripts/renew-password").Include(
                        "~/Scripts/Rnc2/renew-password.js"));

            bundles.Add(new ScriptBundle("~/scripts/menu").Include(
                        "~/Scripts/Rnc2/menu.js"));

            bundles.Add(new StyleBundle("~/styles/main-mobile").Include(
                        "~/Content/style.css",
                        "~/Content/style2.css",
                        "~/Content/rnc2.css",
                        "~/Content/style-app-mobile.css"));

            bundles.Add(new StyleBundle("~/styles/main-desktop").Include(
                        "~/Content/style.css",
                        "~/Content/style2.css",
                        "~/Content/rnc2.css",
                        "~/Content/style-app-desktop.css"));

            bundles.Add(new StyleBundle("~/styles/jquery-ui").Include(
                        "~/Content/jquery-ui.theme.css",
                        "~/Content/jquery-ui.structure.css"
                        ));

            bundles.Add(new StyleBundle("~/styles/icon").Include(
                       "~/Content/font-awesome.css"));
        }
    }
}
