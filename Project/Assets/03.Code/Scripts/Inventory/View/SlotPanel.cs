using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotPanel : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public int slotNumber { get; set; }
    public bool isOccupied { get; private set; }
    
    public Item item { get; private set; }

    private Image icon;
    
    
    public void SetItem(Item item)
    {
        print("SlotPanel SetItem");
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
}
