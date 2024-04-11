using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TS_AOMDV
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            ListView.CheckForIllegalCrossThreadCalls = false;
            ListBox.CheckForIllegalCrossThreadCalls = false;
            TreeView.CheckForIllegalCrossThreadCalls = false;
        }

        Socket _receiveRouteSocketObj = null;
        Thread _resetThreadObj = null, _receiveRouteThreadObj = null, _fileThreadObj = null;
        public static int _udpPort = 12000, _tcpPort = 13000;
        public static string _myIpAddress = string.Empty;
        public static double DEFAULT_SOURCE_TRUST = 1, DEFAULT_ROUTER_TRUST = 1, SOURCE_TRUST_THRESHOLD = 0.33, ROUTER_TRUST_THRESHOLD = 0.4;
        public static int TRUST_RESET_INTERVAL = 120, ACK_REC_TOT = 5, TRUST_TRIAL_COUNT = 3;
        public static AcknowledgementPacket _acknowledgementPacketObj;

        private void MainForm_Load(object sender, EventArgs e)
        {
            _myIpAddress = Dns.GetHostByName(System.Environment.MachineName).AddressList[0].ToString();
            txtSystemIp.Text = _myIpAddress;

            IDSClass.StartService(txtSystemIp.Text);
            ServiceClass.RegisterTcpChannel();
            _receiveRouteThreadObj = new Thread(new ThreadStart(ReceiveRouteRequestReplyPacket));
            _receiveRouteThreadObj.Start();
        }

        private void btnFindNeighbor_Click(object sender, EventArgs e)
        {
            HelloPacket _packetObj = new HelloPacket();
            _packetObj.PacketType = PacketType.HELLO_REQUEST;
            _packetObj.SenderIp = _myIpAddress;

            Socket _sendRouteSocketObj = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _sendRouteSocketObj.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            EndPoint _sendEndPointObj = new IPEndPoint(IPAddress.Broadcast, _udpPort);
            _sendRouteSocketObj.SendTo(PacketClass.Serialize(_packetObj), _sendEndPointObj);
            _sendRouteSocketObj.Close();

            _resetThreadObj = new Thread(new ThreadStart(ResetSourceTrustParameter));
            _resetThreadObj.Start();
        }

        private void ResetSourceTrustParameter()
        {
            while (_resetThreadObj.IsAlive)
            {
                Thread.Sleep(TRUST_RESET_INTERVAL * 1000);
                foreach (ListViewItem _neighborItem in lviewNeighborLog.Items)
                {
                    if (_neighborItem.BackColor.Equals(Color.LightSalmon)) continue;
                    _neighborItem.SubItems[1].Text = DEFAULT_SOURCE_TRUST.ToString();
                    _neighborItem.SubItems[3].Text = "0";
                }
            }
        }

        private void ReceiveRouteRequestReplyPacket()
        {
            _receiveRouteSocketObj = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _receiveRouteSocketObj.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            EndPoint _receiveEndPointObj = new IPEndPoint(IPAddress.Any, _udpPort);
            _receiveRouteSocketObj.Bind(_receiveEndPointObj);

            while (_receiveRouteThreadObj.IsAlive)
            {
                if (_receiveRouteSocketObj.Available > 0)
                {
                    byte[] _byteArray = new byte[_receiveRouteSocketObj.Available];
                    _receiveRouteSocketObj.ReceiveFrom(_byteArray, ref _receiveEndPointObj);
                    if (_receiveEndPointObj.ToString().Split(':')[0].Equals(_myIpAddress)) continue;

                    var _deserializedObject = PacketClass.DeSerialize(_byteArray);
                    if (_deserializedObject.GetType().Equals(typeof(HelloPacket)))
                    {
                        #region HELLOPACKET
                        HelloPacket _packetObj = (HelloPacket)_deserializedObject;
                        if (_packetObj.PacketType.Equals(PacketType.HELLO_REQUEST))
                        {
                            EndPoint _sendEndPointObj = new IPEndPoint(IPAddress.Parse(_packetObj.SenderIp), _udpPort);
                            _packetObj.PacketType = PacketType.HELLO_REPLY;
                            _packetObj.SenderIp = _myIpAddress;
                            _receiveRouteSocketObj.SendTo(PacketClass.Serialize(_packetObj), _sendEndPointObj);
                        }
                        else if (_packetObj.PacketType.Equals(PacketType.HELLO_REPLY))
                        {
                            ListViewItem _neighborItem = new ListViewItem(_packetObj.SenderIp);
                            _neighborItem.Name = _packetObj.SenderIp;
                            _neighborItem.SubItems.Add(DEFAULT_SOURCE_TRUST.ToString());
                            _neighborItem.SubItems.Add(DEFAULT_ROUTER_TRUST.ToString());
                            _neighborItem.SubItems.Add("0");
                            _neighborItem.SubItems.Add("0");
                            _neighborItem.SubItems.Add("0");
                            lviewNeighborLog.Items.Add(_neighborItem);
                        }
                        #endregion
                    }
                    else if (_deserializedObject.GetType().Equals(typeof(RoutePacket)))
                    {
                        #region ROUTEPACKET
                        RoutePacket _packetObj = (RoutePacket)_deserializedObject;
                        if (_packetObj.PacketType.Equals(PacketType.ROUTE_REQUEST))
                        {
                            #region ROUTE_REQUEST
                            if (_packetObj.RouteIps.Contains(_myIpAddress)) continue; //To Avoid Looping

                            if (GetListViewItemByServiceId(_packetObj.ServiceId) == null)
                            {
                                ListViewItem _serviceItem = new ListViewItem(_packetObj.ServiceId);
                                _serviceItem.SubItems.Add(_packetObj.SourceIp);
                                _serviceItem.SubItems.Add(_packetObj.DestinationIp);
                                lviewServiceLog.Items.Add(_serviceItem);
                                lviewServiceLog.EnsureVisible(_serviceItem.Index);

                                //Discard the RREQ Packet from Malicious Node (To Avoid Flooding Attack)
                                int _neighborItemIndex = lviewNeighborLog.Items.IndexOfKey(_packetObj.SourceIp);
                                if (_neighborItemIndex != -1 && lviewNeighborLog.Items[_neighborItemIndex].BackColor.Equals(Color.LightSalmon))
                                {
                                    _serviceItem.BackColor = Color.LightSalmon;
                                    continue;
                                }
                            }

                            if (_packetObj.DestinationIp.Equals(_myIpAddress))
                            {
                                //Request In case of Sink Node [Give Acknowledgement]
                                _packetObj.PacketType = PacketType.ROUTE_REPLY;
                                _packetObj.DestinationIp = _packetObj.SourceIp;
                                _packetObj.SourceIp = _myIpAddress;
                                _packetObj.SenderIp = _myIpAddress;
                                _packetObj.RouteIps.Add(_myIpAddress);
                                SendRouteReplyPacket(_packetObj);
                            }
                            else
                            {
                                //Request In case of Router Node [Forward Request]
                                BroadCastRouteRequestPacket(_packetObj);
                            }
                            #endregion
                        }
                        else if (_packetObj.PacketType.Equals(PacketType.ROUTE_REPLY))
                        {
                            #region ROUTE_REPLY
                            ListViewItem _routeItem = new ListViewItem(_packetObj.ServiceId);
                            _routeItem.SubItems.Add(string.Join(",", _packetObj.RouteIps));
                            lviewRouteLog.Items.Add(_routeItem);
                            lviewRouteLog.EnsureVisible(_routeItem.Index);

                            //Discard the RREP Packet from Malicious Node
                            //string _neighborIp = _packetObj.RouteIps[_packetObj.RouteIps.IndexOf(_myIpAddress) + 1];
                            //int _neighborItemIndex = lviewNeighborLog.Items.IndexOfKey(_neighborIp);
                            //if (_neighborItemIndex != -1 && lviewNeighborLog.Items[_neighborItemIndex].BackColor.Equals(Color.LightSalmon))
                            //{
                            //    _routeItem.BackColor = Color.LightSalmon;
                            //    continue;
                            //}

                            //Discard the RREP Packet if Malicious Node found in RouteIps
                            bool _routeHasMaliciousIp = false;
                            foreach (string _routeIp in _packetObj.RouteIps)
                            {
                                int _neighborItemIndex = lviewNeighborLog.Items.IndexOfKey(_routeIp);
                                if (_neighborItemIndex != -1 && lviewNeighborLog.Items[_neighborItemIndex].BackColor.Equals(Color.LightSalmon))
                                {
                                    _routeItem.BackColor = Color.LightSalmon;
                                    _routeHasMaliciousIp = true;
                                    break;
                                }
                            }
                            if (_routeHasMaliciousIp) continue;


                            if (!_packetObj.DestinationIp.Equals(_myIpAddress))
                            {
                                //Acknowledgement In case of Router Node [Forward Acknowledgement]
                                SendRouteReplyPacket(_packetObj);
                            }
                            else if (_packetObj.DestinationIp.Equals(_myIpAddress))
                            {
                                //Start Message Transmission using received Optimal Path
                                if (_fileThreadObj != null && _fileThreadObj.IsAlive) continue;

                                //Start File Transmission
                                _fileThreadObj = new Thread(new ParameterizedThreadStart(CallForFileTransfer));
                                _fileThreadObj.Start(_packetObj);
                            }
                            #endregion
                        }
                        #endregion
                    }
                }
                Thread.Sleep(1000);
            }
        }

        private ListViewItem GetListViewItemByServiceId(string _serviceId)
        {
            foreach (ListViewItem _serviceItem in lviewServiceLog.Items)
            {
                if (_serviceItem.SubItems[0].Text.Equals(_serviceId))
                    return _serviceItem;
            }
            return null;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog _ofd = new OpenFileDialog();
            _ofd.Filter = "All Files(*.*)|*.*|Text Files|*.txt";
            _ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (_ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = _ofd.FileName;
                txtFileName.Text = Path.GetFileNameWithoutExtension(_ofd.FileName);
                txtFileSize.Tag = new FileInfo(_ofd.FileName).Length;
                txtFileSize.Text = Win32.FileSizeFormatProvider.FileSize(new FileInfo(_ofd.FileName).Length);
                txtFileType.Text = Path.GetExtension(_ofd.FileName).ToUpper().Replace(".", string.Empty);
            }
        }

        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (txtDestinationIp.Text.Equals(String.Empty))
            {
                MessageBox.Show("Error!!\r\nEnter IpAddress before Proceeding", "Route Discovery", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtFilePath.Text.Equals(string.Empty))
            {
                MessageBox.Show("Error!!\r\nSelect File to Send before Proceeding", "Route Discovery", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RoutePacket _packetObj = new RoutePacket();
            _packetObj.ServiceId = new Random().Next(1000, 9999).ToString();
            _packetObj.PacketType = PacketType.ROUTE_REQUEST;
            _packetObj.SourceIp = _myIpAddress;
            _packetObj.SenderIp = _myIpAddress;
            _packetObj.DestinationIp = txtDestinationIp.Text;
            _packetObj.RouteIps = new List<string>();

            ListViewItem _serviceItem = new ListViewItem(_packetObj.ServiceId);
            _serviceItem.SubItems.Add(_packetObj.SourceIp);
            _serviceItem.SubItems.Add(_packetObj.DestinationIp);
            lviewServiceLog.Items.Add(_serviceItem);
            lviewServiceLog.EnsureVisible(_serviceItem.Index);

            BroadCastRouteRequestPacket(_packetObj);
        }

        private void BroadCastRouteRequestPacket(RoutePacket _packetObj)
        {
            _packetObj.SenderIp = _myIpAddress;
            _packetObj.RouteIps.Add(_myIpAddress);

            Socket _sendRouteSocketObj = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _sendRouteSocketObj.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            EndPoint _sendEndPointObj = new IPEndPoint(IPAddress.Broadcast, _udpPort);
            _sendRouteSocketObj.SendTo(PacketClass.Serialize(_packetObj), _sendEndPointObj);
            _sendRouteSocketObj.Close();
        }

        private void SendRouteReplyPacket(RoutePacket _packetObj)
        {
            _packetObj.SenderIp = _myIpAddress;
            string _previousHopIp = _packetObj.RouteIps[_packetObj.RouteIps.IndexOf(_myIpAddress) - 1].ToString();

            Socket _sendRouteSocketObj = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _sendRouteSocketObj.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);

            IPEndPoint _sendEndPointObj = new IPEndPoint(IPAddress.Parse(_previousHopIp), _udpPort);
            _sendRouteSocketObj.SendTo(PacketClass.Serialize(_packetObj), _sendEndPointObj);
            _sendRouteSocketObj.Close();
        }

        private void CallForFileTransfer(object _paramPacketObj)
        {
            this.Cursor = Cursors.WaitCursor;
            Thread.Sleep(2000); //Wait until All RREPs are received
            RoutePacket _packetObj = (RoutePacket)_paramPacketObj;
            btnBrowse.Enabled = false;
            btnSendFile.Enabled = false;
            bool _fileSendStatus = true;

            DataPacket _dataPacketObj = new DataPacket();
            _dataPacketObj.ServiceId = _packetObj.ServiceId;
            _dataPacketObj.FileName = Path.GetFileName(txtFilePath.Text);
            _dataPacketObj.Payload = File.ReadAllBytes(txtFilePath.Text);
            _dataPacketObj.SourceIp = _myIpAddress;
            _dataPacketObj.DestinationIp = _packetObj.SourceIp; //RREP Packet Source is our File Destination
            _dataPacketObj.DataPath = new List<string>();

            _acknowledgementPacketObj = new AcknowledgementPacket();
            (new ServiceClass()).SendDataPacket(_dataPacketObj);
            Thread.Sleep(ACK_REC_TOT * 1000);  //Acknowledgement Receive Timeout
            if (_acknowledgementPacketObj.ServiceId == null || !_acknowledgementPacketObj.ServiceId.Equals(_dataPacketObj.ServiceId) || !_acknowledgementPacketObj.FileName.Equals(_dataPacketObj.FileName))
                _fileSendStatus = false;

            //    _acknowledgementPacketObj = new AcknowledgementPacket();
            //    int _tryCounter = 0;
            //Retry:
            //    _tryCounter++;
            //    if (_tryCounter > 3) { _fileSendStatus = false; goto End; }
            //    (new ServiceClass()).SendDataPacket(_dataPacketObj);
            //    Thread.Sleep(ACK_REC_TOT * 1000);  //Acknowledgement Receive Timeout
            //    if (_acknowledgementPacketObj.ServiceId == null || !_acknowledgementPacketObj.ServiceId.Equals(_dataPacketObj.ServiceId) || !_acknowledgementPacketObj.FileName.Equals(_dataPacketObj.FileName))
            //        goto Retry;
            //End:

            Thread.Sleep(2000); //Wait until previous TCP Connection gets Closed
            btnBrowse.Enabled = true;
            btnSendFile.Enabled = true;
            this.Cursor = Cursors.Default;
            if (_fileSendStatus)
                MessageBox.Show("Success!!\r\nFile Sent Successfully [ServiceId: " + _dataPacketObj.ServiceId + "]", "File Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error!!\r\nFile Sending was UnSuccessfull [ServiceId: " + _dataPacketObj.ServiceId + "]", "File Transfer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _fileThreadObj.Abort();
        }

        private void btnResetAll_Click(object sender, EventArgs e)
        {
            if (_fileThreadObj != null && _fileThreadObj.IsAlive)
                _fileThreadObj.Abort();

            if (_resetThreadObj != null && _resetThreadObj.IsAlive)
                _resetThreadObj.Abort();

            lviewNeighborLog.Items.Clear();
            lviewDataLog.Items.Clear();
            lviewServiceLog.Items.Clear();
            lviewRouteLog.Items.Clear();
            tviewSniferLog.Nodes.Clear();
            lboxTcpAckLog.Items.Clear();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            IDSClass.StopService();
            if (_receiveRouteThreadObj != null && _receiveRouteThreadObj.IsAlive)
            {
                _receiveRouteThreadObj.Abort();
                _receiveRouteSocketObj.Close();
            }

            if (_fileThreadObj != null && _fileThreadObj.IsAlive)
                _fileThreadObj.Abort();

            if (_resetThreadObj != null && _resetThreadObj.IsAlive)
                _resetThreadObj.Abort();
            Application.Exit();
        }
    }
}
