using Verse;

namespace BrainCat.EthelPlush
{
    /// <summary>
    /// Properties for the Ethel Plush comp system.
    /// Configures sound trigger timing, probability scaling, and cooldown behavior.
    /// Note: Mod settings will override baseChance and chanceIncrement values if configured.
    /// </summary>
    public class CompProperties_EthelPlush : CompProperties
    {
        /// <summary>
        /// Number of ticks between sound trigger checks.
        /// Default: 1200 ticks (20 seconds at 60 ticks/second)
        /// </summary>
        public int tickInterval = 1200;

        /// <summary>
        /// Base chance for sound to trigger on each interval check.
        /// Default: 0.01 (1%)
        /// Note: Can be overridden by mod settings.
        /// </summary>
    public float baseChance = 0.01f;

        /// <summary>
        /// Amount to increase chance by after each failed trigger attempt.
        /// Default: 0.01 (1%)
        /// Note: Can be overridden by mod settings. Settings can also disable increment entirely.
        /// </summary>
    public float chanceIncrement = 0.01f;

        /// <summary>
        /// Number of ticks to wait after a sound starts before allowing another trigger.
        /// Default: 300 ticks (5 seconds)
        /// Prevents sound spam/overlapping
        /// </summary>
        public int soundCooldownTicks = 300;

        public CompProperties_EthelPlush()
        {
            compClass = typeof(CompEthelPlush);
        }
    }
}
