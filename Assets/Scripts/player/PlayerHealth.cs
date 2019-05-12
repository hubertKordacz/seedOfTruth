using UnityEngine;
using System.Collections;
using System;

public class PlayerHealth : MonoBehaviour
{


    public PlayerGameplaySlot hudSlot;
    public float maxHealth = 100;
    public bool IsDead { get => movement.isDead;  }

    private float health = 100;

    public ParticleSystem hitParticle;
    private PlayerMovement movement;
    private void Start()
    {
        movement=  this.GetComponent<PlayerMovement>();
        health = maxHealth;
        hudSlot.UpdateHealth(health, maxHealth);
    }

    public void DealDamage(float value)
    {
        if (IsDead)
            return;

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
           movement.Kill();
        }
    }

    public void Heal(float value)
    {
        if (IsDead)
            return;

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
