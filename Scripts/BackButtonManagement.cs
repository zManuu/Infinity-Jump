using UnityEngine.SceneManagement;
using UnityEngine;

public class BackButtonManagement : MonoBehaviour
{

    public int startMenuIndex = 0;
    
    public void OnClick()
    {
        SceneManager.LoadScene(startMenuIndex);
    }

}
