using System.Web;
using System.Web.Optimization;

namespace ModelExperiments
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/modernizr-*",
                        "~/Scripts/angular.js",
                        "~/Scripts/angular-animate.js",
                        "~/Scripts/angular-route.js",
                        "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                        "~/app/CommPortalApp.js",
                        "~/app/home/HomeController.js",
                        "~/app/mail/MailController.js",
                        "~/app/newsFeed/NewsFeedController.js",
                        "~/app/newsFeed/ReadNewsController.js",
                        "~/app/forum/ForumController.js",
                        "~/app/forum/PostAndRepliesController.js",
                        "~/app/admin/AdminController.js",
                        "~/app/publicUserProfile/PublicProfileController.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Bootstrap.css",
                      "~/Content/site.css"));
        }
    }
}
