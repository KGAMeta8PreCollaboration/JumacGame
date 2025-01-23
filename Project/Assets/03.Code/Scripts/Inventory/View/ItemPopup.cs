using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopup : MonoBehaviour
{
	public TextMeshProUGUI titleText;
	public Image iconImage;
	public List<TextMeshProUGUI> descriptionTexts = new List<TextMeshProUGUI>();
	public Button closeButton;
	public Button equipButton;
	public Button unequipButton;
	public Button discardButton;
	
	public InventoryPanel inventoryPanel;
	private int slotNumber;
	
	private void Reset()
	{
		titleText = transform.Find("ItemTitle").GetComponent<TextMeshProUGUI>();
		iconImage = transform.Find("ItemImage").GetComponent<Image>();
		closeButton = transform.Find("CloseButton").GetComponent<Button>();
		equipButton = transform.Find("EquipButton").GetComponent<Button>();
		unequipButton = transform.Find("UnequipButton").GetComponent<Button>();
		discardButton = transform.Find("DiscardButton").GetComponent<Button>();
		TextMeshProUGUI[] childrens  = transform.Find("Descriptions").GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < childrens.Length; i++)
			descriptionTexts.Add(childrens[i]);
	}

	private void Start()
	{
		closeButton.onClick.AddListener(ClosePopup);
		equipButton.onClick.AddListener(EquipButtonClick);
		unequipButton.onClick.AddListener(UnequipButtonClick);
		discardButton.onClick.AddListener(DiscardButtonClick);
	}
	private void DiscardButtonClick()
	{
		inventoryPanel.RemoveButtonClick(slotNumber);
		ClosePopup();
	}

	public void SetItem(Item item, int slotNumber)
	{
		titleText.text = item.itemName;
		iconImage.sprite = item.icon.sprite;
		for (int i = 0; i < item.descriptions.Length; i++)
			descriptionTexts[i].text = item.descriptions[i];
		this.slotNumber = slotNumber;
	}
	
	private void EquipButtonClick()
	{
		print("EquipButtonClick");
		inventoryPanel.EquipItem(slotNumber);
	}
	
	private void UnequipButtonClick()
	{
		print("UnequipButtonClick");
		inventoryPanel.UnequipItem(slotNumber);
	}
	
	private void ClosePopup()
	{
		gameObject.SetActive(false);
	}
	
	public void OpenPopup()
	{
		gameObject.SetActive(true);
	}
}
