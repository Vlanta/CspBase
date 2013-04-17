
namespace CSUST
{
    public enum TDisconnectType
    {
        Normal,     // disconnect normally
        Timeout,    // disconnect because of timeout
        Exception   // disconnect because of exception
    }

    public enum TSessionState
    {
        Active,    // state is active会话是有效的
        Inactive,  // session is inactive and will be closed会话将被清理
        Shutdown,  // session is shutdownling会话Socket正在卸载
        Closed     // session is closed表示会话已经关闭、资源已经清理
    }
}
