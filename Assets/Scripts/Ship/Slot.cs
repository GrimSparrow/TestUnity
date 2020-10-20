using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields

    private IntVector2 gridPos;
    private IntVector2 startItemPos;
    private BaseItem storedItem;
    
    private bool isOccupied = false;
    private bool isBlocked = false;

    #endregion

    #region Properties

    public IntVector2 GridPos
    {
        get => gridPos;
        set => gridPos = value;
    }

    public IntVector2 StartItemPos
    {
        get => startItemPos;
        set => startItemPos = value;
    }

    public BaseItem StoredItem
    {
        get => storedItem;
        set => storedItem = value;
    }

    public bool IsOccupied
    {
        get => isOccupied;
        set => isOccupied = value;
    }

    public bool IsBlocked
    {
        get => isBlocked;
        set => isBlocked = value;
    }

    #endregion

    #region Events

    public void OnDrop(PointerEventData eventData)
    {
        ShipController.Instance.SetItemOnSlots(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShipController.Instance.CheckSlotEnterState(this);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        ShipController.Instance.LeaveSlot(this);
    }

    #endregion
}
