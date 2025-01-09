using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractorTest : MonoBehaviour
{
    public RectTransform interactView;
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
            print($"들어왔음 : {interactable}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.TryGetComponent<IInteractable>(out IInteractable interactable) && _interactables.Contains(interactable)))
        {
            _interactables.Remove(interactable);

            if (interactAction != null)
            {
                print("interactAction 호출");
                interactAction.Invoke();
            }
            else
            {
                print("interactAction이 설정되지 않음");
            }
            print($"나갔음 : {interactable}");
        }
    }
}
