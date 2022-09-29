using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRTwoHandGrabInteractable : XRGrabInteractable
{
    [SerializeField] private XRSimpleInteractable secondGrab;

    private IXRInteractor secondHandInteractor;

    private Quaternion attachInitialRotation;
    // Start is called before the first frame update
    void Start()
    {
        secondGrab.selectEntered.AddListener(OnSecondHandGrab);
        secondGrab.selectExited.AddListener(OnSecondHandRelease);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(GrabWeapon);
        selectExited.AddListener(DropWeapon);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        selectEntered.RemoveListener(GrabWeapon);
        selectExited.RemoveListener(DropWeapon);
    }
    

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondHandInteractor!=null && interactorsSelecting.Count > 0)
        {
            var selectingInteractorAttachTransform = firstInteractorSelecting.GetAttachTransform(this);
            //selectingInteractorAttachTransform.rotation = Quaternion.LookRotation(secondHandInteractor.GetAttachTransform(secondGrab).position - selectingInteractorAttachTransform.position);
            
            selectingInteractorAttachTransform.rotation = Quaternion.LookRotation(secondHandInteractor.GetAttachTransform(secondGrab).position - selectingInteractorAttachTransform.position,selectingInteractorAttachTransform.up);
        }
        base.ProcessInteractable(updatePhase);
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isGrabbed = interactorsSelecting.Count>0 && !interactor.Equals(interactorsSelecting[0]);
        return base.IsSelectableBy(interactor) && isGrabbed;
    }

    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        secondHandInteractor = args.interactorObject;
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        secondHandInteractor = null;
    }

    private void GrabWeapon(SelectEnterEventArgs args)
    {
        secondGrab.enabled = true;
        attachInitialRotation = args.interactorObject.GetAttachTransform(args.interactableObject).localRotation;
    }
    private void DropWeapon(SelectExitEventArgs args)
    {
        secondHandInteractor = null;
        secondGrab.enabled = false;
        args.interactorObject.GetAttachTransform(args.interactableObject).localRotation = attachInitialRotation;
    }
    
}
