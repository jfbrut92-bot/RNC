using System.Web.Optimization;

namespace Utac.Rnc2.UI.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/scripts/jquery").Include(
                    "~/Scripts/jquery-3.5.1.js"));

            bundles.Add(new ScriptBundle("~/scripts/jquery-ui").Include(
                    "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/scripts/bootsrap").Include(
                       "~/lib/bootstrap/dist/js/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/scripts/date-picker").Include(
                    "~/Scripts/jquery-ui.js",
                    "~/Scripts/jquery-ui-i18n.js",
                    "~/Scripts/Rnc2/date-picker.js"));

            bundles.Add(new ScriptBundle("~/scripts/ckeditor/editor").Include(
                    "~/Scripts/ckeditor/ckeditor.js"));

            bundles.Add(new ScriptBundle("~/scripts/jquery-validate").Include(
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.unobtrusive.js"));

            // Application Bundles
            bundles.Add(new ScriptBundle("~/scripts/dropdown-menu").Include(
                    "~/Scripts/Rnc2/dropdown-menu.js"));

            bundles.Add(new ScriptBundle("~/scripts/utilisateurs").Include(
                    "~/Scripts/Rnc2/utilisateurs.js"));

            bundles.Add(new ScriptBundle("~/scripts/creation-utilisateur").Include(
                    "~/Scripts/Rnc2/creation-utilisateur.js"));

            bundles.Add(new ScriptBundle("~/scripts/saisie-numeric").Include(
                    "~/Scripts/Rnc2/saisie-numeric.js"));

            bundles.Add(new ScriptBundle("~/scripts/set-focus").Include(
                    "~/Scripts/Rnc2/set-focus.js"));

            bundles.Add(new ScriptBundle("~/scripts/creation-centre").Include(
                    "~/Scripts/Rnc2/creation-centre.js"));

            bundles.Add(new ScriptBundle("~/scripts/download").Include(
                   "~/Scripts/Rnc2/download.js"));

            bundles.Add(new ScriptBundle("~/scripts/renew-password").Include(
                   "~/Scripts/Rnc2/renew-password.js"));

            bundles.Add(new ScriptBundle("~/scripts/ajout-sanction-controleur").Include(
                   "~/Scripts/Rnc2/ajout-sanction-controleur.js"));

            bundles.Add(new ScriptBundle("~/scripts/ajout-sanction-centre").Include(
                    "~/Scripts/Rnc2/ajout-sanction-centre.js"));

            bundles.Add(new ScriptBundle("~/scripts/ajout-formation-controleur").Include(
                    "~/Scripts/Rnc2/ajout-formation-controleur.js"));

            bundles.Add(new ScriptBundle("~/scripts/common").Include(
                    "~/Scripts/Rnc2/common.js"));

            bundles.Add(new ScriptBundle("~/scripts/delete-centre").Include(
                    "~/Scripts/Rnc2/delete-centre.js"));

            bundles.Add(new ScriptBundle("~/scripts/url").Include(
                    "~/Scripts/Rnc2/url.js"));

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

            bundles.Add(new StyleBundle("~/styles/externe").Include(
                       "~/Content/style.css",
                       "~/Content/style2.css",
                       "~/Content/rnc2.css",
                       "~/Content/site_externe.css"));
        }
    }
}
