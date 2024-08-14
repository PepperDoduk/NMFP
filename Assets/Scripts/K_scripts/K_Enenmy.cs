using UnityEngine;
using UnityEngine.AI;

public class K_Enenmy : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        
        GameObject player = GameObject.FindWithTag("Player");
        
      
            target = player.transform;
        
       
    }

    void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            agent.SetDestination(target.position);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
