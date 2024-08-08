using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject Player1;
    public GameObject Player2;
    public Transform[] spawnPoints;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayerAtPosition(Player1, 0, false, -4f, 0f); 
        }
        else
        {
            SpawnPlayerAtPosition(Player2, 1, true, 0f, 4f); 
        }
    }

    void SpawnPlayerAtPosition(GameObject prefab, int spawnIndex, bool faceLeft, float leftLimit, float rightLimit)
    {
        if (prefab == null)
        {
            Debug.LogError("Player prefab is not assigned in PlayerSpawner!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned in PlayerSpawner!");
            return;
        }

        if (spawnIndex < spawnPoints.Length)
        {
            Vector3 spawnPosition = spawnPoints[spawnIndex].position;
            GameObject player = PhotonNetwork.Instantiate(prefab.name, spawnPosition, Quaternion.identity);
            PlayerController playerController = player.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.faceLeft = faceLeft;
                playerController.leftLimit = leftLimit;
                playerController.rightLimit = rightLimit;
            }
        }
        else
        {
            Debug.LogError("Invalid spawn point index!");
        }
    }
}
