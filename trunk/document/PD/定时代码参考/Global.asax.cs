using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using SxShWeb.Areas.Filters;
using Microsoft.AspNet.SignalR;
using SxShWeb.DBMonitor;
using System.Collections;
using Common;
using System.Web.Configuration;
using System.Configuration;
using SxShWeb.Areas.Models;

namespace SxShWeb
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private int i = 0;
        public string ss = System.Environment.CurrentDirectory;
        private MobileLastPosMonitor mobileMonitor;
        protected void Application_Start()
        {
            mobileMonitor = new MobileLastPosMonitor();
            //RecordLog.RecordInfo("WebSite Start");
            //定义定时器 
            System.Timers.Timer myTimer = new System.Timers.Timer(180000);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;

            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new JsonAttribute());//注册JSON，兼容IE下返回JSON时弹出下载的提示
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            RegisterDatabaseNotice();
        }
        protected void Session_End(object sender, EventArgs e)
        {
            Hashtable useronLine = (Hashtable)Application["Online"];
            if (useronLine != null)
            {
                if (useronLine[Session.SessionID] != null)
                {
                    useronLine.Remove(Session.SessionID);
                    Application.Lock();
                    Application["Online"] = useronLine;
                    Application.UnLock();
                }
            }
        }

        //沈阳,南昌
        void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            try
            {
                //RecordLog.RecordError("报警计算标记：" + System.DateTime.Now + ":" + SxShWeb.Areas.Models.Model_JOB_WARNING.WARNFLAG.ToString());

                if (!SxShWeb.Areas.Models.Model_JOB_WARNING.WARNFLAG)
                {
                    SxShWeb.Areas.Models.Model_JOB_WARNING.Task();
                }
                //以下沈阳局自动导计划专用,南昌零点自动对讲分组
                /*
                if (i == 20)//保证一小时一次
                {
                    //沈阳局自动导计划专用
                    //SxShWeb.Areas.Models.CONVERT_PLAN_SHENYANG.ReadXls();


                    //南昌零点自动对讲分组
                    if (System.DateTime.Now.Hour == 0)//
                    {
                        SxShWeb.Areas.Models.Model_JOB_WARNING.GetAllPhoneGroup("零点自动对讲分组");//南昌零点自动对讲分组
                         //SxShWeb.Areas.Models.Model_GATE_GATEMNG_AUTHORIZATION.AutoAuthSkylight();//北京门禁零点自动授权门禁
                    }

                    i = 0;
                }
                i++;
                */
                #region 门禁超时报警
                //RecordLog.RecordInfo("作业门报警计算标记:" + DateTime.Now + ":" + Model_GATE_WARNING.WarnFlag.ToString());
                //if (Model_GATE_WARNING.WarnFlag)
                //{
                //    Model_GATE_WARNING.GateTask();
                //}
                #endregion
            }
            catch (Exception ee)
            {
                Trace.WriteLine(ee);
            }

        }

        public override void Init()
        {
            base.Init();
        }

        void MvcApplication_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.SetSessionStateBehavior(
                SessionStateBehavior.Required);
        }

        void RegisterDatabaseNotice()
        {
            //断轨监测
            //Remote.CreateRemoteObject(ConfigurationManager.AppSettings["REMOTE_URL"]);//南昌,集通,青藏,郑州,沈阳,太原,北京,呼和,武汉

            //RecordLog.RecordWarn("注册门禁监控");
            //GATEMonitor gm = new GATEMonitor();//太原门禁

            //GateInfoMonitor gmnew = new GateInfoMonitor();//北京门禁

            //RecordLog.RecordWarn("注册计划监控");
            //JobPlanMonitor jpm = new JobPlanMonitor();//太原

            //Model_JOB_WARNING.GetAllPhoneIconPlan();//南昌

            RainMonitor rm = new RainMonitor();//南昌,集通,青藏,太原,兰州,北京,呼和,武汉,上海,合肥
            RailTemperatureMonitor rtm = new RailTemperatureMonitor();//南昌,集通,青藏,郑州,沈阳,太原,兰州,北京,呼和,上海,合肥

            MobileFileMonitor mf = new MobileFileMonitor();//南昌,集通,青藏,郑州,沈阳,太原,武汉,兰州,哈尔滨,北京,呼和,武汉,上海,奎屯,昆明,合肥,济南
            WarnMonitor wm = new WarnMonitor();//南昌,集通,青藏,郑州,沈阳,太原,武汉,哈尔滨,北京,呼和,武汉,上海,合肥,济南

            //CommandMonitor cod = new CommandMonitor();
            //BOOTMonitor bm = new BOOTMonitor();

            //WaterMonitor waterMonitor = new WaterMonitor();
            //WindMonitor wind = new WindMonitor();//集通
            //SnowMonitor snow = new SnowMonitor();//集通
            //CZMonitor cz = new CZMonitor();//青藏
        }
    }

}