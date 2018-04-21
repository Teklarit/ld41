using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private float _healthLoseRate;
    [SerializeField] private float _healthGainPerClick;

    private float _health;
    private int _heartbeatsClicked;
    private int _lighterClicked;

    public void Init()
    {
        _health = 1f;
        _heartbeatsClicked = 0;
        _lighterClicked = 0;
    }

    public void AddHeartbeatClick()
    {
        if (HeartbeatTime())
        {
            ++_heartbeatsClicked;
            _health += _healthGainPerClick;
        }
    }

    public void AddLighterClick()
    {
        ++_lighterClicked;
    }

    public bool IsDead
    {
        get { return _health <= 0f; }
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
