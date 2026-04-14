# Unity Farm Game — Technical Design

## Stack
- Unity (mobile portrait, 1080×1920)
- uGUI (Canvas Scaler: Scale With Screen Size, reference 1080×1920)
- C# scripts following Unity MonoBehaviour pattern

## Available UI shape sprites
Path: `Assets/Textures/Shapes/` — white sprites, tint with `Image.color`, sliceable ones use `Image.Type = Sliced`.

| Shape | Border | Primary use |
|-------|--------|-------------|
| `Rectangle.png` | 70px | All rounded panels: HUD, Toolbar, SeedTray, Sheet, Buttons |
| `Squircle.png` | 128px | Side buttons (72×72), Tool buttons (92×92), Capsules |
| `Rectangle-Outline.png` | 80px | Selected borders, tile highlight overlays |
| `Circle.png` | — | Avatar, circular icons |
| `Square.png` | — | Grab handle, small icon BGs |

See full component→shape mapping in `.kiro/steering/unity-conventions.md`.

---

## 1. Scene Hierarchy

```
FarmScene
├── [Camera] MainCamera
├── [World] FarmWorld          ← isometric tilemap, full-screen
├── [UI] Canvas (1080×1920)
│   ├── TopHUD
│   ├── SideButtons_Right
│   ├── SideButtons_Left
│   ├── BottomToolbar
│   ├── ModeBar
│   ├── SeedTray
│   ├── BottomSheet
│   └── PopupConfirm
└── [Managers]
    ├── TileManager
    ├── ToolModeManager
    ├── SeedInventoryManager
    ├── CropGrowthManager
    └── InputRouter
```

---

## 2. Tile State Machine

### Enum: TileState
```csharp
public enum TileState
{
    WildGrass,
    NormalSoil,
    TilledSoil,
    PlantedGrowing,
    PlantedReady,
    PlantedDead
}
```

### TileData (per tile)
```csharp
public class TileData
{
    public TileState state;
    public string cropType;        // set when PlantedGrowing
    public int growthPhase;        // 0-based
    public float growthProgress;   // 0.0–1.0
    public bool isWatered;
    public bool hasWeeds;
    public bool hasPests;
    public bool isNearReady;       // true when progress > 0.85
}
```

### Valid State Transitions
| From | Action | To |
|------|--------|----|
| WildGrass | Hoe | TilledSoil |
| NormalSoil | Hoe | TilledSoil |
| TilledSoil | Plant(seed) | PlantedGrowing |
| PlantedGrowing | TimerComplete | PlantedReady |
| PlantedReady | Harvest | NormalSoil |
| PlantedGrowing | PestTimeout | PlantedDead |
| PlantedDead | Cleanup | NormalSoil |

---

## 3. Tool Mode State Machine

### Enum: ToolMode
```csharp
public enum ToolMode
{
    None,
    HoeMode,
    SeedSelectMode,
    PlantingMode,   // carries seedType
    WaterMode,
    HarvestMode,
    WeedMode,
    PestMode
}
```

### ToolModeManager
- Holds `CurrentMode` and `SelectedSeedType`
- `SetMode(ToolMode mode)` — replaces current mode, fires `OnModeChanged` event
- `ExitMode()` — sets None, fires `OnModeChanged`
- Rules enforced by manager:
  - Lifting finger does NOT call `ExitMode()`
  - Camera gestures do NOT call `ExitMode()`
  - Selecting another tool calls `SetMode()` which replaces

### Transitions table
| Trigger | Result |
|---------|--------|
| Tap toolbar Hoe | HoeMode |
| Tap toolbar Seed | SeedSelectMode |
| Select seed card | PlantingMode(seedType) |
| Tap toolbar Water | WaterMode |
| Tap toolbar Harvest | HarvestMode |
| Tap toolbar Weed | WeedMode |
| Tap toolbar Pest | PestMode |
| Tap active tool again | None |
| Tap mode bar X | None |
| Run out of selected seed | SeedSelectMode |

---

## 4. Input System

### InputRouter
Intercepts all touch/pointer events and routes based on current tool mode and gesture type.

```
TouchDown → record start position
TouchMove → if distance > threshold → drag mode
TouchUp   → if drag mode → end drag (do NOT exit tool mode)
           → if not drag → tap action
```

### One-Finger Rule
- One finger = tool action or tile select
- One finger never pans camera

### Two-Finger Rule
- Two fingers = camera pan
- Two-finger pinch = camera zoom
- Two-finger gestures do NOT exit tool mode

### Drag Chain Rule
- Drag starts when finger moves beyond tap-threshold (~10px)
- Each tile entered during drag is evaluated and processed if valid
- Finger lift = end drag chain = does NOT exit mode
- Popup confirm never fires during drag; queued until drag ends

---

## 5. UI Layout Dimensions

### Safe Area
| Edge | Value |
|------|-------|
| Top | 44 |
| Left | 24 |
| Right | 24 |
| Bottom | 34 |

### Component Positions
| Component | Width | Height | Top Y | Bottom Y |
|-----------|-------|--------|-------|----------|
| Top HUD | 1032 | 92 | 44 | 136 |
| Bottom Toolbar | 1032 | 120 | 1766 | 1886 |
| Mode Bar | 560 | 64 | 1686 | 1750 |
| Seed Tray | 1032 | 180 | 1490 | 1670 |
| Bottom Sheet (collapsed) | 1080 | 260 | 1660 | 1920 |
| Bottom Sheet (expanded) | 1080 | 540 | 1380 | 1920 |

### Layer Z-Order (back to front)
1. FarmWorld (z=0)
2. TopHUD (z=10)
3. SideButtons (z=20)
4. BottomToolbar (z=30)
5. ModeBar (z=40)
6. SeedTray (z=50)
7. BottomSheet (z=60)
8. PopupConfirm (z=70)

---

## 6. Toolbar Slot Spec

| Slot | Tool | Mode |
|------|------|------|
| 1 | Cuốc đất | HoeMode |
| 2 | Hạt giống | SeedSelectMode |
| 3 | Tưới nước | WaterMode |
| 4 | Thu hoạch | HarvestMode |
| 5 | Nhổ cỏ | WeedMode |
| 6 | Bắt sâu | PestMode |
| 7 | More | — |

### Button States
- Default: neutral background
- Active: bright fill + glow + scale 1.06
- Disabled: opacity 50%

---

## 7. Seed Tray Design

### Data Source
`SeedInventoryManager.GetOwnedSeeds()` — returns seeds with quantity > 0, sorted by grow time ascending.

### SeedCard (per card)
- Size: 160×132, radius 22
- Icon: 56×56
- Name: 16–18pt
- Grow time: 14–16pt
- States: Default / Selected (scale 1.04, bright border) / OutOfStock (opacity reduced, count=0)

### Shop Shortcut
- Button 72×72 at right end of tray
- Opens Shop at Seed tab

### Out-of-Stock During Drag
1. Planting stops immediately
2. Drag chain ends
3. SeedTray becomes visible again
4. Mode stays SeedSelectMode (player can pick another seed)
5. Shop does NOT auto-open

---

## 8. Bottom Sheet Design

### Shell
Single shared panel. Content controlled by `BottomSheetController` which reads the selected tile's `TileData`.

### Content by State
| State | Title | Status | Actions |
|-------|-------|--------|---------|
| WildGrass | Cỏ hoang | — | Cuốc đất |
| NormalSoil | Đất thường | — | Cuốc đất |
| TilledSoil | Đất đã cuốc | — | Gieo hạt, Mở thanh hạt giống |
| PlantedGrowing | crop name | Đang phát triển | Tưới nước (if !isWatered), Nhổ cỏ (if hasWeeds), Bắt sâu (if hasPests) |
| PlantedReady | crop name | Sẵn sàng thu hoạch | Thu hoạch |
| PlantedDead | crop name | Đã chết | Dọn xác cây |

### Expand/Collapse
- Grab handle at top (84×8, margin-top 14)
- Swipe up = expand, swipe down = collapse
- Expanded shows: phase, progress %, time remaining, care status details

### Visibility Controller
- Bottom sheet and seed tray are mutually exclusive (enforced by `UILayerManager`)
- Mode bar hides when bottom sheet open; restores on close if tool mode still active

---

## 9. Tile Highlight System

### TileHighlightManager
Listens to `ToolModeManager.OnModeChanged` and `InputRouter.OnPointerMoved`.

| Highlight Type | Visual |
|----------------|--------|
| Valid tile | Very light tint |
| Currently pointed tile | Bright border + short glow |
| Invalid tile | Light grey or red tint |

### Valid tile definition per mode
| Mode | Valid States |
|------|-------------|
| HoeMode | WildGrass, NormalSoil |
| PlantingMode | TilledSoil |
| WaterMode | PlantedGrowing |
| WeedMode | PlantedGrowing where hasWeeds |
| PestMode | PlantedGrowing where hasPests |
| HarvestMode | PlantedReady |
| CleanupMode | PlantedDead |

---

## 10. Problem State & Visuals

### World Feedback (on TileData change)
- `!isWatered`: pale soil texture + crack overlay art
- `hasWeeds`: weed overlay art on tile
- `hasPests`: pest art on crop

### Problem Icons (20×20, per tile overlay)
- Dry: water drop / warning icon
- Weeds: small grass icon
- Pests: small bug icon

### Rule: Both required
World art change + icon overlay must both be present. Neither alone is acceptable.

---

## 11. Crop Growth System

### CropGrowthManager
Runs on game time (not real-time unless specified).

Per PlantedGrowing tile, each update tick:
1. If hasPests → grow slower or no grow; if pest duration > threshold → PlantedDead
2. If hasWeeds → grow slower
3. If !isWatered → grow slower or reduce yield multiplier
4. Accumulate growthProgress toward 1.0
5. When growthProgress >= phaseThreshold → advance growthPhase, trigger art update
6. When growthProgress >= 1.0 → set state to PlantedReady

### Phase counts by crop type
- Short: 3 phases
- Medium: 4 phases
- Long: 5 phases

---

## 12. Drag Conflict Resolution (Hoe)

### HoeDragProcessor
During hoe drag:
- Valid tile (WildGrass/NormalSoil) → process immediately
- Conflict tile (e.g., PlantedGrowing) → add to `pendingConflicts` list

On drag end (finger lift):
- If `pendingConflicts` is empty → done
- If `pendingConflicts` has entries → show PopupConfirm

### PopupConfirm
- Size: 760×320, radius 28
- Content: "X ô đã được cuốc. Y ô xung đột — có trồng đang dở. Bỏ qua?"
- Buttons: Hủy (56h, 180w min) | Xác nhận / Bỏ qua ô xung đột (56h, 180w min)

---

## 13. Size System

### Icon Sizes
| Context | Size |
|---------|------|
| Top utility | 22×22 |
| Currency | 24×24 |
| Side floating | 32×32 |
| Main tool | 42×42 |
| Mode bar | 28×28 |
| Seed card | 56×56 |
| Bottom sheet action | 22×22 |
| Close button | 18×18 |
| Problem icon | 20×20 |
| Timer badge | 18×18 |

### Typography Scale (pt)
| Role | Size |
|------|------|
| Large title | 24 |
| Main / action | 18 |
| Secondary | 16 |
| Caption / timer | 14 |
| Emphasis number | 20 |

### Spacing Scale (px)
| Name | Value |
|------|-------|
| Micro | 8 |
| Small | 12 |
| Medium | 16 |
| Large | 24 |
| Section | 32 |

### Radius Scale (px)
| Name | Value |
|------|-------|
| Small control | 18 |
| Medium card | 22 |
| Large panel | 28–32 |

---

## 14. Implementation Phases (Agent Execution Order)

| Phase | What to build |
|-------|--------------|
| 0 — Skeleton | Farm full-screen, TopHUD, SideButtons, BottomToolbar, ModeBar placeholder, SeedTray placeholder, BottomSheet placeholder |
| 1 — Tile States + Bottom Sheet | 6 tile states, BottomSheet 2-level, content by state |
| 2 — Tool Mode Framework | Toolbar active state, ModeBar, valid tile highlight, invalid feedback, exit rules |
| 3 — Planting Flow | SeedTray, seed select, PlantingMode, tap plant, drag plant, out-of-seed fallback |
| 4 — Care Flow | Water, weed, pest flows; world feedback + problem icons |
| 5 — Harvest & Dead Plant | HarvestMode flow, skip unready, PlantedDead display, cleanup, reward "cỏ" |
| 6 — Polish | Animations, glow, transitions, easing (only after 0–5 verified) |
