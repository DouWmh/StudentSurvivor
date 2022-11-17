using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField] float rotationSpd;
    [SerializeField] float rotationTimer;
    float playerScaleX;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider2D;

    public float Damage { get => damage; set => damage = value; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
    public void Start()
    {
        playerScaleX = GameObject.FindGameObjectWithTag("Player").transform.localScale.x;
        if (gameObject != null)
        {
            StartCoroutine(RotateAxe());
        }
    }
    IEnumerator RotateAxe()
    {
        float timer = rotationTimer;
        rotationSpd = (playerScaleX > 0) ? rotationSpd : -rotationSpd;
        Quaternion initialState = transform.rotation;
        while (timer > 0)
        {
            transform.Rotate(new Vector3(0, 0, 1f), rotationSpd * Time.deltaTime);
            timer -= Time.deltaTime;
            yield return null;
        }
        transform.rotation = initialState;
        Destroy(gameObject);
        yield return null;
    }
    void Update()
    {

    }
}
