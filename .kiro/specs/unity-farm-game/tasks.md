# Unity Farm Game — Implementation Tasks

## How to use
Work through phases in order. Do NOT start Phase N+1 until Phase N is verified against requirements.md checklist.

---

## Phase 0 — Layout Skeleton

- [x] 0.1 Create Canvas with ScaleWithScreenSize at 1080×1920 portrait
- [x] 0.2 Add FarmWorld background (full-screen, z=0)
- [x] 0.3 Build TopHUD (width 1032, height 92, Y 44, radius 24) with placeholder content
- [x] 0.4 Build SideButtons_Right stack (72×72 each, right margin 24, start Y 250, gap 16) with 4 placeholder buttons
- [x] 0.5 Build BottomToolbar (width 1032, height 120, bottom offset 34, radius 30) with 7 slots
- [x] 0.6 Add toolbar slot labels: Cuốc đất, Hạt giống, Tưới nước, Thu hoạch, Nhổ cỏ, Bắt sâu, More
- [x] 0.7 Build ModeBar placeholder (width 560, height 64, top Y 1686, radius 22) — hidden by default
- [x] 0.8 Build SeedTray placeholder (width 1032, height 180, top Y 1490, radius 28) — hidden by default
- [x] 0.9 Build BottomSheet placeholder (width 1080, collapsed 260, expanded 540, radius-top 32) — hidden by default
- [x] 0.10 Verify layer z-order: Farm → TopHUD → SideButtons → Toolbar → ModeBar → SeedTray → BottomSheet → Popup
- [x] 0.11 Verify farm is full-screen with no fixed panel splitting the viewport

---

## Phase 1 — Tile States + Bottom Sheet

- [x] 1.1 Implement TileState enum: WildGrass, NormalSoil, TilledSoil, PlantedGrowing, PlantedReady, PlantedDead
- [x] 1.2 Implement TileData class (state, cropType, growthPhase, growthProgress, isWatered, hasWeeds, hasPests, isNearReady)
- [x] 1.3 Implement TileManager: holds grid of TileData, exposes GetTile(coord) and SetTileState(coord, state)
- [x] 1.4 Add distinct visual art/placeholder for each of the 6 tile states
- [x] 1.5 Implement tile tap detection (single finger, no drag) → open BottomSheet
- [x] 1.6 Implement BottomSheetController: reads selected tile's TileData, populates content by state
- [x] 1.7 Bottom sheet — WildGrass content: title "Cỏ hoang", action "Cuốc đất"
- [x] 1.8 Bottom sheet — NormalSoil content: title "Đất thường", action "Cuốc đất"
- [x] 1.9 Bottom sheet — TilledSoil content: title "Đất đã cuốc", actions "Gieo hạt" + "Mở thanh hạt giống"
- [x] 1.10 Bottom sheet — PlantedGrowing content: crop name, "Đang phát triển", time remaining, conditional care actions
- [x] 1.11 Bottom sheet — PlantedReady content: crop name, "Sẵn sàng thu hoạch", action "Thu hoạch"
- [x] 1.12 Bottom sheet — PlantedDead content: crop name, "Đã chết", action "Dọn xác cây"
- [x] 1.13 Implement 2-level sheet: collapse (260h) and expand (540h) with grab handle
- [x] 1.14 Verify: bottom sheet does NOT open during an active drag
- [x] 1.15 Verify: conditional actions in PlantedGrowing sheet are hidden when condition is false

---

## Phase 2 — Tool Mode Framework

- [ ] 2.1 Implement ToolMode enum: None, HoeMode, SeedSelectMode, PlantingMode, WaterMode, HarvestMode, WeedMode, PestMode
- [ ] 2.2 Implement ToolModeManager: SetMode(), ExitMode(), CurrentMode property, OnModeChanged event
- [ ] 2.3 Wire toolbar buttons: each tap calls ToolModeManager.SetMode(); tapping active tool calls ExitMode()
- [ ] 2.4 Implement ModeBar: shows current tool icon + mode name + X button; X calls ExitMode()
- [ ] 2.5 ModeBar visibility: visible only when CurrentMode != None; hidden when bottom sheet is open
- [ ] 2.6 Implement InputRouter: distinguish tap vs drag (threshold ~10px), route single-finger to tool action
- [ ] 2.7 Verify: finger lift does NOT call ExitMode()
- [ ] 2.8 Verify: two-finger pan/pinch does NOT call ExitMode()
- [ ] 2.9 Verify: entering a new tool replaces current mode (no double-mode)
- [ ] 2.10 Implement TileHighlightManager: listen to OnModeChanged, highlight valid tiles per mode
- [ ] 2.11 Implement pointed-tile highlight: brighter border + glow on tile under finger
- [ ] 2.12 Implement invalid-tile feedback: grey/red-light tint
- [ ] 2.13 Verify: toolbar button shows active state (scale 1.06, bright fill) when mode is active

---

## Phase 3 — Planting Flow

- [ ] 3.1 Implement SeedInventoryManager: holds seed counts by type, exposes GetOwnedSeeds() sorted by grow time
- [ ] 3.2 Build SeedTray UI: horizontal list of SeedCards, shop shortcut at right end
- [ ] 3.3 Implement SeedCard: 160×132, icon 56×56, name 16–18pt, grow time 14–16pt
- [ ] 3.4 SeedCard states: Default / Selected (scale 1.04, bright border) / OutOfStock (reduced opacity, qty=0, not tappable)
- [ ] 3.5 Show SeedTray when entering SeedSelectMode; hide when mode changes away
- [ ] 3.6 Tapping a seed card → enter PlantingMode(seedType); hide SeedTray; show ModeBar
- [ ] 3.7 Implement planting: tap TilledSoil → set to PlantedGrowing with seedType, deduct 1 from inventory
- [ ] 3.8 Implement drag planting: each TilledSoil tile entered during drag → plant immediately
- [ ] 3.9 Invalid targets during planting drag → silently skip (no popup, no mode change)
- [ ] 3.10 Out-of-seed mid-drag: stop planting, end drag chain, show SeedTray, stay in SeedSelectMode
- [ ] 3.11 Shop shortcut in SeedTray: open Shop at Seed tab
- [ ] 3.12 Verify: SeedTray and BottomSheet never open simultaneously (UILayerManager enforces)
- [ ] 3.13 Verify: entering seed mode closes BottomSheet completely before showing SeedTray

---

## Phase 4 — Care Flow

- [ ] 4.1 Implement WaterMode tool: tap/drag PlantedGrowing → set isWatered=true
- [ ] 4.2 Already-watered tile: accept action (don't interrupt drag), do not apply extra effect
- [ ] 4.3 Implement WeedMode tool: tap/drag tile with hasWeeds → set hasWeeds=false; skip tiles without weeds silently
- [ ] 4.4 Implement PestMode tool: tap/drag tile with hasPests → set hasPests=false; skip tiles without pests silently
- [ ] 4.5 Implement CropGrowthManager: per-tick growth accumulation with problem modifiers
- [ ] 4.6 Random problem generation: spawn dry/weed/pest events per phase transition with defined probabilities
- [ ] 4.7 Problem effect — dry (!isWatered): reduce growth speed
- [ ] 4.8 Problem effect — weeds: reduce growth speed
- [ ] 4.9 Problem effect — pests: reduce growth speed; if pest duration > threshold → PlantedDead
- [ ] 4.10 World feedback — dry: pale soil + crack overlay art
- [ ] 4.11 World feedback — weeds: weed art overlay on tile
- [ ] 4.12 World feedback — pests: pest art on crop
- [ ] 4.13 Problem icon overlay — dry: water drop icon 20×20
- [ ] 4.14 Problem icon overlay — weeds: grass icon 20×20
- [ ] 4.15 Problem icon overlay — pests: bug icon 20×20
- [ ] 4.16 Verify: problem state always shows BOTH world art change AND icon (neither alone is acceptable)

---

## Phase 5 — Harvest & Dead Plant Flow

- [ ] 5.1 Implement growth phase advancement: growthProgress milestones trigger crop art updates
- [ ] 5.2 Implement PlantedReady transition: when growthProgress >= 1.0 → set state PlantedReady
- [ ] 5.3 HarvestMode: tap/drag PlantedReady → set state to NormalSoil, award crop item
- [ ] 5.4 HarvestMode: skip non-PlantedReady tiles silently (no popup)
- [ ] 5.5 Verify: after harvest, tile returns to NormalSoil (not TilledSoil — player must hoe again)
- [ ] 5.6 Implement PlantedDead state: show wilted plant art on tile
- [ ] 5.7 Dead plant cleanup: tap/drag PlantedDead → set to NormalSoil, award "cỏ" item
- [ ] 5.8 Implement timer display rule: only show timer when tile is selected OR isNearReady
- [ ] 5.9 Implement hoe drag conflict: record conflicting tiles (planted) during drag, show popup only on drag end
- [ ] 5.10 PopupConfirm: container 760×320 radius 28, state conflict count, buttons Hủy + Xác nhận 56h 180w-min
- [ ] 5.11 Verify: popup never fires during active drag chain
- [ ] 5.12 Verify: growth phases are readable by plant silhouette alone in world view

---

## Phase 6 — Polish (only after Phases 0–5 verified)

- [ ] 6.1 Add button press scale feedback: press 0.96–0.98, release normal
- [ ] 6.2 Tool active scale animation: 1.0 → 1.06 ease
- [ ] 6.3 Seed card selected animation: 1.0 → 1.04
- [ ] 6.4 Bottom sheet slide animation: smooth slide up/down, no strong bounce
- [ ] 6.5 Tile action success flash/glow (short)
- [ ] 6.6 Pointed tile border glow refinement
- [ ] 6.7 Mode bar appear/disappear transition
- [ ] 6.8 Seed tray slide-up transition
- [ ] 6.9 Growth phase transition particle or visual pop
- [ ] 6.10 Final acceptance checklist sweep (see requirements.md §1–§11)
