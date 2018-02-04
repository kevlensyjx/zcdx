using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.Attributes;
using SxShWeb.Areas.Models;
using SxShWeb.Controllers;
using System.Data.Entity;
using System.Transactions;
using Model;
using SxShWeb.Areas.ViewModels;
using AutoMapper;

namespace SxShWeb.Areas.GATE.Controllers
{
    public class ACCESS_GATE_BDATA_CARDController : BaseController
    {
        //
        // GET: /GATE/ACCESS_GATE_BDATA_CARD/
        [AjaxRequest]
        [Description("查看门卡")]
        [HttpGet]
        public ActionResult Index()
        {
            string ControllerUrl = "/api/GATE/ACCESS_GATE_BDATA_CARD/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = Model_SYS_MENU.HasPermission("GATE", "ACCESS_GATE_BDATA_CARD", "List", HttpMethod.Post),
                    a_add = Model_SYS_MENU.HasPermission("GATE", "ACCESS_GATE_BDATA_CARD", "Add", HttpMethod.Post),
                    a_edit = Model_SYS_MENU.HasPermission("GATE", "ACCESS_GATE_BDATA_CARD", "Edit", HttpMethod.Get),
                    a_del = Model_SYS_MENU.HasPermission("GATE", "ACCESS_GATE_BDATA_CARD", "Del", HttpMethod.Get),
                    a_unbind = Model_SYS_MENU.HasPermission("GATE", "ACCESS_GATE_BDATA_CARD", "Edit", HttpMethod.Get),
                    //a_abandon = true,
                },
                resx = new
                {
                    listTitle = "您没有【查看门卡】权限",
                    addTitle = "您没有【新增门卡】权限",
                    editTitle = "您没有【编辑门卡】权限！",
                    deleteTitle = "您没有【删除门卡】权限！",
                },
                urls = new//请求URL
                {
                    save = ControllerUrl + "Save",
                    list = ControllerUrl + "List",
                    edit = ControllerUrl + "Edit",
                    del = ControllerUrl + "Del",
                    unbind = ControllerUrl + "unBind",
                    abandon = ControllerUrl + "Abandon",
                    dataGgridName = "data_grid",//列表ID
                    dataGgridType = "datagrid",//列表类型
                    dataAddName = "data_add",//增加窗口
                    dataFormName = "DataForm",//提交表单
                },
                searchForm = new VIEW_GATE_BDATA_CARD()//查询
                {
                },
                addForm = new VIEW_GATE_BDATA_CARD()
                { //添加修改
                },
                extForm = new//扩展类
                {
                    //部门和人
                    extA = Model_GATE_BDATA_CARD.GetORGMemberEasyUITree(oc.CurrentUser.SYS_DEPT.DEPT_CODE, oc.CurrentUser.SYS_DEPT.PARENT_CODE),
                    extB = Model_GATE_BDATA_CARD.GetMyORGEasyUITree(oc.CurrentUser.SYS_DEPT.DEPT_CODE, oc.CurrentUser.SYS_DEPT.PARENT_CODE),
                    extC = Model_GATE_BDATA_TOOL.GetToolEasyUiTreeNodes(),
                    //extD = Model_GATE_BDATA_GATE.GetORGGateTreeList(oc.CurrentUser.SYS_DEPT.DEPT_CODE, oc.CurrentUser.SYS_DEPT.PARENT_CODE,"bj"),//门禁
                    extE = Model_GATE_BDATA_CARD.GetORGMemberEasyUITree(oc.CurrentUser.SYS_DEPT.DEPT_CODE, oc.CurrentUser.SYS_DEPT.PARENT_CODE)//手机状态
                }
            };
            return View(viewModel);
        }

        #region 获取人员信息
        [HttpPost]
        [AjaxRequest]
        [Skip]
        [Description("查看人员信息")]
        public ActionResult GetORGMemberTree()
        {
            return Content(Model_GATE_BDATA_CARD.GetORGMemberTree(oc.CurrentUser.SYS_DEPT.DEPT_CODE, oc.CurrentUser.SYS_DEPT.PARENT_CODE));
        }
        #endregion

        #region 查看我的组织机构树
        [Skip]
        [AjaxRequest]
        [HttpPost]
        [Description("查看我的组织机构树")]
        public ActionResult GetMyORGTree()
        {
            return Content(Model_GATE_BDATA_CARD.GetMyORGTree(oc.CurrentUser.SYS_DEPT.DEPT_CODE, oc.CurrentUser.SYS_DEPT.PARENT_CODE));
        }
        #endregion

        #region 获取持卡人信息
        [HttpPost]
        [AjaxRequest]
        [Skip]
        [Description("获取持卡人信息")]
        public ActionResult GetORGCardTree()
        {
            return Content(Model_GATE_BDATA_CARD.GetORGCardTree(oc.CurrentUser.SYS_DEPT.DEPT_CODE, oc.CurrentUser.SYS_DEPT.PARENT_CODE));
        }
        #endregion

        #region 根据所选部门信息联动人员信息
        [HttpPost]
        [AjaxRequest]
        [Skip]
        [Description("根据所选部门信息联动部分人员信息")]
        public ActionResult GetORGMemberEasyUITreeByDeptCode(GATE_BDATA_CARD cardModel)
        {
            return Json(Model_GATE_BDATA_CARD.GetORGMemberEasyUITreeByDeptCode(cardModel), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AjaxRequest]
        [Skip]
        [Description("根据所选部门信息联动所有人员信息")]
        public ActionResult GetORGMemberTreeByDeptCodeAll(string id)
        {
            return Content(Model_GATE_BDATA_CARD.GetORGMemberTreeByDeptCodeAll(id));
        }
        #endregion

        #region 根据所选部门信息联动所属门禁信息
        [HttpPost]
        [AjaxRequest]
        [Skip]
        [Description("根据所选部门信息联动部分人员信息")]
        public ActionResult GetORGGateEasyUITreeByDeptCode(GATE_BDATA_CARD cardModel)
        {
            string dept_code = "";
            if (cardModel.DEPT_CODE.Length > 4)
                dept_code = cardModel.DEPT_CODE.Substring(0, 4);
            else
                dept_code = cardModel.DEPT_CODE;
            return Json(Model_GATE_BDATA_GATE.GetORGGateTreeList(dept_code, dept_code.Substring(0, dept_code.Length - 2), "bj"), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 卡操作

        #region 废弃卡
        [HttpGet]
        [AjaxRequest]
        [Skip]
        [Description("废弃门卡")]
        public JsonResult Abandon(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    return PackagingAjaxmsg(Model_GATE_BDATA_CARD.Abandon_GATE_CARD(id));
                }
                return PackagingAjaxmsg(new Message().NewAmm);
            }
            catch (Exception)
            {
                return PackagingAjaxmsg(new Message().NewAmm);
            }
        }
        #endregion

        #region 解除废弃卡
        [HttpGet]
        [AjaxRequest]
        [Skip]
        [Description("解除废弃门卡")]
        public JsonResult unAbandon(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    return PackagingAjaxmsg(Model_GATE_BDATA_CARD.unAbandon(id));
                }
                return PackagingAjaxmsg(new Message().NewAmm);
            }
            catch (Exception)
            {
                return PackagingAjaxmsg(new Message().NewAmm);
            }
        }
        #endregion

        #region 解绑
        [HttpGet]
        [AjaxRequest]
        [Skip]
        [Description("解绑门卡")]
        public JsonResult unBind(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    return PackagingAjaxmsg(Model_GATE_BDATA_CARD.unBind_GATE_CARD(id));
                }
                return PackagingAjaxmsg(new Message().NewAmm);
            }
            catch (Exception)
            {
                return PackagingAjaxmsg(new Message().NewAmm);
            }
        }
        #endregion


        [HttpGet]
        [AjaxRequest]
        [Skip]
        [Description("获取门禁信息")]
        public ActionResult GetGateName(string id)
        {
            string result = string.Empty;
             
           
            List<GATE_BDATA_GATE> gatelist = oc.BllSession.IGATE_BDATA_GATEService.Entities.ToList();
            try
            {
                #region 获取门禁信息前，将已成功返回结果的解绑信息删除
                List<string> lastgateStrList = oc.BllSession.IGATE_GATEMNG_LASTCARDLISTService.Entities.Where(o => o.CARD_NO == id).Select(o => o.LIST_ID).ToList();
                IQueryable<GATE_GATEMNG_CARDLIST> cardlistsEntites =
                    oc.BllSession.IGATE_GATEMNG_CARDLISTService.Entities.Where(o => lastgateStrList.Contains(o.LIST_ID));
                   
                using (TransactionScope ts = new TransactionScope())
                {
                    List<string> delcardlists =
                        cardlistsEntites.Where(o => o.OP_RESULT.Equals("1") && o.OP_TYPE == 2 || o.UNBIND.Equals("1"))
                            .Select(o => o.LIST_ID)
                            .ToList();
                    oc.BllSession.IGATE_GATEMNG_LASTCARDLISTService.DelByWhere(o => delcardlists.Contains(o.LIST_ID));
                    ts.Complete();
                }
                
                #endregion
                
                List<string> lastgateStrList1 = oc.BllSession.IGATE_GATEMNG_LASTCARDLISTService.Entities.Where(o => o.CARD_NO == id).Select(o => o.LIST_ID).ToList();
                List<GATE_GATEMNG_CARDLIST> cardlists = oc.BllSession.IGATE_GATEMNG_CARDLISTService.Entities.Where(o => lastgateStrList1.Contains(o.LIST_ID)).ToList();
                var gatelistdetail =
                    (from last in cardlists
                        join gate in gatelist on last.GATE_NO equals gate.GATE_NO
                        select new
                        {
                            PKID = last.LIST_ID,
                            GATE_NAME = gate.GATE_NAME,
                            LINE_NAME = gate.LINE_NAME,
                            LINE_DIR = gate.LINE_DIR,
                            MILEAGE_COORDINATE = Convert.ToDecimal(gate.MILEAGE_COORDINATE).ToString("#0.000"),
                            optime = last.OP_TIME,
                            optype = last.OP_TYPE,
                            opresu = last.OP_RESULT,
                            listdate = last.LIST_DATE,
                            cardno = last.CARD_NO,
                            mileageorder = gate.MILEAGE_COORDINATE
                        }).OrderBy(o => o.mileageorder).ToList();

                if (gatelistdetail.Count > 0)
                {
                    AjaxMsgModel amm = new Message().NewAmm;
                    amm.Statu = AjaxStatu.ok;
                    amm.Data = Json(gatelistdetail, JsonRequestBehavior.AllowGet);
                    return PackagingAjaxmsg(amm);
                }
                else
                {
                    AjaxMsgModel amm = new Message().NewAmm;
                    amm.Msg = "该门禁卡未绑定门禁信息";
                    return PackagingAjaxmsg(amm);
                }
            }
            catch (Exception)
            {
                AjaxMsgModel amm = new Message().NewAmm;
                amm.Msg = "查询失败";
                return PackagingAjaxmsg(amm);
            }

        }

        #region 人为解除绑定
        [HttpGet]
        [AjaxRequest]
        [Skip]
        [Description("获取门禁信息")]
        public ActionResult SingelUnBind(string id)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            string list_id = id.Split('!')[0];
            string card_no = id.Split('!')[1];
            amm = Model_GATE_BDATA_CARD.SingelUnBind(list_id);
            if (amm.Statu == AjaxStatu.ok)
            {
                string result = string.Empty;
                List<GATE_GATEMNG_LASTCARDLIST> lastgatelist =
                    oc.BllSession.IGATE_GATEMNG_LASTCARDLISTService.Entities.Where(o => o.CARD_NO == card_no).ToList();
                List<string> lastgateStrList = lastgatelist.Select(o => o.LIST_ID).ToList();
                List<GATE_GATEMNG_CARDLIST> cardlists = oc.BllSession.IGATE_GATEMNG_CARDLISTService.Entities.Where(o => lastgateStrList.Contains(o.LIST_ID)).ToList();
                List<GATE_BDATA_GATE> gatelist = oc.BllSession.IGATE_BDATA_GATEService.Entities.ToList();
                var gatelistdetail =
                    (from last in cardlists
                     join gate in gatelist on last.GATE_NO equals gate.GATE_NO
                     select new
                     {
                         PKID = last.LIST_ID,
                         GATE_NAME = gate.GATE_NAME,
                         LINE_NAME = gate.LINE_NAME,
                         LINE_DIR = gate.LINE_DIR,
                         MILEAGE_COORDINATE = Convert.ToDecimal(gate.MILEAGE_COORDINATE).ToString("#0.000"),
                         optime = last.OP_TIME,
                         optype = last.OP_TYPE,
                         opresu = last.OP_RESULT,
                         listdate = last.LIST_DATE,
                         cardno = last.CARD_NO,
                         mileageorder = gate.MILEAGE_COORDINATE
                     }).OrderBy(o => o.mileageorder).ToList();
                amm.Data = Json(gatelistdetail, JsonRequestBehavior.AllowGet);
            }
            

            return PackagingAjaxmsg(amm);
        }
        #endregion
        #region 重新发送白名单
        [HttpGet]
        [AjaxRequest]
        [Skip]
        [Description("获取门禁信息")]
        public ActionResult SingelBind(string id)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            string list_id = id.Split('!')[0];
            string card_no = id.Split('!')[1];
            amm = Model_GATE_BDATA_CARD.SingelBind(list_id);
            if (amm.Statu == AjaxStatu.ok)
            {
                string result = string.Empty;
                List<GATE_GATEMNG_LASTCARDLIST> lastgatelist =
                    oc.BllSession.IGATE_GATEMNG_LASTCARDLISTService.Entities.Where(o => o.CARD_NO == card_no).ToList();
                List<string> lastgateStrList = lastgatelist.Select(o => o.LIST_ID).ToList();
                List<GATE_GATEMNG_CARDLIST> cardlists = oc.BllSession.IGATE_GATEMNG_CARDLISTService.Entities.Where(o => lastgateStrList.Contains(o.LIST_ID)).ToList();
                List<GATE_BDATA_GATE> gatelist = oc.BllSession.IGATE_BDATA_GATEService.Entities.ToList();
                var gatelistdetail =
                    (from last in cardlists
                     join gate in gatelist on last.GATE_NO equals gate.GATE_NO
                     select new
                     {
                         PKID = last.LIST_ID,
                         GATE_NAME = gate.GATE_NAME,
                         LINE_NAME = gate.LINE_NAME,
                         LINE_DIR = gate.LINE_DIR,
                         MILEAGE_COORDINATE = Convert.ToDecimal(gate.MILEAGE_COORDINATE).ToString("#0.000"),
                         optime = last.OP_TIME,
                         optype = last.OP_TYPE,
                         opresu = last.OP_RESULT,
                         listdate = last.LIST_DATE,
                         cardno = last.CARD_NO,
                         mileageorder = gate.MILEAGE_COORDINATE
                     }).OrderBy(o => o.mileageorder).ToList();
                amm.Data = Json(gatelistdetail, JsonRequestBehavior.AllowGet);
            }
            return PackagingAjaxmsg(amm);
        }
        #endregion
        #endregion
    }

    public class ACCESS_GATE_BDATA_CARDApiController : BaseApiController
    {
        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_GATE_BDATA_CARD data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            //查询条件
            IQueryable<GATE_BDATA_CARD> listCard = oc.BllSession.IGATE_BDATA_CARDService.Entities.Where(c => 
                c.DEPT_CODE.StartsWith(oc.CurrentUser.SYS_DEPT.DEPT_CODE) &&
                c.CARD_TYPE != "1"
                );
            //绑定了门禁卡的卡信息
            List<string> cardGatenoList = oc.BllSession.IGATE_GATEMNG_LASTCARDLISTService.Entities.Select(o=>o.CARD_NO).Distinct().ToList();
            //部门信息
            List<SYS_DEPT> deptList = oc.BllSession.ISYS_DEPTService.Entities.ToList();
            
            
            
            //持卡人选择到部门一级，则查询部门
            //持卡人
            if (!string.IsNullOrEmpty(data.MEMBER_ID))
            {
                if (data.MEMBER_ID.IndexOf("dept_code")>=0)
                {
                    listCard = listCard.Where(u => u.DEPT_CODE.Contains(data.MEMBER_ID.Replace("dept_code", "")));
                }
                else
                {
                    listCard = listCard.Where(u => u.MEMBER_ID.Equals(data.MEMBER_ID));
                }
            }
            
            //卡号
            if (!string.IsNullOrEmpty(data.CARD_NO))
            { listCard = listCard.Where(u => u.CARD_NO.Contains(data.CARD_NO)); }
            //卡状态
            if (!string.IsNullOrEmpty(data.CARD_STATE))
            { listCard = listCard.Where(u => u.CARD_STATE.Equals(data.CARD_STATE)); }

            int total = 0;

            total = listCard.Count();
            List<Gate_Card> listGATA_CARD = listCard.OrderByDescending(u => u.CARD_INDATE)
                .Skip(pageSize * (pageIndex - 1)).Take(pageSize).Select(card => new Gate_Card
                {
                    CARD_NO = card.CARD_NO,
                    deptcode = card.DEPT_CODE,
                    people = string.IsNullOrEmpty(card.MEMBER_ID) ? "" : card.SYS_MEMBER.NAME,
                    tool = string.IsNullOrEmpty(card.TOOL_ID) ? "" : card.GATE_BDATA_TOOL.NAME,
                    phoneNo = string.IsNullOrEmpty(card.MEMBER_ID) ? "" : card.SYS_MEMBER.MOBILE,
                    insertDate = card.CARD_INDATE,
                    memo = card.CARD_NOTE,
                    state = card.CARD_STATE,
                    delDate = card.CARD_DELDATE,
                    cardtype = card.CARD_TYPE,
                    cardsection = card.CARD_SECTION,
                    DOOR_CARD_NO = card.DOOR_CARD_NO,
                    havegate = cardGatenoList.Contains(card.DOOR_CARD_NO)
                }).ToList();
            //List<GATE_BDATA_CARD> listCARD = listCard.OrderByDescending(u => u.CARD_INDATE)
            //    .Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();

            foreach (Gate_Card gate_card in listGATA_CARD)
            {
                string deptname = Model_SYS_DEPT.GetDeptInfo(gate_card.deptcode, deptList);
                gate_card.deptname1 = deptname.Split(',')[0];
                gate_card.deptname2 = deptname.Split(',')[1];
                gate_card.deptname3 = deptname.Split(',')[2];
            }
         
            //return ObjToJson.GetToJson(listMenu, total, true);
            return ObjToJson.ViewModelToJson(listGATA_CARD, total);
        }
        #endregion

        #region 门禁卡辅助类
        public class Gate_Card
        {
             public string CARD_NO { set; get; }
             public string DOOR_CARD_NO { set; get; }
             public string deptcode { set; get; }
             public string deptname1 { set; get; }
             public string deptname2 { set; get; }
             public string deptname3 { set; get; }
             public string people { set; get; }
             public string tool { set; get; }
             public string phoneNo { set; get; }
             public DateTime? insertDate { set; get; }
             public string memo { set; get; }
             public string state { set; get; }
             public DateTime? delDate { set; get; }
             public string cardtype { set; get; }
             public string cardsection { set; get; }
             public bool havegate { set; get; }
        }
        #endregion

        #region 保存
        [HttpPost]
        [AjaxRequest]
        [ValidateInput(false)]
        public AjaxMsgModel Save(VIEW_GATE_BDATA_CARD data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            amm.Msg = "非法操作";
            amm.Statu = AjaxStatu.err;
            try
            {
                Mapper.CreateMap<VIEW_GATE_BDATA_CARD, GATE_BDATA_CARD>();
                GATE_BDATA_CARD u = Mapper.Map<VIEW_GATE_BDATA_CARD, GATE_BDATA_CARD>(data);

                string gatenolist = string.Empty;
                string oldgatenolist = string.Empty;

                gatenolist = string.IsNullOrEmpty(data.GATENO) ? "" : data.GATENO;
                oldgatenolist = string.IsNullOrEmpty(data.OLDGATENO) ? "" : data.OLDGATENO;
                u.TOOL_ID = string.Empty;
                u.CARD_STATE = string.IsNullOrEmpty(u.MEMBER_ID)
                    ? Constant.Gate_CardState.UnDelUnBindValue
                    : Constant.Gate_CardState.UnDelIsBindValue;
                u.CARD_TYPE = "0";
                u.CARD_SECTION = null;
                if (string.IsNullOrEmpty(data.PKID))
                {
                    u.CARD_INDATE = DateTime.Now;
                    return Model_GATE_BDATA_CARD.Add_GATE_CARD(u, gatenolist);
                }
                else
                {
                    if (data.PKID == data.CARD_NO)
                    {
                        return Model_GATE_BDATA_CARD.Edit_GATE_CARD(u, gatenolist, oldgatenolist);
                    }
                }
                return amm;
            }
            catch (Exception ex)
            {
                RecordLog.RecordError(ex.ToString());
                return new Message().NewAmm;
            }
        }
        #endregion

        #region 修改
        [HttpPost]
        public AjaxMsgModel Edit(VIEW_GATE_BDATA_CARD data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            try
            {
                GATE_BDATA_CARD gatacard = oc.BllSession.IGATE_BDATA_CARDService.Entities.Where(m => m.CARD_NO == data.CARD_NO).FirstOrDefault();
                string gateNolist = string.Empty;
                //处理门禁卡授权
                if (gatacard.CARD_TYPE == "0")
                {
                    List<GATE_GATEMNG_LASTCARDLIST> listLastcardlists = new List<GATE_GATEMNG_LASTCARDLIST>();
                    listLastcardlists =
                        oc.BllSession.IGATE_GATEMNG_LASTCARDLISTService.Entities.Where(m => m.CARD_NO == data.DOOR_CARD_NO).ToList();
                    foreach (var cardinfo in listLastcardlists)
                    {
                        gateNolist += cardinfo.GATE_NO + ",";
                    }
                    gateNolist = string.IsNullOrEmpty(gateNolist) ? "" : gateNolist.Substring(0, gateNolist.Length - 1);
                }
                if (gatacard != null)
                {
                    Mapper.CreateMap<GATE_BDATA_CARD, VIEW_GATE_BDATA_CARD>();
                    VIEW_GATE_BDATA_CARD vm = Mapper.Map<GATE_BDATA_CARD, VIEW_GATE_BDATA_CARD>(gatacard);
                    vm.GATENO = gateNolist;
                    vm.OLDGATENO = gateNolist;
                    amm.Statu = AjaxStatu.ok;
                    amm.Data = vm;
                    return amm;
                }
                else
                {
                    amm.Msg = string.Format(Message.NotFound, "卡");
                    return amm;
                }
            }
            catch (Exception)
            {
                return amm;
            }
        }
        #endregion

        #region 删除
        [HttpPost]
        public AjaxMsgModel Del(VIEW_GATE_BDATA_CARD data)
        {
            return Model_GATE_BDATA_CARD.Del_GATE_CARD(data.CARD_NO);
        }
        #endregion

        
    }
}
