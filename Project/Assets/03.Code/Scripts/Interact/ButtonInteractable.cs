using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Button _interactionButtonPrefab;

    public void Interact(Interactor interactor)
    {
        Button interactButton = Instantiate<Button>(_interactionButtonPrefab, interactor.interactView);
        interactButton.onClick.AddListener(InteractionButtonClick);

        interactor.onExitAction = () => Destroy(interactButton.gameObject);
    }

    protected abstract void InteractionButtonClick();
}
