using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace SlaveRebellionsImproved;

internal class LordJob_SlaveEscape : LordJob
{
    private readonly IntVec3 exitPoint;

    private IntVec3 groupUpLoc;

    public LordJob_SlaveEscape()
    {
    }

    public LordJob_SlaveEscape(IntVec3 groupUpLoc, IntVec3 exitPoint)
    {
        this.groupUpLoc = groupUpLoc;
        this.exitPoint = exitPoint;
    }

    public override StateGraph CreateGraph()
    {
        var stateGraph = new StateGraph();
        if (!ModLister.CheckIdeology("Slave rebellion"))
        {
            return stateGraph;
        }

        var firstSource = (LordToil_Travel)(stateGraph.StartingToil = new LordToil_Travel(groupUpLoc)
        {
            maxDanger = Danger.Deadly,
            useAvoidGrid = true
        });
        var lordToil_SlavesEscapeNoViolence = new LordToil_SlavesEscapeNoViolence(exitPoint);
        stateGraph.AddToil(lordToil_SlavesEscapeNoViolence);
        var lordToil_ExitMap = new LordToil_ExitMap(LocomotionUrgency.Jog, true);
        stateGraph.AddToil(lordToil_ExitMap);
        var lordToil_ExitMap2 = new LordToil_ExitMap(LocomotionUrgency.Jog)
        {
            useAvoidGrid = true
        };
        stateGraph.AddToil(lordToil_ExitMap2);
        var transition = new Transition(firstSource, lordToil_SlavesEscapeNoViolence);
        transition.AddTrigger(new Trigger_Memo("TravelArrived"));
        stateGraph.AddTransition(transition);
        var transition2 = new Transition(lordToil_SlavesEscapeNoViolence, lordToil_ExitMap);
        transition2.AddSource(lordToil_ExitMap2);
        transition2.AddTrigger(new Trigger_PawnCannotReachMapEdge());
        stateGraph.AddTransition(transition2);
        var transition3 = new Transition(lordToil_ExitMap, lordToil_ExitMap2);
        transition3.AddSource(lordToil_SlavesEscapeNoViolence);
        transition3.AddTrigger(new Trigger_PawnCanReachMapEdge());
        stateGraph.AddTransition(transition3);
        return stateGraph;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref groupUpLoc, "groupUpLoc");
    }

    public override void Notify_PawnAdded(Pawn p)
    {
        ReachabilityUtility.ClearCacheFor(p);
    }

    public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
    {
        ReachabilityUtility.ClearCacheFor(p);
    }

    public override bool CanOpenAnyDoor(Pawn p)
    {
        return true;
    }

    public override bool ValidateAttackTarget(Pawn searcher, Thing target)
    {
        if (target is not Pawn { MentalStateDef: var mentalStateDef })
        {
            return true;
        }

        if (mentalStateDef == null)
        {
            return true;
        }

        return !mentalStateDef.escapingPrisonersIgnore;
    }
}