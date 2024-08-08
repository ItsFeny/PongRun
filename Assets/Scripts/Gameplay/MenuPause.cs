using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MenuPause : MonoBehaviourPunCallbacks
{
    public GameObject pauseMenu, options;
    public bool isPaused;
    public AudioSource theMusic, levelMusic;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }

            if (options.activeSelf)
            {
                options.SetActive(false);
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        photonView.RPC("ShowPauseMenu", RpcTarget.All);
        levelMusic.Pause();
        theMusic.Play();
        photonView.RPC("PauseGame", RpcTarget.All);
    }

    public void Resume()
    {
        isPaused = false;
        photonView.RPC("HidePauseMenu", RpcTarget.All);
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

    [PunRPC]
    void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    [PunRPC]
    void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
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
}
