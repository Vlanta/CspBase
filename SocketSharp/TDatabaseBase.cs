﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace CSUST
{
    /// <summary>
    /// 数据库抽象类, 只给出了几个抽象方法, 派生后需要增加实现
    /// 1) Open方法, 给出具体的SqlConnection/OleDbConnection
    /// 2) 其它抽象方法, 这些实现要在 TSocketServerBase 中调用
    /// 3) 已经给出了两个派生类：TSqlServerBase/TOleDatabaseBase
    /// </summary>
    public abstract class TDatabaseBase
    {
        protected string m_dbConnectionString = string.Empty;
        protected DbConnection m_dbConnection;

        public event EventHandler<TExceptionEventArgs> DatabaseOpenException;
        public event EventHandler<TExceptionEventArgs> DatabaseException;
        public event EventHandler<TExceptionEventArgs> DatabaseCloseException;

        /// <summary>
        /// 作泛型参数类型时, 必须有无参构造函数
        /// </summary>
        public TDatabaseBase() { }

        public ConnectionState State
        {
            get { return m_dbConnection.State; }
        }

        /// <summary>
        /// 在Session抽象方法AnalyzeDatagram中要该连接对象
        /// </summary>
        public abstract DbConnection DbConnection { get; }

        /// <summary>
        /// 1) 替构造函数初始化参数
        /// 2) dbConnectionString 是数据库连接串
        /// </summary>
        public void Initiate(string dbConnectionString)
        {
            m_dbConnectionString = dbConnectionString;
        }

        /// <summary>
        /// 抽象方法, 重写时需要:
        /// 1) 创建具体类型的连接对象
        ///    (1) Ole数据库连接：m_dbConnection = new OleDbConnection();
        ///    (2) SqlServer连接：m_dbConnection = new SqlConnection();
        /// 2) 创建其它与具体连接对象的对象如：SqlCommand/OleDbCommand等
        /// </summary>
        public abstract void Open();

        public virtual void Store(byte[] datagramBytes, TSessionBase session) { }

        public void Close()
        {
            if (m_dbConnection == null)
            {
                return;
            }

            try
            {
                this.Clear();  // 清理派生类的相关资源
                m_dbConnection.Close();
            }
            catch (Exception err)
            {
                this.OnDatabaseCloseException(err);
            }
        }

        /// <summary>
        /// 1) 关闭数据库前清理非连接(Connection)资源
        /// 2) 可在派生类中重写该方法
        /// </summary>
        protected virtual void Clear() { }

        protected virtual void OnDatabaseOpenException(Exception err)
        {
            EventHandler<TExceptionEventArgs> handler = this.DatabaseOpenException;
            if (handler != null)
            {
                TExceptionEventArgs e = new TExceptionEventArgs(err);
                handler(this, e);
            }
        }

        protected virtual void OnDatabaseCloseException(Exception err)
        {
            EventHandler<TExceptionEventArgs> handler = this.DatabaseCloseException;
            if (handler != null)
            {
                TExceptionEventArgs e = new TExceptionEventArgs(err);
                handler(this, e);
            }
        }

        /// <summary>
        /// Session中触发的事件
        /// </summary>
        protected virtual void OnDatabaseException(Exception err)
        {
            EventHandler<TExceptionEventArgs> handler = this.DatabaseException;
            if (handler != null)
            {
                TExceptionEventArgs e = new TExceptionEventArgs(err);
                handler(this, e);
            }
        }

    }

}
