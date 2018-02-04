using AutoMapper;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Models;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.SYS.Controllers
{
    public class SYS_DEPTController : BaseController
    {
        // GET: SYS/SYS_DEPT

        #region Identity
        private IBaseService bs = null;
        public SYS_DEPTController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion
        #region 查看部门
        [Description("查看部门")]
        [HttpGet]
        public ActionResult Index()
        {
            string ControllerUrl = "/api/SYS/SYS_DEPT/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_DEPT", "List", Common.HttpMethod.Post),
                    a_add = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_DEPT", "Add", Common.HttpMethod.Post),
                    a_edit = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_DEPT", "Edit", Common.HttpMethod.Get),
                    a_del = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_DEPT", "Del", Common.HttpMethod.Get),
                    a_excelin = oc.CurrentUser.USER_NAME == "sxsh" ? true : false
                    //a_excelout = Model_SYS_MENU.HasPermission("SYS", "SYS_DEPT", "List", Common.HttpMethod.Post)
                },
                resx = new
                {
                    listTitle = "您没有【查看部门】权限",
                    addTitle = "您没有【新增部门】权限",
                    editTitle = "您没有【编辑部门】权限！",
                    deleteTitle = "您没有【删除部门】权限！"
                },
                urls = new//请求URL
                {
                    save = ControllerUrl + "Save",
                    list = ControllerUrl + "List",
                    edit = ControllerUrl + "Edit",
                    del = ControllerUrl + "Del",
                    //writexls = ControllerUrl + "WirteExcel",
                    readxls = ControllerUrl + "ReadXls",
                    dataGgridName = "data_grid",//列表ID
                    dataGgridType = "treegrid",//列表类型
                    dataAddName = "data_add",//增加窗口
                    //titleName="sss",
                    dataFormName = "DataForm",//提交表单
                },
                searchForm = new VIEW_SYS_DEPT()//查询
                {
                },
                addForm = new VIEW_SYS_DEPT()
                { //添加修改
                },
                extForm = new//扩展类
                {
                    extA = Model_SYS_DEPT.Create.GetMyORGNoGQTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE),//部门列表
                    extB = new List<EasyUIComBoBoxNode>() { new EasyUIComBoBoxNode() { id = "0", text = "单位" }, new EasyUIComBoBoxNode() { id = "1", text = "部门" } }
                }
            };
            return View(viewModel);
        }
        #endregion

        
    }
    public class SYS_DEPTApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public SYS_DEPTApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion


        #region 查看我的组织机构除工区树
        [System.Web.Http.HttpPost]
        public List<EasyUITreeNode> GetMyORGNoGQTree(VIEW_SYS_DEPT data)
        {
            return Model_SYS_DEPT.Create.GetMyORGNoGQTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE);//部门列表
        }
        #endregion

        #region 查询
        [HttpPost]//查询
        public List<EasyUIDEPTTree> List(VIEW_SYS_DEPT data)
        {
            string dept_code = oc.CurrentUser.DEPT_CODE;
            string parent_code = oc.CurrentUser.PARENT_CODE;
            //查询条件
            if (data != null && data.PARENT_CODE != null && data.PARENT_CODE != "")
            {
                dept_code = data.PARENT_CODE;
                if (dept_code.Length > 2)
                {
                    parent_code = dept_code.Substring(0, dept_code.Length - 2);
                }
            }
            //if (dept_code == "01")
            //{
            //    dept_code = "0101";
            //    parent_code = "01";
            //}
            Expression<Func<SYS_DEPT, bool>> slamda = m => m.DEL_FLAG == "0" && (m.PARENT_CODE.StartsWith(dept_code) || m.DEPT_CODE == dept_code);
            string strJson = string.Empty;
            //获得组织
            List<SYS_DEPT> listOrgMenu = bs.Entities<SYS_DEPT>().Where(slamda).OrderBy(m => m.DEPT_ORDER).ThenBy(m => m.DEPT_CODE)
                                                .ToList();
            List<EasyUIDEPTTree> listTreeNodes = Model_SYS_DEPT.Create.ConvertTreeNodes(listOrgMenu, parent_code, dept_code);
            return listTreeNodes;
        }
        #endregion

        #region 修改
        [HttpPost]
        public AjaxMsgModel Edit(VIEW_SYS_DEPT data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                SYS_DEPT member = bs.Entities<SYS_DEPT>().Where(m => m.DEPT_CODE == data.DEPT_CODE).OrderBy(m => m.DEPT_CODE).FirstOrDefault();

                if (member != null)
                {
                    Mapper.CreateMap<SYS_DEPT, VIEW_SYS_DEPT>();
                    VIEW_SYS_DEPT vm = Mapper.Map<SYS_DEPT, VIEW_SYS_DEPT>(member);
                    amm.Statu = AjaxStatu.ok;
                    amm.Data = vm;
                    return amm;
                }
                else
                {
                    amm.Msg = string.Format(Message.NotFound, "部门");
                    return amm;
                }
            //}
            //catch (Exception)
            //{
            //    return amm;
            //}
        }
        #endregion

        #region 保存
        [HttpPost]
        [ValidateInput(false)]
        public AjaxMsgModel Save(VIEW_SYS_DEPT data)
        {
            //try
            //{
                Mapper.CreateMap<VIEW_SYS_DEPT, SYS_DEPT>();
                SYS_DEPT u = Mapper.Map<VIEW_SYS_DEPT, SYS_DEPT>(data);


                //最后一个DEPT_CODE
                var lastCode = bs.Entities<SYS_DEPT>().Where(m => m.PARENT_CODE == data.PARENT_CODE).OrderByDescending(m => m.DEPT_CODE).Select(m => new { m.DEPT_CODE }).FirstOrDefault();
                int lastCodeId = 1;
                if (lastCode != null)
                {
                    lastCodeId = int.Parse(lastCode.DEPT_CODE.Substring(data.PARENT_CODE.Length, 2)) + 1;
                }
                string codeNum = lastCodeId < 10 ? "0" + lastCodeId.ToString() : lastCodeId.ToString();

                u.DEL_FLAG = "0";
                u.STATUS_FLAG = "0";
                u.C_ICO = data.PARENT_CODE.Length == 8 ? "icon-org" : "icon-DepartMent";

                if (u.DEPT_CODE == null || u.DEPT_CODE == "")
                {
                    u.DEPT_CODE = data.PARENT_CODE + codeNum;
                    u.DEPT_ORDER = 1;
                    return Model_SYS_DEPT.Create.Add(u);
                }
                else
                {
                    string newDeptCode = data.PARENT_CODE + codeNum;
                    return Model_SYS_DEPT.Create.Edit(u, u.DEPT_CODE, newDeptCode);
                }
            //}
            //catch (Exception)
            //{
            //    return new Message().NewAmm;
            //}
        }
        #endregion

        #region 删除
        [HttpPost]
        public AjaxMsgModel Del(VIEW_SYS_DEPT data)
        {
            return Model_SYS_DEPT.Create.Del(data.DEPT_CODE);
        }
        #endregion
    }
}