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
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(inRect);
        listing_Standard.Label("MinSupressionLabel".Translate());
        listing_Standard.Label("CurrentValueForSlaves".Translate(Math.Round(Settings.rebellionMin * 100f, 2)));
        Settings.rebellionMin = listing_Standard.Slider(Settings.rebellionMin, 0f, 1f);
        if (Settings.rebellionLoyal < Settings.rebellionMin)
        {
            Settings.rebellionLoyal = Settings.rebellionMin;
        }

        listing_Standard.Label("LoyalSupressionLabel".Translate());
        listing_Standard.Label("CurrentValueForSlaves".Translate(Math.Round(Settings.rebellionLoyal * 100f, 2)));
        Settings.rebellionLoyal = listing_Standard.Slider(Settings.rebellionLoyal, Settings.rebellionMin, 1f);
        if (listing_Standard.ButtonText("SlaveDefaultValues".Translate()))
        {
            Settings.rebellionMin = 0.6f;
            Settings.rebellionLoyal = 0.8f;
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("SRI.CurrentModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "Slave Rebellions Improved";
    }
}