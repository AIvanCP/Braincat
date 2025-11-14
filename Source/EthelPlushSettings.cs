using Verse;

namespace BrainCat.EthelPlush
{
    /// <summary>
    /// Settings for the Ethel Plush mod.
    /// Allows players to configure sound trigger behavior.
    /// </summary>
    public class EthelPlushSettings : ModSettings
    {
        /// <summary>
        /// Whether to increase trigger chance after each failed attempt.
        /// If false, chance stays constant at base value.
        /// Default: true
        /// </summary>
        public bool enableChanceIncrease = true;

        /// <summary>
        /// Amount to increase chance by after each failed trigger (when enabled).
        /// Range: 0.001 (0.1%) to 0.01 (1%)
        /// Default: 0.01 (1%)
        /// </summary>
        public float chanceIncrementAmount = 0.01f;

        /// <summary>
        /// Base chance for sound to trigger on each interval check.
        /// Range: 0.001 (0.1%) to 0.05 (5%)
        /// Default: 0.01 (1%)
        /// </summary>
        public float baseChance = 0.01f;

        /// <summary>
        /// Save/load settings
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref enableChanceIncrease, "enableChanceIncrease", true);
            Scribe_Values.Look(ref chanceIncrementAmount, "chanceIncrementAmount", 0.01f);
            Scribe_Values.Look(ref baseChance, "baseChance", 0.01f);
        }
    }
}
