using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public float speed;
    public float attackCooldown;
    public LayerMask layerEnemies;
    public LayerMask map;
    public int damage;
    public float flyDistance;
    public float overheatIncrement;
    public Vector2 velocity;

    protected Vector3 shootPos;
    private Animator animator;
    protected Rigidbody2D Rigidbody;

    protected void Start()
    {
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        shootPos = transform.position;
        Rigidbody.velocity = transform.right * speed;
    }

    private void Update()
    {
        var newVelocity = transform.right * speed;
        newVelocity.y = Rigidbody.velocity.y;
        Rigidbody.velocity = newVelocity;
        velocity = Rigidbody.velocity;
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