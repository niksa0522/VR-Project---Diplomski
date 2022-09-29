using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PegGrabInteractable : XRGrabInteractable
{
    private Player player;
    public int pileNum;
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindObjectOfType<Player>();
    }

    public override bool IsHoverableBy(IXRHoverInteractor interactor)
    {
        if (interactor.transform.name.Contains("Pile"))
        {
            return true;
        }
        else
        {
            //Debug.Log(interactor.transform.name);
            if (player.playerNum != pileNum)
            {
                return false;
            }
        }
        return base.IsHoverableBy(interactor);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (interactor.transform.name.Contains("Pile"))
        {
            return true;
        }
        else
        {
            //Debug.Log(interactor.transform.name);
            if (player.playerNum != pileNum)
            {
                return false;
            }
        }

        return base.IsSelectableBy(interactor);
    }

   /* public override bool CanHover(IXRHoverInteractable interactable)
    {
        if (player.playerNum != pileNum)
            return false;
        return base.CanHover(interactable);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (player.playerNum != pileNum)
            return false;
        return base.CanSelect(interactable);
    }*/
}
