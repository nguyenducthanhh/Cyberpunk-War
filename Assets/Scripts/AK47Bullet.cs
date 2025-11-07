using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private float timeDestroy = 1f;
    [SerializeField] private float damage = 10f;
    // [SerializeField] private GameObject bloodPrefab;
    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }


    void Update()
    {
        MoveBullet();

    }
    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //GameObject blood = Instantiate(bloodPrefab, transform.position, Quaternion.identity);
            // Destroy(blood, 1f);
            // Enemy enemy = collision.GetComponent<Enemy>();
            //enemy.TakeDamage(damage);
        }
    }
}
