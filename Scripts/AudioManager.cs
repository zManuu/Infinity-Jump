using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public AudioSource playingSong;
    public AudioSource[] songs;

    private int currentSong;

    void Awake()
    {
        this.tag = "GameManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        } else
        {
            // Code will be executed, if this script is loaded for the first time
            PlaySong();
        }
        if (!int.TryParse(SceneManager.GetActiveScene().name, out int i))
        {
            // Code will be executed, if the scene isn't a level scene
            if (playingSong.isPlaying)
            {
                playingSong.Pause();
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void OnClickPause()
    {
        if (playingSong.isPlaying)
        {
            playingSong.Pause();
        } else
        {
            playingSong.UnPause();
        }
    }

    public void OnClickNext()
    {
        if (currentSong >= songs.Length - 1)
        {
            PlayerPrefs.SetInt("Music_Song_ID", songs.Length - 1);
            return;
        }
        Debug.Log("[Music] Stopping: " + currentSong);
        playingSong.Stop();
        PlayerPrefs.SetInt("Music_Song_ID", currentSong + 1);
        PlaySong();
    }

    public void OnClickLast()
    {
        if (currentSong <= 0)
        {
            PlayerPrefs.SetInt("Music_Song_ID", 0);
            return;
        }
        Debug.Log("[Music] Stopping: " + currentSong);
        playingSong.Stop();
        PlayerPrefs.SetInt("Music_Song_ID", currentSong - 1);
        PlaySong();
    }

    private void PlaySong()
    {
        currentSong = PlayerPrefs.GetInt("Music_Song_ID", 0);
        Debug.Log("[Music] Playing: " + currentSong);
        playingSong = songs[currentSong];
        playingSong.volume = PlayerPrefs.GetFloat("Music_Volume", 0.2f);
        playingSong.Play();
    }

}
