---
inclusion: always
---

# Farm Game — Unity C# Conventions

## Project structure
```
_Project/
  Assets/
    Scripts/
      Core/          ← TileManager, ToolModeManager, InputRouter, CropGrowthManager
      UI/            ← BottomSheetController, SeedTrayController, ModeBarController, UILayerManager
      Data/          ← TileData, CropData, SeedInventoryManager
      Tools/         ← HoeTool, WaterTool, WeedTool, PestTool, HarvestTool, PlantTool, CleanupTool
      Utils/
    Prefabs/
    Sprites/
    Scenes/
```

## C# style
- PascalCase for classes, methods, and public properties
- camelCase for private fields (prefix with `_` for instance fields: `_toolMode`)
- Use `[SerializeField] private` for inspector-exposed fields
- Prefer events (`System.Action`, `UnityEvent`) over polling for state changes
- Managers are MonoBehaviours on persistent GameObjects; use `[RequireComponent]` where needed

## Key managers and their responsibilities
| Manager | Responsibility |
|---------|---------------|
| `TileManager` | Grid of TileData; state transitions |
| `ToolModeManager` | Current tool mode; mode transitions; fires OnModeChanged |
| `InputRouter` | Touch routing; tap vs drag detection; finger count |
| `CropGrowthManager` | Growth tick; random problem generation; death check |
| `UILayerManager` | Enforces mutual-exclusion rules for SeedTray / BottomSheet / ModeBar |
| `SeedInventoryManager` | Seed counts per type; sorted list for SeedTray |

## Event pattern
```csharp
// In ToolModeManager:
public event Action<ToolMode> OnModeChanged;
public void SetMode(ToolMode mode) { _currentMode = mode; OnModeChanged?.Invoke(mode); }
```

## No polling rule
Do not use Update() to poll ToolModeManager.CurrentMode in UI components.
Subscribe to events instead.

## Canvas
- Canvas Scaler: Scale With Screen Size
- Reference Resolution: 1080 × 1920
- Screen Match Mode: Match Width or Height, Match = 0.5

## Performance notes
- Tile highlights: use a single overlay sprite per tile, not instantiate/destroy per frame
- SeedTray cards: pool cards, update content on open rather than rebuild each time
- BottomSheet: one shared panel, swap content — do not instantiate new sheets per state

---

## Shape sprites for UI

All UI shapes are in `Assets/Textures/Shapes/`. They are white sprites — tint with `Image.color` to apply any color. All sliceable shapes use `Image.Type = Sliced`.

| File | 9-Slice Border | Image.Type | Use for |
|------|---------------|------------|---------|
| `Rectangle.png` | 70px all sides | Sliced | TopHUD, BottomToolbar, SeedTray, SeedCard, BottomSheet, ModeBar, ActionButton, PopupConfirm |
| `Squircle.png` | 128px all sides | Sliced | Side utility buttons (72×72), toolbar visual buttons (92×92), currency capsules |
| `Rectangle-Outline.png` | 80px all sides | Sliced | Selected-state border, tile valid/pointed highlight overlay, SeedCard selected border |
| `Circle.png` | none | Simple | Profile avatar, circular badges, coin/gem icons |
| `Square.png` | none | Simple | Grab handle (84×8 cropped), small square icon backgrounds |

### Component → Shape mapping

| UI Component | Shape | Notes |
|-------------|-------|-------|
| TopHUD panel | Rectangle | radius 24 → works with border 70 |
| BottomToolbar panel | Rectangle | radius 30 |
| ModeBar panel | Rectangle | radius 22 |
| SeedTray panel | Rectangle | radius 28 |
| SeedCard | Rectangle | radius 22 |
| BottomSheet panel | Rectangle | radius 32 (top only — mask bottom corners) |
| PopupConfirm panel | Rectangle | radius 28 |
| Action button | Rectangle | radius 18 |
| Side utility button | Squircle | 72×72, strong squircle corners |
| Toolbar tool button | Squircle | 92×92 visual area |
| Currency capsule | Squircle | height 44, variable width |
| Profile avatar | Circle | 56×56 inner |
| Tile highlight overlay | Rectangle-Outline | tinted green/red/white per state |
| SeedCard selected ring | Rectangle-Outline | bright border tint |
| Bottom sheet grab handle | Square | scale to 84×8, white tint |

### Usage pattern
```csharp
// Example: set up a panel with Rectangle shape
var img = GetComponent<Image>();
img.sprite = Resources.Load<Sprite>("Textures/Shapes/Rectangle");
img.type = Image.Type.Sliced;
img.color = new Color(0.1f, 0.1f, 0.1f, 0.85f); // dark semi-transparent panel

// Example: tint a Squircle button active state
img.color = Color.white; // or accent color for active state
```
