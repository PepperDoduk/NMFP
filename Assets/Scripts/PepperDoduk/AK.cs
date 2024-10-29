using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AK : MonoBehaviour
{
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo;
    public int currentAmmo;

    public bool reroading = false;

    private void Update()
    {
        anim.SetInteger("AK", animNum);
        if (Input.GetKeyDown(KeyCode.R))
        {
            animNum = 1;
        }
    }
    public void Fire()
    {
        Debug.Log("Fire");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18fire);
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
        animNum = 0;
    }
}
