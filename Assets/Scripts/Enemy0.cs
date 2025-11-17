//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Enemy0 : Enemy
//{
//    [Header("Attack settings")]
//    public Transform attackPoint;
//    public float attackRange = 1.0f;
//    public int attackDamage = 10;
//    public float attackCooldown = 1.2f;
//    public LayerMask playerLayer;

//    [Header("Optional checks")]
//    public bool requireLineOfSight = true;
//    public LayerMask obstacleLayer;

//    private Animator animator;
//    private float nextAttackTime = 0f;
//    private Transform detectedPlayer;
//    private float originalMoveSpeed;

//    void Awake()
//    {
//        animator = GetComponent<Animator>();
//        if (attackPoint == null) attackPoint = transform;
//        originalMoveSpeed = enemyMoveSpeed;
//    }

//    protected override void Update()
//    {
//        if (player == null) return;

//        // Kiểm tra player trong tầm attack
//        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

//        bool canAttack = false;

//        if (hit != null)
//        {
//            detectedPlayer = hit.transform;

//            if (requireLineOfSight)
//            {
//                Vector2 dir = (detectedPlayer.position - attackPoint.position).normalized;
//                float dist = Vector2.Distance(attackPoint.position, detectedPlayer.position);
//                RaycastHit2D ray = Physics2D.Raycast(attackPoint.position, dir, dist, obstacleLayer);
//                canAttack = (ray.collider == null);
//            }
//            else
//            {
//                canAttack = true;
//            }
//        }
//        else
//        {
//            detectedPlayer = null;
//        }

//        if (canAttack)
//        {
//            enemyMoveSpeed = 0f; // dừng di chuyển
//            if (Time.time >= nextAttackTime)
//            {
//                animator.SetTrigger("Attack");
//                nextAttackTime = Time.time + attackCooldown;
//            }
//        }
//        else
//        {
//            enemyMoveSpeed = originalMoveSpeed; // di chuyển về phía player
//        }

//        base.Update(); // di chuyển enemy nếu tốc độ > 0
//    }

//    public void DealDamageAnimationEvent()
//    {
//        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
//        foreach (var c in hits)
//        {
//            PlayerController pc = c.GetComponent<PlayerController>();
//            if (pc != null)
//            {
//                pc.TakeDamage(attackDamage);

//                Rigidbody2D rb = c.GetComponent<Rigidbody2D>();
//                if (rb != null)
//                {
//                    Vector2 knockDir = (c.transform.position - transform.position).normalized;
//                    rb.AddForce(knockDir * 100f);
//                }
//            }
//        }
//    }

//    void OnDrawGizmosSelected()
//    {
//        if (attackPoint == null) return;
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy0 : Enemy
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

   // private Animator animator;
    private float nextAttackTime = 0f;
    // private Transform detectedPlayer; // <-- ĐÃ XÓA. Chúng ta dùng 'targetPlayer' từ class cha
    private float originalMoveSpeed;

    void Awake()
    {
      //  animator = GetComponent<Animator>();
        if (attackPoint == null) attackPoint = transform;
        originalMoveSpeed = enemyMoveSpeed;
    }

    protected override void Update()
    {
        // 'player' và 'targetPlayer' được kế thừa từ class 'Enemy' (cha)
        // Chúng ta dựa vào code trong 'Enemy.Start()' để tìm chúng
        if (targetPlayer == null)
        {
            // Nếu không có mục tiêu (Player), gọi base.Update()
            // để nó có thể tiếp tục thử tìm Player (nếu bạn có logic đó)
            // hoặc đơn giản là gọi logic di chuyển (sẽ không làm gì nếu targetPlayer là null)
            base.Update();
            return;
        }

        // 1. Kiểm tra xem Player có *vật lý* trong tầm không
        Collider2D hit = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);

        bool canAttack = false;

        if (hit != null) // Nếu CÓ, thì kiểm tra tầm nhìn
        {
            // detectedPlayer = hit.transform; // <-- ĐÃ XÓA

            if (requireLineOfSight)
            {
                // --- PHẦN ĐÃ SỬA ---
                // Kiểm tra tầm nhìn bằng cách bắn Raycast đến 'targetPlayer' (giữa người)
                // thay vì 'hit.transform.position' (chân)
                Vector2 dir = ((Vector2)targetPlayer.position - (Vector2)attackPoint.position).normalized;
                float dist = Vector2.Distance(attackPoint.position, targetPlayer.position);
                RaycastHit2D ray = Physics2D.Raycast(attackPoint.position, dir, dist, obstacleLayer);

                // Nếu tia không trúng vật cản -> có thể tấn công
                canAttack = (ray.collider == null);
            }
            else
            {
                // Không cần tầm nhìn, chỉ cần trong tầm là đủ
                canAttack = true;
            }
        }
        // 'else' của hit != null đã được xóa vì không cần gán detectedPlayer = null nữa

        // 3. Hành động
        if (canAttack)
        {
            enemyMoveSpeed = 0f; // Dừng di chuyển
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("Attack");
                nextAttackTime = Time.time + attackCooldown;
            }
        }
        else
        {
            enemyMoveSpeed = originalMoveSpeed; // Tiếp tục di chuyển
        }

        // 4. Gọi base.Update()
        // Hàm này sẽ gọi MoveToPlayer() từ class cha (Enemy.cs)
        // và MoveToPlayer() đã được sửa để di chuyển về phía 'targetPlayer'
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

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}