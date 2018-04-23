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
    [SerializeField] private Text _totalHealthClicks;
    [SerializeField] private Text _totalLightClicks;
    [Space]
    [SerializeField] private Image _firstHeart;
    [SerializeField] private Image _firstHeartbeatBeat;
    [Space]
    [SerializeField] private Image _secondHeart;
    [SerializeField] private Image _secondHeartbeatBeat;
    [Space]
    [SerializeField] private AnimationCurve _heartVisibilityCurve;
    [SerializeField] private float _heartSizeMult;
    [Space]
    [SerializeField] private Image[] _healthClicks;
    [SerializeField] private Image _healthClickInitial;
    [SerializeField] private Image[] _lightClicks;
    [SerializeField] private Image _lightClickInitial;
    [SerializeField] private float _clickMovementDuration;
    [SerializeField] private AnimationCurve _clickMovementCurve;
    [SerializeField] private AnimationCurve _clickVisibilityCurve;

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
    private Vector2 _firstHeartInitialSizeDelta;

    private Vector3 _healthClickInitialPosition;
    private Quaternion _healthClickInitialRotation;
    private Vector3[] _healthClickTargetPositions;
    private Quaternion[] _healthClickTargetRotations;

    private Vector3 _lightClickInitialPosition;
    private Quaternion _lightClickInitialRotation;
    private Vector3[] _lightClickTargetPositions;
    private Quaternion[] _lightClickTargetRotations;

    private struct ClickData
    {
        public float TimePassed;
        public int ImageId;
        public bool Busy;
    }

    private ClickData[] _healthClicksData;
    private ClickData[] _lightClicksData;

    public void Init()
    {
        _state = State.Game;
        _timePassed = 0f;

        _deathScreen.gameObject.SetActive(false);
        _winScreen.gameObject.SetActive(false);

        _heartSourcePosition = _firstHeart.rectTransform.localPosition;
        _heartTargetPosition = _secondHeart.rectTransform.localPosition;
        _secondHeart.gameObject.SetActive(false);

        _firstHeartInitialSizeDelta = _firstHeart.rectTransform.sizeDelta;

        _healthClicksData = new ClickData[_healthClicks.Length];
        _lightClicksData = new ClickData[_lightClicks.Length];

        _healthClickInitialPosition = _healthClickInitial.transform.localPosition;
        _healthClickInitialRotation = _healthClickInitial.transform.localRotation;

        _lightClickInitialPosition = _lightClickInitial.transform.localPosition;
        _lightClickInitialRotation = _lightClickInitial.transform.localRotation;

        _healthClickTargetPositions = new Vector3[_healthClicks.Length];
        _healthClickTargetRotations = new Quaternion[_healthClicks.Length];
        for (var i = 0; i < _healthClicks.Length; i++)
        {
            _healthClickTargetPositions[i] = _healthClicks[i].rectTransform.localPosition;
            _healthClickTargetRotations[i] = _healthClicks[i].rectTransform.localRotation;
            _healthClicks[i].gameObject.SetActive(false);
        }

        _lightClickTargetPositions = new Vector3[_lightClicks.Length];
        _lightClickTargetRotations = new Quaternion[_lightClicks.Length];
        for (var i = 0; i < _lightClicks.Length; i++)
        {
            _lightClickTargetPositions[i] = _lightClicks[i].rectTransform.localPosition;
            _lightClickTargetRotations[i] = _lightClicks[i].rectTransform.localRotation;
            _lightClicks[i].gameObject.SetActive(false);
        }

        _healthClickInitial.gameObject.SetActive(false);
        _lightClickInitial.gameObject.SetActive(false);

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
        _health.fillAmount = _playerStatsController.HealthPart;
        _heartbeatProgress.fillAmount = _playerStatsController.HeartbeatProgressPart;
        _brightness.fillAmount = _playerStatsController.BrightnessPart;
        _lightProgress.fillAmount = _playerStatsController.LightProgressPart;
        _totalHealthClicks.text = _playerStatsController.HeartbeatClickedTotal.ToString();
        _totalLightClicks.text = _playerStatsController.LighterClickedTotal.ToString();

        UpdateHearts(dt);
        AddClicks(_healthClicks, _healthClicksData, _playerStatsController.HealthClickedLastFrame);
        AddClicks(_lightClicks, _lightClicksData, _playerStatsController.LightClickedLastFrame);
        UpdateClicks(_healthClicksData, _healthClicks, _healthClickInitialPosition, _healthClickTargetPositions, _healthClickInitialRotation, _healthClickTargetRotations, dt);
        UpdateClicks(_lightClicksData, _lightClicks, _lightClickInitialPosition, _lightClickTargetPositions, _lightClickInitialRotation, _lightClickTargetRotations, dt);
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

        _firstHeartbeatBeat.rectTransform.localPosition = Vector3.Lerp(firstHeartbeatBeginPosition, firstHeartbeatEndPosition, 0.5f);
        _secondHeartbeatBeat.rectTransform.localPosition = Vector3.Lerp(secondHeartbeatBeginPosition, secondHeartbeatEndPosition, 0.5f);

        var passedPart = _playerStatsController.HeartbeatSequencePassedPart;

        if (passedPart >= firstHeartbeatBeginPart && passedPart <= firstHeartbeatEndPart)
        {
            var totalDistance = firstHeartbeatEndPart - firstHeartbeatBeginPart;
            var currentDistance = passedPart - firstHeartbeatBeginPart;
            var sizeInc = Mathf.Sin(currentDistance / totalDistance * Mathf.PI) * _heartSizeMult;
            _firstHeart.rectTransform.sizeDelta = _firstHeartInitialSizeDelta + new Vector2(sizeInc, sizeInc);
        }
        else if (passedPart >= secondHeartbeatBeginPart && passedPart <= secondHeartbeatEndPart)
        {
            var totalDistance = secondHeartbeatEndPart - secondHeartbeatBeginPart;
            var currentDistance = passedPart - secondHeartbeatBeginPart;
            var sizeInc = Mathf.Sin(currentDistance / totalDistance * Mathf.PI) * _heartSizeMult;
            _firstHeart.rectTransform.sizeDelta = _firstHeartInitialSizeDelta + new Vector2(sizeInc, sizeInc);
        }
        else
        {
            _firstHeart.rectTransform.sizeDelta = _firstHeartInitialSizeDelta;
        }

        var firstHeartPosition = Vector3.Lerp(_heartSourcePosition, _heartTargetPosition, passedPart);
        _firstHeart.rectTransform.localPosition = firstHeartPosition;

        var firstHeartColor = _firstHeart.color;
        firstHeartColor.a = _heartVisibilityCurve.Evaluate(passedPart);
        _firstHeart.color = firstHeartColor;
    }

    private void AddClicks(Image[] images, ClickData[] clickData, bool clicked)
    {
        if (clicked)
        {
            for (var i = 0; i < clickData.Length; i++)
            {
                if (!clickData[i].Busy)
                {
                    clickData[i].TimePassed = 0f;
                    clickData[i].ImageId = i;
                    clickData[i].Busy = true;
                    images[clickData[i].ImageId].gameObject.SetActive(true);

                    break;
                }
            }
        }
    }

    private void UpdateClicks(ClickData[] clicksData, Image[] images, Vector3 initialPosition, Vector3[] targetPositions,
        Quaternion initialRotation, Quaternion[] targetRotations, float dt)
    {
        for (var i = 0; i < clicksData.Length; i++)
        {
            if (!clicksData[i].Busy)
            {
                continue;
            }

            clicksData[i].TimePassed += dt;

            var passedPart = Mathf.Clamp01(clicksData[i].TimePassed / _clickMovementDuration);
            var curvedPart = _clickMovementCurve.Evaluate(passedPart);

            images[clicksData[i].ImageId].rectTransform.localPosition = Vector3.Lerp(initialPosition, targetPositions[i], curvedPart);
            images[clicksData[i].ImageId].rectTransform.localRotation = Quaternion.Lerp(initialRotation, targetRotations[i], curvedPart);

            var alphaPart = _clickVisibilityCurve.Evaluate(passedPart);
            var color = images[clicksData[i].ImageId].color;
            color.a = alphaPart;
            images[clicksData[i].ImageId].color = color;

            if (clicksData[i].TimePassed >= _clickMovementDuration)
            {
                images[clicksData[i].ImageId].gameObject.SetActive(false);
                clicksData[i].Busy = false;
            }
        }
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
