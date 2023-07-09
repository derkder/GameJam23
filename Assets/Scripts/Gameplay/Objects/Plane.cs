using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : Singleton<Plane>
{
    // Start is called before the first frame update
    void Start() {
    }

    public void UpdateImage(string name, Material mat) {
        GetComponent<MeshRenderer>().material = mat;
    }
}
