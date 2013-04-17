using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Data;
using System.Threading;

namespace CSUST
{
    public class TSocketServerBase<TSession, TDatabase> : IDisposable
        where TSession : TSessionBase, new()
        where TDatabase : TDatabaseBase, new()
    {
        #region  member fields

        private Socket m_serverSocket;
        private int m_loopWaitTime = 25;  // millisecond
        private bool m_serverClosed = true;
        private bool m_serverListenPaused = false;
        private int m_servertPort = 3130;

        private int m_sessionSequenceNo = 0;  // sessionID 流水号
        private int m_sessionCount;
        private int m_receivedDatagramCount;
        private int m_errorDatagramCount;
        private int m_datagramQueueLength;

        private int m_databaseExceptionCount;
        private int m_serverExceptCount;
        private int m_sessionExceptionCount;

        private int m_maxReceiveBufferSize = 16 * 1024;  // 64 K
        private int m_maxDatagramSize = 1024 * 1024;     // 1M
        private int m_maxSessionTableLength = 1024;
        private int m_maxSessionTimeout = 120;   // 2 minutes
        private int m_maxListenQueueLength = 16;
        private int m_maxSameIPCount = 64;

        private Dictionary<int, TSession> m_sessionTable;
        private TDatabase m_databaseObj = null;

        private bool m_disposed = false;
        private ManualResetEvent m_checkServerListenResetEvent;
        private ManualResetEvent m_checkSessionTableResetEvent;
        private ManualResetEvent m_checkDatagramQueueResetEvent;

        #endregion

        #region  public properties
        /// <summary>
        /// 服务器已经关闭
        /// </summary>
        public bool Closed
        {
            get { return m_serverClosed; }
        }
        /// <summary>
        /// 服务器暂时停止客户端连接请求
        /// </summary>
        public bool ListenPaused
        {
            get { return m_serverListenPaused; }
        }
        /// <summary>
        /// 服务器端口号，默认值为3130
        /// </summary>
        public int ServerPort
        {
            set { m_servertPort = value; }
        }
        /// <summary>
        /// 服务器异常次数
        /// </summary>
        public int ServerExceptionCount
        {
            get { return m_serverExceptCount; }
        }

        public int DatabaseExceptionCount
        {
            get { return m_databaseExceptionCount; }
        }
        /// <summary>
        /// 会话异常个数
        /// </summary>
        public int SessionExceptionCount
        {
            get { return m_sessionExceptionCount; }
        }
        /// <summary>
        /// 当前会话个数
        /// </summary>
        public int SessionCount
        {
            get { return m_sessionCount; }
        }
        /// <summary>
        /// 接收数据包个数
        /// </summary>
        public int ReceivedDatagramCount
        {
            get { return m_receivedDatagramCount; }
        }
        /// <summary>
        /// 错误数据包个数
        /// </summary>
        public int ErrorDatagramCount
        {
            get { return m_errorDatagramCount; }
        }

        public int DatagramQueueLength
        {
            get { return m_datagramQueueLength; }
        }
        /// <summary>
        /// Socket.Listen方法中的等待时间(ms)，默认值为25ms
        /// </summary>
        public int LoopWaitTime
        {
            set
            {
                if (value < 0)
                {
                    m_loopWaitTime = value;
                }
                else
                {
                    m_loopWaitTime = value;
                }
            }
        }
        /// <summary>
        /// 允许最大会话表长度，默认值为1024
        /// </summary>
        public int MaxSessionTableLength
        {
            set
            {
                if (value <= 1)
                {
                    m_maxSessionTableLength = 1;
                }
                else
                {
                    m_maxSessionTableLength = value;
                }
            }
        }
        /// <summary>
        /// 允许数据包接收缓冲区的最大长度，默认值为16K
        /// </summary>
        public int MaxReceiveBufferSize
        {
            set
            {
                if (value < 1024)
                {
                    m_maxReceiveBufferSize = 1024;
                }
                else
                {
                    m_maxReceiveBufferSize = value;
                }
            }
        }
        /// <summary>
        /// 允许数据包的最大长度，默认值为1024K
        /// </summary>
        public int MaxDatagramSize
        {
            set
            {
                if (value < 1024)
                {
                    m_maxDatagramSize = 1024;
                }
                else
                {
                    m_maxDatagramSize = value;
                }
            }
        }
        /// <summary>
        /// 最大侦听队列长度，默认值为16
        /// </summary>
        public int MaxListenQueueLength
        {
            set
            {
                if (value <= 1)
                {
                    m_maxListenQueueLength = 2;
                }
                else
                {
                    m_maxListenQueueLength = value;
                }
            }
        }
        /// <summary>
        /// 允许最大的会话超时间隔(s)，默认值为120s
        /// </summary>
        public int MaxSessionTimeout
        {
            set
            {
                if (value < 120)
                {
                    m_maxSessionTimeout = 120;
                }
                else
                {
                    m_maxSessionTimeout = value;
                }
            }
        }
        /// <summary>
        /// 允许同地址IP的会话Socket个数，默认值为64
        /// </summary>
        public int MaxSameIPCount
        {
            set
            {
                if (value < 1)
                {
                    m_maxSameIPCount = 1;
                }
                else
                {
                    m_maxSameIPCount = value;
                }
            }
        }
        /// <summary>
        /// 当前会话表信息清单
        /// </summary>
        public List<TSessionCoreInfo> SessionCoreInfoList
        {
            get
            {
                List<TSessionCoreInfo> sessionList = new List<TSessionCoreInfo>();
                lock (m_sessionTable)
                {
                    foreach (TSession session in m_sessionTable.Values)
                    {
                        sessionList.Add((TSessionCoreInfo)session);
                    }
                }
                return sessionList;
            }
        }

        #endregion

        #region  class events
        /// <summary>
        /// 服务器启动后
        /// </summary>
        public event EventHandler ServerStarted;
        /// <summary>
        /// 服务器关闭后
        /// </summary>
        public event EventHandler ServerClosed;
        /// <summary>
        /// 服务器暂停连接请求后
        /// </summary>
        public event EventHandler ServerListenPaused;
        /// <summary>
        /// 服务器恢复连接请求后
        /// </summary>
        public event EventHandler ServerListenResumed;
        /// <summary>
        /// 服务器异常
        /// </summary>
        public event EventHandler<TExceptionEventArgs> ServerException;
        /// <summary>
        /// 连接请求被拒绝
        /// </summary>
        public event EventHandler SessionRejected;
        /// <summary>
        /// 建立一个会话连接
        /// </summary>
        public event EventHandler<TSessionEventArgs> SessionConnected;
        /// <summary>
        /// 断开一个会话连接
        /// </summary>
        public event EventHandler<TSessionEventArgs> SessionDisconnected;
        /// <summary>
        /// 会话超时
        /// </summary>
        public event EventHandler<TSessionEventArgs> SessionTimeout;
        /// <summary>
        /// 数据包界限符错误
        /// </summary>
        public event EventHandler<TSessionEventArgs> DatagramDelimiterError;
        /// <summary>
        /// 数据包超长错误
        /// </summary>
        public event EventHandler<TSessionEventArgs> DatagramOversizeError;
        /// <summary>
        /// 会话接收数据异常
        /// </summary>
        public event EventHandler<TSessionExceptionEventArgs> SessionReceiveException;
        /// <summary>
        /// 会话发送数据异常
        /// </summary>
        public event EventHandler<TSessionExceptionEventArgs> SessionSendException;
        /// <summary>
        /// 接受了一个完整数据包
        /// </summary>
        public event EventHandler<TSessionEventArgs> DatagramAccepted;
        /// <summary>
        /// 数据包错误
        /// </summary>
        public event EventHandler<TSessionEventArgs> DatagramError;
        /// <summary>
        /// 处理了一个数据包
        /// </summary>
        public event EventHandler<TSessionEventArgs> DatagramHandled;
        /// <summary>
        /// 数据库打开异常
        /// </summary>
        public event EventHandler<TExceptionEventArgs> DatabaseOpenException;
        /// <summary>
        /// 数据库关闭异常
        /// </summary>
        public event EventHandler<TExceptionEventArgs> DatabaseCloseExcpetion;
        /// <summary>
        /// 数据库异常
        /// </summary>
        public event EventHandler<TExceptionEventArgs> DatabaseExcpetion;

        public event EventHandler<TExceptionEventArgs> ShowDebugMessage;

        #endregion

        #region  class constructor

        public TSocketServerBase(string dbConnectionString)
        {
            this.Initiate(dbConnectionString);
        }

        public TSocketServerBase(int tcpPort, string dbConnectionString)
        {
            m_servertPort = tcpPort;
            this.Initiate(dbConnectionString);
        }

        private void Initiate(string dbConnectionString)
        {
            m_sessionTable = new Dictionary<int, TSession>();

            m_checkServerListenResetEvent = new ManualResetEvent(true);
            m_checkSessionTableResetEvent = new ManualResetEvent(true);
            m_checkDatagramQueueResetEvent = new ManualResetEvent(true);

            m_databaseObj = new TDatabase();
            m_databaseObj.Initiate(dbConnectionString);

            m_databaseObj.DatabaseOpenException += new EventHandler<TExceptionEventArgs>(this.OnDatabaseOpenException);  // 转递数据库事件
            m_databaseObj.DatabaseCloseException += new EventHandler<TExceptionEventArgs>(this.OnDatabaseCloseException);
            m_databaseObj.DatabaseException += new EventHandler<TExceptionEventArgs>(this.OnDatabaseException);
        }

        ~TSocketServerBase()  //
        {
            this.Dispose(false);
        }
        /// <summary>
        /// 关闭服务器并释放系统资源
        /// </summary>
        public void Dispose()
        {
            if (!m_disposed)
            {
                m_disposed = true;
                this.Close();
                this.Dispose(true);
                GC.SuppressFinalize(this);  // Finalize 不会第二次执行
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)  // 对象正在被显示释放, 不是执行 Finalize()
            {
                // 释放托管资源
                m_sessionTable = null;  
            }
            // 释放非托管资源
            m_checkServerListenResetEvent.Close();  
            m_checkSessionTableResetEvent.Close();
            m_checkDatagramQueueResetEvent.Close();
        }

        #endregion

        #region  public methods
        /// <summary>
        /// 启动服务器
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            if (!m_serverClosed)
            {
                return true;
            }

            m_serverClosed = true;  // 在其它方法中要判断该字段
            m_serverListenPaused = true;

            this.Close();
            this.ClearCountValues();

            try
            {
                m_databaseObj.Open();
                if (m_databaseObj.State != ConnectionState.Open)
                {
                    return false;
                }

                if (!this.CreateServerSocket()) return false;
                //客户端监听
                if (!ThreadPool.QueueUserWorkItem(this.StartServerListen)) return false;
                //数据包处理
                if (!ThreadPool.QueueUserWorkItem(this.CheckDatagramQueue)) return false;
                //会话表检测
                if (!ThreadPool.QueueUserWorkItem(this.CheckSessionTable)) return false;

                m_serverClosed = false;
                m_serverListenPaused = false;

                this.OnServerStarted();
            }
            catch (Exception err)
            {
                this.OnServerException(err);
            }
            return !m_serverClosed;
        }

        /// <summary>
        /// 暂停侦听连接请求
        /// </summary>
        public void PauseListen()
        {
            m_serverListenPaused = true;
            this.OnServerListenPaused();
        }
        /// <summary>
        /// 恢复侦听连接请求
        /// </summary>
        public void ResumeListen()
        {
            m_serverListenPaused = false;
            this.OnServerListenResumed();
        }
        /// <summary>
        /// 关闭服务器
        /// </summary>
        public void Stop()
        {
            this.Close();
        }
        /// <summary>
        /// 关闭一个会话
        /// </summary>
        /// <param name="sessionID"></param>
        public void CloseSession(int sessionID)
        {
            TSession session = null;
            lock (m_sessionTable)
            {
                if (m_sessionTable.ContainsKey(sessionID))  // 包含该会话 ID
                {
                    session = (TSession)m_sessionTable[sessionID];
                }
            }

            if (session != null)
            {
                session.SetInactive();
            }
        }
        /// <summary>
        /// 关闭全部会话
        /// </summary>
        public void CloseAllSessions()
        {
            lock (m_sessionTable)
            {
                foreach (TSession session in m_sessionTable.Values)
                {
                    session.SetInactive();
                }
            }
        }
        /// <summary>
        /// 给一个会话发送消息
        /// </summary>
        /// <param name="sessionID"></param>
        /// <param name="datagramText"></param>
        public void SendToSession(int sessionID, string datagramText)
        {
            TSession session = null;
            lock (m_sessionTable)
            {
                session = (TSession)m_sessionTable[sessionID];
            }

            if (session != null)
            {
                session.SendDatagram(datagramText);
            }
        }
        /// <summary>
        /// 给所有会话发送消息
        /// </summary>
        /// <param name="datagramText"></param>
        public void SendToAllSessions(string datagramText)
        {
            lock (m_sessionTable)
            {
                foreach (TSession session in m_sessionTable.Values)
                {
                    session.SendDatagram(datagramText);
                }
            }
        }

        #endregion

        #region  private methods

        private void Close()
        {
            if (m_serverClosed)
            {
                return;
            }

            m_serverClosed = true;
            m_serverListenPaused = true;

            m_checkServerListenResetEvent.WaitOne();  // 等待3个线程
            m_checkSessionTableResetEvent.WaitOne();
            m_checkDatagramQueueResetEvent.WaitOne();

            if (m_databaseObj != null)
            {
                m_databaseObj.Close();
            }

            if (m_sessionTable != null)
            {
                lock (m_sessionTable)
                {
                    foreach (TSession session in m_sessionTable.Values)
                    {
                        session.Close();
                    }
                }
            }

            this.CloseServerSocket();

            if (m_sessionTable != null)  // 清空会话列表
            {
                lock (m_sessionTable)
                {
                    m_sessionTable.Clear();
                }
            }

            this.OnServerClosed();
        }

        private void ClearCountValues()
        {
            m_sessionCount = 0;
            m_receivedDatagramCount = 0;
            m_errorDatagramCount = 0;
            m_datagramQueueLength = 0;

            m_databaseExceptionCount = 0;
            m_serverExceptCount = 0;
            m_sessionExceptionCount = 0;
        }

        private bool CreateServerSocket()
        {
            try
            {
                m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_serverSocket.Bind(new IPEndPoint(IPAddress.Any, m_servertPort));
                m_serverSocket.Listen(m_maxListenQueueLength);
                return true;
            }
            catch (Exception err)
            {
                this.OnServerException(err);
                return false;
            }
        }

        private bool CheckSocketIP(Socket clientSocket)
        {
            IPEndPoint iep = (IPEndPoint)clientSocket.RemoteEndPoint;
            string ip = iep.Address.ToString();

            if (ip.Substring(0, 7) == "127.0.0")   // local machine
            {
                return true;
            }

            lock (m_sessionTable)
            {
                int sameIPCount = 0;
                foreach (TSession session in m_sessionTable.Values)
                {
                    if (session.IP == ip)
                    {
                        sameIPCount++;
                        if (sameIPCount > m_maxSameIPCount)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 侦听客户端连接请求
        /// </summary>
        private void StartServerListen(object state)
        {
            m_checkServerListenResetEvent.Reset();
            Socket clientSocket = null;

            while (!m_serverClosed)
            {
                if (m_serverListenPaused)  // pause server
                {
                    this.CloseServerSocket();
                    Thread.Sleep(m_loopWaitTime);
                    continue;
                }

                if (m_serverSocket == null)
                {
                    this.CreateServerSocket();
                    continue;
                }

                try
                {
                    if (m_serverSocket.Poll(m_loopWaitTime, SelectMode.SelectRead))
                    {
                        // 频繁关闭、启动时，这里容易产生错误（提示套接字只能有一个）
                        clientSocket = m_serverSocket.Accept();

                        if (clientSocket != null && clientSocket.Connected)
                        {
                            if (m_sessionCount >= m_maxSessionTableLength || !this.CheckSocketIP(clientSocket))  // 当前列表已经存在该 IP 地址
                            {
                                this.OnSessionRejected(); // 拒绝登录请求
                                this.CloseClientSocket(clientSocket);
                            }
                            else
                            {
                                this.AddSession(clientSocket);  // 添加到队列中, 并调用异步接收方法
                            }
                        }
                        else  // clientSocket is null or connected == false
                        {
                            this.CloseClientSocket(clientSocket);
                        }
                    }
                }
                catch (Exception)  // 侦听连接的异常频繁, 不捕获异常
                {
                    this.CloseClientSocket(clientSocket);
                }
            }

            m_checkServerListenResetEvent.Set();
        }

        private void CloseServerSocket()
        {
            if (m_serverSocket == null)
            {
                return;
            }

            try
            {
                lock (m_sessionTable)
                {
                    if (m_sessionTable != null && m_sessionTable.Count > 0)
                    {
                        // 可能结束服务器端的 AcceptClientConnect 的 Poll
                        //                        m_serverSocket.Shutdown(SocketShutdown.Both);  // 有连接才关
                    }
                }
                m_serverSocket.Close();
            }
            catch (Exception err)
            {
                this.OnServerException(err);
            }
            finally
            {
                m_serverSocket = null;
            }
        }

        /// <summary>
        /// 强制关闭客户端请求时的 Socket
        /// </summary>
        private void CloseClientSocket(Socket socket)
        {
            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception) { }  // 强制关闭, 忽略错误
            }
        }

        /// <summary>
        /// 增加一个会话对象
        /// </summary>
        private void AddSession(Socket clientSocket)
        {
            Interlocked.Increment(ref m_sessionSequenceNo);

            TSession session = new TSession();
            session.Initiate(m_maxReceiveBufferSize, m_maxDatagramSize, m_sessionSequenceNo, clientSocket, m_databaseObj);

            session.DatagramDelimiterError += new EventHandler<TSessionEventArgs>(this.OnDatagramDelimiterError);
            session.DatagramOversizeError += new EventHandler<TSessionEventArgs>(this.OnDatagramOversizeError);
            session.DatagramError += new EventHandler<TSessionEventArgs>(this.OnDatagramError);
            session.DatagramAccepted += new EventHandler<TSessionEventArgs>(this.OnDatagramAccepted);
            session.DatagramHandled += new EventHandler<TSessionEventArgs>(this.OnDatagramHandled);
            session.SessionReceiveException += new EventHandler<TSessionExceptionEventArgs>(this.OnSessionReceiveException);
            session.SessionSendException += new EventHandler<TSessionExceptionEventArgs>(this.OnSessionSendException);

            session.ShowDebugMessage += new EventHandler<TExceptionEventArgs>(this.ShowDebugMessage);

            lock (m_sessionTable)
            {
                m_sessionTable.Add(session.ID, session);
            }
            session.BeginReceiveDatagram();

            this.OnSessionConnected(session);
        }

        /// <summary>
        /// 资源清理线程, 分若干步完成
        /// </summary>
        private void CheckSessionTable(object state)
        {
            m_checkSessionTableResetEvent.Reset();

            while (!m_serverClosed)
            {
                lock (m_sessionTable)
                {
                    List<int> sessionIDList = new List<int>();

                    foreach (TSession session in m_sessionTable.Values)
                    {
                        if (m_serverClosed)
                        {
                            break;
                        }

                        if (session.State == TSessionState.Inactive)  // 分三步清除一个 Session
                        {
                            session.Shutdown();  // 第一步: shutdown, 结束异步事件
                        }
                        else if (session.State == TSessionState.Shutdown)
                        {
                            session.Close();  // 第二步: Close
                        }
                        else if (session.State == TSessionState.Closed)
                        {
                            sessionIDList.Add(session.ID);
                            this.DisconnectSession(session);

                        }
                        else // 正常的会话 Active
                        {
                            if (this.IsSessionTimeout(session))  // 超时，则准备断开连接
                            {
                                session.DisconnectType = TDisconnectType.Timeout;
                                session.SetInactive();  // 标记为将关闭、准备断开
                            }
                        }
                    }

                    foreach (int id in sessionIDList)  // 统一清除
                    {
                        m_sessionTable.Remove(id);
                    }

                    sessionIDList.Clear();
                }
            }

            m_checkSessionTableResetEvent.Set();
        }

        private bool IsSessionTimeout(TSession session)
        {
            if (session.SessionTimeInterval > m_maxSessionTimeout)  // 超时，则准备断开连接
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 数据包处理线程
        /// </summary>
        private void CheckDatagramQueue(object state)
        {
            m_checkDatagramQueueResetEvent.Reset();

            while (!m_serverClosed)
            {
                lock (m_sessionTable)
                {
                    foreach (TSession session in m_sessionTable.Values)
                    {
                        if (m_serverClosed)
                        {
                            break;
                        }
                        session.HandleDatagram();
                    }
                }
            }//end while

            m_checkDatagramQueueResetEvent.Set();
        }

        private void DisconnectSession(TSession session)
        {
            if (session.DisconnectType == TDisconnectType.Normal)
            {
                this.OnSessionDisconnected(session);
            }
            else if (session.DisconnectType == TDisconnectType.Timeout)
            {
                this.OnSessionTimeout(session);
            }
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        private void OnShowDebugMessage(string message)
        {
            if (this.ShowDebugMessage != null)
            {
                TExceptionEventArgs e = new TExceptionEventArgs(message);
                this.ShowDebugMessage(this, e);
            }
        }

        #endregion

        #region  protected virtual methods

        protected virtual void OnDatabaseOpenException(object sender, TExceptionEventArgs e)
        {
            Interlocked.Increment(ref m_databaseExceptionCount);

            EventHandler<TExceptionEventArgs> handler = this.DatabaseOpenException;
            if (handler != null)
            {
                handler(sender, e);  // 转发事件的激发者
            }
        }

        protected virtual void OnDatabaseCloseException(object sender, TExceptionEventArgs e)
        {
            Interlocked.Increment(ref m_databaseExceptionCount);

            EventHandler<TExceptionEventArgs> handler = this.DatabaseCloseExcpetion;
            if (handler != null)
            {
                handler(sender, e);  // 转发事件的激发者
            }
        }

        protected virtual void OnDatabaseException(object sender, TExceptionEventArgs e)
        {
            Interlocked.Increment(ref m_databaseExceptionCount);

            EventHandler<TExceptionEventArgs> handler = this.DatabaseExcpetion;
            if (handler != null)
            {
                handler(sender, e);  // 转发事件的激发者
            }
        }

        protected virtual void OnSessionRejected()
        {
            EventHandler handler = this.SessionRejected;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnSessionConnected(TSession session)
        {
            Interlocked.Increment(ref m_sessionCount);

            EventHandler<TSessionEventArgs> handler = this.SessionConnected;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(session);
                handler(this, e);
            }
        }

        protected virtual void OnSessionDisconnected(TSession session)
        {
            Interlocked.Decrement(ref m_sessionCount);

            EventHandler<TSessionEventArgs> handler = this.SessionDisconnected;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(session);
                handler(this, e);
            }
        }

        protected virtual void OnSessionTimeout(TSession session)
        {
            Interlocked.Decrement(ref m_sessionCount);

            EventHandler<TSessionEventArgs> handler = this.SessionTimeout;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(session);
                handler(this, e);
            }
        }

        protected virtual void OnSessionReceiveException(object sender, TSessionExceptionEventArgs e)
        {
            Interlocked.Decrement(ref m_sessionCount);
            Interlocked.Increment(ref m_sessionExceptionCount);

            EventHandler<TSessionExceptionEventArgs> handler = this.SessionReceiveException;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSessionSendException(object sender, TSessionExceptionEventArgs e)
        {
            Interlocked.Decrement(ref m_sessionCount);
            Interlocked.Increment(ref m_sessionExceptionCount);

            EventHandler<TSessionExceptionEventArgs> handler = this.SessionSendException;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnServerException(Exception err)
        {
            Interlocked.Increment(ref m_serverExceptCount);

            EventHandler<TExceptionEventArgs> handler = this.ServerException;
            if (handler != null)
            {
                TExceptionEventArgs e = new TExceptionEventArgs(err);
                handler(this, e);
            }
        }

        protected virtual void OnServerStarted()
        {
            EventHandler handler = this.ServerStarted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnServerListenPaused()
        {
            EventHandler handler = this.ServerListenPaused;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnServerListenResumed()
        {
            EventHandler handler = this.ServerListenResumed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnServerClosed()
        {
            EventHandler handler = this.ServerClosed;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        protected virtual void OnDatagramDelimiterError(object sender, TSessionEventArgs e)
        {
            Interlocked.Increment(ref m_receivedDatagramCount);
            Interlocked.Increment(ref m_errorDatagramCount);

            EventHandler<TSessionEventArgs> handler = this.DatagramDelimiterError;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDatagramOversizeError(object sender, TSessionEventArgs e)
        {
            Interlocked.Increment(ref m_receivedDatagramCount);
            Interlocked.Increment(ref m_errorDatagramCount);

            EventHandler<TSessionEventArgs> handler = this.DatagramOversizeError;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDatagramAccepted(object sender, TSessionEventArgs e)
        {
            Interlocked.Increment(ref m_receivedDatagramCount);
            Interlocked.Increment(ref m_datagramQueueLength);

            EventHandler<TSessionEventArgs> handler = this.DatagramAccepted;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDatagramError(object sender, TSessionEventArgs e)
        {
            Interlocked.Increment(ref m_errorDatagramCount);
            Interlocked.Decrement(ref m_datagramQueueLength);

            EventHandler<TSessionEventArgs> handler = this.DatagramError;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnDatagramHandled(object sender, TSessionEventArgs e)
        {
            Interlocked.Decrement(ref m_datagramQueueLength);

            EventHandler<TSessionEventArgs> handler = this.DatagramHandled;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

    }
}
