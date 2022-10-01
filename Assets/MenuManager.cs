using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public GameObject menu;
    public InputAction showButton;
    public Transform head;
    public float spawnDistance = 2;

    private void Awake()
    {
        showButton.started += Pressed;
    }
    public void Pressed(InputAction.CallbackContext context)
    {
        menu.SetActive(!menu.activeSelf);

        menu.transform.position =
            head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
    }

    private void Update()
    {
        menu.transform.LookAt(new Vector3(head.position.x,menu.transform.position.y,head.position.z));
        menu.transform.forward *= -1;
    }
}
