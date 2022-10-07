using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class XRJoystick : XRBaseInteractable
{

    
    [SerializeField] private float maxAngle;
    [SerializeField] private float deadZoneAngle;
    [SerializeField] private Vector2 value = Vector2.zero;
    [SerializeField] private Transform handle;
    [SerializeField] private bool recenterOnRelease = true;

    public UnityEvent<float> onValueChangeX, onValueChangeY;
    
    private IXRInteractor interactor;
    
    void Start()
    {
        if (recenterOnRelease)
            SetHandleAngle(Vector2.zero);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(StartGrab);
        selectExited.AddListener(EndGrab);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(StartGrab);
        selectExited.RemoveListener(EndGrab);
    }

    private void StartGrab(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject;
    }

    private void EndGrab(SelectExitEventArgs args)
    {
        UpdateValue();

        if (recenterOnRelease)
        {
            SetHandleAngle(Vector2.zero);
            SetValue(Vector2.zero);
        }

        interactor = null;
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);

        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isSelected)
            {
                UpdateValue();
            }
        }
    }

    private Vector3 GetLookDirection()
    {
        Vector3 direction = interactor.GetAttachTransform(this).position - handle.position;
        direction = transform.InverseTransformDirection(direction);

        direction.y = Mathf.Clamp(direction.y, 0.01f, 1.0f);
        return direction.normalized;
    }

    private void UpdateValue()
    {
        Vector3 lookDirection = GetLookDirection();

        float upDownAngle = Mathf.Atan2(lookDirection.z, lookDirection.y) * Mathf.Rad2Deg;
        float leftRightAngle = Mathf.Atan2(lookDirection.x, lookDirection.y) * Mathf.Rad2Deg;

        float signX = Mathf.Sign(leftRightAngle);
        float signY = Mathf.Sign(upDownAngle);
        
        upDownAngle = Mathf.Abs(upDownAngle);
        leftRightAngle = Mathf.Abs(leftRightAngle);

        Vector2 stickValue = new Vector2(leftRightAngle, upDownAngle) * (1.0f / maxAngle);

        if (stickValue.magnitude > 1.0f)
        {
            stickValue.Normalize();
        }

        leftRightAngle = stickValue.x * signX * maxAngle;
        upDownAngle = stickValue.y * signY * maxAngle;

        float deadZone = deadZoneAngle / maxAngle;
        float aliveZone = 1.0f - deadZone;
        stickValue.x = Mathf.Clamp01(stickValue.x - deadZone) / aliveZone;
        stickValue.y = Mathf.Clamp01(stickValue.y - deadZone) / aliveZone;

        stickValue.x *= signX;
        stickValue.y *= signY;
        
        SetHandleAngle(new Vector2(leftRightAngle,upDownAngle));
        SetValue(stickValue);
    }


    private void SetHandleAngle(Vector2 angle)
    {
        if (handle == null)
            return;

        var xComp = Mathf.Tan(angle.x * Mathf.Deg2Rad);
        var zComp = Mathf.Tan(angle.y * Mathf.Deg2Rad);
        var largerComp = Mathf.Max(Mathf.Abs(xComp), Mathf.Abs(zComp));
        var yComp = Mathf.Sqrt(1.0f - largerComp * largerComp);

        handle.up = (transform.up * yComp) + (transform.right * xComp) + (transform.forward * zComp);
    }

    private void SetValue(Vector2 value)
    {
        this.value = value;
        onValueChangeX.Invoke(this.value.x);
        onValueChangeY.Invoke(this.value.y);
    }
}
