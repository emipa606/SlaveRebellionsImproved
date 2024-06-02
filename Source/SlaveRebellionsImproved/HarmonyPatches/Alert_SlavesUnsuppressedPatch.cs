using HarmonyLib;
using RimWorld;
using Verse;

namespace SlaveRebellionsImproved.HarmonyPatches;

[HarmonyPatch(typeof(Alert_SlavesUnsuppressed), nameof(Alert_SlavesUnsuppressed.GetExplanation))]
public static class Alert_SlavesUnsuppressedPatch
{
    public static bool Prefix(ref TaggedString __result, Alert_SlavesUnsuppressed __instance)
    {
        if (__instance.Targets.Count <= 1)
        {
            return true;
        }

        var text = "";
        foreach (var target in __instance.Targets)
        {
            text = string.Concat(text, "\n" + target.NameShortColored.Colorize(ColoredText.NameColor));
        }

        __result = "NewSlavesUnsuppressedDesc".Translate(text);
        return false;
    }
}