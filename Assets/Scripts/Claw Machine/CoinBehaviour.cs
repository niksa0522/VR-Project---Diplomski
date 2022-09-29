using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 0.1f;
    
    public void MoveToSlot(Transform coinSlot,Action<CoinBehaviour> onFinished)
    {
        StartCoroutine(Move(coinSlot,onFinished));
    }

    IEnumerator Move(Transform coinSlot,Action<CoinBehaviour> onFinished)
    {
        var step = speed * Time.deltaTime;
        while (Vector3.Distance(transform.position, coinSlot.position) < 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position,coinSlot.position,step);
            yield return null;
        }
        onFinished.Invoke(this);
    }
}
