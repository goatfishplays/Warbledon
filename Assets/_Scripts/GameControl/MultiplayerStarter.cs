using FishNet.Managing;
using FishNet.Managing.Scened;
using FishNet.Object;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerStarter : MonoBehaviour
{
    public NetworkManager nm;
    public static string IP = "localhost";
    public static string username = "Username";
    public static bool isHosting = false;
    public TMP_InputField ipInput;
    public TMP_InputField nameInput;
    public Toggle isHostToggle;

    private void Start()
    {
        if (ipInput)
        {
            ipInput.text = IP;
            nameInput.text = username;
            isHostToggle.isOn = isHosting;
        }
        else
        {
            StartMatch();
            Debug.Log($"Username: {username}");
            Debug.Log($"IP: {IP}");
            Debug.Log($"Host: {isHosting}");
        }

    }

    public void SetIP(string newIP)
    {
        IP = newIP;
    }
    public void SetName(string newName)
    {
        username = newName;
    }

    public void SetIsHosting(bool hosting)
    {
        isHosting = hosting;
    }
    public void StartMatch()
    {
        if (isHosting)
        {
            nm.ServerManager.StartConnection();
            nm.ClientManager.StartConnection("localhost");
        }
        else
        {
            nm.ClientManager.StartConnection(IP);
        }
    }

}
