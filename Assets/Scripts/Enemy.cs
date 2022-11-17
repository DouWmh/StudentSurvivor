using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum Direction
    {
        Left, Right, Up, Down, Chase
    }
    float scaleX;
    [Header("Stats")]
    [SerializeField] float damage;
    [SerializeField] float speed;
    [SerializeField] float maxHP;
    protected float currentHP;

    [Header("UI")]
    [SerializeField] HealthBar enemyHpBar;
    [SerializeField] GameObject dmgText;
    DmgReceived dmgReceivedScript;
    [SerializeField] Canvas canvas;

    [Header("Rewards")]
    [SerializeField] GameObject coin;
    [SerializeField] GameObject crystal;
    [SerializeField] GameObject magnet;
    [SerializeField] Food[] foods;

    public bool isChasing = true;
    public Direction movementDirection = Direction.Chase;
    protected GameObject player;
    Player playerScript;
    SpriteRenderer spriteRenderer;
    Material fadeMat;
    [SerializeField] float zoomSpeed;
    [SerializeField] float zoomMax;
    [SerializeField] float zoomMin;
    [SerializeField] int zoomNumber;

    public bool isInvulnerable;
    public bool isBoss;
    public bool isEnraged;
    public bool isDOT;


    protected virtual void Awake()
    {
        currentHP = maxHP;
        enemyHpBar.SetMaxHealth(maxHP);
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerScript = player.GetComponent<Player>();
    }
    protected virtual void Start()
    {
        if (isBoss)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            fadeMat = spriteRenderer.material;
            StartCoroutine(BossEncounter());
            fadeMat.SetFloat("_Fader", GetHPRatio());
        }
    }

    IEnumerator BossEncounter()
    {
        PlayerCamera camScript = Camera.main.GetComponent<PlayerCamera>();
        Time.timeScale = 0;
        camScript.target = gameObject;
        for (int i = 0; i < zoomNumber; i++)
        {
            while (Camera.main.orthographicSize > zoomMin)
            {
                Camera.main.orthographicSize -= Time.unscaledDeltaTime * zoomSpeed;
                yield return null;
            }
            while (Camera.main.orthographicSize < zoomMax)
            {
                Camera.main.orthographicSize += Time.unscaledDeltaTime * zoomSpeed;
                yield return null;
            }
        }
        while (Camera.main.orthographicSize > 5f)
        {
            Camera.main.orthographicSize -= Time.unscaledDeltaTime * zoomSpeed;
            yield return null;
        }
        camScript.target = player;
        Time.timeScale = 1f;
    }
    protected virtual void Update()
    {
        isInvulnerable = false;
        Movement();
    }
    //maybe tag the weapon on enemy
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (playerScript != null)
            {
                if (isBoss && !isDOT)
                {
                    isDOT = true;
                    StartCoroutine(BossDOT(playerScript));
                }
                else if (!isBoss)
                {
                    playerScript.OnDamage(damage);
                    Destroy(enemyHpBar.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            if (isBoss)
            {
                isDOT = false;
                StopCoroutine(BossDOT(playerScript));
            }
        }
    }
    IEnumerator BossDOT(Player playerScript)
    {
        while (isDOT)
        {
            playerScript.OnDamage(damage);
            yield return new WaitForSeconds(1f);
        }
    }
    public float GetHPRatio()
    {
        return currentHP / maxHP;
    }
    public virtual void TakeDamage(float damage)
    {
        if (!enemyHpBar.gameObject.activeSelf)
            enemyHpBar.gameObject.SetActive(true);
        if (!isInvulnerable)
        {
            isInvulnerable = true;
            damage *= player.GetComponent<Player>().damageMultiplier;
            damage = Mathf.Floor(damage);
            SpawnDamageText(damage);
            currentHP -= damage;
            enemyHpBar.SetHealthDown(currentHP);
            if (currentHP <= 0)
                Dies();
            if (isBoss && !isEnraged && currentHP < maxHP / 2)
                Enrage();
            if (isBoss)
            {
                fadeMat.SetFloat("_Fader", GetHPRatio());
            }
        }
    }

    private void SpawnDamageText(float damage)
    {
        GameObject go = Instantiate(dmgText, transform.position, Quaternion.identity, canvas.transform);
        go.transform.localScale = transform.localScale;
        dmgReceivedScript = go.GetComponent<DmgReceived>();
        dmgReceivedScript.DisplayDmg(damage, gameObject);
    }

    private void Enrage()
    {
        isEnraged = true;
        speed *= 2f;
        damage *= 1.75f;
    }

    public void Dies()
    {
        playerScript.PlayRandomDeathSound();
        Destroy(enemyHpBar.gameObject);
        Destroy(gameObject);
        //Rewards Weight Ratio
        //
        int randomReward = Random.Range(0, 1000);

        if (randomReward < 50)
        {
            int randomFoodIndex = Random.Range(0, foods.Length);
            Instantiate(foods[randomFoodIndex], transform.position, Quaternion.identity);
        }
        else if (randomReward < 60)
            Instantiate(magnet, transform.position, Quaternion.identity);
        else if (randomReward < 90)
            Instantiate(coin, transform.position, Quaternion.identity);
        else if (randomReward < 1000)
            Instantiate(crystal, transform.position, Quaternion.identity);
        else
            Instantiate(crystal, transform.position, Quaternion.identity);
    }
    public void Movement()
    {
        if (player == null)
            return;
        //Set the trajectory vectors
        Vector3 destination = player.transform.position;
        Vector3 source = gameObject.transform.position;
        Vector3 direction = destination - source;

        //Set the 'not chasing' direction
        if (movementDirection != Direction.Chase)
        {
            switch (movementDirection)
            {
                case Direction.Left:
                    direction = Vector3.left;
                    break;
                case Direction.Right:
                    direction = Vector3.right;
                    break;
                case Direction.Up:
                    direction = Vector3.up;
                    break;
                case Direction.Down:
                    direction = Vector3.down;
                    break;
                default:
                    break;
            }
        }
        //Actual movement performed
        direction.Normalize();
        transform.position += speed * Time.deltaTime * direction;

        //Change horizontal face
        scaleX = (direction.x < 0) ? 1f : -1f;
        transform.localScale = new Vector3(scaleX, 1, 1);
    }
    public void SetupHealthBar(Canvas canvas)
    {
        enemyHpBar.transform.SetParent(canvas.transform);
    }

}