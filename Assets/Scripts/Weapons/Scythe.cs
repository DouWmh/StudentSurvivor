using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour, IPooledObject
{
    [SerializeField] float damage;
    [SerializeField] float timerDestroy;

    public float Damage { get => damage; set => damage = value; }


    // Update is called once per frame
    void Update()
    {
        transform.position += 5 * Time.deltaTime * transform.right;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    public void OnObjectSpawn()
    {
        gameObject.SetActive(true);
    }
}
