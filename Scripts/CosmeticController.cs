using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class CosmeticController : MonoBehaviour
{

    [SerializeField] private Cosmetic[] cosmetics;
    [SerializeField] private bool isCosmeticSelection;
    [SerializeField] private Image imageHat, imageChest, imageShoes;
    [SerializeField] private Text priceHat, priceChest, priceShoes;
    [SerializeField] private Text buyHat, buyChest, buyShoes;
    [SerializeField] private Text coinIndicator;

    private List<Cosmetic> unlockedCosmetics;
    private Cosmetic activeHat;
    private Cosmetic activeChest;
    private Cosmetic activeShoes;
    private int currentHat = 0;
    private int currentChest = 3;
    private int currentShoes;
    private SpriteRenderer rendererHat;
    private SpriteRenderer rendererChest;
    private SpriteRenderer rendererShoes;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("Coins", 10000);
        if (!PlayerPrefs.HasKey("Cosmetic_Active_Hat"))
        {
            PlayerPrefs.SetString("Cosmetics_Unlocked_" + cosmetics[0].id, "true");
            PlayerPrefs.SetString("Cosmetics_Unlocked_" + cosmetics[GetFirstIndexOf(CosmeticType.Chest)].id, "true");
            PlayerPrefs.SetString("Cosmetic_Active_Hat", cosmetics[0].id);
            PlayerPrefs.SetString("Cosmetic_Active_Chest", cosmetics[GetFirstIndexOf(CosmeticType.Chest)].id);
        }

        unlockedCosmetics = new List<Cosmetic>();
        for (int i = 0; i < cosmetics.Length; i++)
        {
            if (bool.Parse(PlayerPrefs.GetString("Cosmetic_Unlocked_" + cosmetics[i].id, "false")))
            {
                unlockedCosmetics.Add(cosmetics[i]);
            }
        }

        activeHat = cosmetics[GetIndexByID(PlayerPrefs.GetString("Cosmetic_Active_Hat"))];
        activeChest = cosmetics[GetIndexByID(PlayerPrefs.GetString("Cosmetic_Active_Chest"))];
        //activeShoes = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Shoes")];

        if (isCosmeticSelection)
        {
            currentHat = GetIndex(activeHat);
            UpdateView(CosmeticType.Hat);
            currentChest = GetIndex(activeChest);
            UpdateView(CosmeticType.Chest);
            //currentShoes = GetIndex(activeShoes);
            //UpdateView(CosmeticType.Shoes);
        }
        else
        {
            rendererHat = GameObject.Find("Player").transform.GetChild(2).GetComponent<SpriteRenderer>();
            rendererHat.transform.localScale = activeHat.preferedSizeRenderer;
            rendererHat.transform.position += new Vector3(0, activeHat.yOffset, 0);
            rendererHat.sprite = activeHat.sprite;
            rendererChest = GameObject.Find("Player").transform.GetChild(3).GetComponent<SpriteRenderer>();
            rendererChest.sprite = activeChest.sprite;
            rendererShoes = GameObject.Find("Player").transform.GetChild(4).GetComponent<SpriteRenderer>();
        }
    }

    public void Save()
    {
        unlockedCosmetics.ForEach(unlockedCosmetic =>
        {
            PlayerPrefs.SetString("Cosmetic_Unlocked_" + unlockedCosmetic.id, "true");
        });
        PlayerPrefs.SetString("Cosmetic_Active_Hat", activeHat.id);
        PlayerPrefs.SetString("Cosmetic_Active_Chest", activeChest.id);
        //PlayerPrefs.SetString("Cosmetic_Active_Shoes", activeShoes.id);
    }

    public int GetIndex(Cosmetic cosmetic)
    {
        for (int i=0; i<cosmetics.Length; i++)
        {
            if (cosmetics[i].Equals(cosmetic))
                return i;
        }
        return -1;
    }
    public int GetIndexByID(string id)
    {
        for(int i=0; i<cosmetics.Length; i++)
        {
            if (cosmetics[i].id.Equals(id))
            {
                return i;
            }
        }
        return -1;
    }
    public Cosmetic GetCosmetic(int index)
    {
        return cosmetics[index];
    }
    private int GetCosmeticCount(CosmeticType type)
    {
        int c = 0;
        for (int i=0; i<cosmetics.Length; i++)
        {
            if (cosmetics[i].type == type)
            {
                c++;
            }
        }
        return c;
    }
    private int GetFirstIndexOf(CosmeticType type)
    {
        for (int i=0; i<cosmetics.Length; i++)
        {
            if (cosmetics[i].type == type)
                return i;
        }
        return -1;
    }
    private void UpdateView(CosmeticType type)
    {
        switch (type)
        {
            case CosmeticType.Hat:
                Cosmetic newHat = cosmetics[currentHat];
                imageHat.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newHat.preferedSizeImage.x);
                imageHat.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHat.preferedSizeImage.y);
                imageHat.sprite = newHat.sprite;
                priceHat.text = string.Format("{0} [{1}]", newHat.name, newHat.price.ToString());
                if (unlockedCosmetics.Contains(newHat))
                {
                    buyHat.text = "Select";
                } else
                {
                    buyHat.text = "Buy";
                }
                break;
            case CosmeticType.Chest:
                Cosmetic newChest = cosmetics[currentChest];
                imageChest.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newChest.preferedSizeImage.x);
                imageChest.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newChest.preferedSizeImage.y);
                imageChest.sprite = newChest.preview;
                priceChest.text = string.Format("{0} [{1}]", newChest.name, newChest.price.ToString());
                if (unlockedCosmetics.Contains(newChest))
                {
                    buyChest.text = "Select";
                }
                else
                {
                    buyChest.text = "Buy";
                }
                break;
        }
    }

    void OnApplicationQuit() => Save();

    public void OnNextHat()
    {
        if (currentHat == GetCosmeticCount(CosmeticType.Hat)-1)
        {
            return;
        }
        currentHat++;
        UpdateView(CosmeticType.Hat);
    }
    public void OnLastHat()
    {
        if (currentHat == GetFirstIndexOf(CosmeticType.Hat))
        {
            return;
        }
        currentHat--;
        UpdateView(CosmeticType.Hat);
    }
    public void OnSelectHat()
    {
        Cosmetic c = cosmetics[currentHat];
        if (unlockedCosmetics.Contains(c))
        {
            activeHat = c;
        } else
        {
            if (CoinManagement.GetCoins() >= c.price)
            {
                CoinManagement.RemoveCoins(c.price);
                unlockedCosmetics.Add(c);
                activeHat = c;
                UpdateView(CosmeticType.Hat);
                coinIndicator.text = CoinManagement.GetCoins().ToString();
            }
        }
        Save();
    }

    public void OnNextChest()
    {
        if (currentChest == (GetCosmeticCount(CosmeticType.Chest) + GetCosmeticCount(CosmeticType.Hat)) - 1)
        {
            return;
        }
        currentChest++;
        UpdateView(CosmeticType.Chest);
    }
    public void OnLastChest()
    {
        if (currentChest == GetFirstIndexOf(CosmeticType.Chest))
        {
            return;
        }
        currentChest--;
        UpdateView(CosmeticType.Chest);
    }
    public void OnSelectChest()
    {
        Cosmetic c = cosmetics[currentChest];
        if (unlockedCosmetics.Contains(c))
        {
            activeChest = c;
        }
        else
        {
            if (CoinManagement.GetCoins() >= c.price)
            {
                CoinManagement.RemoveCoins(c.price);
                unlockedCosmetics.Add(c);
                activeChest = c;
                UpdateView(CosmeticType.Chest);
                coinIndicator.text = CoinManagement.GetCoins().ToString();
            }
        }
        Save();
    }

    public enum CosmeticType
    {
        Hat,
        Chest,
        Shoes
    }

}
