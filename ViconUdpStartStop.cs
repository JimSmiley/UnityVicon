// this sends the xml to Nexus, Tracker and maybe shogun to remote start stop. You need to arm for recording...
using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Linq;

public class ViconUdpStartStop : MonoBehaviour
{
    public static int localPort;

    public string IP  = "192.168.50.255";  // Our wireless network is 192.168.50, and 255 is broadcast udp.Change this to one of the  values shown in nexus if on the local pc. Will need to match the port values as well.
    public string newFilename;
    string currentFileName;
    public int port = 9000;
    public string notes;
    public string description;
    string xml;

  
    IPEndPoint remoteEndPoint;
    UdpClient client;

    


   void Start()
    {

        remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);
        client = new UdpClient();

    }
    void GenerateFileName()
    {
        currentFileName = newFilename + DateTime.Now.ToString("HH''mm''ss");
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // for testing only- Call the methods however you would like
        {
           
            ViconStart();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {

            ViconStop();
        }
    }
  
    public void ViconStart()
    {
        GenerateFileName();
        int packetNum = UnityEngine.Random.Range(10000, 99999);
        string num = packetNum.ToString();
       
       // Need to change the filepath in here, but Nexus seems to ignore it which is nice. ...
        xml = $@"<?xml version=""1.0"" encoding=""UTF - 8"" standalone=""no""?><CaptureStart><Name VALUE=""{currentFileName}""/><Notes VALUE=""{notes}""/><Description VALUE=""{description}""/><DatabasePath VALUE=""C:\JimNexus\JimTesting\Jim\JimAnimTesting\""/><Delay VALUE=""0""/><PacketID VALUE=""{num}""/></CaptureStart>";
        SendString(xml);


    }
    public void ViconStop()
    {
        int packetNum = UnityEngine.Random.Range(10000, 99999);
        string num = packetNum.ToString();
        xml = $@"<?xml version=""1.0"" encoding=""UTF - 8"" standalone=""no""?><CaptureStop><Name VALUE=""{currentFileName}""/><DatabasePath VALUE=""C:\JimNexus\JimTesting\Jim\JimAnimTesting\""/><Delay VALUE=""0""/><PacketID VALUE=""{num}""/></CaptureStop>";
        SendString(xml);

    }

   

   void SendString(string message)
    {
        print("Sent" + message);
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
           

            client.Send(data, data.Length, remoteEndPoint);
            
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }



   
}
