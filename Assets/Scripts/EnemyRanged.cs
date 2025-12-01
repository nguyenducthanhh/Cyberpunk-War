using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{

    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 1.0f;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackCooldown = 1.2f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] private GameObject BulletPrefabs;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float speedDan = 10f;
    [SerializeField] private float dieAnimationTime = 1f;
    [SerializeField] bool requireLineOfSight = true;
    [SerializeField] LayerMask obstacleLayer;

    private float nextAttackTime = 0f;
    private Transform detectedPlayer;
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
        if (player == null) return;

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
    private void shoot()
    {
        if (targetPlayer == null)
        {
          
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
              
                Transform targetChild = playerObj.transform.Find("targetPlayer");

                if (targetChild != null)
                {
                    targetPlayer = targetChild;
                }
                else
                {
                    targetPlayer = playerObj.transform;
                }
            }

            if (targetPlayer == null)
            {
                Debug.LogError("Hàm shoot() không tìm thấy Player!");
                return;
            }
        }

        Vector3 directionToPlayer = targetPlayer.position - firePoint.transform.position;
        directionToPlayer.Normalize();

        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        GameObject bullet = Instantiate(BulletPrefabs, firePoint.transform.position, rotation);

        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
        {
            enemyBullet.SetmovementDirection(directionToPlayer * speedDan);
        }
    }

    protected void DropLoot()
    {
        float randomValue = Random.Range(0f, 100f);
        if (coinPrefab != null && randomValue <= coinDropRate)
        {
            GameObject money = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Destroy(money, 5f);
            return;
        }
        randomValue = Random.Range(0f, 100f);

        if (healthPotionPrefab != null && randomValue <= healthPotionDropRate)
        {
            GameObject medicine = Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
            Destroy(medicine, 5f);
            return;
        }
    }

    public override void Die()
    {
        DropLoot();
        base.Die();
        Destroy(gameObject, dieAnimationTime);
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

   
}
