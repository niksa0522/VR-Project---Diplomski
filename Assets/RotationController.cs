using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RotationController : MonoBehaviour
{

    public ActionBasedSnapTurnProvider snapTurn;
    public ActionBasedContinuousTurnProvider continuousTurnProvider;

    public void SetTypeFromIndex(int index)
    {
        if (index == 0)
        {
            snapTurn.enabled = false;
            continuousTurnProvider.enabled = true;
        }
        else if(index==1)
        {
            snapTurn.enabled = true;
            continuousTurnProvider.enabled = false;
        }
        
    }
}
