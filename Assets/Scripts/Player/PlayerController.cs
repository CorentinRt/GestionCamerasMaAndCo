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

    [SerializeField] private bool _useLockDirectionByCameraOnSwitch = true;
    [SerializeField] private FreeFollowView freeFollowView;

    Rigidbody _rigidbody = null;

    private bool _canChangeReferenceDirection = true;
    private Vector3 _right = Vector3.right;
    private Vector3 _forward = Vector3.forward;

    protected bool IsActive { get; private set; }

    public void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 direction = Vector3.zero;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        if (_useLockDirectionByCameraOnSwitch && (freeFollowView.Weight > 0f || (!_canChangeReferenceDirection && horizontalInput == 0f && verticalInput == 0f)))
        {
            _canChangeReferenceDirection = true;
        }

        switch (controls)
        {
            case CONTROLS.CAMERA:
                if (_canChangeReferenceDirection || !_useLockDirectionByCameraOnSwitch)
                {
                    _canChangeReferenceDirection = false;

                    Vector3 camForward = Camera.main.transform.forward;
                    Vector3 camRight = Camera.main.transform.right;

                    camForward.y = 0;
                    camRight.y = 0;

                    if (camForward.sqrMagnitude < 0.01f || camRight.sqrMagnitude < 0.01f)
                    {
                        _forward = Vector3.forward;
                        _right = Vector3.right;
                    }
                    else
                    {
                        _forward = camForward.normalized;
                        _right = camRight.normalized;
                    }
                }
                break;

            case CONTROLS.WORLD:
                _forward = Vector3.forward;
                _right = Vector3.right;
                break;
        }

        direction += horizontalInput * _right;
        direction += verticalInput * _forward;
        direction.Normalize();

        _rigidbody.velocity = direction * speed + Vector3.up * _rigidbody.velocity.y;
    }

}
