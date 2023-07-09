using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestruct : MonoBehaviour
{
    private Explodable _explodable;

    void Start()
    {
        _explodable = GetComponent<Explodable>();
    }

    /// <summary>
    /// ²¥·ÅÆÆËé¶¯Ð§
    /// </summary>
    public void PlayDesTructEffect()
    {
        _explodable.allowRuntimeFragmentation = true;
        _explodable.explode();
        ExplosionForce ef = GameObject.FindObjectOfType<ExplosionForce>();
        ef.doExplosion(transform.position);
    }
}
