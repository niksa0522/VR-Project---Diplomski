using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTargetReached : MonoBehaviour
{
    [SerializeField]
    private float threshold = 0.02f;
    [SerializeField]
    private Transform target;

    
    public UnityEvent OnReached;

    private bool hasReached = false;
    // Start is called before the first frame update
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        if (distance < threshold && !hasReached)
        {
            hasReached = true;
            OnReached.Invoke();
        }
        else if (distance >= threshold)
        {
            hasReached = false;
        }
    }
}
