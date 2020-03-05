using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net.NetworkInformation;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;    
    
    //온습도 텍스트 
    //public TMP_Text text_temp;
    //public TMP_Text text_hum;

    //라지그 IP PORT 정보 
    public TMP_InputField Razig_IP;

    public string Razig_Move_PORT = "9001";
    public string Razig_TH_PORT = "9002";

    //udp서버를 실행할 쓰레드 
    Thread udp_thread;

    string MyPort = "9003";
    string MyIP;

    public bool connect = false;

    //받아온 온습도를 받아서 텍스트를 업데이트할 큐 
    Queue<string> temp_q;
    Queue<string> hum_q;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //로컬 아이피 얻기 
        IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
        string localIP;
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                MyIP = localIP;
                Debug.Log(MyIP);
            }
        }

        //온도, 습도 큐 초기화 
        //temp_q = new Queue<string>();
        //hum_q = new Queue<string>();

        //온,습도 업데이트하는 코루틴 시작 
        //StartCoroutine(Update_Temp_Hum());

        //서버 쓰레드 시작 
        //udp_thread = new Thread(UDP_Server);
        //udp_thread.Start();
    }

    //주기적을 큐에서 온,습도 정보를 가져와 텍스트 업데이트 
    IEnumerator Update_Temp_Hum()
    {
        while(true)
        {
            //0.5초 주기로 탐색
            //yield return new WaitForSeconds(0.5f);
            //if (!connect)
            //    continue;
            //if (temp_q.Count > 0)
            //    text_temp.text = temp_q.Dequeue();
            //if (hum_q.Count > 0)
            //    text_hum.text = hum_q.Dequeue();            
        }     
        
    }

    //쓰레드 함수 - UDP서버 열기 
    private void UDP_Server()
    {        
        //udp소켓 생성 
        Socket udp_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        //EndPoint 설정(IP와 Port)
        IPEndPoint ep = new IPEndPoint(IPAddress.Any,int.Parse(MyPort));

        //소켓 바인드 
        udp_socket.Bind(ep);
        
        //수신 시작 
        while (true)
        {
            int recv = 0;
            byte[] buff = new byte[1024];

            //Read를 하기 위한 클라의 EndPoint, 0은 모든포트에 대한 감시이지만 이미 9000에 바인드 됨
            EndPoint repANY = new IPEndPoint(IPAddress.Any, 0);
            //위에서 지정한 EndPoint로 들어오는 정보를 수신한다.
            recv = udp_socket.ReceiveFrom(buff, ref repANY);
            //수신한 길이가 0 이상이면 data를 받는다 
            if (recv > 0)
            {
                string dataFromClient = Encoding.Default.GetString(buff, 0, recv);
                Debug.Log(dataFromClient);

                if (dataFromClient.Substring(0, 2) == "TS")
                {
                    string temp_s = dataFromClient.Substring(2, 5);
                    string hum_s = dataFromClient.Substring(7, 5);                    
                                        
                    temp_q.Enqueue(temp_s);
                    hum_q.Enqueue(hum_s);

                }

            }

        }
    }

    //라지그에 내 IP와 포트번호 전송하기 
    public void Connect()
    {
        connect = true;
        Send(MyIP + ":"+ MyPort, Razig_TH_PORT);        
        Debug.Log("connect");
    }

    //자율주행 모드로 설정 
    public void SendAuto()
    {
        Send("auto", Razig_Move_PORT);
    }

    //수동주행 모드로 설정 
    public void SendPassive()
    {
        Send("passive", Razig_Move_PORT);
    }

    //UDP로 메시지 전송 
    void Send(string cmd, string port)
    {
        if (!connect)
            return;
        //소켓 생성 
        Socket sock_local = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //라지그 EndPoint 
        EndPoint epUDP = new IPEndPoint(IPAddress.Parse(Razig_IP.text), int.Parse(port));

        //라지그에 내 IP,PORT번호 전송 
        sock_local.SendTo(Encoding.Default.GetBytes(cmd), epUDP);
    }

    // Update is called once per frame
    void Update()
    {
        //안드로이드 기준 뒤로가기 입력 시 
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
