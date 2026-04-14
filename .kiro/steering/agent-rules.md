---
inclusion: always
---

# Farm Game — Agent Hard Rules

These rules are non-negotiable. Any agent working on this project must follow them exactly.

---

## Layout rules — MUST follow
- Canvas is exactly `1080 × 1920`, portrait only
- Farm world is full-screen — no fixed panels splitting the viewport
- Bottom toolbar is always visible during farm gameplay
- There are exactly 6 primary tool slots + More (7 total)
- Mode bar sits above the toolbar (gap: 16px) and is only visible when a tool mode is active
- Seed tray is a horizontal floating strip that rises from below — NOT a shop
- Bottom sheet is a 2-level sheet (collapsed 260px, expanded 540px) — NOT a mini popup

## Conflict rules — MUST follow
- Seed tray and bottom sheet (at any level) must NEVER be open simultaneously
- When entering seed mode → close bottom sheet first, then show seed tray
- When bottom sheet opens → hide seed tray AND hide mode bar
- When bottom sheet closes → restore mode bar if tool mode is still active

## Input rules — MUST follow
- 1 finger = tool action or tile select — never pan camera
- 2 fingers = pan camera
- 2-finger pinch = zoom
- Lifting finger does NOT exit tool mode
- Camera gestures do NOT exit tool mode
- Bottom sheet must NOT open during an active drag
- Bottom sheet must NOT interrupt a drag chain

## Tool mode rules — MUST follow
- Tool mode persists until: player taps same tool, player taps X on mode bar, tool has no valid targets
- Selecting a different tool replaces current mode (no stacking)
- Active tool mode must show all 3 signals: button highlighted + valid tiles highlighted + mode bar visible
- Popup confirm must NEVER fire during an active drag chain

## Tile rules — MUST follow
- After harvest → NormalSoil (NOT TilledSoil — player must hoe again)
- After dead-crop cleanup → NormalSoil + award "cỏ" item
- Already-watered tiles: accept water action (don't interrupt drag) but apply no extra effect
- Watering tool valid target is ONLY PlantedGrowing (not TilledSoil, not PlantedReady)
- Planting tool valid target is ONLY TilledSoil

## Problem state rules — MUST follow
- Problem states (dry, weeds, pests) must show BOTH: world art change AND small icon (20×20)
- Neither world feedback alone nor icon alone is acceptable
- Timer must NOT display on all planted tiles by default — only on selected tile or near-ready tile

---

## Forbidden changes — NEVER do these
- Never split the farm screen with a side panel or inventory panel
- Never shrink the farm into a small frame in the center of the screen
- Never replace the bottom sheet with a floating mini popup above a tile
- Never turn the seed tray into a full shop catalog
- Never auto-exit tool mode after each tap action
- Never exit tool mode on finger lift
- Never show growth timers on all planted tiles simultaneously
- Never allow 1 finger to simultaneously pan camera and use a tool
- Never fire a popup confirm during an active drag chain
