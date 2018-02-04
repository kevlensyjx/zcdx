using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionWeb.ViewModels
{
    public class UserRoleModel
    {
        public string deptcode { set; get; }
        public string deptname { set; get; }
        public string USER_NAME { set; get; }
        public string ZSNAME { set; get; }
        public string rolename { set; get; }
        public string depticon { set; get; }
        public string upuser { set; get; }
        public virtual ICollection<VIEW_SYS_USER_ROLE_MAP> SYS_USER_ROLE_MAP { get; set; }
        public Nullable<System.DateTime> uptime { get; set; }
    }
}
