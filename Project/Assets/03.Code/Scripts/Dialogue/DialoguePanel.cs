using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialoguePanel : MonoBehaviour
{
	private TextMeshProUGUI _dialogueText;

	private void Reset()
	{
		_dialogueText = GetComponentInChildren<TextMeshProUGUI>();
	}
	
	private void Start()
	{
		_dialogueText = GetComponentInChildren<TextMeshProUGUI>();
		gameObject.SetActive(false);
	}

	public void SetDialogueText(string text)
	{
		if (_dialogueText)
			_dialogueText.text = text;
	}
}
