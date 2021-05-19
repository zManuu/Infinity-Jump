using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;

public class DiscordManagement : MonoBehaviour
{

    private static Discord.Discord discord;
    private static ActivityManager activityManager;

    [SerializeField] private string largeImage;
    [SerializeField] private string largeText;

    private void Awake()
    {
        this.tag = "GameManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
            ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        discord = new Discord.Discord(844148451449110528, (ulong)CreateFlags.Default);
        activityManager = discord.GetActivityManager();
        ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
    }

    private void Update()
    {
        discord.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        activityManager.ClearActivity((res) => { });
        discord.Dispose();
    }

    public void ApplyPresence(string smallTexture, string smallText)
    {
        Activity a = GenerateActivity(smallTexture, smallText);
        activityManager.UpdateActivity(a, (res) =>
        {
            if (res == Result.Ok)
                Debug.Log(string.Format("Discord status was set! [{0} | {1}]", SceneManager.GetActiveScene().name, smallText));
            else
                Debug.LogWarning("Settings the discord status failed!");
        });
    }
    private Activity GenerateActivity(string smallTexture, string smallText)
    {
        return new Activity
        {
            Details = "Level: " + SceneManager.GetActiveScene().name,
            State = "Deaths: " + GameManager.deaths,
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
