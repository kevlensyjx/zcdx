using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SolutionWeb.Common
{
    public class BaseApiController : ApiController
    {
        #region UI层与Service之间操作的上下文
        protected OperContext oc = OperContext.CurrentContext;
        #endregion


        #region 验证是否登录状态
        [System.Web.Http.HttpPost]
        public AjaxMsgModel IsLogin()
        {
            AjaxMsgModel amm = new AjaxMsgModel
            {
                BackUrl = "",
                Data = null,
                Msg = "",
                Statu = AjaxStatu.ok
            };
            return amm;
        }

        [System.Web.Http.HttpGet]
        public AjaxMsgModel IsGetLogin()
        {
            AjaxMsgModel amm = new AjaxMsgModel
            {
                BackUrl = "",
                Data = null,
                Msg = "",
                Statu = AjaxStatu.ok
            };
            return amm;
        }
        #endregion
    }
}
