using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip sound,magazineIn,noAmmo;
    [SerializeField] private Magazine magazine;
    [SerializeField] private XRSocketInteractor socket;
    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    private bool hasReloaded = true;
    public bool isGrabbedByPlayer = false;


    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
        
        socket.selectEntered.AddListener(AddMagazine);
        socket.selectExited.AddListener(RemoveMagazine);
    }

    public void Fire()
    {
        if(magazine && magazine.numOfBullets>0 && hasReloaded)
            gunAnimator.SetTrigger("Fire");
        else
        {
            source.PlayOneShot(noAmmo);
        }
    }

    public void GunGrabbed(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.name.Contains("Controller"))
        {
        isGrabbedByPlayer = true;
        if (args.interactorObject.transform.name.Contains("Left"))
        {
            leftHand.SetActive(true);
        }
        else
        {
            rightHand.SetActive(true);
        }
        }
            
    }

    public void GunDropped(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.name.Contains("Controller"))
        {
            isGrabbedByPlayer = false;
            leftHand.SetActive(false);
            rightHand.SetActive(false);
        }
            
    }

    public void AddMagazine(SelectEnterEventArgs args)
    {
        magazine = args.interactableObject.transform.GetComponent<Magazine>();
        source.PlayOneShot(magazineIn);
        hasReloaded = false;
        magazine.Gun = this;
    }

    public void RemoveMagazine(SelectExitEventArgs args)
    {
        magazine.Gun = null;
        magazine = null;
        source.PlayOneShot(magazineIn);
    }

    public void Reload()
    {
        hasReloaded = true;
    }
    


    //This function creates the bullet behavior
    void Shoot()
    {
        magazine.numOfBullets--;
        magazine.OnBulletFired();
        source.PlayOneShot(sound);
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
