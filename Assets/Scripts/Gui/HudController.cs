using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    [SerializeField] private PlayerStatsController _playerStatsController;

    [SerializeField] private Image _health;
    [SerializeField] private Image _heartbeatProgress;
    [SerializeField] private Image _lightProgress;
    [SerializeField] private Text _heartbeatLevel;
    [SerializeField] private Text _lightLevel;

    public void Init()
    {
        Refresh();
    }

    private void Refresh()
    {
        _health.fillAmount = _playerStatsController.Health;
        _heartbeatProgress.fillAmount = _playerStatsController.HeartbeatProgress;
        _lightProgress.fillAmount = _playerStatsController.LightProgress;
        _heartbeatLevel.text = "Level " + _playerStatsController.HeartbeatLevel;
        _lightLevel.text = "Level " + _playerStatsController.LigthtLevel;
    }

    public void CustomUpdate(float dt)
    {
        Refresh();
    }
}
