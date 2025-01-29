using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueLoader : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    // private Action _afterAction;

    private void Reset()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>(true);
    }

    private void Start()
    {
        if (_dialogueManager == null)
            _dialogueManager = FindObjectOfType<DialogueManager>(true);
        // StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        //LoadDialogue(null);
        yield return null;
    }

    public void LoadDialogue(Dialogue dialogue, Action afterAction)
    {
        _dialogueManager.SetDialogue(dialogue, afterAction);
        _dialogueManager.StartDialogue();
    }
}
