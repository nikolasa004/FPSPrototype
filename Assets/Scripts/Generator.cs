using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public ParticleSystem sparks;  //Efekat varnica

    private void Start()
    {
        StartCoroutine(Vreme());
    }
    IEnumerator Vreme()
    {
        int r = Random.Range(1, 61); 
        while (true) //Stalno je tacno
        {
            sparks.Play();  //Pokrece se efekat
            yield return new WaitForSeconds(r);  //Cekamo za od 1 do 60 sekundi
           
        }
    }

}
