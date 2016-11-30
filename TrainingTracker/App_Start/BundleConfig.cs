using System.Web;
using System.Web.Optimization;

namespace TrainingTracker
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
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

             bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            #region Scripts
           

            //Scripts
            bundles.Add(new ScriptBundle("~/bundles/LayoutViewScripts").Include(
                "~/Scripts/jquery-1.10.2.js",
                "~/Scripts/knockout-3.4.0.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/Custom/App.js",
                "~/Scripts/Custom/AjaxService.js",
                "~/Scripts/Custom/UserService.js",
                "~/Scripts/Custom/BindingHandlers.js",
                "~/Scripts/Custom/Layout.js",
                "~/Scripts/Custom/AddEditProfile.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ProfileViewScripts").Include(
              "~/Scripts/Chart.Scatter.min.js" ,
               "~/Scripts/jquery-confirm.js" ,
               "~/Scripts/autosize.js" ,
              "~/Scripts/bootstrap-datepicker.min.js" ,
              "~/Scripts/jquery.steps.js" ,
              "~/Scripts/typehead.js" ,
              "~/Scripts/Custom/FeedbackChart.js" ,
              "~/Scripts/Custom/UserProfile.js" ,
              "~/Scripts/Custom/FeedbackThread.js" ,
              "~/Scripts/wz_tooltip.js" 
              ));

            bundles.Add(new ScriptBundle("~/bundles/ReleaseViewScripts").Include(
                "~/Scripts/bootstrap-datepicker.min.js",
                "~/Scripts/Custom/ReleaseService.js",
                "~/Scripts/Custom/Release.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/DashBoardViewScripts").Include(
               "~/Scripts/Custom/Dashboard.js" ,
               "~/Scripts/wz_tooltip.js" 
               ));

            bundles.Add(new ScriptBundle("~/bundles/SessionViewScripts").Include(
              "~/Scripts/bootstrap-datepicker.min.js" ,
              "~/Scripts/Custom/AllSessions.js" ,
              "~/Scripts/Custom/SessionService.js",
              "~/Scripts/Custom/Video.js",
              "~/Scripts/Video.js",
              "~/Scripts/wz_tooltip.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/AllProfileViewScripts").Include(
            "~/Scripts/Custom/AllProfile.js"
            ));
            #endregion

            #region Styles
            bundles.Add(new StyleBundle("~/bundles/LayoutViewStyles").Include(
                      "~/Content/bootstrap.min.css" ,
                      "~/StyleSheets/Layout.css",
                      "~/StyleSheets/AddEditProfile.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/ProfileViewStyles").Include(
                      "~/Content/DatePicker/bootstrap-datepicker3.css" ,
                      "~/StyleSheets/SurveyWizard.css" ,
                       "~/StyleSheets/jquery-confirm.css" ,
                      "~/Content/Timeline.css" ,
                      "~/StyleSheets/Profile.css",
                      "~/StyleSheets/FeedbackPlot.css",
                      "~/StyleSheets/FeedbackThread.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/ReleaseViewStyles").Include(
                      "~/Content/DatePicker/bootstrap-datepicker3.css" ,
                      "~/StyleSheets/Release.css" 
                      ));

            bundles.Add(new StyleBundle("~/bundles/DashboardViewStyles").Include(
                "~/StyleSheets/Dashboard.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/SessionViewStyles").Include(
               "~/StyleSheets/Profile.css",
               "~/Content/DatePicker/bootstrap-datepicker3.css",
               "~/StyleSheets/Session.css",
               "~/StyleSheets/Video-js.css"
               ));

            bundles.Add(new StyleBundle("~/bundles/AllProfileViewStyles").Include(
             "~/StyleSheets/AllProfiles.css" 
             ));
            #endregion

        }
    }
}
