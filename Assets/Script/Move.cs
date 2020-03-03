using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using TMPro;

public enum DIR
{
    go,
    back,
    right,
    left,
    close
};

public class Move : MonoBehaviour
{
    public DIR dir;    
    TMP_InputField IP_field;        

    // Start is called before the first frame update
    void Start()
    {
        IP_field = GameObject.Find("Field_IP").GetComponent<TMP_InputField>();               
    }

    public void Send_Move()
    {
        if (GameManager.instance.connect == false)
            return;
        string port = GameManager.instance.Razig_Move_PORT;

        Debug.Log(IP_field.text);

        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        EndPoint ep = new IPEndPoint(IPAddress.Parse(IP_field.text.ToString()), int.Parse(port));

        sock.SendTo(Encoding.Default.GetBytes(dir.ToString()), ep);
    }
}
