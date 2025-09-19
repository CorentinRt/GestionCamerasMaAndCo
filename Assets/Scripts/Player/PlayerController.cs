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
                Vector3 camForward = Camera.main.transform.forward;
                Vector3 camRight = Camera.main.transform.right;

                camForward.y = 0;
                camRight.y = 0;

                if (camForward.sqrMagnitude < 0.01f || camRight.sqrMagnitude < 0.01f)
                {
                    forward = Vector3.forward;
                    right = Vector3.right;
                }
                else
                {
                    forward = camForward.normalized;
                    right = camRight.normalized;
                }
                break;

            case CONTROLS.WORLD:
                forward = Vector3.forward;
                right = Vector3.right;
                break;
        }

        direction += Input.GetAxisRaw("Horizontal") * right;
        direction += Input.GetAxisRaw("Vertical") * forward;
        direction.Normalize();

        _rigidbody.velocity = direction * speed + Vector3.up * _rigidbody.velocity.y;
    }

}
