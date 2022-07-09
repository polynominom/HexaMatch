using UnityEngine;

public class Constants : MonoBehaviour 
{
    public readonly static int ID_HOME = 0;
    public readonly static int ID_LEVELS = 1;
    public readonly static int ID_FIRST_LEVEL = 2;
    public readonly static int ID_EASY_RANDOM_LEVEL = 25;
    public readonly static int ID_MEDIUM_RANDOM_LEVEL = 26;
    public readonly static int ID_HARD_RANDOM_LEVEL = 27;

    public readonly static int MAX_ESSENCE = 999999;

    public readonly static string GAME_DATA_SAVE_LOCATION = "/game_data.json";
    public readonly static string STATISTICS_SAVE_LOCATION = "/statistics_v_0_85.json";

    public readonly static float CAMERA_GRAY_OFFSET = 61F;
    public readonly static float SHADOW_HEXAGONAL_GRAY_OFFSET = 109F;

    public readonly static Color UNLOCKED_HEXAGON_COLOR_NORMAL = 
        new Color(2.0F / 255.0F, 119.0F / 255.0F, 189.0F / 255.0F, 1.0F);

    public readonly static Color UNLOCKED_HEXAGON_COLOR_LIGHT =
        new Color(88.0F / 255.0F, 165.0F / 255.0F, 240.0F / 255.0F, 1.0F);

    public readonly static Color UNLOCKED_HEXAGON_COLOR_DARK =
        new Color(2.0F / 255.0F, 119.0F / 255.0F, 189.0F / 255.0F, 1.0F);

    public readonly static Color LOCKED_HEXAGON_COLOR_NORMAL =
        new Color(245.0F / 255.0F, 245.0F / 255.0F, 245.0F / 255.0F, 1.0F);

    public readonly static Color LOCKED_HEXAGON_COLOR_DARK =
        new Color(194.0F / 255.0F, 194.0F / 255.0F, 194.0F / 255.0F, 1.0F);

    public readonly static int SKILL_COST_SWAP = 50;

    public readonly static int LevelCount = 23;
}
