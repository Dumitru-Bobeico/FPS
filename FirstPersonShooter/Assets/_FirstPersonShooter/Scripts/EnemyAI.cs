using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask IsGround;
    public LayerMask IsPlayer; // used for CheckSphere if you keep it

    [Header("Patrol")]
    public Vector3 walkPoint;
    private bool walkPointSet;
    public float walkPointRange;

    [Header("Combat")]
    public float timeBetweenAttacks = 1.2f;
    bool alreadyAttacked;
    public float sightRange = 15f;
    public float attackRange = 10f;
    public bool playerInSightRange, playerInAttackRange;

    [Header("Shooting (raycast)")]
    public Transform shootOrigin;            // assign muzzle/eye transform in inspector (optional)
    public float eyeHeight = 1.5f;           // fallback origin = transform.position + Vector3.up * eyeHeight
    public LayerMask shootMask = ~0;         // what layers the bullet can hit (include Player layer)
    public float damage = 10f;
    public GameObject hitVFX;                // optional, spawn where ray hits non-player
    public float debugRayDuration = 0.5f;
    public bool debugLogs = true;

    [Header("Stats")]
    public float health = 30f;

    private void Awake()
    {
        if (player == null)
        {
            var pgo = GameObject.FindGameObjectWithTag("Player");
            if (pgo != null) player = pgo.transform;
        }

        agent = GetComponent<NavMeshAgent>();

        if (debugLogs) Debug.Log($"Enemy Awake: player found = {player != null}");
    }

    private void Update()
    {
        // Optional: simpler distance checks (less layer-mask-fiddly)
        if (player != null)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            playerInSightRange = dist <= sightRange;
            playerInAttackRange = dist <= attackRange;
        }
        else
        {
            playerInSightRange = playerInAttackRange = false;
        }

        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, IsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        if (player != null) agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        if (player == null) return;

        transform.LookAt(player);

        if (alreadyAttacked) return;

        // Build origin and direction
        Vector3 origin = (shootOrigin != null) ? shootOrigin.position : transform.position + Vector3.up * eyeHeight;
        Vector3 dir = (player.position - origin).normalized;

        // Move origin slightly forward so ray doesn't hit enemy's own collider
        origin += dir * 0.6f;

        // Always draw debug ray so you see it in Scene view even if it misses
        Debug.DrawRay(origin, dir * attackRange, Color.magenta, debugRayDuration);

        int layerMask = shootMask.value == 0 ? ~0 : shootMask.value;

        if (Physics.Raycast(origin, dir, out RaycastHit hit, attackRange, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (debugLogs) Debug.Log($"Enemy ray hit: {hit.collider.name}");

            // Try to find Health on hit object or parent (works if the collider is a child)
            var hp = hit.collider.GetComponentInParent<Health>();
            if (hp != null)
            {
                hp.TakeDamage((int)damage);
                if (debugLogs) Debug.Log($"Dealt {damage} damage to {hp.name}");
            }
            else
            {
                // not a player/health object -> spawn hit vfx on surface
                if (hitVFX != null)
                {
                    var v = Instantiate(hitVFX, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
                    Destroy(v, 2f);
                }
            }
        }
        else
        {
            if (debugLogs) Debug.Log("Enemy raycast missed");
        }

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damageAmount)
    {
        var hp = GetComponent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damageAmount);
        }
    }


    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
