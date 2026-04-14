---
name: phase-gate
description: Before starting a new implementation phase, verify the previous phase passes its acceptance checklist
trigger:
  - event: user_message
    pattern: "*(phase|Phase) [1-6]*"
---

The user is about to start or continue an implementation phase.

Before proceeding, check `.kiro/specs/unity-farm-game/tasks.md` and verify:

1. **What is the current phase being requested?** (Phase 0–6)
2. **What is the previous phase?** (current - 1)
3. **Are all tasks in the previous phase marked complete?** (all `- [x]` entries)

If the previous phase has unchecked tasks (`- [ ]`):
- List the incomplete tasks
- Warn that starting the next phase before completing the previous one risks spec violations
- Ask the user to confirm they want to continue anyway

If the previous phase is fully complete OR this is Phase 0:
- Confirm it's safe to proceed
- Summarize what Phase N requires based on tasks.md
- Remind the agent of any hard rules from agent-rules.md that are especially relevant to this phase
