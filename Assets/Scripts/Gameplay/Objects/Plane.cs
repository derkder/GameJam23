using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Singleton<Plane>
{
    public Material mat;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
