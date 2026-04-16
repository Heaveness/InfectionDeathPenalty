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
        if (player == null || !player.Alive || player.Owner.IsBot || !player.IsLocallyOwned) return;

        float currentInf = __instance.Infection;
        float maxAllowed = 0.85f;

        if (currentInf >= maxAllowed) return;

        float amountToAdd = Plugin.InfectionPercentage.Value / 100f;

        pInfection addInfectionData = new()
        {
            amount = amountToAdd,
            mode = pInfectionMode.Add
        };

        __instance.ModifyInfection(addInfectionData, true, true);

        if (__instance.Infection > maxAllowed)
        {
            pInfection capData = new()
            {
                amount = maxAllowed,
                mode = pInfectionMode.Set
            };
        
            __instance.ModifyInfection(capData, false, false);
        }
    }
}