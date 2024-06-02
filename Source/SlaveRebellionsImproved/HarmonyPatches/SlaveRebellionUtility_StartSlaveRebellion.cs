using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace SlaveRebellionsImproved.HarmonyPatches;

[HarmonyPatch(typeof(SlaveRebellionUtility), nameof(SlaveRebellionUtility.StartSlaveRebellion), [
    typeof(Pawn),
    typeof(string),
    typeof(string),
    typeof(LetterDef),
    typeof(LookTargets),
    typeof(bool)
], [
    ArgumentType.Normal,
    ArgumentType.Out,
    ArgumentType.Out,
    ArgumentType.Out,
    ArgumentType.Out,
    ArgumentType.Normal
])]
public class SlaveRebellionUtility_StartSlaveRebellion
{
    public static bool Prefix(ref bool __result, List<Pawn> ___rebellingSlaves,
        List<Pawn> ___allPossibleRebellingSlaves, Pawn initiator, out string letterText, out string letterLabel,
        out LetterDef letterDef, out LookTargets lookTargets, bool forceAggressive = false)
    {
        letterText = null;
        letterLabel = null;
        letterDef = null;
        lookTargets = null;
        if (!ModLister.CheckIdeology("Slave rebellion"))
        {
            __result = false;
            return false;
        }

        ___rebellingSlaves.Clear();
        ___rebellingSlaves.Add(initiator);
        ___allPossibleRebellingSlaves.Clear();
        var slavesOfColonySpawned = initiator.Map.mapPawns.SlavesOfColonySpawned;
        foreach (var pawn in slavesOfColonySpawned)
        {
            if (pawn != initiator && SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
            {
                ___allPossibleRebellingSlaves.Add(pawn);
            }
        }

        var list = new List<Pawn>();
        foreach (var pawn in slavesOfColonySpawned)
        {
            if (pawn != initiator && OriginalCanParticipateInSlaveRebellion(pawn))
            {
                list.Add(pawn);
            }
        }

        var count = ___allPossibleRebellingSlaves.Count;
        var count2 = slavesOfColonySpawned.Count;
        var slaveRebellionType = !(count > count2 * 0.5)
            ? SlaveRebellionType.LocalRebellion
            : SlaveRebellionType.GrandRebellion;
        switch (slaveRebellionType)
        {
            case SlaveRebellionType.GrandRebellion:
            {
                foreach (var pawn in list)
                {
                    var need_Suppression = pawn.needs.TryGetNeed<Need_Suppression>();
                    if (need_Suppression != null &&
                        need_Suppression.CurLevelPercentage < SlaveMod.Instance.Settings.rebellionLoyal)
                    {
                        ___rebellingSlaves.Add(pawn);
                    }
                }

                break;
            }
            case SlaveRebellionType.LocalRebellion:
            {
                foreach (var pawn in ___allPossibleRebellingSlaves)
                {
                    if (!(initiator.Position.DistanceTo(pawn.Position) > 25f))
                    {
                        ___rebellingSlaves.Add(pawn);
                    }
                }

                break;
            }
        }

        if (___rebellingSlaves.Count == 1)
        {
            slaveRebellionType = SlaveRebellionType.SingleRebellion;
        }
        else if (___rebellingSlaves.Count > count * 0.5)
        {
            slaveRebellionType = SlaveRebellionType.GrandRebellion;
        }

        if (!RCellFinder.TryFindRandomExitSpot(initiator, out var spot, TraverseMode.PassDoors))
        {
            __result = false;
            return false;
        }

        if (!PrisonBreakUtility.TryFindGroupUpLoc(___rebellingSlaves, spot, out var groupUpLoc))
        {
            __result = false;
            return false;
        }

        var isAggressive = false;
        if (forceAggressive)
        {
            isAggressive = true;
        }
        else if (slaveRebellionType == SlaveRebellionType.SingleRebellion)
        {
            if (initiator.Map.mapPawns.FreeColonistsSpawnedCount < 3)
            {
                isAggressive = CanApplyWeaponFactor(initiator);
            }
        }
        else
        {
            var num = 0;
            foreach (var pawn in ___rebellingSlaves)
            {
                num += CanApplyWeaponFactor(pawn) ? 1 : 0;
            }

            if (num > initiator.Map.mapPawns.FreeColonistsSpawnedCount * 0.4)
            {
                isAggressive = true;
            }
        }

        switch (slaveRebellionType)
        {
            case SlaveRebellionType.GrandRebellion:
                if (isAggressive)
                {
                    letterLabel = "LetterLabelGrandSlaveRebellion".Translate();
                    letterText = "LetterGrandSlaveRebellion".Translate(GenLabel.ThingsLabel(___rebellingSlaves));
                }
                else
                {
                    letterLabel = "LetterLabelGrandSlaveEscape".Translate();
                    letterText = "LetterGrandSlaveEscape".Translate(GenLabel.ThingsLabel(___rebellingSlaves));
                }

                break;
            case SlaveRebellionType.LocalRebellion:
                if (isAggressive)
                {
                    letterLabel = "LetterLabelLocalSlaveRebellion".Translate();
                    letterText =
                        "LetterLocalSlaveRebellion".Translate(initiator, GenLabel.ThingsLabel(___rebellingSlaves));
                }
                else
                {
                    letterLabel = "LetterLabelLocalSlaveEscape".Translate();
                    letterText =
                        "LetterLocalSlaveEscape".Translate(initiator, GenLabel.ThingsLabel(___rebellingSlaves));
                }

                break;
            case SlaveRebellionType.SingleRebellion:
                if (isAggressive)
                {
                    letterLabel = "LetterLabelSingleSlaveRebellion".Translate() + (": " + initiator.LabelShort);
                    letterText = "LetterSingleSlaveRebellion".Translate(initiator);
                }
                else
                {
                    letterLabel = "LetterLabelSingleSlaveEscape".Translate() + (": " + initiator.LabelShort);
                    letterText = "LetterSingleSlaveEscape".Translate(initiator);
                }

                break;
        }

        letterText += "\n\n" + "SlaveRebellionSuppressionExplanation".Translate();
        lookTargets = new LookTargets(___rebellingSlaves);
        letterDef = LetterDefOf.ThreatBig;
        var sapperThingID = -1;
        if (Rand.Value < 0.5f)
        {
            sapperThingID = initiator.thingIDNumber;
        }

        foreach (var pawn in ___rebellingSlaves)
        {
            pawn.GetLord()?.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord);
        }

        if (!isAggressive)
        {
            LordMaker.MakeNewLord(___rebellingSlaves[0].Faction, new LordJob_SlaveEscape(groupUpLoc, spot),
                initiator.Map, ___rebellingSlaves);
        }
        else
        {
            LordMaker.MakeNewLord(___rebellingSlaves[0].Faction,
                new LordJob_SlaveRebellion(groupUpLoc, spot, sapperThingID, false), initiator.Map,
                ___rebellingSlaves);
        }

        foreach (var pawn in ___rebellingSlaves)
        {
            if (!pawn.Awake())
            {
                RestUtility.WakeUp(pawn);
            }

            pawn.drafter.Drafted = false;
            if (pawn.CurJob != null)
            {
                pawn.jobs.EndCurrentJob(JobCondition.Ongoing | JobCondition.Incompletable);
            }

            pawn.Map.attackTargetsCache.UpdateTarget(pawn);
            if (pawn.carryTracker.CarriedThing != null)
            {
                pawn.carryTracker.TryDropCarriedThing(pawn.Position,
                    ThingPlaceMode.Near, out _);
            }
        }

        ___rebellingSlaves.Clear();
        __result = true;
        return false;
    }

    private static bool OriginalCanParticipateInSlaveRebellion(Pawn pawn)
    {
        if (!pawn.Downed && pawn.Spawned && pawn.IsSlave && !pawn.InMentalState && pawn.Awake())
        {
            return !SlaveRebellionUtility.IsRebelling(pawn);
        }

        return false;
    }

    private static bool CanApplyWeaponFactor(Pawn pawn)
    {
        var primary = pawn.equipment.Primary;
        if (primary == null || !primary.def.IsWeapon || !SlaveRebellionUtility.WeaponUsableInRebellion(primary))
        {
            return GoodWeaponInSameRoom(pawn);
        }

        return true;
    }

    private static bool GoodWeaponInSameRoom(Pawn pawn)
    {
        var room = pawn.GetRoom();
        if (room == null || room.PsychologicallyOutdoors)
        {
            return false;
        }

        var thingReq = ThingRequest.ForGroup(ThingRequestGroup.Weapon);
        return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, thingReq, PathEndMode.Touch,
            TraverseParms.For(pawn), 6.9f,
            t => EquipmentUtility.CanEquip(t, pawn) && SlaveRebellionUtility.WeaponUsableInRebellion(t) &&
                 t.GetRoom() == room) != null;
    }

    private enum SlaveRebellionType
    {
        GrandRebellion,
        LocalRebellion,
        SingleRebellion
    }
}