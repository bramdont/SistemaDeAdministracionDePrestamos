using System.Web;
using System.Web.Optimization;

namespace SistemaDeAdministracionDePrestamos
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new  ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Content/plugins/bower_components/jquery/dist/jquery.min.js",
                        "~/Content/html/bootstrap/dist/js/bootstrap.min.js",
                        "~/Content/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.js",
                        "~/Content/html/js/jquery.slimscroll.js",
                        "~/Content/html/js/waves.js",
                        "~/Content/plugins/bower_components/waypoints/lib/jquery.waypoints.js",
                        "~/Content/plugins/bower_components/counterup/jquery.counterup.min.js",
                        "~/Content/plugins/bower_components/chartist-js/dist/chartist.min.js",
                        "~/Content/plugins/bower_components/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.min.js",
                        "~/Content/plugins/bower_components/jquery-sparkline/jquery.sparkline.min.js",
                        "~/Content/html/js/custom.min.js",
                        "~/Content/html/js/dashboard1.js",
                        "~/Content/plugins/bower_components/toast-master/js/jquery.toast.js",
                        "~/Scripts/otf.js"));

           /* bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js")); */

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/html/bootstrap/dist/css/bootstrap.min.css",
                      "~/Content/site.css",
                      "~/Content/plugins/bower_components/sidebar-nav/dist/sidebar-nav.min.css",
                      "~/Content/plugins/bower_components/toast-master/css/jquery.toast.css",
                      "~/Content/plugins/bower_components/morrisjs/morris.css",
                      "~/Content/plugins/bower_components/chartist-js/dist/chartist.min.css",
                      "~/Content/plugins/bower_components/chartist-plugin-tooltip-master/dist/chartist-plugin-tooltip.css",
                      "~/Content/html/css/animate.css",
                      "~/Content/html/css/style.css",
                      "~/Content/html/css/colors/default.css",
                      "~/Content/Pagedlist.css"));
        }
    }
}
