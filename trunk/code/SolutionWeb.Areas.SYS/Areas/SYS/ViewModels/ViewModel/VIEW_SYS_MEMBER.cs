  
using System;
using System.Collections.Generic;
namespace SolutionWeb.ViewModels
{
	public partial class VIEW_SYS_MEMBER{
		public string page { get; set; }
		public string rows { get; set; }
		public string sort { get; set; }
		public string order { get; set; }
		public string editcellname { get; set; }
		public string MEMBER_ID{ get; set; }
		public string DEPT_CODE{ get; set; }
		public string NAME{ get; set; }
		public string PHONE{ get; set; }
		public string JOB{ get; set; }
		public string NOTE{ get; set; }
		public Nullable<System.DateTime> UPDATE_DATE{ get; set; }
		public string UPDATE_USER{ get; set; }
		public string DEL_FLAG{ get; set; }
	}
}  
