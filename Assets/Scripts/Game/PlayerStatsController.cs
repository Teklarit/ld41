using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private float _initialHealth;
    [SerializeField] private float _healthLoseRate;
    [SerializeField] private float _healthGainPerClick;
    [SerializeField] private int _heartbeatClicksTillNextLevel;
    [SerializeField] private int _lightClicksTillNextLevel;

    private float _health;
    private int _heartbeatClicked;
    private int _lighterClicked;
    private int _heartbeatLevel;
    private int _ligthtLevel;

    public void Init()
    {
        _health = _initialHealth;
        _heartbeatClicked = 0;
        _lighterClicked = 0;
        _heartbeatLevel = 0;
        _ligthtLevel = 0;
    }

    public void AddHeartbeatClick()
    {
        if (HeartbeatTime())
        {
            ++_heartbeatClicked;
            _health += _healthGainPerClick;

            if (_heartbeatClicked >= _heartbeatClicksTillNextLevel)
            {
                ++_heartbeatLevel;
                _heartbeatClicked -= _heartbeatClicksTillNextLevel;
            }
        }
    }

    public void AddLighterClick()
    {
        ++_lighterClicked;

        if (_lighterClicked >= _lightClicksTillNextLevel)
        {
            ++_lighterClicked;
            _lighterClicked -= _lightClicksTillNextLevel;
        }
    }

    public bool IsDead
    {
        get { return _health <= 0f; }
    }

    public float Health
    {
        get { return _health; }
    }

    public float HeartbeatProgress
    {
        get { return (float)_heartbeatClicked / _heartbeatClicksTillNextLevel; }
    }

    public float LightProgress
    {
        get { return (float)_lighterClicked / _lightClicksTillNextLevel; }
    }

    public int HeartbeatLevel
    {
        get { return _heartbeatLevel; }
    }

    public int LigthtLevel
    {
        get { return _ligthtLevel; }
    }

    private bool HeartbeatTime()
    {
        return true;
    }

    public void CustomUpdate(float dt)
    {
        _health -= Mathf.Clamp01(_healthLoseRate * dt);
    }
}
