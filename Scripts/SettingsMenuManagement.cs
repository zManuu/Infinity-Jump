using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManagement : MonoBehaviour
{

    public Text musicVolumeIndicator;
    public Slider musicVolumeSlider;

    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music_Volume", 0.01f) * 100;
    }

    public void OnMusicVolumeChange()
    {
        musicVolumeIndicator.text = musicVolumeSlider.value.ToString() + "%";
        PlayerPrefs.SetFloat("Music_Volume", musicVolumeSlider.value / 100);
    }

}
