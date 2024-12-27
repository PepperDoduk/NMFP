using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G18c : MonoBehaviour
{
    private PlayerWeapon Weapon;
    public Animator anim;
    public int animNum = 0;
    public int randF;
    public bool reloading = false;

    public int testbullet;

    void Start()
    {
        anim = GetComponent<Animator>();
        Weapon = GameObject.Find("Player").GetComponent<PlayerWeapon>();
    }

    void Update()
    {
        testbullet = Weapon.CurrentBullet;

        anim.SetInteger("g18", animNum);

        if (Weapon.CurrentBullet < 1)
        {
            reloading = true;
            animNum = 1;
            Debug.Log("Reroading");
        }

        if(Weapon.IsRepeat)
        {
            if (Input.GetMouseButton(0) && Weapon.CurrentBullet > 0 && !reloading)
            {
                randF = Random.Range(-3, 0);

                animNum = randF;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && Weapon.CurrentBullet > 0 && !reloading)
            {
                randF = Random.Range(-3, 0);

                animNum = randF;
            }
        }

        if(Input.GetKeyDown(KeyCode.R) && Weapon.CurrentBullet != Weapon.MaxBullet)
        {
            animNum = 1;
        }
    }

    public void Fire()
    {
        Debug.Log("Fire");
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
        Weapon.ReloadBullet();
        animNum = 0;
        reloading = false;
    }

    public void ResetAnimation()
    {
        animNum = 0;
    }
}
