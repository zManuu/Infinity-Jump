using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static int deaths = 0;

    [SerializeField] private int levelSceneOffset;
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private int settingsMenuIndex;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RequestRespawn()
    {
        deaths++;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RequestNextLevel()
    {
        deaths = 0;
        int currentUnlockedLevel = PlayerPrefs.GetInt("lastUnlockedLevel", 1);
        int currentLevel = SceneManager.GetActiveScene().buildIndex - levelSceneOffset;
        if (currentLevel > currentUnlockedLevel)
        {
            PlayerPrefs.SetInt("lastUnlockedLevel", currentLevel + 1);
        }
        SceneManager.LoadScene(currentLevel + levelSceneOffset + 1);
        PauseController.paused = false;
    }

    public void RequestMainMenu()
    {
        SceneManager.LoadScene(mainMenuIndex);
    }
    public void RequestSettingsMenu()
    {
        SceneManager.LoadScene(settingsMenuIndex);
    }

}
