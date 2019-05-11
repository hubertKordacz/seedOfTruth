using System;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    public enum WeaponType
    {
        NONE= 0,
        MACHINEGUN = 1,
        SHOTGUN = 2,
        FLAMETHROWER = 3,
        SNIPERRIFFLE = 4,
        PISTOL = 5,
        ROCKETLAUNCHER = 6

    }

    public BulletBase bulletPrefab;
    public float cooldown;
    public float maxBullets;
    public float bullets;
    public WeaponType type;
    private PlayerWeaponController playerWeaponController;

    public bool HasBullets { get => bullets>0; }

    public void Fire(Vector3 direction , Vector3 position)
    {
        if (bulletPrefab == null)
            return;
        bullets--;
         var bullet = CreateBullet(direction, position);
        bullet.Init(direction);
    }

    private BulletBase CreateBullet(Vector3 direction, Vector3 position)
    {

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
      

        var bullet=  Instantiate(bulletPrefab, position, Quaternion.AngleAxis(angle, Vector3.forward));

        Physics2D.IgnoreCollision(playerWeaponController.PlayerCollider, bullet.GetComponent<Collider2D>());

        return bullet;
    }

    internal void Deactvate()
    {
     
    }



    internal void Activate(PlayerWeaponController playerWeaponController)
    {
        bullets = maxBullets;
        this.playerWeaponController = playerWeaponController;
    }
}
