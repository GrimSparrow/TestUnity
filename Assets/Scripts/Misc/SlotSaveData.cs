using System;
using UnityEngine;

[Serializable]
public class SlotSaveData
{
    #region Fields

    private String itemName;
    
    private Vector2Int startItemPos;

    #endregion

    #region Properties

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public Vector2Int StartItemPos
    {
        get => startItemPos;
        set => startItemPos = value;
    }

    #endregion

    #region Constructor

    public SlotSaveData(Slot slot)
    {
        ItemName = slot.StoredItem.ItemName;
        StartItemPos = slot.StartItemPos;
    }

    #endregion
}