using System;
using TheHeartbeat.Utilities;
using UnityEngine;

public class SceneStateController : SingletonBehaviour<SceneStateController>
{
    [SerializeField] private Transform _logics;

    private static string SlideshowScene = "Slideshow";
    private static string MainScene = "Main";

    private enum State
    {
        PreLoad,
        Slideshow,
        Main
    }

    private State _state;

    private SlideshowMenu _slideshowMenu;
    private MainSceneController _mainSceneController;

    private bool _nextSceneLoading;

    protected override void OnAwake()
    {
        SceneLoadHandler.Instance.Init();

        _state = State.PreLoad;
        LoadSlideshowAsync(0f, 1f, null, InitSlideshowScene);
    }
    
    private void LoadSceneAsync(string sceneName, float fadeInDuration, float fadeOutDuration,
        Action onPreLoad = null, Action onPostLoad = null, Action onPostFade = null)
    {
        SceneLoadHandler.Instance.LoadSceneAsync(sceneName, fadeInDuration, fadeOutDuration,
            onPreLoad, onPostLoad, onPostFade);
    }

    private void LoadSlideshowAsync(float fadeInDuration, float fadeOutDuration,
        Action onPreLoad = null, Action onPostLoad = null, Action onPostFade = null)
    {
        LoadSceneAsync(SlideshowScene, fadeInDuration, fadeOutDuration, onPreLoad, onPostLoad, onPostFade);
    }

    private void LoadMainAsync(float fadeInDuration, float fadeOutDuration,
        Action onPreLoad = null, Action onPostLoad = null, Action onPostFade = null)
    {
        LoadSceneAsync(MainScene, fadeInDuration, fadeOutDuration, onPreLoad, onPostLoad, onPostFade);
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Slideshow:
                UpdateSlideshow(Time.deltaTime);

                break;
            case State.Main:
                UpdateMain(Time.deltaTime);

                break;
        }
    }

    private void FixedUpdate()
    {
        switch (_state)
        {
            case State.Main:
                FixedUpdateMain(Time.fixedDeltaTime);

                break;
        }
    }

    private void LateUpdate()
    {
        switch (_state)
        {
            case State.Main:
                LateUpdateMain(Time.deltaTime);

                break;
        }
    }

    private void InitSlideshowScene()
    {
        _state = State.Slideshow;
        _nextSceneLoading = false;

        SceneLoadHandler.Instance.FindFadeModel();

        _slideshowMenu = FindObjectOfType<SlideshowMenu>();
        _slideshowMenu.Init();
    }

    private void UpdateSlideshow(float dt)
    {
        if (!_slideshowMenu.Completed)
        {
            _slideshowMenu.CustomUpdate(Time.deltaTime);
        }

        if (_slideshowMenu.Completed && !_nextSceneLoading)
        {
            LoadMainAsync(1f, 1f, null, InitMainScene);
            _nextSceneLoading = true;
        }
    }

    private void InitMainScene()
    {
        _state = State.Main;
        _nextSceneLoading = false;

        SceneLoadHandler.Instance.FindFadeModel();

        _mainSceneController = FindObjectOfType<MainSceneController>();
        _mainSceneController.Init();
    }

    private void UpdateMain(float dt)
    {
        _mainSceneController.CustomUpdate(dt);
    }

    private void FixedUpdateMain(float dt)
    {
        _mainSceneController.CustomFixedUpdate(dt);
    }

    private void LateUpdateMain(float dt)
    {
        _mainSceneController.CustomLateUpdate(dt);
    }
}
