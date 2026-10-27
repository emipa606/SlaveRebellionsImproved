using RimWorld;
using Verse.AI;

namespace SlaveRebellionsImproved;

[DefOf]
public static class DefOfClass
{
    public static DutyDef SlaveEscapeNoViolence;

    static DefOfClass()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DefOfClass));
    }
}