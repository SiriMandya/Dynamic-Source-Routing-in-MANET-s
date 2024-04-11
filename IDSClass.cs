using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TS_AOMDV
{
    public class IDSClass
    {
        private static Socket _mainSocketObj; //The socket which captures all incoming packets
        private static byte[] _byteDataObj = new byte[4096];
        private static List<Tuple<double, double>> _finalAckList = new List<Tuple<double, double>>(); //Tuple<SequenceNo,AcknowledgementNo>
        private static Hashtable _neighborIpList = new Hashtable(); //Key-Tuple<SequenceNo,AcknowledgementNo>, Value-NeighborIpAddress

        public static void StartService(string _networkInferfaceAddress)
        {
            try
            {
                //Start capturing the packets...
                //For sniffing the socket to capture the packets has to be a raw socket, with the
                //address family being of type internetwork, and protocol being IP
                _mainSocketObj = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);

                //Bind the socket to the selected IP address
                _mainSocketObj.Bind(new IPEndPoint(IPAddress.Parse(_networkInferfaceAddress), 0));

                //Set the socket  options
                //Applies only to IP packets, Set the include the header option to true
                _mainSocketObj.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);

                byte[] _optionInValue = new byte[4] { 1, 0, 0, 0 }; //Capture incoming packets
                byte[] _optionOutValue = new byte[4] { 1, 0, 0, 0 }; //Capture outgoing packets

                //Socket.IOControl is analogous to the WSAIoctl method of Winsock 2
                //Equivalent to SIO_RCVALL constant of Winsock 2
                _mainSocketObj.IOControl(IOControlCode.ReceiveAll, _optionInValue, _optionOutValue);

                //Start receiving the packets asynchronously
                _mainSocketObj.BeginReceive(_byteDataObj, 0, _byteDataObj.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Packet Sniffer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void StopService()
        {
            //To stop capturing the packets close the socket
            _mainSocketObj.Close();
        }

        private static void OnReceive(IAsyncResult _result)
        {
            try
            {
                int nReceived = _mainSocketObj.EndReceive(_result);
                //Analyze the bytes received...
                ParseData(_byteDataObj, nReceived);
                _byteDataObj = new byte[4096];
                //Another call to BeginReceive so that we continue to receive the incoming packets
                _mainSocketObj.BeginReceive(_byteDataObj, 0, _byteDataObj.Length, SocketFlags.None, new AsyncCallback(OnReceive), null);
            }
            catch (ObjectDisposedException) { }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Packet Sniffer", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void ParseData(byte[] _byteDataObj, int nReceived)
        {
            TreeNode _rootNode = new TreeNode();

            //Since all protocol packets are encapsulated in the IP datagram
            // so we start by parsing the IP header and see what protocol data is being carried by it
            IPHeader _ipHeader = new IPHeader(_byteDataObj, nReceived);

            TreeNode _ipNode = MakeIPTreeNode(_ipHeader);
            _rootNode.Nodes.Add(_ipNode);

            //Now according to the protocol being carried by the IP datagram we parse the data field of the datagram
            switch (_ipHeader.ProtocolType)
            {
                case IPHeader.Protocol.TCP:
                    //IPHeader.Data stores the data being carried by the IP datagram, 
                    //IPHeader.MessageLength Length of the data field
                    TCPHeader _tcpHeader = new TCPHeader(_ipHeader.Data, _ipHeader.MessageLength);
                    if (!_tcpHeader.SourcePort.Equals(MainForm._tcpPort.ToString()) && !_tcpHeader.DestinationPort.Equals(MainForm._tcpPort.ToString()))
                    {
                        _rootNode.Nodes.Remove(_ipNode);
                        return;
                    }

                    if (_tcpHeader.Flags.Equals("0x11 (FIN, ACK)"))
                    {
                        Program._mainFormObj.lboxTcpAckLog.Items.Add(Program._mainFormObj.tviewSniferLog.Nodes.Count + ":" + _tcpHeader.Flags + ":" + _tcpHeader.SequenceNumber + ":" + _tcpHeader.AcknowledgementNumber + ":" + _ipHeader.SourceAddress + ":" + _ipHeader.DestinationAddress);
                        if (_ipHeader.SourceAddress.ToString().Equals(MainForm._myIpAddress))
                        {
                            //Incase if CurrentNode sending (FIN, ACK) Packet after "DataPacket" or "DataAcknowledgementPacket"
                            if (_finalAckList.Contains(new Tuple<double, double>(double.Parse(_tcpHeader.AcknowledgementNumber) - 1, double.Parse(_tcpHeader.SequenceNumber))))
                            {
                                //Increment AckCount for DestinationIp
                                IncrementPacketCount(_ipHeader.DestinationAddress.ToString(), false);
                                _finalAckList.Remove(new Tuple<double, double>(double.Parse(_tcpHeader.AcknowledgementNumber) - 1, double.Parse(_tcpHeader.SequenceNumber)));
                            }
                            else
                            {
                                _finalAckList.Add(new Tuple<double, double>(double.Parse(_tcpHeader.SequenceNumber), double.Parse(_tcpHeader.AcknowledgementNumber)));
                                //If Data Packet Sent to NeighborIp, Record NeighborIP in HashTable for Monitoring
                                _neighborIpList.Add(new Tuple<double, double>(double.Parse(_tcpHeader.SequenceNumber), double.Parse(_tcpHeader.AcknowledgementNumber)), _ipHeader.DestinationAddress);
                            }
                        }
                        else if (_ipHeader.DestinationAddress.ToString().Equals(MainForm._myIpAddress))
                        {
                            //Incase if CurrentNode receiving (FIN, ACK) Packet (from NeigborIp) after "DataPacket" or "DataAcknowledgementPacket"                            
                            if (_finalAckList.Contains(new Tuple<double, double>(double.Parse(_tcpHeader.AcknowledgementNumber) - 1, double.Parse(_tcpHeader.SequenceNumber))))
                            {
                                //Increment DataCount for SourceIp
                                IncrementPacketCount(_ipHeader.SourceAddress.ToString(), true);
                                _finalAckList.Remove(new Tuple<double, double>(double.Parse(_tcpHeader.AcknowledgementNumber) - 1, double.Parse(_tcpHeader.SequenceNumber)));

                                string _neighborIp = _neighborIpList[new Tuple<double, double>(double.Parse(_tcpHeader.AcknowledgementNumber) - 1, double.Parse(_tcpHeader.SequenceNumber))].ToString();
                                new Thread(new ThreadStart(delegate
                                {
                                    Thread.Sleep(MainForm.ACK_REC_TOT * 1000);//Wait Until Acknowledgement Packet Received from NeighborIp
                                    CheckForRoutingAttack(_neighborIp);
                                })).Start();
                                _neighborIpList.Remove(new Tuple<double, double>(double.Parse(_tcpHeader.AcknowledgementNumber) - 1, double.Parse(_tcpHeader.SequenceNumber)));
                            }
                            else
                                _finalAckList.Add(new Tuple<double, double>(double.Parse(_tcpHeader.SequenceNumber), double.Parse(_tcpHeader.AcknowledgementNumber)));
                        }
                    }

                    TreeNode _tcpNode = MakeTCPTreeNode(_tcpHeader);
                    _rootNode.Nodes.Add(_tcpNode);
                    break;
                case IPHeader.Protocol.UDP:
                    //IPHeader.Data stores the data being carried by the IP datagram, 
                    //IPHeader.MessageLength Length of the data field
                    UDPHeader _udpHeader = new UDPHeader(_ipHeader.Data, (int)_ipHeader.MessageLength);
                    if (!_udpHeader.SourcePort.Equals(MainForm._udpPort.ToString()) && !_udpHeader.DestinationPort.Equals(MainForm._udpPort.ToString()))
                    {
                        _rootNode.Nodes.Remove(_ipNode);
                        return;
                    }

                    var _deserializedObject = PacketClass.DeSerialize(_udpHeader.Data);
                    if (_deserializedObject.GetType().Equals(typeof(RoutePacket)))
                    {
                        RoutePacket _packetObj = (RoutePacket)_deserializedObject;
                        if (_packetObj.PacketType.Equals(PacketType.ROUTE_REQUEST))
                        {
                            //Check if the packet received from originating node
                            if (_packetObj.SourceIp.Equals(_ipHeader.SourceAddress.ToString()))
                            {
                                //Increments the RREQ count of the corresponding source 
                                int _neighborItemIndex = Program._mainFormObj.lviewNeighborLog.Items.IndexOfKey(_packetObj.SourceIp);
                                if (_neighborItemIndex != -1)
                                {
                                    int _rreqCount = int.Parse(Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[3].Text);
                                    Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[3].Text = (_rreqCount + 1).ToString();

                                    //Update Source Trust Parameter
                                    double _sourceTrustParamter = Math.Round((double)1 / int.Parse(Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[3].Text), 2);
                                    Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[1].Text = _sourceTrustParamter.ToString();
                                    if (_sourceTrustParamter < MainForm.SOURCE_TRUST_THRESHOLD)
                                        Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].BackColor = Color.LightSalmon;
                                }
                            }
                        }
                    }

                    TreeNode _udpNode = MakeUDPTreeNode(_udpHeader);
                    _rootNode.Nodes.Add(_udpNode);
                    break;
                case IPHeader.Protocol.Unknown:
                    _rootNode.Nodes.Remove(_ipNode);
                    break;
            }

            if (_rootNode.Nodes.Count == 0) return;
            _rootNode.Text = _ipHeader.SourceAddress.ToString() + "-" + _ipHeader.DestinationAddress.ToString();

            //Thread safe adding of the nodes
            TS_AOMDV.Program._mainFormObj.tviewSniferLog.Invoke((MethodInvoker)
                 delegate
                 {
                     TS_AOMDV.Program._mainFormObj.tviewSniferLog.Nodes.Add(_rootNode);
                     TS_AOMDV.Program._mainFormObj.tviewSniferLog.Nodes[TS_AOMDV.Program._mainFormObj.tviewSniferLog.Nodes.Count - 1].EnsureVisible();
                 });
        }

        private static void IncrementPacketCount(string _neighborIp, bool _isForSent)
        {
            //Increments the Sent/Received count of the corresponding NeighborIp 
            int _neighborItemIndex = Program._mainFormObj.lviewNeighborLog.Items.IndexOfKey(_neighborIp);
            if (_neighborItemIndex != -1)
            {
                if (_isForSent)
                {
                    int _sentCount = int.Parse(Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[4].Text);
                    Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[4].Text = (_sentCount + 1).ToString();
                }
                else
                {
                    int _receivedCount = int.Parse(Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[5].Text);
                    Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[5].Text = (_receivedCount + 1).ToString();
                }
            }
        }

        private static void CheckForRoutingAttack(string _neighborIp)
        {
            int _neighborItemIndex = Program._mainFormObj.lviewNeighborLog.Items.IndexOfKey(_neighborIp);
            if (_neighborItemIndex != -1)
            {
                int _sentCount = int.Parse(Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[4].Text);
                int _receivedCount = int.Parse(Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[5].Text);
                if (_receivedCount <= _sentCount)
                {
                    //Update Router Trust Parameter
                    //double _failureRate = Math.Round((double)(_sentCount - _receivedCount) / _sentCount, 2);
                    //double _successRate = 1 - _failureRate;
                    //Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[2].Text = _successRate.ToString();
                    //if (_sentCount > MainForm.TRUST_TRIAL_COUNT && _successRate < MainForm.ROUTER_TRUST_THRESHOLD) //Minimum Trails required before Declaring Malicious
                    //    Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].BackColor = Color.LightSalmon;

                    double _failureRate = Math.Round((double)_receivedCount / _sentCount, 2);
                    Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[2].Text = _failureRate.ToString();
                    if (_sentCount > MainForm.TRUST_TRIAL_COUNT && _failureRate < MainForm.ROUTER_TRUST_THRESHOLD) //Minimum Trails required before Declaring Malicious
                        Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].BackColor = Color.LightSalmon;
                }
            }
        }

        //Helper function which returns the information contained in the IP header as a tree node
        private static TreeNode MakeIPTreeNode(IPHeader _ipHeader)
        {
            TreeNode _ipNode = new TreeNode();
            _ipNode.Text = "IP";
            _ipNode.Nodes.Add("Ver: " + _ipHeader.Version);
            _ipNode.Nodes.Add("Header Length: " + _ipHeader.HeaderLength);
            _ipNode.Nodes.Add("Differntiated Services: " + _ipHeader.DifferentiatedServices);
            _ipNode.Nodes.Add("Total Length: " + _ipHeader.TotalLength);
            _ipNode.Nodes.Add("Identification: " + _ipHeader.Identification);
            _ipNode.Nodes.Add("Flags: " + _ipHeader.Flags);
            _ipNode.Nodes.Add("Fragmentation Offset: " + _ipHeader.FragmentationOffset);
            _ipNode.Nodes.Add("Time to live: " + _ipHeader.TTL);
            switch (_ipHeader.ProtocolType)
            {
                case IPHeader.Protocol.TCP:
                    _ipNode.Nodes.Add("Protocol: " + "TCP");
                    break;
                case IPHeader.Protocol.UDP:
                    _ipNode.Nodes.Add("Protocol: " + "UDP");
                    break;
                case IPHeader.Protocol.Unknown:
                    _ipNode.Nodes.Add("Protocol: " + "Unknown");
                    break;
            }
            _ipNode.Nodes.Add("Checksum: " + _ipHeader.Checksum);
            _ipNode.Nodes.Add("Source: " + _ipHeader.SourceAddress.ToString());
            _ipNode.Nodes.Add("Destination: " + _ipHeader.DestinationAddress.ToString());
            return _ipNode;
        }

        //Helper function which returns the information contained in the TCP header as a tree node
        private static TreeNode MakeTCPTreeNode(TCPHeader _tcpHeader)
        {
            TreeNode _tcpNode = new TreeNode();
            _tcpNode.Text = "TCP";
            _tcpNode.Nodes.Add("Source Port: " + _tcpHeader.SourcePort);
            _tcpNode.Nodes.Add("Destination Port: " + _tcpHeader.DestinationPort);
            _tcpNode.Nodes.Add("Sequence Number: " + _tcpHeader.SequenceNumber);
            if (_tcpHeader.AcknowledgementNumber != "")
                _tcpNode.Nodes.Add("Acknowledgement Number: " + _tcpHeader.AcknowledgementNumber);
            _tcpNode.Nodes.Add("Header Length: " + _tcpHeader.HeaderLength);
            _tcpNode.Nodes.Add("Flags: " + _tcpHeader.Flags);
            _tcpNode.Nodes.Add("Window Size: " + _tcpHeader.WindowSize);
            _tcpNode.Nodes.Add("Checksum: " + _tcpHeader.Checksum);
            if (_tcpHeader.UrgentPointer != "")
                _tcpNode.Nodes.Add("Urgent Pointer: " + _tcpHeader.UrgentPointer);
            return _tcpNode;
        }

        //Helper function which returns the information contained in the UDP header as a tree node
        private static TreeNode MakeUDPTreeNode(UDPHeader _udpHeader)
        {
            TreeNode _udpNode = new TreeNode();
            _udpNode.Text = "UDP";
            _udpNode.Nodes.Add("Source Port: " + _udpHeader.SourcePort);
            _udpNode.Nodes.Add("Destination Port: " + _udpHeader.DestinationPort);
            _udpNode.Nodes.Add("Length: " + _udpHeader.Length);
            _udpNode.Nodes.Add("Checksum: " + _udpHeader.Checksum);
            return _udpNode;
        }
    }
}
