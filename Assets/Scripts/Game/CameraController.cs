using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerController _playerController;

    public void Init()
    {

    }

    public void CustomUpdate(float dt)
    {
        _mainCamera.transform.position = _playerController.ViewPosition;
        _mainCamera.transform.rotation = _playerController.ViewRotation;
    }
}
