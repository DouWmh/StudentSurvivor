using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giant : Enemy
{
    enum GiantState
    {
        Idle = 0,
        Chasing = 1,
        Attack = 2,
        Berserk = 3
    }
    [SerializeField] GameObject knifePrefab;
    [SerializeField] Vector3 knifeOffset;

    private Animator animator;
    GiantState giantState = GiantState.Idle;
    float waitTimer = 2f;

    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    protected override void Start()
    {

        base.Start();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.isInvulnerable = false;
        switch (giantState)
        {
            case GiantState.Idle:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0)
                {
                    giantState = GiantState.Chasing;
                }
                break;
            case GiantState.Chasing:
                float distance = Vector3.Distance(transform.position, player.transform.position);
                base.Update();

                if (distance > 5f)
                {
                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    animator.SetBool("IsWalking", false);
                    giantState = GiantState.Attack;
                }
                break;
            case GiantState.Attack:
                animator.SetTrigger("Attack");
                giantState = GiantState.Idle;
                waitTimer = 3f;
                break;
            case GiantState.Berserk:
                break;
            default:
                break;
        }
    }
    public override void TakeDamage(float damage)
    {
        if (giantState != GiantState.Berserk)
        {
            animator.Play("GiantIdle");
            giantState = GiantState.Idle;
            waitTimer = 2f;
        }
        base.TakeDamage(damage);
        if (base.GetHPRatio() <= 0.5f && giantState != GiantState.Berserk)
        {
            giantState = GiantState.Berserk;
            StartCoroutine(Berserk());
        }
    }

    IEnumerator Berserk()
    {
        while (true)
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(2f);
        }
    }

    public void SpawnKnife(int number)
    {

        Vector3 knifeSpawnPoint = new Vector3(transform.position.x - (transform.localScale.x * knifeOffset.x),
                                                transform.position.y + knifeOffset.y,
                                                transform.position.z + knifeOffset.z);
        GameObject go = Instantiate(knifePrefab, knifeSpawnPoint, Quaternion.identity);
    }
}
