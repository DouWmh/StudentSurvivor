using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe_Spawner : BaseWeapon
{
    [SerializeField] float cooldown;
    public bool onCooldown = false;

    
    void Start()
    {
        size = 1f;
        numToSpawn = 1;
        damageModifier = 1f;
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (!onCooldown)
            {
                for (int i = 1; i <= numToSpawn; i++)
                {
                    float randomAngle = Random.Range(0, 360f);
                    Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);
                    GameObject go = ObjectPooler.Instance.SpawnFromPool("Scythe", transform.position, rotation);
                    go.GetComponent<Scythe>().Damage *= base.damageModifier;
                }
                yield return new WaitForSeconds(cooldown);
                onCooldown = false;
            }
        }
    }
}
