using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool _autoClick;
    [SerializeField] private float _autoClickDuration;
    [Space]
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
    [SerializeField] private Transform _leftShoulder;
    [SerializeField] private Transform _rightShoulder;
    [SerializeField] private float _shoulderRotationDuration;
    [SerializeField] private float _shoulderRotationMinValue;
    [SerializeField] private float _shoulderRotationMaxValue;
    [SerializeField] private float _shoulderSideMovementSpeedMult;
    [SerializeField] private float _shoulderMovementLerpCoef;
    [Space]
    [SerializeField] private float _attractionRadiusMultiplier;
    [Space]
    [SerializeField] private float _deathSequenceDuration;
    [SerializeField] private float _winSequenceDuration;
    [SerializeField] private AnimationCurve _lastSequenceDurationCurve;

    private enum State
    {
        Idle,
        DeathSequence,
        WinSequence
    }

    private State _state;
    private float _timePassed;

    private Quaternion _leftShoulderInitialRotation;
    private Quaternion _rightShoulderInitialRotation;
    private Quaternion _leftShoulderViewInitialRotation;
    private Quaternion _rightShoulderViewInitialRotation;
    private Vector3 _leftShoulderForward;
    private Vector3 _rightShoulderForward;
    private float _shoulderRotationTimePassed;

    private float _autoClickTimePassed;
    private bool _shouldersInitialized;

    private bool _playerWon;
    private float _handsHeight;
    private float _handsForward;

    public void Init()
    {
        _playerMovementController.Init();
        _playerViewController.Init();
        _playerStatsController.Init();
        _playerAnimationController.Init();

        _timePassed = 0f;
        _state = State.Idle;

        _autoClickTimePassed = 0f;
        _shouldersInitialized = false;
        _playerWon = false;
    }

    public float AttractionRadius
    {
        get { return _playerStatsController.Brightness * _attractionRadiusMultiplier; }
    }

    public bool PlayerDied
    {
        get { return _playerStatsController.IsDead; }
    }

    public bool PlayerWon
    {
        get { return _playerWon; }
    }

    public void LaunchDeathSequence()
    {
        _state = State.DeathSequence;
        _handsHeight = _playerViewController.GetHandsHeight();
        _handsForward = _playerViewController.GetHandsForward();
    }

    public bool DeathSequenceEnded
    {
        get { return _timePassed >= _deathSequenceDuration; }
    }

    public void LaunchWinSequence()
    {
        _state = State.WinSequence;
        _handsHeight = _playerViewController.GetHandsHeight();
        _handsForward = _playerViewController.GetHandsForward();
    }

    public bool WinSequenceEnded
    {
        get { return _timePassed >= _winSequenceDuration; }
    }

    public void SetPlayerWon()
    {
        _playerWon = true;
    }

    public void CustomUpdate(float dt)
    {
        switch (_state)
        {
            case State.Idle:
                _playerStatsController.ResetClicks();

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

                if (Input.GetKeyDown(KeyCode.Mouse0) || (_autoClick && AutoClickPerformed(dt)))
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

                UpdateFlashlight();

                break;
            case State.DeathSequence:
                {
                    _timePassed += dt;

                    _playerViewController.SetHandsHeight(_handsHeight * (1f - _timePassed / _winSequenceDuration));
                    _playerViewController.SetHandsForward(_handsForward - _lastSequenceDurationCurve.Evaluate(_timePassed / _winSequenceDuration) * (15f));

                    break;
                }
            case State.WinSequence:
                {
                    _timePassed += dt;

                    _playerViewController.SetHandsHeight(_handsHeight * (1f - _timePassed / _winSequenceDuration));
                    _playerViewController.SetHandsForward(_handsForward - _lastSequenceDurationCurve.Evaluate(_timePassed / _winSequenceDuration) * (15f));

                    break;
                }
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

    public void CustomLateUpdate(float dt)
    {
        switch (_state)
        {
            case State.Idle:
                _playerViewController.CustomLateUpdate(dt);
                UpdateShouldersMovement(dt);

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
        if (!_shouldersInitialized)
        {
            _leftShoulderInitialRotation = _leftShoulder.localRotation;
            _rightShoulderInitialRotation = _rightShoulder.localRotation;
            _leftShoulderViewInitialRotation = _leftShoulderView.localRotation;
            _rightShoulderViewInitialRotation = _rightShoulderView.localRotation;

            _shouldersInitialized = true;
        }

        var localMovementDirection = _playerMovementController.LocalMoveDirection;
        var movementSpeedMult = _playerMovementController.SpeedPart;

        var forwardMovementMult = Mathf.Lerp(_shoulderRotationMinValue, _shoulderRotationMaxValue, Mathf.Abs(localMovementDirection.z) * movementSpeedMult);
        var sideMovementMult = Mathf.Lerp(_shoulderRotationMinValue, _shoulderRotationMaxValue, Mathf.Abs(localMovementDirection.x) * movementSpeedMult);

        var clampedSpeedMult = Mathf.Lerp(_shoulderRotationMinValue, _shoulderRotationMaxValue, _playerMovementController.SpeedPart);
        _shoulderRotationTimePassed += clampedSpeedMult * dt;
        var passedPart = Mathf.Clamp01(_shoulderRotationTimePassed / _shoulderRotationDuration);

        var angle = passedPart * 2f * Mathf.PI;
        var leftShoulderForwardOffset = Mathf.Sin(angle);
        var rightShoulderForwardOffset = Mathf.Sin(angle + Mathf.PI);
        var leftShoulderSideOffset = Mathf.Sin(angle * _shoulderSideMovementSpeedMult);
        var rightShoulderSideOffset = Mathf.Sin(angle * _shoulderSideMovementSpeedMult + Mathf.PI);

        var leftShoulderForward = new Vector3(leftShoulderSideOffset * sideMovementMult, leftShoulderForwardOffset * forwardMovementMult, 1f).normalized;
        var rightShoulderForward = new Vector3(rightShoulderSideOffset * sideMovementMult, rightShoulderForwardOffset * forwardMovementMult, 1f).normalized;

        _leftShoulderForward = Vector3.Lerp(_leftShoulderForward, leftShoulderForward, Mathf.Clamp01(_shoulderMovementLerpCoef * dt));
        _rightShoulderForward = Vector3.Lerp(_rightShoulderForward, rightShoulderForward, Mathf.Clamp01(_shoulderMovementLerpCoef * dt));

        var leftShoulderAdditionalRotation = Quaternion.LookRotation(_leftShoulderForward);
        var rightShoulderAdditionalRotation = Quaternion.LookRotation(_rightShoulderForward);

        _leftShoulder.localRotation = leftShoulderAdditionalRotation * _leftShoulderInitialRotation;
        _rightShoulder.localRotation = rightShoulderAdditionalRotation * _rightShoulderInitialRotation;
        _leftShoulderView.localRotation = leftShoulderAdditionalRotation * _leftShoulderViewInitialRotation;
        _rightShoulderView.localRotation = rightShoulderAdditionalRotation * _rightShoulderViewInitialRotation;

        if (_shoulderRotationTimePassed >= _shoulderRotationDuration)
        {
            _shoulderRotationTimePassed -= _shoulderRotationDuration;
        }
    }

    private bool AutoClickPerformed(float dt)
    {
        _autoClickTimePassed += dt;
        if (_autoClickTimePassed >= _autoClickDuration)
        {
            _autoClickTimePassed -= _autoClickDuration;
            return true;
        }

        return false;
    }
}
