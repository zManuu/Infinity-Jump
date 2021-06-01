using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectionMenuManagement : MonoBehaviour
{

    public Transform buttonContainer;
    public char lockedChar;
    public int levelSceneOffset;
    public bool clearLastUnlockedLevel;
    [SerializeField] private int managementSceneIndex;

    public int levelCount;

    private int lastUnlockedLevel;
    private Transform[] buttons;

    private void Start()
    {
        if (clearLastUnlockedLevel)
        {
            PlayerPrefs.SetInt("lastUnlockedLevel", 1);
        }
        lastUnlockedLevel = PlayerPrefs.GetInt("lastUnlockedLevel", 1);
        Debug.Log(lastUnlockedLevel);
        buttons = new Transform[buttonContainer.childCount];
        for (int i=0; i<buttonContainer.childCount; i++)
        {
            buttons[i] = buttonContainer.GetChild(i);
        }
        LockButtons();
    }

    private void LockButtons()
    {
        for (int i = levelCount - 1; i > lastUnlockedLevel - 1; i--)
        {
            Transform button = buttons[i];
            button.GetComponent<Button>().enabled = false;
            Text textChildren = button.GetChild(0).GetComponent<Text>();
            textChildren.text = new string(lockedChar, 1);
            if (i > 8)
            {
                Text defaultText = buttons[0].GetChild(0).GetComponent<Text>();
                textChildren.alignment = defaultText.alignment;
                textChildren.fontSize = defaultText.fontSize;
                textChildren.alignByGeometry = false;
            }
        }
    }

    public void OnButtonClick(int levelIndex)
    {
        SceneManager.LoadScene(levelSceneOffset + levelIndex);
        SceneManager.LoadScene(managementSceneIndex, LoadSceneMode.Additive);
    }

}
