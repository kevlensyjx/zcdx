using Microsoft.Practices.Unity;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SolutionWeb
{
    public class WebApiLoginHandler : DelegatingHandler
    {

        #region Ioc
        private IUserLogin ul { set; get; }
        public WebApiLoginHandler()
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


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            int matchHeaderCount = request.Headers.Count((item) =>
           {
               if ("keyword".Equals(item.Key))
               {
                   foreach (var str in item.Value)
                   {
                       if ("ZCDX".Equals(str))
                       {
                           return true;
                       }
                   }
               }
               return false;
           });
            if (matchHeaderCount > 0)
            {
                return base.SendAsync(request, cancellationToken);
            }
            else
            {
                if (request.RequestUri.Segments[2].ToUpper() == "COMPANY/" || request.RequestUri.Segments[2].ToUpper() == "USERS/")//如果是企业
                {
                    if (request.RequestUri.Segments[2].ToUpper() == "COMPANY/" && request.RequestUri.Segments[4] != "CompanyRegisterIn")//如果不是注册
                    {
                        if (OperContext.CurrentContext.CompanyUser == null)
                        {
                            AjaxMsgModel amm = new AjaxMsgModel
                            {
                                BackUrl = "/Users/CompanyLogin?msg=noLogin",
                                Data = null,
                                Msg = Message.NotLogin,
                                Statu = AjaxStatu.nologin
                            };
                            var response = request.CreateResponse(System.Net.HttpStatusCode.OK, amm);
                            //var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                            var task = new TaskCompletionSource<HttpResponseMessage>();
                            task.SetResult(response);
                            return task.Task;
                        }
                    }
                }
                else
                {
                    if (!ul.IsLogin())
                    {
                        AjaxMsgModel amm = new AjaxMsgModel
                        {
                            BackUrl = "/Home/Login?msg=noLogin",
                            Data = null,
                            Msg = Message.NotLogin,
                            Statu = AjaxStatu.nologin
                        };
                        var response = request.CreateResponse(System.Net.HttpStatusCode.OK, amm);
                        //var response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                        var task = new TaskCompletionSource<HttpResponseMessage>();
                        task.SetResult(response);
                        return task.Task;
                    }
                }
                return base.SendAsync(request, cancellationToken);
            }
        }

    }
}