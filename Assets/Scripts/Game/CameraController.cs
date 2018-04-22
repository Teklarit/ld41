using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerViewController _playerViewController;

    public void Init()
    {

    }

    public void CustomUpdate(float dt)
    {
        _mainCamera.transform.position = _playerViewController.ViewPosition;
        _mainCamera.transform.rotation = _playerViewController.ViewRotation;
    }
}
