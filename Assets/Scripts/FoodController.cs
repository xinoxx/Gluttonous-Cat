using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public static FoodController instance;

    public Collider2D area;

    public float createTime;   // 生成食物间隔时间
    public float deleteTime;   // 消除食物间隔时间

    [SerializeField]
    private List<GameObject> typeFoods = new(); // 不同种类食物

    private List<Transform> playerPos;
    private GameObject foodPrefab;
    private List<GameObject> foods = new();
    private int allCount = 0;
    private int timeCount = 0;  // 计时器
    private float preTimeScale; // 初始TimeScale
    private readonly Dictionary<string, int> foodsScore = new Dictionary<string, int>{ { "Food1(Clone)", 1}, { "Food2(Clone)", 2 }, { "Food3(Clone)", 3 } };

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    void Start()
    {
    }

    void Update()
    {
        DeleteItem();
    }

    public void Init()
    {
        preTimeScale = GameManager.instance.GetGameTimeScale();
        playerPos = PlayerController.instance.GetBodys();
        for (int i = 0; i < foods.Count; i++)
        {
            Destroy(foods[i].gameObject);
        }
        foods.Clear();
        allCount = (typeFoods.Count + 1) * typeFoods.Count / 2; // 生成1~n的和
        StartTimer();
    }

    private void CreateItem()
    {
        foodPrefab = GetFood();
        GameObject newFood = Instantiate(foodPrefab);

        bool findPos = true;  // 防止食物生成在player身上
        int randomCount = 0;
        while (findPos)
        {
            randomCount++;
            float newX = Random.Range(area.bounds.min.x, area.bounds.max.x);
            float newY = Random.Range(area.bounds.min.y, area.bounds.max.y);
            newX = Mathf.Round(newX * 10.0f) / 10;
            newY = Mathf.Round(newY * 10.0f) / 10;
            Vector3 pos = new (newX, newY, 0);
            findPos = false;
            for (int i = 0; i < playerPos.Count; i++)
            {
                float playerX = playerPos[i].position.x;
                float playerY = playerPos[i].position.y;
                if (pos.x >= playerX - 0.5f && pos.x <= playerX + 0.5f) { findPos = true; break; }
                if (pos.y >= playerY - 0.5f && pos.y <= playerY + 0.5f) { findPos = true; break; }
            }
            newFood.transform.position = pos;
            if (randomCount >= 10000) break;
        }
        foods.Add(newFood);
    }

    private void DeleteItem()
    {
        if (Time.time > timeCount * preTimeScale) timeCount += 1;
        if (foods.Count > 0 && timeCount % ((int)(deleteTime / preTimeScale)) == 0)
        {
            timeCount += 1;
            Destroy(foods[0]);
            foods.RemoveAt(0);  // 移除队首食物
        }
    }

    private GameObject GetFood()
    {
        // 模拟权重生成食物，例如有3种类型食物，类型1食物有3/6的可能生成，类型2食物有2/6的可能生成，类型3的食物只有1/6的可能生成
        int random = Random.Range(1, allCount + 1);
        int index = typeFoods.Count - Mathf.FloorToInt((float)random / allCount * typeFoods.Count) - 1;
        index = index < 0 ? 0 : index;
        index = index >= typeFoods.Count ? typeFoods.Count - 1 : index;
        return typeFoods[index];
    }

    // 移除被吃掉的食物
    public void EatFood(GameObject food)
    {
        for(int i = 0; i < foods.Count; i++)
        {
            if (foods[i] == food)
            {
                Destroy(food);
                foods.RemoveAt(i);
                UIManager.Instance.AddScore(foodsScore[food.name]);
                AudioManager.instance.PlayEatAudio();
                break;
            }
        }
    }

    public void StartTimer()
    {
        InvokeRepeating(nameof(CreateItem), 0.02f, createTime); // 重复生成食物
    }

    public void CancleTimer()
    {
        CancelInvoke(nameof(CreateItem));
    }    
}
