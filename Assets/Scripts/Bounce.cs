using UnityEngine;


public class Bounce : MonoBehaviour
{
    [Header("Bounce settings")]
    [Tooltip("Defines the difference in angle between a side and up/down")]
    public float sideTresh = 10f;
    [Tooltip("Every hit, it will gain value% speed")]
    public float gainPercent = 1f;

    private float currentSpeed = 0f; // Keeps consistent increasing speed value
    private AudioSource audioHit; // On hit sound
    private Vector2 lastFrameVelocity; // Previous frame velocity
    private Rigidbody2D rb;

    // Initial velocity will give the object momentum at start and set initial speed
    private Vector2 _initialVelocity = Vector2.zero;
    public Vector2 InitialVelocity
    {
        set
        {
            _initialVelocity = value;
            rb.velocity = _initialVelocity;
            currentSpeed = _initialVelocity.magnitude;
        }
    }

    // Get componenent
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        audioHit = GetComponent<AudioSource>();
    }


    private void Update()
    {
        // Make sure velocity speed does not slow down
        if (rb.velocity.magnitude < currentSpeed)
            rb.velocity = rb.velocity.normalized * currentSpeed;

        // Set last frame velocity
        lastFrameVelocity = rb.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Bounce reflect on collide
        Rebound(collision);
    }

    // Send object flying in reflective direction
    private void Rebound(Collision2D collision)
    {
        // Increase speed
        currentSpeed += currentSpeed * (gainPercent / 100);
        float speed = lastFrameVelocity.magnitude;

        // Angle of approach, used to make rebound more affected by normal
        float angle = Vector2.Angle(lastFrameVelocity, collision.contacts[0].normal);

        // Check angle towards middle, if inside treshold then hit on side
        float sideAngle = Vector2.Angle(collision.contacts[0].normal, collision.transform.position);

        // Hit side of box, set velocity to normal
        if (sideAngle < (180 - sideTresh) && sideAngle > sideTresh)
        {
            Vector2 direction = collision.contacts[0].normal;
            rb.velocity = direction.normalized * Mathf.Max(speed, currentSpeed);
        }
        // Hit top or bottom of box, set velocity to reflect on normal
        else
        {
            Vector2 direction = Vector2.Reflect(lastFrameVelocity.normalized, collision.contacts[0].normal * (180.0f / angle));
            rb.velocity = direction.normalized * Mathf.Max(speed, currentSpeed);
        }

        // Play sound
        audioHit.Play();
    }
}
