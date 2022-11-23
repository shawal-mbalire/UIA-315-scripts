using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public float objectHealth = 100f;
    public float totalHealth = 100f;

    public void objectHitDamage(float amount)
    {
        objectHealth -= amount;

        if(objectHealth < 0f) {
            Die();
        }
    }
    public void objectHealthUpdate(float amount)
    {
        totalHealth += amount;
        objectHealth += amount;
    }
   
    void Die()
    {
        Destroy(gameObject);
    }
}
