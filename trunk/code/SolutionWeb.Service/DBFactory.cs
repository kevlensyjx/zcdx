using SolutionWeb.Common;
using SolutionWeb.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using System.Diagnostics;

namespace SolutionWeb.Service
{
    public class DBFactory
    {
        public static  DbContext GetDbContext<T>()
        {
            Type final = null;
            Type[] t = typeof(T).Assembly.GetTypes();
            foreach (var item in t)
            {
                if (item.Name.EndsWith("Entities"))
                {
                    final = item;
                    break;
                }
            }
            string contextName = final.Namespace + "." + final.Name;

            DbContext dbContext = CallContext.LogicalGetData(contextName) as DbContext;
            if (dbContext == null)
            {
                dbContext = DIFactory.GetContainer().Resolve<DbContext>(contextName);
                CallContext.LogicalSetData(contextName, dbContext);
            }
            return dbContext;
            
        }
        
    }
}
