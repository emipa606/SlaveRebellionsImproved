using HarmonyLib;
using RimWorld;
using Verse;

namespace SlaveRebellionsImproved.HarmonyPatches;

[HarmonyPatch(typeof(SlaveRebellionUtility), nameof(SlaveRebellionUtility.CanParticipateInSlaveRebellion))]
public static class SlaveRebellionUtility_CanParticipateInSlaveRebellion
{
    public static void Postfix(ref bool __result, Pawn pawn)
    {
        var need_Suppression = pawn.needs.TryGetNeed<Need_Suppression>();
        if (need_Suppression != null && need_Suppression.CurLevelPercentage > SlaveMod.Instance.Settings.rebellionMin)
        {
            __result = false;
        }
    }
}