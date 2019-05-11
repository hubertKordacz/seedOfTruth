using UnityEngine;
using System.Collections;
using System;

public class BulletExploding : BulletBase
{
    public float exlosionTime = 0.4f;
    public float exlosionRange = 5.0f;
    public float exlosionPushForce= 5.0f;
    public ParticleSystem explodeParticle;


    protected override void Hit(Collision2D collision)
    {
        isActive = false;
       var all=  Physics2D.CircleCastAll(this.transform.position, exlosionRange, Vector2.one,0);
    
        foreach (var item in all)
        {
            if (item.rigidbody)
            {
                var movement = item.rigidbody.gameObject.GetComponent<PlayerMovement>();

                if (movement)
                {

                    movement.Push(exlosionPushForce / item.distance, item.point - item.centroid);
                }


                var health = item.rigidbody.gameObject.GetComponent<PlayerHealth>();
                if (health)
                {
                    health.DealDamage(damage);

                }
            }
        }
        StartCoroutine(Explode());
    }


    private IEnumerator Explode()
    {
        // particle
        explodeParticle.Play(true);
        this.rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(exlosionTime);

          Destroy(this.gameObject);
    }
}
