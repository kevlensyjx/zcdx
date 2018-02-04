  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_DEPT{
		public SYS_DEPT ToPOCO(){
		return new SYS_DEPT(){
				DEPT_CODE = DEPT_CODE,
				DEPT_NAME = DEPT_NAME,
				PARENT_CODE = PARENT_CODE,
				C_ICO = C_ICO,
				STATUS_FLAG = STATUS_FLAG,
				DEL_FLAG = DEL_FLAG,
				NOTE = NOTE,
				DEPT_ORDER = DEPT_ORDER,
				DEPT_FLAG = DEPT_FLAG,
				ROLE_ID = ROLE_ID,
			};
		}
	}
}  
