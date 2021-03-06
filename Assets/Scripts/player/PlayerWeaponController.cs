﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    public List<WeaponBase> weapons = new List<WeaponBase>();
    public Bomb bombPrefab = null;
    public float bombInterval = 10.0f;
    private float lastBombTimeStamp = 0;

    private WeaponBase currentWeapon;
    private float shotTimeStamp = 0;

    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private Collider2D playerCollider;
    private PlayerHealth playerHealth;
  
    private PlayerGameplaySlot hudSlot;


    public Collider2D PlayerCollider { get => playerCollider;  }

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();
        playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth)
            hudSlot = playerHealth.hudSlot;

        lastBombTimeStamp = Time.time;

        UpdateWeaponHud();

    }
    private void Update()
    {
        if ( playerInput== null || playerMovement==null)
            return;
        var dir = new Vector3(playerMovement.Direction, 0, 0);

        if (bombPrefab!=null && lastBombTimeStamp + bombInterval < Time.time)
        {
            Instantiate(bombPrefab,   dir * -.5f + this.transform.position + Vector3.up * -0.32f, Quaternion.identity );
            lastBombTimeStamp = Time.time;
        }

        if(currentWeapon !=null && (playerInput.fireHeld  || playerInput.firePressed) && !playerMovement.isOnWall &&  shotTimeStamp + currentWeapon.cooldown <    Time.time && currentWeapon.HasBullets )
        {
        
            currentWeapon.Fire(dir, dir * .5f + this.transform.position  + Vector3.up * 0.32f);
            shotTimeStamp = Time.time;
            UpdateWeaponHud();
        }

    }
    public void SetCurrentWeapon(WeaponBase.WeaponType type)
    {
        Debug.Log("SET WEAPON: "+ type);

        if (currentWeapon)
            currentWeapon.Deactvate();


        foreach (var weapon in weapons)
        {
            if(weapon.type == type)
            {
                weapon.Activate(this);
                currentWeapon = weapon;
                shotTimeStamp = Time.time- currentWeapon.cooldown;

                UpdateWeaponHud();
                return;
            }
        }


        currentWeapon = null;
    }

    private void UpdateWeaponHud()
    {
        if (hudSlot)
        {
            if (currentWeapon==null)
                hudSlot.UpdateBullets(0, 1);
            else

                hudSlot.UpdateBullets(currentWeapon.bullets, currentWeapon.maxBullets);
        }
    }
}
