  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_MENUOPT{
		public SYS_MENUOPT ToPOCO(){
		return new SYS_MENUOPT(){
				MENUOPT_ID = MENUOPT_ID,
				MENUOPT_NAME = MENUOPT_NAME,
				MENU_ID = MENU_ID,
				C_ICO = C_ICO,
				AREA = AREA,
				CONTROLLER = CONTROLLER,
				ACTION = ACTION,
				NOTE = NOTE,
			};
		}
	}
}  
