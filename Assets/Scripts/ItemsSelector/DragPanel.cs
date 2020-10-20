using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragPanel : MonoBehaviour
{
    #region Fields
    
    [SerializeField] 
    private GameObject dragPrefab;
    
    [SerializeField] 
    private GameObject buttonPrefab;
    
    [SerializeField] 
    private Transform scrollViewContent;

    private List<BaseItem> currentItems;
    
    private List<BaseItem> items;
    
    #endregion

    #region Mono Events

    private void Start()
    {
        items = ResourcesManager.Instance.AllItems;
        currentItems = new List<BaseItem>();
        AllCategories();
    }
    
    #endregion

    #region Main fuctionality

     public void AllCategories()
    {
        ClearDragPanel();
        CreateSubcategoryButtons(typeof(BaseItem));
    }

    private void CreateSubcategoryButtons(Type selectedCategory, bool isLast = false)
    {
        ClearDragPanel();
        
        var subclassTypes = Assembly
            .GetAssembly(selectedCategory)
            .GetTypes()
            .Where(t => t.IsSubclassOf(selectedCategory) && t.BaseType == selectedCategory);

        foreach (var type in subclassTypes)
        {
            var buttonObject = Instantiate(buttonPrefab, scrollViewContent);

            var button = buttonObject.GetComponent<Button>();
            
            button.onClick.AddListener(() =>
            {
                if (isLast)
                {
                    FilterBySubcategory(type.Name);
                }
                else
                {
                    CreateSubcategoryButtons(type, true); 
                }
            });
            
            var classDisplayName =
                ((DisplayNameAttribute)
                    type
                        .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                        .FirstOrDefault())?.DisplayName;
            
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(classDisplayName);
        }
    }

    private void FilterBySubcategory(string className)
    {
        ClearDragPanel();
        var type = Type.GetType(className);

        currentItems = items.Where(i => i.GetType() == type).ToList();
        
        BuildItems();
    }
    
    private void BuildItems()
    {
        for (int i = 0; i < currentItems.Count; ++i)
        {
            var dragObject = Instantiate(dragPrefab, scrollViewContent);

            var dragElement = dragObject.GetComponent<DragElement>();

            dragObject.GetComponent<Image>().sprite = currentItems[i].Sprite;
            dragElement.DefaultParentTransform = scrollViewContent;
            dragElement.DragParentTransform = transform;
            dragElement.SiblingIndex = i;
            dragElement.MainImage = currentItems[i].Sprite;
            dragElement.ItemSize = currentItems[i].ItemSize;
            dragElement.Item = currentItems[i].Item;
        }
    }

    private void ClearDragPanel()
    {
        foreach (Transform child in scrollViewContent)
        {
            Destroy(child.gameObject);
        }
    }

    #endregion
}
