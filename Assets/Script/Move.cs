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
    public TMP_InputField IP_field;
    public TMP_InputField PORT_field;    
    

    // Start is called before the first frame update
    void Start()
    {
        IP_field = GameObject.Find("Field_IP").GetComponent<TMP_InputField>();        
        PORT_field = GameObject.Find("Field_PORT").GetComponent<TMP_InputField>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Send_Move()
    {
        Debug.Log(IP_field.text);

        Debug.Log(PORT_field.text);

        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        EndPoint ep = new IPEndPoint(IPAddress.Parse(IP_field.text.ToString()), int.Parse(PORT_field.text));

        sock.SendTo(Encoding.Default.GetBytes(dir.ToString()), ep);
    }
}
