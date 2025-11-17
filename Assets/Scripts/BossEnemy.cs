//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static UnityEngine.Rendering.DebugUI;

//public class BossEnemy : Enemy // Kế thừa từ class 'Enemy' của bạn
//{
//    [Header("Boss Attack Settings")]
//    [Tooltip("Điểm trung tâm để kiểm tra tầm tấn công.")]
//    [SerializeField] private Transform attackPoint; // <-- TÂM ĐỂ KIỂM TRA

//    [Tooltip("Tầm đánh gần (Melee). Ưu tiên cao nhất.")]
//    [SerializeField] private float meleeAttackRange = 2f;
//    [Tooltip("Tầm bắn xa (Ranged). Sẽ dùng nếu ngoài tầm melee.")]
//    [SerializeField] private float rangedAttackRange = 10f;

//    [Header("Melee Attack (Đánh gần)")]
//    [SerializeField] private int meleeAttackDamage = 15;
//    [SerializeField] private float meleeAttackCooldown = 1.5f;
//    private float nextMeleeAttackTime = 0f;

//    [Header("Ranged Attack (Bắn xa)")]
//    [SerializeField] private GameObject bulletPrefab;
//    [SerializeField] private Transform firePoint;
//    [SerializeField] private float bulletSpeed = 10f;
//    [SerializeField] private float rangedAttackCooldown = 3f;
//    private float nextRangedAttackTime = 0f;

//    [Header("Required Components")]
//    [SerializeField] private LayerMask playerLayer; // Cần để kiểm tra tầm

//    private float originalMoveSpeed;

//    // Biến 'animator' và 'targetPlayer' đã được kế thừa từ class cha 'Enemy'

//    // --- Khởi tạo ---
//    protected override void Start()
//    {
//        base.Start();
//        originalMoveSpeed = enemyMoveSpeed;

//        // Gán giá trị dự phòng
//        if (attackPoint == null) attackPoint = transform; // <-- QUAN TRỌNG
//        if (firePoint == null) firePoint = transform;
//    }

//    // --- TRÍ TUỆ NHÂN TẠO CỦA BOSS (ĐÃ SỬA) ---
//    protected override void Update()
//    {
//        if (isDead || targetPlayer == null)
//        {
//            return;
//        }

//        // --- PHẦN ĐÃ SỬA ---
//        // 1. Kiểm tra tầm bằng OverlapCircle thay vì Distance
//        // Dùng 'attackPoint.position' làm tâm
//        bool inMeleeRange = Physics2D.OverlapCircle(attackPoint.position, meleeAttackRange, playerLayer);
//        bool inRangedRange = Physics2D.OverlapCircle(attackPoint.position, rangedAttackRange, playerLayer);
//        // --- KẾT THÚC SỬA ---


//        // --- CÂY QUYẾT ĐỊNH CỦA AI (logic giữ nguyên) ---

//        // 1. ƯU TIÊN 1: TẤN CÔNG GẦN (MELEE)
//        // Nếu Player ở trong tầm đánh gần VÀ đã hết cooldown
//        if (inMeleeRange && Time.time >= nextMeleeAttackTime)
//        {
//            PerformMeleeAttack();
//        }

//        // 2. ƯU TIÊN 2: BẮN XA (RANGED)
//        // Nếu Player ở ngoài tầm gần (do 'else if') NHƯNG trong tầm bắn VÀ đã hết cooldown
//        else if (inRangedRange && Time.time >= nextRangedAttackTime)
//        {
//            ChonSkillNgauNhien();
//        }

//        // 3. ƯU TIÊN 3: ĐUỔI THEO (CHASE)
//        // Nếu Player ở ngoài mọi tầm tấn công (inRangedRange = false)
//        else if (!inRangedRange)
//        {
//            enemyMoveSpeed = originalMoveSpeed;
//            base.MoveToPlayer();
//        }
//        else
//        {
//            // Đứng yên chờ cooldown
//            enemyMoveSpeed = 0f;
//            FlipEnemy();
//        }
//    }

//    // --- CÁC HÀNH ĐỘNG TẤN CÔNG ---
//    // (Không thay đổi)

//    private void PerformMeleeAttack()
//    {
//        enemyMoveSpeed = 0f;
//        FlipEnemy();
//        animator.SetTrigger("MeleeAttack");
//        nextMeleeAttackTime = Time.time + meleeAttackCooldown;
//    }

//    private void PerformRangedAttack()
//    {
//        enemyMoveSpeed = 0f;
//        FlipEnemy();
//        animator.SetTrigger("RangedAttack");
//        nextRangedAttackTime = Time.time + rangedAttackCooldown;
//    }

//    private void PerformTeleport()
//    {
//        enemyMoveSpeed = 0f;
//        FlipEnemy();
//        animator.SetTrigger("Teleport");
//        nextRangedAttackTime = Time.time + rangedAttackCooldown;
//    }

//    // --- CÁC HÀM GỌI BẰNG ANIMATION EVENT ---
//    // (Không thay đổi)

//    public void DealMeleeDamageEvent()
//    {
//        // Sửa lại: Dùng OverlapCircle tại 'attackPoint'
//        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, meleeAttackRange, playerLayer);
//        foreach (var c in hits)
//        {
//            PlayerController pc = c.GetComponent<PlayerController>();
//            if (pc != null)
//            {
//                pc.TakeDamage(meleeAttackDamage);
//            }
//        }
//    }

//    private void Teleport()
//    {
//        if (player != null)
//        {
//            transform.position = player.transform.position;
//        }
//    }

//    public void ShootBulletEvent1()
//    {
//        if (targetPlayer == null || firePoint == null || bulletPrefab == null) return;
//        Vector3 directionToPlayer = (targetPlayer.position - firePoint.position).normalized;
//        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
//        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
//        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

//        EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
//        if (enemyBullet != null)
//        {
//            enemyBullet.SetmovementDirection(directionToPlayer * bulletSpeed);
//        }
//    }

//    public void ShootBulletEvent2()
//    {
//        for (int i = 0; i < 20; i++)
//        {
//            if (targetPlayer == null || firePoint == null || bulletPrefab == null) return;
//            Vector3 directionToPlayer = (targetPlayer.position - firePoint.position).normalized;
//            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
//            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
//            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

//            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
//            if (enemyBullet != null)
//            {
//                enemyBullet.SetmovementDirection(directionToPlayer * bulletSpeed);
//            }
//        }
//    }

//    private void ChonSkillNgauNhien()
//    {
//        int randomSkill = Random.Range(0, 6);
//        switch (randomSkill)
//        {
//            case 0:
//                PerformTeleport();
//                break;
//            case 2:
//                PerformMeleeAttack();
//                break;


//        }

//    }


//    // --- VẼ GIZMOS ĐỂ DỄ DEBUG ---
//    private void OnDrawGizmosSelected()
//    {
//        if (attackPoint == null) return; // Dùng attackPoint làm tâm

//        // Vẽ tầm đánh gần (Màu đỏ)
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRange);

//        // Vẽ tầm bắn xa (Màu xanh)
//        Gizmos.color = Color.blue;
//        Gizmos.DrawWireSphere(attackPoint.position, rangedAttackRange);
//    }
//}

// Thêm 2 dòng 'using' này để Coroutine (IEnumerator) hoạt động
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Kế thừa từ class 'Enemy' (cha) mà bạn đã cung cấp
public class BossEnemy : Enemy
{
    [Header("Boss Attack Settings")]
    [Tooltip("Điểm trung tâm để kiểm tra tầm tấn công.")]
    [SerializeField] private Transform attackPoint; // Tâm để kiểm tra

    [Tooltip("Tầm đánh gần (Melee). Ưu tiên cao nhất.")]
    [SerializeField] private float meleeAttackRange = 2f;
    [Tooltip("Tầm bắn xa (Ranged). Sẽ dùng nếu ngoài tầm melee.")]
    [SerializeField] private float rangedAttackRange = 10f;

    [Header("Melee Attack (Đánh gần)")]
    [SerializeField] private int meleeAttackDamage = 15;
    [SerializeField] private float meleeAttackCooldown = 1.5f;
    private float nextMeleeAttackTime = 0f;

    [Header("Ranged Attack (Bắn xa)")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 30f;
    [SerializeField] private float bulletCircleSpeed = 20f;
    [SerializeField] private float rangedAttackCooldown = 3f;
    [Tooltip("Độ trễ (giây) giữa mỗi viên đạn khi 'sấy'")]
    [SerializeField] private float burstFireDelay = 0.1f;
    private float nextRangedAttackTime = 0f;

    [Header("Required Components")]
    [SerializeField] private LayerMask playerLayer; // Cần để kiểm tra tầm

    private float originalMoveSpeed;

    // Biến 'animator', 'targetPlayer', và 'isDead' đã được kế thừa từ class cha 'Enemy'

    // --- Khởi tạo ---
    protected override void Start()
    {
        // RẤT QUAN TRỌNG: Gọi Start() của class cha (Enemy)
        // để nó tự động tìm 'player', 'targetPlayer', và 'animator'
        base.Start();

        originalMoveSpeed = enemyMoveSpeed;

        // Gán giá trị dự phòng nếu quên gán trong Inspector
        if (attackPoint == null) attackPoint = transform;
        if (firePoint == null) firePoint = transform;
    }

    // --- TRÍ TUỆ NHÂN TẠO CỦA BOSS ---
    protected override void Update()
    {
        // Nếu đã chết hoặc không có mục tiêu, không làm gì cả
        if (isDead || targetPlayer == null)
        {
            return;
        }

        // 1. Kiểm tra tầm bằng OverlapCircle
        bool inMeleeRange = Physics2D.OverlapCircle(attackPoint.position, meleeAttackRange, playerLayer);
        bool inRangedRange = Physics2D.OverlapCircle(attackPoint.position, rangedAttackRange, playerLayer);

        // --- CÂY QUYẾT ĐỊNH CỦA AI ---

        // 1. ƯU TIÊN 1: TẤN CÔNG GẦN (MELEE)
        if (inMeleeRange && Time.time >= nextMeleeAttackTime)
        {
            PerformMeleeAttack();
        }

        // 2. ƯU TIÊN 2: TẤN CÔNG TẦM XA (CHỌN NGẪU NHIÊN)
        else if (inRangedRange && Time.time >= nextRangedAttackTime)
        {
            // Nếu ở tầm xa và hết cooldown, chọn 1 trong 3 skill
            ChonSkillNgauNhien();
        }

        // 3. ƯU TIÊN 3: ĐUỔI THEO (CHASE)
        else if (!inRangedRange)
        {
            enemyMoveSpeed = originalMoveSpeed;
            base.MoveToPlayer(); // Gọi hàm di chuyển của class cha
        }

        // 4. ĐỨNG YÊN (chờ cooldown)
        else
        {
            enemyMoveSpeed = 0f;
            FlipEnemy(); // Vẫn xoay mặt về phía Player
        }
    }

    // --- CÁC HÀNH ĐỘNG TẤN CÔNG (Perform) ---
    // Các hàm này kích hoạt Animation và đặt Cooldown

    private void PerformMeleeAttack()
    {
        enemyMoveSpeed = 0f;
        FlipEnemy();
        animator.SetTrigger("MeleeAttack"); // Trigger cho Animator
        nextMeleeAttackTime = Time.time + meleeAttackCooldown;
    }

    private void PerformBurstAttack()
    {
        enemyMoveSpeed = 0f;
        FlipEnemy();
        animator.SetTrigger("RangedAttack"); // Trigger "Sấy đạn"
        nextRangedAttackTime = Time.time + rangedAttackCooldown;
    }

    private void PerformCircleAttack()
    {
        enemyMoveSpeed = 0f;
        FlipEnemy();
        animator.SetTrigger("RangedCircleAttack"); 
        nextRangedAttackTime = Time.time + rangedAttackCooldown;
    }

    private void PerformTeleport()
    {
        enemyMoveSpeed = 0f;
        FlipEnemy();
        animator.SetTrigger("Teleport"); // Trigger "Dịch chuyển"
        nextRangedAttackTime = Time.time + rangedAttackCooldown;
    }

    private void ChonSkillNgauNhien()
    {
        // Chọn ngẫu nhiên 1 số (0, 1, hoặc 2)
        int randomSkill = Random.Range(0, 4);

        switch (randomSkill)
        {
            case 0:
                PerformBurstAttack(); // Sấy đạn
                break;
            case 2:
                PerformTeleport(); // Dịch chuyển
                break;
            case 3:
                PerformCircleAttack();
                break;
        }
    }


    // --- CÁC HÀM GỌI BẰNG ANIMATION EVENT ---
    // Đây là các hàm bạn phải gán vào Animation Clip

    // Gán vào clip "MeleeAttack"
    public void DealMeleeDamageEvent()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, meleeAttackRange, playerLayer);
        foreach (var c in hits)
        {
            PlayerController pc = c.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.TakeDamage(meleeAttackDamage);
            }
        }
    }

    // Gán vào clip "Teleport"
    public void TeleportEvent()
    {
        if (player != null)
        {
            // Dịch chuyển đến vị trí Player
            // (Bạn có thể thêm 1 khoảng offset nếu không muốn dịch chuyển vào trong)
            transform.position = player.transform.position;
        }
    }


    // Gán vào clip "RangedAttack2" (Sấy đạn)
    public void ShootBulletEvent()
    {
        // Kích hoạt Coroutine để "sấy" đạn
        StartCoroutine(BurstFireCoroutine());
    }

    // Coroutine để "sấy" đạn
    private IEnumerator BurstFireCoroutine()
    {
        for (int i = 0; i < 10; i++)
        {
            if (targetPlayer == null || firePoint == null || bulletPrefab == null)
            {
                yield break; // Dừng nếu mất mục tiêu
            }

            // Code bắn 1 viên
            Vector3 directionToPlayer = (targetPlayer.position - firePoint.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);

            EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet != null)
            {
                enemyBullet.SetmovementDirection(directionToPlayer * bulletSpeed);
            }

            // Tạm dừng 0.1 giây (hoặc giá trị bạn set) rồi mới bắn viên tiếp theo
            yield return new WaitForSeconds(burstFireDelay);
        }
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
            for (int j = 0; j < bulletCount; j++)
            {
                float angle = j * angleStep;
                Vector3 bulletDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 0);
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                EnemyBullet enemyBullet = bullet.GetComponent<EnemyBullet>();
                enemyBullet.SetmovementDirection(bulletDirection * bulletCircleSpeed);
            }
            yield return new WaitForSeconds(1f);
        }
    } 


    // --- VẼ GIZMOS ĐỂ DỄ DEBUG ---
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        // Vẽ tầm đánh gần (Màu đỏ)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, meleeAttackRange);

        // Vẽ tầm bắn xa (Màu xanh)
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, rangedAttackRange);
    }
}