using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static int deaths;

    [SerializeField] private int levelSceneOffset;
    [SerializeField] private int mainMenuIndex;
    [SerializeField] private int settingsMenuIndex;
    [SerializeField] private int managementSceneIndex;

    private Text deathsIndicator;
    private bool deathCalled;

    private void Awake()
    {
        deathsIndicator = GameObject.Find("UI").transform.GetChild(2).GetChild(1).GetChild(2).GetComponent<Text>();
        if (deaths == 0)
        {
            deaths = PlayerPrefs.GetInt("CurrentDeaths", 0);
        }
        deathCalled = false;
        deathsIndicator.text = deaths.ToString();
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

    public IEnumerator RequestRespawn()
    {
        if (!deathCalled)
        {
            GameObject.Find("UI").GetComponent<Animator>().SetTrigger("TransitionEnd");
            deathCalled = true;
            deaths++;
            Save();
            yield return new WaitForSeconds(1f);
            deathCalled = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(managementSceneIndex, LoadSceneMode.Additive);
        }
    }
    public IEnumerator RequestNextLevel()
    {
        GameObject.Find("UI").GetComponent<Animator>().SetTrigger("TransitionEnd");
        deaths = 0;
        Save();
        PauseController.paused = false;
        yield return new WaitForSeconds(1f);
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
