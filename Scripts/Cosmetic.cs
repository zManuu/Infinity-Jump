using UnityEngine;
using static CosmeticController;

[CreateAssetMenu]
public class Cosmetic : ScriptableObject
{

    public CosmeticType type;
    public Sprite sprite;
    public Vector2 preferedSizeImage;
    public int price;

}
