using CCWin;
using CCWin.SkinControl;
using System;
using System.Windows.Forms;
using SocketServerCommonLib;
using System.Threading;
using System.Drawing;
using System.IO;
using HelpCommonLib;
using System.Collections.Generic;

namespace SocketServer
{
    public partial class TCPUDPServer : CCSkinMain
    {
        AsyncSocketServer TcpServer;

        AsyncUDPServer UdpServer;

        /// <summary>
        /// �û����Ӷ��ٴ�
        /// </summary>
        int TCPUserCount = 0;
        int TCPDeviceCount = 0;
        //�豸����ʵʱ���漯��
        List<string> ConnectAry;
        public TCPUDPServer()
        {
            InitializeComponent();
            DelegateState.ServerStateInfo = ServerShowStateInfo;
            DelegateState.TeartbeatServerStateInfo = TeartbeatShowStateInfo;
            DelegateState.AddTCPuserStateInfo = AddTCPuser;
            DelegateState.AddTCPdeviceStateInfo = AddTCPdevice;
            DelegateState.ReomveTCPStateInfo = ReomveTCP;
            DelegateState.ServerConnStateInfo = ConnStateInfo;
        }

        #region  AmosLi produce <��������ģ��>

        /// <summary>
        /// ����TCP����
        /// </summary>
        private void btnTCP_Click(object sender, EventArgs e)
        {
            btnTCP.Enabled = false;
            if (TcpServer == null)
                TcpServer = new AsyncSocketServer();
            if (!TcpServer.IsStartListening)
            {
                TcpServer.Start();
                txtMsg.AppendText(DateTime.Now + Environment.NewLine + "TCP����������" + Environment.NewLine);
                lblTCP.Text = "TCP��������ַ:" + TcpServer.serverconfig.ListenIp + ":" + TcpServer.serverconfig.ListenPort;
                PicBoxTCP.BackgroundImage = Properties.Resources._07822_48x48x8BPP_;
                btnTCP.Text = "TCPֹͣ����";
            }
            else
            {
                TcpServer.Stop();
                PicBoxTCP.BackgroundImage = Properties.Resources._07821_48x48x8BPP_;
                txtMsg.AppendText(DateTime.Now + Environment.NewLine + "TCP������ֹͣ" + Environment.NewLine);
                btnTCP.Text = "TCP����������";
            }
            btnTCP.Enabled = true;
        }


        /// <summary>
        /// UDP����
        /// </summary>
        private void btnUDP_Click(object sender, EventArgs e)
        {
            btnUDP.Enabled = false;
            if (UdpServer == null)
                UdpServer = new AsyncUDPServer();

            if (!UdpServer.IsStartListening)
            {
                UdpServer.Start();
                txtMsg.AppendText(DateTime.Now + Environment.NewLine + "UDP����������" + Environment.NewLine);
                lblUDP.Text = "UDP��������ַ:" + HelpCommonLib.NetworkAddress.GetIPAddress() + ":" + UdpServer.ListenProt;
                PicBoxUDP.BackgroundImage = Properties.Resources._07822_48x48x8BPP_;
                btnUDP.Text = "UDPֹͣ����";
                btnUDP.Enabled = true;
            }
            else
            {
                txtMsg.AppendText(DateTime.Now + Environment.NewLine + "UDP������ֹͣ" + Environment.NewLine);
                UdpServer.Close();
                PicBoxUDP.BackgroundImage = Properties.Resources._07821_48x48x8BPP_;
                btnUDP.Text = "UDP�˿ڴ���";
            }
        }
        #region

        #region TCP�ص���������
        void AddTCPuser(SocketUserToken userToken)
        {
            this.Invoke(new ThreadStart(delegate
            {
                if (userToken.ConnectSocket == null)
                    return;
                tpe3list1.Refresh();
                TCPUserCount++;
                ListViewItem lvi = new ListViewItem();
                lvi.Text = TCPUserCount.ToString();
                lvi.SubItems.Add(userToken.ConnectSocket.RemoteEndPoint.ToString());
                lvi.SubItems.Add(userToken.UserName);
                lvi.SubItems.Add(userToken.ConnectDateTime.ToString());
                lvi.SubItems.Add("TCP");
                tpe3list1.Items.Add(lvi);
            }));
        }

        /// <summary>
        /// ɾ���û������豸
        /// </summary>
        /// <param name="userToken"></param>
        void ReomveTCP(SocketUserToken userToken)
        {
            this.Invoke(new ThreadStart(delegate
            {
                try
                {
                    if (userToken.isDevice)
                    {
                        for (int i = 0; i < tpe2list1.Items.Count; i++)
                        {
                            if (tpe2list1.Items[i].SubItems[1].Text.Contains(userToken.RemotDeviceIp) && userToken.UserName == tpe2list1.Items[i].SubItems[2].Text)
                            {
                                tpe2list1.Items.Remove(tpe2list1.Items[i]);
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < tpe3list1.Items.Count; i++)
                        {
                            if (tpe3list1.Items[i].SubItems[1].Text.Contains(userToken.RemotDeviceIp) && userToken.UserName == tpe3list1.Items[i].SubItems[2].Text)
                            {
                                tpe3list1.Items.Remove(tpe3list1.Items[i]);
                                break;
                            }
                        }
                    }
                }
                catch { }
            }));
        }

        void AddTCPdevice(SocketUserToken userToken)
        {
            this.Invoke(new ThreadStart(delegate
            {
                if (userToken.ConnectSocket == null)
                    return;

                tpe2list1.Refresh();
                TCPDeviceCount++;
                ListViewItem lvi = new ListViewItem();
                lvi.Text = TCPDeviceCount.ToString();
                lvi.SubItems.Add(userToken.ConnectSocket.RemoteEndPoint.ToString() + "-" + userToken.RemotDeviceIp);
                lvi.SubItems.Add(userToken.UserName);
                lvi.SubItems.Add(userToken.ConnectDateTime.ToString());
                lvi.SubItems.Add(userToken.firmware);
                tpe2list1.Items.Add(lvi);
            }));
        }
        #endregion

        void ConnStateInfo(string RemoteIp, string TCPUDP)
        {
            this.Invoke(new ThreadStart(delegate
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = listAllView.Items.Count.ToString();
                lvi.SubItems.Add(RemoteIp);
                lvi.SubItems.Add(DateTime.Now.ToString());
                lvi.SubItems.Add(TCPUDP);
                listAllView.Items.Add(lvi);
            }));
        }


        /// <summary>
        /// ��Ϣ���
        /// </summary>
        /// <param name="msg"></param>
        void ServerShowStateInfo(string msg)
        {
            this.Invoke(new ThreadStart(delegate
            {
                tpe2txtMsg.AppendText(DateTime.Now + ":" + msg + Environment.NewLine);
            }));
        }

        /// <summary>
        /// ����ʱ��
        /// </summary>
        void TeartbeatShowStateInfo(int num)
        {
            this.Invoke(new ThreadStart(delegate
            {
                txtMsg.AppendText(Environment.NewLine + DateTime.Now + ":" + num + "���Ӽ��");
                lblNum1.NormlBack = ImageListAllUpdate(num / 10 % 10);
                lblNum2.NormlBack = ImageListAllUpdate(num % 10);
                if (listAllView.Items.Count > 1000 || tpe2txtMsg.Text.Length > 10000)
                {
                    LogHelper.WriteLog(tpe2txtMsg.Text);
                    listAllView.Items.Clear();
                    tpe2txtMsg.Clear();
                    txtMsg.Clear();
                    linkOut_LinkClicked(null, null);
                }
            }));
        }
        #endregion
        /// <summary>
        /// ͼƬ����
        /// </summary>
        Image ImageListAllUpdate(int Num)
        {
            switch (Num)
            {
                case 0:
                    return Properties.Resources._00034_17x25x8BPP_;
                case 1:
                    return Properties.Resources._00035_17x25x8BPP_;
                case 2:
                    return Properties.Resources._00036_17x25x8BPP_;
                case 3:
                    return Properties.Resources._00037_17x25x8BPP_;
                case 4:
                    return Properties.Resources._00038_17x25x8BPP_;
                case 5:
                    return Properties.Resources._00039_17x25x8BPP_;
                case 6:
                    return Properties.Resources._00040_17x25x8BPP_;
                case 7:
                    return Properties.Resources._00041_17x25x8BPP_;
                case 8:
                    return Properties.Resources._00042_17x25x8BPP_;
                case 9:
                    return Properties.Resources._00043_17x25x8BPP_;
                default:
                    return null;
            }
        }
        #endregion
        /// <summary>
        /// ˢ���豸�б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkdeviceRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (TcpServer == null)
                return;

            linkdeviceRefresh.Enabled = false;
            GC.Collect();
            GC.Collect();
            lbldevice.Text = "�ϴ�ˢ������ " + DateTime.Now.Hour + " �㣬������ " + TcpServer.AsyncSocketDeviceList.Devicelist.Count + " ���豸��";
            linkdeviceRefresh.Enabled = true;
        }

        private void linkuserRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (TcpServer == null)
                return;

            linkuserRefresh.Enabled = false;
            GC.Collect();
            GC.Collect();
            lbluser.Text = "�ϴ�ˢ������ " + DateTime.Now.Hour + " �㣬������ " + TcpServer.AsyncSocketUserList.Userlist.Count + "/" + TcpServer.serverconfig.numConnections + " ���˿ڡ�";
            linkuserRefresh.Enabled = true;
        }
        /// <summary>
        /// ������Ϣ
        /// </summary>
        private void linkSaveMsg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter write = new StreamWriter(saveFileDialog1.FileName))
                {
                    write.WriteLine(tpe2txtMsg.Text);
                }
            }
        }

        /// <summary>
        /// �������ǽ
        /// </summary>
        private void btnNetFw_Click(object sender, EventArgs e)
        {
            INetFwManger.NetFwAddApps("SocketServer", Application.ExecutablePath);
        }

        /// <summary>
        /// �رշ�����
        /// </summary>
        private void TCPUDPServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            e.Cancel = true;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
        }
        /// <summary>
        /// �豸����ȥ�ظ�
        /// </summary>
        private void linkOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ConnectAry = new List<string>();
            for (int i = 0; i < tpe2list1.Items.Count; i++)
            {
                if (ConnectAry.Contains(tpe2list1.Items[i].SubItems[1].Text))
                {
                    tpe2list1.Items.Remove(tpe2list1.Items[i]);
                    i--;
                    continue;
                }
                ConnectAry.Add(tpe2list1.Items[i].SubItems[1].Text);
            }
        }

    }
}
