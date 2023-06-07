using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage;
    public float force;
    public float lifeTime;
    private Rigidbody2D rb;
    private Animator animator;
    public AnimatorClipInfo[] currentClipInfo;
    float currentClipLength;
    private bool isReached = false;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
        rb.AddForce(transform.right * force, ForceMode2D.Impulse);
        Destroy(gameObject, lifeTime);
    }
    private void FixedUpdate()
    {
        Rotate();
    }
    private void Rotate()
    {
        var direction = rb.velocity;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 180);
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.tag == "Enemy" && !isReached)
        {
            isReached = true;
            var enemy = other.gameObject.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            if(enemy.IsDead)
            {
                animator = other.gameObject.GetComponent<Animator>();
                currentClipInfo = animator.GetCurrentAnimatorClipInfo(0);
                currentClipLength = currentClipInfo[0].clip.length;
                animator.SetBool("isDead", true);
                Destroy(other.gameObject, currentClipLength);
            }
            Destroy(gameObject);
        }
    }
}
