using UnityEngine;

// 원하는 방향으로 이동하고 싶다. 
// गुरुत्वाकर्षण लागू करें. 
public class SimpleMove_2 : MonoBehaviour
{
    public Vector3 dir = new Vector3(0, 0, 1);
    public float speed = 1; // m/s 

    public float jumpPower = 5f;    // 점프(점프) 힘 
    public float gravity = -9.8f;   // 중력
    public float yVelocity = 0;     // y축 변화량 

    CharacterController controller;

    // Ground Check variables
    private bool isGrounded;
    public float groundCheckDistance = 0.2f; // How far below the player to check for ground
    public LayerMask groundLayer; // Which layers to consider as ground

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // By default, consider everything as ground except the player itself.
        // The user can refine this in the Inspector.
        if (groundLayer == 0) // If not set in inspector
        {
            groundLayer = ~LayerMask.GetMask("Player");
        }
    }

    void Update()
    {
        // --- Horizontal Movement ---
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        dir = new Vector3(h, 0, v);
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        dir.Normalize();

        // --- Vertical Movement (Gravity and Ground Check) ---
        
        // Perform the ground check
        CheckGroundStatus();

        if (isGrounded)
        {
            // When on valid ground, reset vertical velocity.
            // This prevents accumulating gravity while on the ground.
            yVelocity = -0.1f; 

            // Handle Jump
            if (Input.GetButtonDown("Jump"))
            {
                yVelocity = jumpPower;
            }
        }
        else
        {
            // When in the air, apply gravity.
            // This will also apply when falling after hitting a "Wrong" platform.
            yVelocity += gravity * Time.deltaTime;
        }

        // Apply final movement
        dir.y = yVelocity;
        controller.Move(dir * speed * Time.deltaTime);
    }

    private void CheckGroundStatus()
    {
        // Use a SphereCast to check for ground below the player
        RaycastHit hit;
        // Start the cast from the controller's center, go down.
        // The distance is half the height + a small buffer (groundCheckDistance).
        if (Physics.SphereCast(transform.position, controller.radius, Vector3.down, out hit, controller.height / 2 - controller.radius + groundCheckDistance, groundLayer, QueryTriggerInteraction.Ignore))
        {
            // If we hit something, check its tag.
            if (hit.collider.CompareTag("Wrong"))
            {
                // It's a wrong platform. Destroy it and treat the player as not grounded.
                Destroy(hit.collider.gameObject);
                isGrounded = false;
            }
            else
            {
                // It's a valid ground platform.
                isGrounded = true;
            }
        }
        else
        {
            // The cast hit nothing, so we are in the air.
            isGrounded = false;
        }
    }

    // OnControllerColliderHit is now only needed for things that require a physical push, like the Goal.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Goal"))
        {
            somethingFunction();
        }
    }

    public void somethingFunction() { }
}
