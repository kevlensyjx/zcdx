  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.POLICY
{
	public partial class POLICY_NOTICE_INFO{
		public POLICY_NOTICE_INFO ToPOCO(){
		return new POLICY_NOTICE_INFO(){
				SID = SID,
				CREATE_DT = CREATE_DT,
				CREATE_BY = CREATE_BY,
				UPDATE_DT = UPDATE_DT,
				UPDATE_BY = UPDATE_BY,
				NOTICE_TITLE = NOTICE_TITLE,
				NOTICE_CONTENT = NOTICE_CONTENT,
				IS_SHOW = IS_SHOW,
				NOTICE_TYPE = NOTICE_TYPE,
			};
		}
	}
}  
