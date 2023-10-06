using UnityEngine;

public class ground : MonoBehaviour
{
    public Vector3 Ground;
    public bool IsAwake;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = Ground;
        _rigidbody.WakeUp();
        IsAwake = !_rigidbody.IsSleeping();
    }

    private void Update()
    {
        if (!IsAwake)
        {
            _rigidbody.WakeUp();
            IsAwake = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + transform.rotation * Ground, 1f);
    }
}
