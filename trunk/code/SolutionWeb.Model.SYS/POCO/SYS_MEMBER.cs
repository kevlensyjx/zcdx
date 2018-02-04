  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_MEMBER{
		public SYS_MEMBER ToPOCO(){
		return new SYS_MEMBER(){
				MEMBER_ID = MEMBER_ID,
				DEPT_CODE = DEPT_CODE,
				NAME = NAME,
				PHONE = PHONE,
				JOB = JOB,
				NOTE = NOTE,
				UPDATE_DATE = UPDATE_DATE,
				UPDATE_USER = UPDATE_USER,
				DEL_FLAG = DEL_FLAG,
			};
		}
	}
}  
