using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _postDeathLifespan = 10.0f;
    [SerializeField] private float _health = 100.0f;
    [SerializeField] private float _deathMaterialSpeed = 1.0f;
    [SerializeField] private EnemyAI _ai;
    [SerializeField] private Animator _animator;
    [SerializeField] private Renderer _monsterRenderer;

    private bool _prevStatAttack = false;

    private bool _isAlive = true;
    public bool GetIsAlive() { return _isAlive; }
    private float _healthDefault;

    private float _postDeathAlpha = 1.0f;

    private void Start()
    {
        _healthDefault = _health;
    }

    private void Update()
    {
        UpdatePostDeathMaterial();
        CheckAttackState();
        CheckPostDeathLifespan();
    }

    private void UpdatePostDeathMaterial()
    {
        if (!_isAlive)
        {
            _postDeathAlpha -= Time.deltaTime * _deathMaterialSpeed;
            if (_postDeathAlpha <= 0)
                _animator.SetTrigger("Death");
            _postDeathAlpha = Mathf.Clamp01(_postDeathAlpha);

            var color = _monsterRenderer.material.color;
            color.a = _postDeathAlpha;
            _monsterRenderer.material.color = color;
        }
    }

    private void CheckAttackState()
    {
        bool isNowAttack = (_ai.GetEnemyState() == EnemyAI.ET_ENEMY_AI_STATE.MOVE_TO_PLAYER);
        if (isNowAttack != _prevStatAttack)
        {
            if (isNowAttack)
                _animator.SetTrigger("Attack");
            else
                _animator.SetTrigger("Idle");
        }
        _prevStatAttack = isNowAttack;
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
