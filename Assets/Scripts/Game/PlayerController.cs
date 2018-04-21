using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;

    public void Init()
    {
        _playerMovementController.Init();
    }

    public void CustomUpdate(float dt)
    {
        _playerMovementController.CustomUpdate(dt);
    }
}
