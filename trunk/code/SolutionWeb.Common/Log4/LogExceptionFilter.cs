using SolutionWeb.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SolutionWeb.Common
{

    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class LogExceptionFilter : HandleErrorAttribute
    {
        //private Logger logger = Logger.CreateLogger(typeof(LogExceptionFilter));
        public override void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)//异常有没有被处理过
            {
                string controllerName = (string)filterContext.RouteData.Values["controller"];
                string actionName = (string)filterContext.RouteData.Values["action"];
                string msgTemplate = "在执行 controller[{0}] 的 action[{1}] 时产生异常";
                //logger.Error(string.Format(msgTemplate, controllerName, actionName), filterContext.Exception);
                RecordLog.RecordError(string.Format(msgTemplate, controllerName, actionName)+":"+filterContext.Exception.InnerException.Message);
                if (actionName== "LoginIn" || filterContext.HttpContext.Request.IsAjaxRequest())//检查请求头
                {
                    filterContext.Result = new JsonResult()
                    {
                        Data = new AjaxMsgModel()
                        {
                            Statu = AjaxStatu.err,
                            Msg = "系统出现异常，请联系管理员",
                            Data = filterContext.Exception.InnerException.Message
                        }//这个就是返回的结果
                    };
                }
                else
                {
                    filterContext.Result = new ViewResult()
                    {
                        ViewName = "~/Views/Shared/Error.cshtml",
                        ViewData = new ViewDataDictionary<string>(filterContext.Exception.InnerException.Message)
                    };
                }
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
