using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.Sound;

namespace BrainCat.EthelPlush
{
    /// <summary>
    /// Main component for Ethel Plush functionality.
    /// Handles random sound triggering with dynamic probability scaling
    /// and buff application to player pawns.
    /// </summary>
    public class CompEthelPlush : ThingComp
    {
        // Properties cast from base
        private CompProperties_EthelPlush Props => (CompProperties_EthelPlush)props;

        // Current chance to trigger sound (scales up on failures)
        private float currentChance;

        // Tick counter for interval checks
        private int tickCounter = 0;

        // Cooldown counter to prevent sound spam
        private int cooldownCounter = 0;

        // List of all available Ethel sound defs
        private static List<SoundDef> ethelSounds;

        // Flag to check if sounds were initialized
        private static bool soundsInitialized = false;

        /// <summary>
        /// Initialize comp state when spawned
        /// </summary>
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            
            // Initialize chance to base value from mod settings
            if (currentChance == 0f)
            {
                currentChance = EthelPlushMod.settings?.baseChance ?? Props.baseChance;
            }

            // Initialize sound list once
            if (!soundsInitialized)
            {
                InitializeSounds();
            }
        }

        /// <summary>
        /// Initialize the list of available Ethel sounds
        /// </summary>
        private void InitializeSounds()
        {
            ethelSounds = new List<SoundDef>();

            // Try to find all custom Ethel sounds
            var allSoundDefs = DefDatabase<SoundDef>.AllDefsListForReading;
            foreach (var soundDef in allSoundDefs)
            {
                if (soundDef.defName.StartsWith("EthelPlush_"))
                {
                    ethelSounds.Add(soundDef);
                }
            }

            // Log results
            if (ethelSounds.Count > 0)
            {
                Log.Message($"[Ethel Plush] Found {ethelSounds.Count} custom sound(s)");
                soundsInitialized = true;
            }
            else
            {
                Log.Warning("[Ethel Plush] No custom sounds found! Will use fallback sound.");
                soundsInitialized = true;
            }
        }

        /// <summary>
        /// Main tick function - runs sound trigger logic
        /// </summary>
        public override void CompTick()
        {
            base.CompTick();

            // Safety check: ensure parent is spawned and has a map
            if (parent == null || !parent.Spawned || parent.Map == null)
            {
                return;
            }

            // Decrement cooldown if active
            if (cooldownCounter > 0)
            {
                cooldownCounter--;
                return; // Skip logic while on cooldown
            }

            // Increment tick counter
            tickCounter++;

            // Check if interval has passed
            if (tickCounter >= Props.tickInterval)
            {
                tickCounter = 0; // Reset counter
                TryTriggerSound();
            }
        }

        /// <summary>
        /// Attempt to trigger a sound based on current chance
        /// </summary>
        private void TryTriggerSound()
        {
            // Roll for sound trigger
            if (Rand.Chance(currentChance))
            {
                // Success! Play sound and apply buffs
                PlayRandomSound();
                ApplyBuffsToPlayerPawns();

                // Reset chance to base value from mod settings
                currentChance = EthelPlushMod.settings?.baseChance ?? Props.baseChance;

                // Start cooldown
                cooldownCounter = Props.soundCooldownTicks;
            }
            else
            {
                // Failed, increase chance for next time (if enabled in settings)
                if (EthelPlushMod.settings?.enableChanceIncrease ?? true)
                {
                    float increment = EthelPlushMod.settings?.chanceIncrementAmount ?? Props.chanceIncrement;
                    currentChance += increment;

                    // Cap at 100% to prevent overflow
                    if (currentChance > 1.0f)
                    {
                        currentChance = 1.0f;
                    }
                }
                // If increment disabled, chance stays the same
            }
        }

        /// <summary>
        /// Play a random Ethel sound at the plush's position
        /// </summary>
        private void PlayRandomSound()
        {
            // Safety check
            if (parent?.Map == null)
            {
                return;
            }

            SoundDef soundToPlay = null;

            // Try to select a random custom sound
            if (ethelSounds != null && ethelSounds.Count > 0)
            {
                soundToPlay = ethelSounds.RandomElement();
            }

            // Fallback to vanilla cat meow if no custom sounds available
            if (soundToPlay == null)
            {
                // Use a runtime lookup instead of SoundDefOf.Pawn_Cat_Meow which may not exist in SoundDefOf
                soundToPlay = DefDatabase<SoundDef>.GetNamedSilentFail("Pawn_Cat_Meow");
                if (soundToPlay == null)
                {
                    Log.Warning("[Ethel Plush] Fallback sound 'Pawn_Cat_Meow' not found; no sound will be played.");
                }
                else
                {
                    Log.Warning("[Ethel Plush] Using fallback sound (vanilla cat meow)");
                }
            }

            // Play the sound at the plush's position with reduced volume (40%)
            if (soundToPlay != null)
            {
                // Use SoundInfo to set volume factor; InMap attaches the sound to the map
                var target = new TargetInfo(parent.Position, parent.Map, false);
                var soundInfo = SoundInfo.InMap(target, MaintenanceType.None);
                soundInfo.volumeFactor = 0.4f;
                soundToPlay.PlayOneShot(soundInfo);
            }
        }

        /// <summary>
        /// Apply buff hediff and mood thought to all player-owned pawns
        /// </summary>
        private void ApplyBuffsToPlayerPawns()
        {
            // Safety check
            if (parent?.Map == null)
            {
                return;
            }

            // Get hediff and thought defs
            HediffDef buffHediff = DefDatabase<HediffDef>.GetNamedSilentFail("EthelBuff_Hediff");
            ThoughtDef buffThought = DefDatabase<ThoughtDef>.GetNamedSilentFail("EthelBuff_Thought");

            // Safety check for defs
            if (buffHediff == null)
            {
                Log.Error("[Ethel Plush] EthelBuff_Hediff not found!");
                return;
            }

            // Get all pawns on the map
            IReadOnlyList<Pawn> allPawns = parent.Map.mapPawns.AllPawnsSpawned;

            int pawnsBuffed = 0;

            foreach (Pawn pawn in allPawns)
            {
                // Only apply to player-owned, alive, humanlike pawns
                if (pawn == null || pawn.Dead || !pawn.Spawned)
                {
                    continue;
                }

                if (pawn.Faction == null || pawn.Faction != Faction.OfPlayer)
                {
                    continue;
                }

                // Only buff humanlike pawns (not animals)
                if (!pawn.RaceProps.Humanlike)
                {
                    continue;
                }

                // Check if pawn already has the hediff
                Hediff existingHediff = pawn.health?.hediffSet?.GetFirstHediffOfDef(buffHediff);

                if (existingHediff != null)
                {
                    // Refresh duration by removing and re-adding
                    pawn.health.RemoveHediff(existingHediff);
                }

                // Add the buff hediff
                Hediff newHediff = HediffMaker.MakeHediff(buffHediff, pawn);
                pawn.health.AddHediff(newHediff);

                // Add mood thought if available
                if (buffThought != null && pawn.needs?.mood?.thoughts?.memories != null)
                {
                    pawn.needs.mood.thoughts.memories.TryGainMemory(buffThought);
                }

                pawnsBuffed++;
            }

            // Log success
            if (pawnsBuffed > 0)
            {
                Log.Message($"[Ethel Plush] Applied buff to {pawnsBuffed} pawn(s)");
            }
        }

        /// <summary>
        /// Save/load support for current chance and counters
        /// </summary>
        public override void PostExposeData()
        {
            base.PostExposeData();
            float defaultChance = EthelPlushMod.settings?.baseChance ?? Props.baseChance;
            Scribe_Values.Look(ref currentChance, "currentChance", defaultChance);
            Scribe_Values.Look(ref tickCounter, "tickCounter", 0);
            Scribe_Values.Look(ref cooldownCounter, "cooldownCounter", 0);
        }

        /// <summary>
        /// Inspector string - shows current state for debugging
        /// </summary>
        public override string CompInspectStringExtra()
        {
            string result = "";

            // Show current trigger chance as percentage
            result += $"Trigger chance: {(currentChance * 100f):F1}%";

            // Show cooldown status
            if (cooldownCounter > 0)
            {
                float secondsRemaining = cooldownCounter / 60f;
                result += $"\nCooldown: {secondsRemaining:F1}s";
            }

            return result;
        }
    }
}
