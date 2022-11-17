using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecallMagnet : MonoBehaviour
{
    Player playerScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerScript = collision.GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.PlayMagnetSound();
            GameObject[] crystalsInGame = GameObject.FindGameObjectsWithTag("Crystal");
            if (crystalsInGame.Length > 0)
            {
                foreach (GameObject crystal in crystalsInGame)
                {
                    //Check Visibility
                    if (crystal.GetComponent<Renderer>().isVisible)
                    {
                        Crystal crystalScript = crystal.GetComponent<Crystal>();
                        crystalScript.AttractToPlayer();
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}
