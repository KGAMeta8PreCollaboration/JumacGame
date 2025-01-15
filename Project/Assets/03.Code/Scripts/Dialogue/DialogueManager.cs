using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
	private DialoguePanel _dialoguePanel;
	private Dialogue _currentDialogue;
	private int _currentSentenceIndex;
	private Action afterAction;
	private PlayerInput _playerInput;
	
	private void Start()
	{
		_dialoguePanel = FindObjectOfType<DialoguePanel>(true);
		_playerInput = FindObjectOfType<PlayerInput>(true);
	}
	
	public void SetDialogue(Dialogue dialogue, Action afterAction)
	{
		_currentDialogue = dialogue;
		_currentSentenceIndex = 0;
		this.afterAction = afterAction;
	}
	private string _prevActionMap;
	private string _dialogueActionMap = "Dialogue";
	
	public void StartDialogue()
	{
		_dialoguePanel.gameObject.SetActive(true);
		_dialoguePanel.SetDialogueText(_currentDialogue.sentences[0]);
		if (_playerInput == null)
			_playerInput = FindObjectOfType<PlayerInput>();
		_prevActionMap = _playerInput.currentActionMap.name;
		_playerInput.SwitchCurrentActionMap(_dialogueActionMap);
		_playerInput.actions["Dialogue/Interact"].performed += DialogueInput;
	}
	
	private void DialogueInput(InputAction.CallbackContext context)
	{
		if (context.performed)
			DisplaySentence();
	}

	public void DisplaySentence()
	{
		if (_currentSentenceIndex < _currentDialogue.sentences.Length)
		{
			_dialoguePanel.SetDialogueText(_currentDialogue.sentences[_currentSentenceIndex]);
			_currentSentenceIndex++; 
		}
		else
		{
			_playerInput.actions["Dialogue/Interact"].performed -= DialogueInput;
			EndDialogue();
		}
	}
	
	private void EndDialogue()
	{
		_dialoguePanel.gameObject.SetActive(false);
		_playerInput.SwitchCurrentActionMap(_prevActionMap);
		afterAction?.Invoke();
		_currentDialogue = null;
		_currentSentenceIndex = 0;
		
		
	}
}
