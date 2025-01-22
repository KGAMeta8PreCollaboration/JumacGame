using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class ButtonInteractable : MonoBehaviour, IInteractable
{
    protected string buttonName;
    [SerializeField] protected InteractionButton _interactionButtonPrefab;

    public void Interact(Interactor interactor)
    {
        InteractionButton interactButton = Instantiate<InteractionButton>(_interactionButtonPrefab, interactor.interactView);

        interactButton.SetTitle(buttonName);

        Button button = interactButton.GetComponent<Button>();

        button.onClick.AddListener(InteractionButtonClick);
        button.onClick.AddListener(interactor.DestroyButtonAll);

        interactor.onExitAction = () => Destroy(interactButton.gameObject);
    }

    protected abstract void InteractionButtonClick();
}
