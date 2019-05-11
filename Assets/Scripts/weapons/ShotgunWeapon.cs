using UnityEngine;
using System.Collections;

public class ShotgunWeapon : WeaponBase
{

    public int batchSize = 5;
    public int batchAngle = 45;
    public override void Fire(Vector3 direction, Vector3 position, Rigidbody2D body = null)
    {
        if (bulletPrefab == null)
            return;
        bullets--;

        int num = batchSize / 2;
        for (int i = 1; i <= num; i++)
        {
            //Debug.Log(batchAngle / num * i);

            FireSingle(Quaternion.Euler(0, 0,batchAngle / num * i) * direction, position);
            FireSingle(Quaternion.Euler(0,0, batchAngle/num * -i) * direction , position);
        }
        FireSingle(direction, position);
    }

    private void FireSingle(Vector3 direction, Vector3 position)
    {
        var bullet = CreateBullet(direction, position);
        bullet.Init(direction);
    }
}
