using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using AutoMapper;
using TrainingTracker.Common.Utility;


namespace TrainingTracker
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SqlUtility.ConnectionString =
                System.Configuration.ConfigurationManager.ConnectionStrings["TTConStr"].ConnectionString;

            Mapper.Initialize(cfg => cfg.AddProfile<BLL.ModelMapper.BLLMappingProfile>());
        }

        private void Application_Error()
        {
            
            Exception exception = Server.GetLastError();
            LogUtility.ErrorRoutine(exception);
            Server.ClearError();

            if (new HttpRequestWrapper(Context.Request).IsAjaxRequest())
            {
                Context.Response.ContentType = "application/json";
                Context.Response.StatusCode = 500;
                Context.Response.Write(new JavaScriptSerializer().Serialize(
                        new { error = "Some Error occured in your request" }
                    )
                );
            }

        }
    }

}
