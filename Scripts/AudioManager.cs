using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource musicSource;

    void Awake()
    {
        this.tag = "AudioManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("AudioManager");
        musicSource.volume = PlayerPrefs.GetFloat("Music_Volume", 0.2f);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
