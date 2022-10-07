using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DistanceRayController : MonoBehaviour
{
    public GameObject leftDistanceRay;

    public GameObject rightDistanceRay;

    public XRDirectInteractor leftDirectInteractor;

    public XRDirectInteractor rightDirectInteractor;
    void Update()
    {
        leftDistanceRay.SetActive(leftDirectInteractor.interactablesSelected.Count==0);
        rightDistanceRay.SetActive(rightDirectInteractor.interactablesSelected.Count==0);
    }
}
