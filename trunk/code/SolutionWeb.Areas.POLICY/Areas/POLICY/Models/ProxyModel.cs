  
  
  
  
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolutionWeb.Common;
using SolutionWeb.Interface;
using Microsoft.Practices.Unity;
using System.Runtime.Remoting.Messaging;
namespace SolutionWeb.Models
{
	public partial class Model_BASE_PROJECT_CONFIG
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_BASE_PROJECT_CONFIG()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_BASE_PROJECT_CONFIG Create
		{
			get
			{
				Model_BASE_PROJECT_CONFIG opContext = CallContext.LogicalGetData(typeof(Model_BASE_PROJECT_CONFIG).Name) as Model_BASE_PROJECT_CONFIG;
				if (opContext == null)
				{
					opContext = new Model_BASE_PROJECT_CONFIG();
					CallContext.LogicalSetData(typeof(Model_BASE_PROJECT_CONFIG).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_BASE_PROJECT_INFO
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_BASE_PROJECT_INFO()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_BASE_PROJECT_INFO Create
		{
			get
			{
				Model_BASE_PROJECT_INFO opContext = CallContext.LogicalGetData(typeof(Model_BASE_PROJECT_INFO).Name) as Model_BASE_PROJECT_INFO;
				if (opContext == null)
				{
					opContext = new Model_BASE_PROJECT_INFO();
					CallContext.LogicalSetData(typeof(Model_BASE_PROJECT_INFO).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_BASE_STATUS_DIC
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_BASE_STATUS_DIC()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_BASE_STATUS_DIC Create
		{
			get
			{
				Model_BASE_STATUS_DIC opContext = CallContext.LogicalGetData(typeof(Model_BASE_STATUS_DIC).Name) as Model_BASE_STATUS_DIC;
				if (opContext == null)
				{
					opContext = new Model_BASE_STATUS_DIC();
					CallContext.LogicalSetData(typeof(Model_BASE_STATUS_DIC).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_BASE_WORKFLOW_INFO
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_BASE_WORKFLOW_INFO()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_BASE_WORKFLOW_INFO Create
		{
			get
			{
				Model_BASE_WORKFLOW_INFO opContext = CallContext.LogicalGetData(typeof(Model_BASE_WORKFLOW_INFO).Name) as Model_BASE_WORKFLOW_INFO;
				if (opContext == null)
				{
					opContext = new Model_BASE_WORKFLOW_INFO();
					CallContext.LogicalSetData(typeof(Model_BASE_WORKFLOW_INFO).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_CORPORATION_BASE_INFO
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_CORPORATION_BASE_INFO()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_CORPORATION_BASE_INFO Create
		{
			get
			{
				Model_CORPORATION_BASE_INFO opContext = CallContext.LogicalGetData(typeof(Model_CORPORATION_BASE_INFO).Name) as Model_CORPORATION_BASE_INFO;
				if (opContext == null)
				{
					opContext = new Model_CORPORATION_BASE_INFO();
					CallContext.LogicalSetData(typeof(Model_CORPORATION_BASE_INFO).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_POLICY_APPLY_FILE
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_POLICY_APPLY_FILE()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_POLICY_APPLY_FILE Create
		{
			get
			{
				Model_POLICY_APPLY_FILE opContext = CallContext.LogicalGetData(typeof(Model_POLICY_APPLY_FILE).Name) as Model_POLICY_APPLY_FILE;
				if (opContext == null)
				{
					opContext = new Model_POLICY_APPLY_FILE();
					CallContext.LogicalSetData(typeof(Model_POLICY_APPLY_FILE).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_POLICY_MAIN_INFO
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_POLICY_MAIN_INFO()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_POLICY_MAIN_INFO Create
		{
			get
			{
				Model_POLICY_MAIN_INFO opContext = CallContext.LogicalGetData(typeof(Model_POLICY_MAIN_INFO).Name) as Model_POLICY_MAIN_INFO;
				if (opContext == null)
				{
					opContext = new Model_POLICY_MAIN_INFO();
					CallContext.LogicalSetData(typeof(Model_POLICY_MAIN_INFO).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_POLICY_NOTICE_INFO
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_POLICY_NOTICE_INFO()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_POLICY_NOTICE_INFO Create
		{
			get
			{
				Model_POLICY_NOTICE_INFO opContext = CallContext.LogicalGetData(typeof(Model_POLICY_NOTICE_INFO).Name) as Model_POLICY_NOTICE_INFO;
				if (opContext == null)
				{
					opContext = new Model_POLICY_NOTICE_INFO();
					CallContext.LogicalSetData(typeof(Model_POLICY_NOTICE_INFO).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_POLICY_STATUS_CHANGE
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_POLICY_STATUS_CHANGE()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_POLICY_STATUS_CHANGE Create
		{
			get
			{
				Model_POLICY_STATUS_CHANGE opContext = CallContext.LogicalGetData(typeof(Model_POLICY_STATUS_CHANGE).Name) as Model_POLICY_STATUS_CHANGE;
				if (opContext == null)
				{
					opContext = new Model_POLICY_STATUS_CHANGE();
					CallContext.LogicalSetData(typeof(Model_POLICY_STATUS_CHANGE).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_POLICY_APPLY_PUBLICITY_DETAIL
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_POLICY_APPLY_PUBLICITY_DETAIL()
		{
			bs = uc.Resolve<IBaseService>();
		}
		private IUnityContainer uc
		{
			get
			{
				return DIFactory.GetContainer();
			}
		}
		#endregion
		#region 实例化
		public static Model_POLICY_APPLY_PUBLICITY_DETAIL Create
		{
			get
			{
				Model_POLICY_APPLY_PUBLICITY_DETAIL opContext = CallContext.LogicalGetData(typeof(Model_POLICY_APPLY_PUBLICITY_DETAIL).Name) as Model_POLICY_APPLY_PUBLICITY_DETAIL;
				if (opContext == null)
				{
					opContext = new Model_POLICY_APPLY_PUBLICITY_DETAIL();
					CallContext.LogicalSetData(typeof(Model_POLICY_APPLY_PUBLICITY_DETAIL).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
}  
