using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace SolutionWeb.Common
{
    public class OperContext
    {
        public static OperContext CurrentContext
        {
            get
            {
                OperContext opContext = CallContext.GetData(typeof(OperContext).Name) as OperContext;
                if (opContext == null)
                {
                    opContext = new OperContext();
                    CallContext.SetData(typeof(OperContext).Name, opContext);

                }
                return opContext;
            }
        }
        
        
        

        #region 封装HTTP对象
        #region Http上下文
        /// <summary>
        /// Http上下文
        /// </summary>
        HttpContext ContextHttp
        {
            get
            {
                return HttpContext.Current;
            }
        }
        #endregion

        #region Response对象
        public HttpResponse Response
        {
            get
            {
                return ContextHttp.Response;
            }
        }
        #endregion

        #region Request对象
        public HttpRequest Request
        {
            get
            {
                return ContextHttp.Request;
            }
        }
        #endregion


        #region Session对象
        public HttpSessionState Session
        {
            get
            {
                return ContextHttp.Session;
            }
        }

        #endregion

        #endregion

        #region 当前用户
        /// <summary>
        /// 当前用户
        /// </summary>
        public SESS_USER CurrentUser
        {
            get
            {

                return Session["Admin_User"] as SESS_USER;
            }

            set
            {
                Session["Admin_User"] = value;
                Session.Timeout = 600;
            }
        }
        #endregion

        #region 当前用户验证码
        /// <summary>
        /// 当前用户验证码
        /// </summary>
        public string CurrentUserVcode
        {
            get
            {

                return Session["ValidateCode"] as string;
            }

            set
            {
                Session["ValidateCode"] = value;
            }
        }
        #endregion

        #region 当前用户菜单权限
        /// <summary>
        /// 当前用户权限
        /// </summary>
        public List<SESS_MENU> UserMenuPermission
        {
            get
            {

                return Session["Admin_Permission"] as List<SESS_MENU>;
            }

            set
            {
                Session["Admin_Permission"] = value;
            }
        }
        #endregion
        

        #region 保存当前登录用户的用户名
        //cookie;
        /// <summary>
        /// 保存当前登录用户的用户名
        /// </summary>
        public string CurrentUserName
        {
            set
            {

                HttpCookie tmpCooki = new HttpCookie("systemLoginName");
                tmpCooki.Value = value;

                tmpCooki.Expires = DateTime.Now.AddDays(1);
                Response.Cookies.Add(tmpCooki);
            }
            get
            {
                if (Request.Cookies["systemLoginName"] == null)
                {
                    return "";
                }
                else
                {
                    return Request.Cookies["systemLoginName"].Value;
                }
            }
        }
        #endregion


        #region 企业用户
        /// <summary>
        /// 企业用户
        /// </summary>
        public SESS_USER CompanyUser
        {
            get
            {

                return Session["Company_User"] as SESS_USER;
            }

            set
            {
                Session["Company_User"] = value;
                Session.Timeout = 600;
            }
        }
        #endregion
    }
}
