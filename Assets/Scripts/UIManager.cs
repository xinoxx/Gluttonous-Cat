using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GameEnum.Enums;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject mainMenu;
    public GameObject gameMenu;
    public GameObject endMenu;
    public GameObject tipMenu;

    private TMP_Text scoreText;
    private TMP_Text endScore;
    private GameObject rushTextTip;
    private GameObject actionTip;
    private GameObject rushTip;
    private Animator actionTipAnim;
    private int score = 0;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
    }

    private void Start()
    {
        scoreText = gameMenu.transform.GetChild(0).GetComponent<TMP_Text>();
        endScore = endMenu.transform.GetChild(0).GetComponent<TMP_Text>();
        rushTextTip = tipMenu.transform.GetChild(0).gameObject;
        actionTip = tipMenu.transform.GetChild(1).gameObject;
        rushTip = tipMenu.transform.GetChild(2).gameObject;
        actionTipAnim = actionTip.GetComponent<Animator>();
        rushTextTip.SetActive(false);
        actionTip.SetActive(false);
        rushTip.SetActive(false);
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        gameMenu.SetActive(false);
        endMenu.SetActive(false);
        tipMenu.SetActive(true);
    }

    public void StartGame()
    {
        if (scoreText) scoreText.text = "Score: 0";
        score = 0;
        GameManager.instance.StartGame();
        mainMenu.SetActive(false);
        gameMenu.SetActive(true);
        endMenu.SetActive(false);
    }

    public void ShowEndMenu()
    {
        endScore.text = scoreText.text;
        mainMenu.SetActive(false);
        gameMenu.SetActive(false);
        endMenu.SetActive(true);
        tipMenu.SetActive(false);

        rushTextTip.SetActive(false);
        actionTip.SetActive(false);
        rushTip.SetActive(false);
    }

    public void ShowRushTip(bool active)
    {
        rushTextTip.SetActive(active);
    }
    
    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = "Score: " + score.ToString();
    }

    // Êä³ö¼üÅÌ²Ù×÷
    public void ShowActionTip(ActionType action)
    {
        actionTip.SetActive(false);
        rushTip.SetActive(false);
        if (action == ActionType.Rush)
        {
            rushTip.SetActive(true);
            Animator animator = rushTip.GetComponent<Animator>();
            animator.SetBool("show", true);
            animator.Play("RushTipMove", 0, 0);
        }
        else
        {
            actionTip.SetActive(true);
            actionTipAnim.SetBool("show", true);
            actionTipAnim.Play("ActionMove", 0, 0);
            actionTip.transform.eulerAngles = new Vector3(0, 0, 0);
            switch (action)
            {
                case ActionType.KeyDown:
                    actionTip.transform.eulerAngles = new Vector3(180.0f, 0, 0);
                    break;
                case ActionType.KeyUp:
                    break;
                case ActionType.KeyLeft:
                    actionTip.transform.eulerAngles = new Vector3(0, 0, 90.0f);
                    break;
                case ActionType.KeyRight:
                    actionTip.transform.eulerAngles = new Vector3(0, 0, -90.0f);
                    break;
            }
        }
    }
}
