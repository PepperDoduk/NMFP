using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class K_missilebullet : MonoBehaviour
{
    private Vector3 targetPosition;
    public float speed = 10f;
    public float destroyDistance = 0.1f;
    public Material explosionMaterial; // ���� ȿ���� ����� Material
    private Renderer missileRenderer; // �̻����� ������

    void Start()
    {
        missileRenderer = GetComponent<Renderer>(); // ������ ������Ʈ ��������
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
        missileRenderer.material = explosionMaterial; // ���� Material ����
        Destroy(gameObject, 0.5f); // 0.5�� �� �̻��� �ı�
    }
}
