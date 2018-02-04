using Microsoft.Practices.Unity;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb
{
    public class LoginValidateAttribute : AuthorizeAttribute
    {
       
        #region Ioc
        private IUserLogin ul { set; get; }
        public LoginValidateAttribute()
        {
            ul = uc.Resolve<IUserLogin>();
        }

        private IUnityContainer uc
        {
            get
            {
                return DIFactory.GetContainer();
            }
        }
        #endregion
        
        #region  在过程请求授权时调用
        //
        // 摘要: 
        //     在过程请求授权时调用。
        //
        // 参数: 
        //   filterContext:
        //     筛选器上下文，它封装有关使用 System.Web.Mvc.AuthorizeAttribute 的信息。
        //
        // 异常: 
        //   System.ArgumentNullException:
        //     filterContext 参数为 null。
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            /**
             * 如果请求的区域包含area并且area的名称等于SYSs
             * 那么就进行权限验证 
             * */

            if (filterContext.RouteData.DataTokens.Keys.Contains("area"))
            {
                string strAreaName = filterContext.RouteData.DataTokens["area"].ToString().ToLower();
                string strControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
                string strActionName = filterContext.ActionDescriptor.ActionName.ToLower();

                string strHttpMethod = filterContext.HttpContext.Request.HttpMethod;
                HttpMethod httpMethod = strHttpMethod.ToLower().Equals("get") ? HttpMethod.Get
                    : strHttpMethod.ToLower().Equals("post") ? HttpMethod.Post : HttpMethod.HEAD;

                if (strAreaName == "company")//如果是企业
                {
                    if (!filterContext.ActionDescriptor.AttributeExists<SkipAttribute>(false)
                            && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipAttribute), false))
                    {
                        if (OperContext.CurrentContext.CompanyUser == null)
                        {
                            filterContext.Result = new BaseController().Redirect("/Users/CompanyLogin?msg=noLogin", filterContext.ActionDescriptor, httpMethod, AjaxStatu.nologin);
                        }
                    }
                }
                else
                {
                    if (!ul.IsLogin())
                    {
                        filterContext.Result = new BaseController().Redirect("/Home/Login?msg=noLogin", filterContext.ActionDescriptor, httpMethod, AjaxStatu.nologin);
                    }
                    else
                    {

                        if (!filterContext.ActionDescriptor.AttributeExists<SkipAttribute>(false)
                            && !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(SkipAttribute), false))
                        {
                            //验证该登录用户是否有访问该页面的权限

                            if (strActionName == "index")
                            {
                                string[] url = filterContext.HttpContext.Request.FilePath.Split('/');
                                if (url.Length > 4)
                                {
                                    for (int i = 4; i < url.Length; i++)
                                    {
                                        strActionName = strActionName + "/" + url[i];
                                    }
                                }
                            }


                            if (!ul.HasPermission(strAreaName, strControllerName, strActionName, httpMethod))
                            {
                                if (strActionName.IndexOf("index") > -1)
                                {
                                    filterContext.Result = new ViewResult()
                                    {
                                        ViewName = "~/Views/Shared/Error.cshtml",
                                        ViewData = new ViewDataDictionary<string>(string.Format(Message.OptNoPermission.Replace("<br/>", ""), filterContext.ActionDescriptor.GetDescription()))
                                    };
                                }
                                else
                                {
                                    filterContext.Result = new BaseController().Redirect("/Home/Login?msg=noPermission", filterContext.ActionDescriptor, httpMethod, AjaxStatu.noperm);

                                }
                            }
                            else
                            {
                                if (strActionName.ToLower() == "list" && filterContext.HttpContext.Request["page"] != null && filterContext.HttpContext.Request["rows"] != null)
                                {
                                    string pageIndex = filterContext.HttpContext.Request["page"].ToString();
                                    string pageSize = filterContext.HttpContext.Request["rows"].ToString();
                                    if (pageIndex == "0" && pageSize == "0")
                                    {
                                        filterContext.Result = ObjToJson.GetToJson(null, 0, true);
                                    }
                                }
                            }
                        }
                    }
                }
                
            }

        }
        #endregion
    }
}