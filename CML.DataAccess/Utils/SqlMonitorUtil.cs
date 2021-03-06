﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CML.DataAccess.DbClient;
using CML.Lib.Logging;

namespace CML.DataAccess.Utils
{
    /// <summary>
    /// Copyright (C) 2017 cml 版权所有。
    /// 类名：SqlMonitorUtil.cs
    /// 类属性：公共类（非静态）
    /// 类功能描述：SqlMonitorUtil
    /// 创建标识：cml 2018/2/7 16:40:34
    /// </summary>
    internal static class SqlMonitorUtil
    {
        static readonly ILog LogUtil = LogFactory.GetInstance(typeof(SqlMonitorUtil));
        #region 监控消耗时间

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <param name="action">执行方法</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="memberName">调用方法</param>
        public static void Monitor(Action action, string dbType = null, string memberName = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql方法:{memberName}");
                LogUtil.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <param name="query">SqlQuery</param>
        /// <param name="action">执行方法</param>
        /// <param name="dbType">数据库类型</param>
        public static void Monitor(SqlQuery query, Action action, string dbType = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql语句:{query.CommandText}");
                LogUtil.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="action">执行方法</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="memberName">调用方法</param>
        /// <returns>返回值</returns>
        public static T Monitor<T>(Func<T> action, string dbType = null, string memberName = null)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql方法:{memberName}");
                LogUtil.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="query">SqlQuery</param>
        /// <param name="action">执行方法</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回值</returns>
        public static T Monitor<T>(SqlQuery query, Func<T> action, string dbType = null)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql语句:{query.CommandText}");
                LogUtil.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <param name="action">异步方法</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="memberName">调用方法</param>
        /// <returns>任务</returns>
        public async static Task MonitorAsync(Func<Task> action, string dbType = null, string memberName = null)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql方法:{memberName}");
                LogUtil.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <param name="query">SqlQuery</param>
        /// <param name="action">异步方法</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>任务</returns>
        public async static Task MonitorAsync(SqlQuery query, Func<Task> action, string dbType = null)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql语句:{query.CommandText}");
                LogUtil.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="action">异步方法</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="memberName">调用方法</param>
        /// <returns>返回值</returns>
        public async static Task<T> MonitorAsync<T>(Func<Task<T>> action, string dbType = null, string memberName = null)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql方法:{memberName}");
                LogUtil.Error(ex);
                throw;
            }
        }

        /// <summary>
        /// 监控消耗时间
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="query">SqlQuery</param>
        /// <param name="action">异步方法</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回值</returns>
        public async static Task<T> MonitorAsync<T>(SqlQuery query, Func<Task<T>> action, string dbType = null)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                LogUtil.Error($"执行的sql语句:{query.CommandText}");
                LogUtil.Error(ex);
                throw;
            }
        }

        #endregion 监控消耗时间
    }
}