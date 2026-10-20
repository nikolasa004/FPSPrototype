using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MilkShake;

public class CameraShake : MonoBehaviour
{
    public Shaker my_shaker;   //Shaker od kamere
    public ShakePreset shake_preset;   //Podaci o tome koliko ce da se trese
	public void Shake()
    {
        my_shaker.Shake(shake_preset);   //Trese se
    }
}
