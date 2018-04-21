using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerViewController _playerViewController;

    public void Init()
    {
        _playerMovementController.Init();
        _playerViewController.Init();
    }

    public void CustomUpdate(float dt)
    {
        _playerMovementController.CustomUpdate(dt);// TODO: add fixed update
        _playerViewController.CustomUpdate(dt);
    }
}
