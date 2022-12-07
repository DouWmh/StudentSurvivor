using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] float speed;
    Player player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.PlayCrystalSound();
            player.AddExp();
            gameObject.SetActive(false);
            if (GameManager.Collected.ContainsKey("Crystal"))
            {
                GameManager.Collected["Crystal"]++;
            }
            else
            {
                GameManager.Collected.Add("Crystal", 1);
            }
        }
    }
    public void AttractToPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player)
            StartCoroutine(MoveToPlayer(player));
    }

    IEnumerator MoveToPlayer(Player player)
    {
        while (player)
        {
            Vector3 destination = player.transform.position;
            Vector3 source = gameObject.transform.position;
            Vector3 direction = destination - source;

            direction.Normalize();
            transform.position += speed * Time.deltaTime * direction;
            yield return null;
        }
    }
}
