using SolutionWeb.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Web;

namespace SolutionWeb.Models
{
    public class POLICY_TIMER
    {
        public POLICY_TIMER()
        {
            //一天一次
            //Timer myTimer = new Timer(1000*60*60*24);
            Timer myTimer = new Timer(20000);
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            //RecordLog.RecordError("定时服务POLICY_TIMER启动:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        
        void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {
            //写你的代码
            string hour = ConfigurationManager.AppSettings["POLICITY_HOUR"];
            string minute = ConfigurationManager.AppSettings["POLICITY_MINUTE"];
            string test_flag = ConfigurationManager.AppSettings["POLICITY_TEST"];
            //事项公示是否测试 1为测试，0为正式，2为关闭计算
            int int_hour = 8;
            int int_minute = 30;
            int.TryParse(hour, out int_hour);
            int.TryParse(minute, out int_minute);

            if (test_flag == "1")
                Model_POLICY_MAIN_INFO.Create.Task();
            else if (test_flag == "0")
            {
                //第一次时间：取配置时间
                if (DateTime.Now.Hour == int_hour && DateTime.Now.Minute == int_minute)
                    Model_POLICY_MAIN_INFO.Create.Task();
                //第二次时间：取配置时间+5分钟
                if (DateTime.Now.Hour == int_hour && DateTime.Now.Minute == int_minute + 5)
                    Model_POLICY_MAIN_INFO.Create.Task();
                //第二次时间：取配置时间+10分钟
                if (DateTime.Now.Hour == int_hour && DateTime.Now.Minute == int_minute + 10)
                    Model_POLICY_MAIN_INFO.Create.Task();
            }
           
        }
    }
}