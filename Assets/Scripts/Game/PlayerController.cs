using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerViewController _playerViewController;

    public Vector3 ViewPosition
    {
        get { return _playerViewController.ViewPosition; }
    }

    public Quaternion ViewRotation
    {
        get { return _playerViewController.ViewRotation; }
    }

    public void Init()
    {
        _playerMovementController.Init();
        _playerViewController.Init();
    }

    public void CustomUpdate(float dt)
    {
        _playerViewController.CustomUpdate(dt);
    }

    public void CustomFixedUpdate(float dt)
    {
        _playerMovementController.CustomFixedUpdate(dt);
    }
}
