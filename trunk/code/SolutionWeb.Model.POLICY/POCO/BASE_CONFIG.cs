  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.POLICY
{
	public partial class BASE_CONFIG{
		public BASE_CONFIG ToPOCO(){
		return new BASE_CONFIG(){
				SID = SID,
				CONFIG_TYPE = CONFIG_TYPE,
				CONFIG_NAME = CONFIG_NAME,
				CONFIG_VALUE = CONFIG_VALUE,
				CONFIG_REMARK = CONFIG_REMARK,
			};
		}
	}
}  
