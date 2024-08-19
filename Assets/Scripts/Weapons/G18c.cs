using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G18c : MonoBehaviour
{
    public N_PlayerRayCast playerRay;
    public N_WeaponController Weapon;
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo;
    public int currentAmmo;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerRay = GameObject.Find("Player").GetComponent<N_PlayerRayCast>();
        Weapon = GameObject.Find("Player").GetComponent<N_WeaponController>();
        maxAmmo = GetComponent<N_WeaponData>().MaxAmmo;
    }

    void Update()
    {
        currentAmmo = Weapon.curWeapon.Data.CurAmmo;
        anim.SetInteger("g18", animNum);

        if (currentAmmo < 1)
        {
            animNum = 1;
            playerRay.reroad = true;
            Weapon.curWeapon.Data.CurAmmo = maxAmmo;
            Debug.Log("Reroaded");
        }

        if(playerRay.repeater)
        {
            if (Input.GetMouseButton(0) && currentAmmo > 0 && playerRay.canShot)
            {
                randF = Random.Range(-3, 0);

                animNum = randF;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && playerRay.canShot)
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
        Weapon.curWeapon.Data.CurAmmo--;
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
        Debug.Log("reroading");
        Weapon.curWeapon.Data.CurAmmo = maxAmmo;
        animNum = 0;
        playerRay.reroad = false;
    }

    public void ResetAnimation()
    {
        animNum = 0;
    }
}
