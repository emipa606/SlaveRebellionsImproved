using HarmonyLib;
using RimWorld;
using Verse;

namespace SlaveRebellionsImproved.HarmonyPatches;

[HarmonyPatch(typeof(SlaveRebellionUtility), nameof(SlaveRebellionUtility.CanParticipateInSlaveRebellion))]
public static class SlaveRebellionUtility_CanParticipateInSlaveRebellion
{
    public static void Postfix(ref bool __result, Pawn pawn)
    {
        var needSuppression = pawn.needs.TryGetNeed<Need_Suppression>();
        if (needSuppression != null && needSuppression.CurLevelPercentage > SlaveMod.Instance.Settings.RebellionMin)
        {
            __result = false;
        }
    }
}