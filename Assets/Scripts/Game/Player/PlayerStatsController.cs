using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private bool _god;

    [Serializable] private struct HeartbeatStats
    {
        public float MaxHealth;
        public float HealthLoseRate;
        public float HealthGainPerClick;
        public int HeartbeatClicksTillNextLevel;
    }

    [Serializable] private struct LightStats
    {
        public float MaxBrightness;
        public float BrightnessLoseRate;
        public float BrightnessGainPerClick;
        public int LightClicksTillNextLevel;
    }

    [SerializeField] private HeartbeatStats[] _heartbeatStats;
    [SerializeField] private LightStats[] _lightStats;

    [Space]
    [SerializeField] private float _heartbeatsSequenceDuration;
    [SerializeField] private float _firstHeartbeatBeginTime;
    [SerializeField] private float _firstHeartbeatEndTime;
    [SerializeField] private float _secondHeartbeatBeginTime;
    [SerializeField] private float _secondHeartbeatEndTime;
    [SerializeField] private float _fearMultiplierLerpCoef;
    [SerializeField] private float _fearLossRate;

    [Space]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _enemaSqueezeAudioClips;
    [SerializeField] private AudioClip[] _flashlightSqueezeAudioClips;
    [SerializeField] private AudioClip[] _heartbeatAudioClips;
    [SerializeField] private float _firstHeartBeatTime;
    [SerializeField] private float _secondHeartBeatTime;

    private float _health;
    private float _brightness;
    private float _fearMultiplier;
    private float _targetFearMultiplier;
    private int _heartbeatClickedThisLevel;
    private int _lighterClickedThisLevel;
    private int _heartbeatClickedTotal;
    private int _lighterClickedTotal;
    private int _heartbeatLevel;
    private int _lightLevel;

    private float _heartbeatTimePassed;
    private float _heartbeatSequencePassedPart;
    private bool _heartbeatClickAvailable;
    private bool _heartbeatClickPerformed;

    private bool _firstHeartbeatSoundPlayed;
    private bool _secondHeartbeatSoundPlayed;

    private bool _healthClickedLastFrame;
    private bool _lightClickedLastFrame;

    public void Init()
    {
        _heartbeatLevel = 0;
        _lightLevel = 0;

        _health = _heartbeatStats[_heartbeatLevel].MaxHealth;
        _brightness = 0f;
        _fearMultiplier = 1f;
        _targetFearMultiplier = 1f;

        _heartbeatClickedThisLevel = 0;
        _lighterClickedThisLevel = 0;
        _heartbeatClickedTotal = 0;
        _lighterClickedTotal = 0;

        _heartbeatTimePassed = 0f;
        _heartbeatSequencePassedPart = 0f;
        _heartbeatClickAvailable = false;
        _heartbeatClickPerformed = false;

        _firstHeartbeatSoundPlayed = false;
        _secondHeartbeatSoundPlayed = false;

        _healthClickedLastFrame = false;
        _lightClickedLastFrame = false;
    }

    public void AddHeartbeatClick()
    {
        ++_heartbeatClickedThisLevel;
        ++_heartbeatClickedTotal;
        _health += _heartbeatStats[_heartbeatLevel].HealthGainPerClick * (HeartbeatOnTime() ? 1f : -0.25f);
        _health = Mathf.Min(_health, _heartbeatStats[_heartbeatLevel].MaxHealth);

        var clicksTillNextLevel = _heartbeatStats[_heartbeatLevel].HeartbeatClicksTillNextLevel;
        if (_heartbeatClickedThisLevel >= clicksTillNextLevel && _heartbeatLevel < _heartbeatStats.Length - 1)
        {
            _heartbeatClickedThisLevel -= clicksTillNextLevel;
            ++_heartbeatLevel;
        }

        _audioSource.PlayOneShot(_enemaSqueezeAudioClips[Random.Range(0, _enemaSqueezeAudioClips.Length)], 0.5f);
        _healthClickedLastFrame = true;
    }

    public void AddLighterClick()
    {
        ++_lighterClickedThisLevel;
        ++_lighterClickedTotal;
        _brightness += _lightStats[_lightLevel].BrightnessGainPerClick;
        _brightness = Mathf.Min(_brightness, _lightStats[_lightLevel].MaxBrightness);

        var clicksTillNextLevel = _lightStats[_lightLevel].LightClicksTillNextLevel;
        if (_lighterClickedThisLevel >= clicksTillNextLevel && _lightLevel < _lightStats.Length - 1)
        {
            _lighterClickedThisLevel -= clicksTillNextLevel;
            ++_lightLevel;
        }

        _audioSource.PlayOneShot(_flashlightSqueezeAudioClips[Random.Range(0, _flashlightSqueezeAudioClips.Length)], 0.02f);
        _lightClickedLastFrame = true;
    }

    public bool HealthClickedLastFrame
    {
        get { return _healthClickedLastFrame; }
    }

    public bool LightClickedLastFrame
    {
        get { return _lightClickedLastFrame; }
    }

    public bool IsDead
    {
        get { return _health <= 0f && !_god; }
    }

    public float Health
    {
        get { return _health; }
    }

    public float HeartbeatProgress
    {
        get { return (float)_heartbeatClickedThisLevel / _heartbeatStats[_heartbeatLevel].HeartbeatClicksTillNextLevel; }
    }

    public float LightProgress
    {
        get { return (float)_lighterClickedThisLevel / _lightStats[_lightLevel].LightClicksTillNextLevel; }
    }

    public int HeartbeatLevel
    {
        get { return _heartbeatLevel; }
    }

    public int LightLevel
    {
        get { return _lightLevel; }
    }

    public float Brightness
    {
        get { return _brightness; }
    }

    public float HealthPart
    {
        //get { return (_health / _heartbeatStats[_heartbeatLevel].MaxHealth / _heartbeatStats.Length) + ((1f / _heartbeatStats.Length) * (_heartbeatLevel)); }
        get { return _health / _heartbeatStats[_heartbeatLevel].MaxHealth / _heartbeatStats.Length * (_heartbeatLevel + 1); }
    }

    public float HeartbeatProgressPart
    {
        get { return (HeartbeatProgress / _heartbeatStats.Length) + ((1f / _heartbeatStats.Length) * (_heartbeatLevel + 1)); }
    }

    public float BrightnessPart
    {
        //get { return (_brightness / _lightStats[_lightLevel].MaxBrightness / _lightStats.Length) + ((1f / _lightStats.Length) * (_lightLevel)); }
        get { return _brightness / _lightStats[_lightLevel].MaxBrightness / _lightStats.Length * (_lightLevel + 1); }
    }

    public float LightProgressPart
    {
        get { return (LightProgress / _lightStats.Length) + ((1f / _lightStats.Length) * (_lightLevel + 1)); }
    }

    public bool HeartbeatClickAvailable
    {
        get { return _heartbeatClickAvailable; }
    }

    public float HeartbeatSequencePassedPart
    {
        get { return _heartbeatSequencePassedPart; }
    }

    public float FirstHeartbeatBeginPart
    {
        get { return _firstHeartbeatBeginTime / _heartbeatsSequenceDuration; }
    }

    public float FirstHeartbeatEndPart
    {
        get { return _firstHeartbeatEndTime / _heartbeatsSequenceDuration; }
    }

    public float SecondHeartbeatBeginPart
    {
        get { return _secondHeartbeatBeginTime / _heartbeatsSequenceDuration; }
    }

    public float SecondHeartbeatEndPart
    {
        get { return _secondHeartbeatEndTime / _heartbeatsSequenceDuration; }
    }

    public int HeartbeatClickedTotal
    {
        get { return _heartbeatClickedTotal; }
    }

    public int LighterClickedTotal
    {
        get { return _lighterClickedTotal; }
    }

    public void SetFearMultiplier(float fearMultiplier)
    {
        if (_targetFearMultiplier < fearMultiplier)
        {
            _targetFearMultiplier = fearMultiplier;
        }
    }

    private bool HeartbeatOnTime()
    {
        if (_heartbeatClickAvailable && !_heartbeatClickPerformed)
        {
            _heartbeatClickPerformed = true;
            return true;
        }

        return false;
    }

    public void ResetClicks()
    {
        _healthClickedLastFrame = false;
        _lightClickedLastFrame = false;
    }

    public void CustomUpdate(float dt)
    {
        _fearMultiplier = Mathf.Lerp(_fearMultiplier, _targetFearMultiplier, Mathf.Clamp01(_fearMultiplierLerpCoef * dt));
        _fearMultiplier = Mathf.Max(_fearMultiplier, 1f);

        _targetFearMultiplier -= _fearLossRate * dt;
        _targetFearMultiplier = Mathf.Max(_targetFearMultiplier, 1f);

        _heartbeatTimePassed += _fearMultiplier * dt;
        _heartbeatSequencePassedPart = Mathf.Clamp01(_heartbeatTimePassed / _heartbeatsSequenceDuration);

        _heartbeatClickAvailable = (_heartbeatSequencePassedPart >= FirstHeartbeatBeginPart &&
                                    _heartbeatSequencePassedPart <= FirstHeartbeatEndPart) ||
                                    (_heartbeatSequencePassedPart >= SecondHeartbeatBeginPart &&
                                     _heartbeatSequencePassedPart <= SecondHeartbeatEndPart);

        if (!_heartbeatClickAvailable)
        {
            _heartbeatClickPerformed = false;
        }

        if (!_firstHeartbeatSoundPlayed && _heartbeatTimePassed >= _firstHeartBeatTime)
        {
            _audioSource.PlayOneShot(_heartbeatAudioClips[Random.Range(0, _heartbeatAudioClips.Length)], 1f);
            _firstHeartbeatSoundPlayed = true;
        }

        if (!_secondHeartbeatSoundPlayed && _heartbeatTimePassed >= _secondHeartBeatTime)
        {
            _audioSource.PlayOneShot(_heartbeatAudioClips[Random.Range(0, _heartbeatAudioClips.Length)], 1f);
            _secondHeartbeatSoundPlayed = true;
        }

        if (_heartbeatTimePassed >= _heartbeatsSequenceDuration)
        {
            _heartbeatTimePassed -= _heartbeatsSequenceDuration;
            _firstHeartbeatSoundPlayed = false;
            _secondHeartbeatSoundPlayed = false;
        }

        _health -= _heartbeatStats[_heartbeatLevel].HealthLoseRate * _fearMultiplier * dt;
        _health = Mathf.Clamp01(_health);

        _brightness -= _lightStats[_lightLevel].BrightnessLoseRate * dt;
        _brightness = Mathf.Clamp01(_brightness);
    }
}
