using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEnum.Enums;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public Collider2D area;
    public Transform body;
    public float moveScale = 0.5f;

    [Header("Rush Setting")]
    public float rushInterval = 2.0f; // ��̼��
    public float rushTime = 0.5f;   // �ɳ��ʱ��

    private int timer = 0;
    private Vector3 direction;
    private Vector2 minPoint, maxPoint;
    private List<Transform> bodys = new();
    private float lastRushTime = 0.0f;     // ����̼��
    private bool showRushTip = false;
    private readonly Vector3[] dirs = { Vector3.up, Vector3.down, Vector3.right, Vector3.left };

    private void Awake()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }

    private void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void Init()
    {
        direction = dirs[Random.Range(0, 4)]; // ��������˶�����
        minPoint = new Vector2(area.bounds.min.x, area.bounds.min.y);
        maxPoint = new Vector2(area.bounds.max.x, area.bounds.max.y);
        float newX = Random.Range(minPoint.x, maxPoint.x);
        float newY = Random.Range(minPoint.y, maxPoint.y);
        newX = Mathf.Round(newX * 10.0f) / 10;
        newY = Mathf.Round(newY * 10.0f) / 10;
        transform.position = new (newX, newY, 0);    // �������λ��

        for (int i = 1; i  < bodys.Count; i++)  // �Ƴ����ݣ����׶����ܱ�����
        {
            Destroy(bodys[i].gameObject);
        }
        bodys.Clear();
        bodys.Add(transform);
    }

    // �������
    private void CheckInput()
    {
        // �ж������˶�����
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && direction != Vector3.down) 
        { 
            direction = Vector3.up;
            UIManager.Instance.ShowActionTip(ActionType.KeyUp);
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && direction != Vector3.up) 
        { 
            direction = Vector3.down;
            UIManager.Instance.ShowActionTip(ActionType.KeyDown);
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && direction != Vector3.left) 
        { 
            direction = Vector3.right;
            UIManager.Instance.ShowActionTip(ActionType.KeyRight);
        }
        else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && direction != Vector3.right) 
        { 
            direction = Vector3.left;
            UIManager.Instance.ShowActionTip(ActionType.KeyLeft);
        }

        // �ж��Ƿ��� ������������3�ſ��Գ��
        if (Input.GetKeyDown(KeyCode.Space) && bodys.Count >= 3)
        {
            UIManager.Instance.ShowActionTip(ActionType.Rush);
            if (Time.time - lastRushTime > rushInterval)
            {
                lastRushTime = Time.time;
                GameManager.instance.SetGameSpeed(3);
            }
            if (showRushTip)
            {
                showRushTip = false;
                UIManager.Instance.ShowRushTip(false);
            }
        }
        if (Time.time - lastRushTime > rushTime)
        {
            GameManager.instance.ResetGameSpeed();
        }
    }
    
    // ����ƶ�λ��
    private void Movement()
    {
        if (timer > 0 && timer % GameManager.instance.GetGameSpeed() == 0)
        {
            timer = 0;
            for (int i = bodys.Count - 1; i > 0; i--)
            {
                bodys[i].position = bodys[i - 1].position;
                if (!bodys[i].gameObject.activeSelf) bodys[i].gameObject.SetActive(true);
            }

            transform.Translate(direction * moveScale);

            // ��Ե��飬��Ե��ײǽ
            if (transform.position.x - minPoint.x < -0.11f) { transform.position = new Vector3(maxPoint.x, transform.position.y); }
            else if (transform.position.x - maxPoint.x > 0.11f) { transform.position = new Vector3(minPoint.x, transform.position.y); }
            else if (transform.position.y - minPoint.y < -0.11f) { transform.position = new Vector3(transform.position.x, maxPoint.y); }
            else if (transform.position.y - maxPoint.y > 0.11f) { transform.position = new Vector3(transform.position.x, minPoint.y); }
        }
        timer = timer + 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food")) // �Ե�ʳ����ӳ�����
        {
            Transform newBody = Instantiate(body, transform.position, Quaternion.identity);
            bodys.Add(newBody);
            newBody.gameObject.SetActive(false);
            if (bodys.Count == 3)
            {
                showRushTip = true;
                UIManager.Instance.ShowRushTip(true);
            }
        }

        if (collision.CompareTag("Obstacle")) // ײ���ϰ�����壩
        {
            GameManager.instance.EndGame();
        }
    }

    public List<Transform> GetBodys()
    {
        return bodys;
    }
}
 