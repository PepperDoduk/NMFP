using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G18c : MonoBehaviour
{
    [SerializeField] private N_WeaponData Data;
    public N_PlayerRayCast playerRay;
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo;
    public int currentAmmo;

    public bool reroading = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerRay = GameObject.Find("Player").GetComponent<N_PlayerRayCast>();
        maxAmmo = Data.MaxAmmo;
    }

    void Update()
    {
        currentAmmo = Data.CurAmmo;
        anim.SetInteger("g18", animNum);

        if (currentAmmo < 1)
        {
            reroading = true;
            animNum = 1;
            Debug.Log("Reroading");
        }

        if(playerRay.repeater)
        {
            if (Input.GetMouseButton(0) && currentAmmo > 0 && !reroading && playerRay.canShot)
            {
                randF = Random.Range(-3, 0);

                animNum = randF;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !reroading && playerRay.canShot)
            {
                randF = Random.Range(-3, 0);

                animNum = randF;
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo)
        {
            animNum = 1;
        }
    }

    public void Fire()
    {
        Debug.Log("Fire");
        Data.CurAmmo--;
        playerRay.Shot();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18fire);
    }
    public void R1()
    {
        Debug.Log("R1");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18r1);
    }
    public void R2()
    {
        Debug.Log("R2");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18r2);
    }
    public void R3()
    {
        Debug.Log("R3");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18r3);
    }

    public void Reaload()
    {
        Debug.Log("Reroaded");
        Data.CurAmmo = maxAmmo;
        animNum = 0;
        reroading = false;
    }

    public void ResetAnimation()
    {
        animNum = 0;
    }
}
