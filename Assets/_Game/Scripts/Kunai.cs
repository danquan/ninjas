using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float kunaiSpeed = 5f;
    [SerializeField] private float damage = 30f;
    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        rb.velocity = transform.right * kunaiSpeed;
        Invoke(nameof(OnDespawn), 3.2f);
    }

    private void OnDespawn()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().OnHit(damage);
            OnDespawn();
        }
    }
}
