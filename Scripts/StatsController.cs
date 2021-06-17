using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Globalization;

public class StatsController : MonoBehaviour
{

    [SerializeField] private int levelSceneOffset;

    public float time;
    public bool paused;
    public float levelHighscore;

    private static Text timeIndicator;
    private Coroutine timeCoroutine;
    private CultureInfo ci;

    private void Awake()
    {
        this.tag = "GameManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
            paused = false;
            time = 0;
            timeIndicator = FindObjectOfType<UIContainer>().timeIndicator;
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        ci = new CultureInfo("de-DE");
        time = 0;
        timeIndicator = FindObjectOfType<UIContainer>().timeIndicator;
        timeCoroutine = StartCoroutine(TimeCoroutine());
    }

    public IEnumerator TimeCoroutine()
    {
        while (true)
        {
            if (!paused)
            {
                time += 0.2f;
                timeIndicator.text = time.ToString("N02", ci) + "s";
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public string GetPlayerPrefsPath() => "Highscore_" + (SceneManager.GetActiveScene().buildIndex - levelSceneOffset);

    public void RequestHighscoreUpdate()
    {
        if (!PlayerPrefs.HasKey(GetPlayerPrefsPath()))
        {
            PlayerPrefs.SetFloat(GetPlayerPrefsPath(), levelHighscore);
        }

        float lastHighscore = PlayerPrefs.GetFloat(GetPlayerPrefsPath());
        if (levelHighscore < lastHighscore)
        {
            PlayerPrefs.SetFloat(GetPlayerPrefsPath(), levelHighscore);
        }
    }

}
