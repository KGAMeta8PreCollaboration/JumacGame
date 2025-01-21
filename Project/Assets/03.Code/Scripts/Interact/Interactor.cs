using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class Interactor : MonoBehaviour
{
    public RectTransform interactView;
    [Range(0, 10)]
    public float radius;
    public Action onExitAction;

    private SphereCollider _coll;
    private List<IInteractable> _interactables = new List<IInteractable>();

    private void Awake()
    {
        _coll = GetComponent<SphereCollider>();
        _coll.radius = radius;
    }

    private void Start()
    {
        interactView = GameObject.Find("InteractView").GetComponent<RectTransform>();
        print(interactView);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable) && !_interactables.Contains(interactable))
        {
            _interactables.Add(interactable);
            interactable.Interact(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.TryGetComponent<IInteractable>(out IInteractable interactable) && _interactables.Contains(interactable)))
        {
            _interactables.Remove(interactable);
            onExitAction?.Invoke();
        }
    }

    public void DestroyButtonAll()
    {
        Button[] removeButtons = interactView.GetComponentsInChildren<Button>();

        foreach (Button button in removeButtons)
        {
            Destroy(button.gameObject, 0.1f);
        }

        _interactables.Clear();
    }
}
