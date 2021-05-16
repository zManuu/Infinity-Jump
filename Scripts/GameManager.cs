using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int levelSceneOffset;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void RequestRespawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RequestNextLevel()
    {
        int currentUnlockedLevel = PlayerPrefs.GetInt("lastUnlockedLevel", 1);
        int currentLevel = SceneManager.GetActiveScene().buildIndex - levelSceneOffset;
        if (currentLevel > currentUnlockedLevel)
        {
            PlayerPrefs.SetInt("lastUnlockedLevel", currentLevel + 1);
        }
        SceneManager.LoadScene(currentLevel + levelSceneOffset + 1);
    }

}
