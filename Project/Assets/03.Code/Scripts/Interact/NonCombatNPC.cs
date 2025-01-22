using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class NonCombatNPC : ButtonInteractable
{
    [SerializeField] private Dialogue _dialogue;
    private DialogueLoader _dialogueLoader;

    private void Start()
    {
        _dialogueLoader = GetComponent<DialogueLoader>();
        buttonName = _dialogue.name;
    }
    protected override void InteractionButtonClick()
    {
        _dialogueLoader.LoadDialogue(_dialogue, () => print("Dialogue ended"));
    }
}
