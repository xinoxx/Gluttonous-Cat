using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public static FoodController instance;

    public Collider2D area;

    public float createTime;   // ����ʳ����ʱ��
    public float deleteTime;   // ����ʳ����ʱ��

    [SerializeField]
    private List<GameObject> typeFoods = new(); // ��ͬ����ʳ��

    private List<Transform> playerPos;
    private GameObject foodPrefab;
    private List<GameObject> foods = new();
    private int allCount = 0;
    private int timeCount = 0;  // ��ʱ��
    private float preTimeScale; // ��ʼTimeScale
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
        allCount = (typeFoods.Count + 1) * typeFoods.Count / 2; // ����1~n�ĺ�
        StartTimer();
    }

    private void CreateItem()
    {
        foodPrefab = GetFood();
        GameObject newFood = Instantiate(foodPrefab);

        bool findPos = true;  // ��ֹʳ��������player����
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
            foods.RemoveAt(0);  // �Ƴ�����ʳ��
        }
    }

    private GameObject GetFood()
    {
        // ģ��Ȩ������ʳ�������3������ʳ�����1ʳ����3/6�Ŀ������ɣ�����2ʳ����2/6�Ŀ������ɣ�����3��ʳ��ֻ��1/6�Ŀ�������
        int random = Random.Range(1, allCount + 1);
        int index = typeFoods.Count - Mathf.FloorToInt((float)random / allCount * typeFoods.Count) - 1;
        index = index < 0 ? 0 : index;
        index = index >= typeFoods.Count ? typeFoods.Count - 1 : index;
        return typeFoods[index];
    }

    // �Ƴ����Ե���ʳ��
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
        InvokeRepeating(nameof(CreateItem), 0.02f, createTime); // �ظ�����ʳ��
    }

    public void CancleTimer()
    {
        CancelInvoke(nameof(CreateItem));
    }    
}
