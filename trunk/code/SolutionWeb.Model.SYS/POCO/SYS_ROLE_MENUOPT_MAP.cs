  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_ROLE_MENUOPT_MAP{
		public SYS_ROLE_MENUOPT_MAP ToPOCO(){
		return new SYS_ROLE_MENUOPT_MAP(){
				ROLE_MENUOPT_ID = ROLE_MENUOPT_ID,
				ROLE_ID = ROLE_ID,
				MENUOPT_ID = MENUOPT_ID,
			};
		}
	}
}  
