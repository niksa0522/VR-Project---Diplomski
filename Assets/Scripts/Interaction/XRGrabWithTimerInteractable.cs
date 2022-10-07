using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Is used for removing tag from magazine. This enables magazine to snap to gun only when magazine is dropped into slot
public class XRGrabWithTimerInteractable : XRGrabInteractable
{
    [SerializeField] private Magazine _magazine;
    [SerializeField] private string tag;
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        gameObject.tag = tag;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        StartCoroutine(RemoveTagIfNotGrabbed());
        base.OnSelectExited(args);
    }
    IEnumerator RemoveTagIfNotGrabbed()
    {
        for (int i = 0; i < 10; i++)
            yield return null;
        if (!isSelected)
            gameObject.tag = "Untagged";
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (interactor.transform.name.Contains("Controller"))
        {
            if (_magazine.Gun != null)
            {
                return _magazine.Gun.isGrabbedByPlayer;
            }
        }
        else if (interactor.transform.name.Contains("Sphere"))
        {
            if (_magazine.Gun != null)
            {
                return base.IsSelectableBy(interactor);
            }
            GunPointer gunPointer = interactor.transform.GetComponent<GunPointer>();
            return gunPointer.gun.isGrabbedByPlayer && base.IsSelectableBy(interactor);
        }
        return base.IsSelectableBy(interactor);
    }

    
}
