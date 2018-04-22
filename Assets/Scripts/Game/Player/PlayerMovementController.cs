using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10.0f;
    [SerializeField] private float _runSpeed = 20.0f;
    [SerializeField] private float _strafeSpeed = 5.0f;
    [SerializeField] private Rigidbody _rigidbody;

    public void Init()
    {
        _rigidbody.freezeRotation = true;
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
        var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        var maxValue = Mathf.Max(Mathf.Abs(moveDirection.x), Mathf.Abs(moveDirection.z));
        if (maxValue > 0f)
        {
            var maxMoveDirection = moveDirection / maxValue;
            moveDirection /= maxMoveDirection.magnitude;
        }

        var targetVelocity = new Vector3(moveDirection.x * _strafeSpeed, 0, moveDirection.z * forwardAndBackSpeed);
        targetVelocity = transform.TransformDirection(targetVelocity);

        // apply a force that attempts to reach our target velocity
        var velocity = _rigidbody.velocity;
        var velocityChange = targetVelocity - velocity;
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }
}
