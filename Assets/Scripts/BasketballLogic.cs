using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasketballLogic : MonoBehaviour
{
    [SerializeField] private TextMeshPro score;

    [SerializeField] private TextMeshPro time;
    [SerializeField] private TextMeshPro gameText;
    // Start is called before the first frame update
    private bool timerIsRunning = false;
    private float timeRemaining = 99;
    private bool enableGame = true;
    private int scoreVal = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                enableGame = false;
                gameText.gameObject.SetActive(true);
                gameText.text = "Game Over";
                StartCoroutine(EnableGame());
            }
        }
    }

    public IEnumerator EnableGame()
    {
        yield return new WaitForSeconds(5f);
        enableGame = true;
        gameText.text = "Throw to start new game";
    }

    public void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float seconds = Mathf.FloorToInt(timeToDisplay);
        time.text = seconds.ToString();
    }

    public void AddPoint()
    {
        if (timerIsRunning)
        {
            scoreVal += 3;
            score.text = scoreVal.ToString();
        }
        else
        {
            if (enableGame)
            {
                gameText.gameObject.SetActive(false);
                timerIsRunning = true;
                scoreVal = 3;
                score.text = scoreVal.ToString();
            }
        }
    }
}
