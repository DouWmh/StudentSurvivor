using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : BaseWeapon
{
    [SerializeField] float damage;
    [SerializeField] float timeOn;
    [SerializeField] float timeOff;    

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider2D;

    public float Damage { get => damage; set => damage = value; }

    private void Start()
    {
        size = 1f;
        damageModifier = 1f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        StartCoroutine(KatantaCoroutine());
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage * damageModifier);
        }        
    }

    IEnumerator KatantaCoroutine()
    {
        while (true)
        {
            transform.localScale = Vector3.one * size;
            spriteRenderer.enabled = true;
            boxCollider2D.enabled = true;

            yield return new WaitForSeconds(timeOn);
            spriteRenderer.enabled = false;
            boxCollider2D.enabled = false;
            yield return new WaitForSeconds(timeOff);
        }

    }
}
