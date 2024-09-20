using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_missilebullet : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 10f;
    public float destroyDistance = 0.1f;
    public Material explosionMaterial; // 폭발 효과로 사용할 Material
    private Renderer missileRenderer; // 미사일의 렌더러

    void Start()
    {
        missileRenderer = GetComponent<Renderer>(); // 렌더러 컴포넌트 가져오기
    }

    public void SetTarget(Transform target)
    {
        targetPosition = target.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) <= destroyDistance)
        {
            Explode();
            return;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("wall"))
        {
            Explode();
        }
    }

    void Explode()
    {
        missileRenderer.material = explosionMaterial; // 폭발 Material 적용
        Destroy(gameObject, 0.5f); // 0.5초 후 미사일 파괴
    }
}
