using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FridgeDrawerController : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable grabInteractable;

    [SerializeField] private FridgeDoorController fridgeDoorController;

    private Vector3 startPosition;

    private bool isMoved = false;

    public bool IsMoved
    {
        get => isMoved;
        set => isMoved = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
        /*if (fridgeDoorController.IsOpen())
        {
            grabInteractable.enabled = true;
        }
        else
        {
            grabInteractable.enabled = false;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.y >= startPosition.y-0.05f)
        {
                isMoved = false;
        }
        else
        {
                isMoved = true;
        }

        
    }

    private void FixedUpdate()
    {
        if (transform.localPosition.y >= startPosition.y)
        {
            transform.localPosition = startPosition;
        }
    }

    public void EnableGrabInteractable()
    {
        grabInteractable.enabled = true;
    }

    public void DisableGrabInteractable()
    {
        grabInteractable.enabled = false;
    }
}
