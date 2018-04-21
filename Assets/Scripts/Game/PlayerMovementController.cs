using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 10.0f;
    [SerializeField] private float _runSpeed = 20.0f;
    [SerializeField] private float _strafeSpeed = 5.0f;

    private Rigidbody _rigidbody;

    public void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void CustomUpdate(float dt)
    {
        // get correct speed
        float forwardAndBackSpeed = _walkSpeed;

        // if running, set run speed
        /*if (isRunning)
        { 
            forwardAndBackSpeed = runSpeed;
        }*/

        // calculate how fast it should be moving
        Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") * _strafeSpeed, 0, Input.GetAxis("Vertical") * forwardAndBackSpeed);
        targetVelocity = transform.TransformDirection(targetVelocity);

        // apply a force that attempts to reach our target velocity
        Vector3 velocity = _rigidbody.velocity;
        Vector3 velocityChange = (targetVelocity - velocity);
        velocityChange.y = 0;
        _rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

    }

}
