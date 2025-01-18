// using System.Collections.Generic;
// using UnityEngine;
//
// public class InventoryPanel : MonoBehaviour
// {
// 	public GameObject itemSlotPrefab;
// 	public Transform itemSlotParent;
// 	// public 
// 	
// 	private List<SlotPanel> slotPanels = new List<SlotPanel>();
//
// 	private void Start()
// 	{
// 		for (int i = 0; i < 20; i++)
// 			CreateSlotPanel(i);
// 	}
// 	
// 	public bool AddItem(Item item)
// 	{
// 		print("InventoryPanel AddItem");
// 		for (int i = 0; i < slotPanels.Count; i++)
// 		{
// 			if (!slotPanels[i].isOccupied)
// 			{
// 				slotPanels[i].SetItem(item);
// 				return true;
// 			}
// 		}
// 		return false;
// 	}
// 	
// 	public bool RemoveItem(int slotNumber)
// 	{
// 		if (slotPanels.Count <= slotNumber)
// 			return false;
// 		if (slotPanels[slotNumber].isOccupied)
// 		{
// 			slotPanels[slotNumber].RemoveItem();
// 			return true;
// 		}
// 		return false;
// 	}
// 	
// 	public bool RemoveItem(Item item)
// 	{
// 		for (int i = 0; i < slotPanels.Count; i++)
// 		{
// 			
// 			if (slotPanels[i].isOccupied && slotPanels[i].item == item)
// 			{
// 				print("Remove");
// 				slotPanels[i].RemoveItem();
// 				return true;
// 			}
// 		}
// 		return false;
// 	}
// 	
// 	private void CreateSlotPanel(int slotNumber)
// 	{
// 		GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotParent);
// 		SlotPanel slotPanel = itemSlot.GetComponent<SlotPanel>();
// 		slotPanel.slotNumber = slotNumber;
// 		slotPanel.inventoryPanel = this;
// 		slotPanels.Add(slotPanel);
// 	}
// 	
// 	public void OpenPanel()
// 	{
// 		gameObject.SetActive(true);
// 	}
// 	
// 	public void ClosePanel()
// 	{
// 		gameObject.SetActive(false);
// 	}
//
// }

using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
	public GameObject itemSlotPrefab;
	public Transform itemSlotParent;

	private List<SlotPanel> slotPanels = new List<SlotPanel>();

	private void Start()
	{
		for (int i = 0; i < 20; i++)
			CreateSlotPanel(i);
	}

	public void AddItem(int slotNumber, Item item)
	{
		if (slotPanels.Count > slotNumber && !slotPanels[slotNumber].isOccupied)
		{
			slotPanels[slotNumber].SetItem(item);
		}
	}

	public void RemoveItem(int slotNumber)
	{
		print($"InventoryPanel RemoveItem 1 : {slotNumber}");
		if (slotPanels.Count > slotNumber && slotPanels[slotNumber].isOccupied)
		{
			print($"InventoryPanel RemoveItem 2 : {slotNumber}");
			slotPanels[slotNumber].RemoveItem();
		}
	}

	private void CreateSlotPanel(int slotNumber)
	{
		GameObject itemSlot = Instantiate(itemSlotPrefab, itemSlotParent);
		SlotPanel slotPanel = itemSlot.GetComponent<SlotPanel>();
		slotPanel.slotNumber = slotNumber;
		slotPanel.inventoryPanel = this;
		slotPanels.Add(slotPanel);
	}

	public void OpenPanel()
	{
		gameObject.SetActive(true);
	}

	public void ClosePanel()
	{
		gameObject.SetActive(false);
	}
}