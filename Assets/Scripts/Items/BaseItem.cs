using UnityEngine;

public class BaseItem : ScriptableObject
{
    #region Fields

    [SerializeField] 
    private Sprite sprite;
    
    [SerializeField] 
    private string  itemName;
    
    [SerializeField] 
    private Vector2Int itemSize;
    
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

    public Vector2Int ItemSize
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

    public BaseItem CreateInstance()
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


