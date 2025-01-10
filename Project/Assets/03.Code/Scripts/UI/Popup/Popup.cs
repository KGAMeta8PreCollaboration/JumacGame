using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
	protected Action closeAction;

	[SerializeField] protected Button closeButton;

	protected virtual void OnEnable()
	{
		closeButton.onClick.AddListener(CloseButtonClick);
	}

	protected virtual void OnDisable()
	{
		closeButton.onClick.RemoveListener(CloseButtonClick);
	}

	private void CloseButtonClick()
	{
		PopupManager.Instance.PopupClose();
		closeAction?.Invoke();
	}
}
