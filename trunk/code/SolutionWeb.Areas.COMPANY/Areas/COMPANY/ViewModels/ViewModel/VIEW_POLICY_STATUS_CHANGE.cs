  
using System;
using System.Collections.Generic;
namespace SolutionWeb.User.ViewModels
{
	public partial class VIEW_POLICY_STATUS_CHANGE{
		public string page { get; set; }
		public string rows { get; set; }
		public string sort { get; set; }
		public string order { get; set; }
		public string editcellname { get; set; }
		public string SID{ get; set; }
		public string POLICY_SID{ get; set; }
		public string DEPT_CODE{ get; set; }
		public string ITEM_TYPE{ get; set; }
		public string STATUS_NAME{ get; set; }
		public string STATUS_CODE{ get; set; }
		public Nullable<System.DateTime> CREATE_DT{ get; set; }
		public string HANDLE_SID{ get; set; }
		public Nullable<System.DateTime> HANDLE_DT{ get; set; }
		public string HANDLE_RESULT{ get; set; }
		public string HANDLE_CONTENT{ get; set; }
		public string IS_HANDLE{ get; set; }
		public string IS_CALCULATE{ get; set; }
		public Nullable<System.DateTime> CALCULATE_DT{ get; set; }
		public Nullable<int> TIME_LIMIT{ get; set; }
		public string HANDLE_NAME{ get; set; }
		public string DEPT_NAME{ get; set; }
		public string IS_VOTE{ get; set; }
		public string SEAL_SID{ get; set; }
	}
}  
