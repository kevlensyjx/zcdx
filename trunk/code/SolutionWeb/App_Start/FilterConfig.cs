using System.Web;
using System.Web.Mvc;
using SolutionWeb.Common;

namespace SolutionWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new LoginValidateAttribute());
            filters.Add(new LogExceptionFilter());//升级成自定义的异常扩展
        }
    }
}
