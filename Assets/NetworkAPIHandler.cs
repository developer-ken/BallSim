using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class NetworkAPIHandler : MonoBehaviour
{
    public static Socket client;
    static Thread socksthread;

    public GameObject Ball;
    public GameObject Board;
    public GameObject Camera;
    public static EndPoint point;

    // Start is called before the first frame update
    void Start()
    {
        if (client == null)
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            client.Bind(new IPEndPoint(IPAddress.Parse("0.0.0.0"), 65531));
        }
        if (socksthread == null)
        {
            point = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 65532);
            socksthread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    byte[] buffer = new byte[128];
                    int length = client.ReceiveFrom(buffer, ref point);
                    string message = Encoding.UTF8.GetString(buffer, 0, length);
                    Debug.Log(message);
                    try
                    {
                        string[] msgd = message.Split('!');
                        string[] argss = msgd[1].Split(',');
                        float[] args = new float[1];
                        if (argss[0] != msgd[1])
                        {//有参数
                            args = new float[argss.Length];
                            int i = 0;
                            foreach (var item in argss)
                            {
                                args[i] = float.Parse(item);
                                i++;
                            }
                        }

                        CommandProcessor(msgd[0], args);
                    }
                    catch (Exception err)
                    {
                        client.SendTo(Encoding.UTF8.GetBytes("ERR!" + err.Message), point);
                        continue;
                    }
                }
            }));
            socksthread.IsBackground = true;
            socksthread.Start();
            InvokeRepeating("SendState", 0, 1f / 60f);//60帧重复执行
        }
    }

    void SendState()
    {
        var angle = Board.transform.rotation.eulerAngles;
        client.SendTo(Encoding.UTF8.GetBytes("S_BL!" + Ball.transform.position.x + "," + Ball.transform.position.y + "," + Ball.transform.position.z), point);
        client.SendTo(Encoding.UTF8.GetBytes("S_BR!" + angle.x + "," + angle.y + "," + angle.z), point);
    }

    public static void SendMsg(string cmd, string args)
    {
        if (client != null && point != null)
            client.SendTo(Encoding.UTF8.GetBytes(cmd + "!" + args), point);
    }

    float rx, ry, rz;
    bool reset = false;

    void CommandProcessor(string cmd, float[] args)
    {
        switch (cmd.ToUpper())
        {
            case "S_RA"://Rotation Absolute
                {
                    rx = args[0]; ry = args[1]; rz = args[2];
                }
                break;
            case "S_RST"://Reset
                {
                    reset = true;
                }
                break;
            case "PING":
                {
                    SendMsg("PONG", "0");
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(rx, ry, rz);
        Board.transform.rotation = rotation;
        if (reset)
        {
            var rrr = new Quaternion();
            rrr.eulerAngles = new Vector3(0, 0, 0);
            Board.transform.rotation = rrr;
            Ball.transform.position = new Vector3(0, 0.125f, 0);
            reset = false;
        }
    }
}
