  
using System;
using System.Collections.Generic;
namespace SolutionWeb.ViewModels
{
	public partial class VIEW_SYS_MENU{
		public string page { get; set; }
		public string rows { get; set; }
		public string sort { get; set; }
		public string order { get; set; }
		public string editcellname { get; set; }
		public string MENU_ID{ get; set; }
		public string MENU_NAME{ get; set; }
		public string C_ICO{ get; set; }
		public string AREA{ get; set; }
		public string CONTROLLER{ get; set; }
		public string ACTION{ get; set; }
		public string PARENT_ID{ get; set; }
		public decimal MENU_LEVEL{ get; set; }
		public Nullable<decimal> MENU_ORDER{ get; set; }
		public string GIS_ORDER{ get; set; }
		public string ISSHOW_FLAG{ get; set; }
		public string DEFMENU_ID{ get; set; }
	}
}  
