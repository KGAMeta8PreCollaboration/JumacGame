using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Interactor : MonoBehaviour
{
    public RectTransform interactView;
    [Range(0, 10)]
    public float radius;
    public Action interactAction;

    private SphereCollider _coll;
    private List<IInteractable> _interactables = new List<IInteractable>();

    private void Awake()
    {
        _coll = GetComponent<SphereCollider>();
        _coll.radius = radius;
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
            interactAction?.Invoke();
        }
    }
}
