using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public float gravity = 9.8f;

    private Rigidbody2D rb;

    private void Start()
    {
        transform.Rotate(0f, 0f, 90f);
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed+transform.right*(speed/6);

    }

    private void FixedUpdate()
    {
        // 중력 적용
        rb.AddForce(Vector2.down * gravity * rb.mass);
    }
    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        rb = GetComponent<Rigidbody2D>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
}
