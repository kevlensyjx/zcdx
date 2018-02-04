  
using System;
using System.Collections.Generic;
namespace SolutionWeb.User.ViewModels
{
	public partial class VIEW_POLICY_NOTICE_INFO{
		public string page { get; set; }
		public string rows { get; set; }
		public string sort { get; set; }
		public string order { get; set; }
		public string editcellname { get; set; }
		public string SID{ get; set; }
		public Nullable<System.DateTime> CREATE_DT{ get; set; }
		public string CREATE_BY{ get; set; }
		public Nullable<System.DateTime> UPDATE_DT{ get; set; }
		public string UPDATE_BY{ get; set; }
		public string NOTICE_TITLE{ get; set; }
		public string NOTICE_CONTENT{ get; set; }
		public string IS_SHOW{ get; set; }
		public string NOTICE_TYPE{ get; set; }
	}
}  
