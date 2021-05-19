using UnityEngine;

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
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
