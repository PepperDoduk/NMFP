using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barret : MonoBehaviour
{

    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo = 10;
    public int currentAmmo;


    void Start()
    {
        anim = GetComponent<Animator>();

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        anim.SetInteger("M82", animNum);

        if (currentAmmo < 1)
        {
            animNum = 1;
            
        }

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            randF = Random.Range(-3, 0);

            animNum = randF;
        }
        if(Input.GetMouseButtonUp(0))
        {
            animNum = 0;
        }
    }

    public void Fire()
    {
        
        currentAmmo--;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BarretFire);
    }
    public void RemoveMag()
    {
        
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BarretR1);
    }
    public void MagCombination()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BarretR2);
    }
    public void MagHit()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BarretR3);
    }
    public void RetractBolt()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BarretR4);
    }
    public void Boltadvance()
    {
        
        AudioManager.instance.PlaySfx(AudioManager.Sfx.BarretR5);
    }

    public void Reaload()
    {
        currentAmmo = maxAmmo;
        animNum = 0;
    }
}
