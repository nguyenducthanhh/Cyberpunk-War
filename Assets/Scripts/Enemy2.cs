using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy
{
    [Header("Attack settings")]
    public Transform attackPoint;
    public float attackRange = 1.0f;
    public int attackDamage = 10;
    public float attackCooldown = 1.2f;
    public LayerMask playerLayer;

    [Header("Optional checks")]
    public bool requireLineOfSight = true;
    public LayerMask obstacleLayer;

    //private Animator animator;
    private float nextAttackTime = 0f;
    private Transform detectedPlayer;
    private float originalMoveSpeed;

    void Awake()
    {
       // animator = GetComponent<Animator>();
        if (attackPoint == null) attackPoint = transform;
        originalMoveSpeed = enemyMoveSpeed;
    }

    protected override void Update()
    {
        if (player == null) return;

        // Kiểm tra player trong tầm attack
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        bool canAttack = false;

        if (hit != null)
        {
            detectedPlayer = hit.transform;

            if (requireLineOfSight)
            {
                Vector2 dir = (detectedPlayer.position - attackPoint.position).normalized;
                float dist = Vector2.Distance(attackPoint.position, detectedPlayer.position);
                RaycastHit2D ray = Physics2D.Raycast(attackPoint.position, dir, dist, obstacleLayer);
                canAttack = (ray.collider == null);
            }
            else
            {
                canAttack = true;
            }
        }
        else
        {
            detectedPlayer = null;
        }

        if (canAttack)
        {
            enemyMoveSpeed = 0f; // dừng di chuyển
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            enemyMoveSpeed = originalMoveSpeed; // di chuyển về phía player
        }

        base.Update(); // di chuyển enemy nếu tốc độ > 0
    }

    public void DealDamageAnimationEvent()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (var c in hits)
        {
            PlayerController pc = c.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(attackDamage);

                Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 knockDir = (c.transform.position - transform.position).normalized;
                    rb.AddForce(knockDir * 100f);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
