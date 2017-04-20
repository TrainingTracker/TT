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
                     "~/Content/bootstrap.min.css",
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
                "~/Scripts/Custom/WebWorker/WebWorker.js",
                "~/Scripts/Custom/Layout.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ProfileViewScripts").Include(
              "~/Scripts/Chart.Scatter.min.js",
               "~/Scripts/jquery-confirm.js",
               "~/Scripts/autosize.js",
              "~/Scripts/bootstrap-datepicker.min.js",
              "~/Scripts/jquery.steps.js",
              "~/Scripts/typehead.js",
              "~/Scripts/Custom/FeedbackChart.js",
              "~/Scripts/Custom/UserProfile.js",
              "~/Scripts/Custom/DiscussionForumService.js",
              "~/Scripts/Custom/DiscussionForum.js",
              "~/Scripts/Custom/FeedbackThread.js",
              "~/Scripts/Custom/DiscussionThread.js",
              "~/Scripts/wz_tooltip.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/ReleaseViewScripts").Include(
                "~/Scripts/bootstrap-datepicker.min.js",
                "~/Scripts/Custom/ReleaseService.js",
                "~/Scripts/Custom/Release.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/HelpForumScripts").Include(
                "~/Scripts/Custom/HelpForumService.js",
                "~/Scripts/Custom/HelpForum.js",
                 "~/Scripts/jquery-confirm.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/DashBoardViewScripts").Include(
               "~/Scripts/Custom/Dashboard.js",
               "~/Scripts/wz_tooltip.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/SessionViewScripts").Include(
              "~/Scripts/bootstrap-datepicker.min.js",
              "~/Scripts/jquery-confirm.js",
              "~/Scripts/Custom/AllSessions.js",
              "~/Scripts/Custom/SessionService.js",
              "~/Scripts/Custom/Video.js",
              "~/Scripts/Video.js",
              "~/Scripts/wz_tooltip.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/AllProfileViewScripts").Include(
            "~/Scripts/Custom/AllProfile.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/CourseEditorViewScripts").Include(
                "~/Scripts/ko.mapping.js",
                //  "~/Scripts/ckeditor/ckeditor.js",
               "~/Scripts/jquery-confirm.js",
               "~/Scripts/notify.js",
               "~/Scripts/jquery-ui.js",
               "~/Scripts/knockout-sortable.js",
               "~/Scripts/Custom/CourseService.js",
                "~/Scripts/Custom/CourseEditor.js"

            ));

            bundles.Add(new ScriptBundle("~/bundles/AllCoursesViewScripts").Include(
              "~/Scripts/Custom/CourseService.js",
              "~/Scripts/Custom/AllCourses.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/LearningMapViewScripts").Include(
              "~/Scripts/ko.mapping.js",
              "~/Scripts/jquery-confirm.js",
              "~/Scripts/notify.js",
              "~/Scripts/jquery-ui.js",
              "~/Scripts/knockout-sortable.js",
              "~/Scripts/Custom/LearningMapService.js",
              "~/Scripts/Custom/LearningMap.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/CourseViewScripts").Include(
             "~/Scripts/ko.mapping.js",
             "~/Scripts/jquery-confirm.js",
             "~/Scripts/Custom/FeedbackThread.js",
             "~/Scripts/Custom/CourseService.js",
             "~/Scripts/Custom/Course.js",
             "~/Scripts/autosize.js"
             ));

            bundles.Add(new ScriptBundle("~/bundles/UserSettingViewScripts").Include(
               "~/Scripts/Custom/AddEditProfile.js",
               "~/Scripts/Custom/NotificationSetting.js",
               "~/Scripts/Custom/UserSetting.js",
               "~/Scripts/notify.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/MirrorViewScripts").Include(
              "~/Scripts/Custom/Chart.Scatter.Shekhar.js",
              "~/Scripts/bootstrap-datepicker.min.js",
               "~/Scripts/Custom/FeedbackThread.js",
              "~/Scripts/Custom/Services/MirrorServices.js",
              "~/Scripts/Custom/Mirror.js",
              "~/Scripts/Custom/Shared/MirrorSummary.js",
               "~/Scripts/Custom/Shared/MirrorReport.js"
             ));

             bundles.Add(new ScriptBundle("~/bundles/UserSettingViewScripts").Include(
                 "~/Scripts/Custom/MemberDetails.js",
                "~/Scripts/Custom/AddEditProfile.js",
                "~/Scripts/Custom/NotificationSetting.js",
                "~/Scripts/Custom/UserSetting.js",
                "~/Scripts/notify.js"               
               ));
            #endregion

            #region Styles
            bundles.Add(new StyleBundle("~/bundles/LayoutViewStyles").Include(
                      "~/StyleSheets/Layout.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/ProfileViewStyles").Include(
                      "~/Content/DatePicker/bootstrap-datepicker3.css",
                      "~/StyleSheets/SurveyWizard.css",
                       "~/StyleSheets/jquery-confirm.css",
                      "~/Content/Timeline.css",
                      "~/StyleSheets/Profile.css",
                      "~/StyleSheets/FeedbackPlot.css",
                      "~/StyleSheets/FeedbackThread.css",
                      "~/StyleSheets/DiscussionThread.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/ReleaseViewStyles").Include(
                      "~/Content/DatePicker/bootstrap-datepicker3.css",
                      "~/StyleSheets/Release.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/HelpForumStyles").Include(
                        "~/Content/Site.css",
                        "~/StyleSheets/HelpForum.css",
                         "~/StyleSheets/jquery-confirm.css"
                      ));

            bundles.Add(new StyleBundle("~/bundles/DashboardViewStyles").Include(
                "~/StyleSheets/Dashboard.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/SessionViewStyles").Include(

               "~/Content/DatePicker/bootstrap-datepicker3.css",
               "~/StyleSheets/Session.css",
               "~/StyleSheets/jquery-confirm.css",
               "~/StyleSheets/Video-js.css"
               ));

            bundles.Add(new StyleBundle("~/bundles/AllProfileViewStyles").Include(
             "~/StyleSheets/AllProfiles.css"
             ));

            bundles.Add(new StyleBundle("~/bundles/AllCoursesViewStyles").Include(
            "~/StyleSheets/AllCourse.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/CoursesEditorViewStyles").Include(
                    "~/StyleSheets/CourseEditor.css",
                   "~/StyleSheets/jquery-confirm.css",
                   "~/Content/site.css"
             ));

            bundles.Add(new StyleBundle("~/bundles/LearningMapViewStyles").Include(
                 "~/StyleSheets/LearningMap.css",
                 "~/StyleSheets/jquery-confirm.css",
                 "~/Content/site.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/CourseViewStyles").Include(
                  "~/StyleSheets/FeedbackThread.css",
                  "~/StyleSheets/FeedbackPlot.css",
                  "~/Content/Site.css",
                  "~/StyleSheets/Course.css",
                   "~/StyleSheets/jquery-confirm.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/UserSettingViewStyles").Include(
                    "~/StyleSheets/UserSetting.css",
                    "~/StyleSheets/ProfileSetting.css",
                    "~/StyleSheets/NotificationSetting.css",
                    "~/StyleSheets/GpsUserSetting.css"
            ));

            bundles.Add(new StyleBundle("~/bundles/MirrorViewStyles").Include(
                  "~/StyleSheets/Mirror.css",
                  "~/Content/DatePicker/bootstrap-datepicker3.css",
                  "~/StyleSheets/FeedbackThread.css",
                  "~/StyleSheets/MirrorSummary.css",
                  "~/StyleSheets/MirrorReport.css"
          ));

            #endregion

        }
    }
}
