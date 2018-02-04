using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolutionWeb.Common
{
    public class SESS_MENU
    {

        public string MENU_ID { get; set; }
        
        public string MENU_NAME { get; set; }
        
        public string C_ICO { get; set; }
        
        public string AREA { get; set; }
        
        public string CONTROLLER { get; set; }
        
        public string ACTION { get; set; }
        
        public string PARENT_ID { get; set; }

        public decimal MENU_LEVEL { get; set; }

        public decimal? MENU_ORDER { get; set; }
        
        public string GIS_ORDER { get; set; }
        
        public string ISSHOW_FLAG { get; set; }
        
        public string DEFMENU_ID { get; set; }
    }
}
