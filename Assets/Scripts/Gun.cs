using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private Transform barrel;

    [SerializeField] private AudioSource source;

    [SerializeField] private AudioClip clip,magIn,noAmmo;

    [SerializeField] private float speed = 40f;

    [SerializeField] private Magazine magazine;

    [SerializeField] private XRSocketInteractor socket;

    private bool hasReloaded = true;

    private void Start()
    {
        socket.selectEntered.AddListener(AddMagazine);
        socket.selectExited.AddListener(RemoveMagazine);
    }
    public void AddMagazine(SelectEnterEventArgs args)
    {
        magazine = args.interactableObject.transform.GetComponent<Magazine>();
        source.PlayOneShot(magIn);
        hasReloaded = false;
    }

    public void RemoveMagazine(SelectExitEventArgs args)
    {
        magazine = null;
        source.PlayOneShot(magIn);
    }
    public void Reload()
    {
        hasReloaded = true;
    }

    public void FirePressed()
    {
        if(magazine && hasReloaded && magazine.numOfBullets>0)
            Fire();
        else
            source.PlayOneShot(noAmmo);
    }

    private void Fire()
    {
        magazine.numOfBullets--;
        magazine.OnBulletFired();
        GameObject spawnedBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        spawnedBullet.GetComponent<Rigidbody>().velocity = speed * barrel.forward;
        source.PlayOneShot(clip);
        Destroy(spawnedBullet,2);
    }
}
