using SolutionWeb.Common;
using System.Collections.Generic;

namespace SolutionWeb.Interface
{

    public interface IUserLogin
    {
        AjaxMsgModel LoginIn(string strLoginName, string strLoginPwd, string strYzm);
        bool IsLogin();
        bool HasPermission(string areaName, string controllerName, string actionName, HttpMethod httpmethod);
        string IsWarn();
        AjaxMsgModel EditPass(string username, string oldpass, string newpass);
        int TestAdd();
        List<EasyUITreeNode> GetMyDEPTandRYTree(string DEPT_CODE, string PARENT_CODE);

        string GetMyORGandRYTree_Two(string DEPT_CODE, string PARENT_CODE, bool isCheckAll);

        List<EasyUITreeNode> GetMyDEPTTree(string DEPT_CODE, string PARENT_CODE);
        /// <summary>
        /// 获取地图左侧菜单
        /// </summary>
        /// <returns></returns>
        string GetMyGISMenu();
        /// <summary>
        /// 获取用户对应部门组织树
        /// </summary>
        /// <returns></returns>
        string GetDeptMenu();
        string GetOrganizeMenu(bool isCheckAll);

        List<EasyUITreeNode> GetMyOnlyTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0);
        List<EasyUITreeNode> GetMyORGTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0);
        List<EasyUITreeNode> GetMyORGNoGQTree(string DEPT_CODE, string PARENT_CODE, int DEPT_FLAG = 0);
        
    }
}
