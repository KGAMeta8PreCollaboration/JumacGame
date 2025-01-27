using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
	public GameObject itemSlotPrefab;
	public Transform itemSlotParent;
	public ItemPopup itemPopup;
	
	public event Action<int> OnRemoveItem;
	public event Action<int, bool> OnEquipItem;
	public event Action<int> OnUnequipItem;
	
	private List<SlotPanel> slotPanels = new List<SlotPanel>();
	
	private void Awake()
	{
		print("InventoryPanel Awake");
		for (int i = 0; i < 20; i++)
			CreateSlotPanel(i);
		
		itemPopup.inventoryPanel = this;
		ClosePanel();
	}

	public void AddItem(int slotNumber, Item item)
	{
		print("InventoryPanel AddItem");
		if (slotPanels.Count > slotNumber && !slotPanels[slotNumber].isOccupied)
		{
			slotPanels[slotNumber].SetItem(item);
		}
		if (slotPanels.Count <= slotNumber)
		{
			for (int i = slotPanels.Count; i < slotNumber + 5; i++)
			{
				CreateSlotPanel(i);
			}
			slotPanels[slotNumber].SetItem(item);
		}
	}

	public void RemoveItem(int slotNumber)
	{
		if (slotPanels.Count > slotNumber && slotPanels[slotNumber].isOccupied)
		{
			slotPanels[slotNumber].RemoveItem();
		}
	}

	public void RemoveButtonClick(int slotNumber)
	{
		OnRemoveItem?.Invoke(slotNumber);
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
	public void EquipItem(int slotNumber)
	{
		print($"EquipButtonClick : {OnEquipItem}");
		OnEquipItem?.Invoke(slotNumber, true);
	}
	
	public void UnequipItem(int slotNumber)
	{
		OnUnequipItem?.Invoke(slotNumber);
	}
}