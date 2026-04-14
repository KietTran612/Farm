---
inclusion: always
---

# Farm Game — Project Steering

## What this project is
A mobile portrait farming game built in Unity. Canvas: 1080×1920. Portrait only. Isometric-light visual style, modern casual UX. The goal is a clean, fast-feeling farming loop with tap and drag interactions.

## Current scope
Only the farming vertical slice is in scope:
- Clear land (WildGrass/NormalSoil → TilledSoil)
- Plant (TilledSoil → PlantedGrowing)
- Grow (multi-phase with care events)
- Care (water, weed, pest)
- Harvest (PlantedReady → NormalSoil)
- Dead crop cleanup (PlantedDead → NormalSoil)

NOT in scope yet: social, full quest system, complete shop, backpack, decoration, animals, factory.

## Where the specs live
All specs are in `Docs/` and `.kiro/specs/unity-farm-game/`:
- `Docs/farm-screen-uiux-spec-v1.1.md` — master reference (full)
- `Docs/farm-layout-spec-v1.1.md` — pixel-precise dimensions
- `Docs/farm-interaction-spec-v1.1.md` — tool behaviors and drag rules
- `Docs/farm-state-machine-spec-v1.1.md` — tile lifecycle and tool mode states
- `Docs/farm-agent-checklist-v1.1.md` — hard rules and acceptance checklist

## Where Unity code lives
Unity project is under `_Project/`. C# scripts should go in `_Project/Assets/Scripts/`.

## Implementation order
Always follow the 7-phase plan in `.kiro/specs/unity-farm-game/tasks.md`.
Do not start Phase N+1 before Phase N passes the acceptance checklist.
Phase 6 (polish) is the last phase — never add animations before functionality is verified.
