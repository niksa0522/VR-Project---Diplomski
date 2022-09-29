using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballHoop : MonoBehaviour
{
    [SerializeField] private BasketballLogic basketballLogic;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Contains("Basketball"))
        {
            var velocityNorm = other.attachedRigidbody.velocity.normalized;
            Debug.Log(velocityNorm);
            if (Vector3.Dot(velocityNorm, Vector3.down) > 0.25)
            {
                basketballLogic.AddPoint();
            }
        }
    }
}
