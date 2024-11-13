using System.Collections;
using UnityEngine;

public class K_UFO : MonoBehaviour
{

    public float lookRadius = 10f;
    public Transform target;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly; 
    public float flyHeight = 5f; 
    public float flySpeed = 5f; 
    public float shootingRange = 10f;


    public GameObject enemybullet;
    public Transform spawnPoint;
    private bool isShooting = false;
    private float bulletTime;
    [SerializeField] private float time = 5f;
    public int damageAmount = 10;


    private Rigidbody rb;
    public Animator ani;


    private bool isDead = false;

 
    public event System.Action OnDeath;

    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        ani = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        target = playerObject?.transform;
        currentHp = Hp;

      
        if (fly)
        {
            Vector3 flyPosition = transform.position;
            flyPosition.y = flyHeight;
            transform.position = flyPosition; 

            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    void Update()
    {
        if (isDead || target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        
        if (currentHp <= 0)
        {
            Die();
            return;
        }

        
        if (fly)
        {
            if (distance <= lookRadius)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                direction.y = 0; 
                transform.position += direction * flySpeed * Time.deltaTime; 

           Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
       

     
        if (distance <= shootingRange)
        {
            if (!isShooting)
            {
                isShooting = true;
                StartCoroutine(ShootAtPlayer());
            }
        }
        else
        {
            isShooting = false;
        }
        if(Hp==0)
        {
            Die();
        }
    }

    
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"Damage taken: {damage}. Current HP: {currentHp}"); 
        if (currentHp <= 0)
        {
            Die();
        }
    }


    public void Die()
    {
        if (isDead) return;

        isDead = true; 
        OnDeath?.Invoke();


        if (rb != null)
        {
            rb.useGravity = true; 
            rb.isKinematic = false; 
            rb.velocity = Vector3.zero;

      
            rb.AddForce(new Vector3(0, -5f, 5f), ForceMode.Impulse); 
        }

        if (ani != null)
        {
            ani.SetTrigger("Die"); 
        }

        TryDropItem();
        Destroy(gameObject, 2f); 
    }

  
    void TryDropItem()
    {
        if (dropItems.Length == 0) return;

        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue <= dropChance)
        {
            int randomIndex = UnityEngine.Random.Range(0, dropItems.Length);
            GameObject dropItem = dropItems[randomIndex];
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }


    IEnumerator ShootAtPlayer()
    {
        while (isShooting && !isDead)
        {
            bulletTime -= Time.deltaTime;
            if (bulletTime <= 0)
            {
                bulletTime = time;
                GameObject bulletObj = Instantiate(enemybullet, spawnPoint.position, spawnPoint.rotation);
                K_missilebullet bulletScript = bulletObj.GetComponent<K_missilebullet>();
                if (bulletScript != null)
                {
                    bulletScript.SetTarget(target);
                }

                Destroy(bulletObj, 5f);
            }

            yield return null;
        }
    }
}
