using System;
using UnityEngine;

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
    [SerializeField] private float _fearMultiplierLerpCoef;

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
    }

    public void AddHeartbeatClick()
    {
        ++_heartbeatClickedThisLevel;
        ++_heartbeatClickedTotal;
        _health += _heartbeatStats[_heartbeatLevel].HealthGainPerClick * (HeartbeatOnTime() ? 1 : -1);
        _health = Mathf.Min(_health, _heartbeatStats[_heartbeatLevel].MaxHealth);

        var clicksTillNextLevel = _heartbeatStats[_heartbeatLevel].HeartbeatClicksTillNextLevel;
        if (_heartbeatClickedThisLevel >= clicksTillNextLevel)
        {
            _heartbeatClickedThisLevel -= clicksTillNextLevel;
            ++_heartbeatLevel;
            _heartbeatLevel = Math.Min(_heartbeatLevel, _heartbeatStats.Length - 1);
        }
    }

    public void AddLighterClick()
    {
        ++_lighterClickedThisLevel;
        ++_lighterClickedTotal;
        _brightness += _lightStats[_lightLevel].BrightnessGainPerClick;
        _brightness = Mathf.Min(_brightness, _lightStats[_lightLevel].MaxBrightness);

        var clicksTillNextLevel = _lightStats[_lightLevel].LightClicksTillNextLevel;
        if (_lighterClickedThisLevel >= clicksTillNextLevel)
        {
            _lighterClickedThisLevel -= clicksTillNextLevel;
            ++_lightLevel;
            _lightLevel = Math.Min(_lightLevel, _lightStats.Length - 1);
        }
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

    public void SetFearMultiplier(float fearMultiplier)
    {
        _targetFearMultiplier = fearMultiplier;
    }

    private bool HeartbeatOnTime()
    {
        return true;
    }

    public void CustomUpdate(float dt)
    {
        _fearMultiplier = Mathf.Lerp(_fearMultiplier, _targetFearMultiplier, Mathf.Clamp01(_fearMultiplierLerpCoef * dt));

        _health -= _heartbeatStats[_heartbeatLevel].HealthLoseRate * _fearMultiplier * dt;
        _health = Mathf.Clamp01(_health);

        _brightness -= _lightStats[_lightLevel].BrightnessLoseRate * dt;
        _brightness = Mathf.Clamp01(_brightness);
    }
}
