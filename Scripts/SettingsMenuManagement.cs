using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuManagement : MonoBehaviour
{

    public Text musicVolumeIndicator;
    public Slider musicVolumeSlider;
    public Toggle discordRPCCheckbox;

    private void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music_Volume", 0f) * 100;
        musicVolumeIndicator.text = musicVolumeSlider.value.ToString() + "%";
        discordRPCCheckbox.isOn = bool.Parse(PlayerPrefs.GetString("Discord_RPC_Enabled", "true"));
    }

    public void OnMusicVolumeChange()
    {
        musicVolumeIndicator.text = musicVolumeSlider.value.ToString() + "%";
        PlayerPrefs.SetFloat("Music_Volume", musicVolumeSlider.value / 100);
    }

    public void OnDiscordRPCChange()
    {
        PlayerPrefs.SetString("Discord_RPC_Enabled", discordRPCCheckbox.isOn.ToString());
    }

}
