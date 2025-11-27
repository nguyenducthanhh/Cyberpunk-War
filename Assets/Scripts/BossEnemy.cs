using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    [SerializeField] private Transform attackPoint;

    [SerializeField] private float meleeAttackRange = 2f;

    [SerializeField] private float rangedAttackRange = 10f;

    [SerializeField] private int meleeAttackDamage = 15;
    [SerializeField] private float meleeAttackCooldown = 2f;
    private float nextMeleeAttackTime = 0f;


    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float bulletCircleSpeed = 20f;
    [SerializeField] private float rangedAttackCooldown = 3f;

    [SerializeField] private float burstFireDelay = 0.1f;
    private float nextRangedAttackTime = 0f;


    [SerializeField] private LayerMask playerLayer;

    private float originalMoveSpeed;

    private bool isAttacking = false;

    protected override void Start()
    {
        base.Start();
        originalMoveSpeed = enemyMoveSpeed;

        if (attackPoint == null) attackPoint = transform;
        if (firePoint == null) firePoint = transform;
    }

    protected override void Update()
    {
        if (isDead || targetPlayer == null) return;

        if (isAttacking)
        {
            enemyMoveSpeed = 0f;

            FlipEnemy();
           
        }

        enemyMoveSpeed = originalMoveSpeed;

        bool inMeleeRange = Physics2D.OverlapCircle(attackPoint.position, meleeAttackRange, playerLayer);
        bool inRangedRange = Physics2D.OverlapCircle(attackPoint.position, rangedAttackRange, playerLayer);

        if (inMeleeRange && Time.time >= nextMeleeAttackTime )
        {
            PerformMeleeAttack();
        }

        else if (inRangedRange && Time.time >=  nextRangedAttackTime && Time.time >= nextMeleeAttackTime)
        {
            ChonSkillNgauNhien();
        }

        else if (!inRangedRange)
        {
            base.MoveToPlayer();
        }
        else if(inRangedRange && !isAttacking)
        {
           
            enemyMoveSpeed = originalMoveSpeed;
            base.MoveToPlayer();
            FlipEnemy();
        }
    }

    private void PerformMeleeAttack()
    {
       
        FlipEnemy();
        animator.SetTrigger("MeleeAttack");
        nextMeleeAttackTime = Time.time + meleeAttackCooldown;
    }

    private void PerformBurstAttack()
    {
        isAttacking = true; 
        FlipEnemy();
        animator.SetTrigger("RangedAttack");
        nextRangedAttackTime = Time.time + rangedAttackCooldown;
    }

    private void PerformCircleAttack()
    {
        isAttacking = true; 
        FlipEnemy();
        animator.SetTrigger("RangedCircleAttack");
        nextRangedAttackTime = Time.time + rangedAttackCooldown;
    }

    private void PerformTeleport()
    {
        isAttacking = true; 
        FlipEnemy();
        animator.SetTrigger("Teleport");
        nextRangedAttackTime = Time.time + rangedAttackCooldown;
    }

    private void ChonSkillNgauNhien()
    {
        int randomSkill = Random.Range(0, 4);
        switch (randomSkill)
        {
            case 1: PerformBurstAttack(); break;
            case 2: PerformTeleport(); break;
            case 3: PerformCircleAttack(); break;
        }
    }
    public void ShootBulletEvent()
    {
        StartCoroutine(BurstFireCoroutine());
    }

    private IEnumerator BurstFireCoroutine()
    {

        for (int i = 0; i < 10; i++)
        {
            if (targetPlayer == null || isDead) break;

            Vector3 directionToPlayer = (targetPlayer.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
                EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
                if (enemyBullet != null) enemyBullet.SetmovementDirection(directionToPlayer * bulletSpeed);
            }

            yield return new WaitForSeconds(burstFireDelay);
        }

        isAttacking = false;
    }
    public void ShootBulletCircleEvent()
    {
        StartCoroutine(CircleFireCoroutine());
    }

    IEnumerator CircleFireCoroutine()
    {
        const int bulletCount = 24;
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < 2; i++)
        {
            if (isDead) break;

            for (int j = 0; j < bulletCount; j++)
            {
                float angle = j * angleStep;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);

                if (bulletPrefab != null)
                {
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
                    if (enemyBullet) enemyBullet.SetmovementDirection(bulletDirection * bulletCircleSpeed);
                }
            }
            yield return new WaitForSeconds(1f);
        }

        isAttacking = false;
    }

    public void DealMeleeDamageEvent()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, meleeAttackRange, playerLayer);
        foreach (var c in hits)
        {
            PlayerController pc = c.GetComponent<PlayerController>();
            if (pc != null) pc.TakeDamage(meleeAttackDamage);
        }
    }

    public void TeleportEvent()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
        isAttacking = false;
    }
    public void AttackAnimationFinished()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, rangedAttackRange);
    }
    protected override void Die()
    {
        base.Die();
        gameManager.GameWinMenu();
    }
}
