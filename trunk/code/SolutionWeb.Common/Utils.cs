using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionWeb.Common
{
    public class Utils
    {
        #region 关键字解析错误信息转换
        public static string ModelConvertString(ConvertErrorModel errModel)
        {
            if (errModel != null)
            {
                return "{titleName:" + ReStr(errModel.titleName) + ",errorCause:" + ReStr(errModel.errorCause) + ",content:" + ReStr(errModel.content) + "}";
            }
            return "";
        }
        public static string ReStr(string str)
        {
            return str.Replace(":", "▲").Replace(",", "◆").Replace("}", "●").Replace("{", "★").Replace("\r", "").Replace("\n", "");
        }
        #endregion
    }
}
