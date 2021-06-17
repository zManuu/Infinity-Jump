using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelStartManager : MonoBehaviour
{

    [SerializeField] private Text levelIndicator;
    [SerializeField] private Text highscoreIndicator;
    [SerializeField] private Button startLevelButton;
    [SerializeField] private int managementSceneIndex;
    [SerializeField] private int levelSceneOffset;

    void Start()
    {
        int level = PlayerPrefs.GetInt("StartingLevel");
        levelIndicator.text = "Level " + level;
        highscoreIndicator.text = "Highscore: " + PlayerPrefs.GetInt("Highscore_" + level);
        startLevelButton.onClick.AddListener(() =>
        {
            print((levelSceneOffset + level).ToString());
            SceneManager.LoadScene(levelSceneOffset + level);
            SceneManager.LoadScene(managementSceneIndex, LoadSceneMode.Additive);

            StatsController statsController = FindObjectOfType<StatsController>();
            if (statsController != null)
            {
                statsController.time = 0f;
                statsController.paused = false;
            }
        });
    }
}
