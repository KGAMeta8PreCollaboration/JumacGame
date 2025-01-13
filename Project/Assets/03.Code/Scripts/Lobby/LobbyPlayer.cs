using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class LobbyPlayer : MonoBehaviour
{
    public string username;
    public string UID;

    [SerializeField] private float speed = 5f;

    private Vector3 _position;
    private Vector3 dir;

    private NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
            transform.position = _position;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        _navMeshAgent?.SetDestination(pos);
        // position = pos;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        print($"UID : {UID}" +
            $"UID : {UID} : {GameManager.Instance.FirebaseManager.User.UserId}");

        if (UID != GameManager.Instance.FirebaseManager.User.UserId)
            return;
        if (!context.performed)
        {
            dir = Vector3.zero;
            return;
        }

        Vector2 move = context.ReadValue<Vector2>();
        dir = new Vector3(move.x, 0, move.y);
    }
}
