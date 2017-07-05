using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public const int maxHealth = 100;
    public int currentHealth = maxHealth;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "magicProjectile")
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(int amount)
    {
      //  Debug.Log("took damage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
            Destroy(this.gameObject);
        }
    }


}