using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordManagement : MonoBehaviour
{

    public Discord.Discord discord;

    [SerializeField] private string largeImage;
    [SerializeField] private string largeText;

    private void Start()
    {
        discord = new Discord.Discord(844148451449110528, (ulong) CreateFlags.Default);
        ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
    }

    private void Update()
    {
        discord.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Application quit");
        discord.GetActivityManager().ClearActivity((res) => { });
        discord.Dispose();
    }

    public void ApplyPresence(string smallTexture, string smallText)
    {
        Activity a = GenerateActivity(smallTexture, smallText);
        discord.GetActivityManager().UpdateActivity(a, (res) =>
        {
            if (res == Result.Ok)
                Debug.Log("Discord status was set!");
            else
                Debug.LogWarning("Settings the discord status failed!");
        });
    }
    private Activity GenerateActivity(string smallTexture, string smallText)
    {
        return new Activity
        {
            Details = "Playing " + SceneManager.GetActiveScene().name,
            State = "in the overworld...",
            Timestamps =
            {
                Start = 0
            },
            Assets =
            {
                LargeImage = largeImage,
                LargeText = largeText,
                SmallImage = smallTexture,
                SmallText = smallText,
            },
            Instance = true,
        };
    }

}
