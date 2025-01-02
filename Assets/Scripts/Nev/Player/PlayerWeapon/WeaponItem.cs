using UnityEngine;


public class WeaponItem : MonoBehaviour
{
    [SerializeField] private WeaponFactory m_factory;
    [SerializeField] private GameObject m_weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            inventory.ChangeWeapon(m_factory, m_weaponPrefab);
            //Destroy(gameObject);
        }
    }
}