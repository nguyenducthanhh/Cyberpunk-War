using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0.01f;
    [SerializeField] private float maxHp = 200f;
    [SerializeField] private Image hpBar;
    private float currentHp;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimatror;
    private SpriteRenderer mySpriteRenderer;
    private bool isDead = false;
    [SerializeField] private float dieAnimationTime = 1f;
    private void Awake()
    {
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimatror = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();
    }
    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
        myAnimatror.SetFloat("moveX", movement.x);
        myAnimatror.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        Vector2 normalizedMovement = movement.normalized;

        // 2. TÍNH TOÁN VỊ TRÍ MỚI (DÙNG PHÉP NHÂN)
        Vector2 newPosition = rb.position + normalizedMovement * moveSpeed * Time.deltaTime;

        // 3. DI CHUYỂN
        rb.MovePosition(newPosition);

    }

    public bool IsMoving()
    {
        // 'movement' là biến Vector2 mà bạn đã dùng
        // .magnitude > 0.1f (thay vì == 0) để an toàn
        return movement.magnitude > 0.1f;
    }
    public void TakeDamage(float damage)
    {
        myAnimatror.SetTrigger("Hurt");
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        Debug.Log("Player took " + damage + " damage. HP left: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Kích hoạt cờ, chỉ chạy 1 lần
        if (isDead) return;
        isDead = true;

        // Kích hoạt animation
        if (myAnimatror != null)
        {
            myAnimatror.SetTrigger("Die");
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
    private void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
    public float GetCurrentHp()
    {
        return currentHp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }
}
