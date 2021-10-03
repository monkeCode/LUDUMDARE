using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public float attackCooldown;
    public LayerMask layerEnemies;
    public LayerMask map;
    public int damage;
    public float flyDistance;
    public Vector2 velocity;

    private Vector3 shootPos;
    private Animator animator;
    public new Rigidbody2D rigidbody;

    protected void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        shootPos = transform.position;
        rigidbody.velocity = transform.right * speed;
    }

    private void Update()
    {
        var newVelocity = transform.right * speed;
        newVelocity.y = rigidbody.velocity.y;
        rigidbody.velocity = newVelocity;
        velocity = rigidbody.velocity;
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
            OnCollisionWithGround(other);
        }
    }
    public abstract void DealDamage(IDamagable enemy);
    public abstract void OnCollisionWithGround(Collision2D other);
    public abstract void Overheat();

    public abstract void Cooling();
}