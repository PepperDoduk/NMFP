using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject m_weaponPrefab;
    private IWeaponStrategy m_weaponStrategy;
    private WeaponData m_weaponData;

    private int m_currentBullet;

    public bool IsRepeat;
    public int MaxBullet => m_weaponData.MaxBullet;
    public int CurrentBullet => m_currentBullet;

    public void SetCurrentBullet(int pBullet)
    {
        if (pBullet < 0)
            m_currentBullet = 0;
        else
            m_currentBullet = pBullet;
    }

    public void ReloadBullet()
    {
        SetCurrentBullet(MaxBullet);
    }

    public void OnShoot()
    {
        m_weaponStrategy.OnShoot();
        SetCurrentBullet(m_currentBullet - 1);
    }

    public void InstallWeapon()
    {
        m_weaponPrefab.SetActive(true);
    }

    public void UninstallWeapon()
    {
        m_weaponPrefab.SetActive(false);
    }

    public void SetWeapon(IWeaponStrategy pStrategy, WeaponData pData, GameObject pWeaponPrefab)
    {
        m_weaponStrategy = pStrategy;
        m_weaponData = pData;
        m_weaponPrefab = pWeaponPrefab;
        m_currentBullet = 0;

        InstallWeapon();
    }

    private void Awake()
    {
        IsRepeat = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            IsRepeat = true;
        }
    }
}