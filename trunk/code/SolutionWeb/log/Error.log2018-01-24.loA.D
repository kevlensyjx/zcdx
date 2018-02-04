[2018-01-24 20:39:14,726] [ERROR] [Error] - 程序启动开始
[2018-01-24 20:39:18,949] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 20:39:18
[2018-01-24 20:39:18,949] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 20:39:18,963] [ERROR] [Error] - 程序启动完成,用时：4418毫秒
[2018-01-24 21:38:35,604] [ERROR] [Error] - 程序启动开始
[2018-01-24 21:38:36,919] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 21:38:36
[2018-01-24 21:38:36,919] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 21:38:36,929] [ERROR] [Error] - 程序启动完成,用时：1503毫秒
[2018-01-24 21:39:41,910] [ERROR] [Error] - 程序启动开始
[2018-01-24 21:39:45,816] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 21:39:45
[2018-01-24 21:39:45,816] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 21:39:45,823] [ERROR] [Error] - 程序启动完成,用时：4038毫秒
[2018-01-24 21:40:49,977] [ERROR] [Error] - System.Reflection.TargetException: 非静态方法需要一个目标。
   在 System.Reflection.RuntimeMethodInfo.CheckConsistency(Object target)
   在 System.Reflection.RuntimeMethodInfo.InvokeArgumentsCheck(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)
   在 System.Reflection.RuntimeMethodInfo.Invoke(Object obj, BindingFlags invokeAttr, Binder binder, Object[] parameters, CultureInfo culture)
   在 System.Reflection.RuntimePropertyInfo.GetValue(Object obj, BindingFlags invokeAttr, Binder binder, Object[] index, CultureInfo culture)
   在 System.Reflection.RuntimePropertyInfo.GetValue(Object obj, Object[] index)
   在 System.Data.Entity.Core.Objects.ELinq.QueryParameterExpression.TryGetFieldOrPropertyValue(MemberExpression me, Object instance, Object& memberValue)
   在 System.Data.Entity.Core.Objects.ELinq.QueryParameterExpression.TryEvaluatePath(Expression expression, ConstantExpression& constantExpression)
   在 System.Data.Entity.Core.Objects.ELinq.QueryParameterExpression.EvaluateParameter(Object[] arguments)
   在 System.Data.Entity.Core.Objects.ELinq.ELinqQueryState.GetExecutionPlan(Nullable`1 forMergeOption)
   在 System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__6()
   在 System.Data.Entity.Core.Objects.ObjectContext.ExecuteInTransaction[T](Func`1 func, IDbExecutionStrategy executionStrategy, Boolean startLocalTransaction, Boolean releaseConnectionOnSuccess)
   在 System.Data.Entity.Core.Objects.ObjectQuery`1.<>c__DisplayClass7.<GetResults>b__5()
   在 System.Data.Entity.SqlServer.DefaultSqlExecutionStrategy.Execute[TResult](Func`1 operation)
   在 System.Data.Entity.Core.Objects.ObjectQuery`1.GetResults(Nullable`1 forMergeOption)
   在 System.Data.Entity.Core.Objects.ObjectQuery`1.<System.Collections.Generic.IEnumerable<T>.GetEnumerator>b__0()
   在 System.Data.Entity.Internal.LazyEnumerator`1.MoveNext()
   在 System.Linq.Enumerable.FirstOrDefault[TSource](IEnumerable`1 source)
   在 System.Data.Entity.Core.Objects.ELinq.ObjectQueryProvider.<GetElementFunction>b__1[TResult](IEnumerable`1 sequence)
   在 System.Data.Entity.Core.Objects.ELinq.ObjectQueryProvider.ExecuteSingle[TResult](IEnumerable`1 query, Expression queryRoot)
   在 System.Data.Entity.Core.Objects.ELinq.ObjectQueryProvider.System.Linq.IQueryProvider.Execute[TResult](Expression expression)
   在 System.Data.Entity.Internal.Linq.DbQueryProvider.Execute[TResult](Expression expression)
   在 System.Linq.Queryable.FirstOrDefault[TSource](IQueryable`1 source)
   在 SolutionWeb.Areas.POLICY.Controllers.POLICY_COMPANY_APPROVEApiController.GetCorpInfo(VIEW_CORPORATION_BASE_INFO data)
[2018-01-24 21:42:13,279] [ERROR] [Error] - 程序启动开始
[2018-01-24 21:42:14,782] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 21:42:14
[2018-01-24 21:42:14,782] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 21:42:14,792] [ERROR] [Error] - 程序启动完成,用时：1689毫秒
[2018-01-24 21:54:00,916] [ERROR] [Error] - 程序启动开始
[2018-01-24 21:54:04,722] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 21:54:04
[2018-01-24 21:54:04,722] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 21:54:04,729] [ERROR] [Error] - 程序启动完成,用时：3939毫秒
[2018-01-24 22:14:55,562] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:14:56,877] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:14:56
[2018-01-24 22:14:56,877] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:14:56,883] [ERROR] [Error] - 程序启动完成,用时：1502毫秒
[2018-01-24 22:15:51,486] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:15:52,672] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:15:52
[2018-01-24 22:15:52,672] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:15:52,678] [ERROR] [Error] - 程序启动完成,用时：1314毫秒
[2018-01-24 22:20:15,508] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:20:16,651] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:20:16
[2018-01-24 22:20:16,651] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:20:16,652] [ERROR] [Error] - 程序启动完成,用时：1269毫秒
[2018-01-24 22:35:11,984] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:35:13,205] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:35:13
[2018-01-24 22:35:13,205] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:35:13,211] [ERROR] [Error] - 程序启动完成,用时：1344毫秒
[2018-01-24 22:40:48,610] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:40:49,861] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:40:49
[2018-01-24 22:40:49,861] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:40:49,868] [ERROR] [Error] - 程序启动完成,用时：1379毫秒
[2018-01-24 22:44:30,717] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:44:31,928] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:44:31
[2018-01-24 22:44:31,928] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:44:31,934] [ERROR] [Error] - 程序启动完成,用时：1356毫秒
[2018-01-24 22:57:05,540] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:57:06,745] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:57:06
[2018-01-24 22:57:06,745] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:57:06,751] [ERROR] [Error] - 程序启动完成,用时：1338毫秒
[2018-01-24 22:58:03,686] [ERROR] [Error] - 程序启动开始
[2018-01-24 22:58:04,844] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 22:58:04
[2018-01-24 22:58:04,844] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 22:58:04,846] [ERROR] [Error] - 程序启动完成,用时：1290毫秒
[2018-01-24 23:05:31,930] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:05:33,103] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:05:33
[2018-01-24 23:05:33,104] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:05:33,104] [ERROR] [Error] - 程序启动完成,用时：1303毫秒
[2018-01-24 23:09:45,193] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:09:46,385] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:09:46
[2018-01-24 23:09:46,386] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:09:46,392] [ERROR] [Error] - 程序启动完成,用时：1323毫秒
[2018-01-24 23:15:51,405] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:15:52,607] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:15:52
[2018-01-24 23:15:52,608] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:15:52,614] [ERROR] [Error] - 程序启动完成,用时：1334毫秒
[2018-01-24 23:17:15,204] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:17:16,382] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:17:16
[2018-01-24 23:17:16,382] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:17:16,383] [ERROR] [Error] - 程序启动完成,用时：1303毫秒
[2018-01-24 23:18:56,121] [ERROR] [Error] - 手机号：,内容：发送失败
[2018-01-24 23:19:26,072] [ERROR] [Error] - 手机号：,内容：发送失败
[2018-01-24 23:21:58,811] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:22:00,056] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:22:00
[2018-01-24 23:22:00,057] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:22:00,063] [ERROR] [Error] - 程序启动完成,用时：1377毫秒
[2018-01-24 23:24:54,413] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:24:55,629] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:24:55
[2018-01-24 23:24:55,629] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:24:55,636] [ERROR] [Error] - 程序启动完成,用时：1358毫秒
[2018-01-24 23:26:37,105] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:26:38,355] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:26:38
[2018-01-24 23:26:38,355] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:26:38,361] [ERROR] [Error] - 程序启动完成,用时：1381毫秒
[2018-01-24 23:31:42,330] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:31:43,524] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:31:43
[2018-01-24 23:31:43,524] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:31:43,530] [ERROR] [Error] - 程序启动完成,用时：1325毫秒
[2018-01-24 23:32:27,906] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:32:29,122] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:32:29
[2018-01-24 23:32:29,123] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:32:29,129] [ERROR] [Error] - 程序启动完成,用时：1352毫秒
[2018-01-24 23:35:21,647] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:35:22,829] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:35:22
[2018-01-24 23:35:22,829] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:35:22,835] [ERROR] [Error] - 程序启动完成,用时：1321毫秒
[2018-01-24 23:37:18,527] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:37:19,999] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:37:19
[2018-01-24 23:37:19,999] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:37:20,014] [ERROR] [Error] - 程序启动完成,用时：1688毫秒
[2018-01-24 23:38:44,789] [ERROR] [Error] - 程序启动开始
[2018-01-24 23:38:48,666] [ERROR] [Error] - 定时服务POLICY_TIMER启动:2018-01-24 23:38:48
[2018-01-24 23:38:48,666] [ERROR] [Error] - 注册定时服务成功
[2018-01-24 23:38:48,672] [ERROR] [Error] - 程序启动完成,用时：4006毫秒
