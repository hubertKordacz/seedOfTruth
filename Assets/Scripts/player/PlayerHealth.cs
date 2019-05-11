using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{

    private bool isDead;
    public PlayerGameplaySlot hudSlot;
    public float maxHealth = 100;
    public bool IsDead { get => isDead;  }

    private float health = 100;

    public ParticleSystem hitParticle;

    private void Start()
    {
        health = maxHealth;
        hudSlot.UpdateHealth(health, maxHealth);
    }

    public void DealDamage(float value)
    {
        if(hitParticle)
            hitParticle.Play(true);
        health = Mathf.Max(health - value,0);

        if(hudSlot)
        hudSlot.UpdateHealth(health, maxHealth);
        CheckIfIsDeath();
    }

    private void CheckIfIsDeath()
    {
        if (health <= 0)
        {
           isDead = true;
            var movement = this.GetComponent<PlayerMovement>();
            movement.Kill();
        }
    }

    public void Heal(float value)
    {

        health = Mathf.Min(health + value, maxHealth);
        if (hudSlot)
            hudSlot.UpdateHealth(health, maxHealth);
        CheckIfIsDeath();
    }

    public void RemoveFromGame()
    {
        if (hudSlot)
            Destroy(hudSlot.gameObject);


        Destroy(this.gameObject);

    }
}
