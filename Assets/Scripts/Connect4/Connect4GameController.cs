using System;
using System.Collections;
using System.Collections.Generic;
using Connect4.Classes;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Connect4GameController : MonoBehaviour
{
    private int currentPlayer = 1;
    public GameObject redPeg;
    public GameObject yellowPeg;

    [SerializeField] private GameObject canvasRotate;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private int RotationSpeed = 1;

    private GameLogic logic;
    private bool isThinking = false;
    private ContextConnect4 context;
    public List<Transform> columnTops;

    private List<GameObject> pegList = new List<GameObject>();

    private void Awake()
    {
        NewGame();
    }

    private void Update()
    {
        if (isThinking)
        {
            canvasRotate.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        }
    }

    public void RestartGame()
    {
        if (pegList.Count > 1)
        {
            NewGame();
        }
        
    }

    private void NewGame()
    {
        logic = new GameLogic();
        context = new ContextConnect4(new Board(), 1);
        foreach (var peg in pegList)
        {
            Destroy(peg);
        }
        if (currentPlayer != 1)
        {
            
            //TODO: Canvas ili nesto drugo promeniti da izgleda kao da razmislja
            isThinking = true;
            canvasRotate.SetActive(true);
            text.gameObject.SetActive(true);
            text.text = "AI razmišlja";
            /*Move p = logic.ReturnMove(context, 5);
            DoMove(p.y);*/
            StartCoroutine(logic.ReturnMoveCoroutine(context, 7, returnValue => DoMove(returnValue.y)));
        }
    }

    private void DoMove(int y)
    {
        canvasRotate.SetActive(false);
        text.gameObject.SetActive(false);
        isThinking = false;
        int player = 1 + (Convert.ToInt32(context.CurrentState.nbMoves()) % 2);
        if (context.CurrentState.isWinningMove(y))
        {
            context.CurrentState.play(y,context.CurrentPlayer);
            //Refresh table not needed it will instanciate object
            AddPeg(y, player);
            context.Next();
            if (player == 1)
            {
                text.gameObject.SetActive(true);
                text.text = "Igra gotova!";
            }
            else
            {
                text.gameObject.SetActive(true);
                text.text = "Igra gotova!";
            }

            

            NewGame();
            return;
        }
        context.CurrentState.play(y,context.CurrentPlayer);
        AddPeg(y, player);
        context.Next();
    }

    public void AddPeg(int y, int player)
    {
        Transform top = columnTops[y];
        if (player == 1)
        {
            GameObject peg = Instantiate(redPeg, top.position,
                top.rotation);
            peg.transform.SetParent(this.transform);
            pegList.Add(peg);
        }
        else
        {
            GameObject peg = Instantiate(yellowPeg, top.position,
                top.rotation);
            peg.transform.SetParent(this.transform);
            pegList.Add(peg);
        }
    }

    //Checks if peg type placed is appropriate and if it is then places new peg and deletes peg in socket
    public void PegPlaced(SelectEnterEventArgs selectEnterEventArgs)
    {
        int pegType = selectEnterEventArgs.interactableObject.transform.name.Contains("Red") ? 1 : 2;
        string columnText = selectEnterEventArgs.interactorObject.transform.name;
        int column = 1;
        if (columnText.Contains("Column"))
        {
            pegType = 1;
            char lastNum = columnText[columnText.Length - 1];
            int.TryParse(lastNum.ToString(), out column);
        }
        DestroyImmediate(selectEnterEventArgs.interactableObject.transform.gameObject);
        if (pegType == context.CurrentPlayer)//mozda i ovde proveriti da li player koji stavlja je ustvari player koji igra
        {
            if (context.CurrentState.canPlay(column - 1))
            {
                
                DoMove(column-1);
                if (context.CurrentPlayer != currentPlayer)
                {
                    //TODO: Canvas ili nesto drugo promeniti da izgleda kao da razmislja
                    isThinking = true;
                    canvasRotate.SetActive(true);
                    text.gameObject.SetActive(true);
                    text.text = "AI razmišlja";
                    /*Move p = logic.ReturnMove(context, 8);
                    DoMove(p.y);*/
                    StartCoroutine(logic.ReturnMoveCoroutine(context, 7, returnValue => DoMove(returnValue.y)));
                }
            }
        }
        
    }
    
}
