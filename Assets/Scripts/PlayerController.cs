using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;
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
    private void OnDisable()
    {
        playerControls.Disable();

    }
    private void Update()
    {
        if (isDead) return;
        PlayerInput();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGameMenu();
        }
    }

    private void FixedUpdate()
    {
        if (isDead) return;
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
        Vector2 newPosition = rb.position + normalizedMovement * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

    }

    public bool IsMoving()
    {
        return movement.magnitude > 0.1f;
    }
    public void TakeDamage(float damage)
    {
        myAnimatror.SetTrigger("Hurt");
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        audioManager.PlayHurtSound();

        if (currentHp <= 0)
        {
            Die();
     
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Money"))
        {
            gameManager.AddMoney();
            Destroy(collision.gameObject);
            audioManager.PlayMoneySound();
        }
        if (collision.CompareTag("Medicine"))
        {
            gameManager.HealPlayer();
            Destroy(collision.gameObject);
            audioManager.PlayHealSound();
        }
        
       
    }

    public void Heal(float healValue)
    {
        if (currentHp < maxHp)
        {
            currentHp += healValue;
            currentHp = Mathf.Min(currentHp, maxHp);
            UpdateHpBar();
        }
    }

    private void Die()
    {
        isDead = true;
        audioManager.PlayPlayerDie();
        if (myAnimatror != null)
        {
            myAnimatror.SetTrigger("Die");
        }

        this.enabled = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        


    }

    private void EventDie() 
    {
        gameManager.GameOverMenu();
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
