/*using UnityEngine;

public class GreenDuck : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public float changeDirectionTime = 2f;
    public Sprite deadSprite; 

    private Vector2 direction;
    private float speed;
    private float timer;
    private bool isDead = false;
    
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        SetRandomDirection();
    }

    void Update()
    {
        if (isDead) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetRandomDirection();
        }

        rb.linearVelocity = direction * speed;
        CheckBounds();
    }

    void SetRandomDirection()
    {
        float angle = Random.Range(20f, 160f) * Mathf.Deg2Rad; 
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        speed = Random.Range(minSpeed, maxSpeed);
        timer = changeDirectionTime;
    }

    void CheckBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y > 1)
        {
            Destroy(gameObject); 
        }
    }

    void OnMouseDown()
    {
        if (!isDead) Die();
    }

    void Die()
    {
        isDead = true;
        animator.enabled = false; // Stop flying animation
        spriteRenderer.sprite = deadSprite; // Change to dead sprite
        rb.linearVelocity = Vector2.down * 2f; // Make it fall
        Destroy(gameObject, 1.5f);
    }
}*/
using UnityEngine;

public class GreenDuck : MonoBehaviour
{
    public float minSpeed = 2f;
    public float maxSpeed = 5f;
    public float changeDirectionTime = 2f;
    public Sprite deadSprite; 

    private Vector2 direction;
    private float speed;
    private float timer;
    private bool isDead = false;

    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    // Enum to define the duck's flying state
    private enum FlyingState
    {
        Diagonal,
        Up,
        Right
    }

    private FlyingState currentFlyingState;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        SetRandomFlyingState();
    }

    void Update()
    {
        if (isDead) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            SetRandomFlyingState();
        }

        MoveDuck();
        CheckBounds();
    }

    // This function sets a random flying state
    void SetRandomFlyingState()
    {
        currentFlyingState = (FlyingState)Random.Range(0, 3);
        speed = Random.Range(minSpeed, maxSpeed);
        timer = changeDirectionTime;

        // Trigger the correct animation based on the current flying state
        switch (currentFlyingState)
        {
            case FlyingState.Diagonal:
                animator.Play("FlyingDiagonal");
                break;
            case FlyingState.Up:
                animator.Play("FlyingUp");
                break;
            case FlyingState.Right:
                animator.Play("FlyingRight");
                break;
        }
    }

    // This function moves the duck based on the current flying state
    void MoveDuck()
    {
        switch (currentFlyingState)
        {
            case FlyingState.Diagonal:
                // Flying diagonally (up-right direction)
                direction = new Vector2(1, 1).normalized;
                break;
            case FlyingState.Up:
                // Flying straight up
                direction = Vector2.up;
                break;
            case FlyingState.Right:
                // Flying straight right
                direction = Vector2.right;
                break;
        }

        rb.linearVelocity = direction * speed;
    }

    void CheckBounds()
    {
        Vector3 screenPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (screenPosition.x < 0 || screenPosition.x > 1 || screenPosition.y > 1)
        {
            Destroy(gameObject); // Destroy the duck if it goes off-screen
        }
    }

    void OnMouseDown()
    {
        if (!isDead) Die();
    }

    public void Die() // Change from private to public
{
    isDead = true;
    animator.enabled = false; // Stop flying animation
    spriteRenderer.sprite = deadSprite; // Change to dead sprite
    rb.linearVelocity = Vector2.down * 2f; // Make it fall
    Destroy(gameObject, 1.5f);
}


}

