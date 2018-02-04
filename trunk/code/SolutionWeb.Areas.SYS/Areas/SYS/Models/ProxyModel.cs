  
  
  
  
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
	public partial class Model_SYS_DEPT
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_DEPT()
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
		public static Model_SYS_DEPT Create
		{
			get
			{
				Model_SYS_DEPT opContext = CallContext.LogicalGetData(typeof(Model_SYS_DEPT).Name) as Model_SYS_DEPT;
				if (opContext == null)
				{
					opContext = new Model_SYS_DEPT();
					CallContext.LogicalSetData(typeof(Model_SYS_DEPT).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_MEMBER
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_MEMBER()
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
		public static Model_SYS_MEMBER Create
		{
			get
			{
				Model_SYS_MEMBER opContext = CallContext.LogicalGetData(typeof(Model_SYS_MEMBER).Name) as Model_SYS_MEMBER;
				if (opContext == null)
				{
					opContext = new Model_SYS_MEMBER();
					CallContext.LogicalSetData(typeof(Model_SYS_MEMBER).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_MENU
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_MENU()
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
		public static Model_SYS_MENU Create
		{
			get
			{
				Model_SYS_MENU opContext = CallContext.LogicalGetData(typeof(Model_SYS_MENU).Name) as Model_SYS_MENU;
				if (opContext == null)
				{
					opContext = new Model_SYS_MENU();
					CallContext.LogicalSetData(typeof(Model_SYS_MENU).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_MENUOPT
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_MENUOPT()
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
		public static Model_SYS_MENUOPT Create
		{
			get
			{
				Model_SYS_MENUOPT opContext = CallContext.LogicalGetData(typeof(Model_SYS_MENUOPT).Name) as Model_SYS_MENUOPT;
				if (opContext == null)
				{
					opContext = new Model_SYS_MENUOPT();
					CallContext.LogicalSetData(typeof(Model_SYS_MENUOPT).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_ROLE
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_ROLE()
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
		public static Model_SYS_ROLE Create
		{
			get
			{
				Model_SYS_ROLE opContext = CallContext.LogicalGetData(typeof(Model_SYS_ROLE).Name) as Model_SYS_ROLE;
				if (opContext == null)
				{
					opContext = new Model_SYS_ROLE();
					CallContext.LogicalSetData(typeof(Model_SYS_ROLE).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_ROLE_MENUOPT_MAP
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_ROLE_MENUOPT_MAP()
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
		public static Model_SYS_ROLE_MENUOPT_MAP Create
		{
			get
			{
				Model_SYS_ROLE_MENUOPT_MAP opContext = CallContext.LogicalGetData(typeof(Model_SYS_ROLE_MENUOPT_MAP).Name) as Model_SYS_ROLE_MENUOPT_MAP;
				if (opContext == null)
				{
					opContext = new Model_SYS_ROLE_MENUOPT_MAP();
					CallContext.LogicalSetData(typeof(Model_SYS_ROLE_MENUOPT_MAP).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_ROLE_MENU_MAP
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_ROLE_MENU_MAP()
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
		public static Model_SYS_ROLE_MENU_MAP Create
		{
			get
			{
				Model_SYS_ROLE_MENU_MAP opContext = CallContext.LogicalGetData(typeof(Model_SYS_ROLE_MENU_MAP).Name) as Model_SYS_ROLE_MENU_MAP;
				if (opContext == null)
				{
					opContext = new Model_SYS_ROLE_MENU_MAP();
					CallContext.LogicalSetData(typeof(Model_SYS_ROLE_MENU_MAP).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_USER
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_USER()
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
		public static Model_SYS_USER Create
		{
			get
			{
				Model_SYS_USER opContext = CallContext.LogicalGetData(typeof(Model_SYS_USER).Name) as Model_SYS_USER;
				if (opContext == null)
				{
					opContext = new Model_SYS_USER();
					CallContext.LogicalSetData(typeof(Model_SYS_USER).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_USER_DEFAULTMENU
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_USER_DEFAULTMENU()
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
		public static Model_SYS_USER_DEFAULTMENU Create
		{
			get
			{
				Model_SYS_USER_DEFAULTMENU opContext = CallContext.LogicalGetData(typeof(Model_SYS_USER_DEFAULTMENU).Name) as Model_SYS_USER_DEFAULTMENU;
				if (opContext == null)
				{
					opContext = new Model_SYS_USER_DEFAULTMENU();
					CallContext.LogicalSetData(typeof(Model_SYS_USER_DEFAULTMENU).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
	public partial class Model_SYS_USER_ROLE_MAP
	{ 
		#region 操作上下文的静态变量
		static OperContext oc = OperContext.CurrentContext;
		#endregion
		#region Ioc
		private IBaseService bs { set; get; }
		public Model_SYS_USER_ROLE_MAP()
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
		public static Model_SYS_USER_ROLE_MAP Create
		{
			get
			{
				Model_SYS_USER_ROLE_MAP opContext = CallContext.LogicalGetData(typeof(Model_SYS_USER_ROLE_MAP).Name) as Model_SYS_USER_ROLE_MAP;
				if (opContext == null)
				{
					opContext = new Model_SYS_USER_ROLE_MAP();
					CallContext.LogicalSetData(typeof(Model_SYS_USER_ROLE_MAP).Name, opContext);
			}
			return opContext;
			}
		} 
		#endregion
	}
}  
