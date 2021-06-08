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

        CoinManagement.CoinAddEvent += (newBalance) =>
        {
            // Animation
            GameObject.Find("UI").transform.GetChild(2).GetChild(0).GetComponent<Animator>().SetTrigger("Coin_Pickup");

            // Coin indicator
            FindObjectOfType<UIContainer>().coinIndicator.text = newBalance.ToString();

            // Sound
            SoundController soundController = FindObjectOfType<SoundController>();
            soundController.Play(soundController.coinPickup);
        };
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

    public void HandlePauseMusicClick()
    {
        FindObjectOfType<AudioManager>().OnClickPause();
    }
    public void HandleNextMusicClick()
    {
        FindObjectOfType<AudioManager>().OnClickNext();
    }
    public void HandleLastMusicClick()
    {
        FindObjectOfType<AudioManager>().OnClickLast();
    }

}
