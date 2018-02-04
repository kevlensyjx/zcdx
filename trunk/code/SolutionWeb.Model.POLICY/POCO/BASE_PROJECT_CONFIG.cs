  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.POLICY
{
	public partial class BASE_PROJECT_CONFIG{
		public BASE_PROJECT_CONFIG ToPOCO(){
		return new BASE_PROJECT_CONFIG(){
				SID = SID,
				PROJECT_SID = PROJECT_SID,
				DEPT_TYPE = DEPT_TYPE,
				DEPT_CODE = DEPT_CODE,
				DEPT_NAME = DEPT_NAME,
				IS_VOTE = IS_VOTE,
			};
		}
	}
}  
