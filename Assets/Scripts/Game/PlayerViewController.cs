using UnityEngine;

public class PlayerViewController : MonoBehaviour
{
    [SerializeField] private Transform _view;

    private float sensitivityX = 100.0f;
    private float sensitivityY = 100.0f;

    private float minimumX = 0f;
    private float maximumX = 360f;

    private float minimumY = -30f;
    private float maximumY = 45f;

    private float rotationY = 0f;

    public Vector3 ViewPosition
    {
        get { return _view.position; }
    }

    public Quaternion ViewRotation
    {
        get { return _view.rotation; }
    }

    public void Init()
    {
        // TODO:
    }

    public void CustomUpdate(float dt)
    {
        var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX * dt;

        rotationY += Input.GetAxis("Mouse Y") * sensitivityY * dt;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        transform.eulerAngles = new Vector3(0f, rotationX, 0f);
        _view.eulerAngles = new Vector3(-rotationY, rotationX, 0f);
    }
}
