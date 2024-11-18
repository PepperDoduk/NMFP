using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_missilebullet : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 10f;
    public float destroyDistance = 0.1f;
    public N_PlayerModel hp;
    public void SetTarget(Transform target)
    {
        targetPosition = target.position;
    }

    void Update()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Explode();
        }
        if (collision.transform.CompareTag("Player"))
        {
            var playerHp = collision.transform.GetComponent<N_PlayerModel>();
            if (playerHp != null)
            {
                playerHp.TakeDamage(10);
            }
            else
            {
                Debug.LogWarning("Player에서 N_PlayerModel을 찾을 수 없습니다.");
            }
        }
    }

    void Explode()
    {
        // 풀에서 파티클을 가져와서 사용
        GameObject particle = ParticlePool.Instance.GetParticle();
        particle.transform.position = transform.position;
        particle.SetActive(true);

        Destroy(gameObject); // 미사일 파괴
    }
}
