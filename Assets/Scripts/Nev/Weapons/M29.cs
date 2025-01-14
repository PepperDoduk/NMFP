using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M29 : MonoBehaviour
{
    private PlayerWeapon Weapon;

    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo = 6;
    public int currentAmmo;

    public bool reloading = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        maxAmmo = 6;
        Weapon = GameObject.Find("Player").GetComponent<PlayerWeapon>();
        currentAmmo = maxAmmo;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            reloading = true;
            animNum = 1;
        }

        if (Input.GetMouseButtonDown(0) && animNum != 1)
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

        if (Input.GetMouseButtonUp(0) && animNum != 1)
        {
            animNum = 0;
        }

        anim.SetInteger("M29", animNum);
    }

    public void OpenChamber()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.m29openChamber);
    }
    public void InsertChamber()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.m29ammo);
    }
    public void CloseChamber()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.m29closeChamber);
    }

    public void AmmoCheck()
    {
        if (currentAmmo > 5)
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
            Weapon.OnShoot();

            randF = Random.Range(-2, 0);
            animNum = randF;
            currentAmmo--;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.m29fire);
        }
        else
            AudioManager.instance.PlaySfx(AudioManager.Sfx.AKnoAmmo);
        anim.SetInteger("M29", animNum);
    }

    public void ReloadEnd()
    {
        currentAmmo = maxAmmo;

        animNum = 0;

        reloading = false;
    }
}
