using SolutionWeb.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace SolutionWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {

            Stopwatch watch = new Stopwatch();
            watch.Start();
            RecordLog.RecordError("程序启动开始");
            UnityConfig.RegisterComponents();
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new JsonAttribute());//注册JSON，兼容IE下返回JSON时弹出下载的提示
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            TimerConfig.RegisterTimerConfig();//注册定时服务
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory());//控制器的实例化走UnityControllerFactory
            watch.Stop();
            RecordLog.RecordError("程序启动完成,用时：" + watch.ElapsedMilliseconds.ToString() + "毫秒");
        }
        public override void Init()
        {
            //启用API SESSION
            this.PostAuthenticateRequest += (sender, e) => HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
            base.Init();
        }
    }
}
