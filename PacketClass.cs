using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public enum PacketType { HELLO_REQUEST, HELLO_REPLY, ROUTE_REQUEST, ROUTE_REPLY };

[Serializable]
public struct HelloPacket
{
    public PacketType PacketType;
    public string SenderIp;
}

[Serializable]
public struct RoutePacket
{
    public string ServiceId;
    public PacketType PacketType;
    public string SourceIp;
    public string SenderIp;
    public string DestinationIp;
    public List<string> RouteIps;
}

[Serializable]
public struct DataPacket
{
    public string ServiceId;
    public string FileName;
    public byte[] Payload;
    public string SourceIp;
    public string DestinationIp;
    public List<string> DataPath;
}

[Serializable]
public struct AcknowledgementPacket
{
    public string ServiceId;
    public string FileName;
    public string SourceIp;
    public string DestinationIp;
    public List<string> AcknowledgementPath;
}

class PacketClass
{
    public static byte[] Serialize(Object _packetObj)
    {
        BinaryFormatter _formatterObj = new BinaryFormatter();
        MemoryStream _streamObj = new MemoryStream();
        _formatterObj.Serialize(_streamObj, _packetObj);
        return _streamObj.ToArray();
    }

    public static object DeSerialize(byte[] _data)
    {
        BinaryFormatter _formatterObj = new BinaryFormatter();
        MemoryStream _streamObj = new MemoryStream(_data);
        return _formatterObj.Deserialize(_streamObj);
    }
}