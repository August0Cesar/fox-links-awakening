using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public ParticleSystem fxHit;

    public void GetHit(){
        fxHit.Emit(Random.Range(20, 30));
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
