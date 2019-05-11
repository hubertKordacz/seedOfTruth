using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeaponController : MonoBehaviour
{
    public List<WeaponBase> weapons = new List<WeaponBase>();

    private WeaponBase currentWeapon;
    private float shotTimeStamp = 0;

    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private Collider2D playerCollider;

    public Collider2D PlayerCollider { get => playerCollider;  }

    private void Awake()
    {
        playerInput = this.GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();
    }
    private void Update()
    {
        if (currentWeapon == null || playerInput== null || playerMovement==null)
            return;


        if((playerInput.fireHeld  || playerInput.firePressed) && !playerMovement.isOnWall &&  shotTimeStamp + currentWeapon.cooldown <    Time.time )
        {
            var dir = new Vector3(playerMovement.Direction,0, 0);
            currentWeapon.Fire(dir, dir * .5f + this.transform.position  + Vector3.up * 0.32f);
            shotTimeStamp = Time.time;
        }

    }
    public void SetCurrentWeapon(WeaponBase.WeaponType type)
    {
        if (currentWeapon && currentWeapon.type == type)
            return;

        if (currentWeapon)
            currentWeapon.Deactvate();


        foreach (var weapon in weapons)
        {
            if(weapon.type == type)
            {
                weapon.Activate(this);
                currentWeapon = weapon;
                shotTimeStamp = Time.time;

                return;
            }
        }


        currentWeapon = null;
    }
}
