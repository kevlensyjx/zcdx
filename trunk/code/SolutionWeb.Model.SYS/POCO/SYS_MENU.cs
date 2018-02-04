  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_MENU{
		public SYS_MENU ToPOCO(){
		return new SYS_MENU(){
				MENU_ID = MENU_ID,
				MENU_NAME = MENU_NAME,
				C_ICO = C_ICO,
				AREA = AREA,
				CONTROLLER = CONTROLLER,
				ACTION = ACTION,
				PARENT_ID = PARENT_ID,
				MENU_LEVEL = MENU_LEVEL,
				MENU_ORDER = MENU_ORDER,
				GIS_ORDER = GIS_ORDER,
				ISSHOW_FLAG = ISSHOW_FLAG,
				DEFMENU_ID = DEFMENU_ID,
			};
		}
	}
}  
