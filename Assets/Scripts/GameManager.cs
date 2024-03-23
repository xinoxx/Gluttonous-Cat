using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private float gameTimeScale;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 0;
        UIManager.Instance.ShowMainMenu();
    }

    public void StartGame()
    {
        Time.timeScale = gameTimeScale;
        PlayerController.instance.Init();
        FoodController.instance.Init();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        FoodController.instance.CancleTimer();
        UIManager.Instance.ShowMainMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public float GetGameTimeScale()
    {
        return gameTimeScale;
    }    
}
