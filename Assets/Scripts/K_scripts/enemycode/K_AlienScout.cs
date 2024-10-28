using UnityEngine;
using System;

public class K_Enemy : MonoBehaviour
{
    public float lookRadius = 10f;
    public Transform target;
    public int Hp;
    private int currentHp;
    public GameObject[] dropItems;
    public float dropChance = 0.5f;
    public bool fly; // 하늘에 떠 있을지 여부
    public float flyHeight = 5f; // 떠 있을 높이

    private Vector3 targetPosition; // 공중에 떠 있을 목표 위치

    public event Action OnDeath;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        target = player.transform;
        currentHp = Hp;

        // 공중 목표 위치 설정
        if (fly)
        {
            targetPosition = transform.position + Vector3.up * flyHeight;
            transform.position = targetPosition;
        }
    }

    void Update()
    {
        if (!fly && target != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke();
        TryDropItem();
        Destroy(gameObject);
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
}
