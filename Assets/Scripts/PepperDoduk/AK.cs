using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK : MonoBehaviour
{
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int AKfire;

    public int maxAmmo;
    public int currentAmmo;

    public bool reloading = false;

    private void Start()
    {
        maxAmmo = 30;
        currentAmmo = maxAmmo;
        animNum = 0;
    }

    private void Update()
    {
        anim.SetInteger("AK", animNum);

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentAmmo > 0)
            {
                animNum = 2;
            }else
            animNum = 1;
        }

        if (Input.GetMouseButtonDown(0) && animNum != 1)
        {

            if (currentAmmo < 1)
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
            AKfire = 0;
        }
    }

    public void AmmoCheck()
    {
        if(currentAmmo > 30)
        {
            animNum = 0;
            return;
        }
        if (currentAmmo < 1) {
            animNum = 0;
            return;
        }
    }
    public void Fire()
    {
        randF = Random.Range(-2, 0);
        if (currentAmmo > 0)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.AKnoAmmo);
            currentAmmo--;
            if (AKfire != 1 && AKfire == 0)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.AKfire1);
                AKfire = 1;
            }

            else if (AKfire == 1)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.AKfire2);
            }
        }
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
        
        if(animNum == 2)
        {
            currentAmmo = maxAmmo + 1;
            animNum = 0;
        }
        else
        currentAmmo = maxAmmo;
        animNum = 0;

    }
}
