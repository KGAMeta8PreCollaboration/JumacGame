using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CombatNPC : ButtonInteractable
{
    private DialogueLoader _dialogueLoader;

    private void Start()
    {
        _dialogueLoader = GetComponent<DialogueLoader>();
    }
    protected override void InteractionButtonClick()
    {
        _dialogueLoader.LoadDialogue(() => print("Dialogue ended"));
    }
}
