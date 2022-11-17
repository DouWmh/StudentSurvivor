
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    [SerializeField] float damagePerSecond;
    [SerializeField] float speed;
    [SerializeField] float timerDestroy;
    CircleCollider2D collide;

    public float DamagePerSecond { get => damagePerSecond; set => damagePerSecond = value; }

    void Start()
    {
        collide = GetComponent<CircleCollider2D>();
        StartCoroutine(EnergyDamage());
    }
    IEnumerator EnergyDamage()
    {
        while (timerDestroy > 0)
        {
            collide.enabled = true;
            yield return new WaitForSeconds(1f);
            collide.enabled = false;
        }
    }
    void Update()
    {
        timerDestroy -= Time.deltaTime;
        if (timerDestroy <= 0)
            Destroy(gameObject);
        float inflateX = transform.localScale.x + speed * Time.deltaTime;
        float inflateY = transform.localScale.y + speed * Time.deltaTime;
        transform.localScale = new Vector3(inflateX, inflateY, transform.localScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damagePerSecond);
        }
    }
}
