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
    public class ApplyController : BaseController
    {
        // GET: COMPANY/Apply

        #region Identity
        private IBaseService bs = null;
        public ApplyController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion

        [Description("企业申请")]
        [HttpGet]
        public ActionResult Zxsq(string sid,string code)
        {
            var model=new VIEW_POLICY_MAIN_INFO() { };
            try
            {
                if (!string.IsNullOrEmpty(code)) {
                    model = bs.Entities<BASE_PROJECT_INFO>().Where(u => u.ITEM_CODE.Equals(code)).Select(u => new VIEW_POLICY_MAIN_INFO { SID="", APPLY_ITEM_TYPE =u.ITEM_TYPE, APPLY_ITEM_NAME= code.ToUpper()+u.ITEM_NAME }).FirstOrDefault();
                }
                if (!string.IsNullOrEmpty(sid))
                {
                    model = bs.Entities<POLICY_MAIN_INFO>().Where(u => u.SID.Equals(sid))
                            .Select(u => new VIEW_POLICY_MAIN_INFO
                            {
                                SID = u.SID,
                                APPLY_NUMBER = u.APPLY_NUMBER,
                                APPLY_ITEM_TYPE = u.APPLY_ITEM_TYPE,
                                APPLY_ITEM_NAME = u.APPLY_NUMBER.Substring(0, 4) + u.APPLY_ITEM_NAME,

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
                                BANK_NAME=u.BANK_NAME,
                                BANK_ACOUNT=u.BANK_ACOUNT,
                                VAT_NO = u.VAT_NO,
                                COMPANY_NAME = u.COMPANY_NAME,
                                COMPANY_ADDRESS = u.COMPANY_ADDRESS,
                                COMPANY_PHONE = u.COMPANY_PHONE,
                                REJECT_REASON = u.REJECT_REASON
                            }).FirstOrDefault();
                }

            }
            catch (Exception)
            {
            }
            if (model == null) { model = new VIEW_POLICY_MAIN_INFO() { }; };
            return View(model);
        }

        [Description("办件查询")]
        [HttpGet]
        public ActionResult Bjcx()
        {
            return View();
        }
        
        [Description("申请详情")]
        [HttpGet]
        public ActionResult Sqxx(string id)
        {
            VIEW_POLICY_MAIN_INFO MAIN_INFOEntity = bs.Entities<POLICY_MAIN_INFO>().Where(u => u.SID.Equals(id))
                .Select(u => new VIEW_POLICY_MAIN_INFO {
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
                    COMPANY_ADDRESS = u.COMPANY_ADDRESS,
                    COMPANY_PHONE = u.COMPANY_PHONE,
                    REJECT_REASON = u.REJECT_REASON
                }).FirstOrDefault();
            
            return View(MAIN_INFOEntity);
        }

        [Description("申请审批")]
        [HttpGet]
        public ActionResult Sqpass(string id)
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
                    COMPANY_ADDRESS = u.COMPANY_ADDRESS,
                    COMPANY_PHONE = u.COMPANY_PHONE,
                    REJECT_REASON = u.REJECT_REASON
                }).FirstOrDefault();

            return View(MAIN_INFOEntity);
        }
        [Description("银行信息")]
        [HttpGet]
        public ActionResult Bankxx(string id)
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
                    COMPANY_NAME=u.COMPANY_NAME,
                    COMPANY_ADDRESS = u.COMPANY_ADDRESS,
                    COMPANY_PHONE = u.COMPANY_PHONE,
                    REJECT_REASON = u.REJECT_REASON
                }).FirstOrDefault();

            return View(MAIN_INFOEntity);
        }


        
        [Description("修改密码")]
        [HttpGet]
        public ActionResult EditPass()
        {
            return View();
        }

        [Description("企业信息")]
        [HttpGet]
        public ActionResult CompanyXx()
        {
            VIEW_CORPORATION_BASE_INFO MAIN_INFOEntity = bs.Entities<CORPORATION_BASE_INFO>().Where(u => u.USER_NAME.Equals(oc.CompanyUser.USER_NAME))
                .Select(u => new VIEW_CORPORATION_BASE_INFO
                {
                    SID = u.SID,

                    CREATE_DT = u.CREATE_DT,
                    CREATE_BY = u.CREATE_BY,
                    UPDATE_DT = u.UPDATE_DT,
                    UPDATE_BY = u.UPDATE_BY,
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
                    USER_NAME = u.USER_NAME,
                    APPLY_RESULT=u.APPLY_RESULT
                }).FirstOrDefault();

            return View(MAIN_INFOEntity);
        }

        [HttpGet]
        [AjaxRequest]
        public ActionResult ProjectType()
        {
            AjaxMsgModel amm = new Message().NewAmm;
                try
                {
                    List<string> listName = bs.Entities<BASE_PROJECT_INFO>().GroupBy(u=>u.ITEM_TYPE).Select(u => u.Key).ToList();
                    amm.Data = listName;
                    amm.Statu = AjaxStatu.ok;
                }
                catch (Exception)
                {
                }
            return PackagingAjaxmsg(amm);
        }
        [HttpGet]
        [AjaxRequest]
        public ActionResult ProjectInfo(string id)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    List<EasyUIComBoBoxNode> listName = bs.Entities<BASE_PROJECT_INFO>().Where(o => o.ITEM_TYPE == id).OrderBy(u=>u.ITEM_CODE).Select(u =>new EasyUIComBoBoxNode { id = u.ITEM_CODE, text = u.ITEM_NAME }).ToList();
                    amm.Data = listName;
                    amm.Statu = AjaxStatu.ok;
                }
                catch (Exception)
                {
                }
            }
            return PackagingAjaxmsg(amm);
        }
    }

    public class ApplyApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public ApplyApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion


        #region 查询
        [System.Web.Http.HttpPost]//查询
        [AjaxRequest]
        public ViewModelLayui List(VIEW_POLICY_MAIN_INFO data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;
            //查询条件
            IQueryable<POLICY_MAIN_INFO> MAIN_INFOEntity = bs.Entities<POLICY_MAIN_INFO>();
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
                             DATA_STATUS = u.DATA_STATUS
                         }).ToList();
            return ObjToJson.ViewModelToJsonLayui(listROLE, total);
        }
        #endregion

        #region 保存
        [HttpPost]
        [ValidateInput(false)]
        [AjaxRequest]
        public AjaxMsgModel CompanyApply(VIEW_POLICY_MAIN_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            Mapper.CreateMap<VIEW_POLICY_MAIN_INFO, POLICY_MAIN_INFO>();
            POLICY_MAIN_INFO u = Mapper.Map<VIEW_POLICY_MAIN_INFO, POLICY_MAIN_INFO>(data);
            bool addFlag = false;

            if (string.IsNullOrEmpty(u.SID))
            {
                addFlag = true;
                u.SID = Guid.NewGuid().ToString();
                u.CREATE_DT = DateTime.Now;
                u.CREATE_BY = oc.CompanyUser.USER_NAME;
                u.CORPORATION_SID = oc.CompanyUser.DEPT_CODE;
            }
            u.UPDATE_DT = DateTime.Now;
            u.UPDATE_BY = oc.CompanyUser.USER_NAME;

            u.APPLY_STATUS = "通过";
            u.DATA_STATUS = "提交";

            string itemcode = data.APPLY_ITEM_NAME.Substring(4);//下面判断文件编码用
            u.APPLY_ITEM_NAME = itemcode;
            bool codeFlag = true;
            if (!string.IsNullOrEmpty(u.APPLY_NUMBER) && data.APPLY_ITEM_NAME.Substring(0, 4) == data.APPLY_NUMBER.Substring(0,4))
            {
                codeFlag = false;//如果类型没有修改
            }
            if (codeFlag)//重新生成编码
            {
                u.APPLY_NUMBER = data.APPLY_ITEM_NAME.Substring(0, 4) + DateTime.Now.Year.ToString().Substring(2);
                string maxcode = "001";
                try
                {
                    maxcode = bs.Entities<POLICY_MAIN_INFO>().Where(m => m.APPLY_NUMBER.StartsWith(u.APPLY_NUMBER)).OrderByDescending(m => m.APPLY_NUMBER.Length).ThenByDescending(m => m.APPLY_NUMBER).Select(m => m.APPLY_NUMBER).FirstOrDefault();
                    if (maxcode != null)
                    {
                        maxcode = (int.Parse(maxcode.Substring(6)) + 1).ToString().PadLeft(3, '0');
                    }
                    else
                    {
                        maxcode = "001";
                    }
                }
                catch (Exception)
                {
                    maxcode = "001";
                }
                u.APPLY_NUMBER = u.APPLY_NUMBER + maxcode;
            }
            List <POLICY_APPLY_FILE> listFiles = new List<POLICY_APPLY_FILE>();
            string[] filenames = data.FILENAME.Split('|');
            string[] filecodes = data.FILECODE.Split('|');
            string[] filepaths = data.FILEPATH.Split('|');
            for (int i = 0; i < filenames.Length-1; i++)
            {
                string filecode = filecodes[i];
                if (bs.Entities<POLICY_APPLY_FILE>().Where(o => o.FILE_NAME.StartsWith(itemcode) && o.PATENT_NUMBER.Equals(filecode)).Count() > 0)
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "文件编号" +filecodes[i] + "已经申请过此项目！";
                    return amm;
                }
                listFiles.Add(new POLICY_APPLY_FILE()
                {
                    SID = Guid.NewGuid().ToString(),
                    CREATE_DT = DateTime.Now,
                    CREATE_BY = oc.CompanyUser.USER_NAME,
                    UPDATE_DT = DateTime.Now,
                    UPDATE_BY = oc.CompanyUser.USER_NAME,
                    FILE_CLASS = "2",
                    FILE_TYPE = filepaths[i].Split('.')[1],
                    FILE_NAME = u.APPLY_NUMBER,//这里用作判断项目名称
                    DOCUMENT_NAME= filenames[i],
                    PATENT_NUMBER= filecodes[i],
                    FILE_PATH = filepaths[i],
                    MAIN_SID = u.SID
                });
            }
            if (addFlag)
            {
                return Model_POLICY_MAIN_INFO.Create.Add(u, listFiles);
            }
            else
            {
                return Model_POLICY_MAIN_INFO.Create.Edit(u, listFiles);
            }
        }
        #endregion


        #region 查询附件
        [System.Web.Http.HttpPost]//查询
        [AjaxRequest]
        public AjaxMsgModel FileList(VIEW_POLICY_APPLY_FILE data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Statu = AjaxStatu.ok;
            //查询条件
            IQueryable<POLICY_APPLY_FILE> FILEEntity = bs.Entities<POLICY_APPLY_FILE>();
            if (!string.IsNullOrEmpty(data.MAIN_SID))
            {
                FILEEntity = FILEEntity.Where(u => u.MAIN_SID.Equals(data.MAIN_SID));
            }
            amm.Data = FILEEntity.OrderByDescending(u => u.CREATE_DT)
                         .Select(u => new
                         {
                             SID = u.SID,
                             DOCUMENT_NAME = u.DOCUMENT_NAME,
                             PATENT_NUMBER = u.PATENT_NUMBER,
                             FILE_PATH = u.FILE_PATH,
                             FILE_NAME = u.FILE_NAME

                         }).ToList();
            return amm;
        }
        #endregion


        #region 保存银行帐号
        [HttpPost]
        [ValidateInput(false)]
        [AjaxRequest]
        public AjaxMsgModel CompanyApplyBank(VIEW_POLICY_MAIN_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            Mapper.CreateMap<VIEW_POLICY_MAIN_INFO, POLICY_MAIN_INFO>();
            POLICY_MAIN_INFO u = Mapper.Map<VIEW_POLICY_MAIN_INFO, POLICY_MAIN_INFO>(data);
            
            #region 原处理方式
            //int returnValue = bs.UpdateEntity(u, new string[] { "BANK_NAME", "BANK_ACOUNT", "VAT_NO", "COMPANY_NAME" });

            //if (returnValue > 0)
            //{
            //    amm.Statu = AjaxStatu.ok;
            //    amm.Msg = "提交成功";
            //}
            //else
            //{
            //    amm.Statu = AjaxStatu.err;
            //    amm.Msg = "提交失败";
            //} 
            #endregion
            amm = Model_POLICY_MAIN_INFO.Create.CompanyApplyBank(u);
            return amm;
        }
        #endregion

        #region 修改密码
        [HttpPost]
        [ValidateInput(false)]
        [AjaxRequest]
        public AjaxMsgModel UpdatePass(VIEW_CORPORATION_BASE_INFO data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            if (data.NEW_PASSWORD != data.TWO_PASSWORD)
            {
                amm.Statu = AjaxStatu.err;
                amm.Msg = "新密码与确认密码不一致!";
                return amm;
            }
            string oldpass = DataHelper.TOMD5(data.PASSWORD);
            var user = bs.Entities<CORPORATION_BASE_INFO>().Where(u => u.USER_NAME == oc.CompanyUser.USER_NAME && u.PASSWORD == oldpass).Select(u => new
            {
                SID = u.SID
            }).FirstOrDefault();

            if (user !=null)
            {
                CORPORATION_BASE_INFO u = new CORPORATION_BASE_INFO()
                {
                    SID = user.SID,
                    PASSWORD = DataHelper.TOMD5(data.NEW_PASSWORD)
                };
                int returnValue = bs.UpdateEntity(u, new string[] { "PASSWORD" });

                if (returnValue > 0)
                {
                    amm.Statu = AjaxStatu.ok;
                    amm.Msg = "修改成功";
                }
                else
                {
                    amm.Statu = AjaxStatu.err;
                    amm.Msg = "修改失败";
                }
                return amm;
                
            }
            else
            {
                amm.Statu = AjaxStatu.err;
                amm.Msg = "旧密码不正确!";
                return amm;
            }

        }
        #endregion

        #region 删除
        [System.Web.Http.HttpPost]
        public AjaxMsgModel Del(VIEW_POLICY_MAIN_INFO data)
        {
            return Model_POLICY_MAIN_INFO.Create.Del(data.SID);
        }
        #endregion


        #region 确认收款
        [System.Web.Http.HttpPost]
        public AjaxMsgModel Over(VIEW_POLICY_MAIN_INFO data)
        {
            return Model_POLICY_MAIN_INFO.Create.Over(data.SID);
        }
        #endregion

    }
}