using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;  //Zivot
    public void TakeDamge(float amount)
    {
        health -= amount;   //Smanjuje se zivot za jacinu oruzja

        if (health <= 0f)  //Ako je zivot nula ili manje
        {
            Die();
        }
    }

    void Die()  //Sta se desava kada umre
    {
        Destroy(gameObject);  //Kao proba posle smrti ce samo da nestane objekat ali treba staviti neku animaciju, postoji u assetsima kao da "izgori"
    }
}
