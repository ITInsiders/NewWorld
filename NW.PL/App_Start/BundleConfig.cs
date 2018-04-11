using System.Web;
using System.Web.Optimization;

namespace NW.PL
{
    public class BundleConfig
    {
        // Дополнительные сведения об объединении см. на странице https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/SystemStyles").Include(
                "~/Content/bootstrap*",
                "~/Resources/CSS/System/Fonts.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/SystemScripts").Include(
                "~/Scripts/modernizr-{version}.js",
                "~/Scripts/respond*",
                "~/Scripts/jquery-3.3.1.min.js",
                "~/Scripts/popper*",
                "~/Scripts/bootstrap*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                        "~/Scripts/jquery.signalR-2.2.2.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/hashchange").Include(
                        "~/Scripts/hashchange.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
        }
    }
}
