using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class GavelSlam : MonoBehaviour
{
    [SerializeField] private float velocityNeeded;
    [SerializeField] private AudioSource soundSource;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.transform.name.Equals("GavelEnd"))
        {
            Debug.Log("collision");
            Debug.Log(other.relativeVelocity.magnitude);
            if (other.relativeVelocity.magnitude > velocityNeeded)
            {
                soundSource.Stop();
                soundSource.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Equals("GavelEnd"))
        {
            var gavel = other.transform.GetComponentInParent<Gavel>();
            Debug.Log(gavel.interactor.xrController.transform.name);
            float velocity = 0f;
            if (gavel.interactor.xrController.transform.name.Contains("Right"))
            {
                
            }
            if (velocity > velocityNeeded)
            {
                soundSource.Stop();
                soundSource.Play();
            }
        }
    }
}
