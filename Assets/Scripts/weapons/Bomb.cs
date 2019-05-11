using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public float waitTime = 5.0f;
    public float warinigTime = 2.0f;

    public float exlosionTime = 0.4f;
    public float exlosionRange = 5.0f;
    public float exlosionPushForce = 5.0f;
    public float damage = 5.0f;

    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        StartCoroutine(Wait());
    }


    private IEnumerator   Wait()
    {
        yield return new WaitForSeconds(waitTime - warinigTime);

        if (animator)
            animator.SetTrigger("warning");


        yield return new WaitForSeconds(warinigTime);


        var all = Physics2D.CircleCastAll(this.transform.position, exlosionRange, Vector2.one, 0);

        foreach (var item in all)
        {
            if (item.rigidbody)
            {
                var movement = item.rigidbody.gameObject.GetComponent<PlayerMovement>();

                if (movement)
                {

                   // movement.Push(exlosionPushForce / item.distance, item.point - item.centroid);
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

        yield return new WaitForSeconds(exlosionTime);

        Destroy(this.gameObject);
    }
}
