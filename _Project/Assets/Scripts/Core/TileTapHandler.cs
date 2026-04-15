using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Farm Game — TileTapHandler
/// Attached to each tile GameObject.
/// Detects tap (pointer down + up without drag) and notifies BottomSheetController.
/// Drag detection threshold: ~10px (matches InputRouter spec).
/// </summary>
public class TileTapHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private const float DragThreshold = 10f; // pixels

    private Vector2Int _coord;
    private Vector2    _pointerDownPos;
    private bool       _isDragging;

    // ─────────────────────────────────────────────────────────────────────────
    public void Init(Vector2Int coord)
    {
        _coord = coord;
    }

    // ── IPointerDownHandler ───────────────────────────────────────────────────
    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDownPos = eventData.position;
        _isDragging     = false;
    }

    // ── IDragHandler ──────────────────────────────────────────────────────────
    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            float dist = Vector2.Distance(eventData.position, _pointerDownPos);
            if (dist > DragThreshold) _isDragging = true;
        }
    }

    // ── IPointerUpHandler ─────────────────────────────────────────────────────
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_isDragging) return; // drag — not a tap

        // Single-finger tap confirmed → open bottom sheet
        BottomSheetController.Instance?.OpenForTile(_coord);
    }
}
