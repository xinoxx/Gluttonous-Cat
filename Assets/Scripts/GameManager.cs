using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int gamePlaySpeed; // ģ����Ϸ�˶��ٶ�

    private int oriGamePlaySpeed = 0;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        instance = this;
    }

    void Start()
    {
        oriGamePlaySpeed = gamePlaySpeed;
        Time.timeScale = 0;
        UIManager.Instance.ShowMainMenu();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        PlayerController.instance.Init();
        FoodController.instance.Init();
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        FoodController.instance.CancleTimer();
        UIManager.Instance.ShowEndMenu();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public int GetGameSpeed() { return gamePlaySpeed; }

    public void SetGameSpeed(int speed) {  gamePlaySpeed = speed; }

    public void ResetGameSpeed() { gamePlaySpeed = oriGamePlaySpeed; }
}
