////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using UnityEngine.UI;

////public abstract class Enemy : MonoBehaviour
////{
////    [SerializeField] protected float tick = 0.2f;
////    [SerializeField] protected float enterDamage = 10f;
////    [SerializeField] protected float stayDamage = 5f;
////    [SerializeField] protected float maxHp = 50f;
////    [SerializeField] protected float enemyMoveSpeed = 3f;
////    [SerializeField] private Image hpBar;
////    protected Transform targetPlayer;
////    protected float currentHp;
////    protected PlayerController player;
////    protected bool playerInside;

////    protected virtual void Start()
////    {
////        player = FindAnyObjectByType<PlayerController>();
////        currentHp = maxHp;
////        UpdateHpBar();
////    }
////    protected virtual void Update()
////    {
////        MoveToPlayer();
////    }
////    protected void MoveToPlayer()
////    {
////        if (player != null)
////        {
////            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyMoveSpeed * Time.deltaTime);
////        }
////        FlipEnemy();

////    }

////    protected void FlipEnemy()
////    {
////        if (player != null)
////        {
////            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
////        }

////    }
////    public virtual void TakeDamage(float damage)
////    {
////        currentHp -= damage;
////        currentHp = Mathf.Max(currentHp, 0);
////        UpdateHpBar();
////        if (currentHp <= 0)
////        {
////            Die();
////        }

////    }

////    protected virtual void Die()
////    {
////        Destroy(gameObject);
////    }

////    protected void UpdateHpBar()
////    {
////        if (hpBar != null)
////        {
////            hpBar.fillAmount = currentHp / maxHp;
////        }
////    }
////}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public abstract class Enemy : MonoBehaviour
//{
//    [SerializeField] protected float tick = 0.2f;
//    [SerializeField] protected float enterDamage = 10f;
//    [SerializeField] protected float stayDamage = 5f;
//    [SerializeField] protected float maxHp = 50f;
//    [SerializeField] protected float enemyMoveSpeed = 3f;
//    [SerializeField] private Image hpBar;
//    protected Transform targetPlayer; // Biến mục tiêu
//    protected float currentHp;
//    protected PlayerController player;
//    protected bool playerInside;

//    protected virtual void Start()
//    {
//        player = FindAnyObjectByType<PlayerController>();

//        // --- PHẦN THÊM VÀO ---
//        // Tự động tìm và gán mục tiêu
//        if (player != null)
//        {
//            // 1. Thử tìm object con tên "targetPlayer" (giữa người)
//            Transform targetChild = player.transform.Find("targetPlayer");
//            if (targetChild != null)
//            {
//                targetPlayer = targetChild;
//            }
//            else
//            {
//                // 2. Nếu không thấy, dùng gốc (pivot) làm dự phòng
//                Debug.LogWarning("Không tìm thấy 'targetPlayer' trên Player. Sẽ ngắm vào gốc (pivot).");
//                targetPlayer = player.transform;
//            }
//        }
//        else
//        {
//            Debug.LogError(gameObject.name + ": Không tìm thấy Player trong Scene!");
//        }
//        // --- KẾT THÚC PHẦN THÊM VÀO ---

//        currentHp = maxHp;
//        UpdateHpBar();
//    }
//    protected virtual void Update()
//    {
//        MoveToPlayer();
//    }
//    protected void MoveToPlayer()
//    {
//        // --- PHẦN SỬA LẠI ---
//        // Di chuyển về phía 'targetPlayer' thay vì 'player.transform'
//        if (targetPlayer != null)
//        {
//            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, enemyMoveSpeed * Time.deltaTime);
//        }
//        // --- KẾT THÚC PHẦN SỬA ---

//        FlipEnemy();
//    }

//    protected void FlipEnemy()
//    {
//        // Lật người vẫn có thể dựa vào 'player' (vì chỉ cần trục X)
//        if (player != null)
//        {
//            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
//        }
//    }
//    public virtual void TakeDamage(float damage)
//    {
//        currentHp -= damage;
//        currentHp = Mathf.Max(currentHp, 0);
//        UpdateHpBar();
//        if (currentHp <= 0)
//        {
//            Die();
//        }

//    }

//    protected virtual void Die()
//    {
//        Destroy(gameObject);
//    }

//    protected void UpdateHpBar()
//    {
//        if (hpBar != null)
//        {
//            hpBar.fillAmount = currentHp / maxHp;
//        }
//    }

//    // ... (Các hàm TakeDamage, Die, UpdateHpBar giữ nguyên) ...
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float tick = 0.2f;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 5f;
    [SerializeField] protected float maxHp = 50f;
    [SerializeField] protected float enemyMoveSpeed = 3f;
    [SerializeField] private Image hpBar;

    // --- CÁC BIẾN MỚI ---
    [Header("Animation")]
    [Tooltip("Thời gian (giây) của animation 'Die' trước khi bị hủy")]
    [SerializeField] private float dieAnimationTime = 1f; // Chỉnh thời gian này trong Inspector
    protected Animator animator;
    protected bool isDead = false; // Cờ để tránh gọi Die() nhiều lần
    // --- KẾT THÚC BIẾN MỚI ---

    protected Transform targetPlayer; // Biến mục tiêu
    protected float currentHp;
    protected PlayerController player;
    protected bool playerInside;

    protected virtual void Start()
    {
        // Tự động lấy Animator
        animator = GetComponent<Animator>();

        player = FindAnyObjectByType<PlayerController>();

        // Tự động tìm và gán mục tiêu
        if (player != null)
        {
            Transform targetChild = player.transform.Find("targetPlayer");
            if (targetChild != null)
            {
                targetPlayer = targetChild;
            }
            else
            {
                Debug.LogWarning("Không tìm thấy 'targetPlayer' trên Player. Sẽ ngắm vào gốc (pivot).");
                targetPlayer = player.transform;
            }
        }
        else
        {
            Debug.LogError(gameObject.name + ": Không tìm thấy Player trong Scene!");
        }

        currentHp = maxHp;
        UpdateHpBar();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (targetPlayer != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPlayer.position, enemyMoveSpeed * Time.deltaTime);
        }
        FlipEnemy();
    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    // --- HÀM TAKEDAMAGE ĐÃ SỬA ---
    public virtual void TakeDamage(float damage)
    {
        // Nếu đã chết, không nhận thêm sát thương
        if (isDead) return;

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();

        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            // Nếu chưa chết, chạy animation Hurt
            if (animator != null)
            {
                animator.SetTrigger("Hurt");
            }
        }
    }

    // --- HÀM DIE ĐÃ SỬA ---
    protected virtual void Die()
    {
        // Kích hoạt cờ, chỉ chạy 1 lần
        if (isDead) return;
        isDead = true;

        // Kích hoạt animation
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Vô hiệu hóa Enemy để nó không di chuyển, tấn công, hoặc bị bắn
        this.enabled = false; // Tắt script này (dừng Update/MoveToPlayer)

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Hủy object SAU KHI animation chạy xong
        Destroy(gameObject, dieAnimationTime);
    }

    protected void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}