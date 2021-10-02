using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public float attackCooldown;
    public LayerMask layerEnemies;
    public LayerMask map;
    public int damage;
    public float flyDistance;

    private Vector3 shootPos;
    private Animator animator;
    private new Rigidbody2D rigidbody;
    private new CircleCollider2D collider;

    private void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<CircleCollider2D>();
        animator = gameObject.GetComponent<Animator>();
        shootPos = transform.position;
    }

    private void Update()
    {
        rigidbody.velocity = (transform.up + transform.right) * speed;
        if ((transform.position - shootPos).magnitude >= flyDistance)
            Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (((1 << other.gameObject.layer) & layerEnemies) != 0)
        {
            DealDamage(other.gameObject.GetComponent<IDamagable>());
        }
        if (((1 << other.gameObject.layer) & map) != 0)
        {
            SelfDestroy();
        }
    }
    public abstract void DealDamage(IDamagable enemy);
    public abstract void SelfDestroy();
    public abstract void Overheat();

    public abstract void Cooling();
}