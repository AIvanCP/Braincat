using UnityEngine;
using Verse;

namespace BrainCat.EthelPlush
{
    /// <summary>
    /// Main mod class for Ethel Plush.
    /// Handles mod initialization and settings UI.
    /// </summary>
    public class EthelPlushMod : Mod
    {
        /// <summary>
        /// Mod settings instance
        /// </summary>
        public static EthelPlushSettings settings;

        /// <summary>
        /// Constructor - load settings
        /// </summary>
        public EthelPlushMod(ModContentPack content) : base(content)
        {
            settings = GetSettings<EthelPlushSettings>();
        }

        /// <summary>
        /// Mod settings menu name
        /// </summary>
        public override string SettingsCategory()
        {
            return "Ethel Plush";
        }

        /// <summary>
        /// Draw settings UI
        /// </summary>
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);

            // Section header
            Text.Font = GameFont.Medium;
            listingStandard.Label("Sound Trigger Settings");
            Text.Font = GameFont.Small;
            listingStandard.Gap(4f);
            listingStandard.Label("Configure how often the Ethel Plush triggers sounds");
            listingStandard.GapLine(12f);

            // Base chance slider
            listingStandard.Label($"Base Trigger Chance: {(settings.baseChance * 100f):F1}%");
            listingStandard.Gap(4f);
            settings.baseChance = listingStandard.Slider(settings.baseChance, 0.001f, 0.05f);
            listingStandard.Label("Initial chance for sound to play each check (every 20 seconds)", -1f);
            listingStandard.Gap(12f);

            // Enable chance increase toggle
            listingStandard.CheckboxLabeled(
                "Enable Chance Increase on Fail",
                ref settings.enableChanceIncrease,
                "If enabled, trigger chance increases each time it fails to play a sound");
            listingStandard.Gap(12f);

            // Chance increment slider (only if enabled)
            if (settings.enableChanceIncrease)
            {
                listingStandard.Label($"Chance Increase per Fail: {(settings.chanceIncrementAmount * 100f):F1}%");
                listingStandard.Gap(4f);
                settings.chanceIncrementAmount = listingStandard.Slider(settings.chanceIncrementAmount, 0.001f, 0.01f);
                listingStandard.Label("Amount to increase chance by after each failed trigger attempt", -1f);
                listingStandard.Gap(12f);

                // Calculate approximate time to guarantee
                float maxChance = 1.0f;
                int checksToGuarantee = Mathf.CeilToInt((maxChance - settings.baseChance) / settings.chanceIncrementAmount) + 1;
                float minutesToGuarantee = (checksToGuarantee * 20f) / 60f; // 20 seconds per check

                listingStandard.Label($"Approximate time to guarantee: ~{minutesToGuarantee:F1} minutes");
                listingStandard.Label("If unlucky, sound is guaranteed after this many checks", -1f);
                listingStandard.Gap(12f);
            }
            else
            {
                listingStandard.Label("Chance will remain constant at base value");
                listingStandard.Gap(12f);
            }

            // Reset to defaults button
            listingStandard.GapLine(12f);
            if (listingStandard.ButtonText("Reset to Defaults"))
            {
                settings.enableChanceIncrease = true;
                settings.chanceIncrementAmount = 0.01f;
                settings.baseChance = 0.01f;
            }

            listingStandard.Gap(12f);
            listingStandard.Label("Note: Changes take effect immediately for all Ethel Plush items in your colony.", -1f);

            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }
    }
}
