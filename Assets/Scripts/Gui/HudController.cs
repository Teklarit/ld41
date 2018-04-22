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

    public void Init()
    {
        _state = State.Game;
        _timePassed = 0f;

        _deathScreen.gameObject.SetActive(false);
        _winScreen.gameObject.SetActive(false);

        Refresh();
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

    private void Refresh()
    {
        _health.fillAmount = _playerStatsController.Health;
        _heartbeatProgress.fillAmount = _playerStatsController.HeartbeatProgress;
        _brightness.fillAmount = _playerStatsController.Brightness;
        _lightProgress.fillAmount = _playerStatsController.LightProgress;
        _heartbeatLevel.text = "Level " + _playerStatsController.HeartbeatLevel;
        _lightLevel.text = "Level " + _playerStatsController.LightLevel;
    }

    public void CustomUpdate(float dt)
    {
        switch (_state)
        {
            case State.Game:
                Refresh();

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
