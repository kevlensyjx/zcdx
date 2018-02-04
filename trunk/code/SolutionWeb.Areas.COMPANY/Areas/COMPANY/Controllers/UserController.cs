using AutoMapper;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.POLICY;
using SolutionWeb.User.Models;
using SolutionWeb.User.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.COMPANY.Controllers
{
    public class UserController : BaseController
    {
        // GET: COMPANY/User
        #region Identity
        private IBaseService bs = null;
        public UserController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion
        
        [Description("企业中心")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }


    public class UserApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public UserApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion



        #region 保存
        [HttpPost]
        [ValidateInput(false)]
        [AjaxRequest]
        public AjaxMsgModel CompanyRegisterIn(VIEW_CORPORATION_BASE_INFO data)
        {
            if (!data.checkcode.Equals(oc.CurrentUserVcode))
            {
                AjaxMsgModel amm = new Message().NewAmm;
                amm.Msg = "验证码不正确";
                return amm;
            }
            if (!string.IsNullOrEmpty(data.USER_NAME) && data.USER_NAME != "null")
            {
                if (bs.Entities<CORPORATION_BASE_INFO>().Where(o => o.USER_NAME == data.USER_NAME).Count() > 0)
                {
                    AjaxMsgModel amm = new Message().NewAmm;
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "企业用户名已经存在！";
                    return amm;
                }
            }
            Mapper.CreateMap<VIEW_CORPORATION_BASE_INFO, CORPORATION_BASE_INFO>();
            CORPORATION_BASE_INFO u = Mapper.Map<VIEW_CORPORATION_BASE_INFO, CORPORATION_BASE_INFO>(data);
            u.SID = Guid.NewGuid().ToString(); 
            u.CREATE_DT = DateTime.Now;
            u.UPDATE_DT = DateTime.Now;
            u.PASSWORD = DataHelper.TOMD5(u.PASSWORD);
            u.CORP_STATUS = "提交";

            POLICY_APPLY_FILE yyzzfile = new POLICY_APPLY_FILE()
            {
                SID = Guid.NewGuid().ToString(),
                CREATE_DT = DateTime.Now,
                FILE_CLASS = "1",
                FILE_TYPE = "jpg",
                FILE_NAME = "营业执照",
                FILE_PATH = data.FilePathYyzz,
                MAIN_SID = u.SID
            };
            POLICY_APPLY_FILE zzjgfile = new POLICY_APPLY_FILE()
            {
                SID = Guid.NewGuid().ToString(),
                CREATE_DT = DateTime.Now,
                FILE_CLASS = "1",
                FILE_TYPE = "jpg",
                FILE_NAME = "组织机构证",
                FILE_PATH = data.FilePathZzjg,
                MAIN_SID = u.SID
            };
            return Model_CORPORATION_BASE_INFO.Create.Add(u, yyzzfile, zzjgfile);

        }
        #endregion

        
    }
}