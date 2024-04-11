using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;

public class ServiceClass : MarshalByRefObject
{
    static Hashtable _hashByteListObj = new Hashtable();

    public static void RegisterTcpChannel()
    {
        TcpChannel _channelObj = new TcpChannel(TS_AOMDV.MainForm._tcpPort);
        ChannelServices.RegisterChannel(_channelObj);
        RemotingConfiguration.RegisterWellKnownServiceType(typeof(ServiceClass), "ServiceClass", WellKnownObjectMode.SingleCall);
        RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
    }

    public void SendDataPacket(DataPacket _dataPacketObj)
    {
        new Thread(new ThreadStart(delegate
        {
            _dataPacketObj.DataPath.Add(TS_AOMDV.MainForm._myIpAddress);
            if (_dataPacketObj.DestinationIp.Equals(TS_AOMDV.MainForm._myIpAddress))
            {
                CreatePacketLogEntry(_dataPacketObj, string.Empty);
                TS_AOMDV.Program._mainFormObj.Invoke((MethodInvoker)
                delegate
                {
                    new Thread(new ThreadStart(delegate
                    {
                        DialogResult _result = MessageBox.Show("Alert!!\r\nIncoming File from " + _dataPacketObj.SourceIp + " [ServiceId: " + _dataPacketObj.ServiceId + "]\r\nProceed to Save File?", "File Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
                        if (_result.Equals(DialogResult.Yes))
                            TS_AOMDV.Program._mainFormObj.Invoke((MethodInvoker)delegate { CallForFileSave(_dataPacketObj.FileName, _dataPacketObj.Payload); });
                    })).Start();
                });

                AcknowledgementPacket _acknowledgementPacketObj = new AcknowledgementPacket();
                _acknowledgementPacketObj.ServiceId = _dataPacketObj.ServiceId;
                _acknowledgementPacketObj.FileName = _dataPacketObj.FileName;
                _acknowledgementPacketObj.SourceIp = TS_AOMDV.MainForm._myIpAddress;
                _acknowledgementPacketObj.DestinationIp = _dataPacketObj.SourceIp;
                _acknowledgementPacketObj.AcknowledgementPath = new List<string>(_dataPacketObj.DataPath);
                _acknowledgementPacketObj.AcknowledgementPath.Reverse(); //Reverse the Data Path for sending the Acknowledgement
                SendEndToEndAcknowledgementPacket(_acknowledgementPacketObj);
            }
            else
            {
                if (TS_AOMDV.Program._mainFormObj.chkDropDataPacket.Checked)
                    return;
                else
                {
                    string _nextHopIp = ChooseNextHopForFileTransmission(_dataPacketObj.ServiceId);
                    CreatePacketLogEntry(_dataPacketObj, _nextHopIp);
                    if (_nextHopIp.Equals(string.Empty)) return; //Incase No Routes Found

                    ServiceClass _proxyObj = (ServiceClass)Activator.GetObject(typeof(ServiceClass), "tcp://" + _nextHopIp + ":" + TS_AOMDV.MainForm._tcpPort + "/ServiceClass");
                    _proxyObj.SendDataPacket(_dataPacketObj);
                }
            }
        })).Start();
    }

    private void CallForFileSave(string _fileName, byte[] _fileContent)
    {
        SaveFileDialog _sfd = new SaveFileDialog();
        _sfd.FileName = _fileName;
        string _extension = new FileInfo(_fileName).Extension.ToString().Replace(".", string.Empty);
        _sfd.Filter = _extension.ToUpper() + " Files(*." + _extension + ")|*." + _extension + "|All Files(*.*)|*.*";
        _sfd.FilterIndex = 1;
        _sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        if (_sfd.ShowDialog() == DialogResult.OK)
        {
            File.WriteAllBytes(_sfd.FileName, _fileContent);
            MessageBox.Show("Success!!\r\nFile saved at your desired path successfully.\r\nPath: " + _sfd.FileName, "File Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private string ChooseNextHopForFileTransmission(string _serviceId)
    {
        //string _nextHopIp = string.Empty;
        //foreach (ListViewItem _routeItem in TS_AOMDV.Program._mainFormObj.lviewRouteLog.Items)
        //{
        //    if (!_routeItem.SubItems[0].Text.Equals(_serviceId) || _routeItem.BackColor.Equals(Color.LightSalmon)) continue;

        //    List<string> _routeIps = _routeItem.SubItems[1].Text.Split(',').ToList();
        //    _nextHopIp = _routeIps[_routeIps.IndexOf(TS_AOMDV.MainForm._myIpAddress) + 1].ToString();
        //    _routeItem.BackColor = Color.LightGreen;
        //    break;
        //}
        //return _nextHopIp;

        //Choosing Nexthop with Highest ROUTER_TRUST Parameter
        Dictionary<ListViewItem, double> _dictionaryObj = new Dictionary<ListViewItem, double>();
        foreach (ListViewItem _routeItem in TS_AOMDV.Program._mainFormObj.lviewRouteLog.Items)
        {
            if (!_routeItem.SubItems[0].Text.Equals(_serviceId) || _routeItem.BackColor.Equals(Color.LightSalmon)) continue;

            List<string> _routeIps = _routeItem.SubItems[1].Text.Split(',').ToList();
            string _nextHopIp = _routeIps[_routeIps.IndexOf(TS_AOMDV.MainForm._myIpAddress) + 1].ToString();

            int _neighborItemIndex = TS_AOMDV.Program._mainFormObj.lviewNeighborLog.Items.IndexOfKey(_nextHopIp);
            if (_neighborItemIndex != -1)
                _dictionaryObj.Add(_routeItem, double.Parse(TS_AOMDV.Program._mainFormObj.lviewNeighborLog.Items[_neighborItemIndex].SubItems[2].Text));
        }

        var _sortedList = _dictionaryObj.OrderByDescending(_item => _item.Value);
        if (_sortedList.Count() > 0)
        {
            KeyValuePair<ListViewItem, double> _keyValueObj = _sortedList.ElementAt(0);
            _keyValueObj.Key.BackColor = Color.LightGreen;

            List<string> _routeIps = _keyValueObj.Key.SubItems[1].Text.Split(',').ToList();
            string _nextHopIp = _routeIps[_routeIps.IndexOf(TS_AOMDV.MainForm._myIpAddress) + 1].ToString();
            return _nextHopIp;
        }
        else
            return string.Empty;
    }

    public void SendEndToEndAcknowledgementPacket(AcknowledgementPacket _acknowledgementPacketObj)
    {
        new Thread(new ThreadStart(delegate
        {
            if (_acknowledgementPacketObj.DestinationIp.Equals(TS_AOMDV.MainForm._myIpAddress))
                TS_AOMDV.MainForm._acknowledgementPacketObj = _acknowledgementPacketObj;
            else
            {
                string _nextHopIp = _acknowledgementPacketObj.AcknowledgementPath[_acknowledgementPacketObj.AcknowledgementPath.IndexOf(TS_AOMDV.MainForm._myIpAddress) + 1].ToString();
                ServiceClass _proxyObj = (ServiceClass)Activator.GetObject(typeof(ServiceClass), "tcp://" + _nextHopIp + ":" + TS_AOMDV.MainForm._tcpPort + "/ServiceClass");
                _proxyObj.SendEndToEndAcknowledgementPacket(_acknowledgementPacketObj);
            }
        })).Start();
    }

    private void CreatePacketLogEntry(DataPacket _dataPacketObj, string _nextHopIp)
    {
        int _ipIndex = _dataPacketObj.DataPath.IndexOf(TS_AOMDV.MainForm._myIpAddress);
        string _previousHopIp = _ipIndex == 0 ? string.Empty : _dataPacketObj.DataPath[_ipIndex - 1];

        ListViewItem _dataItem = new ListViewItem(_dataPacketObj.ServiceId);
        _dataItem.SubItems.Add(_dataPacketObj.FileName);
        _dataItem.SubItems.Add(_previousHopIp);
        _dataItem.SubItems.Add(_nextHopIp);
        TS_AOMDV.Program._mainFormObj.lviewDataLog.Items.Add(_dataItem);
        TS_AOMDV.Program._mainFormObj.lviewDataLog.EnsureVisible(_dataItem.Index);
    }
}
