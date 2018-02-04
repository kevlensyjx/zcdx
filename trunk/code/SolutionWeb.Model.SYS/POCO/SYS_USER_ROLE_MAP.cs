  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_USER_ROLE_MAP{
		public SYS_USER_ROLE_MAP ToPOCO(){
		return new SYS_USER_ROLE_MAP(){
				USER_ROLE_ID = USER_ROLE_ID,
				USER_NAME = USER_NAME,
				ROLE_ID = ROLE_ID,
			};
		}
	}
}  
