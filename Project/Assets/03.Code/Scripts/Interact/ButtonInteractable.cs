using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Button _interactionButtonPrefab;
    public string nextPopup;

    public void Interact(Interactor interactor)
    {
        Button interactButton = Instantiate<Button>(_interactionButtonPrefab, interactor.interactView);
        interactButton.onClick.AddListener(InteractionButtonClick);

        interactor.interactAction = () => Destroy(interactButton.gameObject);
    }

    private void InteractionButtonClick()
    {
        PopupManager.Instance.PopupOpen(nextPopup);
    }
}
