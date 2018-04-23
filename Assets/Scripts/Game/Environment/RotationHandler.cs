using UnityEngine;

public class RotationHandler : MonoBehaviour
{
    [SerializeField] private float _angularSpeed;

    private Quaternion _initialRotation;
    private Vector3 _initialUp;
    private float _angle;

    private void Awake()
    {
        _angle = Random.Range(0f, 360f);
        _initialUp = transform.up;
        _initialRotation = transform.localRotation;
        transform.localRotation = Quaternion.AngleAxis(_angle, _initialUp) * _initialRotation;
    }

    private void Update()
    {
        _angle += _angularSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.AngleAxis(_angle, _initialUp) * _initialRotation;
        //transform.rotation *= Quaternion.AngleAxis(_angularSpeed * Time.deltaTime, _initialUp);
    }
}
