using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantKnife : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float damage;
    [SerializeField] float timerDestroy;
    Vector3 trajectory;
    GameObject player;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameManager.player;
        Vector3 destination = player.transform.position;
        Vector3 source = transform.position;
        trajectory = destination - source;
        float angle = Mathf.Atan2(trajectory.y, trajectory.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        trajectory.Normalize();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.position += trajectory * speed * Time.deltaTime;
        timerDestroy -= Time.deltaTime;
        if (timerDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            Destroy(gameObject);
            player.OnDamage(damage);
        }
    }
}
