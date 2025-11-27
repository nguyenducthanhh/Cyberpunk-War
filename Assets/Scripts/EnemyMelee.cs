using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : Enemy
{
   
    public Transform attackPoint;
    public float attackRange = 1.0f;
    public int attackDamage = 10;
    public float attackCooldown = 1.2f;
    public LayerMask playerLayer;

    
    public bool requireLineOfSight = true;
    public LayerMask obstacleLayer;

   
    private float nextAttackTime = 0f;
 
    private float originalMoveSpeed;

  
    [SerializeField] private GameObject coinPrefab;
    [Range(0, 100)][SerializeField] private float coinDropRate = 50f;

    
    [SerializeField] private GameObject healthPotionPrefab;
    [Range(0, 100)][SerializeField] private float healthPotionDropRate = 20f;
    void Awake()
    {
        if (attackPoint == null) attackPoint = transform;
        originalMoveSpeed = enemyMoveSpeed;
    }

    protected override void Update()
    {
        if (targetPlayer == null)
        {
            base.Update();
            return;
        }

        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        bool canAttack = false;

        if (hit != null)
        {
            if (requireLineOfSight)
            {
                Vector2 dir = ((Vector2)targetPlayer.position - (Vector2)attackPoint.position).normalized;
                float dist = Vector2.Distance(attackPoint.position, targetPlayer.position);
                RaycastHit2D ray = Physics2D.Raycast(attackPoint.position, dir, dist, obstacleLayer);
                canAttack = (ray.collider == null);
            }
            else
            {
                canAttack = true;
            }
        }
        if (canAttack)
        {
            enemyMoveSpeed = 0f;
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            enemyMoveSpeed = originalMoveSpeed;
        }
        base.Update();
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


    protected void DropLoot()
    {
        float randomValue = Random.Range(0f, 100f);
        if (coinPrefab != null && randomValue <= coinDropRate)
        {
            GameObject money = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Destroy(money,5f);         
        }
        else if (healthPotionPrefab != null && randomValue <= (healthPotionDropRate + coinDropRate))
        {
          GameObject medicine =  Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
            Destroy(medicine, 5f);
        }
    }

    protected override void Die()
    {
        DropLoot();
        base.Die();
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}