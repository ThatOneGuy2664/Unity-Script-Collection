using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BasicChaseAI : MonoBehaviour
{
    public Transform target;
    public float chaseDistance = 10f;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (target == null && Camera.main != null)
        {
            target = Camera.main.transform; // Default to camera if no target assigned
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= chaseDistance)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.ResetPath(); // Stop chasing if out of range
        }
    }
}
