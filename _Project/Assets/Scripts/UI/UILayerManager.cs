using UnityEngine;

/// <summary>
/// Farm Game — UILayerManager
/// Enforces mutual-exclusion rules between UI layers:
///   - SeedTray and BottomSheet never open simultaneously
///   - ModeBar hides when BottomSheet is open; restores on close
/// 
/// Subscribe to BottomSheetController events and ToolModeManager events.
/// </summary>
public class UILayerManager : MonoBehaviour
{
    [SerializeField] private GameObject _seedTrayRoot;
    [SerializeField] private GameObject _modeBarRoot;

    private bool _modeBarWasActive = false;

    // ─────────────────────────────────────────────────────────────────────────
    private void OnEnable()
    {
        if (BottomSheetController.Instance != null)
        {
            BottomSheetController.Instance.OnSheetOpened += HandleSheetOpened;
            BottomSheetController.Instance.OnSheetClosed += HandleSheetClosed;
        }
    }

    private void OnDisable()
    {
        if (BottomSheetController.Instance != null)
        {
            BottomSheetController.Instance.OnSheetOpened -= HandleSheetOpened;
            BottomSheetController.Instance.OnSheetClosed -= HandleSheetClosed;
        }
    }

    // ── Sheet opened ──────────────────────────────────────────────────────────
    private void HandleSheetOpened()
    {
        // REQ-2.3: Hide seed tray when sheet opens
        if (_seedTrayRoot != null) _seedTrayRoot.SetActive(false);

        // REQ-2.4: Hide mode bar when sheet opens; remember its state
        if (_modeBarRoot != null)
        {
            _modeBarWasActive = _modeBarRoot.activeSelf;
            _modeBarRoot.SetActive(false);
        }
    }

    // ── Sheet closed ──────────────────────────────────────────────────────────
    private void HandleSheetClosed()
    {
        // REQ-2.4: Restore mode bar if it was active before sheet opened
        if (_modeBarRoot != null && _modeBarWasActive)
            _modeBarRoot.SetActive(true);
    }

    // ── Public: called by SeedMode entry ─────────────────────────────────────

    /// <summary>
    /// REQ-2.2: Close sheet first, then show seed tray.
    /// Called by ToolModeManager when entering SeedSelectMode.
    /// </summary>
    public void OnEnterSeedMode()
    {
        BottomSheetController.Instance?.Close();
        if (_seedTrayRoot != null) _seedTrayRoot.SetActive(true);
    }

    /// <summary>Hide seed tray (called when leaving seed mode).</summary>
    public void OnExitSeedMode()
    {
        if (_seedTrayRoot != null) _seedTrayRoot.SetActive(false);
    }
}
