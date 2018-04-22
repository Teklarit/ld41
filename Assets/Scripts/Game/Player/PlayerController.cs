using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerViewController _playerViewController;
    [SerializeField] private PlayerStatsController _playerStatsController;
    [SerializeField] private PlayerAnimationController _playerAnimationController;
    [Space]
    [SerializeField] private Light _flashlightSpotlight;
    [SerializeField] private float _flashlightSpotlightMaxIntensity;
    [SerializeField] private AnimationCurve _flashlightSpotlightCurve;
    [Space]
    [SerializeField] private Light _flashlightPointlight;
    [SerializeField] private float _flashlightPointightMaxIntensity;
    [SerializeField] private AnimationCurve _flashlightPointlightCurve;
    [Space]
    [SerializeField] private Transform _leftShoulderView;
    [SerializeField] private Transform _rightShoulderView;
    [Space]
    [SerializeField] private float _deathSequenceDuration;
    [SerializeField] private float _winSequenceDuration;

    private enum State
    {
        Idle,
        DeathSequence,
        WinSequence
    }

    private State _state;
    private float _timePassed;

    public void Init()
    {
        _playerMovementController.Init();
        _playerViewController.Init();
        _playerStatsController.Init();
        _playerAnimationController.Init();

        _timePassed = 0f;
        _state = State.Idle;
    }

    public bool PlayerDied
    {
        get { return _playerStatsController.IsDead; }
    }

    public bool PlayerWon
    {
        get { return false; }
    }

    public void LaunchDeathSequence()
    {
        _state = State.DeathSequence;
    }

    public bool DeathSequenceEnded
    {
        get { return _timePassed >= _deathSequenceDuration; }
    }

    public void LaunchWinSequence()
    {
        _state = State.WinSequence;
    }

    public bool WinSequenceEnded
    {
        get { return _timePassed >= _winSequenceDuration; }
    }

    public void CustomUpdate(float dt)
    {
        switch (_state)
        {
            case State.Idle:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _playerStatsController.AddHeartbeatClick();
                    _playerAnimationController.SetHandState(PlayerAnimationController.Hand.Left, PlayerAnimationController.HandState.EnemaSqueezed);
                    _playerAnimationController.SetEnemaState(PlayerAnimationController.EnemaState.Squeezed);
                }
                else
                {
                    _playerAnimationController.SetHandState(PlayerAnimationController.Hand.Left, PlayerAnimationController.HandState.EnemaIdle);
                    _playerAnimationController.SetEnemaState(PlayerAnimationController.EnemaState.Idle);
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _playerStatsController.AddLighterClick();
                    _playerAnimationController.SetHandState(PlayerAnimationController.Hand.Right, PlayerAnimationController.HandState.FlashlightSqueezed);
                    _playerAnimationController.SetFlashlightState(PlayerAnimationController.FlashlightState.Squeezed);
                }
                else
                {
                    _playerAnimationController.SetHandState(PlayerAnimationController.Hand.Right, PlayerAnimationController.HandState.FlashlightIdle);
                    _playerAnimationController.SetFlashlightState(PlayerAnimationController.FlashlightState.Idle);
                }

                _playerViewController.CustomUpdate(dt);
                _playerStatsController.CustomUpdate(dt);

                UpdateShouldersMovement(dt);
                UpdateFlashlight();

                break;
            case State.DeathSequence:
                _timePassed += dt;

                break;
            case State.WinSequence:
                _timePassed += dt;

                break;
        }
    }

    public void CustomFixedUpdate(float dt)
    {
        switch (_state)
        {
            case State.Idle:
                _playerMovementController.CustomFixedUpdate(dt);

                break;
        }
    }

    private void UpdateFlashlight()
    {
        var flashlightIntensity = _flashlightSpotlightCurve.Evaluate(_playerStatsController.Brightness);
        _flashlightSpotlight.intensity = flashlightIntensity * _flashlightSpotlightMaxIntensity;

        var flashlightSpotlightIntensity = _flashlightPointlightCurve.Evaluate(_playerStatsController.Brightness);
        _flashlightPointlight.intensity = flashlightSpotlightIntensity * _flashlightPointightMaxIntensity;
    }

    private void UpdateShouldersMovement(float dt)
    {

    }
}
