using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using XRController = UnityEngine.InputSystem.XR.XRController;

public class TeleportController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject TeleportRay;
    public XRDirectInteractor DirectInteractor;
    public XRRayInteractor distanceInteractor;
    public GameObject distanceRay;
    public InputAction action;

    public bool EnableTeleport { get; set; } = true;


    private void Awake()
    {
        action.started += Pressed;
        action.canceled += Released;
    }

    private void OnDestroy()
    {
        action.started -= Pressed;
        action.canceled -= Released;
    }

    // Update is called once per frame
    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    public void Pressed(InputAction.CallbackContext context)
    {
        if (EnableTeleport)
        {
            TeleportRay.SetActive(true);
            DirectInteractor.enabled = false;
            distanceInteractor.enabled = false;
            distanceRay.SetActive(false);
            
        }
        
    }

    public void Released(InputAction.CallbackContext context)
    {
        TeleportRay.SetActive(false);
        DirectInteractor.enabled = true;
        distanceRay.SetActive(true);
        distanceInteractor.enabled = true;
    }
}
