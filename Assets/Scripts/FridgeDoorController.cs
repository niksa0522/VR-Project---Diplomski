using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FridgeDoorController : MonoBehaviour
{
    [SerializeField] private HingeJoint hingeJoint;

    [SerializeField] private FridgeDrawerController firstDrawerController;
    [SerializeField] private FridgeDrawerController secondDrawerController;

    [SerializeField] private XRGrabInteractable grabInteractable;

    [SerializeField] private float firstDrawerOpenAngle;

    [SerializeField] private float secondDrawerOpenAngle;
    

    [SerializeField] private float maxLimits = -180f;
    [SerializeField] private float minLimits = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localRotation.z <= firstDrawerOpenAngle)
            {
                firstDrawerController.EnableGrabInteractable();
            }
            else
            {
                firstDrawerController.DisableGrabInteractable();
            }

            if (transform.localRotation.z <= secondDrawerOpenAngle)
            {
                secondDrawerController.EnableGrabInteractable();
            }
            else
            {
                secondDrawerController.DisableGrabInteractable();
            }

            GetDrawerValues();
    }

    private void GetDrawerValues()
    {
        JointLimits limits = hingeJoint.limits;
        if (secondDrawerController.IsMoved)
        {
            limits.min = secondDrawerOpenAngle;
        }
        else if(firstDrawerController.IsMoved)
        {
            limits.min = firstDrawerOpenAngle;
        }
        else
        {
            limits.min = minLimits;
        }
    }

    public bool IsOpen()
    {
        return transform.localRotation.z <= hingeJoint.limits.min + secondDrawerOpenAngle;
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
