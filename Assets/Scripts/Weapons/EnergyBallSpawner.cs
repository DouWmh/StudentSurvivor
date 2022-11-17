using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBallSpawner : BaseWeapon
{
    [SerializeField] GameObject energyPrefab;
    [SerializeField] float cooldown;
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
                for (int i = 1; i <= numToSpawn; i++)                {

                    onCooldown = true;
                    Vector3 spawnPosition;
                    spawnPosition = UnityEngine.Random.insideUnitCircle;
                    spawnPosition += transform.position;
                    GameObject go = Instantiate(energyPrefab, spawnPosition, Quaternion.identity);
                    go.GetComponent<EnergyBall>().DamagePerSecond *= damageModifier;
                    go.transform.localScale = Vector3.one * size;
                    yield return new WaitForSeconds(0.2f);
                }
                yield return new WaitForSeconds(cooldown);
                onCooldown = false;
            }
        }
    }
}
