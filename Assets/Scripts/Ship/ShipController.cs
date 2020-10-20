using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    private enum CheckCellState
    {
        Empty,
        FilledByOne,
        FullFilled
    }

    #region Fields
    
    private static ShipController instance;

    private Vector2Int itemSize;
    
    private CheckCellState checkInt;
    
    private GameObject selectedItem;

    private Vector2Int otherItemPos = Vector2Int.zero;

    private ShipViewer grid;

    private ShipSchemeGenerator currentShipScheme;

    #endregion

    #region Properties

    public Vector2Int ItemSize
    {
        get => itemSize;
        set => itemSize = value;
    }

    public GameObject SelectedItem
    {
        get => selectedItem;
        set => selectedItem = value;
    }

    public static ShipController Instance
    {
        get => instance;
    }

    public ShipSchemeGenerator CurrentShipScheme
    {
        get => currentShipScheme;
        set => currentShipScheme = value;
    }

    #endregion

    #region Unity Events

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        grid = FindObjectOfType<ShipViewer>();
    }

    #endregion

    #region Save Load

    public void LoadSave(List<SlotSaveData> saves)
    {
        foreach (var save in saves)
        {
            Load(save);
        }
    }

    public void Save()
    {
        var result = new List<SlotSaveData>();
        
        foreach (var slot in grid.SlotGrid)
        {
            var current = slot.GetComponent<Slot>();
            
            if (current.IsOccupied && current.GridPos == current.StartItemPos)
            {
                result.Add(new SlotSaveData(current));
            }
        }

        currentShipScheme.Save = result;
    }
    
    private void Load(SlotSaveData save)
    {
        var slot = grid.SlotGrid[save.StartItemPos.x, save.StartItemPos.y].GetComponent<Slot>();
        var item = ResourcesManager.Instance.GetItemByName(save.ItemName);
        
        var select = (BaseItem)item.CreateInstance();
        
        for (var y = 0; y < item.ItemSize.y; y++)
        {
            for (var x = 0; x < item.ItemSize.x; x++)
            {

                var currentSlot = grid.SlotGrid[x + slot.GridPos.x, y + slot.GridPos.y].GetComponent<Slot>();
                currentSlot.StartItemPos = slot.GridPos;
                currentSlot.StoredItem = select;
                currentSlot.IsOccupied = true;
                currentSlot.GetComponent<Image>().sprite = item.Sprite;
                grid.SlotGrid[x + slot.GridPos.x, y + slot.GridPos.y].GetComponent<Image>().color = Color.white;
            }
        }
    }

    #endregion

    #region Main Functionality

    public void SetItemOnSlots(Slot slot)
    {
        if (selectedItem == null)
        {
            return;
        }
        
        switch (checkInt)
        {
            case CheckCellState.Empty: //устанавливаем в пустую ячейку
                StoreItem(slot);
                break;
            case CheckCellState.FilledByOne: //устанавливаем в занятую ячейку
                var otherSlot = grid.SlotGrid[otherItemPos.x, otherItemPos.y].GetComponent<Slot>();
                RemoveItem(otherSlot);
                StoreItem(slot);
                break;
            case CheckCellState.FullFilled: //Занято двумя элементами, запрещено заменять
                LeaveSlot(slot);
                break;
        }
    }
    
    public void CheckSlotEnterState(Slot curentSlot)
    {
        checkInt = SlotCheck(curentSlot);
        var color = GetColor(checkInt);
        
        GridColorChange(curentSlot, color, true);
    }

    public void LeaveSlot(Slot curentSlot)
    {
        GridColorChange(curentSlot, Color.white, false);
    }

    public bool IsFull()
    {
        for (var y = 0; y < grid.SlotGrid.GetLength(0); y++)
        {
            for (var x = 0; x < grid.SlotGrid.GetLength(0); x++)
            {
                var slot = grid.SlotGrid[x, y].GetComponent<Slot>();

                if (!slot.IsBlocked && !slot.IsOccupied)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void StoreItem(Slot slot)
    {
        
        var select = (BaseItem)selectedItem.GetComponent<DragElement>().Item.CreateInstance();
        
        for (var y = 0; y < itemSize.y; y++)
        {
            for (var x = 0; x < itemSize.x; x++)
            {

                var currentSlot = grid.SlotGrid[x + slot.GridPos.x, y + slot.GridPos.y].GetComponent<Slot>();
                currentSlot.StartItemPos = slot.GridPos;
                currentSlot.StoredItem = select;
                currentSlot.IsOccupied = true;
                currentSlot.GetComponent<Image>().sprite = selectedItem.GetComponent<DragElement>().MainImage;
                grid.SlotGrid[x + slot.GridPos.x, y + slot.GridPos.y].GetComponent<Image>().color = Color.white;
            }
        }
    }

    private void RemoveItem(Slot slot)
    {
        var removeItemSize = slot.StoredItem.ItemSize;
        
        for (var y = 0; y < removeItemSize.y; y++)
        {
            for (var x = 0; x < removeItemSize.x; x++)
            {
                var currentSlot = grid.SlotGrid[x + slot.StartItemPos.x, y + slot.StartItemPos.y].GetComponent<Slot>();
                currentSlot.GetComponent<Image>().sprite = null;
                currentSlot.StoredItem = null;
                currentSlot.IsOccupied = false;
            }
        }
    }

    private Color GetColor(CheckCellState checkState)
    {
        var color = Color.white;
        
        switch (checkState)
        {
            case CheckCellState.Empty:
                color = Color.green;
                break;
            case CheckCellState.FilledByOne:
                color = Color.yellow;
                break;
            case CheckCellState.FullFilled:
                color = Color.red; 
                break;
        }

        return color;
    }
    
    private void GridColorChange(Slot slot, Color color, bool enter)
    {
        var checkArea = GetCheckArea(slot);

        if ((itemSize.x + slot.GridPos.x > grid.SlotGrid.GetLength(0)) || (itemSize.y + slot.GridPos.y > grid.SlotGrid.GetLength(1)))
        {
            if (enter)
            {
                color = Color.red;
                checkInt = CheckCellState.FullFilled;
            }  
        }
        for (var y = 0; y < checkArea.y; y++)
        {
            for (var x = 0; x < checkArea.x; x++)
            {
                var curSlot = grid.SlotGrid[x + slot.GridPos.x, y + slot.GridPos.y];
                var isBlocked = curSlot.GetComponent<Slot>().IsBlocked;
                curSlot.GetComponent<Image>().color = isBlocked ? Color.clear : color;
            }
        }
    }
    
    private CheckCellState SlotCheck(Slot slot)
    {
        BaseItem obj = null;
        var checkArea = GetCheckArea(slot);

        for (var y = 0; y < checkArea.y; y++)
        {
            for (var x = 0; x < checkArea.x; x++)
            {
                var currentSlot = grid.SlotGrid[x + slot.GridPos.x, y + slot.GridPos.y].GetComponent<Slot>();
                if (currentSlot.IsBlocked)
                {
                    return CheckCellState.FullFilled;
                }
                if (currentSlot.IsOccupied)
                {
                    if (obj == null)
                    {
                        obj = currentSlot.StoredItem;
                        otherItemPos = currentSlot.StartItemPos;
                    }
                    else if (obj != currentSlot.StoredItem)
                    {
                        return CheckCellState.FullFilled;
                    }
                }
            }
        }
        
        return obj == null ? CheckCellState.Empty : CheckCellState.FilledByOne;
    }

    private Vector2Int GetCheckArea(Slot slot)
    {
        Vector2Int checkArea = Vector2Int.zero;
        
        checkArea.x = Mathf.Clamp(itemSize.x, 0, grid.SlotGrid.GetLength(0) - slot.GridPos.x);
        checkArea.y = Mathf.Clamp(itemSize.y, 0, grid.SlotGrid.GetLength(1) - slot.GridPos.y);

        return checkArea;
    }

    #endregion
}
