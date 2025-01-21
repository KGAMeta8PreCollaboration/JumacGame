using System;
using System.Collections;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    private DialogueManager _dialogueManager;
    // private Action _afterAction;

    private void Reset()
    {
        _dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void Start()
    {
        if (_dialogueManager == null)
            _dialogueManager = FindObjectOfType<DialogueManager>();

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
        // afterAction.Invoke();
        // _afterAction = afterAction;
        // _dialogueManager.dialogueEndAction = _afterAction;
        // _dialogueManager.StartDialogue(dialogue);
    }
    // {
    // 	// _dialogueManager.dialogueEndAction = _afterAction;
    // 	// _dialogueManager.StartDialogue(dialogue);
    // }
}
