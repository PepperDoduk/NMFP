using UnityEngine;
using UnityEngine.AI;

public class K_Enenmy : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public NavMeshAgent agent;
    public int Hp;
    private int currenthp;
    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject player = GameObject.FindWithTag("Player");
        target = player.transform;
        currenthp = Hp;
    }

    void Update()
    {

        float distance = Vector3.Distance(target.position, transform.position);
        agent.SetDestination(target.position);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
    public void TakeDamage(int damage)
    {
        currenthp -= damage;
        if(currenthp<=0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
}
