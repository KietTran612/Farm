# Unity Farm Game — Requirements

## Overview
Mobile portrait farming game (1080×1920) with isometric-light visual style. This spec covers the complete farming loop: clear land, till, plant, grow, care, harvest, and handle crop death.

---

## 1. Screen & Layout

### REQ-1.1 Canvas and Orientation
The game shall render at 1080×1920 in portrait-only orientation.

### REQ-1.2 Farm World
The farm world shall be a full-screen visual layer with no fixed panels splitting the screen.

### REQ-1.3 Top HUD
When the farm screen is active, the system shall display a top HUD (height: 92px, Y: 44) containing profile button, level badge, currency capsules (gold + gem), and utility buttons.

### REQ-1.4 Bottom Toolbar
The system shall always display a bottom toolbar (width: 1032, height: 120, bottom offset: 34) containing exactly 6 primary tools + More slot.

### REQ-1.5 Mode Bar
When a tool mode is active, the system shall display a mode bar (width: 560, height: 64) positioned 16px above the toolbar, showing the current tool icon, mode name, and an X close button.
When no tool mode is active, the mode bar shall be hidden.

### REQ-1.6 Seed Tray
The system shall display a seed tray (width: 1032, height: 180) as a horizontal floating strip rising from below, showing only seeds the player currently owns, sorted by grow time (shortest first), with a shortcut to open Shop > Seed tab.

### REQ-1.7 Bottom Sheet
The system shall display a 2-level bottom sheet (collapsed: height 260, expanded: height 540) whose content changes based on the selected tile state.

### REQ-1.8 Side Utility Buttons
The system shall display a right-side stack of utility buttons (72×72 each) starting at Y: 250, in order: Shop, Backpack, Quest placeholder, Social placeholder.

### REQ-1.9 Layer Order
The system shall render layers from back to front: Farm world → Top HUD → Side buttons → Toolbar → Mode bar → Seed tray → Bottom sheet → Popup confirm.

---

## 2. Conflict Rules (Seed Tray vs Bottom Sheet)

### REQ-2.1 Mutual Exclusion
The seed tray and the bottom sheet (at any level) shall never be open at the same time.

### REQ-2.2 Seed Mode Entry
When the player enters seed mode, the system shall fully close the bottom sheet before showing the seed tray.

### REQ-2.3 Bottom Sheet Open
When the bottom sheet is open (collapsed or expanded), the system shall hide the seed tray.

### REQ-2.4 Mode Bar + Bottom Sheet
When the bottom sheet is open at any level, the system shall temporarily hide the mode bar.
When the bottom sheet is closed, the system shall restore the mode bar if a tool mode is still active.

---

## 3. Input & Camera

### REQ-3.1 Single Finger
One finger shall perform tool actions or select a tile. One finger shall not simultaneously pan the camera.

### REQ-3.2 Two Fingers
Two fingers shall pan the camera.

### REQ-3.3 Pinch
A two-finger pinch gesture shall zoom the camera.

### REQ-3.4 Tap vs Drag
A tap on a tile shall open the bottom sheet for that tile (when not in an active drag).
A drag shall process multiple tiles quickly without interruption.

---

## 4. Tool Mode System

### REQ-4.1 Tool Selection
When the player taps a toolbar tool, the system shall enter the corresponding tool mode.

### REQ-4.2 Mode Persistence
The current tool mode shall persist until: the player taps the same tool again, the player taps X on the mode bar, or the tool has no valid targets remaining.

### REQ-4.3 Finger Lift
Lifting the finger (end of drag) shall not exit tool mode.

### REQ-4.4 Camera Gesture
Camera pan and zoom gestures shall not exit tool mode.

### REQ-4.5 Mode Replacement
Selecting a different tool shall replace the current tool mode.

### REQ-4.6 Active Mode Signals
When a tool mode is active, the system shall show all three signals simultaneously: tool button highlighted, valid tiles highlighted, mode bar visible.

---

## 5. Tile States

### REQ-5.1 State List
Each tile shall have exactly one of these states: WildGrass, NormalSoil, TilledSoil, PlantedGrowing, PlantedReady, PlantedDead.

### REQ-5.2 WildGrass → TilledSoil
When the player uses Hoe on a WildGrass tile, the tile state shall become TilledSoil.

### REQ-5.3 NormalSoil → TilledSoil
When the player uses Hoe on a NormalSoil tile, the tile state shall become TilledSoil.

### REQ-5.4 TilledSoil → PlantedGrowing
When the player plants a seed on a TilledSoil tile, the tile state shall become PlantedGrowing with the seed type recorded.

### REQ-5.5 PlantedGrowing → PlantedReady
When a PlantedGrowing tile completes its growth time, the tile state shall become PlantedReady.

### REQ-5.6 PlantedReady → NormalSoil
When the player harvests a PlantedReady tile, the tile state shall become NormalSoil (not TilledSoil).

### REQ-5.7 PlantedGrowing → PlantedDead
When a PlantedGrowing tile has a pest problem left unresolved for too long, the tile state shall become PlantedDead.

### REQ-5.8 PlantedDead → NormalSoil
When the player cleans up a PlantedDead tile, the tile shall become NormalSoil and award a "cỏ" item.

---

## 6. PlantedGrowing Flags

### REQ-6.1 Overlay Flags
A PlantedGrowing tile shall track these boolean flags independently: isWatered, hasWeeds, hasPests, isNearReady.

### REQ-6.2 Random Problem Generation
The system shall randomly generate one or more problems (dry, weeds, pests) at defined probabilities per growth phase.

### REQ-6.3 Problem Effects
Each problem shall slow growth or reduce yield. Pests left unresolved beyond the time limit shall cause plant death.

---

## 7. Growth Phases

### REQ-7.1 Phase Count
Each crop type shall have a minimum of 3 and a maximum of 5 growth phases.

### REQ-7.2 World View Readability
The player shall be able to identify the crop's current phase from the world view by the plant's shape/silhouette alone.

### REQ-7.3 Detail View
When a tile is focused (selected or bottom sheet open), the system shall display the current phase number, progress percentage, and time remaining.

---

## 8. Tool Behaviors

### REQ-8.1 Hoe Tool
When in HoeMode, the system shall process WildGrass and NormalSoil tiles immediately on drag-over.
Tiles in a conflicting state (e.g., planted) during a drag shall be recorded and shown in a confirmation popup only after the drag ends.
The system shall never show a popup during an active drag chain.

### REQ-8.2 Seed Tool — Entry
The player shall enter seed mode via: (a) tap Seed tool → seed tray opens → select seed, or (b) tap TilledSoil → bottom sheet → Gieo hạt → seed tray opens.

### REQ-8.3 Seed Tool — Planting
When PlantingMode is active, tapping a TilledSoil tile shall plant one seed. Dragging shall plant multiple tiles.
Invalid targets shall be silently skipped (no popup, no mode change).

### REQ-8.4 Seed Tool — Out of Stock
When the player runs out of the selected seed mid-drag, the system shall stop planting, end the drag chain, and return to the seed tray view without opening the shop.

### REQ-8.5 Water Tool
When in WaterMode, tapping or dragging a PlantedGrowing tile shall apply water immediately.
If the tile is already watered, the action shall be accepted (drag chain not interrupted) but shall not add extra effect.

### REQ-8.6 Weed Tool
When in WeedMode, tapping or dragging a tile with weeds shall remove weeds immediately. Tiles without weeds shall be silently skipped.

### REQ-8.7 Pest Tool
When in PestMode, tapping or dragging a tile with pests shall remove pests immediately. Tiles without pests shall be silently skipped.

### REQ-8.8 Harvest Tool
When in HarvestMode, tapping or dragging a PlantedReady tile shall harvest it and set the tile to NormalSoil.
Tiles that are not PlantedReady shall be silently skipped (no popup).

### REQ-8.9 Dead Cleanup Tool
When the player activates dead-plant cleanup, tapping or dragging a PlantedDead tile shall clear it, award a "cỏ" item, and set the tile to NormalSoil.

---

## 9. Bottom Sheet Content

### REQ-9.1 WildGrass Sheet
The bottom sheet for WildGrass shall show: title "Cỏ hoang", action "Cuốc đất".

### REQ-9.2 NormalSoil Sheet
The bottom sheet for NormalSoil shall show: title "Đất thường", action "Cuốc đất".

### REQ-9.3 TilledSoil Sheet
The bottom sheet for TilledSoil shall show: title "Đất đã cuốc", actions "Gieo hạt" and "Mở thanh hạt giống".

### REQ-9.4 PlantedGrowing Sheet
The bottom sheet for PlantedGrowing shall show: crop name, status "Đang phát triển", time remaining, and conditional actions: "Tưới nước" only when !isWatered, "Nhổ cỏ" only when hasWeeds, "Bắt sâu" only when hasPests. If no problems, no care actions shall be shown.

### REQ-9.5 PlantedReady Sheet
The bottom sheet for PlantedReady shall show: crop name, status "Sẵn sàng thu hoạch", action "Thu hoạch".

### REQ-9.6 PlantedDead Sheet
The bottom sheet for PlantedDead shall show: crop name, status "Đã chết", action "Dọn xác cây".

---

## 10. Problem State Visuals

### REQ-10.1 World Feedback Mandatory
Every problem state (dry, weeds, pests) shall display both a world-art change (soil cracking, grass growing, pest visible on plant) AND a small icon (20×20) on the tile.

### REQ-10.2 Timer Display
The system shall NOT display growth timers on all planted tiles by default.
Timers shall only appear when: the tile is selected, or the tile is near completion.

---

## 11. Popup Confirm

### REQ-11.1 Usage Scope
A confirmation popup shall only appear for risky or conflicting actions (e.g., hoe drag over planted tiles).

### REQ-11.2 No Mid-Drag Popup
The system shall never show a popup during an active drag chain.

### REQ-11.3 Popup Content
The popup shall clearly state: how many tiles are conflicting, which have already been processed, and which are awaiting confirmation.

### REQ-11.4 Popup Dimensions
The popup container shall be: width 760, height 320, radius 28. Buttons (Hủy / Xác nhận) shall be height 56, min-width 180.
