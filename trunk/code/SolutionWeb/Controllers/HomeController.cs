using SolutionWeb.Common;
using SolutionWeb.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Controllers
{
    public class HomeController : BaseController
    {

        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;
        public HomeController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        public ActionResult Login()
        {
            ViewBag.title = "";
            return View();
        }

        #region 登录验证，只允许POST提交
        /// <summary>
        /// 登录验证，只允许POST提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult LoginIn()
        {
            string ip = Request.ServerVariables["REMOTE_ADDR"].ToString();
            if (Request.Form["username"] == null || Request.Form["password"] == null || Request.Form["checkcode"] == null)
            {
                return PackagingAjaxmsg(AjaxStatu.err, "登录名|密码|验证码-不能为空!");
            }
            string username = Request["username"].ToString();
            string password = Request["password"].ToString();
            string checkcode = Request["checkcode"].ToString();

            AjaxMsgModel amm = ul.LoginIn(username, password, checkcode);

            //GenerateValidateCode();//北京后台刷新验证码
            return PackagingAjaxmsg(amm);

        }
        #endregion


        #region 验证码的实现
        /// <summary>
        /// 验证码的实现
        /// </summary>
        /// <returns>返回验证码</returns>
        [HttpGet]
        public ActionResult CheckCode()
        {
            //得到验证码的图片
            byte[] bytes = GenerateValidateCode();
            ////最后将验证码返回
            return File(bytes, @"image/jpeg");
        }
        #region 生成验证码
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public byte[] GenerateValidateCode()
        {

            //首先实例化验证码的类
            KenceryValidateCode validateCode = new KenceryValidateCode();
            //生成验证码指定的长度
            string code = validateCode.CreateValidateCode(5);
            //string code = "11111";
            //将验证码赋值给Session变量
            //Session["ValidateCode"] = code;
            oc.CurrentUserVcode = code;
            //创建验证码的图片
            byte[] bytes = validateCode.CreateValidateGraphic(code);
            return bytes;
            //最后将验证码返回
            //return File(bytes, @"image/jpeg");
        }
        #endregion
        [HttpGet]
        public ActionResult CheckWordCode()
        {
            using (MemoryStream m = new MemoryStream())
            {
                VerificationCode va = new VerificationCode(105, 30);
                var s = va.Create(m);
                string code = va.IdentifyingCode.ToLower();//string code = Common.DEncrypt.DESEncrypt.Encrypt(va.IdentifyingCode.ToLower());
                oc.CurrentUserVcode = code;
                byte[] bytes = m.ToArray();
                return File(bytes, @"image/gif");
            }
        }
        #endregion


        #region 退出系统
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        [Description("退出登录")]
        public JsonResult LoginOut()
        {
            //清除Sesson
            Session.Abandon();
            //Session.Clear();
            oc.CurrentUser = null;
            oc.UserMenuPermission = null;
            if (Request.Cookies["systemLoginName"] != null)
            {
                HttpCookie tmpCooki = new HttpCookie("systemLoginName");

                tmpCooki.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(tmpCooki);
            }

            return PackagingAjaxmsg(AjaxStatu.ok, "", null, "/Home/Login");
            
        }
        #endregion

    }
}