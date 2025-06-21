using Verse;

namespace SlaveRebellionsImproved;

public class SlaveSettings : ModSettings
{
    public float RebellionLoyal = 0.8f;
    public float RebellionMin = 0.6f;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref RebellionMin, "rebellionMin", 0.6f);
        Scribe_Values.Look(ref RebellionLoyal, "rebellionLoyal", 0.8f);
        base.ExposeData();
        if (RebellionMin != 0f || RebellionLoyal != 0f)
        {
            return;
        }

        RebellionMin = 0.6f;
        RebellionLoyal = 0.8f;
    }
}