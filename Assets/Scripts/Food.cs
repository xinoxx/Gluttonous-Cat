using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FoodController.instance.RemoveFood(gameObject, true);
        }
    }

    // ¶¯»­½áÊøÊ±ÒÆ³ý
    public void DeleteFood()
    {
        FoodController.instance.RemoveFood(gameObject, false);
        Destroy(gameObject);
    }    
}
