  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.POLICY
{
	public partial class BASE_PROJECT_INFO{
		public BASE_PROJECT_INFO ToPOCO(){
		return new BASE_PROJECT_INFO(){
				SID = SID,
				ITEM_TYPE = ITEM_TYPE,
				ITEM_NAME = ITEM_NAME,
				CASHING_WAY = CASHING_WAY,
				LAY_ORDER = LAY_ORDER,
				ITEM_CODE = ITEM_CODE,
			};
		}
	}
}  
