using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;

    void Awake()
    {
        this.tag = "GameManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        musicSource.volume = PlayerPrefs.GetFloat("Music_Volume", 0.2f);

        if (objs.Length > 1)
        {
            if (musicSource.isPlaying)
            {
                musicSource.UnPause();
            }
            Destroy(this.gameObject);
        }
        if (!int.TryParse(SceneManager.GetActiveScene().name, out int i))
        {
            // Code will be executed, if the scene isn't a level scene
            if (musicSource.isPlaying)
            {
                musicSource.Pause();
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void OnClickPause()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        } else
        {
            musicSource.UnPause();
        }
    }

}
