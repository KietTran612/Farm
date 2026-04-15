using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Farm Game — BottomSheetController
/// Controls the 2-level bottom sheet panel.
/// Reads selected tile's TileData and populates content by state.
/// 
/// Levels:
///   Collapsed: height 260
///   Expanded:  height 540
/// 
/// Conflict rules (enforced here + UILayerManager):
///   - Sheet must NOT open during an active drag
///   - Sheet hides ModeBar while open; restores on close
/// </summary>
public class BottomSheetController : MonoBehaviour
{
    // ── Singleton ─────────────────────────────────────────────────────────────
    public static BottomSheetController Instance { get; private set; }

    // ── Sheet heights ─────────────────────────────────────────────────────────
    private const float HeightCollapsed = 260f;
    private const float HeightExpanded  = 540f;

    // ── Inspector refs ────────────────────────────────────────────────────────
    [Header("Panel")]
    [SerializeField] private RectTransform _sheetRect;
    [SerializeField] private GameObject    _sheetRoot;

    [Header("Content")]
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _statusText;
    [SerializeField] private TextMeshProUGUI _timerText;

    [Header("Action Buttons")]
    [SerializeField] private GameObject _btnHoe;            // "Cuốc đất"
    [SerializeField] private GameObject _btnPlant;          // "Gieo hạt"
    [SerializeField] private GameObject _btnOpenSeedTray;   // "Mở thanh hạt giống"
    [SerializeField] private GameObject _btnWater;          // "Tưới nước"
    [SerializeField] private GameObject _btnWeed;           // "Nhổ cỏ"
    [SerializeField] private GameObject _btnPest;           // "Bắt sâu"
    [SerializeField] private GameObject _btnHarvest;        // "Thu hoạch"
    [SerializeField] private GameObject _btnCleanup;        // "Dọn xác cây"

    [Header("Grab Handle")]
    [SerializeField] private RectTransform _handleRect;

    // ── State ─────────────────────────────────────────────────────────────────
    private Vector2Int _selectedCoord;
    private bool       _isOpen     = false;
    private bool       _isExpanded = false;

    // ── Events ────────────────────────────────────────────────────────────────
    public event Action OnSheetOpened;
    public event Action OnSheetClosed;

    // ─────────────────────────────────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        SetSheetActive(false);
    }

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>Open sheet for the tapped tile. Ignored during active drag.</summary>
    public void OpenForTile(Vector2Int coord)
    {
        // REQ-1.14: Do not open during active drag
        // InputRouter will gate this — TileTapHandler already filters drags,
        // but double-check via UILayerManager if needed.

        _selectedCoord = coord;
        var tile = TileManager.Instance?.GetTile(coord);
        if (tile == null) return;

        PopulateContent(tile);
        SetSheetActive(true);
        SetExpanded(false); // always open collapsed first
        OnSheetOpened?.Invoke();
    }

    /// <summary>Close the sheet.</summary>
    public void Close()
    {
        SetSheetActive(false);
        _isOpen     = false;
        _isExpanded = false;
        OnSheetClosed?.Invoke();
    }

    /// <summary>Toggle between collapsed and expanded.</summary>
    public void ToggleExpand()
    {
        SetExpanded(!_isExpanded);
    }

    public bool IsOpen     => _isOpen;
    public bool IsExpanded => _isExpanded;

    // ── Content population ────────────────────────────────────────────────────

    private void PopulateContent(TileData tile)
    {
        // Hide all action buttons first
        SetAllActionsHidden();

        switch (tile.state)
        {
            case TileState.WildGrass:
                SetTitle("Cỏ hoang");
                SetStatus("");
                SetTimer("");
                Show(_btnHoe);
                break;

            case TileState.NormalSoil:
                SetTitle("Đất thường");
                SetStatus("");
                SetTimer("");
                Show(_btnHoe);
                break;

            case TileState.TilledSoil:
                SetTitle("Đất đã cuốc");
                SetStatus("");
                SetTimer("");
                Show(_btnPlant);
                Show(_btnOpenSeedTray);
                break;

            case TileState.PlantedGrowing:
                SetTitle(string.IsNullOrEmpty(tile.cropType) ? "Cây trồng" : tile.cropType);
                SetStatus("Đang phát triển");
                // Timer only shown when selected (REQ-10.2) — we are selected here
                SetTimer(FormatTimeRemaining(tile));
                // Conditional care actions (REQ-9.4)
                if (!tile.isWatered) Show(_btnWater);
                if (tile.hasWeeds)   Show(_btnWeed);
                if (tile.hasPests)   Show(_btnPest);
                break;

            case TileState.PlantedReady:
                SetTitle(string.IsNullOrEmpty(tile.cropType) ? "Cây trồng" : tile.cropType);
                SetStatus("Sẵn sàng thu hoạch");
                SetTimer("");
                Show(_btnHarvest);
                break;

            case TileState.PlantedDead:
                SetTitle(string.IsNullOrEmpty(tile.cropType) ? "Cây trồng" : tile.cropType);
                SetStatus("Đã chết");
                SetTimer("");
                Show(_btnCleanup);
                break;
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void SetAllActionsHidden()
    {
        Hide(_btnHoe);
        Hide(_btnPlant);
        Hide(_btnOpenSeedTray);
        Hide(_btnWater);
        Hide(_btnWeed);
        Hide(_btnPest);
        Hide(_btnHarvest);
        Hide(_btnCleanup);
    }

    private void SetTitle(string text)
    {
        if (_titleText != null) _titleText.text = text;
    }

    private void SetStatus(string text)
    {
        if (_statusText != null)
        {
            _statusText.text = text;
            _statusText.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }

    private void SetTimer(string text)
    {
        if (_timerText != null)
        {
            _timerText.text = text;
            _timerText.gameObject.SetActive(!string.IsNullOrEmpty(text));
        }
    }

    private static void Show(GameObject go) { if (go != null) go.SetActive(true); }
    private static void Hide(GameObject go) { if (go != null) go.SetActive(false); }

    private void SetSheetActive(bool active)
    {
        _isOpen = active;
        if (_sheetRoot != null) _sheetRoot.SetActive(active);
    }

    private void SetExpanded(bool expanded)
    {
        _isExpanded = expanded;
        if (_sheetRect != null)
            _sheetRect.sizeDelta = new Vector2(_sheetRect.sizeDelta.x,
                expanded ? HeightExpanded : HeightCollapsed);
    }

    private static string FormatTimeRemaining(TileData tile)
    {
        // Placeholder — CropGrowthManager will provide real time in Phase 4
        float remaining = (1f - tile.growthProgress) * 60f; // rough estimate
        int mins = Mathf.FloorToInt(remaining / 60f);
        int secs = Mathf.FloorToInt(remaining % 60f);
        return mins > 0 ? $"{mins}m {secs}s" : $"{secs}s";
    }

    // ── Grab handle drag (expand/collapse) ───────────────────────────────────

    private Vector2 _dragStart;
    private float   _dragStartHeight;

    public void OnHandleDragBegin(Vector2 screenPos)
    {
        _dragStart       = screenPos;
        _dragStartHeight = _sheetRect != null ? _sheetRect.sizeDelta.y : HeightCollapsed;
    }

    public void OnHandleDrag(Vector2 screenPos)
    {
        if (_sheetRect == null) return;
        float delta  = screenPos.y - _dragStart.y;
        float newH   = Mathf.Clamp(_dragStartHeight + delta, HeightCollapsed, HeightExpanded);
        _sheetRect.sizeDelta = new Vector2(_sheetRect.sizeDelta.x, newH);
    }

    public void OnHandleDragEnd(Vector2 screenPos)
    {
        if (_sheetRect == null) return;
        float mid = (HeightCollapsed + HeightExpanded) * 0.5f;
        SetExpanded(_sheetRect.sizeDelta.y >= mid);
    }
}
