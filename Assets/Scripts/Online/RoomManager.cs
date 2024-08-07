using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField createRoomInput;
    public TMP_InputField joinRoomInput;

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomInput.text))
        {
            Debug.LogWarning("El nombre de la sala no puede estar vac�o");
            return;
        }
        NetworkManager.Instance.CreateRoom(createRoomInput.text);
    }

    public void JoinRoom()
    {
        if (string.IsNullOrEmpty(joinRoomInput.text))
        {
            Debug.LogWarning("El nombre de la sala no puede estar vac�o");
            return;
        }
        NetworkManager.Instance.JoinRoom(joinRoomInput.text);
    }
}