using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuPause : MonoBehaviourPunCallbacks
{
    public GameObject pauseMenu, options;
    public bool isPaused;
    public AudioSource theMusic, levelMusic;

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            if (options.active)
            {
                options.SetActive(false);
            }
        }
    }

    public void Pause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            levelMusic.Pause();
            theMusic.Play();
            photonView.RPC("PauseGame", RpcTarget.All);
        }
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        theMusic.Stop();
        levelMusic.UnPause();
        photonView.RPC("ResumeGame", RpcTarget.All);
    }

    [PunRPC]
    void PauseGame()
    {
        Time.timeScale = 0f;
    }

    [PunRPC]
    void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        photonView.RPC("RestartGame", RpcTarget.All);
    }

    [PunRPC]
    void RestartGame()
    {
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void Return()
    {
        Time.timeScale = 1f;
        photonView.RPC("ReturnToMainMenu", RpcTarget.All);
    }

    [PunRPC]
    void ReturnToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Main Menu");
    }

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
}