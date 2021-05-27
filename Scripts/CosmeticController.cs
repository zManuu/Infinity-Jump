using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CosmeticController : MonoBehaviour
{

    [Tooltip("0-9: hats\n10-19: caps\n20-29: shoes")]
    public Cosmetic[] cosmetics;
    public SpriteRenderer rendererHat;
    public SpriteRenderer rendererCape;
    public SpriteRenderer rendererShoes;
    public bool isCosmeticSelection;

    private List<Cosmetic> unlockedCosmetics;
    private Cosmetic activeHat;
    private Cosmetic activeCape;
    private Cosmetic activeShoes;

    private void Start()
    {
        PlayerPrefs.SetString("Cosmetic_Unlocked_" + 1, "true");
        PlayerPrefs.SetInt("Cosmetic_Active_Hat", 1);
        unlockedCosmetics = new List<Cosmetic>();
        for (int i=0; i<30; i++)
        {
            if (bool.Parse(PlayerPrefs.GetString("Cosmetic_Unlocked_" + i, "false")))
            {
                unlockedCosmetics.Add(cosmetics[i]);
            }
        }
        activeHat = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Hat")];
        activeCape = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Cape")];
        activeShoes = cosmetics[PlayerPrefs.GetInt("Cosmetic_Active_Shoes")];
        ActivateCosmetics();
    }

    public void ActivateCosmetics()
    {
        rendererHat.sprite = activeHat.sprite;
        rendererCape.sprite = activeCape.sprite;
        rendererShoes.sprite = activeShoes.sprite;
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

    private void OnApplicationQuit()
    {
        unlockedCosmetics.ForEach(unlockedCosmetic =>
        {
            PlayerPrefs.SetString("Cosmetic_Unlocked_" + GetIndex(unlockedCosmetic), "true");
        });
        PlayerPrefs.SetInt("Cosmetic_Active_Hat", GetIndex(activeHat));
        PlayerPrefs.SetInt("Cosmetic_Active_Cape", GetIndex(activeCape));
        PlayerPrefs.SetInt("Cosmetic_Active_Shoes", GetIndex(activeShoes));
    }

    public enum CosmeticType
    {
        Hat,
        Cape,
        Shoes
    }

}
