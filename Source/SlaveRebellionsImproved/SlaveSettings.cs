using Verse;

namespace SlaveRebellionsImproved;

public class SlaveSettings : ModSettings
{
    public float rebellionLoyal = 0.8f;
    public float rebellionMin = 0.6f;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref rebellionMin, "rebellionMin", 0.6f);
        Scribe_Values.Look(ref rebellionLoyal, "rebellionLoyal", 0.8f);
        base.ExposeData();
        if (rebellionMin != 0f || rebellionLoyal != 0f)
        {
            return;
        }

        rebellionMin = 0.6f;
        rebellionLoyal = 0.8f;
    }
}