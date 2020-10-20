using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIService : MonoBehaviour
{
    #region Fields

    [SerializeField] 
    private Button previous;
    
    [SerializeField] 
    private Button next;
    
    [SerializeField] 
    private Button confirm;

    [SerializeField] 
    private Button complete;

    [SerializeField] 
    private GameObject itemSelector;

    [SerializeField] 
    private ConfirmationWindow confirmationWindow;
    
    private Canvas canvas;
    
    private List<ShipSchemeGenerator> ships;
    
    private int counter = 0;
    
    private ShipViewer shipViewer;

    #endregion

    #region Unity Events

    void Start()
    {
        ships = ResourcesManager.Instance.AllShips;
        
        shipViewer = FindObjectOfType<ShipViewer>();
        canvas = FindObjectOfType<Canvas>();
        
        previous.onClick.AddListener(SelectPrevious);
        next.onClick.AddListener(SelectNext);
        confirm.onClick.AddListener(ConfirmSelection);
        
        complete.onClick.AddListener(CompleteBuild);
        
        shipViewer.CreateSlots(ships[counter]);
    }

    #endregion

    #region Main Functionality

    private void SelectNext()
    {
        if (counter < ships.Count-1)
        {
            counter++;
            shipViewer.CreateSlots(ships[counter]);
        }
    }

    private void SelectPrevious()
    {
        if (counter > 0)
        {
            counter--;
            shipViewer.CreateSlots(ships[counter]);
        }
    }

    private void CompleteBuild()
    {
        var isShipFull = ShipController.Instance.IsFull();
        
        if (!isShipFull)
        {
            var messageWindow = Instantiate(confirmationWindow, canvas.transform);
            
            messageWindow.Initialize("Имеются незаполненные ячейки, подтвердить завершение сборки?",
                DestroyItemList);
        }
        else
        {
            DestroyItemList();
        }
    }

    private void DestroyItemList()
    {
        var itemList = FindObjectOfType<DragPanel>();
        Destroy(itemList.gameObject);
        ActivateShipSelection(true);
        
        ShipController.Instance.Save();
    }

    private void ConfirmSelection()
    {
        ActivateShipSelection(false);
        Instantiate(itemSelector, canvas.transform);
    }

    private void ActivateShipSelection(bool isActive)
    {
        previous.gameObject.SetActive(isActive);
        next.gameObject.SetActive(isActive);
        confirm.gameObject.SetActive(isActive);
        complete.gameObject.SetActive(!isActive);
    }

    #endregion
}
