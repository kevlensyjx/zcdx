  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_USER{
		public SYS_USER ToPOCO(){
		return new SYS_USER(){
				USER_NAME = USER_NAME,
				PASSWORD = PASSWORD,
				DEPT_CODE = DEPT_CODE,
				UPDATE_DATE = UPDATE_DATE,
				UPDATE_USER = UPDATE_USER,
				NOTE = NOTE,
				MANAGE_DEPT_CODE = MANAGE_DEPT_CODE,
				ZSNAME = ZSNAME,
			};
		}
	}
}  
