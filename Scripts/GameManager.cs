using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static int deaths;

    [SerializeField] private int levelSceneOffset;
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private int settingsMenuIndex;
    [SerializeField] private int managementSceneIndex;

    private void Awake()
    {
        this.tag = "GameManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
            if (int.TryParse(SceneManager.GetActiveScene().name, out int currentLevel))
            {
                int lastUnlockedLevel = PlayerPrefs.GetInt("lastUnlockedLevel");
                if (currentLevel > lastUnlockedLevel)
                {
                    PlayerPrefs.SetInt("lastUnlockedLevel", currentLevel);
                }
            }
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        deaths = PlayerPrefs.GetInt("CurrentDeaths", 0);
        FindObjectOfType<DiscordManagement>().ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void RequestRespawn()
    {
        deaths++;
        Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(managementSceneIndex, LoadSceneMode.Additive);
    }
    public void RequestNextLevel()
    {
        deaths = 0;
        Save();
        PauseController.paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        SceneManager.LoadScene(managementSceneIndex, LoadSceneMode.Additive);
    }
    public void RequestMainMenu()
    {
        Save();
        SceneManager.LoadScene(mainMenuIndex);
    }
    public void RequestSettingsMenu()
    {
        Save();
        SceneManager.LoadScene(settingsMenuIndex);
    }

    private void Save()
    {
        PlayerPrefs.SetInt("CurrentDeaths", deaths);
        CoinManagement.Save();
    }

}
