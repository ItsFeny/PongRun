using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    public GameObject pauseMenu, options;
    public bool isPaused;
    public AudioSource theMusic, levelMusic;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        }
    }

    public void Resume()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        theMusic.Stop();
        levelMusic.UnPause();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Return()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
