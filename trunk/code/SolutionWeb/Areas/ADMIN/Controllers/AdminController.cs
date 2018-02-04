using SolutionWeb.Common;
using SolutionWeb.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.ADMIN.Controllers
{
    public class AdminController : BaseController
    {
        // GET: ADMIN/Admin

        #region Identity
        private IBaseService bs = null;
        private IUserLogin ul = null;
        public AdminController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        [HttpGet]
        [Skip]
        public ActionResult Index()
        {
            ViewBag.CurrentUser = oc.CurrentUser.ZSNAME + "(" + oc.CurrentUser.USER_NAME + ")";

            return View();
        }


        #region 修改密码
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        [Description("修改密码")]
        public JsonResult EditPass()
        {
            if (Request.Form["txtoldpass"] == null || Request.Form["txtnewpass"] == null || Request.Form["txtrepass"] == null)
            {
                return PackagingAjaxmsg(AjaxStatu.err, string.Format(Message.ParGetFail, "密码"));
            }
            string txtoldpass = Request["txtoldpass"].ToString();
            string txtnewpass = Request["txtnewpass"].ToString();
            string txtrepass = Request["txtrepass"].ToString();
            if (txtnewpass != txtrepass)
            {
                return PackagingAjaxmsg(AjaxStatu.err, string.Format(Message.MisMatch, "新密码"));
            }
            AjaxMsgModel amm = ul.EditPass(oc.CurrentUser.USER_NAME, txtoldpass, txtnewpass);
            return PackagingAjaxmsg(amm);
        }
        #endregion
    }
}