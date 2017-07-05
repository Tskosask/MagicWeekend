using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public const int maxHealth = 100;
    public int currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        //vibrate on damage


        //turn on hurt overlay
        ToggleHurtOverlay();
        Debug.Log("took damage");
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead!");
        }
        else
        {
            Invoke("ToggleHurtOverlay", 0.2f);
        }

    }

    void ToggleHurtOverlay()
    {
     //   GameObject.FindGameObjectWithTag("hurtOverlay").GetComponent<Renderer>().enabled = !(GameObject.FindGameObjectWithTag("hurtOverlay").GetComponent<Renderer>().enabled);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyProjectile")
        {
            TakeDamage(25);
        }
    }

}