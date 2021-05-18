using UnityEngine;

public class PotionManager : MonoBehaviour
{

    public static string TAG_SPEED = "Potion_Speed";
    public static string TAG_JUMPBOOST = "Potion_JumpBoost";
    public static string TAG_REGENERATION = "Potion_Regeneration";

    public static float MULTIPLIER_SPEED = 1.5f;
    public static float MULTIPLIER_JUMPBOOST = 1.1f;
    public static float MULTIPLIER_REGENERATION = 1.5f;

    public static float TIME_SPEED = 5f;
    public static float TIME_JUMPBOOST = 5f;
    public static float TIME_REGENERATION = 5f;

    public static string TEXTURE_SPEED = "potion_speed";
    public static string TEXTURE_JUMPBOOST = "potion_jumpboost";
    public static string TEXTURE_REGENERATION = "potion_regeneration";
    public static string TEXTURE_NONE = "potion_none";

    public static string TEXT_SPEED = "Current potion: Speed";
    public static string TEXT_JUMPBOOST = "Current potion: Jumpboost";
    public static string TEXT_REGENERATION = "Current potion: Regeneration";
    public static string TEXT_NONE = "Current potion: None";

}
