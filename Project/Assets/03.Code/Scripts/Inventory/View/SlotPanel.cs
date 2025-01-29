using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotPanel : MonoBehaviour, IPointerClickHandler
{
    public InventoryPanel inventoryPanel;
    public int slotNumber { get; set; }
    public bool isOccupied { get; private set; }
    public bool isEquipped { get; private set; }
    
    public Item item { get; private set; }

    private Image icon;
    
    
    public void SetItem(Item item)
    {
        this.item = item;
        isOccupied = true;
        icon = Instantiate(item.icon, transform);
        
    }
    
    public void RemoveItem()
    {
        item = null;
        isOccupied = false;
        Destroy(icon.gameObject);
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isOccupied)
            return;
        inventoryPanel.itemPopup.SetItem(item, slotNumber);
        inventoryPanel.itemPopup.OpenPopup();
        // inventoryPanel.RemoveButtonClick(slotNumber);
    }
    
}
