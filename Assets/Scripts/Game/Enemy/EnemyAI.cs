using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum ET_ENEMY_AI_STATE { IDLE, PATROL, MOVE_TO_NOISE, MOVE_TO_PLAYER }

    [SerializeField]
    private ET_ENEMY_AI_STATE _enemyState = ET_ENEMY_AI_STATE.IDLE;
    [Space]
    [SerializeField]
    private float _patrolDestinationAcceptDistance = 1.0f;
    [SerializeField]
    private float _NoiseSensitivityDistance = 10.0f;
    [SerializeField]
    private float _PlayerSensitivityDistance = 5.0f;
    [Space]
    [SerializeField]
    private Transform[] _patrolTransforms;
    private int _targetPatrolIndex = 0;

    private NavMeshAgent _navMeshAgent;
    private Vector3 _lastPlayerVisiblePosition;

    public void DisableNavMeshAgent()
    {
        _navMeshAgent.enabled = false;
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        SetFirstPatrolDestination();
    }

    private void OnEnable()
    {
        EnemiesController.OnClickLighterAction += PlayerClick;
    }

    private void OnDisable()
    {
        EnemiesController.OnClickLighterAction -= PlayerClick;
    }

    void Update()
    {
        UpdateMovement();
    }

    private void PlayerClick(Vector3 position)
    {
        if (_enemyState == ET_ENEMY_AI_STATE.MOVE_TO_PLAYER)
            return;

        NavMeshPath path = new NavMeshPath();
        _navMeshAgent.CalculatePath(position, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
            return;

        float distance = GetPathDistance(path);
        if (distance <= _NoiseSensitivityDistance)
        {
            SetDestination(position, ET_ENEMY_AI_STATE.MOVE_TO_NOISE);
            return;
        }
    }

    // STOP ON HIT!
    private void OnCollisionEnter(Collision collision)
    {
        GetComponent<Rigidbody>().velocity = new Vector3();
    }

    /* Alg:
     1) move to player if distance on navmesh is correct
     2) move to last player position when this one was visible
     3) move to noise position if distance is correct
     4) back to patrol
     */
    private void UpdateMovement()
    {
        if (TryMoveToPlayer()) return;
        if (TryMoveToLastPlayerSeePosition()) return;
        if (TryMoveToNisePosition()) return;

        UpdatePatrolDestination();
    }

    private bool TryMoveToLastPlayerSeePosition()
    {
        if (_enemyState == ET_ENEMY_AI_STATE.MOVE_TO_PLAYER
            && _navMeshAgent.remainingDistance >= _patrolDestinationAcceptDistance)
        {
            return true;
        }
        return false;
    }

    private bool TryMoveToPlayer()
    {
        var player = FindObjectOfType<PlayerController>(); // TODO: use instance
        if (player == null)
            return false;

        NavMeshPath path = new NavMeshPath();
        _navMeshAgent.CalculatePath(player.transform.position, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
            return false;

        float distance = GetPathDistance(path);
        if (distance <= _PlayerSensitivityDistance)
        {
            _lastPlayerVisiblePosition = player.transform.position;
            SetDestination(_lastPlayerVisiblePosition, ET_ENEMY_AI_STATE.MOVE_TO_PLAYER);
            return true;
        }

        return false;
    }

    private bool TryMoveToNisePosition()
    {
        if (_enemyState == ET_ENEMY_AI_STATE.MOVE_TO_NOISE
            && _navMeshAgent.remainingDistance >= _patrolDestinationAcceptDistance)
        {
            return true;
        }
        return false;
    }

    private void SetFirstPatrolDestination()
    {
        if (_patrolTransforms.Length <= 0)
            return;

        _targetPatrolIndex = 0;
        SetDestination(_patrolTransforms[_targetPatrolIndex].position, ET_ENEMY_AI_STATE.PATROL);
    }

    private void UpdatePatrolDestination()
    {
        if (_patrolTransforms.Length <= 1)
        {
            Debug.LogWarning("Give me more patrol Transforms!");
            return;
        }


        if (_enemyState != ET_ENEMY_AI_STATE.PATROL)
            SetDestination(_patrolTransforms[_targetPatrolIndex].position, ET_ENEMY_AI_STATE.PATROL);

        if (_navMeshAgent.remainingDistance <= _patrolDestinationAcceptDistance)
        {
            _targetPatrolIndex = (_targetPatrolIndex + 1) % _patrolTransforms.Length;
            SetDestination(_patrolTransforms[_targetPatrolIndex].position, ET_ENEMY_AI_STATE.PATROL);
        }
    }

    private void SetDestination(Vector3 destination, ET_ENEMY_AI_STATE newEnemyState)
    {
        _navMeshAgent.destination = destination;
        _enemyState = newEnemyState;
    }

    public static float GetPathDistance(NavMeshPath path)
    {
        float length = 0.0f;

        if ((path.status != NavMeshPathStatus.PathInvalid) && (path.corners.Length > 1))
        {
            for (int i = 1; i < path.corners.Length; ++i)
            {
                length += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }
        }
        return length;
    }
}
