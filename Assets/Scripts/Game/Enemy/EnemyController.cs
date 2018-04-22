using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _postDeathLifespan = 10.0f;
    [SerializeField] private float _health = 100.0f;
    [SerializeField] private EnemyAI _ai;

    private bool _isAlive = true;
    public bool GetIsAlive() { return _isAlive; }
    private float _healthDefault;

    private void Start()
    {
        _healthDefault = _health;
    }

    private void Update()
    {
        CheckPostDeathLifespan();
    }

    private void CheckPostDeathLifespan()
    {
        if (!GetIsAlive())
        {
            _postDeathLifespan -= Time.deltaTime;
            if (_postDeathLifespan <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Damage(float damage)
    {
        _health -= damage;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_health <= 0)
        {
            _isAlive = false;
            _ai.DisableNavMeshAgent();
            _ai.enabled = false;
        }
    }
}
