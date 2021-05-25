using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public static bool paused = false;

    private static Transform pauseMenuContainer;

    private void Start()
    {
        pauseMenuContainer = FindObjectOfType<UIContainer>().pauseMenuContainer;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            pauseMenuContainer.gameObject.SetActive(paused);
            if (paused)
            {
                Cursor.lockState = CursorLockMode.None;
                Time.timeScale = 0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
            }
        }
    }

    public void HandleResumeClick()
    {
        paused = false;
        pauseMenuContainer.gameObject.SetActive(paused);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }

    public void HandleSettingsClick()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        FindObjectOfType<GameManager>().RequestSettingsMenu();
    }

    public void HandleQuitClick()
    {
        paused = false;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
        FindObjectOfType<GameManager>().RequestMainMenu();
    }

}
