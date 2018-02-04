using SolutionWeb.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace SolutionWeb
{
    public static class TimerConfig
    {
        public static void RegisterTimerConfig()
        {
                    string file = HttpRuntime.BinDirectory + "SolutionWeb.Areas.POLICY.dll";
                    if (File.Exists(file))
                    {
                        Assembly assdll = Assembly.LoadFrom(file);
                        try
                        {
                            Type t = assdll.GetType("SolutionWeb.Models.POLICY_TIMER");
                            t.GetConstructor(Type.EmptyTypes).Invoke(null);//反射实例化
                            RecordLog.RecordError("注册定时服务成功");
                        }
                        catch (Exception ex)
                        {
                            RecordLog.RecordError("注册定时服务失败:" + ex.ToString());
                        }
                    }
                    else
                    {
                        RecordLog.RecordError("注册定时服务:" + file + "不存在!");
                    }

        }
    }
}
