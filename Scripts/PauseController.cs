using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{

    public static bool paused = false;

    private static Transform pauseMenuContainer;

    private void Start()
    {
        Debug.Log("Starting...");
        pauseMenuContainer = FindObjectOfType<UIContainer>().pauseMenuContainer;
        Debug.Log(pauseMenuContainer.GetInstanceID());
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

}
