using UnityEngine;


public class WeaponItem : MonoBehaviour
{
    [SerializeField] private WeaponFactory m_factory;
    [SerializeField] private GameObject m_weaponPrefab;

    PlayerInventory inventory;
    bool flag = false;

    private void Awake()
    {
        inventory = GameObject.Find("Player").GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && flag)
        {
            inventory.ChangeWeapon(m_factory, m_weaponPrefab, gameObject);

            gameObject.SetActive(false);
            //Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flag = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            flag = false;
        }
    }
}