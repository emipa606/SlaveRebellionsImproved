using System.Reflection;
using HarmonyLib;
using Verse;

namespace SlaveRebellionsImproved;

[StaticConstructorOnStartup]
internal class StartUp
{
    static StartUp()
    {
        new Harmony("SlaveRebellionsImproved.patch").PatchAll(Assembly.GetExecutingAssembly());
    }
}