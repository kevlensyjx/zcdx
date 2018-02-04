using SolutionWeb.Common;
using SolutionWeb.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolutionWeb.Areas.POLICY.Controllers
{
    public class POLICY_APPROVE_DASHBOARDController : BaseController
    {
        // GET: POLICY/POLICY_APPROVE_DASHBOARD
        [Description("待办事项")]
        [HttpGet]
        [Skip]
        public ActionResult Index()
        {
            var viewModel = new
            {
                Permission = new//权限
                {
                    a_list = true,
                },
                resx = new
                {
                    listTitle = ""
                },
                urls = new//请求URL
                {

                }

            };
            return View(viewModel);
        }

        [Skip]
        public ActionResult GetPolicyStatisticsInfo()
        {
            JsonResult jsonresult = Json(Model_POLICY_MAIN_INFO.Create.GetPolicyStatisticsInfo(), JsonRequestBehavior.AllowGet);
            return jsonresult;
        }


    }
}