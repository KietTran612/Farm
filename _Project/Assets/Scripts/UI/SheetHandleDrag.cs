using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Farm Game — SheetHandleDrag
/// Attach to the grab handle of BottomSheet.
/// Routes drag events to BottomSheetController for expand/collapse.
/// </summary>
public class SheetHandleDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        BottomSheetController.Instance?.OnHandleDragBegin(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        BottomSheetController.Instance?.OnHandleDrag(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        BottomSheetController.Instance?.OnHandleDragEnd(eventData.position);
    }

    // Tap on handle = toggle expand/collapse
    public void OnPointerClick(PointerEventData eventData)
    {
        BottomSheetController.Instance?.ToggleExpand();
    }
}
