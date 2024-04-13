using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollissionBall : MonoBehaviour
{
    public GameObject CollissionParticleEffect;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(CollissionParticleEffect, collision.GetContact(0).point, Quaternion.identity);
    }
}
