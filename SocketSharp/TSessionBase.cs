using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace CSUST
{
    /// <summary>
    /// 会话基类(抽象类, 必须实现其 AnalyzeDatagram 方法)
    /// </summary>
    public abstract class TSessionBase : TSessionCoreInfo
    {
        #region  member fields

        private Socket m_socket;
        private int m_maxDatagramSize;
        /// <summary>
        /// 接收客户端Socket数据的缓冲区，如果数据包比该缓冲区长，Socket将自动（异步）读取几次
        /// 每次用方法CopyToDatagramBuffer暂到数据包缓冲区m_datagramBuffer中
        /// </summary>
        private byte[] m_receiveBuffer;
        /// <summary>
        /// 如果m_receiveBuffer接收了非完整的数据包，则使用该缓冲区暂存，直到获得一个完整数据包
        /// </summary>
        private byte[] m_datagramBuffer;

        protected TDatabaseBase m_databaseObj;
        private Queue<byte[]> m_datagramQueue;

        #endregion
        #region 属性
        public TSessionState TSessionState
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public TDisconnectType TDisconnectType
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }
        #endregion 属性
        #region  构造器
        /// <summary>
        /// 作泛型参数类型时, 必须有无参构造函数
        /// </summary>
        public TSessionBase() { }

        /// <summary>
        /// 替构造函数初始化对象
        /// </summary>
        public virtual void Initiate(int maxReceiveBufferSize, int maxDatagramsize, int id, Socket socket, TDatabaseBase database)
        {
            m_maxDatagramSize = maxDatagramsize;
            m_id = id;

            m_socket = socket;
            m_loginTime = DateTime.Now;
            m_lastSessionTime = m_loginTime;
            m_databaseObj = database;

            m_receiveBuffer = new byte[maxReceiveBufferSize];  // 数据接收缓冲区
            m_datagramQueue = new Queue<byte[]>();

            if (m_socket != null)
            {
                IPEndPoint iep = m_socket.RemoteEndPoint as IPEndPoint;
                if (iep != null)
                {
                    m_ip = iep.Address.ToString();
                }
            }
        }

        #endregion
        #region class events

        public event EventHandler<TSessionExceptionEventArgs> SessionReceiveException;
        public event EventHandler<TSessionExceptionEventArgs> SessionSendException;
        public event EventHandler<TSessionEventArgs> DatagramDelimiterError;
        public event EventHandler<TSessionEventArgs> DatagramOversizeError;
        public event EventHandler<TSessionEventArgs> DatagramAccepted;
        public event EventHandler<TSessionEventArgs> DatagramError;
        public event EventHandler<TSessionEventArgs> DatagramHandled;

        public event EventHandler<TExceptionEventArgs> ShowDebugMessage;

        #endregion

      

        #region  public methods

        public void Shutdown()
        {
            lock (this)
            {
                if (m_state != TSessionState.Inactive || m_socket == null)  // Inactive 状态才能 Shutdown
                {
                    return;
                }

                m_state = TSessionState.Shutdown;
                try
                {
                    m_socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception) { }
            }
        }

        public void Close()
        {
            lock (this)
            {
                if (m_state != TSessionState.Shutdown || m_socket == null)  // Shutdown 状态才能 Close
                {
                    return;
                }

                m_receiveBuffer = null;
                m_datagramBuffer = null;

                if (m_datagramQueue != null)
                {
                    while (m_datagramQueue.Count > 0)
                    {

                        byte[] datagramBytes = m_datagramQueue.Dequeue();
                        datagramBytes = null;
                    }
                    m_datagramQueue.Clear();
                }

                try
                {
                    m_state = TSessionState.Closed;
                    m_socket.Close();
                }
                catch (Exception) { }
            }
        }

        public void SetInactive()
        {
            lock (this)
            {
                if (m_state == TSessionState.Active)
                {
                    m_state = TSessionState.Inactive;
                    m_disconnectType = TDisconnectType.Normal;
                }
            }
        }

        public void HandleDatagram()
        {
            lock (this)
            {
                if (m_state != TSessionState.Active || m_datagramQueue.Count == 0)
                {
                    return;
                }

                byte[] datagramBytes = m_datagramQueue.Dequeue();
                this.AnalyzeDatagram(datagramBytes);
            }
        }

        public void BeginReceiveDatagram()
        {
            lock (this)
            {
                if (m_state != TSessionState.Active)
                {
                    return;
                }

                try  // 一个客户端连续做连接 或连接后立即断开，容易在该处产生错误，系统不认为是错误
                {
                    // 开始接受来自该客户端的数据
                    m_socket.BeginReceive(m_receiveBuffer, 0, m_receiveBuffer.Length, SocketFlags.None, this.EndReceiveDatagram, this);

                }
                catch (Exception err)  // 读 Socket 异常，准备关闭该会话
                {
                    m_disconnectType = TDisconnectType.Exception;
                    m_state = TSessionState.Inactive;

                    this.OnSessionReceiveException(err);
                }
            }
        }

        public void SendDatagram(string datagramText)
        {
            lock (this)
            {
                if (m_state != TSessionState.Active)
                {
                    return;
                }

                try
                {
                    byte[] data = Encoding.ASCII.GetBytes(datagramText);  // 获得数据字节数组
                    m_socket.BeginSend(data, 0, data.Length, SocketFlags.None, this.EndSendDatagram, this);
                }
                catch (Exception err)  // 写 socket 异常，准备关闭该会话
                {
                    m_disconnectType = TDisconnectType.Exception;
                    m_state = TSessionState.Inactive;

                    this.OnSessionSendException(err);
                }
            }
        }

        #endregion

        #region  private methods

        /// <summary>
        /// 发送数据完成处理函数, iar 为目标客户端 Session
        /// </summary>
        private void EndSendDatagram(IAsyncResult iar)
        {
            lock (this)
            {
                if (m_state != TSessionState.Active)
                {
                    return;
                }

                try
                {
                    int sent = m_socket.EndSend(iar);
                }
                catch (Exception err)  // 写 socket 异常，准备关闭该会话
                {
                    m_disconnectType = TDisconnectType.Exception;
                    m_state = TSessionState.Inactive;

                    this.OnSessionSendException(err);
                }
            }
        }

        private void EndReceiveDatagram(IAsyncResult iar)
        {
            lock (this)
            {
                if (m_state != TSessionState.Active)
                {
                    return;
                }

                try
                {
                    // Shutdown 时将调用 ReceiveData，此时也可能收到 0 长数据包
                    int readBytesLength = m_socket.EndReceive(iar);
                    if (readBytesLength == 0)
                    {
                        m_disconnectType = TDisconnectType.Normal;
                        m_state = TSessionState.Inactive;
                    }
                    else  // 正常数据包
                    {
                        m_lastSessionTime = DateTime.Now;
                        // 合并报文，按报文头、尾字符标志抽取报文，将包交给数据处理器
                        this.ResolveSessionBuffer(readBytesLength);
                        //Tips:本次数据处理完，然后才能再开始接收下一个数据包
                        this.BeginReceiveDatagram();  // 继续接收
                    }
                }
                catch (Exception err)  // 读 socket 异常，关闭该会话，系统不认为是错误（这种错误可能太多）
                {
                    if (m_state == TSessionState.Active)
                    {
                        m_disconnectType = TDisconnectType.Exception;
                        m_state = TSessionState.Inactive;

                        this.OnSessionReceiveException(err);
                    }
                }
            }
        }

        /// <summary>
        /// 拷贝接收缓冲区的数据到数据缓冲区（即多次读一个包文）
        /// </summary>
        private void CopyToDatagramBuffer(int start, int length)
        {
            int datagramLength = 0;
            if (m_datagramBuffer != null)
            {
                datagramLength = m_datagramBuffer.Length;
            }

            Array.Resize(ref m_datagramBuffer, datagramLength + length);  // 调整长度（m_datagramBuffer 为 null 不出错）
            Array.Copy(m_receiveBuffer, start, m_datagramBuffer, datagramLength, length);  // 拷贝到数据包缓冲区
        }

        #endregion

        #region protected methods

        /// <summary>
        /// 数据包解析方法: 
        /// 提取包时与包规则紧密相关，根据实际规则重定义
        /// </summary>
        protected virtual void ResolveSessionBuffer(int readBytesLength)
        {
            // 上次留下包文非空, 必然含开始字符<
            bool hasHeadDelimiter = (m_datagramBuffer != null);
            int headDelimiter = 1;
            int tailDelimiter = 1;

            int start = 0;   // m_receiveBuffer 缓冲区中包开始位置
            int length = 0;  // 已经搜索的接收缓冲区长度

            int subIndex = 0;  // 缓冲区下标
            while (subIndex < readBytesLength)
            {
                if (m_receiveBuffer[subIndex] == '<')  // 数据包开始字符<，前面包文作废
                {
                    if (hasHeadDelimiter || length > 0)  // 如果 < 前面有数据，则认为错误包
                    {
                        this.OnDatagramDelimiterError();
                    }

                    m_datagramBuffer = null;  // 清空包缓冲区，开始一个新的包

                    start = subIndex;         // 新包起点，即<所在位置
                    length = headDelimiter;   // 新包的长度（即<）
                    hasHeadDelimiter = true;  // 新包有开始字符
                }
                else if (m_receiveBuffer[subIndex] == '>')  // 数据包的结束字符>
                {
                    if (hasHeadDelimiter)  // 两个缓冲区中有开始字符<
                    {
                        length += tailDelimiter;  // 长度包括结束字符“>”

                        this.GetDatagramFromBuffer(start, length); // >前面的为正确格式的包

                        start = subIndex + tailDelimiter;  // 新包起点（一般一次处理将结束循环）
                        length = 0;  // 新包长度
                    }
                    else  // >前面没有开始字符，此时认为结束字符>为一般字符，待后续的错误包处理
                    {
                        length++;  //  hasHeadDelimiter = false;
                    }
                }
                else  // 即非 < 也非 >， 是一般字符，长度 + 1
                {
                    length++;
                }
                ++subIndex;
            }

            if (length > 0)  // 剩下的待处理串，分两种情况
            {
                int mergedLength = length;
                if (m_datagramBuffer != null)
                {
                    mergedLength += m_datagramBuffer.Length;
                }

                // 剩下的包文含首字符且不超长，转存到包文缓冲区中，待下次处理
                if (hasHeadDelimiter && mergedLength <= m_maxDatagramSize)
                {
                    this.CopyToDatagramBuffer(start, length);
                }
                else  // 不含首字符或超长
                {
                    this.OnDatagramOversizeError();
                    m_datagramBuffer = null;  // 丢弃全部数据
                }
            }
        }

        /// <summary>
        /// 数据包分析方法: 
        /// Session重写入口, 基本功能: 
        /// 1) 判断包有效性与包类型(注意：包带起止符号); 
        /// 2) 分解包中的各字段数据
        /// 3) 校验包及其数据有效性
        /// 4) 发送确认消息给客户端(调用方法 SendDatagram())
        /// 5) 存储包数据到数据库中
        /// 6) 存储包原文到数据库中(可选)
        /// 7) 补充字段m_name, 表示数据包发送者的名称/编号
        /// 8) 其它相关方法
        /// </summary>
        protected abstract void AnalyzeDatagram(byte[] datagramBytes);

        protected virtual void GetDatagramFromBuffer(int startPos, int len)
        {
            byte[] datagramBytes;
            if (m_datagramBuffer != null)
            {
                datagramBytes = new byte[len + m_datagramBuffer.Length];
                Array.Copy(m_datagramBuffer, 0, datagramBytes, 0, m_datagramBuffer.Length);  // 先拷贝 Session 的数据缓冲区的数据
                Array.Copy(m_receiveBuffer, startPos, datagramBytes, m_datagramBuffer.Length, len);  // 再拷贝 Session 的接收缓冲区的数据
            }
            else
            {
                datagramBytes = new byte[len];
                Array.Copy(m_receiveBuffer, startPos, datagramBytes, 0, len);  // 再拷贝 Session 的接收缓冲区的数据
            }

            if (m_datagramBuffer != null)
            {
                m_datagramBuffer = null;
            }

            m_datagramQueue.Enqueue(datagramBytes);
        }

        protected virtual void OnDatagramDelimiterError()
        {
            EventHandler<TSessionEventArgs> handler = this.DatagramDelimiterError;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(this);
                handler(this, e);
            }
        }

        protected virtual void OnDatagramOversizeError()
        {
            EventHandler<TSessionEventArgs> handler = this.DatagramOversizeError;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(this);
                handler(this, e);
            }
        }

        protected virtual void OnDatagramAccepted()
        {
            EventHandler<TSessionEventArgs> handler = this.DatagramAccepted;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(this);
                handler(this, e);
            }
        }

        protected virtual void OnDatagramError()
        {
            EventHandler<TSessionEventArgs> handler = this.DatagramError;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(this);
                handler(this, e);
            }
        }

        protected virtual void OnDatagramHandled()
        {
            EventHandler<TSessionEventArgs> handler = this.DatagramHandled;
            if (handler != null)
            {
                TSessionEventArgs e = new TSessionEventArgs(this);
                handler(this, e);
            }
        }

        protected virtual void OnSessionReceiveException(Exception err)
        {
            EventHandler<TSessionExceptionEventArgs> handler = this.SessionReceiveException;
            if (handler != null)
            {
                TSessionExceptionEventArgs e = new TSessionExceptionEventArgs(err, this);
                handler(this, e);
            }
        }

        protected virtual void OnSessionSendException(Exception err)
        {
            EventHandler<TSessionExceptionEventArgs> handler = this.SessionSendException;
            if (handler != null)
            {
                TSessionExceptionEventArgs e = new TSessionExceptionEventArgs(err, this);
                handler(this, e);
            }
        }

        protected void OnShowDebugMessage(string message)
        {
            if (this.ShowDebugMessage != null)
            {
                TExceptionEventArgs e = new TExceptionEventArgs(message);
                this.ShowDebugMessage(this, e);
            }
        }
        #endregion
    }
}
