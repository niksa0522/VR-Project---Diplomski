
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRInfiniteSpawner : MonoBehaviour
{
    [SerializeField] private XRBaseInteractable interactablePrefab;
    private XRBaseInteractor interactor;
    private void OnEnable()
    {
        interactor.selectExited.AddListener(OnSelectExited);
    }
    private void OnDisable()
    {
        interactor.selectExited.RemoveListener(OnSelectExited);
    }
    private void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
        OverrideStartingSelectedInteractable();
    }
    void OnSelectExited(SelectExitEventArgs selectExitEventArgs)
    {
        if (selectExitEventArgs.isCanceled)
            return;
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
