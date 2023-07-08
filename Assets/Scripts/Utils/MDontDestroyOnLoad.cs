using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MDontDestroyOnLoad : MonoBehaviour{
    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
