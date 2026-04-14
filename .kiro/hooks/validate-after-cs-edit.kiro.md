---
name: validate-after-cs-edit
description: After editing any C# script, check that hard rules from agent-rules.md are not violated
trigger:
  - event: file_edit
    pattern: "_Project/Assets/Scripts/**/*.cs"
---

A C# script was just edited in the Farm Unity project.

Check the change against the hard rules in `.kiro/steering/agent-rules.md`. Specifically verify:

1. **Tool mode exit rules** — Does the changed code ever call ExitMode() or set ToolMode to None on finger lift or camera gesture? If yes, flag it.

2. **Seed tray / bottom sheet conflict** — Does the changed code ever show both the seed tray and bottom sheet at the same time? Check UILayerManager calls.

3. **Popup during drag** — Does the changed code ever fire a popup confirm while a drag chain is active? If yes, flag it.

4. **After-harvest state** — Does any harvest logic set tile to TilledSoil after harvest? It must be NormalSoil.

5. **Timer display** — Does any changed UI code show timers on all tiles? It must be limited to selected or near-ready tiles.

If any violation is found, explain which rule is broken and suggest a fix. If no violations, confirm the change is compliant.
