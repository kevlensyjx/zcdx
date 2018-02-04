using System.Web;
using System.Web.Optimization;

namespace SolutionWeb
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/Content/viewModel").Include(
                      "~/Content/ViewModel/json2.js",
                      "~/Content/ViewModel/knockout-3.1.0.js",
                      "~/Content/ViewModel/knockout.mapping-2.4.1.js",
                      "~/Content/ViewModel/knockout-easyui.js",
                      "~/Content/ViewModel/all_viewmodel.js"));

            bundles.Add(new ScriptBundle("~/Content/js").Include(
                      "~/Content/JS/utils.js",
                      "~/Content/JS/ValidateJs.js",
                      "~/Content/JS/processingAjaxMsg.js",
                      "~/Content/JS/lightbox.js", 
                      "~/Content/JS/Constant.js",
                       "~/Content/JS/jquery.rotate.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/Css/base.css",
                      "~/Content/Css/icon/icon.css",
                      "~/Content/Css/page/index.css",
                      "~/Content/Css/fancybox/fancybox.css",
                      "~/Content/Css/lightbox.css"));

            bundles.Add(new ScriptBundle("~/jquery-easyui/js").Include(
                      // "~/Scripts/jquery-1.10.2.min.js",
                      //"~/jquery-easyui-1.5.2/jquery.min.js",
                      //"~/jquery-easyui-1.5.2/jquery.easyui.min.js",
                      //"~/jquery-easyui-1.5.2/locale/easyui-lang-zh_CN.js"
                      "~/jquery-easyui-1.4/jquery.min.js",
                      "~/jquery-easyui-1.4/jquery.easyui.min.js",
                      "~/jquery-easyui-1.4/locale/easyui-lang-zh_CN.js",
                      "~/jquery-easyui-1.4/jquery.easyui.patch.js",
                      "~/jquery-easyui-1.4/datagrid-detailview.js",
                      "~/jquery-easyui-1.4/datagrid-groupview.js"
                      ));

            bundles.Add(new StyleBundle("~/jquery-easyui/css").Include(
                      //"~/jquery-easyui-1.5.2/themes/default/easyui.css",
                      //"~/jquery-easyui-1.5.2/themes/icon.css"
                      "~/jquery-easyui-1.4/themes/default/easyui.css",
                      "~/jquery-easyui-1.4/themes/icon.css"
                      ));

        }
    }
}
