using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] protected Button _interactionButtonPrefab;

    public void Interact(Interactor interactor)
    {
        Button interactButton = Instantiate<Button>(_interactionButtonPrefab, interactor.interactView);
        interactButton.onClick.AddListener(InteractionButtonClick);
        interactButton.onClick.AddListener(interactor.DestroyButtonAll);

        interactor.onExitAction = () => Destroy(interactButton.gameObject);
    }

    protected abstract void InteractionButtonClick();
}
