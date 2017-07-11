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
            //pause the game
            Time.timeScale = 0;
            //turn off the collider so they cant be hurt any more
            this.gameObject.GetComponent<Collider>().enabled = false;
        }
        else
        {
            Invoke("ToggleHurtOverlay", 0.2f);
        }

    }

    void ToggleHurtOverlay()
    {
        GameObject.FindGameObjectWithTag("hurtOverlay").GetComponent<Renderer>().enabled = !(GameObject.FindGameObjectWithTag("hurtOverlay").GetComponent<Renderer>().enabled);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "enemyAttack")
        {
            TakeDamage(25);
        }
    }

}