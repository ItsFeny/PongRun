using UnityEngine;

public class LobbyInitializer : MonoBehaviour
{
    void Start()
    {
        NetworkManager.Instance.ConnectToServer();
    }
}