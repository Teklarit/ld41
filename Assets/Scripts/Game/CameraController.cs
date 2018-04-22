using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private PlayerViewController _playerViewController;
    [Space]
    [SerializeField] private float _deathSequenceDuration;
    [SerializeField] private float _winSequenceDuration;

    private enum State
    {
        FollowingPlayer,
        DeathSequence,
        WinSequence
    }

    private State _state;
    private float _timePassed;

    public void Init()
    {
        _state = State.FollowingPlayer;
        _timePassed = 0f;
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
            case State.FollowingPlayer:
                _mainCamera.transform.position = _playerViewController.ViewPosition;
                _mainCamera.transform.rotation = _playerViewController.ViewRotation;

                break;
            case State.DeathSequence:
                _timePassed += dt;

                _mainCamera.transform.position = _playerViewController.ViewPosition;
                _mainCamera.transform.rotation = _playerViewController.ViewRotation;

                // TODO:

                break;
            case State.WinSequence:
                _timePassed += dt;

                _mainCamera.transform.position = _playerViewController.ViewPosition;
                _mainCamera.transform.rotation = _playerViewController.ViewRotation;

                // TODO:

                break;
        }
    }
}
