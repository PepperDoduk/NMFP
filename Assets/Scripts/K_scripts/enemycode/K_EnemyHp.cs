using System;
using UnityEngine;

public class K_EnemyHp : MonoBehaviour
{
    public int Hp;
    private int currentHp;
    private bool isDead = false;
    public event Action OnDeath;
    private Rigidbody rb;
    public GameObject[] dropItems;
    public float dropChance = 50f;
    public bool Isfly = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        currentHp = Hp;
    }
     private void Update()
    {
        if(currentHp<=0||Hp<=0)
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
            rb.isKinematic = false;
            rb.useGravity = true;

            if (!Isfly)
            {
                rb.AddTorque(new Vector3(0, 0, 5), ForceMode.Impulse);
                rb.AddForce(new Vector3(0, -5f, -5f), ForceMode.Impulse);
                Debug.Log("fly");
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.AddForce(new Vector3(0, -5f, 5f), ForceMode.Impulse);
                Debug.Log("not flying");
            }
        }

        TryDropItem();
        Destroy(gameObject, 2f);
    }

    private void TryDropItem()
    {
        if (dropItems.Length == 0) return;

        float randomValue = UnityEngine.Random.Range(0f, 1f);
        if (randomValue * 100f <= dropChance)
        {
            int randomIndex = UnityEngine.Random.Range(0, dropItems.Length);
            GameObject dropItem = dropItems[randomIndex];
            Instantiate(dropItem, transform.position, Quaternion.identity);
        }
    }
}
