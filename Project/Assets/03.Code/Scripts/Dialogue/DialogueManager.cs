using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour, IPointerClickHandler
{
    private DialoguePanel _dialoguePanel;
    private Dialogue _currentDialogue;
    private int _currentSentenceIndex;
    private Action afterAction;
    private PlayerInput _playerInput;
    private string _prevActionMap;
    private string _dialogueActionMap = "Dialogue";
    
    [SerializeField] private Image _backgroundImage; 
    
    private void Start()
    {
        _dialoguePanel = FindObjectOfType<DialoguePanel>(true);
        _playerInput = FindObjectOfType<PlayerInput>(true);
        _backgroundImage.gameObject.SetActive(false);
        _dialoguePanel.gameObject.SetActive(false);
        Close();
    }

    public void SetDialogue(Dialogue dialogue, Action afterAction)
    {
        _currentDialogue = dialogue;
        _currentSentenceIndex = 0;
        this.afterAction = afterAction;
    }
    
    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    

    public void StartDialogue()
    {
        Open();
        _dialoguePanel.SetDialogueText(_currentDialogue.sentences[0]);
        if (_playerInput == null)
            _playerInput = FindObjectOfType<PlayerInput>();
        _prevActionMap = _playerInput.currentActionMap.name;
        _playerInput.SwitchCurrentActionMap(_dialogueActionMap);
        _playerInput.actions["Dialogue/Interact"].performed += DialogueInput;
        _dialoguePanel.gameObject.SetActive(true);
        _backgroundImage.gameObject.SetActive(true);
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
        Close();
        _dialoguePanel.gameObject.SetActive(false);
        _backgroundImage.gameObject.SetActive(false);
        _playerInput.SwitchCurrentActionMap(_prevActionMap);
        afterAction?.Invoke();
        _currentDialogue = null;
        _currentSentenceIndex = 0;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        DisplaySentence();
    }
}
