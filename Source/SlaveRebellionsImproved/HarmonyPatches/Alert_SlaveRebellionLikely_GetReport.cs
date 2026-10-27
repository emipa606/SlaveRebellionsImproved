using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SlaveRebellionsImproved.HarmonyPatches;

[HarmonyPatch(typeof(Alert_SlaveRebellionLikely), nameof(Alert_SlaveRebellionLikely.GetReport))]
public static class Alert_SlaveRebellionLikely_GetReport
{
    public static bool Prefix(ref AlertReport __result)
    {
        var currentMap = Find.CurrentMap;
        if (!ModsConfig.IdeologyActive || currentMap == null)
        {
            __result = false;
            return false;
        }

        var list = new List<Pawn>();
        foreach (var item in currentMap.mapPawns.SlavesOfColonySpawned.Where(pawn =>
                     mtbMeetsRebelliousThreshold(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn))))
        {
            list.Add(item);
        }

        if (list.Count < 1)
        {
            __result = false;
            return false;
        }

        __result = AlertReport.CulpritsAre(list);
        return false;
    }

    private static bool mtbMeetsRebelliousThreshold(float mtb)
    {
        if (15f > mtb)
        {
            return mtb > 0f;
        }

        return false;
    }
}