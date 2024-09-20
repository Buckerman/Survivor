using UnityEngine;
using UnityEngine.AI;
using QuangDM.Common;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Combat Settings")]
    [SerializeField] private int enemyAttackDamage = 5;
    [SerializeField] private float enemyAttackRange = 1.8f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private float animationSpeed = 1f;

    [Header("Enemy Movement Settings")]
    [SerializeField] private float enemySpeed = 2f;
    [SerializeField] private float enemyRotateSpeed = 150f;
    [SerializeField] private float fallbackDistance = 10f; // Distance below player to fallback to
    [SerializeField] private float maxDistanceFromPlayer = 15f;

    private NavMeshAgent agent;
    private Transform _player;
    private Animator _animator;
    public bool isPaused = false;

    public NavMeshAgent NavMeshAgent => agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        agent.speed = enemySpeed;
    }
    public void Initialize(Transform playerTransform)
    {
        _player = playerTransform;
        agent.enabled = false;
        isPaused = false;
        _animator.SetFloat("Speed", animationSpeed);
        Observer.Instance.AddObserver(EventName.DisableAllEnemies, DisableAllEnemies);
    }
    private void Update()
    {
        if (!isPaused)
        {
            Chase();
        }
    }
    private void Chase()
    {
        if (_player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer > maxDistanceFromPlayer)
        {
            OnDisable();
            return;
        }
        if (distanceToPlayer < 0.1f)
        {
            RotateTowards(_player.position);
            _animator.Play("EnemyAttack");
            return;
        }

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(_player.position, path);
        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(_player.position);

            if (distanceToPlayer <= enemyAttackRange)
            {
                if (IsPlayerInView())
                {
                    agent.isStopped = true;
                    _animator.Play("EnemyAttack");
                }
                else
                {
                    RotateTowards(_player.position);
                    _animator.Play("EnemyWalk");
                }
            }
            else
            {
                agent.isStopped = false;
                _animator.Play("EnemyWalk");
            }
        }
        else
        {
            Vector3 fallbackPosition = GetFallbackPosition();
            if (fallbackPosition != Vector3.zero)
            {
                agent.SetDestination(fallbackPosition);
            }
        }
    }
    private void AttackPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer <= enemyAttackRange && IsPlayerInView())
        {
            _player.GetComponent<PlayerHealth>().TakeDamage(enemyAttackDamage);
        }
    }
    private bool IsPlayerInView()
    {
        Vector3 directionToPlayer = (_player.position - transform.position).normalized;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        return angleToPlayer <= viewAngle / 2f;
    }
    void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, enemyRotateSpeed * Time.deltaTime);
    }
    public void StunEnemy(float duration)
    {
        StartCoroutine(TemporarilyStopEnemy(duration));
    }
    public IEnumerator TemporarilyStopEnemy(float duration)
    {
        isPaused = true;
        agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        agent.isStopped = false;
        isPaused = false;
    }
    private Vector3 GetFallbackPosition()
    {
        Vector3 playerPosition = _player.position;
        Vector3 fallbackPosition = playerPosition - new Vector3(0, fallbackDistance, 0);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fallbackPosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyAttackDamage);
            }
        }
    }
    private void DisableAllEnemies(object data)
    {
        if (this != null)
        {
            OnDisable();
        }
    }
    private void OnDisable()
    {
        Invoke(nameof(RemoveObservers), 0f);
        this.gameObject.SetActive(false);
    }
    private void RemoveObservers()
    {
        if (Observer.Instance != null)
        {
            Observer.Instance.RemoveObserver(EventName.DisableAllEnemies, DisableAllEnemies);
        }
    }
}
