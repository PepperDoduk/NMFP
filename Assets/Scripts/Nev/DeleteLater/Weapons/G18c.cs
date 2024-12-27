using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G18c : MonoBehaviour
{
    private PlayerWeapon Weapon;
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo;
    public int currentAmmo;

    public bool reloading = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        Weapon = GameObject.Find("Player").GetComponent<PlayerWeapon>();
        maxAmmo = Weapon.MaxBullet;
        currentAmmo = Weapon.CurrentBullet;
    }

    void Update()
    {
        anim.SetInteger("g18", animNum);

        if (currentAmmo < 1)
        {
            reloading = true;
            animNum = 1;
            Debug.Log("Reroading");
        }

        if(Weapon.IsRepeat)
        {
            if (Input.GetMouseButton(0) && currentAmmo > 0 && !reloading)
            {
                randF = Random.Range(-3, 0);

                animNum = randF;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && !reloading)
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
        currentAmmo--;
        Weapon.OnShoot();
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
        currentAmmo = maxAmmo;
        animNum = 0;
        reloading = false;
    }

    public void ResetAnimation()
    {
        animNum = 0;
    }
}
