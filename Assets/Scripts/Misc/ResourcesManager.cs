using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    #region Fields

    private static ResourcesManager instance;

    private List<BaseItem> allItems;

    private List<ShipSchemeGenerator> allShips;

    #endregion

    #region Properties

    public static ResourcesManager Instance
    {
        get => instance;
        set => instance = value;
    }

    public List<BaseItem> AllItems
    {
        get => allItems;
        set => allItems = value;
    }

    public List<ShipSchemeGenerator> AllShips
    {
        get => allShips;
        set => allShips = value;
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

        allItems = Resources.LoadAll<BaseItem>("Items").ToList();
        
        allShips = Resources.LoadAll<ShipSchemeGenerator>("ShipSchemes").ToList();
    }

    #endregion

    #region Main Functionality

    public BaseItem GetItemByName(string itemName)
    {
        return allItems.FirstOrDefault(i => i.ItemName == itemName);
    }

    #endregion
    
}
