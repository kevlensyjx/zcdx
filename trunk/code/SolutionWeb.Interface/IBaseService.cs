
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SolutionWeb.Interface
{

    public interface IBaseService //: IDisposable
    {
        #region 0.数据源
        /// <summary>
        /// 提供对单表的查询，
        /// </summary>
        /// <returns>IQueryable类型集合</returns>
        IQueryable<T> Entities<T>() where T : class;
        #endregion

        #region 0.根据条件检索集合中的数据并排序
        /// <summary>
        /// 根据条件检索集合中的数据
        /// </summary>
        /// <param name="doWhere"></param>
        /// <param name="doOrder"></param>
        /// <returns></returns>
        List<T> GetListByCondition<T, Tkey>(Expression<Func<T, bool>> doWhere, Expression<Func<T, Tkey>> doOrder) where T : class;

        #endregion
        #region 1.新增实体
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int AddEntity<T>(T entity) where T : class;
        #endregion

        #region 1.批量新增实体
        /// <summary>
        /// 批量新增实体
        /// </summary>
        /// <param name="entitys"></param>
        /// <returns></returns>
        int AddListEntity<T>(List<T> entitys) where T : class;
        #endregion

        #region 2.修改实体
        /// <summary>
        /// 修改实体
        /// </summary>
        /// <param name="entity">修改对象</param>
        /// <param name="pNas">要修改的属性名称集体</param>
        /// <returns></returns>
        int UpdateEntity<T>(T entity) where T : class;
        int UpdateEntity<T>(T entity, params string[] pNas) where T : class;
        int UpdateEntity<T>(T model, params Expression<Func<T, object>>[] ignoreProperties) where T : class;
        int UpdateListEntity<T>(T entity, Expression<Func<T, bool>> dWhere, params string[] uProName) where T : class;
        #endregion


        #region 3.根据PKID主键删除实体
        /// <summary>
        ///  删除实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int DeleteEntity<T>(T entity) where T : class;
        #endregion


        #region 3.根据条件 删除实体
        /// <summary>
        /// 根据条件 删除实体
        /// </summary>
        /// <param name="doWhere"></param>
        /// <returns></returns>
        int DelByWhere<T>(Expression<Func<T, bool>> doWhere) where T : class;
        #endregion


        #region 根据条件 删除实体--批量SQL删除
        /// <summary>
        /// 根据条件 删除实体
        /// </summary>
        /// <param name="doWhere"></param>
        /// <returns></returns>
        int DelBySqlWhere<T>(Expression<Func<T, bool>> doWhere, params object[] dicList) where T : class;
        #endregion

        #region 根据条件 查询实体--执行SQL
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        T GetModel<T>(string sql) where T : class;
        #endregion

        /// <summary>
        /// 执行sql 返回集合
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQueryable<T> ExcuteSqlQuery<T>(string sql, params SqlParameter[] parameters) where T : class;

        /// <summary>
        /// 执行sql，无返回
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        int ExcuteSql<T>(string sql, params SqlParameter[] parameters) where T : class;
    }
}
