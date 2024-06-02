using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace SlaveRebellionsImproved;

public class LordToil_SlavesEscapeNoViolence(IntVec3 dest) : LordToil_Travel(dest)
{
    public override IntVec3 FlagLoc => Data.dest;

    private new LordToilData_Travel Data => (LordToilData_Travel)data;

    public override bool AllowSatisfyLongNeeds => false;

    protected override float AllArrivedCheckRadius => 20f;

    public override void UpdateAllDuties()
    {
        var lordToilData_Travel = Data;
        foreach (var pawn in lord.ownedPawns)
        {
            pawn.mindState.duty = new PawnDuty(DefOfClass.SlaveEscapeNoViolence, lordToilData_Travel.dest);
        }
    }

    public override void LordToilTick()
    {
        base.LordToilTick();
        foreach (var pawn in lord.ownedPawns)
        {
            pawn.guilt.Notify_Guilty();
        }
    }
}