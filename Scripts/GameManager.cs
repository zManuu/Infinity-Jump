using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static int deaths = 0;

    [SerializeField] private int levelSceneOffset;

    private static DiscordManagement discordManagement;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        discordManagement = FindObjectOfType<DiscordManagement>();
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
    }

}
