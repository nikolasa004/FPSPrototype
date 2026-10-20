using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blur : MonoBehaviour
{
    public Shader shader;
    private void Start()
    {
        Bluring();
    }
    void Bluring()
    {
        if (shader != null)
            GetComponent<Camera>().SetReplacementShader(shader, "");
    }
}
