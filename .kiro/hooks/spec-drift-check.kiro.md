---
name: spec-drift-check
description: When a spec file in Docs/ is edited, check if design.md or requirements.md in the Kiro spec need updating
trigger:
  - event: file_edit
    pattern: "Docs/*.md"
---

A spec file in `Docs/` was just modified.

Compare the changed file against the Kiro spec files:
- `.kiro/specs/unity-farm-game/requirements.md`
- `.kiro/specs/unity-farm-game/design.md`

Identify any content in the changed `Docs/` file that:
1. **Contradicts** something in the Kiro spec files
2. **Adds a new rule or decision** not yet reflected in the Kiro spec files
3. **Changes a number or dimension** (pixel values, heights, radii) that differs from the Kiro spec

Report the specific differences and suggest the exact edits needed to keep the Kiro specs in sync with the `Docs/` source of truth.

Do not auto-edit — report only, so the user can review and approve changes.
