  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.POLICY
{
	public partial class BASE_STATUS_DIC{
		public BASE_STATUS_DIC ToPOCO(){
		return new BASE_STATUS_DIC(){
				SID = SID,
				S_CODE = S_CODE,
				S_NAME = S_NAME,
				NOTE = NOTE,
				S_TYPE = S_TYPE,
				S_SORT = S_SORT,
				S_SHOW = S_SHOW,
			};
		}
	}
}  
