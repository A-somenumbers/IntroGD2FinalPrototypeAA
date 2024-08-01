using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuHandler : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject UI;
    public GameObject deathMenu;
    public GameObject WinScreen;
    bool paused;
    bool dead;
    public movement m;
    public int CurrentScene;
    // Start is called before the first frame update
    private void Start()
    {
        UI.SetActive(true);
        pauseMenu.SetActive(false);
        deathMenu.SetActive(false);
        WinScreen.SetActive(false);
        paused = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !dead)
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
        if (m.health <= 0)
        {
            Death();
        }
        if(m.winCondition == true)
        {
            win();
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        UI.SetActive(false);
        deathMenu.SetActive(false);
        WinScreen.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        UI.SetActive(true);
        deathMenu.SetActive(false);
        WinScreen.SetActive(false);
        Time.timeScale = 1.0f;
        paused = false;
        dead = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Death()
    {
        pauseMenu.SetActive(false);
        UI.SetActive(false);
        deathMenu.SetActive(true);
        WinScreen.SetActive(false);
        Time.timeScale = 0f;
        dead = true;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
    public void win()
    {
        pauseMenu.SetActive(false);
        UI.SetActive(false);
        deathMenu.SetActive(false);
        WinScreen.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void mainMenuButton()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        UI.SetActive(false);
        deathMenu.SetActive(false);
        WinScreen.SetActive(false);
        m.winCondition = false;
    }
    public void Reload()
    {
        SceneManager.LoadScene(CurrentScene);
        Time.timeScale = 1f;
        m.winCondition = false;
        pauseMenu.SetActive(false);
        UI.SetActive(true);
        deathMenu.SetActive(false);
        WinScreen.SetActive(false);
    }

}
