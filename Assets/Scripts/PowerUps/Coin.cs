using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Player playerScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerScript = collision.GetComponent<Player>();
        if (playerScript != null)
        {
            TitleManager.saveData.goldCoins++;
            playerScript.PlayCoinSound();
            Destroy(gameObject);
            if (GameManager.Collected.ContainsKey("Coin"))
            {
                GameManager.Collected["Coin"]++;
            }
            else
            {
                GameManager.Collected.Add("Coin", 1);
            }
        }
    }
}
