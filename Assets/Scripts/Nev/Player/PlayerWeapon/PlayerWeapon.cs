using UnityEngine;


public class PlayerWeapon : MonoBehaviour
{
    private IWeaponStrategy m_weaponStrategy;
    private WeaponData m_weaponData;

    private int m_currentBullet;
    private bool m_isWeaponInstalled;

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
        UninstallWeapon();

        if (m_weaponData == null || m_weaponData.WeaponPrefab == null)
        {
            Debug.LogError("Weapon data or prefab is not set!");
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas not found in the scene!");
            return;
        }

        GameObject weaponInstance = Instantiate(m_weaponData.WeaponPrefab, canvas.transform);

        RectTransform rectTransform = weaponInstance.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero; // 캔버스 중심에 위치
            rectTransform.sizeDelta = new Vector2(100, 100); // 크기 조정 (필요 시)
        }

        Instantiate(m_weaponData.WeaponPrefab);
        m_isWeaponInstalled = true;
    }

    public void UninstallWeapon()
    {
        m_isWeaponInstalled = false;
    }

    public void SetWeapon(IWeaponStrategy pStrategy, WeaponData pData)
    {
        m_weaponStrategy = pStrategy;
        m_weaponData = pData;
        m_currentBullet = 0;

        if (m_isWeaponInstalled)
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