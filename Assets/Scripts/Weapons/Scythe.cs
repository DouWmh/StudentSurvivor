using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float timerDestroy;

    public float Damage { get => damage; set => damage = value; }

    // Update is called once per frame
    void Update()
    {
        timerDestroy -= Time.deltaTime;
        if (timerDestroy <= 0)
            Destroy(gameObject);
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
}
