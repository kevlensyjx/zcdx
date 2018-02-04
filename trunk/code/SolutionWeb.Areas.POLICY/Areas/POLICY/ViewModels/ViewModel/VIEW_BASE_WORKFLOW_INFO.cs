  
using System;
using System.Collections.Generic;
namespace SolutionWeb.ViewModels
{
	public partial class VIEW_BASE_WORKFLOW_INFO{
		public string page { get; set; }
		public string rows { get; set; }
		public string sort { get; set; }
		public string order { get; set; }
		public string editcellname { get; set; }
		public string SID{ get; set; }
		public string ITEM_TYPE{ get; set; }
		public string STATUS_NAME{ get; set; }
		public string STATUS_CODE{ get; set; }
		public string HANDLE_RESULT{ get; set; }
		public string NEXT_STATUS_NAME{ get; set; }
		public string NEXT_STATUS_CODE{ get; set; }
		public Nullable<int> TIME_LIMIT{ get; set; }
		public string BUTTONS_VALUE{ get; set; }
		public string BUTTONS{ get; set; }
		public string BACK_STEP{ get; set; }
		public string PREVIOUS_STEP{ get; set; }
		public string NEXT_STEP{ get; set; }
		public string IS_SEAL{ get; set; }
		public string IS_HAS_FILE{ get; set; }
		public string DEFAULT_HANDLER{ get; set; }
		public string BACK_CLASS{ get; set; }
		public string COUNTERSIGN{ get; set; }
	}
}  
