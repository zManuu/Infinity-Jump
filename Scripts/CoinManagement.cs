using System;
using UnityEngine;
using UnityEngine.UI;

public class CoinManagement : MonoBehaviour
{

    private static int coins;
    public static Action<int> CoinAddEvent;

    [SerializeField] public Text coinIndicator;

    private void Start()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);

        if (coinIndicator == null)
        {
            coinIndicator = FindObjectOfType<UIContainer>().coinIndicator;
        }
        coinIndicator.text = coins.ToString();
    }

    public static int GetCoins()
    {
        return coins;
    }
    public static void AddCoins(int amount)
    {
        coins += amount;
        CoinAddEvent?.Invoke(coins);
    }
    public static void Save()
    {
        PlayerPrefs.SetInt("Coins", coins);
    }

    private void OnApplicationQuit()
    {
        Save();
    }

}
