using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketClientCommonLib
{
    /// <summary>
    /// Socket UDP-UDP远程连接P2P,不做转发,不做大量数据接收(只做NAT穿透服务器)
    /// </summary>
    class AsyncUDPClient
    {
        /// <summary>
        /// Socket字节套
        /// </summary>
        public Socket m_sListen;
        /// <summary>
        /// 服务器主机地址
        /// </summary>
        private IPEndPoint hostPoint;
        /// <summary>
        /// 目标主机地址
        /// </summary>
        private EndPoint remotePoint;
        /// <summary>
        /// 缓存地址
        /// </summary>
        public byte[] BufferData;

        /// <summary>
        /// 启动客户端
        /// </summary>
        /// <param name="ServerIp">服务器IP</param>
        /// <param name="Prot">服务器端口</param>
        public AsyncUDPClient(string ServerIp, int Prot)
        {
            BufferData = new byte[1024 * 20];
            hostPoint = new IPEndPoint(IPAddress.Parse(ServerIp), Prot);
            m_sListen = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            remotePoint = (EndPoint)sender;
        }

        /// <summary>
        /// 监控
        /// </summary>
        public void StartAccept()
        {
            m_sListen.ReceiveFrom(BufferData, ref remotePoint);
            DelegateState.ServerStateInfo(Encoding.UTF8.GetString(BufferData));
            StartAccept();
        }

        /// <summary>
        /// 往服务器发送信息
        /// </summary>
        /// <param name="CommandText"></param>
        public void SendServerMsg(string CommandText)
        {
            m_sListen.SendTo(Encoding.UTF8.GetBytes(CommandText), hostPoint);
        }

        /// <summary>
        /// 往目标发送信息
        /// </summary>
        /// <param name="CommandText"></param>
        public void SendRemoteMsg(string CommandText)
        {
            m_sListen.SendTo(Encoding.UTF8.GetBytes(CommandText), remotePoint);
        }
    }
}
