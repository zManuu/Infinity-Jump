using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CosmeticController : MonoBehaviour
{

    [Tooltip("0-9: hats\n10-19: caps\n20-29: shoes")] [SerializeField]
    private Cosmetic[] cosmetics;

    [SerializeField] private bool isCosmeticSelection;
    [SerializeField] private Image imageHat, imageCape, imageShoes;
    [SerializeField] private Text priceHat, priceCape, priceShoes;

    private List<Cosmetic> unlockedCosmetics;
    private Cosmetic activeHat;
    private Cosmetic activeCape;
    private Cosmetic activeShoes;
    private int currentHat = 1;
    private int currentCape;
    private int currentShoes;
    private SpriteRenderer rendererHat;
    private SpriteRenderer rendererCape;
    private SpriteRenderer rendererShoes;

    private void Start()
    {
        unlockedCosmetics = new List<Cosmetic>();
        for (int i=0; i<30; i++)
        {
            if (bool.Parse(PlayerPrefs.GetString("Cosmetic_Unlocked_" + i, "false")))
            {
                unlockedCosmetics.Add(cosmetics[i]);
            }
        }

        activeHat = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Hat")];
        //activeCape = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Cape")];
        //activeShoes = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Shoes")];

        if (isCosmeticSelection)
        {
            currentHat = GetIndex(activeHat);
            UpdateView(CosmeticType.Hat);
            //currentCape = GetIndex(activeCape);
            //UpdateView(CosmeticType.Cape);
            //currentShoes = GetIndex(activeShoes);
            //UpdateView(CosmeticType.Shoes);
        }
        else
        {
            rendererHat = GameObject.Find("Player").transform.GetChild(2).GetComponent<SpriteRenderer>();
            rendererHat.sprite = activeHat.sprite;
            //rendererCape = GameObject.Find("Player").transform.GetChild(3).GetComponent<SpriteRenderer>();
            //rendererShoes = GameObject.Find("Player").transform.GetChild(4).GetComponent<SpriteRenderer>();
        }
    }

    public void Save()
    {
        unlockedCosmetics.ForEach(unlockedCosmetic =>
        {
            PlayerPrefs.SetString("Cosmetic_Unlocked_" + GetIndex(unlockedCosmetic), "true");
        });
        PlayerPrefs.SetInt("Cosmetic_Active_Hat", GetIndex(activeHat));
        PlayerPrefs.SetInt("Cosmetic_Active_Cape", GetIndex(activeCape));
        PlayerPrefs.SetInt("Cosmetic_Active_Shoes", GetIndex(activeShoes));
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
        if (currentHat == 1)
        {
            return;
        }
        currentHat--;
        UpdateView(CosmeticType.Hat);
    }
    public void OnSelectHat()
    {
        activeHat = cosmetics[currentHat];
        Save();
    }

    public enum CosmeticType
    {
        Hat,
        Cape,
        Shoes
    }

}
