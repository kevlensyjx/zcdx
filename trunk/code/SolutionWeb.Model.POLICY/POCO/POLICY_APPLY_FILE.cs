  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.POLICY
{
	public partial class POLICY_APPLY_FILE{
		public POLICY_APPLY_FILE ToPOCO(){
		return new POLICY_APPLY_FILE(){
				SID = SID,
				CREATE_DT = CREATE_DT,
				CREATE_BY = CREATE_BY,
				UPDATE_DT = UPDATE_DT,
				UPDATE_BY = UPDATE_BY,
				FILE_CLASS = FILE_CLASS,
				MAIN_SID = MAIN_SID,
				FILE_NAME = FILE_NAME,
				FILE_TYPE = FILE_TYPE,
				FILE_PATH = FILE_PATH,
				DOCUMENT_NAME = DOCUMENT_NAME,
				PATENT_NUMBER = PATENT_NUMBER,
			};
		}
	}
}  
