using HarmonyLib;
using Player;

namespace InfectionDeathPenalty;

[HarmonyPatch]
internal static class Patch
{
    [HarmonyPatch(typeof(Dam_PlayerDamageBase), nameof(Dam_PlayerDamageBase.ReceiveSetDead))]
    [HarmonyPrefix]
    public static void OnPlayerDown(Dam_PlayerDamageBase __instance)
    {
        PlayerAgent player = __instance.Owner;
        if (player == null) return;

        if (!player.Alive) return;

        if (player.Owner.IsBot) return;
        if (!player.IsLocallyOwned) return;

        float currentInf = __instance.Infection;
        float maxAllowed = 0.85f;

        if (currentInf >= maxAllowed) return;

        float amountToAdd = Plugin.InfectionPercentage.Value / 100f;

        float newInfection = Math.Min(currentInf + amountToAdd, maxAllowed);

        pInfection infectionData = new()
        {
            amount = newInfection,
            mode = pInfectionMode.Set
        };

        __instance.ModifyInfection(infectionData, true, true);
    }
}