﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G18c : MonoBehaviour
{
    public N_PlayerRayCast playerRay;
    public Animator anim;
    public int animNum = 0;

    public int randF;

    public int maxAmmo = 17;
    public int currentAmmo;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerRay = GameObject.Find("Player").GetComponent<N_PlayerRayCast>();
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        anim.SetInteger("g18", animNum);

        if (currentAmmo < 1)
        {
            animNum = 1;
            playerRay.reroad = true;
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

        if(Input.GetKeyDown(KeyCode.R) && currentAmmo != maxAmmo)
        {
            animNum = 1;
        }
    }

    public void Fire()
    {
        currentAmmo--;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18fire);
    }
    public void R1()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18r1);
    }
    public void R2()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18r2);
    }
    public void R3()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.G18r3);
    }

    public void Reaload()
    {
        currentAmmo = maxAmmo;
        animNum = 0;
        playerRay.reroad = false;
    }
}
