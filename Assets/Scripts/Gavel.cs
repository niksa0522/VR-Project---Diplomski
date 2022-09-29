using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gavel : MonoBehaviour
{
    public XRDirectInteractor interactor;
    public void SetInteractor(SelectEnterEventArgs args)
    {
        interactor = args.interactorObject as XRDirectInteractor;
    }

    public void ClearInteractor()
    {
        interactor = null;
    }

}
