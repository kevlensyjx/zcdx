  
using System;
using System.Collections.Generic;
namespace SolutionWeb.ViewModels
{
	public partial class VIEW_SYS_DEPT{
		public string page { get; set; }
		public string rows { get; set; }
		public string sort { get; set; }
		public string order { get; set; }
		public string editcellname { get; set; }
		public string DEPT_CODE{ get; set; }
		public string DEPT_NAME{ get; set; }
		public string PARENT_CODE{ get; set; }
		public string C_ICO{ get; set; }
		public string STATUS_FLAG{ get; set; }
		public string DEL_FLAG{ get; set; }
		public string NOTE{ get; set; }
		public Nullable<decimal> DEPT_ORDER{ get; set; }
		public Nullable<decimal> DEPT_FLAG{ get; set; }
		public string ROLE_ID{ get; set; }
	}
}  
