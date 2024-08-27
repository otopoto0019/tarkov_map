namespace TarkovMap;

public class Constants
{
    public static List<string> MAPS_OF_TARKOV = new List<string>
    {
        "FACTORY",
        "WOODS",
        "CUSTOMS",
        "SHORELINE",
        "INTERCHANGE",
        "THE LAB",
        "RESERVE",
        "LIGHTHOUSE",
        "STREETS OF TARKOV",
        "GROUND ZERO"
    };
    
    public static readonly List<string> MAP_PATH_LIST = new List<string>
    {
        "factory",
        "woods",
        "customs",
        "shoreline",
        "interchange",
        "labs",
        "reserve",
        "lighthouse",
        "sot",
        "groundzero"
    };
    
    public const int MAP_GRID_HEIGHT = 32;
    public const int MAP_GRID_WIDTH = 128;

    public const int HOTKEY_ID_COMMON1 = 8000;
    public const int HOTKEY_ID_ESC = 9000;
    public const int HOTKEY_ID_F1 = 9001;
    public const uint MOD_SHIFT = 0x0004; // Shiftキー
    public const uint VK_F1 = 0x70; // F1キー
    public const uint VK_ESCAPE = 0x1B; // Espキー
    public const uint MOD_NONE = 0x0000; //None
}