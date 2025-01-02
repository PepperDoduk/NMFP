using UnityEngine;


public class WeaponItem : MonoBehaviour
{
    [SerializeField] private WeaponFactory m_factory;
    [SerializeField] private GameObject m_weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
            weapon.SetWeapon(
                m_factory.CreateWeaponStrategy(),
                m_factory.CreateWeaponData(),
                m_weaponPrefab);
            GameObject.Destroy(gameObject);
        }
    }
}