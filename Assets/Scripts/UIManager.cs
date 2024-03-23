using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject mainMenu;
    public GameObject gameMenu;

    private TMP_Text scoreText;
    private int score = 0;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    private void Start()
    {
        scoreText = gameMenu.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
    }

    public void StartGame()
    {
        if (scoreText) scoreText.text = "0";
        score = 0;
        GameManager.instance.StartGame();
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
    }
    
    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();
    }    
}
