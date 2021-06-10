using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Globalization;

public class StatsController : MonoBehaviour
{

    public float time;
    public bool paused;

    public static Text timeIndicator;
    public static Coroutine timeCoroutine;
    private CultureInfo ci;

    private void Awake()
    {
        paused = false;
        this.tag = "GameManager";
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameManager");
        if (objs.Length > 1)
        {
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

}
