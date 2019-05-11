using UnityEngine;
using System.Collections;
using System;


public class BulletBase : MonoBehaviour
{
    public float speed= 10.0f;
    public float damage = 1.0f;

    public float timeToLive = 5.0f;

    public bool destroyOnWall = true;

    private Rigidbody2D rigidBody;

    private bool isActive; 
    // Use this for initialization
    void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }

    internal void Init(Vector3 direction)
    {
        this.rigidBody.AddForce(direction.normalized * speed, ForceMode2D.Impulse );
        isActive = false;

        StartCoroutine(UpdateLive());
    }


    public IEnumerator UpdateLive()
    {
       
        isActive = true;

        yield return new WaitForSeconds(timeToLive);


        DestroySelf();
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!isActive)
            return;


      // if( collision.gameObject.CompareTag("player"))
        {

            var health = collision.gameObject.GetComponent<PlayerHealth>();
            if(health)
            {
                health.DealDamage(damage);
                DestroySelf();
                return;
            }

        }
        if (destroyOnWall)
            DestroySelf();

    }

    private void DestroySelf()
    {
        if (isActive)
        {
            isActive = false;
            Destroy(this.gameObject);
        }
    }
}
