using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableTest : MonoBehaviour, IInteractable
{
    [SerializeField] private Button _interactionButtonPrefab;

    public void Interact(InteractorTest interactor)
    {
        Button interactButton = Instantiate<Button>(_interactionButtonPrefab, interactor.interactView);
        interactButton.onClick.AddListener(InteractionButtonClick);

        interactor.interactAction = () => Destroy(interactButton.gameObject);
    }

    private void InteractionButtonClick()
    {
        print("가자~~~~ 이세계로!");
    }
}
