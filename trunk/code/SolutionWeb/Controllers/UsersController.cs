using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.POLICY;
using SolutionWeb.User.ViewModels;
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
    public class UsersController : BaseController
    {

        #region Identity
        private IBaseService bs = null;
        private ICompanyLogin cl = null;
        public UsersController(IBaseService baseService, ICompanyLogin companyLogin)
        {
            this.bs = baseService;
            this.cl = companyLogin;
        }
        #endregion

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CompanyLogin()
        {
            return View();
        }


        [HttpGet]
        public ActionResult CompanyRegister()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Sqzn()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Tzgg()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Xxgs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ht()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ph()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ps()
        {
            return View();
        }

        [Description("申请详情")]
        [HttpGet]
        public ActionResult Xxgsxx(string id)
        {
            VIEW_POLICY_MAIN_INFO MAIN_INFOEntity = bs.Entities<POLICY_MAIN_INFO>().Where(u => u.SID.Equals(id))
                .Select(u => new VIEW_POLICY_MAIN_INFO
                {
                    SID = u.SID,
                    APPLY_NUMBER = u.APPLY_NUMBER,
                    APPLY_ITEM_TYPE = u.APPLY_ITEM_TYPE,
                    APPLY_ITEM_NAME = u.APPLY_ITEM_NAME,

                    CREATE_DT = u.CREATE_DT,
                    CREATE_BY = u.CREATE_BY,
                    UPDATE_DT = u.UPDATE_DT,
                    UPDATE_BY = u.UPDATE_BY,
                    STATUS_NAME = u.STATUS_NAME,
                    STATUS_CODE = u.STATUS_CODE,
                    CORPORATION_SID = u.CORPORATION_SID,
                    CORP_NAME = u.CORP_NAME,
                    SOCIAL_CREDIT_CODE = u.SOCIAL_CREDIT_CODE,
                    REGISTERED_ADDRESS = u.REGISTERED_ADDRESS,
                    LEGAL_PERSON = u.LEGAL_PERSON,
                    LEGAL_PERSON_PHONE = u.LEGAL_PERSON_PHONE,
                    OPERATOR = u.OPERATOR,
                    OPERATOR_PHONE = u.OPERATOR_PHONE,
                    OPERATOR_ID_NO = u.OPERATOR_ID_NO,
                    EMIAL = u.EMIAL,
                    REGISTERED_CAPITAL = u.REGISTERED_CAPITAL,
                    APPLY_MONEY_WORDS = u.APPLY_MONEY_WORDS,
                    APPLY_MONEY_NUMBER = u.APPLY_MONEY_NUMBER,
                    APPLY_DT = u.APPLY_DT,
                    APPLY_STATUS = u.APPLY_STATUS,
                    DATA_STATUS = u.DATA_STATUS,
                    BANK_NAME = u.BANK_NAME,
                    BANK_ACOUNT = u.BANK_ACOUNT,
                    VAT_NO = u.VAT_NO,
                    COMPANY_NAME = u.COMPANY_NAME,
                    POLICITY_STATUS = u.POLICITY_STATUS,
                    POLICITY_BEGIN_DT = u.POLICITY_BEGIN_DT,
                    POLICITY_END_DT = u.POLICITY_END_DT
                }).FirstOrDefault();

            return View(MAIN_INFOEntity);
        }


        [Description("公告详情")]
        [HttpGet]
        public ActionResult Tzggxx(string id)
        {
            VIEW_POLICY_NOTICE_INFO MAIN_INFOEntity = bs.Entities<POLICY_NOTICE_INFO>().Where(u => u.SID.Equals(id))
                .Select(u => new VIEW_POLICY_NOTICE_INFO
                {
                    SID = u.SID,
                    CREATE_DT = u.CREATE_DT,
                    CREATE_BY = u.CREATE_BY,
                    UPDATE_DT = u.UPDATE_DT,
                    UPDATE_BY = u.UPDATE_BY,
                    NOTICE_TITLE = u.NOTICE_TITLE,
                    NOTICE_CONTENT = u.NOTICE_CONTENT,
                    IS_SHOW = u.IS_SHOW
                }).FirstOrDefault();

            return View(MAIN_INFOEntity);
        }

        #region 登录验证，只允许POST提交
        /// <summary>
        /// 登录验证，只允许POST提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CompanyLoginIn()
        {
            if (Request.Form["username"] == null || Request.Form["password"] == null || Request.Form["checkcode"] == null)
            {
                return PackagingAjaxmsg(AjaxStatu.err, "登录名|密码|验证码-不能为空!");
            }
            string username = Request["username"].ToString();
            string password = Request["password"].ToString();
            string checkcode = Request["checkcode"].ToString();

            if (!checkcode.Equals(oc.CurrentUserVcode))
            {
                AjaxMsgModel amm = new Message().NewAmm;
                amm.Msg = "验证码不正确";
                return PackagingAjaxmsg(amm);
            }
            else
            {
                AjaxMsgModel amm = cl.CompanyLoginIn(username, password, checkcode);
                return PackagingAjaxmsg(amm);
            }

        }
        #endregion
        

        #region 退出系统
        /// <summary>
        /// 退出系统
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AjaxRequest]
        [Description("退出登录")]
        public JsonResult CompanyLoginOut()
        {
            oc.CompanyUser = null;
            return PackagingAjaxmsg(AjaxStatu.ok, "", null, "/Users/CompanyLogin");
            
        }
        #endregion


        [HttpPost]
        [AjaxRequest]
        public ActionResult UploadSingleFile()
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm = new UploadFile().ToUpLoad(Request.Files, amm, 200, 150, "UpLoad");
            return PackagingAjaxmsg(amm);
        }
    }

    public class UsersApiController :BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public UsersApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        #region 申请指南查询
        [HttpPost]//查询
        [AjaxRequest]
        public ViewModelLayui ItemList(VIEW_BASE_PROJECT_INFO data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;
            //查询条件
            IQueryable<BASE_PROJECT_INFO> MAIN_INFOEntity = bs.Entities<BASE_PROJECT_INFO>();
            if (!string.IsNullOrEmpty(data.ITEM_TYPE))
            {
                MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.ITEM_TYPE.Equals(data.ITEM_TYPE));
            }
            if (!string.IsNullOrEmpty(data.ITEM_NAME))
            {
                MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.ITEM_NAME.Contains(data.ITEM_NAME));
            }
            int total = 0;
            total = MAIN_INFOEntity.Count();
            var listROLE = MAIN_INFOEntity.OrderByDescending(u => u.ITEM_TYPE).ThenBy(u=>u.ITEM_CODE)
                            .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                         .Select(u => new
                         {
                             SID = u.SID,
                             ITEM_TYPE = u.ITEM_TYPE,
                             ITEM_NAME = u.ITEM_NAME,
                             CASHING_WAY = u.CASHING_WAY,
                             LAY_ORDER = u.LAY_ORDER,
                             ITEM_CODE = u.ITEM_CODE

                         }).ToList();
            return ObjToJson.ViewModelToJsonLayui(listROLE, total);
        }
        #endregion

        #region 信息公示查询
        [HttpPost]//查询
        [AjaxRequest]
        public ViewModelLayui XxgsList(VIEW_POLICY_MAIN_INFO data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;
            //查询条件
            IQueryable<POLICY_MAIN_INFO> MAIN_INFOEntity = bs.Entities<POLICY_MAIN_INFO>();
            MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.STATUS_CODE.Equals("C02") && u.POLICITY_STATUS.Equals("正常"));
            DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.POLICITY_BEGIN_DT <= today && today <= u.POLICITY_END_DT);
            if (!string.IsNullOrEmpty(data.APPLY_ITEM_TYPE))
            {
                MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.APPLY_ITEM_TYPE.Equals(data.APPLY_ITEM_TYPE));
            }
            if (!string.IsNullOrEmpty(data.APPLY_ITEM_NAME))
            {
                MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.APPLY_ITEM_NAME.Contains(data.APPLY_ITEM_NAME));
            }
            int total = 0;
            total = MAIN_INFOEntity.Count();
            var listROLE = MAIN_INFOEntity.OrderByDescending(u => u.CREATE_DT)
                            .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                         .Select(u => new
                         {
                             SID = u.SID,
                             APPLY_NUMBER = u.APPLY_NUMBER,
                             APPLY_ITEM_TYPE = u.APPLY_ITEM_TYPE,
                             APPLY_ITEM_NAME = u.APPLY_ITEM_NAME,

                             CREATE_DT = u.CREATE_DT,
                             CREATE_BY = u.CREATE_BY,
                             UPDATE_DT = u.UPDATE_DT,
                             UPDATE_BY = u.UPDATE_BY,
                             STATUS_NAME = u.STATUS_NAME,
                             STATUS_CODE = u.STATUS_CODE,
                             CORPORATION_SID = u.CORPORATION_SID,
                             CORP_NAME = u.CORP_NAME,
                             SOCIAL_CREDIT_CODE = u.SOCIAL_CREDIT_CODE,
                             REGISTERED_ADDRESS = u.REGISTERED_ADDRESS,
                             LEGAL_PERSON = u.LEGAL_PERSON,
                             LEGAL_PERSON_PHONE = u.LEGAL_PERSON_PHONE,
                             OPERATOR = u.OPERATOR,
                             OPERATOR_PHONE = u.OPERATOR_PHONE,
                             OPERATOR_ID_NO = u.OPERATOR_ID_NO,
                             EMIAL = u.EMIAL,
                             REGISTERED_CAPITAL = u.REGISTERED_CAPITAL,
                             APPLY_MONEY_WORDS = u.APPLY_MONEY_WORDS,
                             APPLY_MONEY_NUMBER = u.APPLY_MONEY_NUMBER,
                             APPLY_DT = u.APPLY_DT,
                             APPLY_STATUS = u.APPLY_STATUS,
                             DATA_STATUS = u.DATA_STATUS,
                             POLICITY_STATUS=u.POLICITY_STATUS,
                             POLICITY_BEGIN_DT = u.POLICITY_BEGIN_DT,
                             POLICITY_END_DT = u.POLICITY_END_DT,
                         }).ToList();
            return ObjToJson.ViewModelToJsonLayui(listROLE, total);
        }
        #endregion


        #region 信息公示查询
        [HttpPost]//查询
        [AjaxRequest]
        public ViewModelLayui XxgsIndex()
        {
            IQueryable<POLICY_MAIN_INFO> MAIN_INFOEntity = bs.Entities<POLICY_MAIN_INFO>();
            
            MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.STATUS_CODE.Equals("C02") && u.POLICITY_STATUS.Equals("正常"));
            DateTime today = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
            MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.POLICITY_BEGIN_DT <= today && today <= u.POLICITY_END_DT);

            int total = 0;
            var listROLE = MAIN_INFOEntity.OrderByDescending(u => u.CREATE_DT)
                         .Select(u => new
                         {
                             SID = u.SID,
                             APPLY_NUMBER = u.APPLY_NUMBER,
                             APPLY_ITEM_TYPE = u.APPLY_ITEM_TYPE,
                             APPLY_ITEM_NAME = u.APPLY_ITEM_NAME,

                             CREATE_DT = u.CREATE_DT,
                             CREATE_BY = u.CREATE_BY,
                             UPDATE_DT = u.UPDATE_DT,
                             UPDATE_BY = u.UPDATE_BY,
                             STATUS_NAME = u.STATUS_NAME,
                             STATUS_CODE = u.STATUS_CODE,
                             CORPORATION_SID = u.CORPORATION_SID,
                             CORP_NAME = u.CORP_NAME,
                             SOCIAL_CREDIT_CODE = u.SOCIAL_CREDIT_CODE,
                             REGISTERED_ADDRESS = u.REGISTERED_ADDRESS,
                             LEGAL_PERSON = u.LEGAL_PERSON,
                             LEGAL_PERSON_PHONE = u.LEGAL_PERSON_PHONE,
                             OPERATOR = u.OPERATOR,
                             OPERATOR_PHONE = u.OPERATOR_PHONE,
                             OPERATOR_ID_NO = u.OPERATOR_ID_NO,
                             EMIAL = u.EMIAL,
                             REGISTERED_CAPITAL = u.REGISTERED_CAPITAL,
                             APPLY_MONEY_WORDS = u.APPLY_MONEY_WORDS,
                             APPLY_MONEY_NUMBER = u.APPLY_MONEY_NUMBER,
                             APPLY_DT = u.APPLY_DT,
                             APPLY_STATUS = u.APPLY_STATUS,
                             DATA_STATUS = u.DATA_STATUS,
                             POLICITY_STATUS = u.POLICITY_STATUS,
                             POLICITY_BEGIN_DT = u.POLICITY_BEGIN_DT,
                             POLICITY_END_DT = u.POLICITY_END_DT
                         }).ToList();
            return ObjToJson.ViewModelToJsonLayui(listROLE, total);
        }
        #endregion

        #region 通知公告查询
        [HttpPost]//查询
        [AjaxRequest]
        public ViewModelLayui TzggList(VIEW_POLICY_NOTICE_INFO data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;
            //查询条件
            IQueryable<POLICY_NOTICE_INFO> MAIN_INFOEntity = bs.Entities<POLICY_NOTICE_INFO>().Where(u => u.IS_SHOW.Equals("是"));
            if (!string.IsNullOrEmpty(data.NOTICE_TITLE))
            {
                MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.NOTICE_TITLE.Contains(data.NOTICE_TITLE));
            }
            int total = 0;
            total = MAIN_INFOEntity.Count();
            var listROLE = MAIN_INFOEntity.OrderByDescending(u => u.CREATE_DT)
                            .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                         .Select(u => new
                         {
                             SID = u.SID,
                             CREATE_DT = u.CREATE_DT,
                             CREATE_BY = u.CREATE_BY,
                             UPDATE_DT = u.UPDATE_DT,
                             UPDATE_BY = u.UPDATE_BY,
                             NOTICE_TITLE = u.NOTICE_TITLE,
                             NOTICE_CONTENT = u.NOTICE_CONTENT,
                             IS_SHOW = u.IS_SHOW

                         }).ToList();
            return ObjToJson.ViewModelToJsonLayui(listROLE, total);
        }
        #endregion
        #region 通知公告查询
        [HttpPost]//查询
        [AjaxRequest]
        public ViewModelLayui TzggIndex()
        {
            //查询条件
            IQueryable<POLICY_NOTICE_INFO> MAIN_INFOEntity = bs.Entities<POLICY_NOTICE_INFO>().Where(u => u.IS_SHOW.Equals("是"));
            
             //   MAIN_INFOEntity = MAIN_INFOEntity.Where(u => u.NOTICE_TITLE.Contains(data.NOTICE_TITLE));
            
            int total = 0;
            var listROLE = MAIN_INFOEntity.OrderByDescending(u => u.CREATE_DT)
                         .Select(u => new
                         {
                             SID = u.SID,
                             CREATE_DT = u.CREATE_DT,
                             CREATE_BY = u.CREATE_BY,
                             UPDATE_DT = u.UPDATE_DT,
                             UPDATE_BY = u.UPDATE_BY,
                             NOTICE_TITLE = u.NOTICE_TITLE,
                             NOTICE_CONTENT = u.NOTICE_CONTENT,
                             IS_SHOW = u.IS_SHOW

                         }).ToList();
            return ObjToJson.ViewModelToJsonLayui(listROLE, total);
        }
        #endregion
    }
}