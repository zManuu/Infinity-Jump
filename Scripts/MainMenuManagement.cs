using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagement : MonoBehaviour
{

    public int LevelSelectionIndex;
    public int SettingsIndex;
    public int CustomizationIndex;
    public int CreditsIndex;

    public void OpenLevelSelection()
    {
        SceneManager.LoadScene(LevelSelectionIndex);
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene(SettingsIndex);
    }

    public void OpenCustomization()
    {
        SceneManager.LoadScene(CustomizationIndex);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(CreditsIndex);
    }

    public void QuitApp()
    {
        Debug.Log("Quitting application...");
        Application.Quit();
    }

}
