using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerViewController _playerViewController;
    [SerializeField] private PlayerStatsController _playerStatsController;
    [SerializeField] private PlayerAnimationController _playerAnimationController;

    public void Init()
    {
        _playerMovementController.Init();
        _playerViewController.Init();
        _playerStatsController.Init();
        _playerAnimationController.Init();
    }

    public void CustomUpdate(float dt)
    {
        _playerViewController.CustomUpdate(dt);
        _playerStatsController.CustomUpdate(dt);
    }

    public void CustomFixedUpdate(float dt)
    {
        _playerMovementController.CustomFixedUpdate(dt);
    }
}
