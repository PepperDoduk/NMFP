using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M29revolver : MonoBehaviour
{
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo;
    public int currentAmmo;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
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
            randF = Random.Range(-2, 0);
            animNum = randF;
            currentAmmo--;
                AudioManager.instance.PlaySfx(AudioManager.Sfx.m29fire);
        }
        else
            AudioManager.instance.PlaySfx(AudioManager.Sfx.AKnoAmmo);
        anim.SetInteger("M29", animNum);
    }

}
