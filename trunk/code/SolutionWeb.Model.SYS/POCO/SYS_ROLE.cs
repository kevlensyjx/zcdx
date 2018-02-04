  
using System;
using System.Collections.Generic;
namespace SolutionWeb.Model.SYS
{
	public partial class SYS_ROLE{
		public SYS_ROLE ToPOCO(){
		return new SYS_ROLE(){
				ROLE_ID = ROLE_ID,
				NAME = NAME,
				NOTE = NOTE,
			};
		}
	}
}  
