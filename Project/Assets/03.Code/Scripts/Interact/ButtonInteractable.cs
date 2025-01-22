using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ButtonInteractable : MonoBehaviour, IInteractable
{
    protected string buttonName;
    [SerializeField] protected InteractionButton _interactionButtonPrefab;
    [HideInInspector] public InteractionButton interactionButton;

    public void Interact(Interactor interactor)
    {
        interactionButton = Instantiate<InteractionButton>(_interactionButtonPrefab, interactor.interactView);

        interactionButton.SetTitle(buttonName);

        Button button = interactionButton.GetComponent<Button>();

        button.onClick.AddListener(InteractionButtonClick);
        button.onClick.AddListener(interactor.DestroyButtonAll);
    }

    protected abstract void InteractionButtonClick();
}
