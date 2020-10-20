using UnityEngine;

public class BaseItem : ScriptableObject, IEquipableItem
{
    #region Fields

    [SerializeField] 
    private Sprite sprite;
    
    [SerializeField] 
    private string  itemName;
    
    [SerializeField] 
    private IntVector2 itemSize;
    
    [SerializeField] 
    private int strength;
    
    [SerializeField] 
    private int weight;

    #endregion

    #region Properties

    public Sprite Sprite
    {
        get => sprite;
    }

    public string ItemName
    {
        get => itemName;
        set => itemName = value;
    }

    public IntVector2 ItemSize
    {
        get => itemSize;
        set => itemSize = value;
    }

    public int Strength
    {
        get => strength;
        set => weight = value;
    }

    public int Weight
    {
        get => weight;
        set => weight = value;
    }

    public BaseItem Item
    {
        get => this;
    }

    #endregion

    #region MainFunctionality

    public IEquipableItem CreateInstance()
    {
        var clone = Instantiate(this);
        return clone;
    }

    public void DoSmth()
    {
        //Todo
    }

    #endregion
}


