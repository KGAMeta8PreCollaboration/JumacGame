using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractorTest : MonoBehaviour
{
    public GameObject welcomePanel;
    public float radius;
    public LayerMask targetLayer;

    private SphereCollider _coll;

    private List<IInteractable> _interactables = new List<IInteractable>();

    private void Awake()
    {
        welcomePanel.gameObject.SetActive(false);
        _coll = GetComponent<SphereCollider>();
        _coll.radius = radius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out IInteractable interactable) && !_interactables.Contains(interactable))
        {
            _interactables.Add(interactable);
            print($"들어왔음 : {interactable}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.TryGetComponent<IInteractable>(out IInteractable interactable) && _interactables.Contains(interactable)))
        {
            _interactables.Remove(interactable);
            print($"나갔음 : {interactable}");
        }
    }
}
