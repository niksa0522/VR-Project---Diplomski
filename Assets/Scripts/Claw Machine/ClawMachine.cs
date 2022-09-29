using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ClawMachine : MonoBehaviour
{

    [SerializeField] private Transform clawTransform;
    [SerializeField] private Transform bottomClawTransform;
    [SerializeField] private XRSocketInteractor clawSocket;
    [SerializeField] private float clawSpeedWithoutPrize;
    [SerializeField] private float clawDownSpeed = 0.005f;
    [SerializeField] private float clawSpeedWithPrize;
    [SerializeField] private Vector2 maxPosition;
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private float minYPos = -0.01f;
    [SerializeField] private float maxYPos = 0f;

    [SerializeField] private AudioClip craneDown;
    [SerializeField] private AudioClip craneUp;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshPro coinText;

    private int coinCount = 3;

    private bool buttonValue;
    private Vector2 joystickValue;
    private Vector3 startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = clawTransform.position;
        StartCoroutine(NoPrizeState());
    }

    void UpdateClawPositionXZ()
    {
        var clawPosition = clawTransform.localPosition;

        clawPosition += new Vector3(joystickValue.x * clawSpeedWithoutPrize * Time.deltaTime, 0f,
            joystickValue.y * clawSpeedWithoutPrize * Time.deltaTime);

        clawPosition.x = Mathf.Clamp(clawPosition.x, minPosition.x, maxPosition.x);
        clawPosition.z = Mathf.Clamp(clawPosition.z, minPosition.y, maxPosition.y);

        clawTransform.localPosition = clawPosition;
    }
    void UpdateClawPositionY(bool goingDown)
    {
        var clawPosition = bottomClawTransform.localPosition;

        int sign = goingDown ? -1 : 1;

        clawPosition += new Vector3(0f, 0f,sign*clawDownSpeed*Time.deltaTime
            );
        
        bottomClawTransform.localPosition = clawPosition;
    }

    void UpdateClawX(bool start = false)
    {
        int sign = start ? -1 : 1;
        var clawPosition = clawTransform.localPosition;
        clawPosition += new Vector3( sign*clawSpeedWithPrize * Time.deltaTime, 0f,
            0f);

        clawPosition.x = Mathf.Clamp(clawPosition.x, minPosition.x, maxPosition.x);

        clawTransform.localPosition = clawPosition;
    }
    void UpdateClawZ(bool start = false)
    {
        int sign = start ? -1 : 1;
        var clawPosition = clawTransform.localPosition;
        clawPosition += new Vector3( 0f, 0f,
            sign*clawSpeedWithPrize * Time.deltaTime);

        clawPosition.z = Mathf.Clamp(clawPosition.z, minPosition.y, maxPosition.y);

        clawTransform.localPosition = clawPosition;
    }

    IEnumerator NoPrizeState()
    {
        Debug.Log("While coin");
        while (coinCount == 0)
        {
            yield return null;
        }
        Debug.Log("While btnfirst");
        while (!buttonValue)
        {
            yield return null;
        }
        Debug.Log("While claw");
        buttonValue = false;
        coinCount--;
        coinText.text = coinCount > 9 ? coinCount.ToString() : ("0" + coinCount.ToString());
        //debug se if makes problems, if problems are there remove first button press
        while (!buttonValue)
        {
            UpdateClawPositionXZ();
            yield return null;
        }
        Debug.Log("Done while");

        buttonValue = false;

        StartCoroutine(GrabbingState());
    }

    IEnumerator GrabbingState()
    {
        clawSocket.socketActive = true;
        audioSource.PlayOneShot(craneDown);
        while (bottomClawTransform.localPosition.z>minYPos && !clawSocket.hasSelection)
        {
            UpdateClawPositionY(true);
            yield return null;
        }

        bottomClawTransform.localPosition = new Vector3(0f, 0f, minYPos); 
        
        
        audioSource.PlayOneShot(craneUp);
        while (bottomClawTransform.localPosition.z < maxYPos)
        {
            UpdateClawPositionY(false);
            yield return null;
        }
        bottomClawTransform.localPosition = new Vector3(0f, 0f, maxYPos);

        StartCoroutine(GoBackState());
    }

    IEnumerator GoBackState()
    {
        while (clawTransform.localPosition.x < maxPosition.x)
        {
            UpdateClawX();
            yield return null;
        }

        Debug.Log("y");
        while (clawTransform.localPosition.z < maxPosition.y)
        {
            UpdateClawZ();
            yield return null;
        }

        Debug.Log("socket");
        clawSocket.socketActive = false;
        StartCoroutine(GoToStartState());
    }

    IEnumerator GoToStartState()
    {
        Debug.Log("gobackx");
        while (clawTransform.localPosition.x > 0f)
        {
            UpdateClawX(true);
            yield return null;
        }
        Debug.Log("gobacky");
        while (clawTransform.localPosition.z > 0f)
        {
            UpdateClawZ(true);
            yield return null;
        }
        Debug.Log("finish");
        clawTransform.position = startPosition;
        
        StartCoroutine(NoPrizeState());
    }

    public void AddCoin()
    {
        coinCount++;
        if (coinCount > 99)
            coinCount = 99;
        coinText.text = coinCount > 9 ? coinCount.ToString() : ("0" + coinCount.ToString());
    }

    public void OnButtonPress()
    {
        buttonValue = true;
    }

    public void OnButtonRelease()
    {
        buttonValue = false;
    }

    public void OnJoystickValueChangeX(float x)
    {
        joystickValue.x = x;
    }

    public void OnJoystickValueChangeY(float y)
    {
        joystickValue.y = y;
    }
}
