using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DmgReceived : MonoBehaviour
{
    [SerializeField] TMP_Text dmg;
    [SerializeField] float speed;
    [SerializeField] Vector3 offset;

    public void DisplayDmg(float damage, GameObject enemyTakingDmg)
    {
        transform.position += offset;
        StartCoroutine(DisplayDamage(damage, enemyTakingDmg));
    }
    IEnumerator DisplayDamage(float dmgTaken, GameObject enemyTakingDmg)
    {
        dmg.text = $"{dmgTaken}";
        while (dmg.fontSize < 1)
        {
            transform.localScale = enemyTakingDmg.transform.localScale;
            dmg.fontSize += speed * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        while (dmg.fontSize > 0.2)
        {
            transform.localScale = enemyTakingDmg.transform.localScale;
            dmg.fontSize -= speed * Time.deltaTime;
            yield return null;
        }
        dmg.text = "";
        Destroy(gameObject);
    }
    
}
