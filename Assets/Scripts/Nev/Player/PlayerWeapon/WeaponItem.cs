using UnityEngine;


public class WeaponItem : MonoBehaviour
{
    [SerializeField] private WeaponFactory m_factory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerWeapon weapon = other.GetComponent<PlayerWeapon>();
            weapon.SetWeapon(
                m_factory.CreateWeaponStrategy(),
                m_factory.CreateWeaponData());
            GameObject.Destroy(gameObject);
        }
    }
}