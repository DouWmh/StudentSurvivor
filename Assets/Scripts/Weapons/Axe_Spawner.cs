using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Spawner : BaseWeapon
{
    [SerializeField] GameObject axePrefab;
    [SerializeField] float cooldown = 5f;
    public bool onCooldown = false;

    private void Start()
    {
        damageModifier = 1f;
        numToSpawn = 1;
        size = 1f;
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
                    onCooldown = true;
                    GameObject go = Instantiate(axePrefab, transform.position, Quaternion.identity);
                    go.GetComponent<Axe>().Damage *= base.damageModifier;
                    go.transform.localScale = Vector3.one * size;
                    yield return new WaitForSeconds(0.5f);
                }
                yield return new WaitForSeconds(cooldown);
                onCooldown = false;
            }
        }
    }
}
