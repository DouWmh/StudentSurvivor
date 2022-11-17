using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] float hpHealedRatio;
    Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.PlayFoodSound();
            player.OnHeal(hpHealedRatio);
            Destroy(gameObject);
        }
    }
}
