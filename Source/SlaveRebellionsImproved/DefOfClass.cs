using RimWorld;
using Verse.AI;

namespace SlaveRebellionsImproved;

[DefOf]
public class DefOfClass
{
    public static DutyDef SlaveEscapeNoViolence;

    static DefOfClass()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(DefOfClass));
    }
}