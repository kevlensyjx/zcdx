using SolutionWeb.Common;
using System.Collections.Generic;

namespace SolutionWeb.Interface
{

    public interface ICompanyLogin
    {
        AjaxMsgModel CompanyLoginIn(string strLoginName, string strLoginPwd, string strYzm);
        
    }
}
