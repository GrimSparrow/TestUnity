using System;

[Serializable]
public class SlotSaveData
{
    #region Fields

    private String itemName;
    
    private IntVector2 startItemPos;

    #endregion

    #region Properties

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public IntVector2 StartItemPos
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