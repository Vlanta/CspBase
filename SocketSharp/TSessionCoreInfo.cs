using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSUST
{
    /// <summary>
    /// 会话类核心成员
    /// </summary>
    public class TSessionCoreInfo
    {
        #region  member fields

        protected int m_id;
        protected string m_ip = string.Empty;
        protected string m_name = string.Empty;
        protected TSessionState m_state = TSessionState.Active;
        protected TDisconnectType m_disconnectType;

        protected DateTime m_loginTime;
        protected DateTime m_lastSessionTime;

        #endregion

        #region  public properties

        public int ID
        {
            get { return m_id; }
        }

        public string IP
        {
            get { return m_ip; }
        }

        /// <summary>
        /// 数据包发送者的名称/编号
        /// </summary>
        public string Name
        {
            get { return m_name; }
        }

        public DateTime LoginTime
        {
            get { return m_loginTime; }
        }

        public DateTime LastSessionTime
        {
            get { return m_lastSessionTime; }
        }

        public int SessionTimeInterval
        {
            get
            {
                TimeSpan ts = DateTime.Now.Subtract(m_lastSessionTime);
                return Math.Abs((int)ts.TotalSeconds);
            }
        }

        public TSessionState State
        {
            get { return m_state; }
        }

        public TDisconnectType DisconnectType
        {
            get { return m_disconnectType; }
            set
            {
                lock (this)
                {
                    m_disconnectType = value;
                }
            }
        }

        #endregion
    }
}
