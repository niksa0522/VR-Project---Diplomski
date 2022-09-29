using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInfiniteSpawner : MonoBehaviour
{
    [SerializeField]
    private XRBaseInteractable interactablePrefab;

    private XRBaseInteractor interactor;
    

    // Start is called before the first frame update
    private void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
        //Maybe override starting selected interactable
        OverrideStartingSelectedInteractable();
    }

    private void OnEnable()
    {
        Debug.Log("ovde je doslo");
        interactor.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        Debug.Log("ovde je doslo 2");
        interactor.selectExited.RemoveListener(OnSelectExited);
    }
    
    

    void OnSelectExited(SelectExitEventArgs selectExitEventArgs)
    {
        if (selectExitEventArgs.isCanceled)
            return;
        Debug.Log("test");
        Debug.Log(selectExitEventArgs.interactableObject.transform.name);
        InstantiateAndSelectPrefab();
    }

    void InstantiateAndSelectPrefab()
    {
        if (!gameObject.activeInHierarchy || interactor.interactionManager == null)
            return;
        interactor.interactionManager.SelectEnter((IXRSelectInteractor) interactor, InstantiatePrefab());
    }

    XRBaseInteractable InstantiatePrefab()
    {
        var socketTransform = interactor.transform;
        return Instantiate(interactablePrefab, socketTransform.position, socketTransform.rotation);
    }
    
    void OverrideStartingSelectedInteractable()
    {
        interactor.startingSelectedInteractable = InstantiatePrefab();
    }
}
