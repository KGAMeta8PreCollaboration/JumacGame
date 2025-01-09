using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InputFieldPopup : Popup
{
	[SerializeField] protected Text titleText;
	[SerializeField] protected InputField inputField;

	public void SetPopup(string title = "", Action<string> callback = null)
	{
		titleText.text = title;
		closeAction = () => callback(inputField.text);
	}
}
