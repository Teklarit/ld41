using System.Collections.Generic;
using TheHeartbeat.Utilities;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private HudController _hudController;

    private enum GameState
    {
        None,
        Idle,
        DeathSequence,
        DeathSequenceEnded,
        WinSequence,
        WinSequenceEnded,
    }

    private enum GameTransition
    {
        Idle,
        DeathSequence,
        DeathSequenceEnded,
        WinSequence,
        WinSequenceEnded,
    }

    private FSM<GameState, GameTransition> _gameFsm;

    public bool NeedReload
    {
        get { return _gameFsm.CurrentState() == GameState.DeathSequenceEnded || _gameFsm.CurrentState() == GameState.WinSequenceEnded; }
    }

    public void Init()
    {
        _playerController.Init();
        _cameraController.Init();
        _hudController.Init();

        _gameFsm = new FSM<GameState, GameTransition>(new Dictionary<GameState, FSMState>
            {
                { GameState.None, new FSMState() },
                {
                    GameState.Idle, new FSMState(new Dictionary<FSMState.Immediates, FSMState.Immediate>
                    {
                        { FSMState.Immediates.OnPreEnter, OnIdlePreEnter }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnIdleUpdate }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnIdleFixedUpdate }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnIdleLateUpdate }
                    })
                },
                {
                    GameState.DeathSequence, new FSMState(new Dictionary<FSMState.Immediates, FSMState.Immediate>
                    {
                        { FSMState.Immediates.OnPreEnter, OnDeathSequencePreEnter }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnDeathSequenceUpdate }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnDeathSequenceFixedUpdate }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnDeathSequenceLateUpdate }
                    })
                },
                { GameState.DeathSequenceEnded, new FSMState() },
                {
                    GameState.WinSequence, new FSMState(new Dictionary<FSMState.Immediates, FSMState.Immediate>
                    {
                        { FSMState.Immediates.OnPreEnter, OnWinSequencePreEnter }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnWinSequenceUpdate }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnWinSequenceFixedUpdate }
                    }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                    {
                        { FSMState.Updatables.OnUpdate, OnWinSequenceLateUpdate }
                    })
                },
                { GameState.WinSequenceEnded, new FSMState() }
            },
            new Dictionary<GameTransition, FSMTransition<GameState>>
            {
                { GameTransition.Idle, new FSMTransition<GameState>(new HashSet<GameState> { GameState.None }, GameState.Idle) },
                { GameTransition.DeathSequence, new FSMTransition<GameState>(new HashSet<GameState> { GameState.Idle }, GameState.DeathSequence) },
                { GameTransition.DeathSequenceEnded, new FSMTransition<GameState>(new HashSet<GameState> { GameState.DeathSequence }, GameState.DeathSequenceEnded) },
                { GameTransition.WinSequence, new FSMTransition<GameState>(new HashSet<GameState> { GameState.Idle }, GameState.WinSequence) },
                { GameTransition.WinSequenceEnded, new FSMTransition<GameState>(new HashSet<GameState> { GameState.WinSequence }, GameState.WinSequenceEnded) }
            },
            GameState.None);

        _gameFsm.Transit(GameTransition.Idle);
    }

    private void OnIdlePreEnter()
    {

    }

    private bool OnIdleUpdate(float dt)
    {
        _playerController.CustomUpdate(dt);

        return false;
    }

    private bool OnIdleFixedUpdate(float dt)
    {
        _playerController.CustomFixedUpdate(dt);

        return false;
    }

    private bool OnIdleLateUpdate(float dt)
    {
        _playerController.CustomLateUpdate(dt);
        _hudController.CustomUpdate(dt);
        _cameraController.CustomUpdate(dt);

        if (_playerController.PlayerDied)
        {
            _gameFsm.Transit(GameTransition.DeathSequence);
        }
        else if (_playerController.PlayerWon)
        {
            _gameFsm.Transit(GameTransition.WinSequence);
        }

        return false;
    }

    private void OnDeathSequencePreEnter()
    {
        _playerController.LaunchDeathSequence();
        _hudController.LaunchDeathSequence();
        _cameraController.LaunchDeathSequence();
    }

    private bool OnDeathSequenceUpdate(float dt)
    {
        _playerController.CustomUpdate(dt);
        _hudController.CustomUpdate(dt);

        return false;
    }

    private bool OnDeathSequenceFixedUpdate(float dt)
    {
        _playerController.CustomFixedUpdate(dt);

        return false;
    }

    private bool OnDeathSequenceLateUpdate(float dt)
    {
        _hudController.CustomUpdate(dt);
        _cameraController.CustomUpdate(dt);

        if (_playerController.DeathSequenceEnded && _hudController.DeathSequenceEnded && _cameraController.DeathSequenceEnded)
        {
            _gameFsm.Transit(GameTransition.DeathSequenceEnded);
        }

        return false;
    }

    private void OnWinSequencePreEnter()
    {
        _playerController.LaunchWinSequence();
        _hudController.LaunchWinSequence();
        _cameraController.LaunchWinSequence();
    }

    private bool OnWinSequenceUpdate(float dt)
    {
        _playerController.CustomUpdate(dt);
        _hudController.CustomUpdate(dt);

        return false;
    }

    private bool OnWinSequenceFixedUpdate(float dt)
    {
        _playerController.CustomFixedUpdate(dt);

        return false;
    }

    private bool OnWinSequenceLateUpdate(float dt)
    {
        _hudController.CustomUpdate(dt);
        _cameraController.CustomUpdate(dt);

        if (_hudController.WinSequenceEnded && _hudController.WinSequenceEnded && _cameraController.WinSequenceEnded)
        {
            _gameFsm.Transit(GameTransition.WinSequenceEnded);
        }

        return false;
    }

    public void CustomUpdate(float dt)
    {
        _gameFsm.Update(dt);
    }

    public void CustomFixedUpdate(float dt)
    {
        _gameFsm.FixedUpdate(dt);
    }

    public void CustomLateUpdate(float dt)
    {
        _gameFsm.LateUpdate(dt);
    }
}
