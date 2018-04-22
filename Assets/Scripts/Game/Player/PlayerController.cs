using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private PlayerViewController _playerViewController;
    [SerializeField] private PlayerStatsController _playerStatsController;
    [SerializeField] private PlayerAnimationController _playerAnimationController;
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
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _playerStatsController.AddLighterClick();
                }

                _playerViewController.CustomUpdate(dt);
                _playerStatsController.CustomUpdate(dt);

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
}
