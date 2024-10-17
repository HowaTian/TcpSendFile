using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class TcpSendFile : MonoBehaviour
{
    public string serverIp = "127.0.0.1";
    public int serverPort = 12345;

    public string filePath = "fish.png";

    public Button sendBtn;

    // Start is called before the first frame update
    void Start()
    {
        sendBtn.onClick.AddListener(SendFile);
    }

    private void SendFile()
    {
        if(File.Exists(filePath))
        {
            Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(serverIp, serverPort);
            client.BeginSendFile(filePath,new AsyncCallback(SendCallback),client);
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        Socket client = (Socket)ar.AsyncState;
        client.EndSendFile(ar);
        client.Shutdown(SocketShutdown.Both);
        client.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
