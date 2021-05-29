using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CosmeticController : MonoBehaviour
{

    [Tooltip("0-9: hats\n10-19: caps\n20-29: shoes")]
    public Cosmetic[] cosmetics;
    public bool isCosmeticSelection;
    public Transform menuContainerHat;
    public Transform menuContainerCape;
    public Transform menuContainerShoes;

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
        Debug.Log("Active hat: " + activeHat.name);
        //activeCape = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Cape")];
        //activeShoes = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Shoes")];
        if (!isCosmeticSelection)
            ActivateCosmetics();
    }

    public void ActivateCosmetics()
    {
        rendererHat = GameObject.Find("Player").transform.GetChild(2).GetComponent<SpriteRenderer>();
        //rendererCape = GameObject.Find("Player").transform.GetChild(3).GetComponent<SpriteRenderer>();
        //rendererShoes = GameObject.Find("Player").transform.GetChild(4).GetComponent<SpriteRenderer>();
        rendererHat.sprite = activeHat.sprite;
        //rendererCape.sprite = activeCape.sprite;
        //rendererShoes.sprite = activeShoes.sprite;
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

    void OnApplicationQuit() => Save();

    public void OnNextHat()
    {
        if (currentHat == GetCosmeticCount(CosmeticType.Hat)-1)
        {
            return;
        }
        currentHat++;
        menuContainerHat.GetChild(0).GetComponent<Image>().sprite = cosmetics[currentHat].sprite;
    }
    public void OnLastHat()
    {
        if (currentHat == 1)
        {
            return;
        }
        currentHat--;
        menuContainerHat.GetChild(0).GetComponent<Image>().sprite = cosmetics[currentHat].sprite;
    }
    public void OnSelectHat()
    {
        activeHat = cosmetics[currentHat];
        Save();
        Debug.Log("Saving current hat as " + activeHat.name);
    }

    public enum CosmeticType
    {
        Hat,
        Cape,
        Shoes
    }

}
