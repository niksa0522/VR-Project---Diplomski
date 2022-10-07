using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class CoinSlot : XRBaseInteractable
{
    [SerializeField]
    private InputAction input;

    [SerializeField] private Transform coinSlot;

    [SerializeField] private CoinBehaviour coin;

    [SerializeField] private AudioSource source;

    [SerializeField] private ClawMachine clawMachine;
    // Start is called before the first frame update

    protected override void OnEnable()
    {
        base.OnEnable();
        selectEntered.AddListener(AddCoinSelect);
    }

    public void AddCoinSelect(SelectEnterEventArgs args)
    {
        AddCoin();
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        base.ProcessInteractable(updatePhase);
        
        if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
        {
            if (isHovered && input.WasPressedThisFrame())
            {
                AddCoin();
            }
        }
    }

    private void AddCoin()
    {
        var coinObject = Instantiate(coin, transform);
        source.Play();
        clawMachine.AddCoin();
        coinObject.MoveToSlot(transform,action => {Destroy(coinObject);});
    }
}
