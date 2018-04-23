using UnityEngine;

public class RotationHandler : MonoBehaviour
{
    [SerializeField] private float _angularSpeed;

    private void Awake()
    {
        transform.rotation *= Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);
    }

    private void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(_angularSpeed * Time.deltaTime, Vector3.up);
    }
}
