using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    // MAIN CANVAS STUFF
    public Text creditsText;
    public Text infoText;
    public Text gameOverText;
    public Text endScoreText;
    public Image blackground;
    public GameHandler GameManager;
    
    // SECONDARY CANVAS STUFF
    public Canvas inGameCanvas;
    public Text scoreText;
    public Text livesText;

    // Start is called before the first frame update
    void Start()
    {
        ClearText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ClearText()
    {
        creditsText.enabled = false;
        infoText.enabled = false;
        gameOverText.enabled = false;
        endScoreText.enabled = false;
    }

    public void DisplayCredits()
    {
        ClearText();
        creditsText.enabled = true;
    }

    public void DisplayInfo()
    {
        ClearText();
        infoText.enabled = true;
    }

    public void StartGame()
    {
        this.gameObject.GetComponent<Canvas>().enabled = false;
        blackground.enabled = false;
        GameManager.enabled = true;
        inGameCanvas.enabled = true;
        if (GameManager.firstPlay)
        {
            GameManager.ReceiveStart();
        }
        else
        {
            GameManager.ResetGame();
        }

    }

    public void GameOver(float score)
    {
        inGameCanvas.enabled = false;
        this.gameObject.GetComponent<Canvas>().enabled = true;
        blackground.enabled = true;
        ClearText();
        endScoreText.text = "Score: " + (int)score;
        endScoreText.enabled = true;
        gameOverText.enabled = true;
    }
}
