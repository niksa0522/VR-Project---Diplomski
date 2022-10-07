using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PushableButton : MonoBehaviour
{
    public UnityEvent onPressed, onReleased;

    [SerializeField] private float threshold = 0.1f;

    [SerializeField] private float deadZone = 0.025f;

    private bool isPressed;
    
    [SerializeField] private ConfigurableJoint joint;

    [SerializeField] private Transform buttonTransform;

    private Vector3 buttonStartPos;
    void Start()
    {
        buttonStartPos = buttonTransform.localPosition;
    }
    void Update()
    {
        if(!isPressed && GetValue() + threshold >=1)
            Pressed();
        if(isPressed && GetValue() - threshold <=0)
            Released();
    }
    private float GetValue()
    {
        var value = Vector3.Distance(buttonStartPos, buttonTransform.localPosition) / joint.linearLimit.limit;
        if (Math.Abs(value) < deadZone)
            value = 0;
        return Mathf.Clamp01(value);
    }
    private void Pressed()
    {
        isPressed = true;
        onPressed.Invoke();
    }
    private void Released()
    {
        isPressed = false;
        onReleased.Invoke();
    }
}
