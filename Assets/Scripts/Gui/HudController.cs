using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private PlayerStatsController _playerStatsController;
    [Space]
    [SerializeField] private Image _health;
    [SerializeField] private Image _heartbeatProgress;
    [SerializeField] private Image _brightness;
    [SerializeField] private Image _lightProgress;
    [SerializeField] private Text _heartbeatLevel;
    [SerializeField] private Text _lightLevel;
    [Space]
    [SerializeField] private Image _firstHeart;
    [SerializeField] private Image _firstHeartbeatBegin;
    [SerializeField] private Image _firstHeartbeatEnd;
    [Space]
    [SerializeField] private Image _secondHeart;
    [SerializeField] private Image _secondHeartbeatBegin;
    [SerializeField] private Image _secondHeartbeatEnd;
    [Space]
    [SerializeField] private float _deathSequenceDuration;
    [SerializeField] private Image _deathScreen;
    [SerializeField] private AnimationCurve _deathScreenVisibilityCurve;
    [Space]
    [SerializeField] private float _winSequenceDuration;
    [SerializeField] private Image _winScreen;
    [SerializeField] private AnimationCurve _winScreenVisibilityCurve;

    private enum State
    {
        Game,
        Pause,
        DeathSequence,
        WinSequence
    }

    private State _state;
    private float _timePassed;

    private Vector3 _heartSourcePosition;
    private Vector3 _heartTargetPosition;

    public void Init()
    {
        _state = State.Game;
        _timePassed = 0f;

        _deathScreen.gameObject.SetActive(false);
        _winScreen.gameObject.SetActive(false);

        _heartSourcePosition = _firstHeart.rectTransform.localPosition;
        _heartTargetPosition = _secondHeart.rectTransform.localPosition;

        Refresh(0f);
    }

    public void LaunchDeathSequence()
    {
        _state = State.DeathSequence;
        _deathScreen.gameObject.SetActive(true);
    }

    public bool DeathSequenceEnded
    {
        get { return _timePassed >= _deathSequenceDuration; }
    }

    public void LaunchWinSequence()
    {
        _state = State.WinSequence;
        _winScreen.gameObject.SetActive(true);
    }

    public bool WinSequenceEnded
    {
        get { return _timePassed >= _winSequenceDuration; }
    }

    private void Refresh(float dt)
    {
        _health.fillAmount = _playerStatsController.Health;
        _heartbeatProgress.fillAmount = _playerStatsController.HeartbeatProgress;
        _brightness.fillAmount = _playerStatsController.Brightness;
        _lightProgress.fillAmount = _playerStatsController.LightProgress;
        _heartbeatLevel.text = "Level " + _playerStatsController.HeartbeatLevel;
        _lightLevel.text = "Level " + _playerStatsController.LightLevel;

        UpdateHearts(dt);
    }

    private void UpdateHearts(float dt)
    {
        var firstHeartbeatBeginPart = _playerStatsController.FirstHeartbeatBeginPart;
        var firstHeartbeatEndPart = _playerStatsController.FirstHeartbeatEndPart;
        var secondHeartbeatBeginPart = _playerStatsController.SecondHeartbeatBeginPart;
        var secondHeartbeatEndPart = _playerStatsController.SecondHeartbeatEndPart;

        var firstHeartbeatBeginPosition = Vector3.Lerp(_heartSourcePosition, _heartTargetPosition, firstHeartbeatBeginPart);
        var firstHeartbeatEndPosition = Vector3.Lerp(_heartSourcePosition, _heartTargetPosition, firstHeartbeatEndPart);
        var secondHeartbeatBeginPosition = Vector3.Lerp(_heartSourcePosition, _heartTargetPosition, secondHeartbeatBeginPart);
        var secondHeartbeatEndPosition = Vector3.Lerp(_heartSourcePosition, _heartTargetPosition, secondHeartbeatEndPart);

        _firstHeartbeatBegin.rectTransform.localPosition = firstHeartbeatBeginPosition;
        _firstHeartbeatEnd.rectTransform.localPosition = firstHeartbeatEndPosition;
        _secondHeartbeatBegin.rectTransform.localPosition = secondHeartbeatBeginPosition;
        _secondHeartbeatEnd.rectTransform.localPosition = secondHeartbeatEndPosition;

        var passedPart = _playerStatsController.HeartbeatSequencePassedPart;
        var firstHeartPosition = Vector3.Lerp(_heartSourcePosition, _heartTargetPosition, passedPart);
        _firstHeart.rectTransform.localPosition = firstHeartPosition;
    }

    public void CustomUpdate(float dt)
    {
        switch (_state)
        {
            case State.Game:
                Refresh(dt);

                break;
            case State.DeathSequence:
                _timePassed += dt;

                var deathPassedPart = Mathf.Clamp01(_timePassed / _deathSequenceDuration);
                var deathCurvedPart = _deathScreenVisibilityCurve.Evaluate(deathPassedPart);
                var deathScreenColor = _deathScreen.color;
                deathScreenColor.a = deathCurvedPart;
                _deathScreen.color = deathScreenColor;

                break;
            case State.WinSequence:
                _timePassed += dt;

                var winPassedPart = Mathf.Clamp01(_timePassed / _winSequenceDuration);
                var winCurvedPart = _deathScreenVisibilityCurve.Evaluate(winPassedPart);
                var winScreenColor = _winScreen.color;
                winScreenColor.a = winCurvedPart;
                _winScreen.color = winScreenColor;

                break;
        }
    }
}
