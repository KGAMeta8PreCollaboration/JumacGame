using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSellItemPopup : MonoBehaviour
{
	private TextMeshProUGUI _titleText;
	private Image _iconImage;
	private List<TextMeshProUGUI> _descriptionTexts = new List<TextMeshProUGUI>();
	private TextMeshProUGUI _buyPriceText;
	private Button _buyButton;
	private Shop _shop;
	private Item _item;
	
	private ConfirmPopup _confirmPopup;
	private SuccessPopup _successPopup;
	private FailPopup _failPopup;
	
	private void Init()
	{
		_titleText = transform.Find("ItemTitle").GetComponent<TextMeshProUGUI>();
		_iconImage = transform.Find("ItemImage").GetComponent<Image>();
		_buyButton = transform.Find("BuyButton").GetComponent<Button>();
		_buyPriceText = transform.Find("Gold/GoldText").GetComponent<TextMeshProUGUI>();
		_confirmPopup = FindObjectOfType<ConfirmPopup>(true);
		_successPopup = FindObjectOfType<SuccessPopup>(true);
		_failPopup = FindObjectOfType<FailPopup>(true);
		TextMeshProUGUI[] childrens  = transform.Find("Descriptions").GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < childrens.Length; i++)
			_descriptionTexts.Add(childrens[i]);
	}
	
	public void SetShop(Shop shop)
	{
		_shop = shop;
	}

	private void Awake()
	{
		Init();
	}


	private void Start()
	{
		ClosePopup();
		// Reset을해도 자꾸 참조가 풀려서 Start에서 참조를 다시받아옴 :(
		_buyButton.onClick.AddListener(BuyButtonClick);
	}
	
	private void BuyButtonClick()
	{
		_confirmPopup.yesButton.onClick.AddListener(() =>
		{
			if (_shop.SellItem(_item, _item.buyPrice))
				_successPopup.OpenPopup();
			else
				_failPopup.OpenPopup();
		});
		_confirmPopup.OpenPopup();
		// _shop.SellItem(_item, _item.buyPrice);
		print("물건 살래!!");
	}
	

	public void SetItem(Item item)
	{
		_item = item;
		_titleText.text = item.itemName;
		_iconImage.sprite = item.icon.sprite;
		for (int i = 0; i < item.descriptions.Length; i++)
			_descriptionTexts[i].text = item.descriptions[i];
		_buyPriceText.text = item.buyPrice.ToString();
	}
	
	public void OpenPopup()
	{
		gameObject.SetActive(true);
	}
	
	public void ClosePopup()
	{
		gameObject.SetActive(false);
	}
	
}
