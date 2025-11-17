using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : Enemy
{
    [Header("Attack settings")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 1.0f;
    [SerializeField] int attackDamage = 10;
    [SerializeField] float attackCooldown = 1.2f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] private GameObject BulletPrefabs;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float speedDan = 10f;
 

    [Header("Optional checks")]
    [SerializeField] bool requireLineOfSight = true;
    [SerializeField] LayerMask obstacleLayer;

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
    //private void shoot()
    //{
    //    if (player != null)
    //    {
    //        // Chúng ta lấy playerTarget từ class cha (Enemy.cs)
    //        // (Giả sử bạn đã sửa Enemy.cs để tự tìm 'playerTarget' như các câu trả lời trước)
    //        Vector3 targetPosition = targetPlayer.transform.position;

    //        Vector3 directionToPlayer = targetPosition - firePoint.transform.position;
    //        directionToPlayer.Normalize();

    //        // --- PHẦN SỬA ĐỔI BẮT ĐẦU TỪ ĐÂY ---

    //        // 1. Tính toán góc xoay (tính bằng radian, sau đó chuyển sang độ)
    //        // Atan2(y, x) sẽ cho chúng ta góc chính xác
    //        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

    //        // 2. Tạo một Quaternion (đại diện cho góc xoay) từ góc đó
    //        // Chúng ta xoay quanh trục Z (trục 2D)
    //        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

    //        // 3. Khi Instantiate, dùng 'rotation' thay vì 'Quaternion.identity'
    //        GameObject bullet = Instantiate(BulletPrefabs, firePoint.transform.position, rotation);

    //        // --- KẾT THÚC PHẦN SỬA ĐỔI ---

    //        EnemyBullet enemyBullet = bullet.AddComponent<EnemyBullet>();
    //        if (enemyBullet != null)
    //        {
    //            // Tốc độ đạn giờ đã bao gồm trong hướng
    //            enemyBullet.SetmovementDirection(directionToPlayer * speedDan);
    //        }
    //    }
    //}

    // Dùng trong Enemy3.cs và Enemy5.cs

    private void shoot()
    {
        // --- PHẦN THÊM VÀO ĐỂ "CHỮA CHÁY" ---
        if (targetPlayer == null)
        {
            // 1. Thử tìm mục tiêu bằng Tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

            if (playerObj != null)
            {
                // 2. Thử tìm object con "targetPlayer"
                Transform targetChild = playerObj.transform.Find("targetPlayer");

                if (targetChild != null)
                {
                    targetPlayer = targetChild; // Ngắm vào giữa
                }
                else
                {
                    targetPlayer = playerObj.transform; // Ngắm vào gốc
                }
            }

            // 3. Nếu vẫn không tìm thấy, thoát hàm shoot() để tránh lỗi
            if (targetPlayer == null)
            {
                Debug.LogError("Hàm shoot() không tìm thấy Player!");
                return; // Dừng lại, không bắn
            }
        }
        // --- KẾT THÚC PHẦN "CHỮA CHÁY" ---


        // Code bắn đạn CỦA BẠN (Dòng 84 hoặc 86)
        // Giờ thì 'targetPlayer' đã chắc chắn có giá trị
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
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
