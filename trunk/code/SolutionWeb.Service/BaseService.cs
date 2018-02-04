using SolutionWeb.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using SolutionWeb.Service.Visitor;
using System.Data.SqlClient;

namespace SolutionWeb.Service
{

    public class BaseService : IBaseService
    {


        #region 0.数据源
        public IQueryable<T> Entities<T>() where T : class
        {
            Type[] t = typeof(T).Assembly.GetTypes();
            return DBFactory.GetDbContext<T>().Set<T>().AsNoTracking();
        }
        #endregion

        #region 0.根据条件检索集合中的数据并排序
        /// <summary>
        /// 根据条件检索集合中的数据
        /// </summary>
        /// <param name="doWhere"></param>
        /// <param name="doOrder"></param>
        /// <returns></returns>
        public List<T> GetListByCondition<T, Tkey>(Expression<Func<T, bool>> doWhere, Expression<Func<T, Tkey>> doOrder) where T : class
        {
            return DBFactory.GetDbContext<T>().Set<T>().Where(doWhere).AsNoTracking().OrderBy(doOrder).ToList();
        }
        #endregion

        #region 1.新增实体
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int AddEntity<T>(T entity) where T : class
        {
            DbContext _dbx = DBFactory.GetDbContext<T>();
            _dbx.Set<T>().Add(entity);
            int count = _dbx.SaveChanges();
            return count;
        }
        #endregion
        #region 1.批量新增实体
        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public int AddListEntity<T>(List<T> entitys) where T : class
        {

            DbContext _dbx = DBFactory.GetDbContext<T>();

            foreach (T entity in entitys)
            {
                _dbx.Set<T>().Add(entity);
            }
            return _dbx.SaveChanges();

        }
        #endregion
        #region 2.修改实体
        public int UpdateEntity<T>(T entity) where T : class
        {
            DbContext _dbx = DBFactory.GetDbContext<T>();
            _dbx.Entry<T>(entity).State = EntityState.Detached;

            DbEntityEntry upEntity = _dbx.Entry<T>(entity);
            upEntity.State = EntityState.Unchanged;


            Type t = typeof(T);
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();

            Dictionary<string, PropertyInfo> dicPros = new Dictionary<string, PropertyInfo>();
            proInfos.ForEach(p => dicPros.Add(p.Name, p));
            foreach (string proName in dicPros.Keys)
            {
                //是否可以实现值为NULL时改为空
                upEntity.Property(proName).IsModified = true;
            }
            return _dbx.SaveChanges();

        }
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">修改对象</param>
        /// <param name="pNas">要修改的属性名称集体</param>
        /// <returns></returns>
        public int UpdateEntity<T>(T entity, params string[] pNas) where T : class
        {
            DbContext _dbx = DBFactory.GetDbContext<T>();
            _dbx.Entry<T>(entity).State = EntityState.Detached;//oracle数据库没这一句不能修改

            DbEntityEntry upEntity = _dbx.Entry<T>(entity);
            upEntity.State = EntityState.Unchanged;
            foreach (string pNa in pNas)
            {
                upEntity.Property(pNa).IsModified = true;
            }
            return _dbx.SaveChanges();

        }
        public int UpdateEntity<T>(T entity, params Expression<Func<T, object>>[] ignoreProperties) where T : class
        {

            DbContext _dbx = DBFactory.GetDbContext<T>();
            _dbx.Entry<T>(entity).State = EntityState.Detached;
            _dbx.Set<T>().Attach(entity);

            DbEntityEntry entry = _dbx.Entry<T>(entity);
            entry.State = EntityState.Unchanged;

            Type t = typeof(T);
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();

            Dictionary<string, PropertyInfo> dicPros = new Dictionary<string, PropertyInfo>();
            proInfos.ForEach(p => dicPros.Add(p.Name, p));

            if (ignoreProperties != null)
            {
                foreach (var ignorePropertyExpression in ignoreProperties)
                {
                    //根据表达式得到对应的字段信息
                    var ignorePropertyName = new PropertyExpressionParser<T>(ignorePropertyExpression).Name;
                    dicPros.Remove(ignorePropertyName);
                }
            }

            foreach (string proName in dicPros.Keys)
            {
                entry.Property(proName).IsModified = true;
            }
            return _dbx.SaveChanges();

        }
        #endregion

        #region 批量修改
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="dWhere"></param>
        /// <param name="uProName">要修改的列名</param>
        /// <returns></returns>
        public int UpdateListEntity<T>(T entity, Expression<Func<T, bool>> dWhere, params string[] uProName) where T : class
        {

            DbContext db = DBFactory.GetDbContext<T>();
            //1.查询要修改的数据
            List<T> listModifing = db.Set<T>().Where(dWhere).ToList();
            //2.获取实体类对象
            Type t = typeof(T);
            //3.获取实体类的所有属性
            List<PropertyInfo> pInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //4.创建一个实体属性的字典集合
            Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
            //5.将实体属性中要修改的属性添加到字典集合中，健：属性名，值：属性对象
            pInfos.ForEach(p =>
            {
                if (uProName.Contains(p.Name))
                {
                    dictPros.Add(p.Name, p);
                }
            });
            //6.循环要修改的属性名
            foreach (string proName in uProName)
            {
                //如果存在则取出要修改的属性
                PropertyInfo proInfo = dictPros[proName];
                //取出要修改的值
                object newValue = proInfo.GetValue(entity, null);
                //批量设置要修改对象的属性
                foreach (T su in listModifing)
                {
                    //为要修改的对象属性设置新的值
                    proInfo.SetValue(su, newValue, null);
                }

            }
            return db.SaveChanges();
        }
        #endregion

        #region 3.根据PKID主键删除实体
        /// <summary>
        ///  删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int DeleteEntity<T>(T entity) where T : class
        {
            DbContext _dbx = DBFactory.GetDbContext<T>();
            _dbx.Set<T>().Attach(entity);
            _dbx.Entry<T>(entity).State = EntityState.Deleted;
            return _dbx.SaveChanges();
        }
        #endregion


        #region 3.根据条件 删除实体
        /// <summary>
        /// 根据条件 删除实体
        /// </summary>
        /// <param name="doWhere"></param>
        /// <returns></returns>
        public int DelByWhere<T>(Expression<Func<T, bool>> doWhere) where T : class
        {
            DbContext _dbx = DBFactory.GetDbContext<T>();
            List<T> listDels = _dbx.Set<T>().Where(doWhere).ToList();

            listDels.ForEach(d =>
            {
                _dbx.Set<T>().Attach(d);
                _dbx.Set<T>().Remove(d);
            });
            return _dbx.SaveChanges();

        }
        #endregion


        #region 3.根据条件 删除实体--批量SQL删除
        /// <summary>
        /// 根据条件 删除实体
        /// </summary>
        /// <param name="doWhere"></param>
        /// <returns></returns>
        public int DelBySqlWhere<T>(Expression<Func<T, bool>> doWhere, params object[] dicList) where T : class
        {

            ConditionBuilderVisitor visitor = new ConditionBuilderVisitor();
            visitor.Visit(doWhere);
            string condition = visitor.Condition();

            string sql = string.Format("DELETE FROM {0} WHERE {1}"
                , typeof(T).Name
                , condition);

            string param = "";
            foreach (var item in dicList)
            {
                Type type = item.GetType();
                if (type == typeof(List<string>))
                {
                    param = "'" + string.Join("','", ((List<string>)item).ToArray()) + "'";
                    continue;
                }
                if (type == typeof(List<int>))
                {
                    param = string.Join(",", ((List<int>)item).ToArray());
                    continue;
                }
                if (type == typeof(List<int?>))
                {
                    param = string.Join(",", ((List<int?>)item).ToArray());
                    continue;
                }
                if (type == typeof(List<decimal>))
                {
                    param = string.Join(",", ((List<decimal>)item).ToArray());
                    continue;
                }
                if (type == typeof(List<decimal?>))
                {
                    param = string.Join(",", ((List<decimal?>)item).ToArray());
                    continue;
                }
                sql = sql.Replace("^" + item.ToString() + "^", param);
            }

            return ExcuteSql<T>(sql);
        }
        #endregion

        public T GetModel<T>(string sql) where T : class
        {
            DbContext _dbx = DBFactory.GetDbContext<T>();
            return _dbx.Set<T>().SqlQuery(sql).FirstOrDefault();

        }

        public IQueryable<T> ExcuteSqlQuery<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            DbContext _dbx = DBFactory.GetDbContext<T>();
            return _dbx.Database.SqlQuery<T>(sql, parameters).AsQueryable();
        }

        public int ExcuteSql<T>(string sql, params SqlParameter[] parameters) where T : class
        {
            //DbContextTransaction trans = null;
            //try
            //{
            DbContext _dbx = DBFactory.GetDbContext<T>();
            //trans = _dbx.Database.BeginTransaction();
            return _dbx.Database.ExecuteSqlCommand(sql, parameters);
            //    trans.Commit();
            //}
            //catch (Exception ex)
            //{
            //    if (trans != null)
            //        trans.Rollback();
            //    throw ex;
            //}
        }
    }
}
