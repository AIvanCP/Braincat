# Ethel Plush - RimWorld Mod

A RimWorld mod that adds the **Ethel Plush**, a divine meme furniture item that randomly emits blessed sounds and grants temporary buffs to your colonists.

## 🎮 Features

- **Craftable Furniture**: Made from 50 Cloth at any crafting bench
- **Random Sound System**: Plays 1 of 12 custom meme sounds with dynamic probability scaling
- **Buff System**: Grants temporary buffs to all player-owned colonists
  - +5% Movement Speed
  - +5% Work Speed
  - +5 Mood Bonus
  - Duration: ~1 in-game day (5000 ticks)
- **Beautiful Decoration**: +15 Beauty stat
- **Safe & Optimized**: Uses ThingComp system, no Harmony patches required

## 📦 Installation

### Method 1: From Source (Compilation Required)

1. **Prerequisites**:
   - RimWorld installed
   - .NET SDK (for .NET Framework 4.7.2)
   - Visual Studio, VS Code, or Rider

2. **Update RimWorld Path**:
   - Open `Source/EthelPlush.csproj`
   - Change the `<RimWorldFolder>` path to your RimWorld installation:
     ```xml
     <RimWorldFolder>C:\Program Files (x86)\Steam\steamapps\common\RimWorld</RimWorldFolder>
     ```

3. **Build the Mod**:
   ```powershell
   cd Source
   dotnet build
   ```
   The DLL will automatically copy to `Assemblies/` folder.

4. **Copy to Mods Folder**:
   - Copy the entire `braincat` folder to:
     - **Steam**: `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\`
     - **Standalone**: `<RimWorld Install>\Mods\`

5. **Enable in Game**:
   - Launch RimWorld
   - Go to Mods menu
   - Enable "Ethel Plush - Meme Furniture"
   - Restart the game

### Method 2: Pre-compiled (If Available)

1. Extract the mod to your RimWorld Mods folder
2. Enable in-game and restart

## 🛠️ Technical Details

### Sound System
- **Tick Interval**: 1200 ticks (20 seconds)
- **Base Chance**: 1% per check
- **Scaling**: +1% per failed roll (resets on success)
- **Cooldown**: 300 ticks (5 seconds) after sound plays
- **Fallback**: Uses vanilla cat meow if custom sounds not found

### Comp System
- **CompEthelPlush**: Main component handling sound triggers and buff application
- **CompProperties_EthelPlush**: Configurable properties for modders
- **Null Safety**: Comprehensive checks for Map, Pawn, and Faction

### Buff Application
- **Target**: Player-owned humanlike pawns only
- **Hediff**: `EthelBuff_Hediff` (stat offsets + capacity mods)
- **Thought**: `EthelBuff_Thought` (mood bonus)
- **Refresh**: Existing buffs are refreshed instead of stacking

## 📁 File Structure

```
braincat/
├── About/
│   └── About.xml                          # Mod metadata
├── Assemblies/
│   └── EthelPlush.dll                     # Compiled assembly (after build)
├── Defs/
│   ├── HediffDefs/
│   │   └── EthelBuff.xml                  # Buff hediff definition
│   ├── SoundDefs/
│   │   └── EthelSounds.xml                # All 12 sound definitions
│   └── ThingDefs/
│       └── EthelPlush.xml                 # Building & recipe definition
├── Sounds/
│   └── Ethel/
│       ├── APTOIA.ogg
│       ├── APTOII.ogg
│       ├── Blueuia.ogg
│       ├── lingangulixuia.ogg
│       ├── OIAApple.ogg
│       ├── OIACaramel.ogg
│       ├── OIANokia.ogg
│       ├── OIANow.ogg
│       ├── OIIAMinecraft.ogg
│       ├── uia-uia-uia-cat.ogg
│       ├── uiaxsquid.ogg
│       └── uiaxsugar.ogg
├── Source/
│   ├── CompEthelPlush.cs                  # Main comp logic
│   ├── CompProperties_EthelPlush.cs       # Comp properties
│   └── EthelPlush.csproj                  # C# project file
└── Textures/
    └── Things/
        └── Building/
            └── Furniture/
                └── Ethel.png              # 64x64 sprite
```

## 🧪 Testing & Debugging

### In-Game Inspector
- Select the Ethel Plush in-game
- The inspector shows:
  - Current trigger chance percentage
  - Cooldown status (if active)

### Log Messages
The mod logs important events to RimWorld's dev console:
- Sound initialization count
- Buff application count per trigger
- Warnings if sounds are missing

Enable dev mode (`Ctrl + F12`) to see these messages.

## ⚙️ Configuration (For Modders)

You can modify the comp properties in `Defs/ThingDefs/EthelPlush.xml`:

```xml
<li Class="BrainCat.EthelPlush.CompProperties_EthelPlush">
    <tickInterval>1200</tickInterval>        <!-- Time between checks (in ticks) -->
    <baseChance>0.01</baseChance>           <!-- Starting trigger chance (1%) -->
    <chanceIncrement>0.01</chanceIncrement> <!-- Chance increase per fail (1%) -->
    <soundCooldownTicks>300</soundCooldownTicks> <!-- Cooldown duration (in ticks) -->
</li>
```

## 🐛 Known Issues & Compatibility

- **No known issues** - Mod uses safe ThingComp system
- **Compatible with**: All major RimWorld mods
- **No Harmony patches**: Won't conflict with other mods
- **Save-game safe**: Can be added/removed mid-game

## 📝 Version History

### v1.0.0
- Initial release
- 12 custom sound effects
- Dynamic probability scaling system
- Buff application to player pawns
- Full null safety implementation

## 🤝 Credits

- **Author**: BrainCat
- **Sounds**: Custom Ethel meme collection
- **Engine**: RimWorld by Ludeon Studios

## 📄 License

Feel free to modify and redistribute. Credit appreciated but not required.

---

**"May the divine meow guide your colony to prosperity!"** 🐱✨
