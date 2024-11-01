using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK47 : MonoBehaviour
{
    [SerializeField] private N_WeaponData Data;
    public N_PlayerRayCast playerRay;
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int AKfire;

    public int maxAmmo;
    public int currentAmmo;

    public bool reloading = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerRay = GameObject.Find("Player").GetComponent<N_PlayerRayCast>();
        maxAmmo = Data.MaxAmmo;
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloading = true;
            if (currentAmmo > 0)
                animNum = 2;
            else
                animNum = 1;
        }

        if (Input.GetMouseButtonDown(0) && animNum != 1 && !reloading && playerRay.canShot)
        {

            if (currentAmmo <= 0)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.AKnoAmmo);
            }
            else
            {
                randF = Random.Range(-2, 0);
                animNum = randF;
            }
        }

        if (Input.GetMouseButtonUp(0) && animNum != 1 && !reloading)
        {
            animNum = 0;
            AKfire = 0;
        }

        anim.SetInteger("AK", animNum);
    }

    public void AmmoCheck()
    {
        if (currentAmmo > 30)
        {
            animNum = 0;
            return;
        }
        if (currentAmmo < 1)
        {
            animNum = 0;
            return;
        }
    }
    public void Fire()
    {
        if (currentAmmo > 0)
        {
            randF = Random.Range(-2, 0);
            animNum = randF;
            playerRay.Shot();
            currentAmmo--;

            if (AKfire == 0)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.AKfire1);
                AKfire = 1;
            }
            else
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.AKfire2);
            }

            Debug.Log("Fire");
        }
        else
            AudioManager.instance.PlaySfx(AudioManager.Sfx.AKnoAmmo);
        anim.SetInteger("AK", animNum);
    }



    public void OutMag()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AKr1);
    }
    public void OutMag2()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AKr2);
    }
    public void Vest()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AKr3);
    }
    public void Comb()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AKr4);
    }
    public void Comb2()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AKr5);
    }
    public void Bolt()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AKr6);
    }
    public void Bolt2()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AKr7);
    }

    public void ReloadEnd()
    {
        if (animNum == 2)
            currentAmmo = maxAmmo + 1;
        else
            currentAmmo = maxAmmo;

        reloading = false;
        animNum = 0;
    }
}
