using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse.AI.Group;

namespace SlaveRebellionsImproved.HarmonyPatches;

[HarmonyPatch(typeof(SlaveRebellionUtility), nameof(SlaveRebellionUtility.IsRebelling))]
public static class SlaveRebellionUtility_IsRebelling
{
    private static readonly MethodInfo lordJob = AccessTools.PropertyGetter(typeof(Lord), nameof(Lord.LordJob));

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions,
        ILGenerator il)
    {
        var codes = new List<CodeInstruction>(instructions);
        var label = il.DefineLabel();
        codes[codes.Count - 1].labels.Add(label);
        for (var i = 0; i < codes.Count; i++)
        {
            if (i < codes.Count - 3 && codes[i].opcode == OpCodes.Ldnull)
            {
                yield return new CodeInstruction(OpCodes.Ldnull);
                yield return new CodeInstruction(OpCodes.Cgt_Un);
                yield return new CodeInstruction(OpCodes.Dup);
                yield return new CodeInstruction(OpCodes.Brtrue_S, label);
                yield return new CodeInstruction(OpCodes.Pop);
                yield return new CodeInstruction(OpCodes.Ldloc_0);
                yield return new CodeInstruction(OpCodes.Callvirt, lordJob);
                yield return new CodeInstruction(OpCodes.Isinst, typeof(LordJob_SlaveEscape));
            }

            yield return codes[i];
        }
    }
}