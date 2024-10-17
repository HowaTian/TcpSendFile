using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TcpServer : MonoBehaviour
{
    public int serverPort = 12345;
    TcpListener tcpListener;
    Thread recvThread;
    bool isStarted = false;

    byte[] recvData;
    Texture2D recvTexture;
    public RawImage recvImage;

    bool isReceived = false;
    // Start is called before the first frame update
    void Start()
    {
        recvTexture = new Texture2D(1, 1);
        StartServer();
    }

    void StartServer()
    {
        tcpListener = new TcpListener(IPAddress.Any, serverPort);
        tcpListener.Start();

        recvThread = new Thread(RecveiveData);
        recvThread.Start(tcpListener);

        isStarted = true;
    }

    private void RecveiveData(object obj)
    {
        TcpListener listener = (TcpListener)obj;
        while(isStarted)
        {
            TcpClient client = listener.AcceptTcpClient();
            if(client.Connected)
            {
                NetworkStream networkStream = client.GetStream();
                using(MemoryStream ms = new MemoryStream())
                {
                    networkStream.CopyTo(ms);
                    recvData = ms.ToArray();
                    isReceived = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isReceived)
        {
            recvTexture.LoadImage(recvData);
            recvImage.texture = recvTexture;
            isReceived = false;
        }
    }
}
