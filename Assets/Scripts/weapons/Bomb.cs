using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    public float waitTime = 5.0f;
    public float warinigTime = 2.0f;

    public AudioClip warningSound;
    public AudioClip explosionSound;
    public float exlosionTime = 0.4f;
    public float exlosionRange = 5.0f;
    public float exlosionPushForce = 5.0f;
    public float damage = 5.0f;

    private AudioSource audioSource;
    public ParticleSystem eggParticle;

    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(Wait());
    }


    private IEnumerator   Wait()
    {
        yield return new WaitForSeconds(waitTime - warinigTime);

        if (animator)
            animator.SetTrigger("warning");
      

        if (audioSource != null && warningSound != null)
            audioSource.PlayOneShot(warningSound);

        yield return new WaitForSeconds(warinigTime);


        var all = Physics2D.CircleCastAll(this.transform.position, exlosionRange, Vector2.one, 0);

        foreach (var item in all)
        {
            if (item.rigidbody)
            {
                var movement = item.rigidbody.gameObject.GetComponent<PlayerMovement>();

                if (movement)
                {
                    Debug.Log("distance " + item.distance);
                    movement.Push(item.distance > 0 ? exlosionPushForce / item.distance : exlosionPushForce, item.point - item.centroid);
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
      
        if (audioSource != null && explosionSound != null)
            audioSource.PlayOneShot(explosionSound);

        eggParticle.Play(true);
        yield return new WaitForSeconds(exlosionTime);

        Destroy(this.gameObject);
    }
}
