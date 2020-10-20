using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    #region Fields

    private static IntVector2 itemSize;
    
    private static int checkInt;
    
    private static GameObject selectedItem;
    
    private static ShipController instance;

    private IntVector2 otherItemPos = IntVector2.zero;

    private ShipViewer grid;

    private ShipSchemeGenerator currentShipScheme;

    #endregion

    #region Properties

    public static IntVector2 ItemSize
    {
        get => itemSize;
        set => itemSize = value;
    }

    public static GameObject SelectedItem
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
            case 0: //устанавливаем в пустую ячейку
                StoreItem(slot);
                break;
            case 1: //устанавливаем в занятую ячейку
                var otherSlot = grid.SlotGrid[otherItemPos.x, otherItemPos.y].GetComponent<Slot>();
                RemoveItem(otherSlot);
                StoreItem(slot);
                break;
            case 2: //Занято двумя элементами, запрещено заменять
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

    private Color GetColor(int checkId)
    {
        var color = Color.white;
        
        switch (checkId)
        {
            case 0:
                color = Color.green;
                break;
            case 1:
                color = Color.yellow;
                break;
            case 2:
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
                checkInt = 2;
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
    
    private int SlotCheck(Slot slot)
    {
        IEquipableItem obj = null;
        var checkArea = GetCheckArea(slot);

        for (var y = 0; y < checkArea.y; y++)
        {
            for (var x = 0; x < checkArea.x; x++)
            {
                var currentSlot = grid.SlotGrid[x + slot.GridPos.x, y + slot.GridPos.y].GetComponent<Slot>();
                if (currentSlot.IsBlocked)
                {
                    return 2;
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
                        return 2;
                    }
                }
            }
        }
        
        return obj == null ? 0 : 1;
    }

    private IntVector2 GetCheckArea(Slot slot)
    {
        IntVector2 checkArea;
        
        checkArea.x = Mathf.Clamp(itemSize.x, 0, grid.SlotGrid.GetLength(0) - slot.GridPos.x);
        checkArea.y = Mathf.Clamp(itemSize.y, 0, grid.SlotGrid.GetLength(1) - slot.GridPos.y);

        return checkArea;
    }

    #endregion
}
