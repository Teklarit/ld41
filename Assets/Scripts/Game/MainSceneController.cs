using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private HudController _hudController;

    public void Init()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _playerController.Init();

        _cameraController = FindObjectOfType<CameraController>();
        _cameraController.Init();

        _hudController = FindObjectOfType<HudController>();
        _hudController.Init();
    }

    public void CustomUpdate(float dt)
    {
        _playerController.CustomUpdate(dt);
        _hudController.CustomUpdate(dt);
    }

    public void CustomFixedUpdate(float dt)
    {
        _playerController.CustomFixedUpdate(dt);
    }

    public void CustomLateUpdate(float dt)
    {
        _hudController.CustomUpdate(dt);
        _cameraController.CustomUpdate(dt);
    }
}
