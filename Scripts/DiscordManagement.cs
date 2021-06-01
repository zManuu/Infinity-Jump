using UnityEngine;
using Discord;
using UnityEngine.SceneManagement;
using System;

public class DiscordManagement : MonoBehaviour
{

    private static Discord.Discord discord;
    private static ActivityManager activityManager;
    private static long timestamp;

    public static bool rpcEnabled;
    public static bool rpcSet;

    [SerializeField] private string largeImage;
    [SerializeField] private string largeText;
    public bool test;

    private void Awake()
    {
        this.tag = "GameManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
            Debug.Log("Test");
            rpcEnabled = bool.Parse(PlayerPrefs.GetString("Discord_RPC_Enabled", "false"));
            if (rpcEnabled)
            {
                ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
            }
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        try
        {
            rpcEnabled = bool.Parse(PlayerPrefs.GetString("Discord_RPC_Enabled", "false"));
            Debug.Log(rpcEnabled);
            if (rpcEnabled)
            {
                timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                discord = new Discord.Discord(844148451449110528, (ulong)CreateFlags.NoRequireDiscord);
                if (discord != null)
                    activityManager = discord.GetActivityManager();
                ApplyPresence(PotionManager.TEXTURE_NONE, PotionManager.TEXT_NONE);
            }
            else
            {
                discord = null;
            }
        } catch (Exception e) {
            Debug.Log(e.StackTrace);
            discord = null;
        }
    }

    private void Update()
    {
        if (discord != null && rpcEnabled)
            discord.RunCallbacks();
        test = rpcEnabled;
    }

    public void OnApplicationQuit()
    {
        ClearRPC();
    }

    public void ClearRPC()
    {
        try
        {
            Debug.Log("Clearing RPC...");
            if (discord != null && rpcEnabled)
            {
                activityManager.ClearActivity((res) => { });
                discord.Dispose();
            }
        } catch (Exception e) { Debug.Log(e.StackTrace); }
    }

    public void ApplyPresence(string smallTexture, string smallText)
    {
        if (discord != null && rpcEnabled)
            return;

        Activity a = GenerateActivity(smallTexture, smallText);
        activityManager.UpdateActivity(a, (res) =>
        {
            /*if (res == Result.Ok)
                Debug.Log(string.Format("Discord status was set! [{0} | {1}]", SceneManager.GetActiveScene().name, smallText));
            else
                Debug.LogWarning("Settings the discord status failed!");*/
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
                Start = timestamp
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
