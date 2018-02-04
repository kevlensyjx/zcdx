using AutoMapper;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using SolutionWeb.Model.SYS;
using SolutionWeb.Models;
using SolutionWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.SYS.Controllers
{
    public class SYS_MEMBERController : BaseController
    {
        // GET: SYS/SYS_MEMBER

        #region Ioc
        private IBaseService bs = null;
        private IUserLogin ul = null;
        public SYS_MEMBERController(IBaseService baseService, IUserLogin userLogin)
        {
            this.bs = baseService;
            this.ul = userLogin;
        }
        #endregion
        #region 查看人员
        [Description("查看人员")]
        [HttpGet]
        public ActionResult Index()
        {
            string ControllerUrl = "/api/SYS/SYS_MEMBER/";
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_MEMBER", "List", HttpMethod.Post),
                    a_add = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_MEMBER", "Add", HttpMethod.Post),
                    a_edit = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_MEMBER", "Edit", HttpMethod.Get),
                    a_del = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_MEMBER", "Del", HttpMethod.Get),
                    //a_excelin = Model_SYS_MENU.HasPermission("SYS", "SYS_MEMBER", "List", HttpMethod.Post),
                    //a_excelout = Model_SYS_MENU.Create.HasPermission("SYS", "SYS_MEMBER", "List", HttpMethod.Post),
                },
                resx = new
                {
                    listTitle = "您没有【查看人员】权限",
                    addTitle = "您没有【新增人员】权限",
                    editTitle = "您没有【编辑人员】权限！",
                    deleteTitle = "您没有【删除人员】权限！",
                },
                urls = new//请求URL
                {
                    save = ControllerUrl + "Save",
                    list = ControllerUrl + "List",
                    edit = ControllerUrl + "Edit",
                    del = ControllerUrl + "Del",
                    writexls = ControllerUrl + "WirteExcel",
                    //readxls = ControllerUrl + "ReadXls",
                    dataGgridName = "data_grid",//列表ID
                    dataGgridType = "datagrid",//列表类型
                    dataAddName = "data_add",//增加窗口
                    dataFormName = "DataForm",//提交表单
                },
                searchForm = new VIEW_SYS_MEMBER()//查询
                {
                },
                addForm = new VIEW_SYS_MEMBER()
                { //添加修改
                },
                extForm = new//扩展类
                {
                    extA = Model_SYS_DEPT.Create.GetMyDEPTTree(oc.CurrentUser.DEPT_CODE, oc.CurrentUser.PARENT_CODE),//部门列表
                    extE = new List<EasyUIComBoBoxNode>()//手机状态
                }
            };
            return View(viewModel);
        }
        #endregion
        
    }


    public class SYS_MEMBERApiController : BaseApiController
    {
        #region Ioc
        private IBaseService bs = null;
        public SYS_MEMBERApiController(IBaseService baseService)
        {
            this.bs = baseService;
        }
        #endregion
        #region 查询
        [HttpPost]//查询
        public ViewModel List(VIEW_SYS_MEMBER data)
        {
            int pageSize = int.Parse(data.rows);
            int pageIndex = int.Parse(data.page);
            string sort = data.sort;
            string order = data.order;

            //查询条件
            IQueryable<SYS_MEMBER> SYS_MEMBEREntity = bs.Entities<SYS_MEMBER>();
            if (!string.IsNullOrEmpty(data.PHONE))
            {
                SYS_MEMBEREntity = SYS_MEMBEREntity.Where(u => u.PHONE.Contains(data.PHONE) && u.DEL_FLAG == "0");
            }
            else
            {
                if (data.DEPT_CODE != null && data.DEPT_CODE != "")
                {
                    SYS_MEMBEREntity = SYS_MEMBEREntity.Where(u => u.DEPT_CODE.StartsWith(data.DEPT_CODE) && u.DEL_FLAG == "0");
                }
                else
                {
                    SYS_MEMBEREntity = SYS_MEMBEREntity.Where(u => u.DEPT_CODE.StartsWith(oc.CurrentUser.DEPT_CODE) && u.DEL_FLAG == "0");
                }
            }
            int total = 0;

            total = SYS_MEMBEREntity.Count();
            var listMEMBER = SYS_MEMBEREntity.OrderBy(u => u.DEPT_CODE)
                                                       .Skip(pageSize * (pageIndex - 1)).Take(pageSize).Select(mb =>
                                                        new
                                                        {
                                                            MEMBER_ID = mb.MEMBER_ID,
                                                            deptname = mb.SYS_DEPT.DEPT_NAME,
                                                            depticon = mb.SYS_DEPT.C_ICO,
                                                            deptcode = mb.SYS_DEPT.DEPT_CODE,
                                                            name = mb.NAME,
                                                            update = mb.UPDATE_DATE,
                                                            upuser = mb.UPDATE_USER,
                                                            job = mb.JOB,
                                                            phone = mb.PHONE
                                                        }
                                                        ).ToList();
            
            return ObjToJson.ViewModelToJson(listMEMBER, total);
        }
        #endregion
        
        #region 修改
        [HttpPost]
        public AjaxMsgModel Edit(VIEW_SYS_MEMBER data)
        {
            AjaxMsgModel amm = new Message().NewAmm;
            //try
            //{
                SYS_MEMBER member = bs.Entities<SYS_MEMBER>().Where(m => m.DEL_FLAG == "0" && m.MEMBER_ID == data.MEMBER_ID).OrderBy(m => m.UPDATE_DATE).FirstOrDefault();

                if (member != null)
                {
                    Mapper.CreateMap<SYS_MEMBER, VIEW_SYS_MEMBER>();
                    VIEW_SYS_MEMBER vm = Mapper.Map<SYS_MEMBER, VIEW_SYS_MEMBER>(member);
                    amm.Statu = AjaxStatu.ok;
                    amm.Data = vm;
                    return amm;
                }
                else
                {
                    amm.Msg = string.Format(Message.NotFound, "人员");
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
        public AjaxMsgModel Save(VIEW_SYS_MEMBER data)
        {
            //try
            //{
                Mapper.CreateMap<VIEW_SYS_MEMBER, SYS_MEMBER>();
                SYS_MEMBER u = Mapper.Map<VIEW_SYS_MEMBER, SYS_MEMBER>(data);

                u.UPDATE_DATE = DateTime.Now;
                u.UPDATE_USER = oc.CurrentUser.USER_NAME;
                u.DEL_FLAG = "0";
                if (u.MEMBER_ID == null || u.MEMBER_ID == "")
                {
                    u.MEMBER_ID = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    return Model_SYS_MEMBER.Create.Add(u);
                }
                else
                {
                    return Model_SYS_MEMBER.Create.Edit(u);
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
        public AjaxMsgModel Del(VIEW_SYS_MEMBER data)
        {
            return Model_SYS_MEMBER.Create.Del(data.MEMBER_ID);
        }
        #endregion
        
        
    }
}