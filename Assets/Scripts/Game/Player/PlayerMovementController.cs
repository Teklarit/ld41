using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10.0f;
    [SerializeField] private float _runSpeed = 20.0f;
    [SerializeField] private float _strafeSpeed = 5.0f;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _lastMoveDirection;

    public void Init()
    {
        _rigidbody.freezeRotation = true;
        _lastMoveDirection = Vector3.zero;
    }

    public float SpeedPart
    {
        get { return Mathf.Clamp01(_rigidbody.velocity.magnitude / Mathf.Max(_walkSpeed, _strafeSpeed)); }
    }

    public Vector3 LocalMoveDirection
    {
        get { return _lastMoveDirection; }
    }

    public void CustomFixedUpdate(float dt)
    {
        // get correct speed
        var forwardAndBackSpeed = _walkSpeed;

        // if running, set run speed
        /*if (isRunning)
        { 
            forwardAndBackSpeed = runSpeed;
        }*/

        // calculate how fast it should be moving
        var sideMovement = Input.GetAxis("Horizontal");
        var forwardMovement = Input.GetAxis("Vertical");
        //var moveDirection = new Vector3(Mathf.Sign(sideMovement) * Mathf.Sqrt(Mathf.Abs(sideMovement)), 0, Mathf.Sign(forwardMovement) * Mathf.Sqrt(Mathf.Abs(forwardMovement)));
        var moveDirection = new Vector3(sideMovement, 0, forwardMovement);

        var maxValue = Mathf.Max(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.z));
        if (maxValue > 0f)
        {
            var maxMoveDirection = moveDirection / maxValue;
            moveDirection /= maxMoveDirection.magnitude;
        }

        _lastMoveDirection = moveDirection;

        var targetVelocity = new Vector3(moveDirection.x * _strafeSpeed, 0, moveDirection.z * forwardAndBackSpeed);
        targetVelocity = transform.TransformDirection(targetVelocity);

        // apply a force that attempts to reach our target velocity
        var velocity = _rigidbody.velocity;
        var velocityChange = targetVelocity - velocity;
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }
}
