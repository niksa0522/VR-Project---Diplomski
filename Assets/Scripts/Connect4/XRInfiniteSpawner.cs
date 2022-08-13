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

    [SerializeField] private int playerNum = 1;

    // Start is called before the first frame update
    private void Awake()
    {
        interactor = GetComponent<XRBaseInteractor>();
        
        //Maybe override starting selected interactable
    }

    private void OnEnable()
    {
        interactor.selectExited.AddListener(OnSelectExited);
    }

    private void OnDisable()
    {
        interactor.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectExited(SelectExitEventArgs selectExitEventArgs)
    {
        if (selectExitEventArgs.isCanceled)
            return;
        Debug.Log(selectExitEventArgs.interactorObject.transform.parent.name);
        //checks if parent of controller (CameraOffset) and parent of parent (XRRig) exists and if they have the player script
        if(selectExitEventArgs.interactorObject.transform.parent != null && selectExitEventArgs.interactorObject.transform.parent.parent != null)
            if(selectExitEventArgs.interactorObject.transform.parent.parent.GetComponent<Player>() != null && selectExitEventArgs.interactorObject.transform.parent.parent.GetComponent<Player>().playerNum == playerNum)
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
}
