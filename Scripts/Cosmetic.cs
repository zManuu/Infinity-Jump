using UnityEngine;
using static CosmeticController;

[CreateAssetMenu]
public class Cosmetic : ScriptableObject
{

    public CosmeticType type;
    public Sprite sprite;
    public Sprite preview;
    public Vector2 preferedSizeImage;
    public int price;

    [Space(20f)]
    [Tooltip("DON'T EDIT ONCE IT IS ASSIGNED!!")]
    public string id;

    [Header("Hat Properties")]
    public Vector2 preferedSizeRenderer;
    public float yOffset;

}
