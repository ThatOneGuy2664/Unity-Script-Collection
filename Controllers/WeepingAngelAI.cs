using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Renderer))]
public class WeepingAngelAI : MonoBehaviour
{
    public Transform player;
    public float stopDistance = 1.5f;
    public float fieldOfView = 60f;
    public float checkInterval = 0.1f;

    private NavMeshAgent agent;
    private float nextCheckTime = 0f;
    private bool isPlayerLooking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null && Camera.main != null)
        {
            player = Camera.main.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            isPlayerLooking = IsInView();
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isPlayerLooking && distance > stopDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.isStopped = true;
        }

        if (distance <= stopDistance)
        {
            AttackPlayer();
        }
    }

    bool IsInView()
    {
        Vector3 dirToAngel = transform.position - player.position;
        float angle = Vector3.Angle(player.forward, dirToAngel);

        if (angle < fieldOfView)
        {
            Ray ray = new Ray(player.position, dirToAngel);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == transform)
                {
                    return true;
                }
            }
        }

        return false;
    }

    void AttackPlayer()
    {
        Debug.Log("The Weeping Angel has caught you!");
        // Trigger game over or scare sequence here
    }
}
