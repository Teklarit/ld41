using System;
using System.Collections.Generic;
using TheHeartbeat.Utilities;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;

public class SceneLoadHandler : SingletonBehaviour<SceneLoadHandler>
{
    [SerializeField] private Color _fadeInColor = Color.black;
    [SerializeField] private Color _fadeOutColor = Color.white;

    private bool _useCurrentFadeColor;
    private Color _currentFadeColor;

    private FadeModel _fadeModel;
    private FadeModel.Settings _fadeModelSettings;

    private string _sceneName;
    private Action _onPostLoadPreEnter;
    private Action _onPostLoadPostEnter;
    private Action _onPostLoadPostLeave;
    private AsyncOperation _sceneLoading;

    private enum State
    {
        Idle,
        PreLoad,
        PostLoad
    }

    private enum Transition
    {
        Idle,
        PreLoad,
        PostLoad
    }

    private FSM<State, Transition> _fsm;

    private float _preLoadDuration;
    private float _postLoadDuration;
    private float _timePassed;

    public void Init()
    {
        _fsm = new FSM<State, Transition>(new Dictionary<State, FSMState>
        {
            { State.Idle, new FSMState() },
            { State.PreLoad, new FSMState(new Dictionary<FSMState.Immediates, FSMState.Immediate>
                {
                    { FSMState.Immediates.OnPreEnter, OnPreLoadPreEnter }
                }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                {
                    { FSMState.Updatables.OnUpdate, OnPreLoadUpdate }
                })
            },
            { State.PostLoad, new FSMState(new Dictionary<FSMState.Immediates, FSMState.Immediate>
                {
                    { FSMState.Immediates.OnPreEnter, OnPostLoadPreEnter },
                    { FSMState.Immediates.OnPostEnter, OnPostLoadPostEnter }
                }, new Dictionary<FSMState.Updatables, FSMState.Updatable>
                {
                    { FSMState.Updatables.OnEnter, OnPostLoadEnter },
                    { FSMState.Updatables.OnUpdate, OnPostLoadUpdate }
                })
            }
        },
        new Dictionary<Transition, FSMTransition<State>>
        {
            { Transition.Idle, new FSMTransition<State>(new HashSet<State> { State.PostLoad }, State.Idle) },
            { Transition.PreLoad, new FSMTransition<State>(new HashSet<State> { State.Idle, State.PostLoad }, State.PreLoad) },
            { Transition.PostLoad, new FSMTransition<State>(new HashSet<State> { State.Idle, State.PreLoad }, State.PostLoad) }
        },
        State.Idle);

        FindFadeModel();
    }

    public void FindFadeModel()
    {
        var postProcessingBehavior = FindObjectOfType<PostProcessingBehaviour>();
        if (postProcessingBehavior != null)
        {
            SetFadeModel(postProcessingBehavior.profile.fade);
        }
    }

    private void Update()
    {
        _fsm.Update(Time.deltaTime);
    }

    private void OnPreLoadPreEnter()
    {
        _timePassed = 0f;
        SetFadeColor(_useCurrentFadeColor ? _currentFadeColor : _fadeOutColor);
    }

    private bool OnPreLoadUpdate(float dt)
    {
        _timePassed += dt;
        if (_timePassed >= _preLoadDuration)
        {
            SetFadeColor(_fadeInColor);

            _fsm.Transit(Transition.PostLoad);
            return false;
        }

        SetFadeColor(Color.Lerp(_useCurrentFadeColor ? _currentFadeColor : _fadeOutColor,
            _fadeInColor, _timePassed / _preLoadDuration));
        return false;
    }

    private void OnPostLoadPreEnter()
    {
        _useCurrentFadeColor = false;

        if (_onPostLoadPreEnter != null)
        {
            _onPostLoadPreEnter();
        }

        _sceneLoading = SceneManager.LoadSceneAsync(_sceneName);

        _timePassed = 0f;
        SetFadeColor(_fadeInColor);
    }

    private bool OnPostLoadEnter(float dt)
    {
        return _sceneLoading.isDone && DynamicGI.isConverged;
    }

    private void OnPostLoadPostEnter()
    {
        if (_onPostLoadPostEnter != null)
        {
            _onPostLoadPostEnter();
        }
    }

    private bool OnPostLoadUpdate(float dt)
    {
        _timePassed += dt;
        if (_timePassed >= _postLoadDuration)
        {
            SetFadeColor(_fadeOutColor);

            if (_onPostLoadPostLeave != null)
            {
                _onPostLoadPostLeave();
            }

            _fsm.Transit(Transition.Idle);
            return false;
        }

        SetFadeColor(Color.Lerp(_fadeInColor, _fadeOutColor, _timePassed / _postLoadDuration));
        return false;
    }

    private void SetFadeColor(Color color)
    {
        if (_fadeModel != null)
        {
            _fadeModelSettings.color = color;
            _fadeModel.settings = _fadeModelSettings;
        }
    }

    public void SetFadeModel(FadeModel fadeModel)
    {
        _fadeModel = fadeModel;
    }

    public void LoadSceneAsync(string sceneName, float fadeInDuration, float fadeOutDuration,
        Action onScenePreLoad = null, Action onScenePostLoad = null, Action onPostFadeOut = null)
    {
        _sceneName = sceneName;
        _onPostLoadPreEnter = onScenePreLoad;
        _onPostLoadPostEnter = onScenePostLoad;
        _onPostLoadPostLeave = onPostFadeOut;

        _sceneLoading = null;

        _postLoadDuration = fadeOutDuration;

        // If fade is in process then we will let it be
        if (_fsm.CurrentState() != State.PreLoad)
        {
            _preLoadDuration = fadeInDuration;

            if (_fsm.CurrentState() == State.PostLoad)
            {
                _useCurrentFadeColor = true;
                _currentFadeColor = _fadeModelSettings.color;
            }

            _fsm.Transit(fadeInDuration > 0f ? Transition.PreLoad : Transition.PostLoad);
        }
    }

    public bool SceneLoaded
    {
        get { return _sceneLoading != null && _sceneLoading.isDone; }
    }

    public bool SceneFadedOut
    {
        get { return _fsm.CurrentState() == State.Idle; }
    }

    public float FadeDuration
    {
        get { return _preLoadDuration + _postLoadDuration; }
    }
}
