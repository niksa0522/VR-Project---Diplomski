using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Magazine : MonoBehaviour
{
    public int numOfBullets = 8;
    [SerializeField] private List<GameObject> bullets;
    
    //Used to check if player can grab magazine when its in the gun
    private SimpleShoot gun;

    public SimpleShoot Gun
    {
        get => gun;
        set => gun = value;
    }

    public void OnBulletFired()
    {
        if (bullets.Count > 0)
        {
            var bulletToDelete = bullets[0];
            bullets.RemoveAt(0);
            Destroy(bulletToDelete);
        }
    }

}
