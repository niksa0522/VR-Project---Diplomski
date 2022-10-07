using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using XRController = UnityEngine.InputSystem.XR.XRController;

public class TeleportController : MonoBehaviour
{
    public GameObject teleportRay;
    public XRDirectInteractor directInteractor;
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
            teleportRay.SetActive(true);
            directInteractor.enabled = false;
            distanceRay.SetActive(false);
        }
    }
    public void Released(InputAction.CallbackContext context)
    {
        teleportRay.SetActive(false);
        directInteractor.enabled = true;
        distanceRay.SetActive(true);
    }
}