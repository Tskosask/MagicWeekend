using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public const float maxHealth = 100;
    public float currentHealth = maxHealth;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "magicProjectile")
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(float amount)
    {
      //  Debug.Log("took damage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Enemy Dead!");
            Destroy(this.gameObject);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        TakeDamage(.5f); //every particle is a percent of a hit point
    }


}