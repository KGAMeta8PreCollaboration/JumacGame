using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MerchantNPC : CombatNPC
{
	private Shop _shop;
	protected override void CombatChoice()
	{
		base.CombatChoice();
		if (_shop == null)
			_shop = FindObjectOfType<Shop>(true);
		InteractionButton shopIButton = Instantiate<InteractionButton>(_interactionButtonPrefab, GameObject.Find("InteractView").transform);
		_buttons.Add(shopIButton);
		shopIButton.SetTitle("상점 열기");
		Button shopButton = shopIButton.GetComponent<Button>();
		shopButton.onClick.AddListener(() => _shop.OpenShop());
		shopButton.onClick.AddListener(DestroyButtonAll);

	}
}
