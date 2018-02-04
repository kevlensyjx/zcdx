  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_ROLE_MENU_MAP{
		public SYS_ROLE_MENU_MAP ToPOCO(){
		return new SYS_ROLE_MENU_MAP(){
				ROLE_MENU_ID = ROLE_MENU_ID,
				ROLE_ID = ROLE_ID,
				MENU_ID = MENU_ID,
			};
		}
	}
}  
