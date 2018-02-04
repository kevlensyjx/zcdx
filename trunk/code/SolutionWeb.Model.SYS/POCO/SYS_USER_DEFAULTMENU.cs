  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_USER_DEFAULTMENU{
		public SYS_USER_DEFAULTMENU ToPOCO(){
		return new SYS_USER_DEFAULTMENU(){
				USER_DEFAULT_ID = USER_DEFAULT_ID,
				USER_NAME = USER_NAME,
				MENU_ID = MENU_ID,
			};
		}
	}
}  
