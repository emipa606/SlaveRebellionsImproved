using System;
using Mlie;
using UnityEngine;
using Verse;

namespace SlaveRebellionsImproved;

public class SlaveMod : Mod
{
    private static string currentVersion;
    public static SlaveMod Instance;
    public readonly SlaveSettings Settings;

    public SlaveMod(ModContentPack content)
        : base(content)
    {
        Instance = this;
        Settings = GetSettings<SlaveSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);
        listingStandard.Label("MinSupressionLabel".Translate());
        listingStandard.Label("CurrentValueForSlaves".Translate(Math.Round(Settings.RebellionMin * 100f, 2)));
        Settings.RebellionMin = listingStandard.Slider(Settings.RebellionMin, 0f, 1f);
        if (Settings.RebellionLoyal < Settings.RebellionMin)
        {
            Settings.RebellionLoyal = Settings.RebellionMin;
        }

        listingStandard.Label("LoyalSupressionLabel".Translate());
        listingStandard.Label("CurrentValueForSlaves".Translate(Math.Round(Settings.RebellionLoyal * 100f, 2)));
        Settings.RebellionLoyal = listingStandard.Slider(Settings.RebellionLoyal, Settings.RebellionMin, 1f);
        if (listingStandard.ButtonText("SlaveDefaultValues".Translate()))
        {
            Settings.RebellionMin = 0.6f;
            Settings.RebellionLoyal = 0.8f;
        }

        if (currentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("SRI.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Slave Rebellions Improved";
    }
}