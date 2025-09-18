using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum CONTROLS
    {
        WORLD,
        CAMERA,
    }

    public float speed = 10.0f;
    public CONTROLS controls = CONTROLS.CAMERA;

    Rigidbody _rigidbody = null;
    protected bool IsActive { get; private set; }

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;
        Vector3 forward = Vector3.forward;
        Vector3 right = Vector3.right;

        switch (controls)
        {
            case CONTROLS.CAMERA:
                forward = Camera.main.transform.forward;
                forward.y = 0;
                forward.Normalize();

                right = Camera.main.transform.right;
                right.y = 0;
                right.Normalize();

                break;
        }
        direction += Input.GetAxisRaw("Horizontal") * right;
        direction += Input.GetAxisRaw("Vertical") * forward;
        direction.Normalize();
        _rigidbody.velocity = direction * speed + Vector3.up * _rigidbody.velocity.y;
    }
}
