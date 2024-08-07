using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;
    private bool isConnected = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ConnectToServer()
    {
        if (!isConnected)
        {
            Debug.Log("Conectando al servidor...");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor maestro");
        isConnected = true;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Unido al lobby");
    }

    public void CreateRoom(string roomName)
    {
        if (isConnected)
        {
            Debug.Log("Intentando crear sala: " + roomName);
            PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 2 });
        }
        else
        {
            Debug.LogWarning("No estás conectado al servidor. Intentando conectar...");
            ConnectToServer();
        }
    }

    public void JoinRoom(string roomName)
    {
        if (isConnected)
        {
            Debug.Log("Intentando unirse a la sala: " + roomName);
            PhotonNetwork.JoinRoom(roomName);
        }
        else
        {
            Debug.LogWarning("No estás conectado al servidor. Intentando conectar...");
            ConnectToServer();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Unido a la sala. Cargando escena de juego...");
        PhotonNetwork.LoadLevel("Futbi");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Fallo al crear la sala: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Fallo al unirse a la sala: " + message);
    }
}