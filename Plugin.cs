using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

[BepInPlugin("de.aeksora.sotf", "Strength Scaling", "1.0.0")]
public class Plugin : BasePlugin
{

    public const string PluginGUID = "de.aeksora.sotf";
    public const string PluginName = "Strength Scaling";
    public const string PluginVersion = "1.0.0";

    public const int percentageIncreasePerStrengthLevel = 2;

    public static Plugin Instance;
    private Harmony _harmony;

    public override void Load()
    {
        Instance = this;
        Log.LogInfo($"{PluginName} version {PluginVersion} loaded successfully");

        //_harmony = new Harmony(PluginGUID);
        //_harmony.PatchAll();

        AddComponent<ItemCapacityTester>();
    }
}