using BepInEx;
using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using GTFO.API.Utilities;

namespace InfectionDeathPenalty;

[BepInPlugin("Heaveness.InfectionDeathPenalty", "InfectionDeathPenalty", "1.0.0")]
[BepInDependency("dev.gtfomodding.gtfo-api")]
public class Plugin : BasePlugin
{
    public static ConfigEntry<float> InfectionPercentage;

    public override void Load()
    {
        InfectionPercentage = Config.Bind(
            "Settings",
            "InfectionPercentage",
            10f,
            new ConfigDescription(
                "Infection percent to apply when a player goes down. Does accept decimal values.",
                new AcceptableValueRange<float>(0f, 85f)
            )
        );

        Log.LogInfo("InfectionDeathPenalty is loaded!");
        Log.LogInfo($"Infection percentage set to: {InfectionPercentage.Value}%");

        var listener = LiveEdit.CreateListener(Paths.ConfigPath, Path.GetFileName(Config.ConfigFilePath), false);
        listener.FileChangedEventCooldown = 0.75f;
        listener.FileChanged += _ =>
        {
            Config.Reload();
            Log.LogInfo("LiveEdit: InfectionDeathPenalty Configuration reloaded");
        };

        new Harmony("Heaveness.InfectionDeathPenalty").PatchAll();
    }
}