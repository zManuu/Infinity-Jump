using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static int deaths;

    [SerializeField] private int levelSceneOffset;
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private int settingsMenuIndex;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        deaths = PlayerPrefs.GetInt("CurrentDeaths", 0);
        FindObjectOfType<DiscordManagement>().ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
    }

    private void OnApplicationQuit()
    {
        SaveDeaths();
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
        SaveDeaths();
        SceneManager.LoadScene(mainMenuIndex);
    }
    public void RequestSettingsMenu()
    {
        SaveDeaths();
        SceneManager.LoadScene(settingsMenuIndex);
    }

    private void SaveDeaths()
    {
        PlayerPrefs.SetInt("CurrentDeaths", deaths);
    }

}
